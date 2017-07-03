using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.PWS.Enroll
{
    public class PWS_Enroll_KitUpgrade_Page : PWS_Base_Page
    {
        private ElementCollection<Image> _upgrades;
        private Link _next;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _content.GetElement<Image>(new Param("check.png", AttributeName.ID.Src, RegexOptions.None)).CustomWaitForExist();
            _upgrades = _content.GetElements<Image>(new Param("check.png", AttributeName.ID.Src, RegexOptions.None));
            _next = _content.GetElement<Link>(new Param("btnSubmit"));
        }

        public override bool IsPageRendered()
        {
            return _upgrades.Count > 0;
        }

        public void SelectUpgrade(int index)
        {
            _upgrades[index].CustomClick();
        }

        public PWS_Enroll_ShippingMethod_Page ClickNext(int? timeout = null)
        {
            timeout = _next.CustomClick(timeout);
            return Util.GetPage<PWS_Enroll_ShippingMethod_Page>();
        }
    }
}
