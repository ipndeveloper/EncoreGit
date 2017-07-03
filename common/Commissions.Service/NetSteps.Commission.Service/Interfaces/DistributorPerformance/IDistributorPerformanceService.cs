using NetSteps.Commissions.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Service.Interfaces.DistributorPerformance
{
	public interface IDistributorPerformanceService
	{
		IDistributorPeriodPerformanceData GetDistributorPerformanceData(int accountId, int periodId);
		
		IEnumerable<IAccountKPI> GetDistributorPerformanceOverviewData(int accountId, int periodId);

		IEnumerable<IBonusPayout> GetDistributorBonusData(int accountId, int periodId);

        IEnumerable<IEarningReport> GetEarningRerportData(int accountId, int periodId);

        /// <summary>
        /// Get Data From Report.AccountKPIsDetails
        /// </summary>
        /// <param name="periodId">Period ID</param>
        /// <param name="accountId">Account ID</param>
        /// <param name="kPICode">KPI Code</param>
        /// <returns></returns>
        IEnumerable<IReportAccountKPIDetail> GetReportAccountKPIDetails(int periodId, int accountId, string kPICode);

        /// <summary>
        /// Get Data From Report.AccountKPIsDetails for Legs
        /// </summary>
        /// <param name="periodId">Period Id</param>
        /// <param name="accountId">Account ID</param>
        /// <param name="kPICode">KPI Code</param>
        /// <returns></returns>
        IEnumerable<IReportAccountKPIDetail> GetReportAccountKPIDetailsLegs(int periodId, int accountId, string kPICode);

        /// <summary>
        /// Get Data From Report.BonusDetail
        /// </summary>
        /// <param name="periodId">Period ID</param>
        /// <param name="accountId">Account ID</param>
        /// <param name="bonusCode">Bonus Code</param>
        /// <returns></returns>
        IEnumerable<IReportBonusDetail> GetReportBonusDetail(int periodId, int accountId, string bonusCode);

        /// <summary>
        /// Gets a total earnings for period and year
        /// </summary>
        /// <param name="periodId">period ID</param>
        /// <param name="accountId">Account ID</param>
        /// <param name="totalPeriodEarnings">Out total period earnings</param>
        /// <param name="totalYearEanings">Out total year earnings</param>
        void GetEarningsAmountOnly(int periodId, int accountId, out decimal? totalPeriodEarnings, out decimal? totalYearEanings);
	}
}
