using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Commissions.Service.Interfaces.AccountTitles;
using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Interfaces.Period;
using NetSteps.Commissions.Service.Models;

namespace NetSteps.Commissions.Service.AccountTitles
{
	public class AccountTitleService : IAccountTitleService
	{
		protected readonly IAccountTitleProvider Provider;
		private readonly IPeriodService _periodService;

		public AccountTitleService(IAccountTitleProvider provider, IPeriodService periodService)
		{
			Provider = provider;
			_periodService = periodService;
		}

		public IAccountTitle GetAccountTitle(int accountId, int titleKindId)
		{
			return this.GetAccountTitle(accountId, titleKindId, null);
		}

		public IAccountTitle GetAccountTitle(int accountId, int titleKindId, int? periodId)
		{
			var acctTitles = Provider.GetAccountTitles(accountId);
			if (periodId.HasValue)
			{
				return acctTitles.FirstOrDefault(x => x.TitleKindId == titleKindId && x.PeriodId == periodId.Value);
			}
			else
			{
				return acctTitles.OrderByDescending(at => at.PeriodId).FirstOrDefault(x => x.TitleKindId == titleKindId);
			}
		}

		public IEnumerable<IAccountTitle> GetAccountTitles(int accountId)
		{
			return this.GetAccountTitles(accountId, null);
		}

		public IEnumerable<IAccountTitle> GetAccountTitles(int accountId, int? periodId)
		{
			var acctTitles = Provider.GetAccountTitles(accountId);
			if (periodId.HasValue)
			{
				return acctTitles.Where(x => x.PeriodId == periodId.Value);
			}
			else
			{
				return acctTitles;
			}
		}

		public IEnumerable<IAccountTitle> GetCurrentAccountTitles(int? periodId)
		{
			if (periodId.HasValue)
			{
				return Provider.GetAllAccountTitles(periodId.Value);
			}
			else
			{
				return Provider.GetAllAccountTitles();
			}
		}
	}
}
