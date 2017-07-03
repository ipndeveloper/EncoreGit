using NetSteps.Validation.Common;
using NetSteps.Validation.Common.Model;
using NetSteps.Validation.Handlers.Core;
using NetSteps.Validation.Handlers.Encore.Common.Services;
using NetSteps.Validation.Handlers.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSteps.Validation.Handlers.OrderCustomer
{
    public class OrderCustomer_Subtotal_ValidationHandler : BaseRecordPropertyCalculationHandler
    {
        public OrderCustomer_Subtotal_ValidationHandler(IRecordPropertyCalculationHandlerResolver resolver)
            : base(resolver)
        {
        }

        protected virtual decimal GetMaximumAllowedDeviancy(IRecord orderCustomerRecord)
        {
            return orderCustomerRecord.ChildRecords.Where(x => x.RecordKind == EncoreFieldNames.TableSingularNames.OrderItem).Count() * 0.005M;
        }

        public override void CalculateExpectedValue(Validation.Common.Model.IRecordProperty propertyToCalculate)
        {
            var orderItemPricingService = ServiceLocator.FindService<IOrderItemPricingService>();

            var orderCustomerRecord = propertyToCalculate.ParentRecord;
            
            decimal expectedTotal = 0M;
            foreach (var orderItem in orderCustomerRecord.ChildRecords.Where(x => x.RecordKind == EncoreFieldNames.TableSingularNames.OrderItem))
            {
                var productPriceTypeID = (int)orderItem.Properties[EncoreFieldNames.OrderItem.ProductPriceTypeID].OriginalValue;
                var orderItemPrice = orderItem.ChildRecords.SingleOrDefault(x => x.RecordKind == EncoreFieldNames.TableSingularNames.OrderItemPrice && (int)x.Properties[EncoreFieldNames.OrderItemPrice.ProductPriceTypeID].OriginalValue == productPriceTypeID);
                if (orderItemPrice != null)
                {
                    if (orderItemPricingService.ShouldMultiplyOrderItemPricesByQuantity)
                    {
                        expectedTotal += (decimal)CalculateDependentValue(orderItemPrice, EncoreFieldNames.OrderItemPrice.UnitPrice);
                    }
                    else
                    {
                        var quantity = (int)orderItem.Properties[EncoreFieldNames.OrderItem.Quantity].OriginalValue;
                        expectedTotal += quantity * (decimal)CalculateDependentValue(orderItemPrice, EncoreFieldNames.OrderItemPrice.UnitPrice);
                    }
                }
            }
            propertyToCalculate.ExpectedValue = expectedTotal;

            decimal original = (decimal)propertyToCalculate.OriginalValue;
            decimal expected = (decimal)propertyToCalculate.ExpectedValue;
            var maximumAllowedDeviancy = GetMaximumAllowedDeviancy(orderCustomerRecord);

            if (original == expected)
            {
                propertyToCalculate.SetResult(ValidationResultKind.IsCorrect);
            }
            else if (Math.Abs(original - expected) < maximumAllowedDeviancy)
            {
                propertyToCalculate.ParentRecord.AddValidationComment(ValidationCommentKind.CalculationComment, String.Format("OrderCustomer Subtotal was {0}. Should be {1} but falls within the margin of error {2}.", propertyToCalculate.OriginalValue, propertyToCalculate.ExpectedValue, maximumAllowedDeviancy));
                propertyToCalculate.SetResult(ValidationResultKind.IsWithinMarginOfError);
            }
            else
            {
                propertyToCalculate.ParentRecord.AddValidationComment(ValidationCommentKind.CalculationComment, String.Format("OrderCustomer Subtotal was {0}. Should be {1}.", propertyToCalculate.OriginalValue, propertyToCalculate.ExpectedValue));
                propertyToCalculate.SetResult(ValidationResultKind.IsIncorrect);
            }
        }
    }
}
