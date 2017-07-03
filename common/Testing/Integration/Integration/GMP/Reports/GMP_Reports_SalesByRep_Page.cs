using WatiN.Core;
using System;

namespace NetSteps.Testing.Integration.GMP.Reports
{
    public class GMP_Reports_SalesByRep_Page : GMP_Reports_Viewer_Page
    {
        TextField _startDate;
        TextField _endDate;
        GMP_Reports_TextBox_Control _accountNumber;
        TextField _excludedAccounts;
        private TableRow _parameters;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _startDate = Document.GetElement<TextField>(new Param("rptViewer_ctl04_ctl03_txtValue"));
            _endDate = Document.GetElement<TextField>(new Param("rptViewer_ctl04_ctl05_txtValue"));
            _accountNumber = Control.CreateControl<GMP_Reports_TextBox_Control>(Document.GetElement<Div>(new Param("rptViewer_ctl04_ctl11")));
            _excludedAccounts = Document.GetElement<TextField>(new Param("rptViewer_ctl04_ctl13_txtValue"));
            _parameters = Document.GetElement<TableRow>(new Param("ParametersRowrptViewer"));
        }

        public override bool IsPageRendered()
        {
            return _parameters.Exists;
        }

        public void EnterStartDate(DateTime startDate)
        {
            _startDate.CustomSetTextQuicklyHelper(startDate.ToShortDateString());
        }

        public void EnterEndDate(DateTime endDate)
        {
            _endDate.CustomSetTextQuicklyHelper(endDate.ToShortDateString());
        }

        public void SelectOrderStatus(int index)
        {
            SelectCheckBox("rptViewer_ctl04_ctl07_ddDropDownButton", "rptViewer_ctl04_ctl07_divDropDown", index);
        }

        public void SelectTitles(int index)
        {
            SelectCheckBox("rptViewer_ctl04_ctl09_ddDropDownButton", "rptViewer_ctl04_ctl09_divDropDown", index);
        }

        public void EnterAccountNumber(string accountNumber)
        {
            _accountNumber.Value = accountNumber;
        }

        public void EnterExcludedAccounts(string excludedAccounts)
        {
            _excludedAccounts.CustomSetTextQuicklyHelper(excludedAccounts);
        }
    }
}
