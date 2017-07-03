using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Reports
{
    public class GMP_Reports_AccountCalculationsAudit_Page : GMP_Reports_Viewer_Page
    {
        private TableRow _parameters;
        private SelectList _period;
        private SelectList _accountType;
        private GMP_Reports_TextBox_Control _accountNumbers;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _parameters = Document.GetElement<TableRow>(new Param("ParametersRowrptViewer"));
            _period = Document.GetElement<SelectList>(new Param("rptViewer_ctl04_ctl03_ddValue"));
            _accountType = Document.GetElement<SelectList>(new Param("rptViewer_ctl04_ctl07_ddValue"));
            _accountNumbers = Control.CreateControl<GMP_Reports_TextBox_Control>(Document.GetElement<Div>(new Param("rptViewer_ctl04_ctl09")));
        }

        public override bool IsPageRendered()
        {
            return _parameters.Exists;
        }

        public void SelectPeriod(int index, int? timeout = null, int? delay = 1)
        {
            _period.CustomSelectDropdownItem(index, timeout);
            this.WaitForReport(timeout);
        }

        public void SelectCalculationTypes(int index, int? timeout = null)
        {
            SelectCheckBox( "rptViewer_ctl04_ctl05_ddDropDownButton", "rptViewer_ctl04_ctl05_divDropDown", index, timeout);
        }

        public void SelectAccountTypes(int index, int? timeout = null, int? delay = 1)
        {
            _accountType.CustomSelectDropdownItem(index, timeout);
        }

        public void EnterAccountNumbers(string accounts)
        {
            _accountNumbers.Value = accounts;
        }
    }
}
