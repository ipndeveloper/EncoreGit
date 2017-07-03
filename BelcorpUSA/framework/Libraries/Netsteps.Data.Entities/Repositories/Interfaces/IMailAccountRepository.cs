
using System.Collections.Generic;
using NetSteps.Data.Entities.Business;
namespace NetSteps.Data.Entities.Repositories
{
	public partial interface IMailAccountRepository
	{
		MailAccount Authenticate(string email, string password);
		MailAccount LoadByAccountID(int accountID);
		List<MailAccountSearchData> LoadSlimBatchByAccountIDs(IEnumerable<int> accountIDs);
        bool IsAvailable(string emailAddress, int accountID);
        bool IsAvailable(string emailAddress);
        bool IsOtherAvailable(string emailAddress, int accountID);
    }
}
