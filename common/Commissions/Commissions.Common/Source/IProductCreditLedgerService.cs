using NetSteps.Commissions.Common.Base;
using NetSteps.Commissions.Common.Models;

namespace NetSteps.Commissions.Common
{
	/// <summary>
	/// Product Credit Ledger Service
	/// </summary>
	//[ContractClass(typeof(IProductCreditLedgerServiceContracts))]
	public interface IProductCreditLedgerService : IBaseLedgerService<IProductCreditLedgerEntry>
	{
	}

	// <summary>
	// Contract class for IProductCreditLedgerService
	// </summary>
	//[ContractClassFor(typeof(IProductCreditLedgerService))]
	//public abstract class IProductCreditLedgerServiceContracts : IProductCreditLedgerService
}
