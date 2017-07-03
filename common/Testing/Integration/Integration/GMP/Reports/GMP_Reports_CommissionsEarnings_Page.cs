using WatiN.Core;
namespace NetSteps.Testing.Integration.GMP.Reports
{
    public class GMP_Reports_CommissionsEarnings_Page : GMP_Reports_Viewer_Page
    {
        private TableRow _parameters;
        private SelectList _plan;
        private SelectList _period;
        private GMP_Reports_TextBox_Control _accountNumber;
        private SelectList _rank;
        private SelectList _title;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _parameters = Document.GetElement<TableRow>(new Param("ParametersRowrptViewer"));
            _plan = Document.GetElement<SelectList>(new Param("rptViewer_ctl04_ctl03_ddValue"));
            _period = Document.GetElement<SelectList>(new Param("rptViewer_ctl04_ctl05_ddValue"));
            _accountNumber = Control.CreateControl<GMP_Reports_TextBox_Control>(Document.GetElement<Div>(new Param("rptViewer_ctl04_ctl07")));
            _rank = Document.GetElement<SelectList>(new Param("rptViewer_ctl04_ctl09_ddValue"));
            _title = Document.GetElement<SelectList>(new Param("rptViewer_ctl04_ctl11_ddValue"));
        }

        public override bool IsPageRendered()
        {
            return _parameters.Exists;
        }

        public void SelectPlan(int index)
        {
            _plan.CustomSelectDropdownItem(index);
            WaitForReport();
        }

        public void SelectPeriod(int index)
        {
            _period.CustomSelectDropdownItem(index);
        }

        public void EnterAccountNumber(string account)
        {
            _accountNumber.Value = account;
        }

        public void SelectRank(int index)
        {
            _rank.CustomSelectDropdownItem(index);
        }

        public void SelectTitle(int index)
        {
            _title.CustomSelectDropdownItem(index);
        }

        public void SelectCountry(int index)
        {
            SelectCheckBox("rptViewer_ctl04_ctl13_ddDropDownButton", "rptViewer_ctl04_ctl13_divDropDown", index);
        }

        public void SelectBonusType(int index)
        {
            SelectCheckBox("rptViewer_ctl04_ctl15_ddDropDownButton", "rptViewer_ctl04_ctl15_divDropDown", index);
        }
    }
}
