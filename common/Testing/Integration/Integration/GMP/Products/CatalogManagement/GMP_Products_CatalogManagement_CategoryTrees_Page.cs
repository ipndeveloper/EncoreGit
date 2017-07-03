using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Products.CatalogManagement
{
    public class GMP_Products_CatalogManagement_CategoryTrees_Page : GMP_Products_CatalogManagement_Base_Page
    {
        private GMP_Products_CatalogManagement_CategorySectionHeader_Control _ctlgCtgySectionHeader;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _ctlgCtgySectionHeader = Control.CreateControl<GMP_Products_CatalogManagement_CategorySectionHeader_Control>(Document.GetElement<Div>(new Param("SectionHeader", AttributeName.ID.ClassName)));
        }

        public GMP_Products_CatalogManagement_CategorySectionHeader_Control SectionHeader
        {
            get { return _ctlgCtgySectionHeader; }
        }

         public override bool IsPageRendered()
        {
            return _ctlgCtgySectionHeader.Element.Exists;
        }
    }
}
