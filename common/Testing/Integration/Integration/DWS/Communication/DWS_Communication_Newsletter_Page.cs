using WatiN.Core;

namespace NetSteps.Testing.Integration.DWS.Communication
{
    public class DWS_Communication_Newsletter_Page : DWS_Communications_Base_Page
    {
        private SelectList _newsletter;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _newsletter = Document.SelectList("SelectedCampaignID");
        }

         public override bool IsPageRendered()
        {
            return _newsletter.Exists;
        }
    }
}
