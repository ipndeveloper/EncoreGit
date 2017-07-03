using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP.Reports
{
    public class GMP_Reports_Accounts_Page : GMP_Reports_Base_Page
    {
        private GMP_Reports_AccountReports_Control _reports;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _reports = _coreContent.As<GMP_Reports_AccountReports_Control>();
        }

        public GMP_Reports_AccountReports_Control Reports
        {
            get { return _reports; }
        }

         public override bool IsPageRendered()
        {
            return _secNav.GetElement<Link>(new Param("selected", AttributeName.ID.ClassName).And(new Param("/Reports/Reports/Category/1", AttributeName.ID.Href, RegexOptions.None))).Exists;
        }
    }
}
