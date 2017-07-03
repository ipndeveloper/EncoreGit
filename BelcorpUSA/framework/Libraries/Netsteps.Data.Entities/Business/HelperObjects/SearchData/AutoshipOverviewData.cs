using System;
using System.Collections.Generic;

namespace NetSteps.Data.Entities.Business
{
    public class AutoshipOverviewData
    {
        public int AccountID { get; set; }
        public short AccountStatusID { get; set; }
        public int AutoshipOrderID { get; set; }
        public int AutoshipScheduleID { get; set; }
        public int TemplateOrderID { get; set; }
        public string TemplateOrderNumber { get; set; }
        public short OrderTypeID { get; set; }
        public short OrderStatusID { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? NextRunDate { get; set; }
        public List<OrderItemData> OrderItems { get; set; }
        public bool Active { get; set; }
        public string StatusText { get; set; }

        public class OrderItemData
        {
            public int Quantity { get; set; }
            public string SKU { get; set; }
        }
    }
}
