using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetSteps.Validation.Common;
using NetSteps.Validation.Common.Model;
using NetSteps.Validation.Handlers.Common.Services.ServiceModels;
using NetSteps.Validation.Handlers.Core;
using NetSteps.Validation.Handlers.Encore.Common.Services;
using NetSteps.Validation.Handlers.Helpers;

namespace NetSteps.Validation.Handlers.OrderItemPrice
{
    public class OrderItemPrice_UnitPrice_ValidationHandler : BaseRecordPropertyCalculationHandler
    {
        public OrderItemPrice_UnitPrice_ValidationHandler(IRecordPropertyCalculationHandlerResolver resolver) : base(resolver)
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
            // obtain records
            var orderItemPriceRecord = propertyToCalculate.ParentRecord;
            var orderItemRecord = orderItemPriceRecord.Parent;
            var orderCustomerRecord = orderItemRecord.Parent;
            var orderRecord = orderCustomerRecord.Parent;

            // get orderDate
            string orderDateField;
            var orderDate = OrderDateHelper.GetOrderDate(orderRecord, out orderDateField);
            // get services
            var priceTypeService = ServiceLocator.FindService<IPriceTypeService>();
            var orderItemPricingService = ServiceLocator.FindService<IOrderItemPricingService>();

            // get this product price type
            var currentPriceTypeID = GetCurrentProductPriceType(orderItemPriceRecord);
            var currentPriceType = priceTypeService.GetPriceType(currentPriceTypeID);
                
            // if this is a kit item, return 0
            if (orderItemRecord.Properties["ParentOrderItemID"].OriginalValue != null)
            {
                orderItemPriceRecord.AddValidationComment(ValidationCommentKind.CalculationComment, "OrderItem is part of a kit, and should not have prices.");
                propertyToCalculate.ExpectedValue = 0M;
            }
            else
            {
                // primary currency product price type
                var primaryCurrencyPriceType = GetCurrencyProductPriceType(orderItemRecord);

                // FIND THE START UNIT PRICE
                decimal startPrice;
                var hasStartPrice = OrderItemPriceHelper.GetOriginalUnitPrice(orderItemRecord, orderItemPriceRecord, currentPriceType, false, out startPrice);

                if (!hasStartPrice)
                {
                    // we're in a broken state.  We have to have a start price.
                    propertyToCalculate.ExpectedValue = 0;
                    propertyToCalculate.SetResult(ValidationResultKind.IsBroken);
                    return;
                }

                // determine if this product price type is a currency or commission price type.
                var isCurrencyType = priceTypeService.IsCurrencyPriceType(currentPriceType.PriceTypeID);

                // get the quantity for quantity multiplication....
                var quantity = (int)orderItemRecord.Properties[EncoreFieldNames.OrderItem.Quantity].OriginalValue;

                // calculate expected value
                if (isCurrencyType)
                {
                    var isPrimaryCurrencyType = currentPriceType.PriceTypeID == GetCurrencyProductPriceType(orderItemRecord);
                    if (isPrimaryCurrencyType)
                    {
                        if (orderItemPricingService.ShouldMultiplyOrderItemPricesByQuantity)
                        {
                            propertyToCalculate.ExpectedValue = CalculatePrimaryCurrencyPrice(orderItemRecord, orderItemPriceRecord, currentPriceType) * quantity;
                        }
                        else
                        {
                            propertyToCalculate.ExpectedValue = CalculatePrimaryCurrencyPrice(orderItemRecord, orderItemPriceRecord, currentPriceType);
                        }
                    }
                    else
                    {
                        decimal multiplier = this.CalculateCurrencyMultiplier(orderItemRecord);
                        if (orderItemPricingService.ShouldMultiplyOrderItemPricesByQuantity)
                        {
                            propertyToCalculate.ExpectedValue = multiplier * startPrice * quantity;
                        }
                        else
                        {
                            propertyToCalculate.ExpectedValue = multiplier * startPrice;
                        }
                    }
                }
                else
                {
                    var isPrimaryVolumeType = currentPriceType.PriceTypeID == priceTypeService.GetPrimaryVolumeType().PriceTypeID;
                    if (isPrimaryVolumeType)
                    {
                        if (orderItemPricingService.ShouldMultiplyOrderItemPricesByQuantity)
                        {
                            propertyToCalculate.ExpectedValue = CalculatePrimaryVolumePrice(orderItemRecord, orderItemPriceRecord, quantity, (int)orderRecord.Properties[EncoreFieldNames.Order.CurrencyID].OriginalValue, orderDate, orderItemPricingService ) * quantity;
                        }
                        else
                        {
                            propertyToCalculate.ExpectedValue = CalculatePrimaryVolumePrice(orderItemRecord, orderItemPriceRecord, quantity, (int)orderRecord.Properties[EncoreFieldNames.Order.CurrencyID].OriginalValue, orderDate, orderItemPricingService);
                        }
                    }
                    else
                    {
                        decimal multiplier = this.CalculateVolumeMultiplier(orderItemRecord, priceTypeService.GetPrimaryVolumeType());
                        if (orderItemPricingService.ShouldMultiplyOrderItemPricesByQuantity)
                        {
                            propertyToCalculate.ExpectedValue = multiplier * startPrice;
                        }
                        else
                        {
                            propertyToCalculate.ExpectedValue = multiplier * startPrice * quantity;
                        }
                    }
                }
                propertyToCalculate.ExpectedValue = Math.Round((decimal)propertyToCalculate.ExpectedValue, 2);
            }

