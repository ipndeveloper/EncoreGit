namespace NetSteps.Data.Entities.Repositories.Interfaces
{
    using System.Collections.Generic;
    using NetSteps.Data.Entities.Dto;
    using System;

    /// <summary>
    /// Descripcion de la interface
    /// </summary>
    public interface IOrderPendingConfirmRepository
    {
        /// <summary>
        /// Descripcion del metodo
        /// </summary>
        IEnumerable<OrderDto> OrderSearch(DateTime? DateStart, DateTime? DateEnd, string OrderNumber);

        IEnumerable<OrderItemReturnConfirmDto> GetDetaillOrderItemReturnConfirm(string OrderNumber);

        int InsertOrderPendingConfirm(IEnumerable<OrderItemReturnConfirmDto> lstOrderItemReturnConfirm, int orderId, string orderNote, int noteType);
    }
}
