using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Interfaces.Title;
using NetSteps.Core.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Service.Titles
{
	public class TitleProvider : ActiveLocalMemoryCachedListBase<ITitle>, ITitleProvider
	{
		protected readonly ITitleRepository _repository;
		public TitleProvider(ITitleRepository repository)
		{
			_repository = repository;
		}

		protected override IList<ITitle> PerformRefresh()
		{
			return _repository.FetchAll();
		}

        public IEnumerable<ITitle> GetFromReportByPeriod(int periodId, int accountId)
        {
            return _repository.GetFromReportByPeriod(periodId, accountId);
        }
    }
}
