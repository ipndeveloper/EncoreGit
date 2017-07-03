using WatiN.Core;
using WatiN.Core.Native;

namespace NetSteps.Testing.Integration
{
    [ElementTag("head")]
    public class Head : ElementContainer<Head>
    {
        public Head(DomContainer domContainer, INativeElement element) : base(domContainer, element) { }
        public Head(DomContainer domContainer, ElementFinder finder) : base(domContainer, finder) { }

        public string GetName()
        {
            return "Head";
        }
    }
}
