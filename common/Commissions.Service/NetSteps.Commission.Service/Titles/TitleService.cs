using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Interfaces.Title;
using NetSteps.Commissions.Service.Base;
using NetSteps.Core.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Service.Titles
{
	public class TitleService : ITitleService
	{
		protected ITitleProvider _provider;
		public TitleService(ITitleProvider provider)
		{
			_provider = provider;
		}

		public ITitle GetTitle(string titleName)
		{
			return _provider.SingleOrDefault(x => x.TitleName == titleName);
		}

		public ITitle GetTitle(int titleId)
		{
			return _provider.SingleOrDefault(x => x.TitleId == titleId);
		}

		public IEnumerable<ITitle> GetTitles(Predicate<ITitle> filter)
		{
			return _provider.Where(x => filter(x));
		}

		public IEnumerable<ITitle> GetTitles()
		{
			return _provider.ToArray();
		}


        public IEnumerable<ITitle> GetFromReportByPeriod(int periodId, int accountId)
        {
            return _provider.GetFromReportByPeriod(periodId, accountId);
        }
    }
}
