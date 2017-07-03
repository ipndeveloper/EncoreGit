using WatiN.Core;
using System.Text.RegularExpressions;
using System.Threading;

namespace NetSteps.Testing.Integration.PWS.Shop
{
    public abstract class PWS_Shop_Base_Page : PWS_Base_Page
    {
        private TextField _txtSearchProduct;
        private Link _search;
        private PWS_Shop_SectionNav_Control _sectionNav;
        private SearchSuggestionBox_Control _searchControl;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _txtSearchProduct = Document.GetElement<TextField>(new Param("txtSearch"));
            _search = Document.GetElement<Link>(new Param("btnSearch")); ;
            _sectionNav = Document.GetElement<Div>(new Param("shopNavListContain", AttributeName.ID.ClassName, RegexOptions.None)).As<PWS_Shop_SectionNav_Control>();
            _searchControl = _content.GetElement<TextField>(new Param("txtSearch")).As<SearchSuggestionBox_Control>();
        }

        public PWS_Shop_SectionNav_Control SectionNav
        {
            get { return _sectionNav; }
        }

        /// <summary>
        /// SearchForProduct
        /// </summary>
        /// <param name="item">Requires OrderItem.Product.SKU and OrderItem.Product.Name</param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public PWS_Shop_Product_Page SearchForProduct(OrderItem item, int? timeout = null)
        {
            _searchControl.Select(item.Product.SKU, string.Format("{0} - {1}", item.Product.SKU, item.Product.Name), true);
            return Util.GetPage<PWS_Shop_Product_Page>(timeout);
        }

        public PWS_Shop_SearchResults_Page SearchForProduct(string searchString = null, int? timeout = null, bool pageRequired = true)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                this._txtSearchProduct.CustomSetTextHelper(searchString);
            }
            timeout = _search.CustomClick(timeout); ;
            return Util.GetPage<PWS_Shop_SearchResults_Page>(timeout, pageRequired);
        }

        public string GetPopUpMessage()
        {
            return Document.GetElement<Div>(new Param("CartMessages")).CustomGetText();
        }

        public PWS_Shop_Cart_Page ClickGoToMyCart(int? timeout = null, bool pageRequired = true)
        {
            timeout = this.Document.GetElement<Link>(new Param("/Cart", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            Thread.Sleep(2000);
            return Util.GetPage<PWS_Shop_Cart_Page>(timeout, pageRequired);
        }
    }
}
