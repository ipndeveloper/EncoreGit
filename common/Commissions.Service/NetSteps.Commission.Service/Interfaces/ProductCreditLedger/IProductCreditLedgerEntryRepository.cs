using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Base;
using System.Collections.Generic;

namespace NetSteps.Commissions.Service.Interfaces.ProductCreditLedger
{
	/// <summary>
	/// 
	/// </summary>
	public interface IProductCreditLedgerEntryRepository : IRepository<IProductCreditLedgerEntry, int>
	{
		/// <summary>
		/// Gets the account ledger entry ids.
		/// </summary>
		/// <param name="accountId">The account identifier.</param>
		/// <returns></returns>
		IEnumerable<int> GetProductCreditLedgerEntryIds(int accountId);
	}
}
