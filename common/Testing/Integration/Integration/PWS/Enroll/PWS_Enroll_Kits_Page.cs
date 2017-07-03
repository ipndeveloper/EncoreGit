using WatiN.Core;
using System;

namespace NetSteps.Testing.Integration.PWS.Enroll
{
    public class PWS_Enroll_Kits_Page : PWS_Base_Page
    {
        private ElementCollection<RadioButton> _products;
        private Link _next;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _content.GetElement<RadioButton>(new Param("SelectedProductID")).CustomWaitForExist();
            _products = _content.GetElements<RadioButton>(new Param("SelectedProductID"));
            _next = _content.GetElement<Link>(new Param("btnSubmit"));
        }
        public override bool IsPageRendered()
        {
            return _next.Exists;
        }

        public PWS_Enroll_Kits_Page SelectKit(int? index = null)
        {
            if (!index.HasValue)
                index = Util.GetRandom(0, _products.Count - 1);
            _products[(int)index].CustomSelectRadioButton();
            return this;
        }

        public TPage ClickNext<TPage>(int? timeout = null, bool pageRequired = true) where TPage : NS_Page, new()
        {
            timeout = _next.CustomClick(timeout);
            return Util.GetPage<TPage>(timeout, pageRequired);
        }

    }
}
