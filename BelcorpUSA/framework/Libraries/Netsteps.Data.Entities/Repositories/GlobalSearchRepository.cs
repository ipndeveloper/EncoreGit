using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Common;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class GlobalSearchRepository
	{
		public List<GlobalSearchData> Search(int accountID, int siteID, string query, int? languageID = null)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (var context = new NetStepsEntities())
				{
					var results = new List<GlobalSearchData>();

					int language = languageID.HasValue ? languageID.Value : ApplicationContext.Instance.CurrentLanguageID;

					results.AddRange(context.News.Where(n => n.Active
							   && n.HtmlSection.HtmlSectionContents.Any(hsc => hsc.HtmlContent.HtmlContentStatusID == (int)Constants.HtmlContentStatus.Production
								   && hsc.SiteID == siteID
								   && hsc.HtmlContent.LanguageID == language
								   && hsc.HtmlContent.HtmlElements.FirstOrDefault(e => e.HtmlElementTypeID == (int)Constants.HtmlElementType.Title).Contents.Contains(query))).Select(n => new
								   {
									   n.NewsID,
									   Title = n.HtmlSection.HtmlSectionContents.Any(hsc => hsc.HtmlContent.HtmlContentStatusID == (int)Constants.HtmlContentStatus.Production && hsc.HtmlContent.LanguageID == language)
												   ? n.HtmlSection.HtmlSectionContents.FirstOrDefault(hsc => hsc.HtmlContent.HtmlContentStatusID == (int)Constants.HtmlContentStatus.Production && hsc.HtmlContent.LanguageID == language).HtmlContent.HtmlElements.FirstOrDefault(e => e.HtmlElementTypeID == (int)Constants.HtmlElementType.Title).Contents
												   : ""
								   }).ToList().Select(n => new GlobalSearchData()
								   {
									   ID = n.NewsID.ToString(),
									   DisplayText = n.Title,
									   Type = "News"
								   }));

					results.AddRange(context.usp_accounts_search_downline(accountID, query).Where(a => a.AccountID != accountID).Select(a =>
					{
						var data = new GlobalSearchData()
						{
							ID = a.AccountNumber,
							DisplayText = a.FirstName + " " + a.LastName + " (#" + a.AccountNumber + ")"
						};
						switch (a.AccountTypeID)
						{
							case (int)Constants.AccountType.Prospect:
								data.Type = "Prospects";
								break;
							case (int)Constants.AccountType.RetailCustomer:
								data.Type = "Retail Customers";
								break;
							case (int)Constants.AccountType.PreferredCustomer:
								data.Type = "Preferred Customers";
								break;
							case (int)Constants.AccountType.Distributor:
								data.Type = "Team Members";
								break;
						}
						return data;
					}));

					//results.AddRange(context.Accounts.Where(a => (a.AccountTypeID == (int)Constants.AccountType.Prospect || a.AccountTypeID == (int)Constants.AccountType.RetailCustomer || a.AccountTypeID == (int)Constants.AccountType.PreferredCustomer || a.AccountTypeID == (int)Constants.AccountType.Distributor)
					//    && a.SponsorID == accountID
					//    && (a.AccountNumber.Contains(query) || (a.FirstName + " " + a.LastName).Contains(query))).Select(a => new
					//{
					//    a.AccountID,
					//    DisplayText = a.FirstName + " " + a.LastName + " (#" + a.AccountNumber + ")",
					//    a.AccountTypeID
					//}).ToList().Select(a =>
					//{
					//    var data = new GlobalSearchData()
					//        {
					//            ID = a.AccountID,
					//            DisplayText = a.DisplayText
					//        };
					//    switch (a.AccountTypeID)
					//    {
					//        case (int)Constants.AccountType.Prospect:
					//            data.Type = "Prospects";
					//            break;
					//        case (int)Constants.AccountType.RetailCustomer:
					//            data.Type = "Retail Customers";
					//            break;
					//        case (int)Constants.AccountType.PreferredCustomer:
					//            data.Type = "Preferred Customers";
					//            break;
					//        case (int)Constants.AccountType.Distributor:
					//            data.Type = "Team Members";
					//            break;
					//    }
					//    return data;
					//}));
					// 11903 - JM - Hide items that are not active.
					results.AddRange(context.Archives.Where(a => a.Active && (a.Sites.Select(s => s.SiteID).Contains(siteID) && a.Translations.Any(t => t.Name.Contains(query) || t.LongDescription.Contains(query) || t.ShortDescription.Contains(query)))).Select(a => new
					{
						a.ArchiveID,
						Name = a.Translations.Any(t => t.LanguageID == language) ? a.Translations.FirstOrDefault(t => t.LanguageID == language).Name : a.Translations.Any() ? a.Translations.FirstOrDefault().Name : ""
					}).ToList().Select(a => new GlobalSearchData
					{
						ID = a.ArchiveID.ToString(),
						DisplayText = a.Name,
						Type = "Documents"
					}));

					return results;
				}
			});
		}
	}
}
