using WatiN.Core;
using System.Globalization;
using System.Threading;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.PWS.Shop
{
    public class PWS_Shop_Product_Control : Control<Div>
    {
        private string _name;
        private Link _productInfo;
        private Span _wrapper;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _name = Element.Span(Find.ByClass("productName", false)).CustomGetText();
            _productInfo = Element.GetElement<Link>(new Param("UI-linkAlt", AttributeName.ID.ClassName, RegexOptions.None));
            _wrapper = Element.Span(Find.ByClass("imagewrapper", false));
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Thumbnail
        {
            get { return Element.Image(Find.Any).Src; }
        }

        [System.Obsolete("Use 'SelectProduct<PWS_Shop_Product_Page>()'", true)]
        public PWS_Shop_Product_Page SelectProduct(int? timeout = null, bool pageRequired = true)
        {
            timeout = _productInfo.CustomClick(timeout);
            Thread.Sleep(2000);
            return Util.GetPage<PWS_Shop_Product_Page>(timeout, pageRequired);
        }

        public TPage SelectProduct<TPage>(int? timeout = null, bool pageRequired = true) where TPage: NS_Page, new()
        {
            timeout = _productInfo.CustomClick(timeout);
            Thread.Sleep(2000);
            return Util.GetPage<TPage>(timeout, pageRequired);
        }
        
        public bool IsValid
        {
            get
            {
                return
                    !string.IsNullOrEmpty(Name) &&
                    _wrapper.Exists &&
                    _productInfo.Exists;
                    //Util.ValidateUrl(Thumbnail);
            }
        }
    }
}
