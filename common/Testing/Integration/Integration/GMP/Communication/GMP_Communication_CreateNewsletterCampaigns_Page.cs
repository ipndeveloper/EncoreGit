using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Communication
{
    public class GMP_Communication_CreateNewsletterCampaigns_Page : GMP_Comunication_Base_Page
    {
        private SelectList _market;
        private TextField _name;
        private CheckBox _active;
        private Link _save;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _market = Document.GetElement<SelectList>(new Param("market"));
            _name = Document.GetElement<TextField>(new Param("name"));
            _active = Document.GetElement<CheckBox>(new Param("active"));
            _save = Document.GetElement<Link>(new Param("btnSaveNewsletter"));
        }

         public override bool IsPageRendered()
        {
            return _save.Exists;
        }
    }
}