            // adjustments
            foreach (var orderLineAdjustment in orderItemRecord.ChildRecords.Where(x => x.RecordKind == EncoreFieldNames.TableSingularNames.OrderAdjustmentOrderLineModification && x.Properties[EncoreFieldNames.OrderAdjustmentOrderLineModification.PropertyName].OriginalValue.Equals(currentPriceType.Name)))
            {
                int operationID = (int)orderLineAdjustment.Properties[EncoreFieldNames.OrderAdjustmentOrderLineModification.ModificationOperationID].OriginalValue;
                decimal adjustmentOperand = (decimal)orderLineAdjustment.Properties[EncoreFieldNames.OrderAdjustmentOrderLineModification.ModificationDecimalValue].OriginalValue;
                var lastAmount = (decimal)propertyToCalculate.ExpectedValue;
                switch (operationID)
                {
                    case 2:
                        propertyToCalculate.ExpectedValue = (decimal)propertyToCalculate.ExpectedValue - adjustmentOperand;
                        orderItemPriceRecord.AddValidationComment(ValidationCommentKind.CalculationComment, 
                                        String.Format("Price Type {0} was reduced by Order Adjustments by amount {1}.",
                                                currentPriceType.Name,
                                                adjustmentOperand
                                                ));
                        break;
                    case 3:
                        propertyToCalculate.ExpectedValue = (decimal)propertyToCalculate.ExpectedValue - (adjustmentOperand * (decimal)propertyToCalculate.ExpectedValue);
                        orderItemPriceRecord.AddValidationComment(ValidationCommentKind.CalculationComment, 
                                        String.Format("Price Type {0} was reduced by Order Adjustments by amount {1} (op: {2} x {3} = {4}).",
                                                currentPriceType.Name,
                                                (adjustmentOperand * lastAmount),
                                                adjustmentOperand,
                                                lastAmount,
                                                adjustmentOperand * lastAmount));
                        break;
                }
            }

            decimal original = (decimal)propertyToCalculate.OriginalValue;
            decimal expected = (decimal)propertyToCalculate.ExpectedValue;

