using WatiN.Core;
using System;

namespace NetSteps.Testing.Integration.GMP.Reports
{
    public class GMP_Reports_SalesByState_Page : GMP_Reports_Viewer_Page
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

        public void EnterDate(DateTime from, DateTime to)
        {
            // Set from date.
            _txtFromCompleteSalesDate.CustomSetTextQuicklyHelper(from.ToShortDateString());

            // Set to date.
            _txtToCompleteSalesDate.CustomSetTextQuicklyHelper(to.ToShortDateString());
        }

        public void SelectOrderType(int index)
        {
            SelectCheckBox("rptViewer_ctl04_ctl07_ddDropDownButton", "rptViewer_ctl04_ctl07_divDropDown", index);
        }

        public void SelectCountry(int index)
        {
            SelectCheckBox("rptViewer_ctl04_ctl09_ddDropDownButton", "rptViewer_ctl04_ctl09_divDropDown", index);
            WaitForReport();
        }

        public void SelectState(int index)
        {
            SelectCheckBox("rptViewer_ctl04_ctl11_ddDropDownButton", "rptViewer_ctl04_ctl11_divDropDown", index);
        }
    }
}
