using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Interfaces.DisbursementHold;
using NetSteps.Commissions.Service.Base;
using NetSteps.Core.Cache;

namespace NetSteps.Commissions.Service.DisbursementHolds
{
	public class DisbursementHoldProvider : CommissionsActiveMruCacheAdapter<int, IDisbursementHold>, IDisbursementHoldProvider
	{
		private readonly IDisbursementHoldRepository _repository;

		public DisbursementHoldProvider(IDisbursementHoldRepository repository)
		{
			_repository = repository;
		}

		protected override ICache<int, IDisbursementHold> InitializeCache()
		{
			return new ActiveMruLocalMemoryCache<int, IDisbursementHold>("DisbursementHolds_DisbursementHoldItems", new DelegatedDemuxCacheItemResolver<int, IDisbursementHold>(GetDisbursementHoldItem));
		}

		private bool GetDisbursementHoldItem(int key, out IDisbursementHold value)
		{
			value = _repository.Fetch(key);
			return value != null;
		}

		public IDisbursementHold AddDisbursementHold(IDisbursementHold disbursementHold)
		{
			var added = _repository.Add(disbursementHold);
			return (added != null) ? this.Get(added.DisbursementHoldId) : null;
		}

		public bool DeleteDisbursementHold(int disbursementHoldId)
		{
			return _repository.Delete(disbursementHoldId);
		}

		public IDisbursementHoldSearchResult SearchDisbursementHolds(DisbursementHoldSearchParameters parameters)
		{
			return _repository.SearchDisbursementHolds(parameters);
		}
	}
}
