using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Base;
using System.Collections.Generic;

namespace NetSteps.Commissions.Service.Interfaces.AccountTitles
{
    public interface IAccountTitleRepository : IRepository<IAccountTitle, int>
    {
		IEnumerable<IAccountTitle> GetAccountTitlesForPeriod(int periodId);
    }
}
