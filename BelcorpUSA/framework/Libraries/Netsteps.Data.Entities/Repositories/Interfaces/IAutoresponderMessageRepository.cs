using System.Collections.Generic;

namespace NetSteps.Data.Entities.Repositories
{
	public partial interface IAutoresponderMessageRepository
	{
		List<AutoresponderMessage> GetUnseenMessagesForAccount(int accountID);
	}
}
