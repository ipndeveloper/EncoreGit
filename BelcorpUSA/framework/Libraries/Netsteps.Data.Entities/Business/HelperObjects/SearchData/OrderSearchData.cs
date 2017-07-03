using System;
using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Attributes;

namespace NetSteps.Data.Entities.Business
{
    [Serializable]
    public struct OrderSearchData
    {
        [TermName("ID")]
        [Display(AutoGenerateField = false)]
        public int OrderID { get; set; }

        [TermName("OrderNumber", "Order Number")]
        public string OrderNumber { get; set; }

        /*CS.20AG2016.Inicio*/
        [TermName("InvoiceNumber", "Invoice Number")]
        public string InvoiceNumber { get; set; }
        /*CS.20AG2016.Fin*/

        [TermName("ConsultantCode", "Consultant Code")]
        [Display(AutoGenerateField = true)]
        public string AccountNumber { get; set; }

        [TermName("FirstName", "First Name")]
        public string FirstName { get; set; }

        [TermName("LastName", "Last Name")]
        public string LastName { get; set; }

        [Display(AutoGenerateField = false)]
        public int OrderStatusID { get; set; }

        [TermName("Status")]
        [Display(Name = "Status")]
        [PropertyName("OrderStatus.TermName")]
        public string OrderStatus { get; set; }

        [Display(AutoGenerateField = false)]
        public short OrderTypeID { get; set; }

        [Display(AutoGenerateField = false)]
        public int CurrencyID { get; set; }

        [TermName("Type")]
        [Display(Name = "Type")]
        [PropertyName("OrderType.TermName")]
        public string OrderType { get; set; }


        [TermName("CreationPeriod")]
        [Display(Name = "Creation Period")]
        public string CreationPeriod { get; set; }

        [TermName("CompletePeriod")]
        [Display(Name = "Complete Period")]
        public string CompletePeriod { get; set; }

        [TermName("DateCreated", "Date Created")]
        [Display(AutoGenerateField = false)]
        public DateTime? DateCreated { get; set; }

        [TermName("CompleteDate", "Complete Date")]
        public DateTime? CompleteDate { get; set; }

        [TermName("DateShipped", "Date Shipped")]
        [Display(AutoGenerateField = false)]
        public DateTime? DateShipped { get; set; }

        [TermName("CommissionDate", "Commission Date")]
        [Display(AutoGenerateField = false)]
        public DateTime? CommissionDate { get; set; }

        [TermName("PaymentMethod", "Payment Method")]
        [Display(Name = "PaymentMethod")]
        [Sortable(false)]
        [PropertyName("PaymentMethod")]
        public string PaymentMethod { get; set; }

        [TermName("Subtotal")]
        [Display(Name = "Subtotal")]
        [PropertyName("Subtotal")]
        public decimal Subtotal { get; set; }

        [TermName("GrandTotal")]
        public decimal GrandTotal { get; set; }

        [TermName("Total")]
        [Display(AutoGenerateField = false)]
        public decimal CustomerTotal { get; set; }

        [TermName("Sponsor")]
        [Display(AutoGenerateField = false)]
        public string SponsorAccountNumber { get; set; }

        [TermName("Sponsor")]
        public string Sponsor { get; set; }

        [TermName("TrackingNumber", "Tracking Number")]
        [Display(AutoGenerateField = false)]
        public string TrackingNumber { get; set; }

        [TermName("TrackingUrl", "Tracking Url")]
        [Display(AutoGenerateField = false)]
        public string TrackingUrl { get; set; }


        [TermName("ShippingMethodId", "Shipping Method Id")]
        [Display(AutoGenerateField = false)]
        public int? ShippingMethodId { get; set; }

        public bool IsReturnOrder()
        {
            return Order.IsReturnOrder(this.OrderTypeID);
        }

        public bool IsAutoshipOrder()
        {
            return Order.IsAutoshipOrder(this.OrderTypeID);
        }

        [TermName("Market", "Market")]
        [Display(AutoGenerateField = false)]
        public int? MarketID { get; set; }

        [Display(AutoGenerateField = false)]
        public decimal CommissionableTotal { get; set; }

        /*CS.21AG2016.Inicio*/
        [TermName("RowTotals", "RowTotals")]
        [Display(AutoGenerateField = false)]
        public int RowTotals { get; set; }
        /*CS.21AG2016.fIN*/

        //CGI(CMR)-24/10/2014-Inicio
        [TermName("TotalQV")]
        [Display(Name = "TotalQV")]
        [PropertyName("TotalQV")]
        public decimal TotalQV { get; set; }
        //CGI(CMR)-24/10/2014-Fin

        //INI - GR4172
        [TermName("CompletePeriod", "Period")]
        [Display(AutoGenerateField = false)]
        public string PeriodID { get; set; }
        //FIN - GR4172
    }
}
