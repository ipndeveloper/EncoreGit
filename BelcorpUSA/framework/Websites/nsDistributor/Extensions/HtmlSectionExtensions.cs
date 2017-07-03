using System.Collections.Generic;
using System.Web;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Extensions;
using nsDistributor.Controllers;

namespace nsDistributor.Extensions
{
    public static class HtmlSectionExtensions
    {
        public static HtmlString ToDisplay(this HtmlSection section, NetSteps.Common.Constants.ViewingMode pageMode)//, string cssClass = "content")
        {
            return new HtmlString(section.ToDisplay(BaseController.CurrentSite, pageMode, ApplicationContext.Instance.CurrentLanguageID));//, cssClass);
        }

        public static HtmlString ToDisplay(this HtmlSection section)//, string cssClass = "content")
        {
            return new HtmlString(section.ToDisplay(BaseController.CurrentSite, BaseController.PageMode, ApplicationContext.Instance.CurrentLanguageID));//, cssClass);
        }

        public static HtmlString ToDisplay(this HtmlSection section, NetSteps.Common.Constants.ViewingMode pageMode, int? pageId)//, string cssClass = "content")
        {
            return new HtmlString(section.ToDisplay(BaseController.CurrentSite, pageMode, ApplicationContext.Instance.CurrentLanguageID, pageId));//, cssClass);
        }

        public static HtmlString ToDisplay(this HtmlSection section, int? pageId)//, string cssClass = "content")
        {
            return new HtmlString(section.ToDisplay(BaseController.CurrentSite, BaseController.PageMode, ApplicationContext.Instance.CurrentLanguageID, pageId));//, cssClass);
        }

        public static HtmlString ToDisplayLogo(this HtmlSection section, int? pageId, string url)
        {
            return new HtmlString(section.ToDisplayLogo(BaseController.CurrentSite, BaseController.PageMode, ApplicationContext.Instance.CurrentLanguageID, url, pageId));//, cssClass);
        }

        public static HtmlString DisplayHtmlSection(this IEnumerable<HtmlSection> sections, string sectionName, int? pageID = null)
        {
            return new HtmlString(sections.DisplayHtmlSection(BaseController.CurrentSite, BaseController.PageMode, sectionName, pageID));
        }
    }
}