using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business;

namespace NetSteps.Data.Entities.Repositories
{
	[ContractClass(typeof(Contracts.SiteRepositoryContracts))]
    public partial interface ISiteRepository : ISearchRepository<SiteSearchParameters, PaginatedList<SiteSearchData>>
    {
        List<Site> LoadByAccountID(int accountID);
        List<SiteSettingItem> LoadSiteSettingsInherited(int siteID);
        void SaveSiteSetting(int siteID, string settingName, string settingValue);
        List<Site> LoadByBaseSiteID(int baseSiteId);
        List<Site> LoadBaseSites();
        List<Site> LoadBaseSites(int marketID, int userID);
        Site FindCorporateSite(int marketID);
        Site LoadBaseSiteForNewPWS(int marketID);
        Site LoadSiteWithSiteURLs(int siteID);
        string LoadBackOfficeURL(int siteid);
        Site LoadByAutoshipOrderID(int autoshipOrderID);
        IList<int> QuerySitesBySiteTypeId(int siteTypeId);
        Site LoadSiteForCache(int siteID);
        T LoadByUrl<T>(string url, Func<Site, T> selector);
        Site LoadFullClone(int siteID);
        IEnumerable<Page> LoadPages(int siteID);
        Site LoadFullWithoutContent(int siteID);

        Site SiteWithNewsAndArchive(int siteID);
        Site SiteWithLanguages(int siteID);
        Site SiteWithNews(int siteID);
        Site SiteWithSiteMap(int siteID);
		List<Site> GetOtherBaseSites(Site baseSite);
		int? GetSiteId(string url);

        Site FirstOrDefaultFull(Expression<Func<Site, bool>> predicate);
        Site FirstOrDefaultFull(Expression<Func<Site, bool>> predicate, NetStepsEntities context);
        Site FirstOrDefault(Expression<Func<Site, bool>> predicate, Site.Relations relations);
        Site FirstOrDefault(Expression<Func<Site, bool>> predicate, Site.Relations relations, NetStepsEntities context);
        List<Site> WhereFull(Expression<Func<Site, bool>> predicate);
        List<Site> WhereFull(Expression<Func<Site, bool>> predicate, NetStepsEntities context);
        List<Site> Where(Expression<Func<Site, bool>> predicate, Site.Relations relations);
        List<Site> Where(Expression<Func<Site, bool>> predicate, Site.Relations relations, NetStepsEntities context);
    }

	namespace Contracts
	{
		[ContractClassFor(typeof(ISiteRepository))]
		abstract class SiteRepositoryContracts : ISiteRepository
		{
			public List<Site> LoadByAccountID(int accountID)
			{
				throw new NotImplementedException();
			}

			public List<SiteSettingItem> LoadSiteSettingsInherited(int siteID)
			{
				throw new NotImplementedException();
			}

			public void SaveSiteSetting(int siteID, string settingName, string settingValue)
			{
				throw new NotImplementedException();
			}

			public List<Site> LoadByBaseSiteID(int baseSiteId)
			{
				throw new NotImplementedException();
			}

			public List<Site> LoadBaseSites()
			{
				throw new NotImplementedException();
			}

			public List<Site> LoadBaseSites(int marketID, int userID)
			{
				throw new NotImplementedException();
			}

			public Site FindCorporateSite(int marketID)
			{
				Contract.Requires<ArgumentOutOfRangeException>(marketID > 0);

				throw new NotImplementedException();
			}

			public Site LoadBaseSiteForNewPWS(int marketID)
			{
				throw new NotImplementedException();
			}

			public Site LoadSiteWithSiteURLs(int siteID)
			{
				throw new NotImplementedException();
			}

			public string LoadBackOfficeURL(int siteid)
			{
				throw new NotImplementedException();
			}

			public Site LoadByAutoshipOrderID(int autoshipOrderID)
			{
				throw new NotImplementedException();
			}

			public IList<int> QuerySitesBySiteTypeId(int siteTypeId)
			{
				throw new NotImplementedException();
			}

			public Site LoadSiteForCache(int siteID)
			{
				Contract.Requires<ArgumentOutOfRangeException>(siteID > 0);

				throw new NotImplementedException();
			}

			public T LoadByUrl<T>(string url, Func<Site, T> selector)
			{
				throw new NotImplementedException();
			}

			public Site LoadFullClone(int siteID)
			{
				throw new NotImplementedException();
			}

			public IEnumerable<Page> LoadPages(int siteID)
			{
				throw new NotImplementedException();
			}

			public Site LoadFullWithoutContent(int siteID)
			{
				throw new NotImplementedException();
			}

			public Site SiteWithNewsAndArchive(int siteID)
			{
				throw new NotImplementedException();
			}

			public Site SiteWithLanguages(int siteID)
			{
				throw new NotImplementedException();
			}

			public Site SiteWithNews(int siteID)
			{
				throw new NotImplementedException();
			}

			public Site SiteWithSiteMap(int siteID)
			{
				throw new NotImplementedException();
			}

			public List<Site> GetOtherBaseSites(Site baseSite)
			{
				Contract.Requires<ArgumentNullException>(baseSite != null);

				throw new NotImplementedException();
			}

			public int? GetSiteId(string url)
			{
				throw new NotImplementedException();
			}

