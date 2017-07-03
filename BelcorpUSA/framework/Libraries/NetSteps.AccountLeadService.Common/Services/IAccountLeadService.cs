using System;
using System.Diagnostics.Contracts;

namespace NetSteps.AccountLeadService.Common
{
	[ContractClass(typeof(Contracts.AccountLeadServiceContracts))]
	public interface IAccountLeadService
	{
		/// <summary>
		/// Gets the lead count for the specified account.
		/// </summary>
		/// <param name="accountId">The account to get the lead count for.</param>
		/// <returns>The lead count for the account (if the account hasn't recorded any leads yet, 0 will be returned).</returns>
		int GetLeadCount(int accountId);

		/// <summary>
		/// Sets the lead count for the specified account.
		/// </summary>
		/// <param name="accountId">The account to set the lead count for.</param>
		/// <param name="amount">The amount to set the lead count at.</param>
		void SetLeadCount(int accountId, int amount);

		/// <summary>
		/// Adds the specified amount to the lead count of the specified account.
		/// </summary>
		/// <param name="accountId">The account to increment the lead count for.</param>
		/// <param name="amount">The amount to increment the lead count by.</param>
		void IncrementLeadCount(int accountId, int amount = 1);
	}

	namespace Contracts
	{
		[ContractClassFor(typeof(IAccountLeadService))]
		abstract class AccountLeadServiceContracts : IAccountLeadService
		{
			public int GetLeadCount(int accountId)
			{
				Contract.Requires<ArgumentException>(accountId > 0);

				throw new System.NotImplementedException();
			}

			public void SetLeadCount(int accountId, int amount)
			{
				Contract.Requires<ArgumentException>(accountId > 0);
				Contract.Requires<ArgumentException>(amount >= 0);

				throw new System.NotImplementedException();
			}

			public void IncrementLeadCount(int accountId, int amount)
			{
				Contract.Requires<ArgumentException>(accountId > 0);
				Contract.Requires<ArgumentException>(amount > 0);

				throw new System.NotImplementedException();
			}
		}
	}
}
