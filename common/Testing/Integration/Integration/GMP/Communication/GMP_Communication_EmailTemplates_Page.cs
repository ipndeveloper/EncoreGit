using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP.Communication
{
    public class GMP_Communication_EmailTemplates_Page : GMP_Comunication_Base_Page
    {
        private Link _addEmailTemplate;

        protected override void  InitializeContents()
        {
 	         base.InitializeContents();
            _addEmailTemplate = Document.GetElement<Link>(new Param("/Communication/EmailTemplates/Edit", AttributeName.ID.Href, RegexOptions.None));
        }

        public GMP_Communication_EmailTemplatesEdit_Page ClickAddEmailTemplate(int? timeout = null, bool pageRequired = true)
        {
            timeout = _addEmailTemplate.CustomClick(timeout);
            return Util.GetPage<GMP_Communication_EmailTemplatesEdit_Page>(timeout, pageRequired);
        }

         public override bool IsPageRendered()
        {
            return _addEmailTemplate.Exists;
        }
    }
}
