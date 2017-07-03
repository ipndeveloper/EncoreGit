using WatiN.Core;
using System;

namespace NetSteps.Testing.Integration.GMP.Reports
{
    public class GMP_Reports_OrdersByCreditCardNumber_Page : GMP_Reports_Viewer_Page
    {
        private TextField _date;
        private SelectList _searchBy;
        private TextField _digits;
        private TableRow _parameters;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _date = Document.GetElement<TextField>(new Param("rptViewer_ctl04_ctl03_txtValue"));
            _searchBy = Document.GetElement<SelectList>(new Param("rptViewer_ctl04_ctl05_ddValue"));
            _digits = Document.GetElement<TextField>(new Param("rptViewer_ctl04_ctl07_txtValue"));
            _parameters = Document.GetElement<TableRow>(new Param("ParametersRowrptViewer"));
        }

        public override bool IsPageRendered()
        {
            return _parameters.Exists;
        }

        public void EnterDate(DateTime date)
        {
            _date.CustomSetTextQuicklyHelper(date.ToString());
        }

        public void SelectSearchBy(int index)
        {
            _searchBy.CustomSelectDropdownItem(index);
        }

        public void EnterDigits(string digits)
        {
            _digits.CustomSetTextQuicklyHelper(digits);
        }
    }
}
