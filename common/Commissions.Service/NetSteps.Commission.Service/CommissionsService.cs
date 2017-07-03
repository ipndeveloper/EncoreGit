using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Commissions.Common;
using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Interfaces.AccountTitleOverride;
using NetSteps.Commissions.Service.Interfaces.AccountTitles;
using NetSteps.Commissions.Service.Interfaces.BonusKind;
using NetSteps.Commissions.Service.Interfaces.CalculationKind;
using NetSteps.Commissions.Service.Interfaces.CalculationOverride;
using NetSteps.Commissions.Service.Interfaces.DisbursementHold;
using NetSteps.Commissions.Service.Interfaces.CommissionPlan;
using NetSteps.Commissions.Service.Interfaces.DisbursementProfile;
using NetSteps.Commissions.Service.Interfaces.DistributorPerformance;
using NetSteps.Commissions.Service.Interfaces.LedgerEntryKind;
using NetSteps.Commissions.Service.Interfaces.Override;
using NetSteps.Commissions.Service.Interfaces.OverrideReason;
using NetSteps.Commissions.Service.Interfaces.OverrideReasonSource;
using NetSteps.Commissions.Service.Interfaces.Period;
using NetSteps.Commissions.Service.Interfaces.Title;
using NetSteps.Commissions.Service.Interfaces.TitleKind;
using NetSteps.Commissions.Service.Interfaces.Account;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Commissions.Service
{
    public class CommissionsService : ICommissionsService
    {
        protected readonly IPeriodService PeriodService;
        protected readonly ICommissionPlanService CommissionPlanService;
        protected readonly ITitleService TitleService;
        protected readonly ITitleKindService TitleKindService;
        protected readonly IAccountTitleOverrideService AccountTitleOverrideService;
        protected readonly ICalculationOverrideService CalculationOverrideService;
        protected readonly IDisbursementHoldService DisbursementHoldService;
        protected readonly IOverrideKindService OverrideKindService;
        protected readonly IOverrideReasonService OverrideReasonService;
        protected readonly IOverrideReasonSourceService OverrideReasonSourceService;
        protected readonly ILedgerEntryKindService LedgerEntryKindService;
        protected readonly ICalculationKindService CalculationKindService;
        protected readonly IDisbursementProfileService DisbursementProfileService;
        protected readonly IBonusKindService BonusKindService;
        protected readonly IDistributorPerformanceService DistributorPerformanceService;
        protected readonly IAccountService AccountService;
        protected readonly IAccountTitleService AccountTitleService;

        public CommissionsService(
                IPeriodService periodService,
                ICommissionPlanService commissionPlanService,
                ITitleService titleService,
                ITitleKindService titleKindService,
                IAccountTitleOverrideService accountTitleOverrideService,
                ICalculationOverrideService calculationOverrideService,
                IDisbursementHoldService disbursementHoldService,
                IOverrideKindService overrideKindService,
                IOverrideReasonService overrideReasonService,
                IOverrideReasonSourceService overrideReasonSourceService,
                ILedgerEntryKindService ledgerEntryKindService,
                ICalculationKindService calculationKindservice,
                IDisbursementProfileService disbursementProfileService,
                IBonusKindService bonusKindService,
                IDistributorPerformanceService distributorPerformanceService,
                IAccountService accountService,
                IAccountTitleService accountTitleService
            )
        {
            PeriodService = periodService;
            CommissionPlanService = commissionPlanService;
            TitleService = titleService;
            TitleKindService = titleKindService;
            AccountTitleOverrideService = accountTitleOverrideService;
            CalculationOverrideService = calculationOverrideService;
            DisbursementHoldService = disbursementHoldService;
            OverrideKindService = overrideKindService;
            OverrideReasonService = overrideReasonService;
            OverrideReasonSourceService = overrideReasonSourceService;
            LedgerEntryKindService = ledgerEntryKindService;
            CalculationKindService = calculationKindservice;
            DisbursementProfileService = disbursementProfileService;
            BonusKindService = bonusKindService;
            DistributorPerformanceService = distributorPerformanceService;
            AccountService = accountService;
            AccountTitleService = accountTitleService;
        }

        public IAccountTitleOverride AddAccountTitleOverride(IAccountTitleOverride titleOverride)
        {
            return AccountTitleOverrideService.AddAccountTitleOverride(titleOverride);
        }

        public ICalculationOverride AddCalculationOverride(ICalculationOverride calculationOverride)
        {
            return CalculationOverrideService.AddCalculationOverride(calculationOverride);
        }

        public IDisbursementHold AddDisbursementHold(IDisbursementHold disbursementHold)
        {
            return DisbursementHoldService.AddDisbursementHold(disbursementHold);
        }

        public bool DeleteAccountTitleOverride(int titleOverrideId)
        {
            return AccountTitleOverrideService.DeleteAccountTitleOverride(titleOverrideId);
        }

        public bool DeleteCalculationOverride(int calculationOverrideId)
        {
            return CalculationOverrideService.DeleteCalculationOverride(calculationOverrideId);
        }

        public IAccountTitleOverride GetAccountTitleOverride(int accountTitleOverrideId)
        {
            return AccountTitleOverrideService.GetAccountTitleOverride(accountTitleOverrideId);
        }

        public ICalculationOverride GetCalculationOverride(int calculationOverrideId)
        {
            return CalculationOverrideService.GetCalculationOverride(calculationOverrideId);
        }

        public IDisbursementHold GetDisbursementHold(int disbursementHoldId)
        {
            return DisbursementHoldService.GetDisbursementHold(disbursementHoldId);
        }

        public ICommissionPlan GetCommissionPlan(int commissionPlanId)
        {
            return CommissionPlanService.GetCommissionPlan(commissionPlanId);
        }

        public ICommissionPlan GetCommissionPlan(DisbursementFrequencyKind disbursementFrequency)
        {
            return CommissionPlanService.GetCommissionPlan(disbursementFrequency);
        }

        public IEnumerable<ICommissionPlan> GetCommissionPlans()
        {
            return CommissionPlanService.GetCommissionPlans();
        }

        public IEnumerable<IPeriod> GetCurrentAndPastPeriods()
        {
            return PeriodService.GetPeriods(x => x.StartDateUTC <= DateTime.UtcNow);
        }

        public IPeriod GetCurrentPeriod()
        {
            return PeriodService.GetCurrentPeriod(DisbursementFrequencyKind.Monthly);
        }

        public string GetDisbursementMethodCode(int disbursementMethodId)
        {
            return DisbursementProfileService.GetDisbursementMethodCode(disbursementMethodId);
        }

        public int GetDisbursementProfileCountByAccountAndDisbursementMethod(int accountId, DisbursementMethodKind disbursementMethod)
        {
            return DisbursementProfileService.GetDisbursementProfileCountByAccountAndDisbursementMethod(accountId, disbursementMethod);
        }

        public IEnumerable<IDisbursementProfile> GetDisbursementProfilesByAccountAndDisbursementMethod(int accountId, DisbursementMethodKind disbursementMethod)
        {
            return DisbursementProfileService.GetDisbursementProfilesByAccountAndDisbursementMethod(accountId, disbursementMethod);
        }

        public IEnumerable<IDisbursementProfile> GetDisbursementProfilesByAccountId(int accountId)
        {
            return DisbursementProfileService.GetDisbursementProfilesByAccountId(accountId);
        }

        public IDistributorPeriodPerformanceData GetDistributorPerformanceData(int accountId, int periodId)
        {
            return DistributorPerformanceService.GetDistributorPerformanceData(accountId, periodId);
        }

        public IEnumerable<IAccountKPI> GetDistributorPerformanceOverviewData(int accountId, int periodId)
        {
            return DistributorPerformanceService.GetDistributorPerformanceOverviewData(accountId, periodId);
        }

        public int GetMaximumDisbursementProfiles(DisbursementMethodKind method)
        {
            return DisbursementProfileService.GetMaximumDisbursementProfiles(method);
        }

        public IEnumerable<IPeriod> GetOpenPeriods(DisbursementFrequencyKind disbursementFrequency)
        {
            return PeriodService.GetOpenPeriods();
        }

        public IPeriod GetPeriod(int periodId)
        {
            return PeriodService.GetPeriod(periodId);
        }

        public IEnumerable<IPeriod> GetPeriodsForAccount(int accountId)
        {
            return PeriodService.GetPeriodsForAccount(accountId);
        }

        public ITitle GetTitle(int titleId)
        {
            return TitleService.GetTitle(titleId);
        }

        public IEnumerable<ITitle> GetTitleFromReportByPeriod(int periodId, int accountId)
        {
            return TitleService.GetFromReportByPeriod(periodId, accountId);
        }

        public IEnumerable<ITitle> GetTitles()
        {
            return TitleService.GetTitles();
        }

        public IDisbursementProfile SaveDisbursementProfile(IDisbursementProfile disbursementProfile)
        {
            return DisbursementProfileService.SaveDisbursementProfile(disbursementProfile);
        }

        public IAccount GetAccount(int accountId)
        {
            return AccountService.GetAccount(accountId);
        }

        public IAccount SaveAccount(IAccount account)
        {
            return AccountService.AddAccount(account);
        }

        public bool SavePartialAccountToCommission(int accountId, string accountNumber, int? sponsorId = null, int? enrollerId = null)
        {
            return AccountService.SaveTemporaryAccountToCommission(accountId, accountNumber, sponsorId, enrollerId);
        }

        public IAccountTitleOverrideSearchResult SearchAccountTitleOverrides(AccountTitleOverrideSearchParameters parameters)
        {
            return AccountTitleOverrideService.SearchAccountTitleOverrides(parameters);
        }

        public ICalculationOverrideSearchResult SearchCalculationOverrides(CalculationOverrideSearchParameters parameters)
        {
            return CalculationOverrideService.SearchCalculationOverrides(parameters);
        }

        public IDisbursementHoldSearchResult SearchDisbursementHolds(DisbursementHoldSearchParameters parameters)
        {
            return DisbursementHoldService.SearchDisbursementHolds(parameters);
        }

        public IEnumerable<IPeriod> GetCurrentPeriods()
        {
            return PeriodService.GetCurrentPeriods();
        }

        public IEnumerable<IOverrideReasonSource> GetOverrideReasonSources()
        {
            return OverrideReasonSourceService.GetOverrideReasonSources();
        }

        public IEnumerable<IOverrideReason> GetOverrideReasons()
        {
            return OverrideReasonService.GetOverrideReasons();
        }

        public IEnumerable<IOverrideReason> GetOverrideReasonsForSource(int overrideReasonSourceId)
        {
            return OverrideReasonService.GetOverrideReasonsForSource(overrideReasonSourceId);
        }

        public IEnumerable<IOverrideReason> GetOverrideReasonsForSource(IOverrideReasonSource overrideReasonSource)
        {
            return OverrideReasonService.GetOverrideReasonsForSource(overrideReasonSource.OverrideReasonSourceId);
        }

        public ITitleKind GetTitleKind(int titleKindId)
        {
            return TitleKindService.GetTitleKind(titleKindId);
        }

        public IEnumerable<ITitleKind> GetTitleKinds()
        {
            return TitleKindService.GetTitleKinds();
        }

        public IEnumerable<ICalculationKind> GetCalculationKinds()
        {
            return CalculationKindService.GetCalculationKinds();
        }

        public IEnumerable<IOverrideKind> GetOverrideKinds()
        {
            return OverrideKindService.GetOverrideKinds();
        }

        public IBonusKind GetBonusKind(string bonusCode)
        {
            return BonusKindService.GetBonusKind(bonusCode);
        }

        public IBonusKind GetBonusKind(int bonusKindId)
        {
            return BonusKindService.GetBonusKind(bonusKindId);
        }

        public IEnumerable<IBonusKind> GetBonusKinds()
        {
            return BonusKindService.GetBonusKinds();
        }

        public IAccountTitle GetAccountTitle(int accountId, int titleKindId, int? periodId)
        {
            return AccountTitleService.GetAccountTitle(accountId, titleKindId, periodId);
        }

        public IEnumerable<IAccountTitle> GetAccountTitles(int accountId, int? periodId)
        {
            return AccountTitleService.GetAccountTitles(accountId, periodId);
        }

        public IEnumerable<IAccountTitle> GetCurrentAccountTitles(int? periodId)
        {
            return AccountTitleService.GetCurrentAccountTitles(periodId);
        }

        public IEnumerable<IBonusPayout> GetDistributorBonusData(int accountId, int periodId)
        {
            return DistributorPerformanceService.GetDistributorBonusData(accountId, periodId);
        }

        public IEnumerable<IEarningReport> GetEarningReportData(int accountId, int periodId)
        {
            return DistributorPerformanceService.GetEarningRerportData(accountId, periodId);
        }

        public IEnumerable<IReportAccountKPIDetail> GetReportAccountKPIDetails(int periodId, int accountId, string kPICode)
        {
            return DistributorPerformanceService.GetReportAccountKPIDetails(periodId, accountId, kPICode);
        }

        public IEnumerable<IReportAccountKPIDetail> GetReportAccountKPIDetailsLegs(int periodId, int accountId, string kPICode)
        {
            return DistributorPerformanceService.GetReportAccountKPIDetailsLegs(periodId, accountId, kPICode);
        }

        public IEnumerable<IReportBonusDetail> GetReportBonusDetail(int periodId, int accountId, string bonusCode)
        {
            return DistributorPerformanceService.GetReportBonusDetail(periodId, accountId, bonusCode);
        }


        public void GetEarningsAmountOnly(int periodId, int accountId, out decimal? totalPeriodEarnings, out decimal? totalYearEanings)
        {
            DistributorPerformanceService.GetEarningsAmountOnly(periodId, accountId, out totalPeriodEarnings, out totalYearEanings);
        }
    }
}
