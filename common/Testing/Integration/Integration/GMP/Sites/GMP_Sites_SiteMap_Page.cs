using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Sites
{
    public class GMP_Sites_SiteMap_Page : GMP_Sites_WebsiteBase_Page
    {
         public override bool IsPageRendered()
        {
            return Document.GetElement<Link>(new Param("btnAddLink")).Exists;
        }
    }
}
