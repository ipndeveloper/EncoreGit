// -----------------------------------------------------------------------
// <copyright file="DefaultPaymentMethodModalModel.cs" company="NetSteps">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using NetSteps.Web.Mvc.Controls.Services.Interfaces;

namespace NetSteps.Web.Mvc.Controls.Services
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class DefaultPaymentMethodModalModel : IPaymentMethodModel
    {
        public string DecryptedAccountNumber { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string BillingName { get; set; }
        public string BillingAddress1 { get; set; }
        public string BillingCity { get; set; }
        public string BillingState { get; set; }
        public string BillingPostalCode { get; set; }
        public int? BillingCountryId { get; set; }
        public short OrderPaymentStatusId { get; set; }
        public string TransactionId { get; set; }
        public string PartialName { get; set; }

        public string GetPartialViewName()
        {
            return "DefaultPaymentMethodModal";
        }
    }
}
