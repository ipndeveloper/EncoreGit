using NetSteps.Commissions.Common.Models;
using System.Collections.Generic;

namespace NetSteps.Commissions.Common
{
    /// <summary>
    /// Service contract for the commissions service.
    /// </summary>
    //[ContractClass(typeof(ICommissionsServiceContracts))]
    public interface ICommissionsService
    {
        /// <summary>
        /// Gets the current period.
        /// </summary>
        /// <returns></returns>
        IPeriod GetCurrentPeriod();

        /// <summary>
        /// Gets the current periods.
        /// </summary>
        /// <returns>collection of current periods</returns>
        IEnumerable<IPeriod> GetCurrentPeriods();

        /// <summary>
        /// Gets the specified period.
        /// </summary>
        /// <param name="periodId">The period identifier.</param>
        /// <returns></returns>
        IPeriod GetPeriod(int periodId);

        /// <summary>
        /// Gets the periods available for the specified distributor account.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <returns></returns>
        IEnumerable<IPeriod> GetPeriodsForAccount(int accountId);

        /// <summary>
        /// Gets current and past periods.
        /// </summary>
        /// <returns></returns>
        IEnumerable<IPeriod> GetCurrentAndPastPeriods();

        /// <summary>
        /// Gets the open periods.
        /// </summary>
        /// <param name="disbursementFrequency">The disbursement frequency.</param>
        /// <returns></returns>
        IEnumerable<IPeriod> GetOpenPeriods(DisbursementFrequencyKind disbursementFrequency);

        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <param name="titleId">The title identifier.</param>
        /// <returns></returns>
        ITitle GetTitle(int titleId);

        /// <summary>
        /// Gets the maximum disbursement profiles.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <returns></returns>
        int GetMaximumDisbursementProfiles(DisbursementMethodKind method);

        /// <summary>
        /// Gets the distributor performance data for a particular period.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <param name="periodId">The period identifier.</param>
        /// <returns></returns>
        IDistributorPeriodPerformanceData GetDistributorPerformanceData(int accountId, int periodId);

        /// <summary>
        /// Gets the distributor performance overview data.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <param name="periodId">The period identifier.</param>
        /// <returns></returns>
        IEnumerable<IAccountKPI> GetDistributorPerformanceOverviewData(int accountId, int periodId);

        /// <summary>
        /// Returns the bonus payouts for an account and period
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <param name="periodId">The period identifier.</param>
        /// <returns></returns>
        IEnumerable<IBonusPayout> GetDistributorBonusData(int accountId, int periodId);

        /// <summary>
        /// Gets the kind of the bonus.
        /// </summary>
        /// <param name="bonusKindId">The bonus kind identifier.</param>
        /// <returns></returns>
        IBonusKind GetBonusKind(int bonusKindId);

        /// <summary>
        /// Gets the kind of the bonus.
        /// </summary>
        /// <param name="bonusCode">The bonus code.</param>
        /// <returns></returns>
        IBonusKind GetBonusKind(string bonusCode);

        /// <summary>
        /// Get all bonus kinds
        /// </summary>
        /// <returns></returns>
        IEnumerable<IBonusKind> GetBonusKinds();

        /// <summary>
        /// Gets the titles.
        /// </summary>
        /// <returns></returns>
        IEnumerable<ITitle> GetTitles();

        /// <summary>
        /// Gets a title kinds
        /// </summary>
        /// <param name="titleKindId">The title kind identifier.</param>
        /// <returns></returns>
        ITitleKind GetTitleKind(int titleKindId);

        /// <summary>
        /// Gets the title kinds
        /// </summary>
        /// <returns></returns>
        IEnumerable<ITitleKind> GetTitleKinds();

        /// <summary>
        /// Gets the disbursement profiles by account identifier.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <returns></returns>
        IEnumerable<IDisbursementProfile> GetDisbursementProfilesByAccountId(int accountId);

        /// <summary>
        /// Gets the type of the disbursement profiles by account and.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <param name="disbursementMethod">The disbursement method.</param>
        /// <returns></returns>
        IEnumerable<IDisbursementProfile> GetDisbursementProfilesByAccountAndDisbursementMethod(int accountId, DisbursementMethodKind disbursementMethod);

