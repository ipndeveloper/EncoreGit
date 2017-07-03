using NetSteps.Commissions.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Services.AccountTitleOverride
{
	/// <summary>
	/// 
	/// </summary>
	public interface IAccountTitleOverrideService
	{
		/// <summary>
		/// Searches the account title overrides.
		/// </summary>
		/// <param name="parameters">The parameters.</param>
		/// <returns></returns>
		IEnumerable<IAccountTitleOverride> SearchAccountTitleOverrides(AccountTitleOverrideSearchParameters parameters);

		/// <summary>
		/// Gets the account title override.
		/// </summary>
		/// <param name="overrideId">The override identifier.</param>
		/// <returns></returns>
		IAccountTitleOverride GetAccountTitleOverride(int overrideId);

		/// <summary>
		/// Deletes the account title override.
		/// </summary>
		/// <param name="overrideId">The override identifier.</param>
		/// <returns></returns>
		bool DeleteAccountTitleOverride(int overrideId);

		/// <summary>
		/// Adds the account title override.
		/// </summary>
		/// <param name="accountTitleOverride">The account title override.</param>
		/// <returns></returns>
		IAccountTitleOverride AddAccountTitleOverride(IAccountTitleOverride accountTitleOverride);
	}
}
