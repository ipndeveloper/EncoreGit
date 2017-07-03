using WatiN.Core;
using System;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.PWS.Enroll
{
    public class PWS_Enroll_Products_Page : PWS_Base_Page
    {
        private ElementCollection<Link> _items;
        private Link _firstItem;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _firstItem = _content.GetElement<Link>(new Param("FR Button.* m5$", AttributeName.ID.ClassName, RegexOptions.None));
            _firstItem.CustomWaitForExist();
            _items = _content.GetElements<Link>(new Param("FR Button.* m5$", AttributeName.ID.ClassName, RegexOptions.None));
        }

        public override bool IsPageRendered()
        {
            return _firstItem.Exists;
        }

        public TPage ClickNext<TPage>(int? timeout = null, bool pageRequired = true) where TPage : NS_Page, new()
        {
            timeout = _content.GetElement<Link>(new Param("Button", AttributeName.ID.ClassName)).CustomClick(timeout);
            return Util.GetPage<TPage>(timeout, pageRequired);
        }

        public void SelectItem(int? index = null, int? timeout = null)
        {
            if (!index.HasValue)
                index = Util.GetRandom(0, _items.Count - 1);
            _items[(int)index].CustomClick(timeout);
        }

        public void SelectAllItems(int? timeout = null)
        {
            foreach (Link lnk in _items)
            {
                timeout = lnk.CustomClick(timeout);
            }
        }
    }
}
