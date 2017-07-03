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

namespace NetSteps.Validation.Handlers.Order
{
    public class Order_Subtotal_ValidationHandler : BaseRecordPropertyCalculationHandler
    {
        public Order_Subtotal_ValidationHandler(IRecordPropertyCalculationHandlerResolver resolver)
            : base(resolver)
        {
        }

        protected virtual decimal GetMaximumAllowedDeviancy(IRecord orderRecord)
        {
            return orderRecord.ChildRecords.Where(x => x.RecordKind == EncoreFieldNames.TableSingularNames.OrderCustomer).Sum(x => x.ChildRecords.Where(y => y.RecordKind == EncoreFieldNames.TableSingularNames.OrderItem).Count() * 0.005M);
        }

        public override void CalculateExpectedValue(Validation.Common.Model.IRecordProperty propertyToCalculate)
        {
            var orderItemPricingService = ServiceLocator.FindService<IOrderItemPricingService>();

            var orderRecord = propertyToCalculate.ParentRecord;

            decimal expectedTotal = 0M;
            foreach (var orderCustomer in orderRecord.ChildRecords.Where(x => x.RecordKind == EncoreFieldNames.TableSingularNames.OrderCustomer))
            {
                expectedTotal += (decimal)CalculateDependentValue(orderCustomer, EncoreFieldNames.OrderCustomer.Subtotal); 
            }
            propertyToCalculate.ExpectedValue = expectedTotal;

            decimal original = (decimal)propertyToCalculate.OriginalValue;
            decimal expected = (decimal)propertyToCalculate.ExpectedValue;
            var maximumAllowedDeviancy = GetMaximumAllowedDeviancy(orderRecord);

            if (original == expected)
            {
                propertyToCalculate.SetResult(ValidationResultKind.IsCorrect);
            }
            else if (Math.Abs(original - expected) < maximumAllowedDeviancy)
            {
                propertyToCalculate.ParentRecord.AddValidationComment(ValidationCommentKind.CalculationComment, String.Format("Order Subtotal was {0}. Should be {1} but falls within the margin of error {2}.", propertyToCalculate.OriginalValue, propertyToCalculate.ExpectedValue, maximumAllowedDeviancy));
                propertyToCalculate.SetResult(ValidationResultKind.IsWithinMarginOfError);
            }
            else
            {
                propertyToCalculate.ParentRecord.AddValidationComment(ValidationCommentKind.CalculationComment, String.Format("Order Subtotal was {0}. Should be {1}.", propertyToCalculate.OriginalValue, propertyToCalculate.ExpectedValue));
                propertyToCalculate.SetResult(ValidationResultKind.IsIncorrect);
            }
        }
    }
}
