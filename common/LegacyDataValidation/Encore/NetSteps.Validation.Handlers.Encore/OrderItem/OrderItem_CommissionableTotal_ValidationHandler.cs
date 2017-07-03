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
    public class OrderItem_CommissionableTotal_ValidationHandler : BaseRecordPropertyCalculationHandler
    {
        public OrderItem_CommissionableTotal_ValidationHandler(IRecordPropertyCalculationHandlerResolver resolver)
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

            var quantity = (int)orderItemRecord.Properties[EncoreFieldNames.OrderItem.Quantity].OriginalValue;

            var priceTypeService = ServiceLocator.FindService<IPriceTypeService>();
            var priceType = priceTypeService.GetPrimaryVolumeType();

            decimal workingValue;
            var orderItemPriceRecord = orderItemRecord.ChildRecords.SingleOrDefault(x => x.RecordKind == EncoreFieldNames.TableSingularNames.OrderItemPrice && ((int)x.Properties[EncoreFieldNames.OrderItemPrice.ProductPriceTypeID].OriginalValue == priceType.PriceTypeID));
            if (orderItemPriceRecord != null)
            {
                workingValue = (decimal)CalculateDependentValue(orderItemPriceRecord, EncoreFieldNames.OrderItemPrice.UnitPrice);
            }
            else
            {
                orderItemRecord.AddValidationComment(ValidationCommentKind.Error, String.Format("No OrderItemPrice record for primary volume type {0}.", priceType.Name));
                propertyToCalculate.SetResult(ValidationResultKind.IsBroken);
                propertyToCalculate.ExpectedValue = 0M;
                return;
            }
            var multiplier = this.CalculateCurrencyMultiplier(orderItemRecord);
            
            propertyToCalculate.ExpectedValue = workingValue * quantity;

            decimal original = (decimal)(propertyToCalculate.OriginalValue ?? 0M);
            decimal expected = (decimal)(propertyToCalculate.ExpectedValue ?? 0M);

            if (original == expected)
            {
                propertyToCalculate.SetResult(ValidationResultKind.IsCorrect);
            }
            else if (Math.Abs(original - expected) < MaximumAllowedDeviancy)
            {
                propertyToCalculate.ParentRecord.AddValidationComment(ValidationCommentKind.CalculationComment, String.Format("CommissionableTotal ({0}) was {1}. Should be {2} but falls within the margin of error {3}.", priceType.Name, propertyToCalculate.OriginalValue, propertyToCalculate.ExpectedValue, MaximumAllowedDeviancy));
                propertyToCalculate.SetResult(ValidationResultKind.IsWithinMarginOfError);
            }
            else
            {
                propertyToCalculate.ParentRecord.AddValidationComment(ValidationCommentKind.CalculationComment, String.Format("CommissionableTotal ({0}) was {1}. Should be {2}.", priceType.Name, propertyToCalculate.OriginalValue, propertyToCalculate.ExpectedValue));
                propertyToCalculate.SetResult(ValidationResultKind.IsIncorrect);
            }
        }
    }
}
