using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP.Products.CatalogManagement
{
    public class GMP_Products_CatalogManagement_Edit_Page : GMP_Products_CatalogManagement_Base_Page
    {
        private GMP_Products_CatalogManagement_EditSectionHeader_Control _catalogSectionHeader;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _catalogSectionHeader = Control.CreateControl<GMP_Products_CatalogManagement_EditSectionHeader_Control>(Document.GetElement<Div>(new Param("SectionHeader", AttributeName.ID.ClassName)));
        }

        public GMP_Products_CatalogManagement_EditSectionHeader_Control SectionHeader
        {
            get { return _catalogSectionHeader; }
        }

         public override bool IsPageRendered()
        {
            return !Document.GetElement<Link>(new Param("/Products/Catalogs/Edit", AttributeName.ID.Href, RegexOptions.None)).Exists && Document.GetElement<Link>(new Param("/Products/Catalogs", AttributeName.ID.Href, RegexOptions.None)).Exists;
        }
    }
}
