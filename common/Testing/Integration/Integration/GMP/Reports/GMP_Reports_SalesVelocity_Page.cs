using WatiN.Core;
using System;
using System.Threading;

namespace NetSteps.Testing.Integration.GMP.Reports
{
    public class GMP_Reports_SalesVelocity_Page : GMP_Reports_Viewer_Page
    {
        private SelectList _monthFrom;
        private SelectList _monthTo;
        private SelectList _displayColumns;
        private SelectList _displaySales;
        private TableRow _parameters;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _monthFrom = Document.GetElement<SelectList>(new Param("rptViewer_ctl04_ctl07_ddValue"));
            _monthTo = Document.GetElement<SelectList>(new Param("rptViewer_ctl04_ctl09_ddValue"));
            _displayColumns = Document.GetElement<SelectList>(new Param("rptViewer_ctl04_ctl11_ddValue"));
            _displaySales = Document.GetElement<SelectList>(new Param("rptViewer_ctl04_ctl13_ddValue"));
            _parameters = Document.GetElement<TableRow>(new Param("ParametersRowrptViewer"));
        }

        public override bool IsPageRendered()
        {
            return _parameters.Exists;
        }

        public void SelectOrderType(int index)
        {
            SelectCheckBox("rptViewer_ctl04_ctl03_ddDropDownButton", "rptViewer_ctl04_ctl03_divDropDown", index);
        }

        public void SelectOrderStatus(int index)
        {
            SelectCheckBox("rptViewer_ctl04_ctl05_ddDropDownButton", "rptViewer_ctl04_ctl05_divDropDown", index);
        }

        public void SelectMonthFrom(int index)
        {
            _monthFrom.CustomSelectDropdownItem(index);
            WaitForReport();
        }

        public void SelectMonthTo(int index)
        {
            _monthTo.CustomSelectDropdownItem(index);
        }

        public void SelectDisplayColumns(int index)
        {
            _displayColumns.CustomSelectDropdownItem(index);
        }

        public void SelectDisplaySales(int index)
        {
            _displaySales.CustomSelectDropdownItem(index);
        }
    }
}
