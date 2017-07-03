using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using NetSteps.Common.Base;
using NetSteps.Common.Exceptions;
using NetSteps.Common.Extensions;
using NetSteps.Common.Utility;
using NetSteps.Common.EldResolver;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Sites.Common.Models;
using NSCR = NetSteps.Sites.Common.Repositories;
using EventRepository = NetSteps.Events.Common.Repositories;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Data.Entities.Repositories
{
    [ContainerRegister(typeof(EventRepository.ISiteRepository), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
	public partial class SiteRepository : NSCR.ISiteRepository, EventRepository.ISiteRepository
    {
        public Site SiteWithNewsAndArchive(int siteID)
        {
            return FirstOrDefault(
                x => x.SiteID == siteID,
                Site.Relations.Archives
                | Site.Relations.News
                | Site.Relations.SiteUrls
            );
        }

        public Site SiteWithNews(int siteID)
        {
            return FirstOrDefault(
                x => x.SiteID == siteID,
                Site.Relations.News
            );
        }

        public Site SiteWithSiteMap(int siteID)
        {
            return FirstOrDefault(
                x => x.SiteID == siteID,
                Site.Relations.Navigations
                | Site.Relations.Pages
                | Site.Relations.SiteType
            );
        }

        public Site LoadFullWithoutContent(int siteID)
        {
            return FirstOrDefault(
                x => x.SiteID == siteID,
                Site.Relations.LoadFullWithoutContent
            );
        }

	    public List<Site> LoadByAccountID(int accountID)
        {
            return Where(
                x => x.AccountID == accountID,
                Site.Relations.SiteUrls
            );
        }

        public Site LoadByAutoshipOrderID(int autoshipOrderID)
        {
            return FirstOrDefault(
                x => x.AutoshipOrderID == autoshipOrderID,
                Site.Relations.SiteUrls
            );
        }

        public List<SiteSettingItem> LoadSiteSettingsInherited(int siteID)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    return context.usp_sitesettings_select_values(siteID).ToList();
                }
            });
        }

        public void SaveSiteSetting(int siteID, string settingName, string settingValue)
        {
            ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var site = context.Sites.FirstOrDefault(s => s.SiteID == siteID);
                    int baseSiteID = (!site.IsBase) ? site.BaseSiteID.ToInt() : site.SiteID;

                    var siteSetting = context.SiteSettings.FirstOrDefault(s => s.Name == settingName);
                    if (siteSetting == null)
                    {
                        siteSetting = new SiteSetting();
                        siteSetting.StartTracking();
                        siteSetting.BaseSiteID = baseSiteID;
                        siteSetting.Name = settingName;
                        siteSetting.Save();
                    }

                    var siteSettingValue = context.SiteSettingValues.FirstOrDefault(s => s.SiteSettingID == siteSetting.SiteSettingID);
                    if (siteSettingValue != null)
                    {
                        siteSettingValue.StartTracking();
                        siteSettingValue.Value = settingValue;
                        siteSettingValue.Save();
                    }
                    else
                    {
                        siteSettingValue = new SiteSettingValue();
                        siteSettingValue.StartTracking();
                        siteSettingValue.SiteSettingID = siteSetting.SiteSettingID;
                        siteSettingValue.SiteID = siteID;
                        siteSettingValue.Value = settingValue;
                        siteSettingValue.Save();
                    }

                    site.StartTracking();
                    site.DateLastModified = DateTime.Now;
                    site.Save();
                }
            });
        }

        public List<Site> LoadByBaseSiteID(int baseSiteId)
        {
            return Where(x => x.BaseSiteID == baseSiteId);
        }

        public List<Site> LoadBaseSites()
        {
            return Where(
                x => x.IsBase,
                Site.Relations.Languages
                | Site.Relations.SiteUrls
            );
        }

        public List<Site> LoadBaseSites(int marketID, int corporateUserID)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var corporateUser = context.CorporateUsers.Include("Sites").First(u => u.CorporateUserID == corporateUserID);

                    var query = context.Sites
                        .Where(s =>
                            s.IsBase
                            && s.MarketID == marketID
                        );

                    if (!corporateUser.HasAccessToAllSites)
                    {
                        var allowedSiteIds = corporateUser.Sites
                            .Select(site => site.SiteID)
                            .ToList();

                        query = query
                            .Where(s => allowedSiteIds.Contains(s.SiteID));
                    }

                    return query
                        .ToList()
                        .LoadRelations(context, Site.Relations.SiteUrls)
                        .ToList();
                }
            });
        }

        public Site FindCorporateSite(int marketID)
        {
            return FirstOrDefault(
                x =>
                    x.IsBase
                    && x.MarketID == marketID
                    && x.SiteTypeID == (short)Constants.SiteType.Corporate,
                Site.Relations.SiteUrls
            );
        }

        public Site LoadSiteWithSiteURLs(int siteID)
        {
            return FirstOrDefault(
                x => x.SiteID == siteID,
                Site.Relations.Languages
                | Site.Relations.SiteUrls
            );
        }

        public Site LoadBaseSiteForNewPWS(int marketID)
        {
            return FirstOrDefault(
                x =>
                    x.IsBase
                    && x.SiteTypeID == (short)Constants.SiteType.Replicated
                    && x.MarketID == marketID,
                Site.Relations.SiteUrls
				| Site.Relations.Languages
            ) ?? FirstOrDefault(
                x =>
                    x.IsBase
                    && x.SiteTypeID == (short)Constants.SiteType.Replicated,
                Site.Relations.SiteUrls
				| Site.Relations.Languages
            );
        }

        public PaginatedList<SiteSearchData> Search(SiteSearchParameters searchParams)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var results = new PaginatedList<SiteSearchData>(searchParams);

                    IQueryable<Site> matchingSites = context.Sites.Include("SiteUrls");

	                if(searchParams.BaseSiteID.HasValue)
	                {
		                matchingSites = matchingSites.Where(s => s.BaseSiteID == searchParams.BaseSiteID.Value);
	                }

	                if(searchParams.SiteStatusID.HasValue)
	                {
		                matchingSites = matchingSites.Where(s => s.SiteStatusID == searchParams.SiteStatusID.Value);
	                }

	                if(!string.IsNullOrEmpty(searchParams.SiteName))
	                {
		                matchingSites = matchingSites.Where(s => s.Name.Contains(searchParams.SiteName));
	                }

	                if(!string.IsNullOrEmpty(searchParams.Url))
	                {
		                matchingSites = matchingSites.Where(s => s.SiteUrls.Any(su => su.Url.Contains(searchParams.Url)));
	                }

	                if(!searchParams.OrderBy.IsNullOrEmpty())
	                {
		                if(searchParams.OrderBy.Equals("Url", StringComparison.InvariantCultureIgnoreCase))
		                {
			                matchingSites = matchingSites.ApplyOrderByFilter(searchParams.OrderByDirection,
				                s =>
					                s.SiteUrls.Count(su => su.IsPrimaryUrl) > 0
						                ? s.SiteUrls.FirstOrDefault(su => su.IsPrimaryUrl).Url
						                : s.SiteUrls.FirstOrDefault().Url);
		                }
		                else
		                {
			                matchingSites = matchingSites.ApplyOrderByFilter(searchParams, context);
		                }
	                }
	                else
	                {
		                matchingSites = matchingSites.ApplyOrderByFilter(searchParams.OrderByDirection, s => s.Name);
	                }

                    results.TotalCount = matchingSites.Count();

                    // Apply Paging filter - JHE
	                if(searchParams.PageSize.HasValue)
	                {
		                matchingSites = matchingSites.ApplyPagination(searchParams);
	                }

                    var siteInfos = matchingSites.Select(s => new
                                    {
                                        SiteName = s.Name,
                                        PrimaryUrl = s.SiteUrls.FirstOrDefault(su => (bool)su.IsPrimaryUrl),
                                        s.Account.EnrollmentDateUTC,
                                        s.SiteID,
                                        s.SiteStatusID,
                                        SiteStatus = s.SiteStatus.Name,
                                        s.AccountID,
                                        AccountNumber = s.Account == null ? "" : s.Account.AccountNumber,
                                        s.AutoshipOrderID
                                    }).ToList();

                    results.AddRange(siteInfos.Select(s => new SiteSearchData()
                    {
                        SiteName = s.SiteName,
                        Url = (s.PrimaryUrl == null) ? string.Empty : s.PrimaryUrl.Url,
                        Enrolled = s.EnrollmentDateUTC.UTCToLocal(),
                        SiteID = s.SiteID,
                        SiteStatusID = s.SiteStatusID,
                        SiteStatus = SmallCollectionCache.Instance.SiteStatuses.GetById(s.SiteStatusID).GetTerm(),
                        AccountID = s.AccountID.ToInt(),
                        AccountNumber = s.AccountNumber,
                        AutoshipOrderID = s.AutoshipOrderID
                    }));

                    return results;
                }
            });
        }

        public string LoadBackOfficeURL(int siteID)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var siteInfo = context.Sites
                        .Where(s => s.SiteID == siteID)
                        .Select(s => new
                        {
                            s.MarketID,
                            s.DefaultLanguageID
                        })
                        .FirstOrDefault();
                    
                    if (siteInfo == null)
                    {
                        return string.Empty;
                    }

                    return context.Sites
                        .Where(s =>
                            s.MarketID == siteInfo.MarketID
                            && s.DefaultLanguageID == siteInfo.DefaultLanguageID
                            && s.SiteTypeID == (short)Constants.SiteType.BackOffice
                        )
                        .SelectMany(s => s.SiteUrls
                            .OrderByDescending(su => su.IsPrimaryUrl)
                            .Select(su => su.Url)
                        )
                        .FirstOrDefault() ?? string.Empty;
                }
            });
        }

        public Site LoadSiteForCache(int siteID)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var site = context.Sites
                        .FirstOrDefault(s =>
                            s.SiteID == siteID
                        );

                    if (site == null)
                    {
                        return null;
                    }

                    return site.IsBase
                        ? site.LoadRelations(context, Site.Relations.LoadFullBaseSite)
                        : site.LoadRelations(context, Site.Relations.LoadFullChildSite);
                }
            });
        }

        public T LoadByUrl<T>(string url, Func<Site, T> selector)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    url = url.AppendForwardSlash().ToLower();
                    const int siteStatusId = (int)Constants.SiteStatus.Active;

                    return context.SiteUrls
                        .Where(x => x.Url == url)
                        .OrderByDescending(x => x.Site.SiteStatusID == siteStatusId)
                        .Select(x => x.Site)
                        .Select(selector)
                        .FirstOrDefault();
                }
            });
        }

        public Site LoadFullClone(int siteID)
        {
            return FirstOrDefaultFull(s => s.SiteID == siteID);
        }

        public IEnumerable<Page> LoadPages(int siteID)
        {
            var site = FirstOrDefault(
                s => s.SiteID == siteID,
                Site.Relations.Pages
            );

            if (site == null)
            {
                return Enumerable.Empty<Page>();
            }

            return site.Pages
                .AsEnumerable();
        }

        public Site SiteWithLanguages(int siteID)
        {
            return FirstOrDefault(
                s => s.SiteID == siteID,
                Site.Relations.Languages
            );
        }

        public IList<int> QuerySitesBySiteTypeId(int siteTypeId)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    return (from site in context.Sites where site.SiteTypeID == siteTypeId select site.SiteID).ToList();
                }
            });
        }

        public List<Site> GetOtherBaseSites(Site baseSite)
        {
            return Where(
                s =>
                    s.IsBase
                    && s.SiteTypeID == baseSite.SiteTypeID
                    && s.SiteID != baseSite.SiteID,
                Site.Relations.Languages
                | Site.Relations.SiteUrls
            );
		}

	    public int QueryByUrl(string key)
	    {
	        throw new NotImplementedException();
	    }

	    public virtual ISite GetSite(int siteId)
		{
			return FirstOrDefault(s => s.SiteID == siteId);
		}

	    int? ISiteRepository.GetSiteId(string url)
	    {
		    return GetSiteId(url);
	    }

		public virtual int? GetSiteId(string url)
		{
			var siteId = LoadByUrl(url, s => s.SiteID);
			if (siteId == 0)
			{
				return null;
			}
			return siteId;
		}

        #region Load Helpers

        public override Site LoadFull(int siteID)
        {
            var site = FirstOrDefaultFull(x => x.SiteID == siteID);

            if (site == null)
            {
                throw new NetStepsDataException(string.Format("No Site found with SiteID = {0}.", siteID));
            }

            return site;
        }

		public override List<Site> LoadBatchFull(IEnumerable<int> siteIDs)
        {
			return WhereFull(x => siteIDs.Contains(x.SiteID));
        }

        public override List<Site> LoadAllFull()
        {
            return WhereFull(x => true);
        }

        public virtual Site FirstOrDefaultFull(Expression<Func<Site, bool>> predicate)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    return FirstOrDefaultFull(predicate, context);
                }
            });
        }

        public virtual Site FirstOrDefaultFull(Expression<Func<Site, bool>> predicate, NetStepsEntities context)
        {
            return FirstOrDefault(predicate, Site.Relations.LoadFull, context);
        }

        public virtual Site FirstOrDefault(Expression<Func<Site, bool>> predicate, Site.Relations relations)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    return FirstOrDefault(predicate, relations, context);
                }
            });
        }

        public virtual Site FirstOrDefault(Expression<Func<Site, bool>> predicate, Site.Relations relations, NetStepsEntities context)
        {
            var site = context.Sites.FirstOrDefault(predicate);

            if (site == null)
            {
                return null;
            }

            site.LoadRelations(context, relations);

            return site;
        }

        public virtual List<Site> WhereFull(Expression<Func<Site, bool>> predicate)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    return WhereFull(predicate, context);
                }
            });
        }

        public virtual List<Site> WhereFull(Expression<Func<Site, bool>> predicate, NetStepsEntities context)
        {
            return Where(predicate, Site.Relations.LoadFull, context);
        }

        public virtual List<Site> Where(Expression<Func<Site, bool>> predicate, Site.Relations relations)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    return Where(predicate, relations, context);
                }
            });
        }

        public virtual List<Site> Where(Expression<Func<Site, bool>> predicate, Site.Relations relations, NetStepsEntities context)
        {
            var sites = context.Sites.Where(predicate).ToList();

            sites.LoadRelations(context, relations);

            return sites;
        }

        #endregion

        public string GetPWSUrlByAccountID(int accountID)
        {
            var sites = LoadByAccountID(accountID);
            Site site = sites.Count > 1 ? sites.FirstOrDefault(s => s.SiteStatusID == (short)Constants.SiteStatus.Active && s.PrimaryUrl != null && !s.PrimaryUrl.Url.IsNullOrEmpty()) : sites.FirstOrDefault();
            return site == null ? null : site.PrimaryUrl == null ? null : site.PrimaryUrl.Url.EldEncode();
        }
    }
}