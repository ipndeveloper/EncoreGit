using WatiN.Core;

namespace NetSteps.Testing.Integration.DWS.MyAccount
{
    public class DWS_MyAccount_EditProfile_Page : DWS_MyAccount_Base_Page
    {
        private Link _save;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _save = Document.GetElement<Link>(new Param("btnSaveAccountMainInfo"));
        }

         public override bool IsPageRendered()
        {
            return _save.Exists;
        }
    }
}
