using System;
using System.Collections.Generic;
using NetSteps.Common.Base;
using NetSteps.Common.Interfaces;

namespace NetSteps.Data.Entities.Business
{
    public class OrderPaymentsParameters
    {
        // no null pero no se ve en el documento  Amount default 0,BillingPostalCode ='',PaymentTypeID  foreign key
        public decimal OrderPaymentID { get; set; }
        public int PaymentConfigurationID { get; set; }
        public int FineAndInterestRulesID { get; set; }
        public int TicketNumber { get; set; }
        public int OrderID { get; set; }
        public int OrderCustomerID { get; set; }
        public int CurrencyID { get; set; }
        public int OrderPaymentStatusID { get; set; }        
        public DateTime? OriginalExpirationDateUTC { get; set; }
        public DateTime? CurrentExpirationDateUTC { get; set; }
        public decimal? InitialAmount { get; set; }
        public decimal? TotalAmount { get; set; }
        public DateTime DateLastTotalAmountUTC { get; set; }
        public int ExpirationStatusID { get; set; }
        public int NegotiationLevelID { get; set; }
        public bool IsDeferred { get; set; }
        public DateTime? ProcessOnDateUTC { get; set; }
        public DateTime? ProcessedDateUTC { get; set; }
        public string TransactionID { get; set; }
        public int ModifiedByUserID { get; set; }
        public decimal? PaymentGatewayID { get; set; }
        public DateTime DateCreatedUTC { get; set; }
        public DateTime DateLastModifiedUTC { get; set; }
        public int PaymentTypeID { get; set; }  //  en docum dice guardar null , pero en bd no admite nulos
        public int? DeferredAmount { get; set; }
        
        public  decimal? DiscountedAmount{get; set;}
        public  decimal? ProductCredit{get; set;}
        
    }
}
