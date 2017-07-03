using WatiN.Core;
using NetSteps.Testing.Integration;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration
{
    public abstract class Menu_Control : Control<List>
    {
        protected int _index;

        protected int Index
        {
            get { return _index; }
        }

        protected TPage ClickLink<TPage>(string href, bool slideDown = true, int? timeout = null, bool pageRequired = true) where TPage : NS_Page, new()
        {
            if (slideDown)
                SlideDown();
            timeout = Element.GetElement<Link>(new Param(href, AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<TPage>(timeout, pageRequired);
        }

        protected void SlideDown()
        {
            Element.CustomRunScript("slideDown", null, Index);
        }

        protected void SlideRight(int pixels)
        {
            Element.CustomRunScript("animate", string.Format("{{left: '{0}px'}}", pixels), Index);
        }

        protected void Show()
        {
            Element.CustomRunScript("show", null, Index);
        }

        protected void Hide()
        {
            Element.CustomRunScript("hide", null, Index);
        }
    }
}
