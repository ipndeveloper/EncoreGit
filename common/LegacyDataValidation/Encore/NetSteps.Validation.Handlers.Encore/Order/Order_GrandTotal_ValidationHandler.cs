using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetSteps.Validation.Common;
using NetSteps.Validation.Common.Model;
using NetSteps.Validation.Handlers.Core;
using NetSteps.Validation.Handlers.Helpers;
using NetSteps.Validation.Handlers.Encore.Common.Services;
using NetSteps.Validation.Handlers.Common.Services;

namespace NetSteps.Validation.Handlers.Order
{
    public class Order_GrandTotal_ValidationHandler : BaseRecordPropertyCalculationHandler
    {
        public Order_GrandTotal_ValidationHandler(IRecordPropertyCalculationHandlerResolver resolver)
            : base(resolver)
        {
        }

        protected virtual decimal GetMaximumAllowedDeviancy(IRecord orderRecord)
        {
            return orderRecord.ChildRecords.Where(x => x.RecordKind == EncoreFieldNames.TableSingularNames.OrderCustomer).Sum(x => x.ChildRecords.Where(y => y.RecordKind == EncoreFieldNames.TableSingularNames.OrderItem).Count() * 0.005M);
        }

        public override void CalculateExpectedValue(IRecordProperty propertyToCalculate)
        {
            var orderItemPricingService = ServiceLocator.FindService<IOrderItemPricingService>();

            var orderRecord = propertyToCalculate.ParentRecord;

            var subtotal = (decimal)CalculateDependentValue(orderRecord, EncoreFieldNames.Order.Subtotal);
            var taxTotal = (decimal)CalculateDependentValue(orderRecord, EncoreFieldNames.Order.TaxAmountTotal);
            var shippingTotal = (decimal)CalculateDependentValue(orderRecord, EncoreFieldNames.Order.ShippingTotal);
            var handlingTotal = (decimal)CalculateDependentValue(orderRecord, EncoreFieldNames.Order.HandlingTotal);

            propertyToCalculate.ExpectedValue = subtotal + taxTotal + shippingTotal + handlingTotal;

            decimal original = (decimal)propertyToCalculate.OriginalValue;
            decimal expected = Math.Round((decimal)propertyToCalculate.ExpectedValue, 2);
            var maximumAllowedDeviancy = GetMaximumAllowedDeviancy(orderRecord);

            if (original == expected)
            {
                propertyToCalculate.SetResult(ValidationResultKind.IsCorrect);
            }
            else if (Math.Abs(original - expected) < maximumAllowedDeviancy)
            {
                propertyToCalculate.ParentRecord.AddValidationComment(ValidationCommentKind.CalculationComment, String.Format("Order GrandTotal was {0}. Should be {1} but falls within the margin of error {2}.", propertyToCalculate.OriginalValue, propertyToCalculate.ExpectedValue, maximumAllowedDeviancy));
                propertyToCalculate.SetResult(ValidationResultKind.IsWithinMarginOfError);
            }
            else
            {
                propertyToCalculate.ParentRecord.AddValidationComment(ValidationCommentKind.CalculationComment, String.Format("Order GrandTotal was {0}. Should be {1}.", propertyToCalculate.OriginalValue, propertyToCalculate.ExpectedValue));
                propertyToCalculate.SetResult(ValidationResultKind.IsIncorrect);
            }

            if (orderRecord.Properties[EncoreFieldNames.Order.CompleteDateUTC].OriginalValue != null)
            {
                var paymentService = ServiceLocator.FindService<IPaymentService>();
                decimal paymentTotal;
                if (paymentService.GetPaymentTotal((int)orderRecord.RecordIdentity, out paymentTotal))
                {
                    if (paymentTotal == expected)
                    {
                        if (paymentTotal == original)
                        {
                            propertyToCalculate.SetResult(ValidationResultKind.IsCorrect);
                        }
                        else
                        {
                            propertyToCalculate.SetResult(ValidationResultKind.IsIncorrect);
                        }
                    }
                    else if (Math.Abs(paymentTotal - expected) < maximumAllowedDeviancy)
                    {
                        propertyToCalculate.ParentRecord.AddValidationComment(ValidationCommentKind.CalculationComment, String.Format("Calculated Order GrandTotal {0} does not match Payment total {1} but falls within the margin of error {2}.", expected, paymentTotal, maximumAllowedDeviancy));
                        propertyToCalculate.SetResult(ValidationResultKind.IsWithinMarginOfError);
                    }
                    else if (!paymentTotal.Equals(expected))
                    {
                        propertyToCalculate.ParentRecord.AddValidationComment(ValidationCommentKind.CalculationComment, String.Format("Expected Order GrandTotal {0} does not match Payment total {1}.", expected, paymentTotal));
                        propertyToCalculate.SetResult(ValidationResultKind.IsBroken);

                        if (paymentTotal.Equals(original))
                        {
                            propertyToCalculate.ParentRecord.AddValidationComment(ValidationCommentKind.CalculationComment, String.Format("Stored Order GrandTotal {0} MATCHES Payment total {1}. Possible calculation fail.", original, paymentTotal));
                            propertyToCalculate.SetResult(ValidationResultKind.IsBroken);
                        }
                    }
                    
                    
                }
            }
        }
    }
}
