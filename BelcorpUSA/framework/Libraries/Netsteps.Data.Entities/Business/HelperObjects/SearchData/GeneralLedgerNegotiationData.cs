using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Microsoft.SqlServer.Server;
using System.Data;

namespace NetSteps.Data.Entities.Business
{
    public class GeneralLedgerNegotiationData
    {
        [Display(AutoGenerateField = false)]
        public int TicketNumber { get; set; }
        
        [Display(Name = "Payment Method")]
        public string PaymentMethod { get; set; }

        [Display(Name = "Total Amount")]
        public decimal TotalAmount  { get; set; }

        [Display(Name = "Order Number")]
        public string OrderNumber { get; set; }

        [Display(Name = "Ticket Status")]
        public string TicketStatus { get; set; }

        [Display(Name = "Authorization Number")]
        public string AuthorizationNumber { get; set; }

        [Display(Name = "Payment Type ID")]
        public int PaymentTypeID { get; set; }

        [Display(Name = "Expiration Status ID")]
        public int ExpirationStatusID { get; set; }

        [Display(Name = "Is Deferred")]
        public bool IsDeferred { get; set; }

        [Display(Name = "Negotiation Level ID")]
        public int NegotiationLevelID { get; set; }

        [Display(Name = "Negotiation Level")]
        public string NegotiationLevel { get; set; }

        [Display(Name = "Initial Amount")]
        public decimal InitialAmount { get; set; }

        [Display(Name = "Financial Amount")]
        public decimal FinancialAmount { get; set; }

        [Display(Name = "Maximum Amount Of Payments")]
        public int MaximumAmountOfPayments { get; set; }

        [Display(Name = "Discounted Amount")]
        public decimal DiscountedAmount { get; set; }

        [Display(Name = "Order Payment ID")]
        public int OrderPaymentID { get; set; }

        [Display(Name = "Days For Payment")]
        public int DaysForPayment { get; set; }

        [Display(Name = "OrderID")]
        public int OrderID { get; set; }

        [Display(Name = "PaymentConfigurationID")]
        public int PaymentConfigurationID { get; set; }

        [Display(Name = "FineAndInterestsRulesID")]
        public int FineAndInterestsRulesID { get; set; }

        [Display(Name = "OrderCustomerID")]
        public int OrderCustomerID { get; set; }

        [Display(AutoGenerateField = false)]
        public string CurrentExpirationDateUTC { get; set; }

        [Display(AutoGenerateField = false)]
        public string DayValidate { get; set; }

        [Display(AutoGenerateField = false)]
        public int DayExpiration { get; set; }

        [Display(AutoGenerateField = false)]
        public int ViewMethodsRenegotiation { get; set; }

        [Display(AutoGenerateField = false)]
        public string PaymentCredit { get; set; }

    }
   
}
