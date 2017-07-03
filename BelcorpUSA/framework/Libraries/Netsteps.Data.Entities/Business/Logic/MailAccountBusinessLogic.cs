using System.Collections.Generic;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities.Business.Logic
{
	public partial class MailAccountBusinessLogic
	{
		public virtual MailAccount Authenticate(IMailAccountRepository repository, string email, string password)
		{
			var mailAccount = repository.Authenticate(email, password);
			if (mailAccount != null)
			{
				mailAccount.StartTracking();
				mailAccount.IsLazyLoadingEnabled = true;
			}
			return mailAccount;
		}

		public virtual MailAccount LoadByAccountID(IMailAccountRepository repository, int accountID)
		{
			var mailAccount = repository.LoadByAccountID(accountID);
			if (mailAccount != null)
			{
				mailAccount.StartTracking();
				mailAccount.IsLazyLoadingEnabled = true;
			}
			return mailAccount;
		}

		public virtual List<MailAccountSearchData> LoadSlimBatchByAccountIDs(IMailAccountRepository repository, IEnumerable<int> accountIDs)
		{
			return repository.LoadSlimBatchByAccountIDs(accountIDs);
		}
	}
}
