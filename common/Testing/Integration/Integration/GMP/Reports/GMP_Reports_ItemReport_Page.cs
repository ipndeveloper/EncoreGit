using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Reports
{
    public class GMP_Reports_ItemReport_Page : GMP_Reports_Viewer_Page
    {
        private SelectList _activeProducts;
        private TableRow _parameters;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _activeProducts = Document.GetElement<SelectList>(new Param("rptViewer_ctl04_ctl05_ddValue"));
            _parameters = Document.GetElement<TableRow>(new Param("ParametersRowrptViewer"));
        }

        public override bool IsPageRendered()
        {
            return _parameters.Exists;
        }

        public void SelectProductTypes(int index)
        {
            SelectCheckBox("rptViewer_ctl04_ctl03_ddDropDownButton", "rptViewer_ctl04_ctl03_divDropDown", index);
            WaitForReport();
        }

        public void SelectActiveProducts(int index)
        {
            _activeProducts.CustomSelectDropdownItem(index);
        }
    }
}
