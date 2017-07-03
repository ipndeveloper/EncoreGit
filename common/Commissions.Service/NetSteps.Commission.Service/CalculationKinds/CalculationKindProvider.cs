using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Interfaces.CalculationKind;
using NetSteps.Core.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Service.CalculationKinds
{
	public class CalculationKindProvider : ActiveLocalMemoryCachedListBase<ICalculationKind>, ICalculationKindProvider
	{
		protected readonly ICalculationKindRepository _repository;
		public CalculationKindProvider(ICalculationKindRepository repository)
		{
			_repository = repository;
		}

		protected override IList<ICalculationKind> PerformRefresh()
		{
			return _repository.FetchAll();
		}
	}
}
