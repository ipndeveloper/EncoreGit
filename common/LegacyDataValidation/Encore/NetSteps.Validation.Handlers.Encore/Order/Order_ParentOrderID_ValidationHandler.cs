using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetSteps.Validation.Common;
using NetSteps.Validation.Common.Model;
using NetSteps.Validation.Handlers.Core;

namespace NetSteps.Validation.Handlers.Order
{
    public class Order_ParentOrderID_ValidationHandler : BaseRecordPropertyCalculationHandler
    {
        public Order_ParentOrderID_ValidationHandler(IRecordPropertyCalculationHandlerResolver resolver) : base(resolver)
        {
        }

        public override void CalculateExpectedValue(IRecordProperty propertyToCalculate)
        {
            if (propertyToCalculate.OriginalValue == null)
            {
                propertyToCalculate.ExpectedValue = null;
                propertyToCalculate.SetResult(ValidationResultKind.IsCorrect);
            }
            else
            {
                // verify that we don't have an infinite recursion loop.
                if (propertyToCalculate.OriginalValue.Equals(propertyToCalculate.ParentRecord.Properties["OrderID"].OriginalValue))
                {
                    propertyToCalculate.ParentRecord.AddValidationComment(ValidationCommentKind.CalculationComment, String.Format("Order {0} has a parent id recursively pointing to itself.", propertyToCalculate.ParentRecord.RecordIdentity));
                    propertyToCalculate.ExpectedValue = null;
                    propertyToCalculate.SetResult(ValidationResultKind.IsIncorrect);
                }
                else
                {
                    propertyToCalculate.ExpectedValue = null;
                    propertyToCalculate.SetResult(ValidationResultKind.IsCorrect);
                }
            }
            
        }
    }
}
