using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Base;
using NetSteps.Commissions.Service.Interfaces.CalculationOverride;
using NetSteps.Core.Cache;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Service.CalculationOverrides
{
	public class CalculationOverrideProvider : CommissionsActiveMruCacheAdapter<int, ICalculationOverride>, ICalculationOverrideProvider
	{
		private readonly ICalculationOverrideRepository _repository;

		public CalculationOverrideProvider(ICalculationOverrideRepository repository)
		{
			_repository = repository;
		}

		protected override ICache<int, ICalculationOverride> InitializeCache()
		{
			return new ActiveMruLocalMemoryCache<int, ICalculationOverride>("CalculationOverride_OverrideItems", new DelegatedDemuxCacheItemResolver<int, ICalculationOverride>(GetOverrideItem));
		}

		private bool GetOverrideItem(int key, out ICalculationOverride value)
		{
			value = _repository.Fetch(key);
			return value != null;
		}


		public ICalculationOverride AddCalculationOverride(ICalculationOverride calculationOverride)
		{
			var added = _repository.Add(calculationOverride);
			return this.Get(added.CalculationOverrideId);
		}

		public bool DeleteCalculationOverride(int calculationOverrideId)
		{
			ICalculationOverride removed;
			this.TryRemove(calculationOverrideId, out removed);
			return _repository.Delete(calculationOverrideId);
		}


		public ICalculationOverrideSearchResult SearchCalculationOverrides(CalculationOverrideSearchParameters parameters)
		{
			return _repository.SearchCalculationOverrides(parameters);
		}
	}
}
