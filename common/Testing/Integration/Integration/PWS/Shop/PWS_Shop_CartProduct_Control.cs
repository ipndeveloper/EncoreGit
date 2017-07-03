using WatiN.Core;
using System.Globalization;
using NetSteps.Testing.Integration.PWS.Shop;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.PWS.Shop
{
    public class PWS_Shop_CartProduct_Control : Control<TableRow>
    {
        private string _sku, _thumb, _name, _price;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            // The following is based upon Gold Canyon
            _sku = Element.OwnTableCells[1].CustomGetText();
            _thumb = Element.OwnTableCells[2].Image(Find.Any).Src;
            string name = Element.OwnTableCells[3].CustomGetText();
            _name = name == null ? name : name.Trim();
            _price = GetPrice();
        }

        public string SKU
        {
            get { return _sku; }
        }

        public string Thumbnail
        {
            get { return _thumb; }
        }

        public string Name
        {
            get { return _name; }
        }

        public string Price
        {
            get { return _price; }
        }

        public string GetPrice()
        {
            if (Element.OwnTableCells[4].Span(Find.ByClass("block price discountPrice")).Exists)
                return Element.OwnTableCells[4].Span(Find.ByClass("block price discountPrice")).CustomGetText();
            else
                return Element.OwnTableCells[4].CustomGetText();
                //return Element.OwnTableCells[4].Span(Find.ByClass(new Regex("originalPrice", RegexOptions.None))).CustomGetText();
        }

        public bool IsValid()
        {
         return !string.IsNullOrEmpty(SKU) &&
                Util.ValidateUrl(Thumbnail) &&
                !string.IsNullOrEmpty(_name)&&
                decimal.Parse(Price, NumberStyles.AllowCurrencySymbol | NumberStyles.AllowDecimalPoint) > 0;
        }

        public bool IsValid(PWS_Shop_Product_Control product)
        {
            return !string.IsNullOrEmpty(SKU) && decimal.Parse(Price, NumberStyles.AllowCurrencySymbol | NumberStyles.AllowDecimalPoint) > 0;
            //return
            //    IsValid() &&
            //   Name.Contains(product.Name);
        }

        public bool ForeignIsValid(PWS_Shop_Product_Control product)
        {
            return !string.IsNullOrEmpty(SKU) && Name.Contains(product.Name);
        }

        public void Remove(int? timeout = null)
        {
            Element.GetElement<Link>(new Param("Remove from cart", AttributeName.ID.Title)).CustomClick(timeout);
        }
    }
}
