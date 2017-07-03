using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Sites
{
    public class GMP_Sites_PagesEdit_Page : GMP_Sites_WebsiteBase_Page
    {
        private TextField _url;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _url = Document.GetElement<TextField>(new Param("url"));
        }

         public override bool IsPageRendered()
        {
            return _url.Exists;
        }
    }
}
