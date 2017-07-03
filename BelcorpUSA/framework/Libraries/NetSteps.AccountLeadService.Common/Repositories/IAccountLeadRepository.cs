using System;
using System.Diagnostics.Contracts;

namespace NetSteps.AccountLeadService.Common.Repositories
{
	[ContractClass(typeof(Contracts.AccountLeadRepositoryContracts))]
	public interface IAccountLeadRepository
	{
		/// <summary>
		/// Gets the lead count for the specified account.
		/// </summary>
		/// <param name="accountId">The account to get the lead count for.</param>
		/// <returns>The lead count for the account if there are any, otherwise new int?().</returns>
		int? GetLeadCount(int accountId);

		/// <summary>
		/// Sets the lead count for the specified account.
		/// </summary>
		/// <param name="accountId">The account to set the lead count for.</param>
		/// <param name="amount">The amount to set the lead count at.</param>
		void SetLeadCount(int accountId, int amount);
	}

	namespace Contracts
	{
		[ContractClassFor(typeof(IAccountLeadRepository))]
		abstract class AccountLeadRepositoryContracts : IAccountLeadRepository
		{
			public int? GetLeadCount(int accountId)
			{
				Contract.Requires<ArgumentException>(accountId > 0);
				
				throw new NotImplementedException();
			}

			public void SetLeadCount(int accountId, int amount)
			{
				Contract.Requires<ArgumentException>(accountId > 0);
				Contract.Requires<ArgumentException>(amount >= 0);

				throw new NotImplementedException();
			}
		}
	}
}
