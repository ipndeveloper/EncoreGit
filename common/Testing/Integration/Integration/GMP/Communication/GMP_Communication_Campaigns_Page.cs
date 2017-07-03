using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP.Communication
{
    public class GMP_Communication_Campaigns_Page : GMP_Comunication_Base_Page
    {
        private Link _addCampaign;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _addCampaign = Document.GetElement<Link>(new Param("/Communication/NewsletterCampaigns/Create", AttributeName.ID.Href, RegexOptions.None));
        }
        public GMP_Communication_CreateNewsletterCampaigns_Page ClickAddNewsletterCampaign(int? timeout = null, bool pageRequired = true)
        {
            timeout = _addCampaign.CustomClick(timeout);
            return Util.GetPage<GMP_Communication_CreateNewsletterCampaigns_Page>(timeout, pageRequired);
        }

         public override bool IsPageRendered()
        {
            return _addCampaign.Exists;
        }
    }
}
