using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Common.Attributes;
using System.ComponentModel.DataAnnotations;

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchData
{
   public  class PaymetTycketsReportSearchData
    {
    [TermName("AccountNumber", "Account Number")]
    public string AccountNumber { get; set; }
    [TermName("AccountName", "Account Name")]
    public string AccountName { get; set; }
    [TermName("TicketNumber", "Ticket Number")]
    public int TicketNumber { get; set; }
    [TermName("OrderID", "OrderID")]
    public string OrderNumber { get; set; }
    [TermName("StatusPayment", "Status Payment")]
    public string StatusPaymentName { get; set; }

    [TermName("DescPayConf", "Desc.Payment Conf.")]
    public string DescPayConf { get; set; }

    [TermName("NegotiationLevelName", "Name Level")]
    public string NegotiationLevelName { get; set; }  
  
    [TermName("NameExpiration", "Name Expiration")]
    public string NameExpiration { get; set; }    
    [TermName("ExpirationDays", "Expiration Days")]
    public int ExpirationDays { get; set; }
    [TermName("Forefit", "Forefit")]
    public int Forefit { get; set; }
       
    [TermName("Period", "Period")]
    public string Period { get; set; }
    [TermName("StartDate", "StartDate")]
    public string DateCreatedUTC { get; set; }

    [TermName("ExperiedDate", "Experied Date")]
    public string CurrentExpirationDateUTC { get; set; }
    [TermName("DateValidity", "Date Validity")]
    public string DateValidity { get; set; }
    [TermName("PrincipalAmount", "PrincipalAmount")]
    public decimal InitialAmount { get; set; }
    [TermName("FinancialAmount", "Financial Amount")]
    public decimal FinancialAmount { get; set; }
    [TermName("DiscountedAmount", "Discounted Amount")]
    public decimal DiscountedAmount { get; set; }
    [TermName("TotalAmount", "Total Amount")]
    public decimal TotalAmount { get; set; }
    [TermName("Print", "Print")]
    public string  ViewTicket { get; set; }
    [TermName("Send", "Send")]
    public string Send { get; set; }

   [Display(AutoGenerateField = false)]
    public int OrderID { get; set; }
   [Display(AutoGenerateField = false)]
   public string PaymentTypeID { get; set; }
   [Display(AutoGenerateField = false)]
   public int ExpirationStatusID { get; set; }
    }
}
