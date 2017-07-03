using System;
using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Reports
{
    public class GMP_Reports_ExecutiveDashboard_Page : GMP_Reports_Viewer_Page
    {
        private TextField _startDate, _endDate;
        private RadioButton _parties, _noParties;
        private TableRow _parameters;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _startDate = Document.GetElement<TextField>(new Param("rptViewer_ctl04_ctl03_txtValue"));
            _endDate = Document.GetElement<TextField>(new Param("rptViewer_ctl04_ctl05_txtValue"));
            _parties = Document.GetElement<RadioButton>(new Param("rptViewer_ctl04_ctl13_rbTrue"));
            _noParties = Document.GetElement<RadioButton>(new Param("rptViewer_ctl04_ctl13_rbFalse"));
            _parameters = Document.GetElement<TableRow>(new Param("ParametersRowrptViewer"));
        }

        public override bool IsPageRendered()
        {
            return _parameters.Exists;
        }

        public void EnterDate(DateTime start, DateTime end)
        {
            _startDate.CustomSetTextQuicklyHelper(start.ToShortDateString());
            WaitForReport();
            _endDate.CustomSetTextQuicklyHelper(end.ToShortDateString());
        }

        public void SelectOrderType(int index)
        {
            SelectCheckBox("rptViewer_ctl04_ctl07_ddDropDownButton", "rptViewer_ctl04_ctl07_divDropDown", index);
        }

        public void SelectOrderStatus(int index)
        {
            SelectCheckBox("rptViewer_ctl04_ctl09_ddDropDownButton", "rptViewer_ctl04_ctl09_divDropDown", index);
        }

        public void SelectAccountType(int index)
        {
            SelectCheckBox("rptViewer_ctl04_ctl11_ddDropDownButton", "rptViewer_ctl04_ctl11_divDropDown", index);
        }

        public void SelectParties(bool parties)
        {
            if (parties)
            {
                _parties.CustomClickNoWait();
            }
            else
            {
                _noParties.CustomClickNoWait();
            }
        }
    }
}
