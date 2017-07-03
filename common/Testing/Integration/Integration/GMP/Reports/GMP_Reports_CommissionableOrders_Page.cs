using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Reports
{
    public class GMP_Reports_CommissionableOrders_Page : GMP_Reports_Viewer_Page
    {
        private TableRow _parameters;
        private SelectList _plan;
        private SelectList _period;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _parameters = Document.GetElement<TableRow>(new Param("ParametersRowrptViewer"));
            _period = Document.GetElement<SelectList>(new Param("rptViewer_ctl04_ctl05_ddValue"));
            if (_period.Exists)
            {
                _plan = Document.GetElement<SelectList>(new Param("rptViewer_ctl04_ctl03_ddValue"));
            }
            else
            {
                _period = Document.SelectList("rptViewer_ctl04_ctl03_ddValue");
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
    }
}
