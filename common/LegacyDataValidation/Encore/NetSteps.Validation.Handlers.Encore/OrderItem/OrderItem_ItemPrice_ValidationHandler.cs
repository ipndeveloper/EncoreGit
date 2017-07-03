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

namespace NetSteps.Validation.Handlers.OrderItem
{
    public class OrderItem_ItemPrice_ValidationHandler : BaseRecordPropertyCalculationHandler
    {

        public OrderItem_ItemPrice_ValidationHandler(IRecordPropertyCalculationHandlerResolver resolver)
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

        private const decimal _maxDeviancy = 0.005M;

        public override void CalculateExpectedValue(Validation.Common.Model.IRecordProperty propertyToCalculate)
        {
            var orderItemRecord = propertyToCalculate.ParentRecord;

            var orderItemPricingService = ServiceLocator.FindService<IOrderItemPricingService>();
            var priceTypeService = ServiceLocator.FindService<IPriceTypeService>();
            var priceTypeID = (int)orderItemRecord.Properties[EncoreFieldNames.OrderItem.ProductPriceTypeID].OriginalValue;
            var priceType = priceTypeService.GetPriceType(priceTypeID);

            

            decimal originalPrice;
            var originalPriceFound = OrderItemPriceHelper.GetOriginalUnitPrice(orderItemRecord, propertyToCalculate.ParentRecord, priceType, true, out originalPrice);

            if (!originalPriceFound)
            {
                propertyToCalculate.SetResult(ValidationResultKind.IsBroken);
                return;
            }
            else
            {
                propertyToCalculate.ExpectedValue = originalPrice;
            }
            
            decimal original = (decimal)propertyToCalculate.OriginalValue;
            decimal expected = (decimal)propertyToCalculate.ExpectedValue;

            if (original == expected)
            {
                propertyToCalculate.SetResult(ValidationResultKind.IsCorrect);
            }
            else if (Math.Abs(original - expected) < MaximumAllowedDeviancy)
            {
                propertyToCalculate.ParentRecord.AddValidationComment(ValidationCommentKind.CalculationComment, String.Format("ItemPrice ({0}) was {1}. Should be {2} but falls within the margin of error {3}.", priceType.Name, propertyToCalculate.OriginalValue, propertyToCalculate.ExpectedValue, MaximumAllowedDeviancy));
                propertyToCalculate.SetResult(ValidationResultKind.IsWithinMarginOfError);
            }
            else
            {
                propertyToCalculate.ParentRecord.AddValidationComment(ValidationCommentKind.CalculationComment, String.Format("ItemPrice ({0}) was {1}. Should be {2}.", priceType.Name, propertyToCalculate.OriginalValue, propertyToCalculate.ExpectedValue));
                propertyToCalculate.SetResult(ValidationResultKind.IsIncorrect);
            }
        }
    }
}
