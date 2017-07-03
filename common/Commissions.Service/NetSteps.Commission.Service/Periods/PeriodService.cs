using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Interfaces.Period;
using NetSteps.Commissions.Service.Base;
using NetSteps.Common.Configuration;
using NetSteps.Core.Cache;
using NetSteps.Foundation.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NetSteps.Commissions.Service.Periods
{
	public class PeriodService : IPeriodService
	{
		protected readonly IPeriodProvider _provider;
		public PeriodService(IPeriodProvider provider)
		{
			_provider = provider;
		}

		public IPeriod GetPeriod(int periodID)
		{
			return _provider.SingleOrDefault(x => x.PeriodId == periodID);
		}

		public IEnumerable<IPeriod> GetCurrentPeriods()
		{
			return _provider.Where(x => x.StartDateUTC <= DateTime.UtcNow && x.EndDateUTC > DateTime.UtcNow);
		}

		public IPeriod GetCurrentPeriod(DisbursementFrequencyKind disbursementFrequency)
		{
			return _provider.SingleOrDefault(x => x.StartDateUTC <= DateTime.UtcNow && x.EndDateUTC > DateTime.UtcNow && x.DisbursementFrequency == disbursementFrequency);
		}

		public IEnumerable<IPeriod> GetOpenPeriods()
		{
			return _provider.Where(x => !x.ClosedDateUTC.HasValue);
		}

		public IEnumerable<IPeriod> GetOpenPeriods(DisbursementFrequencyKind disbursementFrequency)
		{
			return _provider.Where(x => !x.ClosedDateUTC.HasValue && x.DisbursementFrequency == disbursementFrequency);
		}

		public IEnumerable<IPeriod> GetPeriods(Predicate<IPeriod> filter)
		{
			return _provider.Where(x => filter(x));
		}


		public IEnumerable<IPeriod> GetPeriodsForAccount(int accountID)
		{
			return _provider.GetPeriodsForAccount(accountID);
		}
	}
}
