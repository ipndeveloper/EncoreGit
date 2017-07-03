using System.Collections.Generic;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities.Business.Logic.Interfaces
{
	public partial interface IMailAccountBusinessLogic
	{
		MailAccount Authenticate(IMailAccountRepository repository, string email, string password);
		MailAccount LoadByAccountID(IMailAccountRepository repository, int accountID);
		List<MailAccountSearchData> LoadSlimBatchByAccountIDs(IMailAccountRepository repository, IEnumerable<int> accountIDs);
	}
}
