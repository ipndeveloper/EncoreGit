using WatiN.Core;

namespace NetSteps.Testing.Integration.DWS.Communication
{
    public class DWS_Communication_Compose_Page : DWS_Communications_Base_Page
    {
        private Link _sendMsg;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _sendMsg = Document.GetElement<Link>(new Param("uxSendEmail"));
        }

        public override bool IsPageRendered()
        {
            return _sendMsg.Exists;
        }
    }
}
