using NetSteps.Validation.Common;
using NetSteps.Validation.Common.Model;
using NetSteps.Validation.Handlers.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSteps.Validation.Handlers.OrderItem
{
    public class OrderItem_ItemPriceActual_ValidationHandler : BaseRecordPropertyCalculationHandler
    {

        public OrderItem_ItemPriceActual_ValidationHandler(IRecordPropertyCalculationHandlerResolver resolver)
            : base(resolver)
        {
        }

        protected virtual decimal MaximumAllowedDeviancy
        {
            get
            {
                return _maxDeviancy;
            }
        }

        private const decimal _maxDeviancy = 0.000M;

        public override void CalculateExpectedValue(Validation.Common.Model.IRecordProperty propertyToCalculate)
        {
            var original = (decimal?)propertyToCalculate.OriginalValue;
            if (original == null)
            {
                propertyToCalculate.ExpectedValue = null;
                propertyToCalculate.SetResult(ValidationResultKind.IsCorrect);
            }
            var orderItemRecord = propertyToCalculate.ParentRecord;
            
            var itemPrice = (decimal)CalculateDependentValue(orderItemRecord, EncoreFieldNames.OrderItem.ItemPrice);
            if (original == itemPrice)
            {
                propertyToCalculate.ExpectedValue = null;
                propertyToCalculate.ParentRecord.AddValidationComment(ValidationCommentKind.CalculationComment, String.Format("ItemPriceActual matches ItemPrice and therefore is invalid. Removing override."));
                propertyToCalculate.SetResult(ValidationResultKind.IsIncorrect);
            }
            else
            {
                propertyToCalculate.ExpectedValue = original;
                propertyToCalculate.SetResult(ValidationResultKind.IsCorrect);
            }
        }
    }
}
