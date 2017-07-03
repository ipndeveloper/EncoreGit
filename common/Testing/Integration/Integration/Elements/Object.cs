using WatiN.Core;
using WatiN.Core.Native;

namespace NetSteps.Testing.Integration
{
    [ElementTag("object")]
    public class Object : ElementContainer<Meta>
    {
        public Object(DomContainer domContainer, INativeElement element) : base(domContainer, element) { }
        public Object(DomContainer domContainer, ElementFinder finder) : base(domContainer, finder) { }

        public string GetName()
        {
            return "Object";
        }
    }
}
