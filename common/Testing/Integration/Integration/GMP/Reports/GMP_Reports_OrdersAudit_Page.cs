using WatiN.Core;
using System;

namespace NetSteps.Testing.Integration.GMP.Reports
{
    public class GMP_Reports_OrdersAudit_Page : GMP_Reports_Viewer_Page
    {
        private TextField _txtOrders;
        private GMP_Reports_TextBox_Control _startDate;
        private GMP_Reports_TextBox_Control _endDate;
        private TableRow _parameters;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _txtOrders = Document.GetElement<TextField>(new Param("rptViewer_ctl04_ctl03_txtValue"));
            _startDate = Control.CreateControl < GMP_Reports_TextBox_Control >(Document.GetElement<Div>(new Param("rptViewer_ctl04_ctl05")));
            _endDate = Control.CreateControl<GMP_Reports_TextBox_Control>(Document.GetElement<Div>(new Param("rptViewer_ctl04_ctl07")));
            _parameters = Document.GetElement<TableRow>(new Param("ParametersRowrptViewer"));
        }

        public override bool IsPageRendered()
        {
            return _parameters.Exists;
        }

        private bool AuditReport()
        {
            try
            {
                return Document.Tables[35].Exists;
            }
            catch (Exception)
            {

                return false;
            }
            
        }

        public void EnterOrderNumbers(string orderNumbers)
        {
            _txtOrders.CustomSetTextQuicklyHelper(orderNumbers);
        }

        public void EnterDate(string from, string to)
        {
            // Set from date
            if (string.IsNullOrEmpty(from))
                _startDate.Value = null;
            else
            {
                _startDate.Value = from;
            }

            // Set to date
            if (string.IsNullOrEmpty(to))
                _endDate.Value = null;
            else
            {
                _endDate.Value = to;
            }
        }

        public void EnterDate(DateTime from, DateTime to)
        {
            EnterDate(from.ToShortDateString(), to.ToShortDateString());
        }
    }
}
