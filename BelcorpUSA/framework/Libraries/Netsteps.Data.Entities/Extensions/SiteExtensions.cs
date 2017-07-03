using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Cache;

namespace NetSteps.Data.Entities.Extensions
{
	/// <summary>
	/// Author: John Egbert
	/// Description: SiteExtensions Extensions
	/// Created: 10-20-2010
	/// </summary>
	public static class SiteExtensions
	{
		public static HtmlSection GetHtmlSectionByName(this Site site, string name)
		{
			Site baseSite = site.IsBase ? site : SiteCache.GetSiteByID(site.BaseSiteID.ToInt());

			return baseSite.HtmlSections.GetBySectionName(name);
		}

		public static Page GetPageByUrl(this Site site, string url)
		{
			Site baseSite = site.IsBase ? site : SiteCache.GetSiteByID(site.BaseSiteID.ToInt());

			return baseSite.Pages.FirstOrDefault(p => p.Url.Equals(url, StringComparison.InvariantCultureIgnoreCase))
					 ?? new Page(); //Give friendly error messages.
		}

		public static List<HtmlContent> GetAllChoices(this Site site, int sectionId)
		{
			return site.GetAllChoices(sectionId, ApplicationContext.Instance.CurrentLanguageID);
		}

		public static List<HtmlContent> GetAllChoices(this Site site, int sectionId, int languageId)
		{
			return site.GetAllSectionChoices(sectionId, languageId).Select(hsc => hsc.HtmlContent).ToList();
		}

		public static List<HtmlSectionChoice> GetAllSectionChoices(this Site site, int sectionId)
		{
			return site.GetAllSectionChoices(sectionId, ApplicationContext.Instance.CurrentLanguageID);
		}

		public static List<HtmlSectionChoice> GetAllSectionChoices(this Site site, int sectionId, int languageId)
		{
			Site contentSite = SiteCache.GetSiteByID(site.SiteID);
			return contentSite.HtmlSectionChoices.Where(hsc => hsc.HtmlSectionID == sectionId && hsc.HtmlContent.LanguageID == languageId).OrderBy(hsc => hsc.SortIndex).ToList();
		}

		public static List<HtmlContent> GetActiveChoices(this Site site, int sectionId)
		{
			return site.GetActiveChoices(sectionId, ApplicationContext.Instance.CurrentLanguageID);
		}

		public static List<HtmlContent> GetActiveChoices(this Site site, int sectionId, int languageId)
		{
			Site contentSite = SiteCache.GetSiteByID(site.SiteID);
			return contentSite.HtmlSectionChoices.Where(hsc => hsc.HtmlSectionID == sectionId && hsc.HtmlContent.LanguageID == languageId && hsc.HtmlContent.PublishDateUTC <= DateTime.Now.ApplicationNow().LocalToUTC()).OrderBy(hsc => hsc.SortIndex).Select(hsc => hsc.HtmlContent).ToList();
		}

		public static int GetMaxChoiceIndex(this Site site, int sectionId)
		{
			return site.GetMaxChoiceIndex(sectionId, ApplicationContext.Instance.CurrentLanguageID);
		}

		public static int GetMaxChoiceIndex(this Site site, int sectionId, int languageId)
		{
			Site contentSite = SiteCache.GetSiteByID(site.SiteID);
			var sectionChoices = contentSite.HtmlSectionChoices.Where(hsc => hsc.HtmlSectionID == sectionId && hsc.HtmlContent.LanguageID == languageId);
			return sectionChoices == null || !sectionChoices.Any() ? 0 : sectionChoices.Max(hsc => hsc.SortIndex);
		}

		#region Html Section Extensions
		public static HtmlContent ContentByStatus(this Site site, int htmlSectionId, Constants.HtmlContentStatus status)
		{
			return site.ContentByStatus(htmlSectionId, status, ApplicationContext.Instance.CurrentLanguageID);
		}

		public static HtmlContent ContentByStatus(this Site site, int htmlSectionId, Constants.HtmlContentStatus status, int languageId)
		{
			var siteContent = SiteCache.GetSiteByID(site.SiteID);

			var sectionContent = siteContent.HtmlSectionContents.FirstOrDefault(hsc => hsc.HtmlSectionID == htmlSectionId && hsc.HtmlContent.HtmlContentStatusID == (int)status && hsc.HtmlContent.LanguageID == languageId);

			if (sectionContent == default(HtmlSectionContent))
			{
				if (site.IsBase)
				{
					return null;
				}

				sectionContent = siteContent.HtmlSectionContents.FirstOrDefault(hsc => hsc.HtmlSectionID == htmlSectionId && hsc.HtmlContent.HtmlContentStatusID == (int)status && hsc.HtmlContent.LanguageID == languageId);

				if (sectionContent == default(HtmlSectionContent))
				{
					return null;
				}
			}

			return sectionContent.HtmlContent;
		}

		public static List<HtmlContent> AllContentByStatus(this Site site, int htmlSectionId, Constants.HtmlContentStatus status)
		{
			return site.AllContentByStatus(htmlSectionId, status, ApplicationContext.Instance.CurrentLanguageID);
		}

		public static List<HtmlContent> AllContentByStatus(this Site site, int htmlSectionId, Constants.HtmlContentStatus status, int languageId)
		{
			var siteContent = SiteCache.GetSiteByID(site.SiteID);

			var content = siteContent.HtmlSectionContents.Where(hsc => hsc.HtmlSectionID == htmlSectionId && hsc.HtmlContent.HtmlContentStatusID == (int)status && hsc.HtmlContent.LanguageID == languageId).Select(hsc => hsc.HtmlContent);

			if (content == null || !content.Any())
			{
				if (site.IsBase)
				{
					return new List<HtmlContent>();
				}

				content = siteContent.HtmlSectionContents.Where(hsc => hsc.HtmlSectionID == htmlSectionId && hsc.HtmlContent.HtmlContentStatusID == (int)status && hsc.HtmlContent.LanguageID == languageId).Select(hsc => hsc.HtmlContent);

				if (content == null || !content.Any())
				{
					return new List<HtmlContent>();
				}
			}
			return content.ToList();
		}
		#endregion
	}
}