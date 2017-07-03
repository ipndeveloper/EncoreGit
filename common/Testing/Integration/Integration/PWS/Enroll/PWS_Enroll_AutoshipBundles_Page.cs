using WatiN.Core;
using System;

namespace NetSteps.Testing.Integration.PWS.Enroll
{
    public class PWS_Enroll_AutoshipBundles_Page : PWS_Base_Page
    {
        private RadioButton _firstBundle;
        private ElementCollection<RadioButton> _bundles;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _firstBundle = _content.GetElement<RadioButton>(new Param("SelectedProductID"));
            _firstBundle.CustomWaitForExist();
            _bundles = _content.GetElements<RadioButton>(new Param("SelectedProductID"));
        }

        public override bool IsPageRendered()
        {
            return _firstBundle.Exists;
        }

        public void SelectBundle(int? index = null)
        {
            if (!index.HasValue)
                index = Util.GetRandom(0, _bundles.Count - 1);
            _bundles[(int)index].CustomSelectRadioButton();
        }

        public TPage ClickNext<TPage>(int? timeout = null, bool pageRequired = true) where TPage : NS_Page, new()
        {
            timeout = _content.GetElement<Link>(new Param("btnSubmit")).CustomClick(timeout);
            return Util.GetPage<TPage>(timeout, pageRequired);
        }
    }
}
