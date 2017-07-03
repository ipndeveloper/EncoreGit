using WatiN.Core;
using System;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.PWS.Enroll
{
    public class PWS_Enroll_ShippingMethod_Page : PWS_Base_Page
    {
        private RadioButton _firstMethod;
        private Div _shippingNA;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _firstMethod = _content.GetElement<RadioButton>(new Param("ShippingMethod", AttributeName.ID.Id, RegexOptions.IgnoreCase));
            _shippingNA = _content.GetElement<Div>(new Param("kitShippingMethods", AttributeName.ID.ClassName, RegexOptions.None));
        }

        public override bool IsPageRendered()
        {
            return (_firstMethod.Exists || _shippingNA.Exists);
        }

        public TPage ClickNext<TPage>(int? timeout = null, bool pageRequired = true) where TPage : NS_Page, new()
        {
            timeout = _content.GetElement<Link>(new Param("Button", AttributeName.ID.ClassName, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<TPage>(timeout, pageRequired);
        }

        public PWS_Enroll_ShippingMethod_Page SelectMethod(int? index = null)
        {
            var methods = _content.GetElements<RadioButton>(new Param("ShippingMethod", AttributeName.ID.Id, RegexOptions.IgnoreCase));
            if (!index.HasValue)
                index = Util.GetRandom(0, methods.Count - 1);
            methods[(int)index].CustomSelectRadioButton();
            return this;
        }
    }
}
