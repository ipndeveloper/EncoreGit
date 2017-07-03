using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP.Sites
{
    public class GMP_Sites_SectionNav_Control : Control<UnorderedList>
    {
        public GMP_Sites_Overview_Page ClickOverview(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Sites/Overview", AttributeName.ID.Href, RegexOptions.IgnoreCase)).CustomClick(timeout);
            return Util.GetPage<GMP_Sites_Overview_Page>(timeout, pageRequired);
        }

        public GMP_Sites_Edit_Page ClickEdit(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Sites/Edit/", AttributeName.ID.Href, RegexOptions.IgnoreCase)).CustomClick(timeout);
            return Util.GetPage<GMP_Sites_Edit_Page>(timeout, pageRequired);
        }

        public GMP_Sites_Pages_Page ClickPages(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Sites/Pages", AttributeName.ID.Href, RegexOptions.IgnoreCase)).CustomClick(timeout);
            return Util.GetPage<GMP_Sites_Pages_Page>(timeout, pageRequired);
        }

        public GMP_Sites_SiteMap_Page ClickSiteMap(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Sites/SiteMap", AttributeName.ID.Href, RegexOptions.IgnoreCase)).CustomClick(timeout);
            return Util.GetPage<GMP_Sites_SiteMap_Page>(timeout, pageRequired);
        }

        public GMP_Sites_News_Page ClickNews(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Sites/News", AttributeName.ID.Href, RegexOptions.IgnoreCase)).CustomClick(timeout);
            return Util.GetPage<GMP_Sites_News_Page>(timeout, pageRequired);
        }

        public GMP_Sites_Documents_Page ClickDocuments(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Sites/Documents", AttributeName.ID.Href, RegexOptions.IgnoreCase)).CustomClick(timeout);
            return Util.GetPage<GMP_Sites_Documents_Page>(timeout, pageRequired);
        }
    }
}
