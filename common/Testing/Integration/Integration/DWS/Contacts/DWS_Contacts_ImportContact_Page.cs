using WatiN.Core;

namespace NetSteps.Testing.Integration.DWS.Contacts
{
    public class DWS_Contacts_ImportContact_Page : DWS_Contacts_Base_Page
    {
        private Button _upload;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _upload = Document.Button("importContactsSubmit");
        }

         public override bool IsPageRendered()
        {
            return _upload.Exists;
        }
    }
}
