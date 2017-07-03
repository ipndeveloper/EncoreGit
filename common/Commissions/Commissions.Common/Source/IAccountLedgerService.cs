using NetSteps.Commissions.Common.Base;
using NetSteps.Commissions.Common.Models;

namespace NetSteps.Commissions.Common
{
	/// <summary>
	/// Service that manages the primary Account Ledger
	/// </summary>
	public interface IAccountLedgerService : IBaseLedgerService<IAccountLedgerEntry>
	{
	}
}
