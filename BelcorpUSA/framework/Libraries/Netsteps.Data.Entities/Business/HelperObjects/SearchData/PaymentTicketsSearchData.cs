using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Attributes;

namespace NetSteps.Data.Entities.Business
{
    public class PaymentTicketsSearchData
    {
        
        public int ID { get; set; }
        public int TicketNumber { get; set; }
        public string OrderNumber { get; set; }
        public DateTime? CompletedOn { get; set; }
        public string Status { get; set; }
        public decimal InitialAmount { get; set; }
        public decimal FinancialAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public DateTime? DateValidity { get; set; }        
        public string EpirationStatus {get;set;}
        public string PaymentType { get; set; }        
        public string ViewTicket { get; set; }
        public string Send { get; set; }
    }
}
