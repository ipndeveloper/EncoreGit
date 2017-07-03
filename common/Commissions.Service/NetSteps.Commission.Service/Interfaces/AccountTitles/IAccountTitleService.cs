using NetSteps.Commissions.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Service.Interfaces.AccountTitles
{
    public interface IAccountTitleService
    {
		IAccountTitle GetAccountTitle(int accountId, int titleKindId);
		IAccountTitle GetAccountTitle(int accountId, int titleKindId, int? periodId);

		IEnumerable<IAccountTitle> GetAccountTitles(int accountId);
		IEnumerable<IAccountTitle> GetAccountTitles(int accountId, int? periodId);

		IEnumerable<IAccountTitle> GetCurrentAccountTitles(int? periodId);
    }
}
