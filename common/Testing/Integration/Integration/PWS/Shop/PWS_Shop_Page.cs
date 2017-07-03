using System;
using WatiN.Core;
using WatiN.Core.Extras;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.PWS.Shop
{
    /// <summary>
    /// Class related to Controls and methods of PWS Shop page.
    /// </summary>
    public class PWS_Shop_Page : PWS_Shop_Base_Page
    {
        private ElementCollection<Link> _categories;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _categories = Document.GetElement<Div>(new Param("ShopNav", AttributeName.ID.ClassName)).GetElements<Link>();
        }
       
         public override bool IsPageRendered()
        {
            return _content.GetElement<Div>(new Param("shoppingMainContent", AttributeName.ID.ClassName)).Exists;
        }

         public PWS_Shop_Category_Page SelectCategory(int? index = null, int? timeout = null, bool pageRequired = true)
        {
            if (!index.HasValue)
                index = Util.GetRandom(0, _categories.Count - 1);
            timeout = _categories[(int)index].CustomClick(timeout);
            return Util.GetPage<PWS_Shop_Category_Page>(timeout, pageRequired);
        }
    }
}