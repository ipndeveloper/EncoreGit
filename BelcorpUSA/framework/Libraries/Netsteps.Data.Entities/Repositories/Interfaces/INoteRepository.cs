using System.Collections.Generic;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business;

namespace NetSteps.Data.Entities.Repositories
{
	public partial interface INoteRepository
	{
		List<Note> LoadByOrderNumber(string orderNumber);
		List<Note> LoadByOrderID(int orderID);
		List<Note> LoadByAccountNumber(string accountNumber);
		List<Note> LoadByAccountID(int accountID);
		List<Note> LoadBySupportTicketID(int supportTicketID);

		PaginatedList<NoteSearchData> Search(NoteSearchParameters searchParameters);
	}
}
