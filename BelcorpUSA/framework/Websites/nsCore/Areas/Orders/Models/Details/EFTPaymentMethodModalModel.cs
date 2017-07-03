
namespace nsCore.Areas.Orders.Models.Details
{
    public class EFTPaymentMethodModalModel : IPaymentMethodModel
    {
        public string DecryptedAccountNumber { get; set; }
        public string RoutingNumber { get; set; }
        public string AccountType { get; set; }
        public string BillingName { get; set; }
        public string BillingAddress1 { get; set; }
        public string BillingCity { get; set; }
        public string BillingState { get; set; }
        public string BillingPostalCode { get; set; }
        public int? BillingCountryId { get; set; }
        public string BankName { get; set; }
        public short OrderPaymentStatusId { get; set; }

        public string GetPartialViewName()
        {
            return "EFTPaymentMethodModal";
        }
    }
}
