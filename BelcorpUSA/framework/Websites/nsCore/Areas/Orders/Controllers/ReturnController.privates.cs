namespace nsCore.Areas.Orders.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using NetSteps.Data.Entities;
    using NetSteps.Data.Entities.Extensions;
    using nsCore.Models;
    using NetSteps.Data.Entities.Business.Logic;
    using NetSteps.Data.Common.Entities;
    using NetSteps.Data.Entities.Business;

    public partial class ReturnController
    {
        /// <summary>
        /// Insets into Item return confirm table
        /// </summary>
        /// <param name="order">Order with returns</param>
        /// <param name="returnedProducts">products selected to return</param>
        private void UpdateOrderItemReturn(Order order, List<ProductReturn> returnedProducts)
        {
            foreach (var orderReturn in order.OrderCustomers.SelectMany(m => m.OrderItems.SelectMany(r => r.OrderItemReturns)))
            {
                OrderItemReturnConfirmExtension.InsItemReturnConfirm(orderReturn.OrderItemReturnID, 0);
            }
        }

        public static void ClearAllocation(IOrder order)
        {
            //Order.PreOrders = new PreOrder();
            //foreach (var item in order.OrderCustomers.SelectMany(m => m.OrderItems))
            //{
            //    int wareHouseID = WarehouseExtensions.WareHouseByAddresID(order.OrderCustomers[0].AccountID);
            //    Order.PreOrders.WareHouseID = wareHouseID;
            //    Order.UpdateLineOrder(item.ProductID.Value, item.Quantity, wareHouseID);
            //}
        }

        /// <summary>
        /// Llama al proceso ApplyCredit
        /// </summary>
        /// <param name="accountId">Id de cuenta</param>       
        /// <param name="entryAmount">Monto a devolver</param>
        /// <param name="orderId">Identificador de la Orden de Devolución OrderID</param>
        /// <param name="orderPaymentId">Identificador de la Orden Padre ParentOrderID</param>
        public static void ApplyCredit(int accountId, decimal entryAmount, int orderId, int? orderPaymentId)
        {
            int entryReasonId = (int)NetSteps.Data.Entities.Generated.ConstantsGenerated.LedgerEntryReasons.ProductReturn;
            int entryOrigin = (int)NetSteps.Data.Entities.Generated.ConstantsGenerated.LedgerEntryOrigins.OrderEntry;
            int entryTypeId = (int)NetSteps.Data.Entities.Generated.ConstantsGenerated.LedgerEntryTypes.ReturnAdjustment;

            NetSteps.Data.Entities.Business.CTE.ApplyCredit(accountId, entryReasonId, entryOrigin, entryTypeId, entryAmount, orderId, orderPaymentId);
        }

        public static void ProcedimientoCuentaCorriente(int OrderID, string TipoMovimiento, decimal MontoParcial, int UserID)
        {
            NetSteps.Data.Entities.Business.CTE.ProcedimientoCuentaCorriente(OrderID, TipoMovimiento, MontoParcial, UserID);
        }

        /// <summary>
        /// update Account Datos
        /// </summary>
        /// <param name="accountID"></param>
        /// <param name="orderId"></param>
        /// <param name="periodID"></param>
        public static int ReverseStatusActivity(int accountID, int orderId, int periodID)
        {
            return OrderBusinessLogic.Instance.ReverseStatusActivity(accountID, orderId, periodID);
        }
    }
}