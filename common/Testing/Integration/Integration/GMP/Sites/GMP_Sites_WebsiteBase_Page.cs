using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Sites
{
    public abstract class GMP_Sites_WebsiteBase_Page : GMP_Sites_Base_Page
    {
        public GMP_Sites_SectionNav_Control SectionNav
        {
            get { return Control.CreateControl<GMP_Sites_SectionNav_Control>(_secNav); }
        }
    }
}
