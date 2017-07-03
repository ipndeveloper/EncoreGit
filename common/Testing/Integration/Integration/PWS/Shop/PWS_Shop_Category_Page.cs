using System;
using WatiN.Core;
using WatiN.Core.Extras;

namespace NetSteps.Testing.Integration.PWS.Shop
{
    /// <summary>
    /// Class related to Controls and methods of PWS Shop products page.
    /// </summary>
    public class PWS_Shop_Category_Page : PWS_Shop_Base_Page
    {
        private Div _category;
        private DivCollection _products;
        private string _name;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _category = _content.GetElement<Div>(new Param("CategoryGroup", AttributeName.ID.ClassName));
            //_category.CustomWaitUntilExist();
            _products = _category.Divs.Filter(Find.ByClass("brdr brdrAll ProductThumbBox", false));
            _name = Util.Browser.Title;
        }

         public override bool IsPageRendered()
        {
            return _category.Exists;
        }

        public string CategoryName
        {
            get { return _name; }
        }

        public ControlCollection<TControl> GetProducts<TControl>() where TControl : PWS_Shop_Product_Control, new()
        {
            return _products.As<TControl>();
        }

        public TControl GetProduct<TControl>(int? index = null) where TControl : PWS_Shop_Product_Control, new()
        {
            if (!index.HasValue)
            {
                index = Util.GetRandom(0, _products.Count - 1);
            }
            // Select the product even if no stock or back order
            return _products[(int)index].As<TControl>();
        }
    }
}