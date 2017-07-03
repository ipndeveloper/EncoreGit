using WatiN.Core;
using System;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP.Reports
{
    public class GMP_Reports_AccountRecordAudit_Page : GMP_Reports_Viewer_Page
    {
        private TableRow _parameters;
        private TextField _accountNumbers;
        private GMP_Reports_TextBox_Control _startDate;
        private GMP_Reports_TextBox_Control _endDate;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _parameters = Document.GetElement<TableRow>(new Param("ParametersRowrptViewer"));
            _accountNumbers = Document.GetElement<TextField>(new Param("rptViewer_ctl04_ctl03_txtValue"));
            _startDate = Control.CreateControl<GMP_Reports_TextBox_Control>(Document.GetElement<Div>(new Param("rptViewer_ctl04_ctl05")));
            _endDate = Control.CreateControl<GMP_Reports_TextBox_Control>(Document.GetElement<Div>(new Param("rptViewer_ctl04_ctl07")));
        }

        public override bool IsPageRendered()
        {
            return _parameters.Exists;
        }

        public void EnterAccountNumbers(string accounts)
        {
            _accountNumbers.Value = accounts;
        }

        public void EnterDates(DateTime startDate, DateTime endDate)
        {
            EnterStartDate(startDate);
            EnterEndDate(endDate);
        }
        
        public void EnterStartDate(DateTime startDate)
        {
            _startDate.Value = startDate.ToShortDateString();
        }

        public void EnterEndDate(DateTime endDate)
        {
            _endDate.Value = endDate.ToShortDateString();
        }
    }
}
