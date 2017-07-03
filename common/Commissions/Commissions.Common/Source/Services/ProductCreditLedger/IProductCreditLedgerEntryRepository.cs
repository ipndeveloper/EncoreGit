using NetSteps.Commissions.Common.Base;
using NetSteps.Commissions.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Services.ProductCreditLedger
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
