using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Interfaces.TitleKind;
using NetSteps.Core.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Service.TitleKinds
{
	public class TitleKindProvider : ActiveLocalMemoryCachedListBase<ITitleKind>, ITitleKindProvider
	{
		protected readonly ITitleKindRepository _repository;
		public TitleKindProvider(ITitleKindRepository repository)
		{
			_repository = repository;
		}

		protected override IList<ITitleKind> PerformRefresh()
		{
			return _repository.FetchAll();
		}
	}
}
