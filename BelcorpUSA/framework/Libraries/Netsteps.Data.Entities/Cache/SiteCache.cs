using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Extensions;
using NetSteps.Core.Cache;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Data.Entities.Cache
{
	/// <summary>
	/// Class to Cache the loaded sites.
	/// </summary>
	public static class SiteCache
	{
        [System.Serializable]
		public class SiteUrlExistsValue
		{
			public int? SiteID { get; set; }
		}

		// Resolvers
		private class SiteResolveByID : DemuxCacheItemResolver<int, Site>
		{
			protected override bool DemultiplexedTryResolve(int siteID, out Site value)
			{
				value = GetSiteValue(siteID);
				return value != null;
			}
		}

		private class SitesResolveByTypeID : DemuxCacheItemResolver<int, IList<int>>
		{
			protected override bool DemultiplexedTryResolve(int siteTypeID, out IList<int> value)
			{
				value = GetSitesByTypeIdValues(siteTypeID);
				return value != null;
			}
		}

		private class SitesResolveByUrl : DemuxCacheItemResolver<string, SiteUrlExistsValue>
		{
			protected override bool DemultiplexedTryResolve(string key, out SiteUrlExistsValue value)
			{
				value = GetSiteUrlExistsValue(key);
				return true;
			}
		}

		private class SiteTypeIdResolveByUrl : DemuxCacheItemResolver<string, SiteTypeIdWrapper>
		{
			protected override bool DemultiplexedTryResolve(string key, out SiteTypeIdWrapper value)
			{
				var siteId = GetSiteUrlExistsValue(key).SiteID;
				var wrapper = new SiteTypeIdWrapper();
				if (siteId.HasValue && siteId.Value != 0)
				{
					wrapper.SiteTypeId = Site.Load(siteId.Value).SiteTypeID;
				}

				value = wrapper;
				return true;
			}
		}

		[System.Serializable]
		private class SiteTypeIdWrapper
		{
			public short? SiteTypeId { get; set; }
		}

		// Data access helper methods.
		public static SiteUrlExistsValue GetSiteUrlExistsValue(string key)
		{
			return new SiteUrlExistsValue { SiteID = Create.New<ISiteRepository>().GetSiteId(key) };
		}

		public static Site GetSiteValue(int siteId)
		{
			return Site.LoadSiteForCache(siteId);
		}

		public static IList<int> GetSitesByTypeIdValues(int siteTypeId)
		{
			return Create.New<ISiteRepository>().QuerySitesBySiteTypeId(siteTypeId);
		}

		static readonly ICache<int, Site> __sites = new ActiveMruLocalMemoryCache<int, Site>("SitesByID", new SiteResolveByID());
		static readonly ICache<int, IList<int>> __sitesByTypeId = new ActiveMruLocalMemoryCache<int, IList<int>>("SitesByTypeId", new SitesResolveByTypeID());
		static readonly ICache<string, SiteUrlExistsValue> __siteByUrl = new ActiveMruLocalMemoryCache<string, SiteUrlExistsValue>("SiteByUrl", new SitesResolveByUrl());
		static readonly ICache<string, SiteTypeIdWrapper> __siteTypeIdByUrl = new ActiveMruLocalMemoryCache<string, SiteTypeIdWrapper>("SitesTypeIDByUrl", new MruCacheOptions { CacheItemLifespan = System.TimeSpan.FromMinutes(5) }, new SiteTypeIdResolveByUrl());

		public static Site GetSiteByUrl(string url)
		{
			SiteUrlExistsValue result;
			url = url.AppendForwardSlash().ToLower();

			__siteByUrl.TryGet(url, out result);

			Site site = null;
			if (result != null && result.SiteID.HasValue)
			{
				site = GetSiteByID(result.SiteID.Value);
			}
			return site;
		}

		public static Site GetSiteByID(int siteID)
		{
			Site result;

			__sites.TryGet(siteID, out result);

			return result;
		}

		public static IList<Site> GetSitesByTypeId(int siteTypeId)
		{
			IList<int> results;

			__sitesByTypeId.TryGet(siteTypeId, out results);

			return results.Select(GetSiteByID).ToList();
		}

		public static short? GetSiteTypeIdByUrl(string url)
		{
			SiteTypeIdWrapper wrapper;
			__siteTypeIdByUrl.TryGet(url, out wrapper);
			return wrapper.SiteTypeId;
		}

		public static void ExpireSite(int siteId)
		{
			Site removed;
			__sites.TryRemove(siteId, out removed);
		}
	}
}