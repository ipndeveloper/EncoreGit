using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business
{
    public class OrderStatusPartiallyPaidSearchData
    {
        public int RowTotal { get; set; }
        public int RowChild { get; set; }
        public int OrderID { get; set; }
        public int TicketNumber { get; set; }
        public string Name { get; set; }
    }
}
