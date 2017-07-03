using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.PWS.Enroll
{
    public class PWS_Enroll_AboutYou_Page : PWS_Base_Page
    {
        private Form aboutYou;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            aboutYou = _content.GetElement<Form>(new Param("/Enroll/AccountInfo/AboutYou", AttributeName.ID.Action, RegexOptions.None));
        }

        public override bool IsPageRendered()
        {
            return aboutYou.Exists;
        }

        public PWS_Enroll_Shipping_Page ClickNext(int? timeout = null, bool pageRequired = true)
        {
            timeout = _content.GetElement<Link>(new Param("btnSubmit")).CustomClick(timeout);
            return Util.GetPage<PWS_Enroll_Shipping_Page>(timeout, pageRequired);
        }
    }
}
