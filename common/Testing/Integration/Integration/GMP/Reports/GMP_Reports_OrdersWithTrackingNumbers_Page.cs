using WatiN.Core;
using System;

namespace NetSteps.Testing.Integration.GMP.Reports
{
    public class GMP_Reports_OrdersWithTrackingNumbers_Page : GMP_Reports_Viewer_Page
    {
        private TextField _startDate;
        private TextField _endDate;
        private GMP_Reports_TextBox_Control _orderNumber;
        private TableRow _parameters;

        protected override void  InitializeContents()
        {
 	         base.InitializeContents();
             _startDate = Document.GetElement<TextField>(new Param("rptViewer_ctl04_ctl03_txtValue"));
             _endDate = Document.GetElement<TextField>(new Param("rptViewer_ctl04_ctl05_txtValue"));
             _orderNumber = Control.CreateControl<GMP_Reports_TextBox_Control>(Document.GetElement<Div>(new Param("rptViewer_ctl04_ctl07")));
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

        public void EnterOrderNumber(string orderumber)
        {
            _orderNumber.Value = orderumber;
        }
    }
}
