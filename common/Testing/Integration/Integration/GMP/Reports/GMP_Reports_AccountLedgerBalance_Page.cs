using WatiN.Core;
using System;

namespace NetSteps.Testing.Integration.GMP.Reports
{
    public class GMP_Reports_AccountLedgerBalance_Page : GMP_Reports_Viewer_Page
    {
        private TableRow _parameters;
        private GMP_Reports_TextBox_Control _accountNumber;
        private TextField _startDate;
        private TextField _endDate;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _parameters = Document.GetElement<TableRow>(new Param("ParametersRowrptViewer"));
            _accountNumber = Control.CreateControl<GMP_Reports_TextBox_Control>(Document.GetElement<Div>(new Param("rptViewer_ctl04_ctl03")));
            _startDate = Document.GetElement<TextField>(new Param("rptViewer_ctl04_ctl05_txtValue"));
            _endDate = Document.GetElement<TextField>(new Param("rptViewer_ctl04_ctl07_txtValue"));
        }

        public override bool IsPageRendered()
        {
            return _parameters.Exists;
        }

        public void EnterAccountNumber(string accountNumber)
        {
            _accountNumber.Value = accountNumber;
        }

        public void EnterDate(DateTime start, DateTime end)
        {
            _startDate.CustomSetTextQuicklyHelper(start.ToShortDateString());
            _endDate.CustomSetTextQuicklyHelper(end.ToShortDateString());
        }
    }
}
