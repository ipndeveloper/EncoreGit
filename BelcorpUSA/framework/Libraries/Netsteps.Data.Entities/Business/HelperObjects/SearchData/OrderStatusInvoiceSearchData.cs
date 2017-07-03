using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business
{
    public class OrderStatusInvoiceSearchData
    {
        public int RowChild { get; set; }        
        public int InvoiceNumber { get; set; }
        public DateTime DateInvoice { get; set; }
        public int RowTotal { get; set; }
    }
}
