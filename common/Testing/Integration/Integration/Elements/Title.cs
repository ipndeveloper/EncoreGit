using WatiN.Core;
using WatiN.Core.Native;

namespace NetSteps.Testing.Integration
{
    [ElementTag("title")]
    public class Title : ElementContainer<Title>
    {
        public Title(DomContainer domContainer, INativeElement element) : base(domContainer, element) { }
        public Title(DomContainer domContainer, ElementFinder finder) : base(domContainer, finder) { }

        public string GetName()
        {
            return "Title";
        }
    }
}
