﻿using WatiN.Core;
using System;

namespace NetSteps.Testing.Integration.GMP.Reports
{
    public class GMP_Reports_Sales_Tax_Details_Page : GMP_Reports_Viewer_Page
    {
        private TextField _startDate;
        private TextField _endDate;
        private SelectList _country;
        private SelectList _currency;
        private TableRow _parameters;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _startDate = Document.GetElement<TextField>(new Param("rptViewer_ctl04_ctl03_txtValue"));
            _endDate = Document.GetElement<TextField>(new Param("rptViewer_ctl04_ctl05_txtValue"));
            _country = Document.GetElement<SelectList>(new Param("rptViewer_ctl04_ctl07_ddValue"));
            _currency = Document.GetElement<SelectList>(new Param("rptViewer_ctl04_ctl15_ddValue"));
            _parameters = Document.GetElement<TableRow>(new Param("ParametersRowrptViewer"));
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

        public void SelectCountry(int index)
        {
            _country.CustomSelectDropdownItem(index);
            WaitForReport();
        }

        public void SelectLocale(int index)
        {
            SelectCheckBox("rptViewer_ctl04_ctl09_ddDropDownButton", "rptViewer_ctl04_ctl09_divDropDown", index);
        }

        public void SelectOrderTypes(int index)
        {
            SelectCheckBox("rptViewer_ctl04_ctl11_ddDropDownButton", "rptViewer_ctl04_ctl11_divDropDown", index);
        }

        public void SelectOrderStatuses(int index)
        {
            SelectCheckBox("rptViewer_ctl04_ctl13_ddDropDownButton", "rptViewer_ctl04_ctl13_divDropDown", index);
        }

        public void SelectCurrency(int index)
        {
            _currency.CustomSelectDropdownItem(index);
        }

    }
}
