using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Modules.Autoship.Common
{
	/// <summary>
	/// Module for performing autoship functions.
	/// </summary>
	public interface IAutoship
	{
		/// <summary>
		/// Search for autoships of a specific schedule for a given account.
		/// </summary>
		/// <param name="accountID"></param>
		/// <param name="autoshipScheduleID"></param>
		/// <param name="ActiveAutoshipsOnly">ActiveAutoshipsOnly</param>
		/// <returns></returns>
		IAutoshipSearchResult Search(int accountID, int? autoshipScheduleID, bool ActiveAutoshipsOnly = true);
		/// <summary>
		/// Cancel an autoship for the given account.
		/// </summary>
		/// <param name="accountID"></param>
		/// <param name="autoshipID"></param>
		/// <returns></returns>
		IAutoshipCancelResult Cancel(int accountID, int autoshipID);
	}
}
