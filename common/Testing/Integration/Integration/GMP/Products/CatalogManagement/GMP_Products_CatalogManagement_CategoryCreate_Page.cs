using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP.Products.CatalogManagement
{
    public class GMP_Products_CatalogManagement_CategoryCreate_page : GMP_Products_CatalogManagement_Base_Page
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
            return !Document.GetElement<Link>(new Param("/Products/Categories/NewTree", AttributeName.ID.Href, RegexOptions.None)).Exists && Document.GetElement<Link>(new Param("/Products/Categories", AttributeName.ID.Href, RegexOptions.None)).Exists;
        }
    }
}
