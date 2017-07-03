using WatiN.Core;

namespace NetSteps.Testing.Integration.DWS.Contacts
{
    public class DWS_Contacts_ViewContact_Page : DWS_Contacts_Base_Page
    {
        private UnorderedList _info;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _info = Document.UnorderedList(Find.ByClass("listNav flatList infoList"));
        }
         public override bool IsPageRendered()
        {
            return _info.Exists;
        }
    }
}
