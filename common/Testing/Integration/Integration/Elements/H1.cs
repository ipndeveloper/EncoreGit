using WatiN.Core;
using WatiN.Core.Native;

namespace NetSteps.Testing.Integration
{
    [ElementTag("h1")]
    public class H1 : ElementContainer<H1>
    {
        public H1(DomContainer domContainer, INativeElement element) : base(domContainer, element) { }
        public H1(DomContainer domContainer, ElementFinder finder) : base(domContainer, finder) { }

        public string GetName()
        {
            return "H1";
        }
    }
}
