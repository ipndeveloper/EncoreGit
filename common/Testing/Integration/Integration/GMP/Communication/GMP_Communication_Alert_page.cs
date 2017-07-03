using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP.Communication
{
    public class GMP_Communication_Alert_page : GMP_Comunication_Base_Page
    {
        private Link _addalertTemplate;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _addalertTemplate = Document.GetElement<Link>(new Param("/Communication/Alerts/Edit", AttributeName.ID.Href, RegexOptions.None));
        }

        public GMP_Communication_AlertEdit_Page ClickAddAlertTemplate(int? timeout = null, bool pageRequired = true)
        {
            timeout = _addalertTemplate.CustomClick(timeout);
            return Util.GetPage<GMP_Communication_AlertEdit_Page>(timeout, pageRequired);
        }

         public override bool IsPageRendered()
        {
            return _addalertTemplate.Exists;
        }
    }
}
