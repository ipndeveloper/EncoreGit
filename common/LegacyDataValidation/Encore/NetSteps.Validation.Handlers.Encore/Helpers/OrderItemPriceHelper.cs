using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetSteps.Validation.Common;
using NetSteps.Validation.Common.Model;
using NetSteps.Validation.Handlers.Common.Services.ServiceModels;
using NetSteps.Validation.Handlers.Encore.Common.Services;

namespace NetSteps.Validation.Handlers.Helpers
{
    internal static class OrderItemPriceHelper
    {
        internal static bool GetOriginalUnitPrice(IRecord orderItemRecord, IRecord orderItemPriceRecord, IProductPriceType priceType, bool addValidationMessages, out decimal workingValue)
        {
            var priceTypeService = ServiceLocator.FindService<IPriceTypeService>();
            var pricingService = ServiceLocator.FindService<IOrderItemPricingService>();

            var orderCustomerRecord = orderItemRecord.Parent;
            var orderRecord = orderCustomerRecord.Parent;

            string orderDateField = string.Empty;
            var orderDate = Helpers.OrderDateHelper.GetOrderDate(orderRecord, out orderDateField);
            var currencyID = (int)orderRecord.Properties[EncoreFieldNames.Order.CurrencyID].OriginalValue;
            var productID = (int)orderItemRecord.Properties[EncoreFieldNames.OrderItem.ProductID].OriginalValue;
            var orderItemPriceType = (int) orderItemRecord.Properties[EncoreFieldNames.OrderItem.ProductPriceTypeID].OriginalValue;

            decimal resultValue = 0;
            bool result = false;
            try
            {
                result = pricingService.GetHistoricalPrice(productID, orderDate, priceType.PriceTypeID, currencyID, out resultValue);
            }
            catch (Exception ex)
            {
                var message = ex.Message;
            }

            if (result)
            {
                workingValue = resultValue;
                return true;
            }
            else
            {
                if (orderItemPriceType == priceType.PriceTypeID)
                {
                    // There is no historical price.  If we are looking at the effective currency price type of the item, we "can" fall back
                    // on OrderItem.ItemPrice ..... we hope.
                    if (addValidationMessages)
                    {
                        orderItemPriceRecord.AddValidationComment(ValidationCommentKind.CalculationComment, 
                            String.Format(
                                "No historical price record found for {0} at order time using {1} of {2} currency {3}.  Using OrderItem.ItemPrice.",
                                priceType.Name, orderDateField, orderDate, currencyID));

                    }
                    workingValue = (decimal) orderItemRecord.Properties[EncoreFieldNames.OrderItem.ItemPrice].OriginalValue;
                    return true;
                }
                else
                {
                    // There is no historical price.  The only thing we can do is divide the "originalunitprice" by quantity and attempt to approximate
                    // what the price was at the time.
                    var quantity = (int) orderItemRecord.Properties[EncoreFieldNames.OrderItem.Quantity].OriginalValue;
                    var legacyOriginalUnitPrice = (decimal?) orderItemPriceRecord.Properties[EncoreFieldNames.OrderItemPrice.OriginalUnitPrice].OriginalValue;
                    if (legacyOriginalUnitPrice.HasValue)
                    {
                        if (addValidationMessages)
                        {
                            orderItemPriceRecord.SetResult(orderItemPriceRecord.Result.EscalateTo(ValidationResultKind.IsWithinMarginOfError));
                            orderItemPriceRecord.AddValidationComment(ValidationCommentKind.Warning, 
                                String.Format(
                                    "No historical price record found for {0} at order time using {1} of {2} currency {3}.  Using legacy OriginalUnitPrice divided by quantity.",
                                    priceType.Name, orderDateField, orderDate, currencyID));

                        }
                        if (quantity != 0)
                        {
                            workingValue = legacyOriginalUnitPrice.Value / quantity;
                        }
                        else
                        {
                            workingValue = 0;
                        }
                        return true;
                    }
                    else
                    {
                        orderItemPriceRecord.SetResult(orderItemPriceRecord.Result.EscalateTo(ValidationResultKind.IsWithinMarginOfError));
                        orderItemPriceRecord.AddValidationComment(ValidationCommentKind.Warning, 
                                String.Format(
                                    "No historical price record found for {0} at order time using {1} of {2} currency {3}.  UNABLE TO CALCULATE LEGACY ORIGINAL UNIT PRICE.",
                                    priceType.Name, orderDateField, orderDate, currencyID));
                        workingValue = 0;
                        return false;
                    }
                }
            }
        }

    }
}
