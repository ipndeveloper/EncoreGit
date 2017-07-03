using WatiN.Core;
using System;

namespace NetSteps.Testing.Integration.GMP.Reports
{
    public class GMP_Reports_AccountsDataDump_Page : GMP_Reports_Viewer_Page
    {
        private TableRow _parameters;
        private TextField _txtFromEnrollmentDate;
        private TextField _txtToEnrollmentDate;
        private SelectList _title;
        private Table _table;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _parameters = Document.GetElement<TableRow>(new Param("ParametersRowrptViewer"));
            _txtFromEnrollmentDate = Document.GetElement<TextField>(new Param("rptViewer_ctl04_ctl03_txtValue"));
            _txtToEnrollmentDate = Document.GetElement<TextField>(new Param("rptViewer_ctl04_ctl05_txtValue"));
            _title = Document.GetElement<SelectList>(new Param("rptViewer_ctl04_ctl13_ddValue"));
            _table = Document.GetElement<Div>(new Param("VisibleReportContentrptViewer_ctl10")).GetElement<Div>().GetElement<Table>();
        }

        public override bool IsPageRendered()
        {
            return _parameters.Exists;
        }

        public void EnterDate(DateTime from, DateTime to)
        {
            // Set from date.
            _txtFromEnrollmentDate.CustomSetTextQuicklyHelper(from.ToShortDateString());
            _txtFromEnrollmentDate.CustomRunScript(Util.strKeyUp);
            WaitForReport();

            // Set to date.
            _txtToEnrollmentDate.CustomSetTextQuicklyHelper(to.ToShortDateString());
            _txtToEnrollmentDate.CustomRunScript(Util.strKeyUp);
            WaitForReport();
        }

        public void SelectAccountType(int index)
        {
            SelectCheckBox("rptViewer_ctl04_ctl07_ddDropDownButton", "rptViewer_ctl04_ctl07_divDropDown", index);
        }

        public void SelectAccountStatus(int index)
        {
            SelectCheckBox("rptViewer_ctl04_ctl09_ddDropDownButton", "rptViewer_ctl04_ctl09_divDropDown", index);
        }

        public void SelectCountry(int index)
        {
            SelectCheckBox("rptViewer_ctl04_ctl11_ddDropDownButton", "rptViewer_ctl04_ctl11_divDropDown", index);
            WaitForReport();
        }

        public void SelectTitle(int index)
        {
            _title.CustomSelectDropdownItem(index);
        }

        public bool IsValid()
        {
            bool valid;
            if
                (
                _txtFromEnrollmentDate.Exists
                && _txtToEnrollmentDate.Exists
                && _title.Exists
                )
                valid = true;
            else
                valid = false;
            return valid;
        }
    }
}
