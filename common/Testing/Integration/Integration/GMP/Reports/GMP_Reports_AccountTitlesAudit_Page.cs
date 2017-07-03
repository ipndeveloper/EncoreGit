using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Reports
{
    public class GMP_Reports_AccountTitlesAudit_Page : GMP_Reports_Viewer_Page
    {
        private TableRow _parameters;
        private SelectList _plan;
        private SelectList _period;
        private SelectList _title;
        private Table _table;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _parameters = Document.GetElement<TableRow>(new Param("ParametersRowrptViewer"));
            _title = Document.GetElement<SelectList>(new Param("rptViewer_ctl04_ctl07_ddValue"));
            if (_title.Exists)
            {
                _plan = Document.GetElement<SelectList>(new Param("rptViewer_ctl04_ctl03_ddValue"));
                _period = Document.GetElement<SelectList>(new Param("rptViewer_ctl04_ctl05_ddValue"));
            }
            else
            {
                _period = Document.GetElement<SelectList>(new Param("rptViewer_ctl04_ctl03_ddValue"));
                _title = Document.GetElement<SelectList>(new Param("rptViewer_ctl04_ctl05_ddValue"));
            }
            _table = Document.GetElement<Table>(new Param(41));
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

        public void SelectTitleType(int index)
        {
            _title.CustomSelectDropdownItem(index);
        }
    }
}
