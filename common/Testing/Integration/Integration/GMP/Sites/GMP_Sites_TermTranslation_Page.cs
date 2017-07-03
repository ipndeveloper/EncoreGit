using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Sites
{
    public class GMP_Sites_TermTranslation_Page : GMP_Sites_Base_Page
    {
         public override bool IsPageRendered()
        {
            return Document.GetElement<TextField>(new Param("txtSearchTerms")).Exists;
        }
    }
}
