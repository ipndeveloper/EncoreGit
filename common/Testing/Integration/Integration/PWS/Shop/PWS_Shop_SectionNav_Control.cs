using WatiN.Core;
using System;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.PWS.Shop
{
    public class PWS_Shop_SectionNav_Control : Control<Div>
    {
        private LinkCollection _lnks;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _lnks = Element.Links;
        }

        public PWS_Shop_Category_Page SelectCategory(int? index = null, int? timeout = null, bool pageRequired = true)
        {
            if (!index.HasValue)
                index = Util.GetRandom(0, _lnks.Count - 1);
            timeout = _lnks[(int)index].CustomClick(timeout);
            return Util.GetPage<PWS_Shop_Category_Page>(timeout, pageRequired);
        }
    }
}