			public Site FirstOrDefaultFull(Expression<Func<Site, bool>> predicate)
			{
				throw new NotImplementedException();
			}

			public Site FirstOrDefaultFull(Expression<Func<Site, bool>> predicate, NetStepsEntities context)
			{
				Contract.Requires<ArgumentNullException>(predicate != null);
				Contract.Requires<ArgumentNullException>(context != null);

				throw new NotImplementedException();
			}

			public Site FirstOrDefault(Expression<Func<Site, bool>> predicate, Site.Relations relations)
			{
				Contract.Requires<ArgumentNullException>(predicate != null);

				throw new NotImplementedException();
			}

			public Site FirstOrDefault(Expression<Func<Site, bool>> predicate, Site.Relations relations, NetStepsEntities context)
			{
				Contract.Requires<ArgumentNullException>(predicate != null);
				Contract.Requires<ArgumentNullException>(context != null);

				throw new NotImplementedException();
			}

			public List<Site> WhereFull(Expression<Func<Site, bool>> predicate)
			{
				Contract.Requires<ArgumentNullException>(predicate != null);

				throw new NotImplementedException();
			}

			public List<Site> WhereFull(Expression<Func<Site, bool>> predicate, NetStepsEntities context)
			{
				Contract.Requires<ArgumentNullException>(predicate != null);
				Contract.Requires<ArgumentNullException>(context != null);

				throw new NotImplementedException();
			}

			public List<Site> Where(Expression<Func<Site, bool>> predicate, Site.Relations relations)
			{
				Contract.Requires<ArgumentNullException>(predicate != null);

				throw new NotImplementedException();
			}

			public List<Site> Where(Expression<Func<Site, bool>> predicate, Site.Relations relations, NetStepsEntities context)
			{
				Contract.Requires<ArgumentNullException>(predicate != null);
				Contract.Requires<ArgumentNullException>(context != null);

				throw new NotImplementedException();
			}

			public NetSteps.Common.PrimaryKeyInfo PrimaryKeyInfo
			{
				get { throw new NotImplementedException(); }
			}

			public Site Load(int primaryKey)
			{
				throw new NotImplementedException();
			}

			public Site LoadFull(int primaryKey)
			{
				throw new NotImplementedException();
			}

			public List<Site> LoadAll()
			{
				throw new NotImplementedException();
			}

			public List<Site> LoadAllFull()
			{
				throw new NotImplementedException();
			}

			public List<Site> LoadBatch(IEnumerable<int> primaryKeys)
			{
				throw new NotImplementedException();
			}

			public List<Site> LoadBatchFull(IEnumerable<int> primaryKeys)
			{
				throw new NotImplementedException();
			}

			public SqlUpdatableList<Site> LoadAllFullWithSqlDependency()
			{
				throw new NotImplementedException();
			}

			public SqlUpdatableList<Site> LoadBatchWithSqlDependency(IEnumerable<int> primaryKeys)
			{
				throw new NotImplementedException();
			}

			public void Save(Site obj)
			{
				throw new NotImplementedException();
			}

			public void SaveBatch(IEnumerable<Site> items)
			{
				throw new NotImplementedException();
			}

			public void Delete(Site obj)
			{
				throw new NotImplementedException();
			}

			public void Delete(int primaryKey)
			{
				throw new NotImplementedException();
			}

			public void DeleteBatch(IEnumerable<int> primaryKeys)
			{
				throw new NotImplementedException();
			}

			public bool Exists(int primaryKey)
			{
				throw new NotImplementedException();
			}

			public PaginatedList<AuditLogRow> GetAuditLog(int primaryKey, AuditLogSearchParameters param)
			{
				throw new NotImplementedException();
			}

			public Site GetRandomRecord()
			{
				throw new NotImplementedException();
			}

			public Site GetRandomRecordFull()
			{
				throw new NotImplementedException();
			}

			public int Count()
			{
				throw new NotImplementedException();
			}

			public int Count(Expression<Func<Site, bool>> predicate)
			{
				throw new NotImplementedException();
			}

			public bool Any()
			{
				throw new NotImplementedException();
			}

			public bool Any(Expression<Func<Site, bool>> predicate)
			{
				throw new NotImplementedException();
			}

			public List<Site> Where(Expression<Func<Site, bool>> predicate)
			{
				throw new NotImplementedException();
			}

			public List<Site> Where(Expression<Func<Site, bool>> predicate, IEnumerable<string> includes)
			{
				throw new NotImplementedException();
			}

			public List<TSelected> WhereSelect<TSelected>(Expression<Func<Site, bool>> predicate, Expression<Func<Site, TSelected>> selector)
			{
				throw new NotImplementedException();
			}

			public Site FirstOrDefault(Expression<Func<Site, bool>> predicate)
			{
				throw new NotImplementedException();
			}

			public Site FirstOrDefault(Expression<Func<Site, bool>> predicate, IEnumerable<string> includes)
			{
				throw new NotImplementedException();
			}

			public TSelected FirstOrDefaultSelect<TSelected>(Expression<Func<Site, bool>> predicate, Expression<Func<Site, TSelected>> selector)
			{
				throw new NotImplementedException();
			}

			public PaginatedList<SiteSearchData> Search(SiteSearchParameters searchParams)
			{
				throw new NotImplementedException();
			}
		}
	}
}