using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Reports
{
    public class GMP_Reports_ExecutiveReports_Control : GMP_Reports_Reports_Control
    {
        public GMP_Reports_ExecutiveDashboard_Page ClickExecutiveDashboard(int? timeout = null)
        {
            return OpenReport<GMP_Reports_ExecutiveDashboard_Page>("Executive\\+Dashboard", timeout);
        }

        public GMP_Reports_ExecutiveSummary_Page ClickSummary(int? timeout = null)
        {
            return OpenReport<GMP_Reports_ExecutiveSummary_Page>("Executive\\+Summary", timeout);
        }

        public GMP_Reports_TopEarnersByVolume_Page ClickTopEarnersByVolume(int? timeout = null)
        {
            return OpenReport<GMP_Reports_TopEarnersByVolume_Page>("Top\\+Earners\\+by\\+Volume", timeout);
        }
    }
}
