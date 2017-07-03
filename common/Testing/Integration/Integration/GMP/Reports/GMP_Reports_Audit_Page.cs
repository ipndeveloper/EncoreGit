using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Reports
{
    public class GMP_Reports_Audit_Page : GMP_Reports_Base_Page
    {
        private GMP_Reports_AuditReports_Control _reports;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _reports = _coreContent.As<GMP_Reports_AuditReports_Control>();
        }

        public GMP_Reports_AuditReports_Control Reports
        {
            get { return _reports; }
        }

         public override bool IsPageRendered()
        {
            return Title.Equals("Audit Reports");
        }
    }
}
