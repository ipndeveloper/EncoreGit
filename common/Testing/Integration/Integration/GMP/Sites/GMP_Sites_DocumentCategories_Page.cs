using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Sites
{
    public class GMP_Sites_DocumentCategories_Page : GMP_Sites_WebsiteBase_Page
    {
        private Link _addRoot;

        protected override void  InitializeContents()
        {
 	         base.InitializeContents();
             _addRoot = Document.GetElement<Link>(new Param("addRoot"));
        }

         public override bool IsPageRendered()
        {
            return _addRoot.Exists;
        }
    }
}
