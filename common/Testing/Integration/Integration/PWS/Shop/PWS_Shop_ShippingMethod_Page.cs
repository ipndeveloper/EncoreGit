using WatiN.Core;
using ListItems = WatiN.Core.ListItem;
using System.Text.RegularExpressions;
using System;

namespace NetSteps.Testing.Integration.PWS.Shop
{
    /// <summary>
    /// Class related to Controls and methods of PWS Shipping method page.
    /// </summary>
    public class PWS_Shop_ShippingMethod_Page : PWS_Base_Page
    {
        private RadioButton _defaultMethod;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _defaultMethod = _content.GetElement<RadioButton>(new Param("shipMethodRadio", AttributeName.ID.ClassName));
        }

         public override bool IsPageRendered()
        {
            return _defaultMethod.Exists;
        }

         public PWS_Shop_Billing_Page ClickContinue(int? timeout = null, bool pageRequired = true)
        {
            timeout = _content.GetElement<Link>(new Param("btnNext")).CustomClick(timeout);
            return Util.GetPage<PWS_Shop_Billing_Page>(timeout, pageRequired);
        }

        public PWS_Shop_ShippingMethod_Page SelectShippingMethod(int? index)
        {
            var shippingMethods = _content.GetElements<RadioButton>(new Param("shipMethodRadio", AttributeName.ID.ClassName));
            if (!index.HasValue)
                index = Util.GetRandom(0, shippingMethods.Count - 1);
            shippingMethods[(int)index].CustomSelectRadioButton();
            return this;
        }
    }
}