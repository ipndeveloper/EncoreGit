using System.Collections.Generic;

namespace NetSteps.Data.Entities.Business.Logic.Interfaces
{
	public partial interface INoteBusinessLogic
	{
		List<Note> LoadByOrderNumber(Repositories.INoteRepository repository, string orderNumber);
		List<Note> LoadByOrderID(Repositories.INoteRepository repository, int orderID);
		List<Note> LoadByAccountNumber(Repositories.INoteRepository repository, string accountNumber);
		List<Note> LoadByAccountID(Repositories.INoteRepository repository, int accountID);
		List<Note> LoadBySupportTicketID(Repositories.INoteRepository Repository, int supportTicketID);
	}
}
