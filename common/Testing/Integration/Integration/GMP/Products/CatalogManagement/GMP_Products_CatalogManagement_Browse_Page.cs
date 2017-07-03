using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP.Products.CatalogManagement
{
    public class GMP_Products_CatalogManagement_Browse_Page : GMP_Products_CatalogManagement_Base_Page
    {
        //private GMP_Products_SubNav_Control _subNav;
        private GMP_Products_CatalogManagement_BrowseSectionHeader_Control _ctlgSectionHeader;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _ctlgSectionHeader = Control.CreateControl<GMP_Products_CatalogManagement_BrowseSectionHeader_Control>(Document.GetElement<Div>(new Param("SectionHeader", AttributeName.ID.ClassName)));
        }

        public GMP_Products_CatalogManagement_BrowseSectionHeader_Control SectionHeader
        {
            get { return _ctlgSectionHeader; }
        }

         public override bool IsPageRendered()
        {
            return Document.GetElement<Link>(new Param("/Products/Catalogs/Edit", AttributeName.ID.Href, RegexOptions.None)).Exists;
        }
    }
}
