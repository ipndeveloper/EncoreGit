using WatiN.Core;

namespace NetSteps.Testing.Integration.DWS.Communication
{
    public class DWS_Communication_Group_Page : DWS_Communications_Base_Page
    {
        private Link _save;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _save = Document.GetElement<Link>(new Param("btnSaveGroup"));
        }

         public override bool IsPageRendered()
        {
            return _save.Exists;
        }
    }
}
