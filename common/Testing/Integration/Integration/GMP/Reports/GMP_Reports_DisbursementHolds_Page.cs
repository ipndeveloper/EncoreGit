using WatiN.Core;
using System;

namespace NetSteps.Testing.Integration.GMP.Reports
{
    public class GMP_Reports_DisbursementHolds_Page : GMP_Reports_Viewer_Page
    {
        private GMP_Reports_TextBox_Control _account;
        private GMP_Reports_TextBox_Control _date;
        private TableRow _parameters;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _account = Control.CreateControl<GMP_Reports_TextBox_Control>(Document.GetElement<Div>(new Param("rptViewer_ctl04_ctl03")));
            _date = Control.CreateControl<GMP_Reports_TextBox_Control>(Document.GetElement<Div>(new Param("rptViewer_ctl04_ctl05")));
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

        public void EnterEffectiveDate(DateTime date)
        {
            if (date == null)
            {
                _date.Value = null;
            }
            else
            {
                DateTime dt = date;
                _date.Value = dt.ToShortDateString();
            }

        }
    }
}
