using WatiN.Core;
using System;

namespace NetSteps.Testing.Integration.GMP.Reports
{
    public class GMP_Reports_Promotions_Page : GMP_Reports_Viewer_Page
    {
        private TextField _txtFromCompleteSalesDate;
        private TextField _txtToCompleteSalesDate;
        private TableRow _parameters;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _txtFromCompleteSalesDate = Document.GetElement<TextField>(new Param("rptViewer_ctl04_ctl03_txtValue"));
            _txtToCompleteSalesDate = Document.GetElement<TextField>(new Param("rptViewer_ctl04_ctl05_txtValue"));
            _parameters = Document.GetElement<TableRow>(new Param("ParametersRowrptViewer"));
        }

        public override bool IsPageRendered()
        {
            return _parameters.Exists;
        }

        public void EnterDate(DateTime start, DateTime end)
        {
            _txtFromCompleteSalesDate.CustomSetTextQuicklyHelper(start.ToShortDateString());
            _txtToCompleteSalesDate.CustomSetTextQuicklyHelper(end.ToShortDateString());
        }
    }
}
