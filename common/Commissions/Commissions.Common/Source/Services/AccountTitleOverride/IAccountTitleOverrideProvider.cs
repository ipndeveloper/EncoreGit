﻿using NetSteps.Commissions.Common.Models;
using NetSteps.Core.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Services.AccountTitleOverride
{
	/// <summary>
	/// 
	/// </summary>
	public interface IAccountTitleOverrideProvider : ICache<int, IAccountTitleOverride>
	{
		/// <summary>
		/// Adds the override.
		/// </summary>
		/// <param name="accountTitleOverride">The account title override.</param>
		/// <returns></returns>
		IAccountTitleOverride AddOverride(IAccountTitleOverride accountTitleOverride);

		/// <summary>
		/// Updates the override.
		/// </summary>
		/// <param name="accountTitleOverride">The account title override.</param>
		/// <returns></returns>
		IAccountTitleOverride UpdateOverride(IAccountTitleOverride accountTitleOverride);

		/// <summary>
		/// Deletes the override.
		/// </summary>
		/// <param name="accountTitleOverrideId">The account title override identifier.</param>
		/// <returns></returns>
		bool DeleteOverride(int accountTitleOverrideId);
	}
}
