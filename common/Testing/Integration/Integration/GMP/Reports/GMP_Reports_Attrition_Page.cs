using WatiN.Core;
using System;

namespace NetSteps.Testing.Integration.GMP.Reports
{
    public class GMP_Reports_Attrition_Page : GMP_Reports_Viewer_Page
    {
        private TextField _txtFromEnrollmentDate;
        private TextField _txtToEnrollmentDate;
        private TableRow _parameters;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _txtFromEnrollmentDate = Document.GetElement<TextField>(new Param("rptViewer_ctl04_ctl05_txtValue"));
            _txtToEnrollmentDate = Document.GetElement<TextField>(new Param("rptViewer_ctl04_ctl07_txtValue"));
            _parameters = Document.GetElement<TableRow>(new Param("ParametersRowrptViewer"));
        }

        public override bool IsPageRendered()
        {
            return _parameters.Exists;
        }

        public void SelectAccountType(int index)
        {
            SelectCheckBox("rptViewer_ctl04_ctl03_ddDropDownButton", "rptViewer_ctl04_ctl03_divDropDown", index);
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
