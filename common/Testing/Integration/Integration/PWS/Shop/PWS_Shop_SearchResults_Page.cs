using WatiN.Core;
using System;

namespace NetSteps.Testing.Integration.PWS.Shop
{
    public class PWS_Shop_SearchResults_Page : PWS_Shop_Base_Page
    {
        private SelectList _filter;
        private DivCollection _products;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _filter = Document.SelectList("sCategory");
            _products = Document.GetElement<Div>(new Param("searchResults")).Divs.Filter(Find.ByClass("brdr brdrAll ProductThumbBox", false));
        }

         public override bool IsPageRendered()
        {
            return _products[0].Exists;
        }

        public TControl GetProduct<TControl>(int? index = null) where TControl : PWS_Shop_Product_Control, new()
        {
            if (!index.HasValue)
            {
                index = Util.GetRandom(0, _products.Count - 1);
            }
            return _products[(int)index].As<TControl>();
        }
    }
}
