using WatiN.Core;
using System;

namespace NetSteps.Testing.Integration.GMP.Reports
{
    public class GMP_Reports_AttritionChart_Page : GMP_Reports_Viewer_Page
    {
        private TableRow _parameters;
        private TextField _txtFromEnrollmentDate;
        private TextField _txtToEnrollmentDate;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _parameters = Document.GetElement<TableRow>(new Param("ParametersRowrptViewer"));
            _txtFromEnrollmentDate = Document.GetElement<TextField>(new Param("rptViewer_ctl04_ctl05_txtValue"));
            _txtToEnrollmentDate = Document.GetElement<TextField>(new Param("rptViewer_ctl04_ctl07_txtValue"));
        }

        public override bool IsPageRendered()
        {
            return _parameters.Exists;
        }

        public void SelectAccountType(int index)
        {
            SelectCheckBox("rptViewer_ctl04_ctl03_ddDropDownButton", "rptViewer_ctl04_ctl03_divDropDown", index);
            WaitForReport();
        }

        public void EnterDate(DateTime from, DateTime to)
        {
            // Set from date.
            _txtFromEnrollmentDate.CustomSetTextQuicklyHelper(from.ToShortDateString());
            WaitForReport();

            // Set to date.
            _txtToEnrollmentDate.CustomSetTextQuicklyHelper(to.ToShortDateString());
        }
    }
}
