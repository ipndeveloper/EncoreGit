using System.Collections.Generic;
using NetSteps.Data.Entities;

namespace DistributorBackOffice.Models.Home
{
    public class AutoshipOrderViewModel
    {
        public AutoshipOrder AutoshipOrder { get; set; }

        public AutoshipSchedule AutoshipSchedule { get; set; }

        public bool IsValid { get; set; }

        public bool IsCanceled { get; set; }

        public IEnumerable<OrderItem> OrderItems { get; set; }

        public Site Site { get; set; }

        public bool IsTemplateEditable { get; set; }

        public bool IsEnrollable { get; set; }

        public AutoshipOrderViewModel(AutoshipSchedule schedule)
        {
            this.AutoshipSchedule = schedule;
            this.IsTemplateEditable = schedule.IsTemplateEditable;
            this.IsEnrollable = schedule.IsEnrollable;
        }
    }
}