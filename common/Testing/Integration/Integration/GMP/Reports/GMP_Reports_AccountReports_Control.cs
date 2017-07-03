using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Reports
{
    public class GMP_Reports_AccountReports_Control : GMP_Reports_Reports_Control
    {
        protected override void InitializeContents()
        {
            base.InitializeContents();
        }

        public GMP_Reports_AccountsDataDump_Page ClickDataDump(int? timeout = null)
        {
            return OpenReport<GMP_Reports_AccountsDataDump_Page>("Accounts\\+Data\\+Dump", timeout);
        }

        public GMP_Reports_Attrition_Page ClickAttrition(int? timeout = null)
        {
            return OpenReport<GMP_Reports_Attrition_Page>("Attrition", timeout);
        }

        public GMP_Reports_AttritionChart_Page ClickAttritionChart(int? timeout = null)
        {
            return OpenReport<GMP_Reports_AttritionChart_Page>("Attrition\\+Chart", timeout);
        }

        public GMP_Reports_Enrollments_Page ClickEnrollments(int? timeout = null)
        {
            return OpenReport<GMP_Reports_Enrollments_Page>("Enrollments", timeout);
        }

        public GMP_Reports_InvalidTaxNumbers_Page ClickInvalidTaxNumbers(int? timeout = null)
        {
            return OpenReport<GMP_Reports_InvalidTaxNumbers_Page>("Invalid\\+Tax+Numbers", timeout);
        }

        public GMP_Reports_MissingAdddresses_Page ClickMissingAddresses(int? timeout = null)
        {
            return OpenReport<GMP_Reports_MissingAdddresses_Page>("Missing\\+Addresses", timeout);
        }

        public GMP_Reports_PersonalWebsites_Page ClickPersonalWebsites(int? timeout = null)
        {
            return OpenReport<GMP_Reports_PersonalWebsites_Page>("Personal\\+Website\\+URLs", timeout);
        }
    }
}
