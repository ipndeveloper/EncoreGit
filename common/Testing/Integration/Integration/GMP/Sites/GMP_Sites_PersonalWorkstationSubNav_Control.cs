using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP.Sites
{
    public class GMP_Sites_PersonalWorkstationSubNav_Control : Control<Div>
    {

        public GMP_Sites_Overview_Page ClickPersonalWebsite(string site = "/Sites/Overview/Index/9218", int? timeout = null, bool pageRequired = true)
        {
            Element.CustomRunScript(Util.strSlideDown, null, 2);
            timeout = Element.GetElement<Link>(new Param(site, AttributeName.ID.Href, RegexOptions.IgnoreCase)).CustomClick(timeout);
            return Util.GetPage<GMP_Sites_Overview_Page>(timeout, pageRequired);
        }
    }
}
