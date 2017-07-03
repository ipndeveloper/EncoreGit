using System.Collections.Generic;

namespace NetSteps.Commissions.Common
{
	/// <summary>
	/// Service contract for downline
	/// </summary>
	public interface IDownlineService
	{
		/// <summary>
		/// Get the full downline for the requested periodId
		/// </summary>
		/// <param name="periodId"></param>
		/// <returns></returns>
		IEnumerable<dynamic> GetDownline(int periodId);

        /// <summary>
        /// Get the full downline for the requested period and account
        /// </summary>
        /// <param name="periodId">Period ID</param>
        /// <param name="sponsorID">Account ID</param>
        /// <returns></returns>
        IEnumerable<dynamic> GetDownline(int periodId, int sponsorID);

		/// <summary>
		/// Get the single layer downline for the downlineId and periodId
		/// </summary>
		/// <param name="downlineId"></param>
		/// <param name="periodId"></param>
		/// <returns></returns>
		IEnumerable<dynamic> GetSingleLayerDownline(int downlineId, int periodId);

		/// <summary>
		/// Get the simple downline
		/// </summary>
		/// <param name="periodId"></param>
		/// <returns></returns>
		IEnumerable<dynamic> GetSimpleDownline(int periodId);
	}
}
