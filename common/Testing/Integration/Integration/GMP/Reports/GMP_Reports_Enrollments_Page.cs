using WatiN.Core;
using System;

namespace NetSteps.Testing.Integration.GMP.Reports
{
    public class GMP_Reports_Enrollments_Page : GMP_Reports_Viewer_Page
    {
        private SelectList _grouping;
        private TextField _txtFromEnrollmentDate;
        private TextField _txtToEnrollmentDate;
        private GMP_Reports_TextBox_Control _enroller;
        private TableRow _parameters;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _grouping = Document.GetElement<SelectList>(new Param("rptViewer_ctl04_ctl03_ddValue"));
            _txtFromEnrollmentDate = Document.GetElement<TextField>(new Param("rptViewer_ctl04_ctl11_txtValue"));
            _txtToEnrollmentDate = Document.GetElement<TextField>(new Param("rptViewer_ctl04_ctl13_txtValue"));
            _enroller = Control.CreateControl<GMP_Reports_TextBox_Control>(Document.GetElement<Div>(new Param("rptViewer_ctl04_ctl17")));
            if (!_enroller.Exists)
                _enroller = Control.CreateControl<GMP_Reports_TextBox_Control>(Document.GetElement<Div>(new Param("rptViewer_ctl04_ctl15")));
            _parameters = Document.GetElement<TableRow>(new Param("ParametersRowrptViewer"));
        }

        public override bool IsPageRendered()
        {
            return _parameters.Exists;
        }

        public void SelectGrouping(int index)
        {
            _grouping.CustomSelectDropdownItem(index);
            WaitForReport();
        }

        public void SelectCountry(int index)
        {
            SelectCheckBox("rptViewer_ctl04_ctl05_ddDropDownButton", "rptViewer_ctl04_ctl05_divDropDown", index);
            WaitForReport();
        }
        
        public void SelectLocale(int index)
        {
            SelectCheckBox("rptViewer_ctl04_ctl07_ddDropDownButton", "rptViewer_ctl04_ctl07_divDropDown", index);
        }

        public void SelectAccountType(int index)
        {
            SelectCheckBox("rptViewer_ctl04_ctl09_ddDropDownButton", "rptViewer_ctl04_ctl09_divDropDown", index);
        }

        public void EnterDate(DateTime from, DateTime to)
        {
            // Set from date.
            _txtFromEnrollmentDate.CustomSetTextQuicklyHelper(from.ToShortDateString());
            WaitForReport();

            // Set to date.
            _txtToEnrollmentDate.CustomSetTextQuicklyHelper(to.ToShortDateString());
        }

        public void SelectStatus(int index)
        {
            SelectCheckBox("rptViewer_ctl04_ctl15_ddDropDownButton", "rptViewer_ctl04_ctl15_divDropDown", index);
        }

        public void EnterEnrollerID(string id)
        {
            _enroller.Value = id;
        }
    }
}
