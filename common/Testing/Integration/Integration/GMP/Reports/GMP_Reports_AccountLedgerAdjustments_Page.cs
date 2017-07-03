using WatiN.Core;
using System;

namespace NetSteps.Testing.Integration.GMP.Reports
{
    public class GMP_Reports_AccountLedgerAdjustments_Page : GMP_Reports_Viewer_Page
    {
        private TableRow _parameters;
        private TextField _startDate;
        private TextField _endDate;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _parameters = Document.GetElement<TableRow>(new Param("ParametersRowrptViewer"));
            _startDate = Document.GetElement<TextField>(new Param("rptViewer_ctl04_ctl03_txtValue"));
            _endDate = Document.GetElement<TextField>(new Param("rptViewer_ctl04_ctl05_txtValue"));
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

        public void SelectEntryReasons(int index)
        {
            SelectCheckBox("rptViewer_ctl04_ctl07_ddDropDownButton", "rptViewer_ctl04_ctl07_divDropDown", index);
        }

        public void SelectEntryOrigins(int index)
        {
            SelectCheckBox("rptViewer_ctl04_ctl09_ddDropDownButton", "rptViewer_ctl04_ctl09_divDropDown", index);
        }

        public void SelectEntryTypes(int index)
        {
            SelectCheckBox("rptViewer_ctl04_ctl11_ddDropDownButton", "rptViewer_ctl04_ctl11_divDropDown", index);
        }
    }
}
