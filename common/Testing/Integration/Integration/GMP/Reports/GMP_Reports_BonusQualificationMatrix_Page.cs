using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Reports
{
    public class GMP_Reports_BonusQualificationMatrix_Page : GMP_Reports_Viewer_Page
    {
        private TableRow _parameters;
        private SelectList _plan;
        private SelectList _period;
        private SelectList _bonusType;
        private TextField _accountNumbers;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _parameters = Document.GetElement<TableRow>(new Param("ParametersRowrptViewer"));
            _accountNumbers = Document.GetElement<TextField>(new Param("rptViewer_ctl04_ctl09_txtValue"));
            if (_accountNumbers.Exists)
            {
                _plan = Document.GetElement<SelectList>(new Param("rptViewer_ctl04_ctl03_ddValue"));
                _period = Document.GetElement<SelectList>(new Param("rptViewer_ctl04_ctl05_ddValue"));
                _bonusType = Document.GetElement<SelectList>(new Param("rptViewer_ctl04_ctl07_ddValue"));
            }
            else
            {
                _period = Document.GetElement<SelectList>(new Param("rptViewer_ctl04_ctl03_ddValue"));
                _bonusType = Document.GetElement<SelectList>(new Param("rptViewer_ctl04_ctl05_ddValue"));
                _accountNumbers = Document.GetElement<TextField>(new Param("rptViewer_ctl04_ctl07_txtValue"));
            }
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

        public void SelectBonusType(int index)
        {
            _bonusType.CustomSelectDropdownItem(index);
        }

        public void EnterAccountNumbers(string accounts)
        {
            _accountNumbers.CustomSetTextQuicklyHelper(accounts);
        }
    }
}
