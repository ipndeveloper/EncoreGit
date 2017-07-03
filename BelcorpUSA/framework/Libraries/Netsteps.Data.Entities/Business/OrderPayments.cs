using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

//Modificaciones:
//@1 20151607 BR-CC-012 GYS MD: Se implemento la clase OrderPayments que representa la tabla OrderPayments 
namespace NetSteps.Data.Entities.Business
{
    [Table("OrderPayments")]
    public class OrderPayments : DynamicModel
    {
        public OrderPayments() : base("Core", "OrderPayments", "OrderPaymentID")
        {
        }

        [Column("OrderPaymentID"), Key]
        public int OrderPaymentID { get; set; }

        [Column("OrderID")]
        public int OrderID { get; set; }

        [Column("OrderCustomerID")]
        public int? OrderCustomerID { get; set; }

        [Column("PaymentTypeID")]
        public int PaymentTypeID { get; set; }

        [Column("CurrencyID")]
        public int CurrencyID { get; set; }

        [Column("OrderPaymentStatusID")]
        public short OrderPaymentStatusID { get; set; }

        [Column("CreditCardTypeID")]
        public short? CreditCardTypeID { get; set; }

        [Column("NameOnCard")]
        public string NameOnCard { get; set; }

        [Column("AccountNumber")]
        public string AccountNumber { get; set; }

        [Column("BillingFirstName")]
        public string BillingFirstName { get; set; }

        [Column("BillingLastName")]
        public string BillingLastName { get; set; }

        [Column("BillingName")]
        public string BillingName { get; set; }

        [Column("BillingAddress1")]
        public string BillingAddress1 { get; set; }

        [Column("BillingAddress2")]
        public string BillingAddress2 { get; set; }

        [Column("BillingAddress3")]
        public string BillingAddress3 { get; set; }

        [Column("BillingCity")]
        public string BillingCity { get; set; }

        [Column("BillingCounty")]
        public string BillingCounty { get; set; }

        [Column("BillingState")]
        public string BillingState { get; set; }

        [Column("BillingStateProvinceID")]
        public int? BillingStateProvinceID { get; set; }

        [Column("BillingPostalCode")]
        public string BillingPostalCode { get; set; }

        [Column("BillingCountryID")]
        public int? BillingCountryID { get; set; }

        [Column("BillingPhoneNumber")]
        public string BillingPhoneNumber { get; set; }

        [Column("IdentityNumber")]
        public string IdentityNumber { get; set; }

        [Column("IdentityState")]
        public string IdentityState { get; set; }

        [Column("Amount")]
        public decimal Amount { get; set; }

        [Column("RoutingNumber")]
        public string RoutingNumber { get; set; }

        [Column("IsDeferred")]
        public bool IsDeferred { get; set; }

        [Column("ProcessOnDateUTC")]
        public DateTime? ProcessOnDateUTC { get; set; }

        [Column("ProcessedDateUTC")]
        public DateTime? ProcessedDateUTC { get; set; }

        [Column("TransactionID")]
        public string TransactionID { get; set; }

        [Column("DeferredAmount")]
        public decimal? DeferredAmount { get; set; }

        [Column("DeferredTransactionID")]
        public string DeferredTransactionID { get; set; }

        [Column("ExpirationDateUTC")]
        public DateTime? ExpirationDateUTC { get; set; }

        [Column("DataVersion")]
        public byte[] DataVersion { get; set; }

        [Column("ModifiedByUserID")]
        public int? ModifiedByUserID { get; set; }

        [Column("Request")]
        public string Request { get; set; }

        [Column("AccountNumberLastFour")]
        public string AccountNumberLastFour { get; set; }

        [Column("PaymentGatewayID")]
        public short? PaymentGatewayID { get; set; }

        [Column("SourceAccountPaymentMethodID")]
        public int? SourceAccountPaymentMethodID { get; set; }

        [Column("BankAccountTypeID")]
        public short? BankAccountTypeID { get; set; }

        [Column("BankName")]
        public string BankName { get; set; }

        [Column("NachaClassType")]
        public string NachaClassType { get; set; }

        [Column("NachaSentDate")]
        public DateTime? NachaSentDate { get; set; }

        [Column("ETLNaturalKey")]
        public string ETLNaturalKey { get; set; }

        [Column("ETLHash")]
        public string ETLHash { get; set; }

        [Column("ETLPhase")]
        public string ETLPhase { get; set; }

        [Column("ETLDate")]
        public DateTime? ETLDate { get; set; }

        [Column("DateCreatedUTC")]
        public DateTime DateCreatedUTC { get; set; }

        [Column("DateLastModifiedUTC")]
        public DateTime? DateLastModifiedUTC { get; set; }

        [Column("BillingStreet")]
        public string BillingStreet { get; set; }

        [Column("NegotiationLevelID")]
        public int? NegotiationLevelID { get; set; }

        [Column("OrderExpirationStatusID")]
        public int? OrderExpirationStatusID { get; set; }

        [Column("PaymentConfigurationID")]
        public int? PaymentConfigurationID { get; set; }

        [Column("FineAndInterestsRulesID")]
        public int? FineAndInterestsRulesID { get; set; }

        [Column("TicketNumber")]
        public int? TicketNumber { get; set; }

        [Column("OriginalExpirationDate")]
        public DateTime? OriginalExpirationDate { get; set; }

        [Column("CurrentExpirationDateUTC")]
        public DateTime? CurrentExpirationDateUTC { get; set; }

        [Column("InitialAmount")]
        public decimal? InitialAmount { get; set; }

        [Column("FinancialAmount")]
        public decimal? FinancialAmount { get; set; }

        [Column("DiscountedAmount")]
        public decimal? DiscountedAmount { get; set; }

        [Column("TotalAmount")]
        public decimal? TotalAmount { get; set; }

        [Column("DateLastTotalAmountUTC")]
        public DateTime? DateLastTotalAmountUTC { get; set; }

        [Column("Accepted")]
        public bool? Accepted { get; set; }

        [Column("Forefit")]
        public bool? Forefit { get; set; }

        [Column("ExpirationStatusID")]
        public int ExpirationStatusID { get; set; }

        [Column("DateValidity")]
        public DateTime? DateValidity { get; set; }

        [Column("RenegotiationConfigurationID")]
        public int? RenegotiationConfigurationID { get; set; }

        [Column("ExpirationDays")]
        public int? ExpirationDays { get; set; }
    }
}