        /// <summary>
        /// Gets the profile count by account and disbursement method.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <param name="disbursementMethod">The disbursement method.</param>
        /// <returns></returns>
        int GetDisbursementProfileCountByAccountAndDisbursementMethod(int accountId, DisbursementMethodKind disbursementMethod);

        /// <summary>
        /// Gets the disbursement method code.
        /// </summary>
        /// <param name="disbursementMethodId">The disbursement method identifier.</param>
        /// <returns></returns>
        string GetDisbursementMethodCode(int disbursementMethodId);

        /// <summary>
        /// Save the disbursement profile
        /// </summary>
        /// <param name="disbursementProfile">the profile to save</param>
        /// <returns></returns>
        IDisbursementProfile SaveDisbursementProfile(IDisbursementProfile disbursementProfile);

        /// <summary>
        /// Saves the temporary account to commission.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <param name="accountNumber">The account number.</param>
        /// <param name="sponsorId"></param>
        /// <param name="enrollerId"></param>
        bool SavePartialAccountToCommission(int accountId, string accountNumber, int? sponsorId = null, int? enrollerId = null);

        /// <summary>
        /// Gets the requested account, null if not found
        /// </summary>
        /// <param name="accountId">the account identifier</param>
        /// <returns></returns>
        IAccount GetAccount(int accountId);

        /// <summary>
        /// Save an account to commissions
        /// </summary>
        /// <param name="account">the account to save</param>
        /// <returns></returns>
        IAccount SaveAccount(IAccount account);

        /// <summary>
        /// Gets the disbursement hold.
        /// </summary>
        /// <param name="disbursementHoldId">The disbursement hold identifier.</param>
        /// <returns></returns>
        IDisbursementHold GetDisbursementHold(int disbursementHoldId);

        /// <summary>
        /// Adds the disbursement hold.
        /// </summary>
        /// <param name="disbursementHold">The disbursement hold.</param>
        IDisbursementHold AddDisbursementHold(IDisbursementHold disbursementHold);

        /// <summary>
        /// Gets the commission plans.
        /// </summary>
        /// <returns></returns>
        ICommissionPlan GetCommissionPlan(DisbursementFrequencyKind disbursementFrequencyKind);

        /// <summary>
        /// Gets the commission plans.
        /// </summary>
        /// <returns></returns>
        ICommissionPlan GetCommissionPlan(int commissionPlanId);

        /// <summary>
        /// Gets the commission plans.
        /// </summary>
        /// <returns></returns>
        IEnumerable<ICommissionPlan> GetCommissionPlans();

        /// <summary>
        /// Gets the account title override.
        /// </summary>
        /// <param name="accountTitleOverrideId">The account title override identifier.</param>
        /// <returns></returns>
        IAccountTitleOverride GetAccountTitleOverride(int accountTitleOverrideId);

        /// <summary>
        /// Adds the account title override.
        /// </summary>
        /// <param name="titleOverride">The title override.</param>
        IAccountTitleOverride AddAccountTitleOverride(IAccountTitleOverride titleOverride);

        /// <summary>
        /// Deletes the account title override.
        /// </summary>
        /// <param name="titleOverrideId">The title override identifier.</param>
        bool DeleteAccountTitleOverride(int titleOverrideId);

        /// <summary>
        /// Gets the calculation override.
        /// </summary>
        /// <param name="calculationOverrideId">The calculation override identifier.</param>
        /// <returns></returns>
        ICalculationOverride GetCalculationOverride(int calculationOverrideId);

        /// <summary>
        /// Adds the calculation override.
        /// </summary>
        /// <param name="calculationOverride">The calculation override.</param>
        ICalculationOverride AddCalculationOverride(ICalculationOverride calculationOverride);

        /// <summary>
        /// Deletes the calculation override.
        /// </summary>
        /// <param name="calculationOverrideId">The calculation override identifier.</param>
        bool DeleteCalculationOverride(int calculationOverrideId);

