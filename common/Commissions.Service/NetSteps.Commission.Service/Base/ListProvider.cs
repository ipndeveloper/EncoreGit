using NetSteps.Commissions.Common.Base;
using NetSteps.Core.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Service.Base
{
	public class ListProvider<TObject, TKey> : ActiveLocalMemoryCachedListBase<TObject>
	{
		protected readonly IRepository<TObject, TKey> _provider;
		public ListProvider(IRepository<TObject, TKey> provider)
		{
			_provider = provider;
		}

		protected override IList<TObject> PerformRefresh()
		{
			return _provider.FetchAll();
		}
	}
}
