using System.Web;
using DistributorBackOffice.Controllers;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Extensions;

namespace DistributorBackOffice.Extensions
{
	public static class HtmlSectionExtensions
	{
		public static HtmlString ToDisplay(this HtmlSection section, NetSteps.Common.Constants.ViewingMode pageMode)//, string cssClass = "content")
		{
			return new HtmlString(section.ToDisplay(BaseController.GetCurrentSite(), pageMode, ApplicationContext.Instance.CurrentLanguageID));//, cssClass);
		}

		public static HtmlString ToDisplay(this HtmlSection section)//, string cssClass = "content")
		{
			return new HtmlString(section.ToDisplay(BaseController.GetCurrentSite(), BaseController.GetPageMode(), ApplicationContext.Instance.CurrentLanguageID));//, cssClass);
		}

		public static HtmlString ToDisplay(this HtmlSection section, NetSteps.Common.Constants.ViewingMode pageMode, int? pageId)//, string cssClass = "content")
		{
			return new HtmlString(section.ToDisplay(BaseController.GetCurrentSite(), pageMode, ApplicationContext.Instance.CurrentLanguageID, pageId));//, cssClass);
		}

		public static HtmlString ToDisplay(this HtmlSection section, int? pageId)//, string cssClass = "content")
		{
			return new HtmlString(section.ToDisplay(BaseController.GetCurrentSite(), BaseController.GetPageMode(), ApplicationContext.Instance.CurrentLanguageID, pageId));//, cssClass);
		}

		public static HtmlString ToDisplayLogo(this HtmlSection section, int? pageId, string url)
		{
			return new HtmlString(section.ToDisplayLogo(BaseController.GetCurrentSite(), BaseController.GetPageMode(), ApplicationContext.Instance.CurrentLanguageID, url, pageId));//, cssClass);
		}
	}
}