using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetSteps.Validation.Common;
using NetSteps.Validation.Common.Model;
using NetSteps.Validation.Handlers.Common.Services;
using NetSteps.Validation.Handlers.Core;
using NetSteps.Validation.Handlers.Encore.Common.Services;
using NetSteps.Validation.Handlers.Helpers;
using NetSteps.Validation.Handlers.Services;

namespace NetSteps.Validation.Handlers.OrderItemPrice
{
    public class OrderItemPrice_OriginalUnitPrice_ValidationHandler : BaseRecordPropertyCalculationHandler
    {
        public OrderItemPrice_OriginalUnitPrice_ValidationHandler(IRecordPropertyCalculationHandlerResolver resolver) : base(resolver)
        {
        }

        protected virtual decimal MaximumAllowedDeviancy
        {
            get
            {
                return _maxDeviancy;
            }
        }

        private const decimal _maxDeviancy = 0.005M;

        public override void CalculateExpectedValue(IRecordProperty propertyToCalculate)
        {
            var order = propertyToCalculate.ParentRecord.Parent.Parent.Parent;
            var orderItem = propertyToCalculate.ParentRecord.Parent;
            var quantity = (int) orderItem.Properties[EncoreFieldNames.OrderItem.Quantity].OriginalValue;
            var priceTypeId =((int?)propertyToCalculate.ParentRecord.Properties[EncoreFieldNames.OrderItemPrice.ProductPriceTypeID].OriginalValue).Value;
            var priceType = ServiceLocator.FindService<IPriceTypeService>().GetPriceType(priceTypeId);
            var pricingService = ServiceLocator.FindService<IOrderItemPricingService>();

            decimal originalPrice;
            var originalPriceFound = OrderItemPriceHelper.GetOriginalUnitPrice(orderItem, propertyToCalculate.ParentRecord, priceType, true, out originalPrice);

            propertyToCalculate.ExpectedValue = originalPrice;
            if (!originalPriceFound)
            {
                propertyToCalculate.SetResult(ValidationResultKind.IsBroken);
            }
            else
            {
                if (pricingService.ShouldMultiplyOrderItemPricesByQuantity)
                {
                    propertyToCalculate.ExpectedValue = Math.Round(originalPrice * quantity, 2);
                }
                else
                {
                    propertyToCalculate.ExpectedValue = Math.Round(originalPrice, 2);
                }

                if (orderItem.Result != ValidationResultKind.IsNew)
                {
                    decimal original = (decimal)(propertyToCalculate.OriginalValue ?? 0M);
                    decimal expected = (decimal)propertyToCalculate.ExpectedValue;


                    if (original == expected)
                    {
                        propertyToCalculate.SetResult(ValidationResultKind.IsCorrect);
                    }
                    else
                    {
                        if (Math.Abs(original - expected) > MaximumAllowedDeviancy)
                        {
                            propertyToCalculate.ParentRecord.AddValidationComment(ValidationCommentKind.CalculationComment, String.Format("Price Type {0} OriginalUnitPrice was {1}. Should be {2}.", priceType.Name, propertyToCalculate.OriginalValue, propertyToCalculate.ExpectedValue));
                            propertyToCalculate.SetResult(ValidationResultKind.IsIncorrect);
                        }
                        else
                        {
                            propertyToCalculate.ParentRecord.AddValidationComment(ValidationCommentKind.CalculationComment, String.Format("Price Type {0} OriginalUnitPrice was {1}. Should be {2} but is within margin of error {3}.", priceType.Name, propertyToCalculate.OriginalValue, propertyToCalculate.ExpectedValue, MaximumAllowedDeviancy));
                            propertyToCalculate.SetResult(ValidationResultKind.IsIncorrect);
                        }
                    }
                }
            }
        }
    }
}
