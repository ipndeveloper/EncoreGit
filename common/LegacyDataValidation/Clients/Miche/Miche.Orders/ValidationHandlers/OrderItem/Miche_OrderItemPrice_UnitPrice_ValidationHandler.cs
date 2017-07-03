using NetSteps.Validation.Common;
using NetSteps.Validation.Handlers;
using NetSteps.Validation.Handlers.Encore.Common.Services;
using NetSteps.Validation.Handlers.OrderItemPrice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miche.Orders.ValidationHandlers.OrderItem
{
    public class Miche_OrderItemPrice_UnitPrice_ValidationHandler : OrderItemPrice_UnitPrice_ValidationHandler
    {
        public Miche_OrderItemPrice_UnitPrice_ValidationHandler(IRecordPropertyCalculationHandlerResolver resolver)
            : base(resolver)
        {
        }

        protected override decimal CalculatePrimaryVolumePrice(NetSteps.Validation.Common.Model.IRecord orderItemRecord, NetSteps.Validation.Common.Model.IRecord orderItemPriceRecord, int quantity, int currencyID, DateTime orderDate, IOrderItemPricingService pricingService)
        {
            if (orderItemRecord.Properties[EncoreFieldNames.OrderItem.HostessRewardRuleID].OriginalValue != null)
            {
                orderItemPriceRecord.AddValidationComment(NetSteps.Validation.Common.Model.ValidationCommentKind.CalculationComment, "Miche rule: Any item with hostess rewards has no volume.");
                return 0;
            }
            return base.CalculatePrimaryVolumePrice(orderItemRecord, orderItemPriceRecord, quantity, currencyID, orderDate, pricingService);
        }
    }
}
