using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Communication
{
    public class GMP_Communication_EmailTemplatesEdit_Page : GMP_Comunication_Base_Page
    {
        private Link _save;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _save = Document.GetElement<Link>(new Param("btnSaveTemplate"));
        }
         public override bool IsPageRendered()
        {
            return _save.Exists;
        }
    }
}