        /// <summary>
        /// Searches the disbursement holds.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        IDisbursementHoldSearchResult SearchDisbursementHolds(DisbursementHoldSearchParameters parameters);

        /// <summary>
        /// Searches the account title overrides.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        IAccountTitleOverrideSearchResult SearchAccountTitleOverrides(AccountTitleOverrideSearchParameters parameters);

        /// <summary>
        /// Searches the calculation overrides.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        ICalculationOverrideSearchResult SearchCalculationOverrides(CalculationOverrideSearchParameters parameters);

        /// <summary>
        /// Gets the override reason sources
        /// </summary>
        /// <returns></returns>
        IEnumerable<IOverrideReasonSource> GetOverrideReasonSources();

        /// <summary>
        /// Gets all override reasons
        /// </summary>
        /// <returns></returns>
        IEnumerable<IOverrideReason> GetOverrideReasons();

        /// <summary>
        /// Gets all override reasons for a particular override reason source
        /// </summary>
        /// <param name="overrideReasonSource">the override reason source</param>
        /// <returns></returns>
        IEnumerable<IOverrideReason> GetOverrideReasonsForSource(IOverrideReasonSource overrideReasonSource);

        /// <summary>
        /// Gets all override reasons for a particular override reason source
        /// </summary>
        /// <param name="overrideReasonSourceId">the override reason source identifier</param>
        /// <returns></returns>
        IEnumerable<IOverrideReason> GetOverrideReasonsForSource(int overrideReasonSourceId);

        /// <summary>
        /// Gets all calculation kinds
        /// </summary>
        /// <returns></returns>
        IEnumerable<ICalculationKind> GetCalculationKinds();

        /// <summary>
        /// Gets all override kinds
        /// </summary>
        /// <returns></returns>
        IEnumerable<IOverrideKind> GetOverrideKinds();

        /// <summary>
        /// Gets the requested account title
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="titleKindId"></param>
        /// <param name="periodId">defaults to the current period</param>
        /// <returns></returns>
        IAccountTitle GetAccountTitle(int accountId, int titleKindId, int? periodId);

        /// <summary>
        /// Gets the requested account titles
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="periodId">defaults to the current period</param>
        /// <returns></returns>
        IEnumerable<IAccountTitle> GetAccountTitles(int accountId, int? periodId);

        /// <summary>
        /// Gets all current AccountTitles for the period supplied, or the current period if null.
        /// </summary>
        /// <param name="periodId"></param>
        /// <returns></returns>
        IEnumerable<IAccountTitle> GetCurrentAccountTitles(int? periodId);

        /// <summary>
        /// Get Earning Data
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="periodId"></param>
        /// <returns></returns>
        IEnumerable<IEarningReport> GetEarningReportData(int accountId, int periodId);

        /// <summary>
        /// Get Carrer and 
        /// </summary>
        /// <param name="periodId"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        IEnumerable<ITitle> GetTitleFromReportByPeriod(int periodId, int accountId);

        /// <summary>
        /// Get Data From Report.BonusDetail
        /// </summary>
        /// <param name="periodId">Period ID</param>
        /// <param name="accountId">Account ID</param>
        /// <param name="bonusCode">Bonus Code</param>
        /// <returns></returns>
        IEnumerable<IReportBonusDetail> GetReportBonusDetail(int periodId, int accountId, string bonusCode);

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
        /// Gets a total earnings for period and year
        /// </summary>
        /// <param name="periodId">period ID</param>
        /// <param name="accountId">Account ID</param>
        /// <param name="totalPeriodEarnings">Out total period earnings</param>
        /// <param name="totalYearEanings">Out total year earnings</param>
        void GetEarningsAmountOnly(int periodId, int accountId, out decimal? totalPeriodEarnings, out decimal? totalYearEanings);
    }
    // <summary>
    // Contracts for the Commissions Service.
    // </summary>
    //[ContractClassFor(typeof(ICommissionsService))]
    //public abstract class ICommissionsServiceContracts : ICommissionsService

}
