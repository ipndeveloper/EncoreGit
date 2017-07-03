using WatiN.Core;
using System;

namespace NetSteps.Testing.Integration.GMP.Reports
{
    public class GMP_Reports_PriceList_Page : GMP_Reports_Viewer_Page
    {
        private SelectList _language;
        private TextField _date;
        private TableRow _parameters;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _language = Document.GetElement<SelectList>(new Param("rptViewer_ctl04_ctl03_ddValue"));
            _date = Document.GetElement<TextField>(new Param("rptViewer_ctl04_ctl07_txtValue"));
            _parameters = Document.GetElement<TableRow>(new Param("ParametersRowrptViewer"));
        }

        public override bool IsPageRendered()
        {
            return _parameters.Exists;
        }

        public void SelectLanguage(int index)
        {
            _language.CustomSelectDropdownItem(index);
        }

        public void SelectPriceTypes(int index)
        {
            SelectCheckBox("rptViewer_ctl04_ctl05_ddDropDownButton", "rptViewer_ctl04_ctl05_divDropDown", index);
        }

        public void EnterDate(DateTime date)
        {
            _date.CustomSetTextQuicklyHelper(date.ToString());
        }
    }
}
