using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business
{
    public class OrderStatusShippedSearchData
    {
        public int RowChild { get; set; }
        public DateTime LogDateUTC { get; set; }
        public string Description { get; set; }
        public int TrackingNumber { get; set; }
        public string Name { get; set; }
        public int RowTotal { get; set; }
    }
}
