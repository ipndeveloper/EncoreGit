using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Reports
{
    public class GMP_Reports_ExpiringCreditCardNumbers_Page : GMP_Reports_Viewer_Page
    {
        private TableRow _parameters;
        private TextField _months;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _parameters = Document.GetElement<TableRow>(new Param("ParametersRowrptViewer"));
            _months = Document.GetElement<TextField>(new Param("rptViewer_ctl04_ctl09_txtValue"));
        }

        public override bool IsPageRendered()
        {
            return _parameters.Exists;
        }

        public void  SelectAutoshipType(int index)
        {
            SelectCheckBox("rptViewer_ctl04_ctl03_ddDropDownButton", "rptViewer_ctl04_ctl03_divDropDown", index);
            WaitForReport();
        }

        public void SelectAutoships(int index)
        {
            SelectCheckBox("rptViewer_ctl04_ctl05_ddDropDownButton", "rptViewer_ctl04_ctl05_divDropDown", index);
        }

        public void SelectPaymentType(int index)
        {
            SelectCheckBox("rptViewer_ctl04_ctl07_ddDropDownButton", "rptViewer_ctl04_ctl07_divDropDown", index);
        }

        public void EnterExpirationMonths(int months)
        {
            _months.CustomSetTextQuicklyHelper(months.ToString());
        }
    }
}
