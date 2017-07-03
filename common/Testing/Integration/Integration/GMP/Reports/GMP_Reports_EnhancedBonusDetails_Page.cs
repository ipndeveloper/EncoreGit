using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Reports
{
    public class GMP_Reports_EnhancedBonusDetails_Page : GMP_Reports_Viewer_Page
    {
        private bool _multiplePlans;
        private SelectList _plan;
        private SelectList _period;
        private GMP_Reports_TextBox_Control _accounts;
        private GMP_Reports_TextBox_Control _order;
        private GMP_Reports_TextBox_Control _downline;
        private TableRow _parameters;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _multiplePlans = Document.GetElement<TextField>(new Param("rptViewer_ctl04_ctl13_txtValue")).Exists;
            if (_multiplePlans)
            {
                _plan = Document.GetElement<SelectList>(new Param("rptViewer_ctl04_ctl03_ddValue"));
                _period = Document.GetElement<SelectList>(new Param("rptViewer_ctl04_ctl05_ddValue"));
                _accounts = Control.CreateControl<GMP_Reports_TextBox_Control>(Document.GetElement<Div>(new Param("rptViewer_ctl04_ctl09")));
                _order = Control.CreateControl<GMP_Reports_TextBox_Control>(Document.GetElement<Div>(new Param("rptViewer_ctl04_ctl11")));
                _downline = Control.CreateControl<GMP_Reports_TextBox_Control>(Document.GetElement<Div>(new Param("rptViewer_ctl04_ctl13")));
            }
            else
            {
                _period = Document.SelectList("rptViewer_ctl04_ctl03_ddValue");
                _accounts = Control.CreateControl<GMP_Reports_TextBox_Control>(Document.GetElement<Div>(new Param("rptViewer_ctl04_ctl07")));
                _order = Control.CreateControl<GMP_Reports_TextBox_Control>(Document.GetElement<Div>(new Param("rptViewer_ctl04_ctl9")));
                _downline = Control.CreateControl<GMP_Reports_TextBox_Control>(Document.GetElement<Div>(new Param("rptViewer_ctl04_ctl11")));
            }
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

        public void SelectBonusType(int index)
        {
            if (_multiplePlans)
            {
                SelectCheckBox("rptViewer_ctl04_ctl07_ddDropDownButton", "rptViewer_ctl04_ctl07_divDropDown", index);
            }
            else
            {
                SelectCheckBox("rptViewer_ctl04_ctl05_ddDropDownButton", "rptViewer_ctl04_ctl05_divDropDown", index);
            }
        }

        public void EnterAccounts(string accounts)
        {
            _accounts.Value = accounts;
        }

        public void EnterOrder(string order)
        {
            _order.Value = order;
        }

        public void EnterDownLine(string downline)
        {
            _downline.Value = downline;
        }
    }
}
