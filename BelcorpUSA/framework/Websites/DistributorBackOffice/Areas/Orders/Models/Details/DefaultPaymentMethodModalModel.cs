using System;

namespace DistributorBackOffice.Areas.Orders.Models.Details
{
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
