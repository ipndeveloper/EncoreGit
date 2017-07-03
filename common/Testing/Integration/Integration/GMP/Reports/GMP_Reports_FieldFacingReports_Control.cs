using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Reports
{
    public class GMP_Reports_FieldFacingReports_Control : GMP_Reports_Reports_Control
    {

        public GMP_Reports_PersonalEarningsDrillThrough_Page ClickPersonalEarningsDrillthrough(int? timeout = null)
        {
            return OpenReport<GMP_Reports_PersonalEarningsDrillThrough_Page>("Personal\\+Earnings\\+Statement\\+Drillthrough", timeout);
        }

        public GMP_Reports_PersonalEarningsStatement_Page ClickPersonalEarningsStatement(int? timeout = null)
        {
            return OpenReport<GMP_Reports_PersonalEarningsStatement_Page>("Earnings\\+Statement", timeout);
        }
    }
}
