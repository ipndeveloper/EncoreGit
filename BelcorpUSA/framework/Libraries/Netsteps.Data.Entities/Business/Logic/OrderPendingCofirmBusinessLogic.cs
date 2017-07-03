namespace NetSteps.Data.Entities.Business.Logic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NetSteps.Data.Entities.Repositories;
    using NetSteps.Data.Entities.Repositories.Interfaces;

    public class OrderPendingCofirmBusinessLogic
    {
        public static OrderPendingCofirmBusinessLogic Instance
        {
            get 
            {
                if(instance == null)
                {
                    instance = new OrderPendingCofirmBusinessLogic();
                    repository = new OrderPendingCofirmRepository();
                }

                return instance;
            }
        }

        private static OrderPendingCofirmBusinessLogic instance;

        private static IOrderPendingConfirmRepository repository; 

        public List<Order> OrderSearch(DateTime? DateStart, DateTime? DateEnd, string OrderNumber)
        {
            return (from o in repository.OrderSearch(DateStart, DateEnd, OrderNumber)
                    select new Order()
                    {
                        OrderNumber = o.OrderNumber,
                        AccountNumber = o.AccountNumber,
                        IDSupportTicket = o.IDSupportTicket,
                        IDNationalMail = o.IDNationalMail,
                        DateCreatedUTC = o.DateCreatedUTC,
                        CustomerFirstName = o.CustomerFirstName,
                        CustomerLastName = o.CustomerLastName,
                        OrderID = o.OrderID  
                    }).ToList();
        }

        public List<OrderItemReturnConfirm> GetDetaillOrderItemReturnConfirm(string OrderNumber)
        {
            return (from oirc in repository.GetDetaillOrderItemReturnConfirm(OrderNumber)
                    select new OrderItemReturnConfirm()
                    {
                        CUV = oirc.CUV,
                        Note = oirc.Note,
                        Diferencia = oirc.Diferencia,
                        OrderItemConfirmID = oirc.OrderItemConfirmID,
                        Quantity = oirc.Quantity,
                        DateLastModifiedUTC = oirc.DateLastModifiedUTC,
                        ModifiedByUserID = oirc.ModifiedByUserID,
                        ProductName = oirc.ProductName,
                        QuantityOrderItem = oirc.QuantityOrderItem,
                        QuantityOrderItemReturn = oirc.QuantityOrderItemReturn,
                        QuantityOrderItemReturnConfirm = oirc.QuantityOrderItemReturnConfirm,
                        OrderItemReturnID = oirc.OrderItemReturnID,
                        ItemPrice = oirc.ItemPrice
                    }).ToList();
        }

        /// <summary>
        /// Inserta o actualiza items recibidos, observaciones y notas
        /// </summary>
        /// <param name="lstOrderItemReturnConfirm"></param>
        /// <param name="orderId"></param>
        /// <param name="orderNote"></param>
        /// <param name="noteTypeID"></param>
        /// <returns></returns>
        public int InsertOrderPendingCofirm(List<OrderItemReturnConfirm> lstOrderItemReturnConfirm, int orderId, string orderNote, int noteTypeID)
        {
            return repository.InsertOrderPendingConfirm((from oirc in lstOrderItemReturnConfirm
                                                         select new NetSteps.Data.Entities.Dto.OrderItemReturnConfirmDto()
                                                         {
                                                             CUV = oirc.CUV,
                                                             Note = oirc.Note,
                                                             Diferencia = oirc.Diferencia,
                                                             OrderItemConfirmID = oirc.OrderItemConfirmID,
                                                             Quantity = oirc.Quantity,
                                                             DateLastModifiedUTC = oirc.DateLastModifiedUTC,
                                                             ModifiedByUserID = oirc.ModifiedByUserID,
                                                             ProductName = oirc.ProductName,
                                                             QuantityOrderItem = oirc.QuantityOrderItem,
                                                             QuantityOrderItemReturn = oirc.QuantityOrderItemReturn,
                                                             QuantityOrderItemReturnConfirm = oirc.QuantityOrderItemReturnConfirm,
                                                             OrderItemReturnID = oirc.OrderItemReturnID,
                                                             ItemPrice = oirc.ItemPrice
                                                         }).ToList(), orderId, orderNote, noteTypeID);
        }
    }
}