            if (propertyToCalculate.ResultKind != ValidationResultKind.IsNew)
            {
                if (original == expected)
                {
                    propertyToCalculate.SetResult(ValidationResultKind.IsCorrect);
                }
                else if (Math.Abs(original - expected) > MaximumAllowedDeviancy)
                {
                    propertyToCalculate.SetResult(ValidationResultKind.IsIncorrect);
                }
                else
                {
                    propertyToCalculate.SetResult(ValidationResultKind.IsWithinMarginOfError);
                }


                switch (propertyToCalculate.ResultKind)
                {
                    case ValidationResultKind.IsWithinMarginOfError:
                        propertyToCalculate.ParentRecord.AddValidationComment(ValidationCommentKind.CalculationComment, String.Format("Price Type {0} UnitPrice was {1}. Should be {2} but falls within the margin of error {3}.", currentPriceType.Name, propertyToCalculate.OriginalValue, propertyToCalculate.ExpectedValue, MaximumAllowedDeviancy));
                        break;
                    case ValidationResultKind.IsIncorrect:
                        propertyToCalculate.ParentRecord.AddValidationComment(ValidationCommentKind.CalculationComment, String.Format("Price Type {0} UnitPrice was {1}. Should be {2}.", currentPriceType.Name, propertyToCalculate.OriginalValue, propertyToCalculate.ExpectedValue));
                        break;
                    case ValidationResultKind.IsBroken:
                        propertyToCalculate.ParentRecord.AddValidationComment(ValidationCommentKind.Error, String.Format("Price Type {0} is totally broken.  FAIL.", currentPriceType.Name, propertyToCalculate.OriginalValue, propertyToCalculate.ExpectedValue));
                        break;
                }
            }
        }

        protected virtual decimal CalculatePrimaryVolumePrice(IRecord orderItemRecord, IRecord orderItemPriceRecord, int quantity, int currencyID, DateTime orderDate, IOrderItemPricingService pricingService)
        {
            decimal? overridePrice = (decimal?)orderItemRecord.Properties[EncoreFieldNames.OrderItem.CommissionableTotalOverride].OriginalValue;
            if (overridePrice.HasValue)
            {
                if (quantity != 0)
                {
                    return overridePrice.Value / quantity;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                var orderItemPricingService = ServiceLocator.FindService<IOrderItemPricingService>();
                var workingValue = 0M;
                var workingValueFound = pricingService.GetHistoricalPrice((int)orderItemRecord.Properties[EncoreFieldNames.OrderItem.ProductID].OriginalValue,
                    orderDate,
                    (int)orderItemPriceRecord.Properties[EncoreFieldNames.OrderItemPrice.ProductPriceTypeID].OriginalValue,
                    currencyID,
                    out workingValue);

                var multiplier = this.CalculateCurrencyMultiplier(orderItemRecord);
                return workingValue * multiplier;
            }
        }

        protected virtual decimal CalculatePrimaryCurrencyPrice(IRecord orderItemRecord, IRecord orderItemPriceRecord, IProductPriceType priceType)
        {
            decimal? overridePrice =
                (decimal?) CalculateDependentValue(orderItemRecord, EncoreFieldNames.OrderItem.ItemPriceActual);
            if (overridePrice.HasValue)
            {
                return overridePrice.Value;
            }

            int productID = (int) orderItemRecord.Properties[EncoreFieldNames.OrderItem.ProductID].OriginalValue;
            int quantity = (int) orderItemRecord.Properties[EncoreFieldNames.OrderItem.Quantity].OriginalValue;
            decimal? discount = (decimal?) orderItemRecord.Properties[EncoreFieldNames.OrderItem.Discount].OriginalValue;
            decimal? discountPercent =
                (decimal?) orderItemRecord.Properties[EncoreFieldNames.OrderItem.DiscountPercent].OriginalValue;

            decimal workingValue = (decimal)CalculateDependentValue(orderItemRecord, EncoreFieldNames.OrderItem.ItemPrice);
            if (discountPercent.HasValue)
            {
                workingValue -= workingValue*discountPercent.Value;
            }
            else if (discount.HasValue)
            {
                // discount is multiplied by quantity; hence we have to divide by quantity to get the real unit value.
                if (quantity != 0)
                {
                    workingValue -= discount.Value / quantity;
                }
            }
            
            return workingValue;
        }

        protected virtual int GetCurrencyProductPriceType(IRecord orderItemRecord)
        {
            return (int)orderItemRecord.Properties[EncoreFieldNames.OrderItem.ProductPriceTypeID]
                    .OriginalValue;
        }

        protected virtual int GetCurrentProductPriceType(IRecord orderItemPriceRecord)
        {
            return (int)orderItemPriceRecord.Properties[EncoreFieldNames.OrderItemPrice.ProductPriceTypeID]
                    .OriginalValue;
        }
    }
}
