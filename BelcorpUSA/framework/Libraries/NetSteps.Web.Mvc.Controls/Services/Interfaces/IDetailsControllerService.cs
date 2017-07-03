// -----------------------------------------------------------------------
// <copyright file="IDetailsController.cs" company="NetSteps">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using NetSteps.Common.Base;
using NetSteps.Data.Entities;

namespace NetSteps.Web.Mvc.Controls.Services.Interfaces
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IDetailsControllerService
    {
        Order LoadOrder(string id);
        OrderPayment LoadPayment(int id);
        bool IsOrderNotComplete(Order order);
        bool IsPartyOrder(Order order);
        IPaymentMethodModel GetPaymentMethodModel(OrderPayment payment);
        BasicResponse DisallowAutoshipTemplateEdits(Order order);
        Order ChangeCommissionConsultant(Order order, int commissionConsultantID);
        Order ChangeAttachedParty(Order order, int newPartyOrderID);
        Order ChangeCommissionDate(Order order, DateTime commissionDate);
    }

    public static class DetailsControllerExtensions
    {
        public static Order ChangeCommissionConsultant(this IDetailsControllerService service, string orderNumber, int commissionConsultantID)
        {
            Order order = service.LoadOrder(orderNumber);
            service.ChangeCommissionConsultant(order, commissionConsultantID);
            return order;
        }

        public static Order ChangeAttachedParty(this IDetailsControllerService service, string orderNumber, int newPartyOrderID)
        {
            Order order = service.LoadOrder(orderNumber);
            service.ChangeAttachedParty(order, newPartyOrderID);
            return order;
        }

        public static Order ChangeCommissionDate(this IDetailsControllerService service, string orderNumber, DateTime commissionDate)
        {
            Order order = service.LoadOrder(orderNumber);
            service.ChangeCommissionDate(order, commissionDate);
            return order;
        }
    }
}