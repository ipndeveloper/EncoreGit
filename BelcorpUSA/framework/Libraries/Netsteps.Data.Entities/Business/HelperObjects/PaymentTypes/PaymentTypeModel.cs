using System;

namespace NetSteps.Data.Entities.Business.HelperObjects.PaymentTypes
{
    public class PaymentTypeModel
    {
        public int PaymentType { get; set; }
        public int? PaymentMethodID { get; set; }
        public string AccountNumber { get; set; }
        public string NameOnCard { get; set; }
        public string Cvv { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public decimal Amount { get; set; }
        public string BillingZipCode { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string AddressLine { get; set; }
        public string GiftCardCode { get; set; }
        public string NameOnAccount { get; set; }
        public string RoutingNumber { get; set; }
        public string BankAccountNumber { get; set; }
        public short BankAccountTypeID { get; set; }
        public string ProfileName { get; set; }
        public string BankName { get; set; }
        public int? NumberCuota { get; set; }
        public string AmountConfiguration { get; set; }
    }
}
