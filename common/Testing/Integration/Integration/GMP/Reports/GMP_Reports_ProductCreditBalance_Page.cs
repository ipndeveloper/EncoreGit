using WatiN.Core;
using System;

namespace NetSteps.Testing.Integration.GMP.Reports
{
    public class GMP_Reports_ProductCreditBalance_Page : GMP_Reports_Viewer_Page
    {
        private GMP_Reports_TextBox_Control _account;
        private TextField _startDate;
        private TextField _endData;
        private TableRow _parameters;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _account = Control.CreateControl<GMP_Reports_TextBox_Control>(Document.GetElement<Div>(new Param("rptViewer_ctl04_ctl03")));
            _startDate = Document.GetElement<TextField>(new Param("rptViewer_ctl04_ctl05_txtValue"));
            _endData = Document.GetElement<TextField>(new Param("rptViewer_ctl04_ctl07_txtValue"));
            _parameters = Document.GetElement<TableRow>(new Param("ParametersRowrptViewer"));
        }

        public override bool IsPageRendered()
        {
            return _parameters.Exists;
        }

        public void EnterAccountNumber(string account)
        {
            _account.Value = account;
        }

        public void EnterDate(DateTime start, DateTime end)
        {
            _startDate.CustomSetTextQuicklyHelper(start.ToShortDateString());

            _endData.CustomSetTextQuicklyHelper(end.ToShortDateString());
        }
    }
}
