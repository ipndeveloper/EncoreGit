using WatiN.Core;

namespace NetSteps.Testing.Integration
{
    public abstract class NS_Page : Page
    {
        public abstract bool IsPageRendered();
    }
}
