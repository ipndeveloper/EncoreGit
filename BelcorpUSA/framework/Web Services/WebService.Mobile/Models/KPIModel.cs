using System.Collections.Generic;

namespace NetSteps.WebService.Mobile.Models
{
    public class KPIModel
    {
        public string periodname;
        public int periodid;
        public decimal pv;
        public decimal gv;
        public string pvGoalPercent;
        public string gvGoalPercent;
        public string title;
        public string paidAsTitle;
        public int PersonallySponsoredCount;
        public int DownlineCount;
        public List<KPIRptWidget> CustomKPIReports;
    }
}