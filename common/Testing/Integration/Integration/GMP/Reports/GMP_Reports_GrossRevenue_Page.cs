using WatiN.Core;
using System;

namespace NetSteps.Testing.Integration.GMP.Reports
{
    public class GMP_Reports_GrossRevenue_Page : GMP_Reports_Viewer_Page
    {
        private TextField _startDate;
        private TextField _endDate;
        private TableRow _parameters;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _startDate = Document.GetElement<TextField>(new Param("rptViewer_ctl04_ctl03_txtValue"));
            _endDate = Document.GetElement<TextField>(new Param("rptViewer_ctl04_ctl05_txtValue"));
            _parameters = Document.GetElement<TableRow>(new Param("ParametersRowrptViewer"));
        }

        public override bool IsPageRendered()
        {
            return _parameters.Exists;
        }

        public void EnterDate(DateTime start, DateTime end)
        {
            _startDate.CustomSetTextQuicklyHelper(start.ToShortDateString());

            _endDate.CustomSetTextQuicklyHelper(end.ToShortDateString());
        }

        public void SelectOrderTypes(int index)
        {
            SelectCheckBox("rptViewer_ctl04_ctl07_ddDropDownButton", "rptViewer_ctl04_ctl07_divDropDown", index);
        }
    }
}
