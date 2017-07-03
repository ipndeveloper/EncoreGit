using NetSteps.Commissions.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Service.Interfaces.AccountTitles
{
    public interface IAccountTitleProvider
    {
		IEnumerable<IAccountTitle> GetAccountTitles(int accountId);

		IEnumerable<IAccountTitle> GetAllAccountTitles();

		IEnumerable<IAccountTitle> GetAllAccountTitles(int periodId);
    }
}
