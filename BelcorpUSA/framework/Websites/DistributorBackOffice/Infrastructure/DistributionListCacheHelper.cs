using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NetSteps.Data.Entities;
using NetSteps.Core.Cache;
using NetSteps.Diagnostics.Utilities;

namespace DistributorBackOffice.Infrastructure
{
	public static class DistributionListCacheHelper
	{
		private static readonly NetSteps.Core.Cache.ICache<int, IEnumerable<DistributionList>> __distributionListByAccountID = new ActiveMruLocalMemoryCache<int, IEnumerable<DistributionList>>("DistributionListByAccountID", new DelegatedDemuxCacheItemResolver<int, IEnumerable<DistributionList>>(DistributionListByAccountIDResolver));

		private static bool DistributionListByAccountIDResolver(int accountID, out IEnumerable<DistributionList> distributionList)
		{
			distributionList = DistributionList.LoadByAccountIDFull(accountID).Where(g => g.Active);
			return distributionList.Any();
		}

		public static IEnumerable<DistributionList> GetDistributionListByAccountID(int accountID)
		{
			var tracer = new object();
			using (var getDistributionListByAccountIDTrace = tracer.TraceActivity(string.Format("DistributionListCacheHelper::GetDistributionListByAccountID accountID {0}", accountID)))
			{
				try
				{
					var list = __distributionListByAccountID.Get(accountID) ?? Enumerable.Empty<DistributionList>();
					return list.Where(g => g.Active);
				}
				catch (Exception excp)
				{
					excp.TraceException(excp);
					throw;
				}
			}
		}

		public static void RemoveDistributionListsForAccountID(int accountID)
		{
			IEnumerable<DistributionList> distributionList;
			__distributionListByAccountID.TryRemove(accountID, out distributionList);
		}
	}
}