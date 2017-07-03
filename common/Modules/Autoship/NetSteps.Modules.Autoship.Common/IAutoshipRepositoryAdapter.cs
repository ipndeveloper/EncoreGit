using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Modules.Autoship.Common;

namespace NetSteps.Modules.Autoship.Common
{
	/// <summary>
	/// Adapter for performing functions for autoships.
	/// </summary>
	public interface IAutoshipRepositoryAdapter
	{
		/// <summary>
		/// Search for autoships of a specific schedule type for an account
		/// </summary>
		/// <param name="accountID"></param>
		/// <param name="autoshipType">AutoshipScheduleID</param>
		/// <param name="ActiveAutoshipsOnly">ActiveAutoshipsOnly</param>
		/// <returns></returns>
		List<ISiteAutoship> SearchByAccount(int accountID, int? autoshipType, bool ActiveAutoshipsOnly = true);
		/// <summary>
		/// Cancel an autoship under a given account
		/// </summary>
		/// <param name="accountID"></param>
		/// <param name="autoshipID"></param>
		/// <returns></returns>
		ISiteAutoship CancelByAccountIDAndAutoshipID(int accountID, int autoshipID);
	}
}
