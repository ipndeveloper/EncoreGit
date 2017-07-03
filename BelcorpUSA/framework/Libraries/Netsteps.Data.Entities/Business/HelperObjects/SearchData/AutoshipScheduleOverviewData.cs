using System.Collections.Generic;

namespace NetSteps.Data.Entities.Business
{
    public class AutoshipScheduleOverviewData
    {
        public int AutoshipScheduleID { get; set; }
        public int AutoshipScheduleTypeID { get; set; }
        public bool Active { get; set; }
        public string LocalizedName { get; set; }
        public List<AutoshipOverviewData> AutoshipOverviews { get; set; }
        public bool IsTemplateEditable { get; set; }
        public bool IsEnrollable { get; set; }
    }
}
