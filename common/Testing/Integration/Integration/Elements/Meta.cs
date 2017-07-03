using WatiN.Core;
using WatiN.Core.Native;

namespace NetSteps.Testing.Integration
{
    [ElementTag("meta")]
    public class Meta : ElementContainer<Meta>
    {
        public Meta(DomContainer domContainer, INativeElement element) : base(domContainer, element) { }
        public Meta(DomContainer domContainer, ElementFinder finder) : base(domContainer, finder) { }

        public string GetName()
        {
            return "Meta";
        }
    }
}
