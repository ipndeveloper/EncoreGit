using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Reports
{
    public class GMP_Reports_TitlePopulations_Page : GMP_Reports_Viewer_Page
    {
        private SelectList _plan;
        private SelectList _period;
        private SelectList _titleType;
        private TableRow _parameters;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _titleType = Document.SelectList("rptViewer_ctl04_ctl07_ddValue");
            if (_titleType.Exists)
            {
                _plan = Document.SelectList("rptViewer_ctl04_ctl03_ddValue");
                _period = Document.SelectList("rptViewer_ctl04_ctl05_ddValue");
            }
            else
            {
                _period = Document.SelectList("rptViewer_ctl04_ctl05_ddValue");
                if (_period.Exists)
                {
                    _plan = Document.SelectList("rptViewer_ctl04_ctl03_ddValue");
                }
                else
                {
                    _period = Document.GetElement<SelectList>(new Param("rptViewer_ctl04_ctl03_ddValue"));
                }
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

        public void SelectTitleType(int index)
        {
            _titleType.CustomSelectDropdownItem(index);
        }
    }
}
