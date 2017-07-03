using System.Linq;
using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Base;
using NetSteps.Commissions.Service.Interfaces.DisbursementKinds;
using NetSteps.Commissions.Service.Interfaces.DisbursementProfile;
using NetSteps.Core.Cache;
using System.Collections.Generic;

namespace NetSteps.Commissions.Service.DisbursementProfiles
{
	public class DisbursementProfileProvider : CommissionsActiveMruCacheAdapter<int, IDisbursementProfile>, IDisbursementProfileProvider
	{
		private readonly ICache<int, IEnumerable<int>> _disbursementProfileAccountIds;
		private readonly IDisbursementKindService _disbursementKindService;

		protected readonly IDisbursementProfileRepository Repository;
		public DisbursementProfileProvider(IDisbursementProfileRepository repository, IDisbursementKindService disbursementKindService)
		{
			Repository = repository;
			_disbursementKindService = disbursementKindService;
			_disbursementProfileAccountIds = new ActiveMruLocalMemoryCache<int, IEnumerable<int>>("DisbursementProfile_AccountIds", new DelegatedDemuxCacheItemResolver<int, IEnumerable<int>>(GetDisbursementProfileIds));
		}

		protected override ICache<int, IDisbursementProfile> InitializeCache()
		{
			return new ActiveMruLocalMemoryCache<int, IDisbursementProfile>("DisbursementProfile_Items", new DelegatedDemuxCacheItemResolver<int, IDisbursementProfile>(GetItem));
		}

		private bool GetDisbursementProfileIds(int accountId, out IEnumerable<int> itemIds)
		{
			itemIds = Repository.GetDisbursementProfileIds(accountId);
			return (itemIds != null && itemIds.Any());
		}

		private bool GetItem(int key, out IDisbursementProfile value)
		{
			value = Repository.Fetch(key);
			return value != null;
		}

		public IEnumerable<IDisbursementProfile> GetDisbursementsProfilesForAccount(int accountId)
		{
			IEnumerable<int> entryIds;
			var getSuceeded = _disbursementProfileAccountIds.TryGet(accountId, out entryIds);
			return getSuceeded ? entryIds.Select(x => Cache.Get(x)) : new List<IDisbursementProfile>();
		}

		public int GetMaximumDisbursementProfiles(DisbursementMethodKind method)
		{
			var disbursementKind = _disbursementKindService.GetDisbursementKind(method);
			return disbursementKind != null ? disbursementKind.NumberAllowed : 1;
		}

		public IDisbursementProfile SaveDisbursementProfile(IDisbursementProfile profile)
		{
			var savedProfile = Repository.AddOrUpdate(profile);
			if (savedProfile == null)
			{
				return null;
			}

			return this.Get(savedProfile.DisbursementProfileId);
		}


        public string GetDisbursementMethodCode(int disbursementMethodId)
        {
            return _disbursementKindService.GetDisbursementMethodCode(disbursementMethodId);
        }
    }
}
