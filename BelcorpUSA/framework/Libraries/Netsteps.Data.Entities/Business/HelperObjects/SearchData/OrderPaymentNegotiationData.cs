using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Microsoft.SqlServer.Server;
using System.Data;

namespace NetSteps.Data.Entities.Business
{
    public class OrderPaymentNegotiationData
    {
        [Display(Name = "OrderID")]
        public int OrderID { get; set; }

        [Display(Name = "OrderCustomerID")]
        public int OrderCustomerID { get; set; }

        [Display(Name = "PaymentTypeID")]
        public int PaymentTypeID { get; set; }

        [Display(Name = "CurrencyID")]
        public int CurrencyID { get; set; }

        [Display(Name = "OrderPaymentStatusID")]
        public Int16 OrderPaymentStatusID { get; set; }

        [Display(Name = "CreditCardTypeID")]
        public Int16 CreditCardTypeID { get; set; }

        [Display(Name = "NameOnCard")]
        public string NameOnCard { get; set; }

        [Display(Name = "AccountNumber")]
        public string AccountNumber { get; set; }

        [Display(Name = "BillingFirstName")]
        public string BillingFirstName { get; set; }

        [Display(Name = "BillingLastName")]
        public string BillingLastName { get; set; }

        [Display(Name = "BillingName")]
        public string BillingName { get; set; }

        [Display(Name = "BillingAddress1")]
        public string BillingAddress1 { get; set; }

        [Display(Name = "BillingAddress2")]
        public string BillingAddress2 { get; set; }

        [Display(Name = "BillingAddress3")]
        public string BillingAddress3 { get; set; }

        [Display(Name = "BillingCity")]
        public string BillingCity { get; set; }

        [Display(Name = "BillingCounty")]
        public string BillingCounty { get; set; }

        [Display(Name = "BillingState")]
        public string BillingState { get; set; }

        [Display(Name = "BillingStateProvinceID")]
        public int BillingStateProvinceID { get; set; }

        [Display(Name = "BillingPostalCode")]
        public string BillingPostalCode { get; set; }

        [Display(Name = "BillingCountryID")]
        public int BillingCountryID { get; set; }

        [Display(Name = "BillingPhoneNumber")]
        public string BillingPhoneNumber { get; set; }

        [Display(Name = "IdentityNumber")]
        public string IdentityNumber { get; set; }

        [Display(Name = "IdentityState")]
        public string IdentityState { get; set; }

        [Display(Name = "Amount")]
        public decimal Amount { get; set; }

        [Display(Name = "RoutingNumber")]
        public string RoutingNumber { get; set; }

        [Display(Name = "IsDeferred")]
        public bool IsDeferred { get; set; }

        [Display(Name = "ProcessOnDateUTC")]
        public DateTime ProcessOnDateUTC { get; set; }

        [Display(Name = "ProcessedDateUTC")]
        public DateTime ProcessedDateUTC { get; set; }

        [Display(Name = "TransactionID")]
        public string TransactionID { get; set; }

        [Display(Name = "DeferredAmount")]
        public decimal DeferredAmount { get; set; }

        [Display(Name = "DeferredTransactionID")]
        public string DeferredTransactionID { get; set; }

        [Display(Name = "ExpirationDateUTC")]
        public DateTime ExpirationDateUTC { get; set; }

        //[Display(Name = "DataVersion")]
        //public TimeSpan DataVersion { get; set; }

        [Display(Name = "ModifiedByUserID")]
        public int ModifiedByUserID { get; set; }

        [Display(Name = "Request")]
        public string Request { get; set; }

        [Display(Name = "AccountNumberLastFour")]
        public string AccountNumberLastFour { get; set; }

        [Display(Name = "PaymentGatewayID")]
        public Int16 PaymentGatewayID { get; set; }

        [Display(Name = "SourceAccountPaymentMethodID")]
        public int SourceAccountPaymentMethodID { get; set; }

        [Display(Name = "BankAccountTypeID")]
        public Int16 BankAccountTypeID { get; set; }

        [Display(Name = "BankName")]
        public string BankName { get; set; }

        [Display(Name = "NachaClassType")]
        public string NachaClassType { get; set; }

        [Display(Name = "NachaSentDate")]
        public DateTime NachaSentDate { get; set; }

        [Display(Name = "ETLNaturalKey")]
        public string ETLNaturalKey { get; set; }

        [Display(Name = "ETLHash")]
        public string ETLHash { get; set; }

        [Display(Name = "ETLPhase")]
        public string ETLPhase { get; set; }

        [Display(Name = "ETLDate")]
        public DateTime ETLDate { get; set; }

        [Display(Name = "DateCreatedUTC")]
        public DateTime DateCreatedUTC { get; set; }

        [Display(Name = "DateLastModifiedUTC")]
        public DateTime DateLastModifiedUTC { get; set; }

        [Display(Name = "BillingStreet")]
        public string BillingStreet { get; set; }

        [Display(Name = "NegotiationLevelID")]
        public int NegotiationLevelID { get; set; }

        [Display(Name = "OrderExpirationStatusID")]
        public int OrderExpirationStatusID { get; set; }

        [Display(Name = "PaymentConfigurationID")]
        public int PaymentConfigurationID { get; set; }

        [Display(Name = "FineAndInterestsRulesID")]
        public int FineAndInterestsRulesID { get; set; }

        [Display(Name = "TicketNumber")]
        public int TicketNumber { get; set; }

        [Display(Name = "OriginalExpirationDate")]
        public DateTime OriginalExpirationDate { get; set; }

        [Display(Name = "CurrentExpirationDateUTC")]
        public string CurrentExpirationDateUTC { get; set; }

        [Display(Name = "InitialAmount")]
        public string InitialAmount { get; set; }

        [Display(Name = "FinancialAmount")]
        public decimal FinancialAmount { get; set; }

        [Display(Name = "DiscountedAmount")]
        public decimal DiscountedAmount { get; set; }

        [Display(Name = "TotalAmount")]
        public string TotalAmount { get; set; }

        [Display(Name = "DateLastTotalAmountUTC")]
        public DateTime DateLastTotalAmountUTC { get; set; }

        [Display(Name = "Accepted")]
        public bool Accepted { get; set; }

        [Display(Name = "Forefit")]
        public bool Forefit { get; set; }

        [Display(Name = "ExpirationStatusID")]
        public int ExpirationStatusID { get; set; }


        [Display(Name = "OrderPaymentID")]
        public int OrderPaymentID { get; set; }

        [Display(Name = "ExpirationDays")]
        public int ExpirationDays { get; set; }

        [Display(Name = "DateValidity")]
        public string DateValidity { get; set; }

        [Display(Name = "RenegotiationConfigurationID")]
        public int RenegotiationConfigurationID { get; set; }
    }
}
