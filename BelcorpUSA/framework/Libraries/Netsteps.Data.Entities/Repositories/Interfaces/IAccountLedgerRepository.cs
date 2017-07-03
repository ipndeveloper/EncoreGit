using System;
using System.Collections.Generic;
using NetSteps.Data.Entities.Commissions;

namespace NetSteps.Data.Entities.Repositories
{
	public partial interface IAccountLedgerRepository
	{
		List<AccountLedger> LoadByAccountID(int accountID);
		decimal GetBalance(int accountID, DateTime? effectiveDate, int? entryID);
	}
}
