using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Interfaces.CalculationOverride;
using NetSteps.Core.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Service.CalculationOverrides
{
	public class CalculationOverrideService : ICalculationOverrideService
	{
		protected readonly ICalculationOverrideProvider Provider;
		public CalculationOverrideService(ICalculationOverrideProvider provider)
		{
			Provider = provider;
		}
		public ICalculationOverride AddCalculationOverride(ICalculationOverride calculationOverride)
		{
			return Provider.AddCalculationOverride(calculationOverride);
		}

		public bool DeleteCalculationOverride(int calculationOverrideId)
		{
			return Provider.DeleteCalculationOverride(calculationOverrideId);
		}

		public ICalculationOverride GetCalculationOverride(int calculationOverrideId)
		{
			return Provider.Get(calculationOverrideId);
		}

		public ICalculationOverrideSearchResult SearchCalculationOverrides(CalculationOverrideSearchParameters parameters)
		{
			return Provider.SearchCalculationOverrides(parameters);
		}
	}
}
