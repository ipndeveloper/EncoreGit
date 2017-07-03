using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Reports
{
    public class GMP_Reports_ActiveLegs_Page : GMP_Reports_Viewer_Page
    {
        private TableRow _parameters;
        private TextField _accountNumber;
        private SelectList _plan;
        private SelectList _period;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _parameters = Document.GetElement<TableRow>(new Param("ParametersRowrptViewer"));
            _accountNumber = Document.GetElement<TextField>(new Param("rptViewer_ctl04_ctl03_txtValue"));
            _plan = Document.GetElement<SelectList>(new Param("rptViewer_ctl04_ctl05_ddValue"));
            _period = Document.GetElement<SelectList>(new Param("rptViewer_ctl04_ctl07_ddValue"));
        }

        public override bool IsPageRendered()
        {
            return _parameters.Exists;
        }

        public void EnterAccountNumber(string accountNumber)
        {
            _accountNumber.CustomSetTextQuicklyHelper(accountNumber);
        }

        public void SelectPlan(int index)
        {
            _plan.CustomSelectDropdownItem(index);
        }

        public void SelectPeriod(int index)
        {
            _period.CustomSelectDropdownItem(index);
        }
    }
}
