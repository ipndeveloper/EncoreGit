using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Reports
{
    public class GMP_Reports_PersonalEarningsStatement_Page : GMP_Reports_Viewer_Page
    {
        private SelectList _plan;
        private SelectList _period;
        private TextField _accountNumber;
        private TableRow _parameters;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _plan = Document.GetElement<SelectList>(new Param("rptViewer_ctl04_ctl03_ddValue"));
            _period = Document.GetElement<SelectList>(new Param("rptViewer_ctl04_ctl05_ddValue"));
            _accountNumber = Document.GetElement<TextField>(new Param("rptViewer_ctl04_ctl07_txtValue"));
            _parameters = Document.GetElement<TableRow>(new Param("ParametersRowrptViewer"));
        }

        public override bool IsPageRendered()
        {
            return _parameters.Exists;
        }

        public void SelectPlan(int index)
        {
            _plan.CustomSelectDropdownItem(index);
        }

        public void SelectPeriod(int index)
        {
            _period.CustomSelectDropdownItem(index);
        }

        public void EnterAccountNumber(string account)
        {
            _accountNumber.CustomSetTextQuicklyHelper(account);
        }
    }
}
