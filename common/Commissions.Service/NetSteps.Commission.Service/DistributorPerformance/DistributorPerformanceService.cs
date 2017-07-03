using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Commissions.Service.Base;
using NetSteps.Commissions.Service.Interfaces.DistributorPerformance;
using NetSteps.Commissions.Common.Models;
using System.Data.SqlClient;
using System.Configuration;
using NetSteps.Foundation.Common;
using System.Data;
using NetSteps.Commissions.Service.Models;
using NetSteps.Commissions.Service.Interfaces.Title;
using NetSteps.Core.Cache;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Commissions.Service.DistributorPerformance
{
    public class DistributorPerformanceService : IDistributorPerformanceService
    {
        private ICache<Tuple<int, int>, IDistributorPeriodPerformanceData> _distributorPeriodPerformanceDataCache;
        private ICache<Tuple<int, int>, IEnumerable<IAccountKPI>> _distributorKPICache;
        private ICache<Tuple<int, int>, IEnumerable<IBonusPayout>> _distributorBonusCache;
        private ICache<Tuple<int, int>, IEnumerable<IEarningReport>> _earningReporCache;
        private ICache<Tuple<int, int, string>, IEnumerable<IReportAccountKPIDetail>> _accountKPIDetailReporCache;
        private ICache<Tuple<int, int, string>, IEnumerable<IReportAccountKPIDetail>> _accountKPIDetailLegReporCache;
        private ICache<Tuple<int, int, string>, IEnumerable<IReportBonusDetail>> _earningBonusDetailReporCache;

        private ITitleProvider _titleProvider;

        private enum PerformanceLandingFields
        {
            CurrentLevel = 0,
            PaidAsLevel,
            SalesIndicatorLevel,
            Volume,
            RequiredVolume
        }

        private enum KPIFields
        {
            KPITypeID,
            KPITypeCode,
            KPIValue,
            DataType,
            TermName
        }

        private enum BonusPayoutFields
        {
            AccountID,
            PeriodID,
            SortOrder,
            BonusCategory,
            BonusSubcategory,
            BonusName,
            BonusAmount
        }

        private enum EarningReportFields
        {
            AccountNumber,
            CareerTitle,
            PaidAsTitle,
            EnrollmentDate,
            AccountName,
            Address,
            State,
            PostalCode,
            Level1CV,
            Level1CBPer,
            Level1CB,
            Level1Code,
            Level2CV,
            Level2CBPer,
            Level2CB,
            Level2Code,
            Level3CV,
            Level3CBPer,
            Level3CB,
            Level3Code,
            Level4CV,
            Level4CBPer,
            Level4CB,
            Level4Code,
            Generation1Title7CV,
            Generation1Title7CBPer,
            Generation1Title7CB,
            Generation1Title7Code,
            Generation2Title7CV,
            Generation2Title7CBPer,
            Generation2Title7CB,
            Generation2Title7Code,
            Generation3Title7CV,
            Generation3Title7CBPer,
            Generation3Title7CB,
            Generation3Title7Code,
            Generation4Title7CV,
            Generation4Title7CBPer,
            Generation4Title7CB,
            Generation4Title7Code,
            Generation5Title7CV,
            Generation5Title7CBPer,
            Generation5Title7CB,
            Generation5Title7Code,
            Generation1Title10CV,
            Generation1Title10CBPer,
            Generation1Title10CB,
            Generation1Title10Code,
            Generation2Title10CV,
            Generation2Title10CBPer,
            Generation2Title10CB,
            Generation2Title10Code,
            TurboInfinityBonusCV,
            TurboInfinityBonusCBPer,
            TurboInfinityBonusCB,
            TurboInfinityBonusCode,
            FastStartBonusCV,
            FastStartBonusCBPer,
            FastStartBonusCB,
            FastStartBonusCode,
            CoachingBonusCV,
            CoachingBonusCBPer,
            CoachingBonusCB,
            CoachingBonusCode,
            TeamBuildingBonusCV,
            TeamBuildingBonusCBPer,
            TeamBuildingBonusCB,
            TeamBuildingBonusCode,
            AdvancementBonusCV,
            AdvancementBonusCBPer,
            AdvancementBonusCB,
            AdvancementBonusCode,
            MatchingAdvacementBonusCV,
            MatchingAdvacementBonusCBPer,
            MatchingAdvacementBonusCB,
            MatchingAdvacementBonusCode,
            ConsistencyBonusCV,
            ConsistencyBonusCBPer,
            ConsistencyBonusCB,
            ConsistencyBonusCode,
            RetailProfitBonusCV,
            RetailProfitBonusCBPer,
            RetailProfitBonusCB,
            RetailProfitBonusCode,
            TotalCV,
            TotalCB
        }

        private enum ReportAccountKPIDetailFields
        {
            AccountKPIDetailID,
            Level,
            DownlineID,
            DownlineName,
            QV
        }

        private enum ReportAccountKPIDetailLegsFields
        {
            AccountKPIDetailID,
            DownlineID,
            DownlineName,
            Level,
            Generation,
            CareerTitle,
            DownlinePaidAsTitle,
            PQV,
            DQV
        }

        private enum ReportBonusDetailFields
        {
            DownlineID,
            DownlineName,
            PQV,
            PCV,
            CB,
            AmountPaid,
            BonusTypeID,
            BonusTypeName,
            BonusValue
        }

        public DistributorPerformanceService(ITitleProvider titleProvider)
        {
            _titleProvider = titleProvider;

            _distributorPeriodPerformanceDataCache = new ActiveMruLocalMemoryCache<Tuple<int, int>, IDistributorPeriodPerformanceData>("DistributorPeriodPerformanceData", new DelegatedDemuxCacheItemResolver<Tuple<int, int>, IDistributorPeriodPerformanceData>(ResolveDistributorPerfomanceData));
            _distributorKPICache = new ActiveMruLocalMemoryCache<Tuple<int, int>, IEnumerable<IAccountKPI>>("Distributor-KPI", new DelegatedDemuxCacheItemResolver<Tuple<int, int>, IEnumerable<IAccountKPI>>(ResolveAccountKPIs));
            _distributorBonusCache = new ActiveMruLocalMemoryCache<Tuple<int, int>, IEnumerable<IBonusPayout>>("Distributor-BonusPayout", new DelegatedDemuxCacheItemResolver<Tuple<int, int>, IEnumerable<IBonusPayout>>(ResolveAccountBonusPayout));
            _earningReporCache = new ActiveMruLocalMemoryCache<Tuple<int, int>, IEnumerable<IEarningReport>>("Distributor-EarningReport", new DelegatedDemuxCacheItemResolver<Tuple<int, int>, IEnumerable<IEarningReport>>(ResolveEarningReport));
            _accountKPIDetailReporCache = new ActiveMruLocalMemoryCache<Tuple<int, int, string>, IEnumerable<IReportAccountKPIDetail>>("Distributor-EarningReport", new DelegatedDemuxCacheItemResolver<Tuple<int, int, string>, IEnumerable<IReportAccountKPIDetail>>(ResolveReportAccountKPIDetail));
            _accountKPIDetailLegReporCache = new ActiveMruLocalMemoryCache<Tuple<int, int, string>, IEnumerable<IReportAccountKPIDetail>>("Distributor-EarningReport", new DelegatedDemuxCacheItemResolver<Tuple<int, int, string>, IEnumerable<IReportAccountKPIDetail>>(ResolveReportAccountKPIDetaiLeg));
            _earningBonusDetailReporCache = new ActiveMruLocalMemoryCache<Tuple<int, int, string>, IEnumerable<IReportBonusDetail>>("Distributor-EarningReport", new DelegatedDemuxCacheItemResolver<Tuple<int, int, string>, IEnumerable<IReportBonusDetail>>(ResolveReportBonusDetail));
        }

        protected virtual string ConnectionProviderName { get { return CommissionsConstants.ConnectionStringNames.KnownFactorsDataWarehouse; } }

        private IConnectionProvider _connectionProvider;
        private IConnectionProvider ConnectionProvider
        {
            get
            {
                if (_connectionProvider == null)
                {
                    _connectionProvider = Create.NewNamed<IConnectionProvider>(ConnectionProviderName);
                }
                return _connectionProvider;
            }
        }

        private bool ResolveDistributorPerfomanceData(Tuple<int, int> key, out IDistributorPeriodPerformanceData result)
        {
            var accountId = key.Item1;
            var periodId = key.Item2;

            if (ConnectionProvider.CanConnect())
            {
                using (var connection = ConnectionProvider.GetConnection())
                {/*CGI(JCT) MLM - 010*/
                    using (var command = new SqlCommand(CommissionsConstants.GetObjectNameParsed((CommissionsConstants.EnvironmentList)CommissionsConstants.ConnectionGetEnvironment.EnvironmentCountry, "usp_get_performancelandingwidgetsMLM", "Encore"), connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@AccountID", accountId));
                        command.Parameters.Add(new SqlParameter("@PeriodID", periodId));

                        connection.Open();

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                var distributorPeriodPerformanceData = new DistributorPeriodPerformanceData
                                {
                                    AccountId = accountId,
                                    CurrentTitle =
                                        _titleProvider.FirstOrDefault(
                                            x => x.TitleId == reader.GetNullable<int>((int)PerformanceLandingFields.CurrentLevel)),
                                    PaidAsTitle =
                                        _titleProvider.FirstOrDefault(
                                            x => x.TitleId == reader.GetNullable<int>((int)PerformanceLandingFields.PaidAsLevel)),
                                    PeriodId = periodId,
                                    RequiredVolume = reader.GetNullable<decimal>((int)PerformanceLandingFields.RequiredVolume),
                                    SalesIndicatorLevel = reader.GetNullable<string>((int)PerformanceLandingFields.SalesIndicatorLevel),
                                    Volume = reader.GetNullable<decimal>((int)PerformanceLandingFields.Volume)
                                };

                                return (result = distributorPeriodPerformanceData) != null;
                            }
                        }
                    }
                }
            }
            result = default(IDistributorPeriodPerformanceData);
            return false;
        }

        private bool ResolveAccountKPIs(Tuple<int, int> key, out IEnumerable<IAccountKPI> result)
        {
            var accountId = key.Item1;
            var periodId = key.Item2;

            if (ConnectionProvider.CanConnect())
            {
                using (var connection = ConnectionProvider.GetConnection())
                {/*CGI(JCT) MLM - 010*/
                    using (var command = new SqlCommand(CommissionsConstants.GetObjectNameParsed((CommissionsConstants.EnvironmentList)CommissionsConstants.ConnectionGetEnvironment.EnvironmentCountry, "uspGetKPIsForAccountMLM", "Commissions"), connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@AccountID", accountId));
                        command.Parameters.Add(new SqlParameter("@PeriodID", periodId));

                        connection.Open();
                        using (var reader = command.ExecuteReader())
                        {
                            var items = new List<IAccountKPI>();
                            while (reader.Read())
                            {
                                items.Add(new AccountKPI
                                {
                                    DataType = reader.GetString((int)KPIFields.DataType),
                                    KPITypeCode = reader.GetString((int)KPIFields.KPITypeCode),
                                    KPIValue = reader.GetNullable<string>((int)KPIFields.KPIValue), //.GetDecimal((int)KPIFields.KPIValue).ToString("F2"),
                                    TermName = reader.GetString((int)KPIFields.TermName)
                                });
                            }
                            return ((result = items) != null) && result.Any();
                        }
                    }
                }
            }
            result = Enumerable.Empty<IAccountKPI>();
            return false;
        }

        private bool ResolveAccountBonusPayout(Tuple<int, int> key, out IEnumerable<IBonusPayout> result)
        {
            var accountId = key.Item1;
            var periodId = key.Item2;

            if (ConnectionProvider.CanConnect())
            {
                using (var connection = ConnectionProvider.GetConnection())
                {/*CGI(JCT) MLM - 010*/
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = String.Format("SELECT {0} FROM {1} WHERE {2}",
                            String.Join(",", Enum.GetNames(typeof(BonusPayoutFields))),
                            CommissionsConstants.GetObjectNameParsed((CommissionsConstants.EnvironmentList)CommissionsConstants.ConnectionGetEnvironment.EnvironmentCountry, "AccountBonuses", "Encore"),
                            String.Format("{0} = @AccountID AND {1} = @PeriodID",
                                BonusPayoutFields.AccountID.ToString(),
                                BonusPayoutFields.PeriodID.ToString()));
                        command.CommandType = CommandType.Text;
                        command.Parameters.Add(new SqlParameter("@AccountID", accountId));
                        command.Parameters.Add(new SqlParameter("@PeriodID", periodId));

                        connection.Open();

                        using (var reader = command.ExecuteReader())
                        {
                            var items = new List<IBonusPayout>();
                            while (reader.Read())
                            {
                                items.Add(new BonusPayout
                                {
                                    AccountID = reader.GetInt32((int)BonusPayoutFields.AccountID),
                                    PeriodID = reader.GetInt32((int)BonusPayoutFields.PeriodID),
                                    SortOrder = reader.GetInt32((int)BonusPayoutFields.SortOrder),
                                    BonusCategory = reader.GetString((int)BonusPayoutFields.BonusCategory),
                                    BonusSubcategory = reader.GetString((int)BonusPayoutFields.BonusSubcategory),
                                    BonusName = reader.GetString((int)BonusPayoutFields.BonusName),
                                    BonusAmount = Math.Round(reader.GetDecimal((int)BonusPayoutFields.BonusAmount), 2)
                                });
                            }
                            return ((result = items) != null) && result.Any();
                        }
                    }
                }
            }
            result = Enumerable.Empty<IBonusPayout>();
            return false;
        }

        /// <summary>
        /// Select data for Earning Report
        /// </summary>
        /// <param name="key">Parameters</param>
        /// <param name="result">Out Result</param>
        /// <returns>Enumerable of IEarning Objects</returns>
        private bool ResolveEarningReport(Tuple<int, int> key, out IEnumerable<IEarningReport> result)
        {
            var accountId = key.Item1;
            var periodId = key.Item2;

            if (ConnectionProvider.CanConnect())
            {
                using (var connection = ConnectionProvider.GetConnection())
                {
                    using (var command = new SqlCommand(CommissionsConstants.GetObjectNameParsed((CommissionsConstants.EnvironmentList)CommissionsConstants.ConnectionGetEnvironment.EnvironmentCountry, "GetEarningReport", "Encore"), connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@periodID", periodId));
                        command.Parameters.Add(new SqlParameter("@accountID", accountId));

                        connection.Open();

                        using (var reader = command.ExecuteReader())
                        {
                            var items = new List<IEarningReport>();
                            while (reader.Read())
                            {
                                items.Add(new EarningReport
                                {
                                    AccountName = reader[((int)EarningReportFields.AccountName)].ToString(),
                                    AccountNumber = reader[((int)EarningReportFields.AccountNumber)].ToString(),
                                    Address = reader[((int)EarningReportFields.Address)].ToString(),
                                    AdvancementBonusCB = GetDecimalValue(reader[((int)EarningReportFields.AdvancementBonusCB)], 2),
                                    AdvancementBonusCBPer = GetDecimalValue(reader[((int)EarningReportFields.AdvancementBonusCBPer)], 2),
                                    AdvancementBonusCV = GetDecimalValue(reader[((int)EarningReportFields.AdvancementBonusCV)], 2),
                                    AdvancementBonusCode = reader[((int)EarningReportFields.AdvancementBonusCode)].ToString(),
                                    CareerTitle = GetIntValue((int)EarningReportFields.CareerTitle),
                                    CoachingBonusCB = GetDecimalValue(reader[((int)EarningReportFields.CoachingBonusCB)], 2),
                                    CoachingBonusCBPer = GetDecimalValue(reader[((int)EarningReportFields.CoachingBonusCBPer)], 2),
                                    CoachingBonusCV = GetDecimalValue(reader[((int)EarningReportFields.CoachingBonusCV)], 2),
                                    CoachingBonusCode = reader[((int)EarningReportFields.CoachingBonusCode)].ToString(),
                                    ConsistencyBonusCB = GetDecimalValue(reader[((int)EarningReportFields.ConsistencyBonusCB)], 2),
                                    ConsistencyBonusCBPer = GetDecimalValue(reader[((int)EarningReportFields.ConsistencyBonusCBPer)], 2),
                                    ConsistencyBonusCV = GetDecimalValue(reader[((int)EarningReportFields.ConsistencyBonusCV)], 2),
                                    ConsistencyBonusCode = reader[((int)EarningReportFields.ConsistencyBonusCode)].ToString(),
                                    EnrollmentDate = reader.GetDateTime((int)EarningReportFields.EnrollmentDate),
                                    FastStartBonusCB = GetDecimalValue(reader[((int)EarningReportFields.FastStartBonusCB)], 2),
                                    FastStartBonusCBPer = GetDecimalValue(reader[((int)EarningReportFields.FastStartBonusCBPer)], 2),
                                    FastStartBonusCV = GetDecimalValue(reader[((int)EarningReportFields.FastStartBonusCV)], 2),
                                    FastStartBonusCode = reader[((int)EarningReportFields.FastStartBonusCode)].ToString(),
                                    Generation1Title10CB = GetDecimalValue(reader[((int)EarningReportFields.Generation1Title10CB)], 2),
                                    Generation1Title10CBPer = GetDecimalValue(reader[((int)EarningReportFields.Generation1Title10CB)], 2),
                                    Generation1Title10CV = GetDecimalValue(reader[((int)EarningReportFields.Generation1Title10CB)], 2),
                                    Generation1Title10Code = reader[((int)EarningReportFields.Generation1Title10Code)].ToString(),
                                    Generation1Title7CB = GetDecimalValue(reader[((int)EarningReportFields.Generation1Title7CB)], 2),
                                    Generation1Title7CBPer = GetDecimalValue(reader[((int)EarningReportFields.Generation1Title7CBPer)], 2),
                                    Generation1Title7CV = GetDecimalValue(reader[((int)EarningReportFields.Generation1Title7CV)], 2),
                                    Generation1Title7Code = reader[((int)EarningReportFields.Generation1Title7Code)].ToString(),

                                    Generation2Title10CB = GetDecimalValue(reader[((int)EarningReportFields.Generation2Title10CB)], 2),
                                    Generation2Title10CBPer = GetDecimalValue(reader[((int)EarningReportFields.Generation2Title10CBPer)], 2),
                                    Generation2Title10CV = GetDecimalValue(reader[((int)EarningReportFields.Generation2Title10CV)], 2),
                                    Generation2Title10Code = reader[((int)EarningReportFields.Generation2Title10Code)].ToString(),

                                    Generation2Title7CB = GetDecimalValue(reader[((int)EarningReportFields.Generation2Title7CB)], 2),
                                    Generation2Title7CBPer = GetDecimalValue(reader[((int)EarningReportFields.Generation2Title7CBPer)], 2),
                                    Generation2Title7CV = GetDecimalValue(reader[((int)EarningReportFields.Generation2Title7CV)], 2),
                                    Generation2Title7Code = reader[((int)EarningReportFields.Generation2Title7Code)].ToString(),

                                    Generation3Title7CB = GetDecimalValue(reader[((int)EarningReportFields.Generation3Title7CB)], 2),
                                    Generation3Title7CBPer = GetDecimalValue(reader[((int)EarningReportFields.Generation3Title7CBPer)], 2),
                                    Generation3Title7CV = GetDecimalValue(reader[((int)EarningReportFields.Generation3Title7CV)], 2),
                                    Generation3Title7Code = reader[((int)EarningReportFields.Generation3Title7Code)].ToString(),

                                    Generation4Title7CB = GetDecimalValue(reader[((int)EarningReportFields.Generation4Title7CB)], 2),
                                    Generation4Title7CBPer = GetDecimalValue(reader[((int)EarningReportFields.Generation4Title7CBPer)], 2),
                                    Generation4Title7CV = GetDecimalValue(reader[((int)EarningReportFields.Generation4Title7CV)], 2),
                                    Generation4Title7Code = reader[((int)EarningReportFields.Generation4Title7Code)].ToString(),

                                    Generation5Title7CB = GetDecimalValue(reader[((int)EarningReportFields.Generation5Title7CB)], 2),
                                    Generation5Title7CBPer = GetDecimalValue(reader[((int)EarningReportFields.Generation5Title7CBPer)], 2),
                                    Generation5Title7CV = GetDecimalValue(reader[((int)EarningReportFields.Generation5Title7CV)], 2),
                                    Generation5Title7Code = reader[((int)EarningReportFields.Generation5Title7Code)].ToString(),

                                    Level1CB = GetDecimalValue(reader[((int)EarningReportFields.Level1CB)], 2),
                                    Level1CBPer = GetDecimalValue(reader[((int)EarningReportFields.Level1CBPer)], 2),
                                    Level1CV = GetDecimalValue(reader[((int)EarningReportFields.Level1CV)], 2),
                                    Level1Code = reader[((int)EarningReportFields.Level1Code)].ToString(),

                                    Level2CB = GetDecimalValue(reader[((int)EarningReportFields.Level2CB)], 2),
                                    Level2CBPer = GetDecimalValue(reader[((int)EarningReportFields.Level2CBPer)], 2),
                                    Level2CV = GetDecimalValue(reader[((int)EarningReportFields.Level2CV)], 2),
                                    Level2Code = reader[((int)EarningReportFields.Level2Code)].ToString(),

                                    Level3CB = GetDecimalValue(reader[((int)EarningReportFields.Level3CB)], 2),
                                    Level3CBPer = GetDecimalValue(reader[((int)EarningReportFields.Level3CBPer)], 2),
                                    Level3CV = GetDecimalValue(reader[((int)EarningReportFields.Level3CV)], 2),
                                    Level3Code = reader[((int)EarningReportFields.Level3Code)].ToString(),

                                    Level4CB = GetDecimalValue(reader[((int)EarningReportFields.Level4CB)], 2),
                                    Level4CBPer = GetDecimalValue(reader[((int)EarningReportFields.Level4CBPer)], 2),
                                    Level4CV = GetDecimalValue(reader[((int)EarningReportFields.Level4CV)], 2),
                                    Level4Code = reader[((int)EarningReportFields.Level4Code)].ToString(),

                                    MatchingAdvacementBonusCB = GetDecimalValue(reader[((int)EarningReportFields.MatchingAdvacementBonusCB)], 2),
                                    MatchingAdvacementBonusCBPer = GetDecimalValue(reader[((int)EarningReportFields.MatchingAdvacementBonusCBPer)], 2),
                                    MatchingAdvacementBonusCV = GetDecimalValue(reader[((int)EarningReportFields.MatchingAdvacementBonusCV)], 2),
                                    MatchingAdvacementBonusCode = reader[((int)EarningReportFields.MatchingAdvacementBonusCode)].ToString(),

                                    PostalCode = reader[((int)EarningReportFields.PostalCode)].ToString(),
                                    RetailProfitBonusCB = GetDecimalValue(reader[((int)EarningReportFields.RetailProfitBonusCB)], 2),
                                    RetailProfitBonusCBPer = GetDecimalValue(reader[((int)EarningReportFields.RetailProfitBonusCBPer)], 2),
                                    RetailProfitBonusCV = GetDecimalValue(reader[((int)EarningReportFields.RetailProfitBonusCV)], 2),
                                    RetailProfitBonusCode = reader[((int)EarningReportFields.RetailProfitBonusCode)].ToString(),

                                    State = reader[((int)EarningReportFields.State)].ToString(),
                                    TeamBuildingBonusCB = GetDecimalValue(reader[((int)EarningReportFields.TeamBuildingBonusCB)], 2),
                                    TeamBuildingBonusCBPer = GetDecimalValue(reader[((int)EarningReportFields.TeamBuildingBonusCBPer)], 2),
                                    TeamBuildingBonusCV = GetDecimalValue(reader[((int)EarningReportFields.TeamBuildingBonusCV)], 2),
                                    TeamBuildingBonusCode = reader[((int)EarningReportFields.TeamBuildingBonusCode)].ToString(),

                                    TotalCB = GetDecimalValue(reader[((int)EarningReportFields.TotalCB)], 2),
                                    TotalCV = GetDecimalValue(reader[((int)EarningReportFields.TotalCV)], 2),
                                    TurboInfinityBonusCB = GetDecimalValue(reader[((int)EarningReportFields.TurboInfinityBonusCB)], 2),
                                    TurboInfinityBonusCBPer = GetDecimalValue(reader[((int)EarningReportFields.TurboInfinityBonusCBPer)], 2),
                                    TurboInfinityBonusCV = GetDecimalValue(reader[((int)EarningReportFields.TurboInfinityBonusCV)], 2),
                                    TurboInfinityBonusCode = reader[((int)EarningReportFields.TurboInfinityBonusCode)].ToString()
                                });
                            }
                            return ((result = items) != null) && result.Any();
                        }
                    }
                }
            }
            result = Enumerable.Empty<IEarningReport>();
            return false;
        }

        private bool ResolveReportAccountKPIDetail(Tuple<int, int, string> key, out IEnumerable<IReportAccountKPIDetail> result)
        {
            var periodId = key.Item1;
            var accountId = key.Item2;
            var kpiCode = key.Item3;

            if (ConnectionProvider.CanConnect())
            {
                using (var connection = ConnectionProvider.GetConnection())
                {
                    using (var command = new SqlCommand(CommissionsConstants.GetObjectNameParsed((CommissionsConstants.EnvironmentList)CommissionsConstants.ConnectionGetEnvironment.EnvironmentCountry, "GetReportAccountKPIDetails", "Encore"), connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@AccountID", accountId));
                        command.Parameters.Add(new SqlParameter("@PeriodID", periodId));
                        command.Parameters.Add(new SqlParameter("@KpiCode", kpiCode));

                        connection.Open();
                        using (var reader = command.ExecuteReader())
                        {
                            var items = new List<IReportAccountKPIDetail>();
                            while (reader.Read())
                            {
                                items.Add(new ReportAccountKPIDetail
                                {
                                    AccountKPIDetailID = reader.GetNullable<int>((int)ReportAccountKPIDetailFields.AccountKPIDetailID),
                                    Level = reader.GetNullable<int>((int)ReportAccountKPIDetailFields.Level),
                                    DownlineID = reader.GetNullable<int>((int)ReportAccountKPIDetailFields.DownlineID),
                                    DownlineName = reader.GetString((int)ReportAccountKPIDetailFields.DownlineName),
                                    QV = reader.GetNullable<decimal>((int)ReportAccountKPIDetailFields.QV)
                                });
                            }
                            return ((result = items) != null) && result.Any();
                        }
                    }
                }
            }

            result = Enumerable.Empty<IReportAccountKPIDetail>();
            return false;
        }

        private bool ResolveReportAccountKPIDetaiLeg(Tuple<int, int, string> key, out IEnumerable<IReportAccountKPIDetail> result)
        {
            var periodId = key.Item1;
            var accountId = key.Item2;
            var kpiCode = key.Item3;

            if (ConnectionProvider.CanConnect())
            {
                using (var connection = ConnectionProvider.GetConnection())
                {
                    using (var command = new SqlCommand(CommissionsConstants.GetObjectNameParsed((CommissionsConstants.EnvironmentList)CommissionsConstants.ConnectionGetEnvironment.EnvironmentCountry, "GetReportAccountKPIDetailsLegs", "Commissions"), connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@AccountID", accountId));
                        command.Parameters.Add(new SqlParameter("@PeriodID", periodId));
                        command.Parameters.Add(new SqlParameter("@KpiCode", kpiCode));

                        connection.Open();
                        using (var reader = command.ExecuteReader())
                        {
                            var items = new List<IReportAccountKPIDetail>();
                            while (reader.Read())
                            {
                                items.Add(new ReportAccountKPIDetail
                                {
                                    AccountKPIDetailID = reader.GetNullable<int>((int)ReportAccountKPIDetailLegsFields.AccountKPIDetailID),
                                    DownlineID = reader.GetNullable<int>((int)ReportAccountKPIDetailLegsFields.DownlineID),
                                    DownlineName = reader.GetString((int)ReportAccountKPIDetailLegsFields.DownlineName),
                                    Level = reader.GetNullable<int>((int)ReportAccountKPIDetailLegsFields.Level),
                                    Generation = reader.GetNullable<int>((int)ReportAccountKPIDetailLegsFields.Generation),
                                    CareerTitle = reader.GetString((int)ReportAccountKPIDetailLegsFields.CareerTitle),
                                    DownlinePaidAsTitle = reader.GetString((int)ReportAccountKPIDetailLegsFields.DownlinePaidAsTitle),
                                    PQV = reader.GetNullable<decimal>((int)ReportAccountKPIDetailLegsFields.PQV),
                                    DQV = reader.GetNullable<decimal>((int)ReportAccountKPIDetailLegsFields.DQV)
                                });
                            }
                            return ((result = items) != null) && result.Any();
                        }
                    }
                }
            }

            result = Enumerable.Empty<IReportAccountKPIDetail>();
            return false;
        }

        private bool ResolveReportBonusDetail(Tuple<int, int, string> key, out IEnumerable<IReportBonusDetail> result)
        {
            var periodId = key.Item1;
            var accountId = key.Item2;
            var bonusCode = key.Item3;

            if (ConnectionProvider.CanConnect())
            {
                using (var connection = ConnectionProvider.GetConnection())
                {
                    using (var command = new SqlCommand(CommissionsConstants.GetObjectNameParsed((CommissionsConstants.EnvironmentList)CommissionsConstants.ConnectionGetEnvironment.EnvironmentCountry, "GetReportBunusDetail", "Encore"), connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@AccountID", accountId));
                        command.Parameters.Add(new SqlParameter("@PeriodID", periodId));
                        command.Parameters.Add(new SqlParameter("@BonusCode", bonusCode));

                        connection.Open();
                        using (var reader = command.ExecuteReader())
                        {
                            var items = new List<IReportBonusDetail>();
                            while (reader.Read())
                            {
                                items.Add(new ReportBonusDetail
                                {
                                    AmountPaid = reader.GetNullable<decimal>((int)ReportBonusDetailFields.AmountPaid),
                                    BonusTypeID = reader.GetNullable<int>((int)ReportBonusDetailFields.BonusTypeID),
                                    BonusTypeName = reader.GetString((int)ReportBonusDetailFields.BonusTypeName),
                                    BonusValue = reader.GetNullable<decimal>((int)ReportBonusDetailFields.BonusValue),
                                    CB = reader.GetNullable<decimal>((int)ReportBonusDetailFields.CB),
                                    DownlineID = reader.GetNullable<int>((int)ReportBonusDetailFields.DownlineID),
                                    DownlineName = reader.GetString((int)ReportBonusDetailFields.DownlineName),
                                    PCV = reader.GetNullable<decimal>((int)ReportBonusDetailFields.PCV),
                                    PQV = reader.GetNullable<decimal>((int)ReportBonusDetailFields.PQV)
                                });
                            }
                            return ((result = items) != null) && result.Any();
                        }
                    }
                }
            }

            result = Enumerable.Empty<IReportBonusDetail>();
            return false;
        }

        private decimal? GetDecimalValue(object value, int decimalPlaces)
        {
            decimal newValue = 0m;
            if (decimal.TryParse(value.ToString(), out newValue))
            {
                newValue = Math.Round(newValue, decimalPlaces);
                return newValue;
            }

            return null;
        }

        private int? GetIntValue(object value)
        {
            int newValue = 0;
            if (int.TryParse(value.ToString(), out newValue))
                return newValue;

            return null;
        }

        public IDistributorPeriodPerformanceData GetDistributorPerformanceData(int accountId, int periodId)
        {
            IDistributorPeriodPerformanceData result;
            if (!_distributorPeriodPerformanceDataCache.TryGet(Tuple.Create(accountId, periodId), out result))
            {
                result = default(IDistributorPeriodPerformanceData);
            }
            return result;
        }

        public IEnumerable<IAccountKPI> GetDistributorPerformanceOverviewData(int accountId, int periodId)
        {
            IEnumerable<IAccountKPI> results;
            if (!_distributorKPICache.TryGet(Tuple.Create(accountId, periodId), out results))
            {
                results = Enumerable.Empty<IAccountKPI>();
            }
            return results;
        }

        public IEnumerable<IBonusPayout> GetDistributorBonusData(int accountId, int periodId)
        {
            IEnumerable<IBonusPayout> results;
            if (!_distributorBonusCache.TryGet(Tuple.Create(accountId, periodId), out results))
            {
                results = Enumerable.Empty<IBonusPayout>();
            }
            return results;
        }

        public IEnumerable<IEarningReport> GetEarningRerportData(int accountId, int periodId)
        {
            IEnumerable<IEarningReport> results;
            if (!_earningReporCache.TryGet(Tuple.Create(accountId, periodId), out results))
            {
                results = Enumerable.Empty<IEarningReport>();
            }
            return results;
        }


        public IEnumerable<IReportAccountKPIDetail> GetReportAccountKPIDetails(int periodId, int accountId, string kPICode)
        {
            IEnumerable<IReportAccountKPIDetail> results;
            if (!_accountKPIDetailReporCache.TryGet(Tuple.Create(periodId, accountId, kPICode), out results))
            {
                results = Enumerable.Empty<IReportAccountKPIDetail>();
            }
            return results;
        }

        public IEnumerable<IReportAccountKPIDetail> GetReportAccountKPIDetailsLegs(int periodId, int accountId, string kPICode)
        {
            IEnumerable<IReportAccountKPIDetail> results;
            if (!_accountKPIDetailLegReporCache.TryGet(Tuple.Create(periodId, accountId, kPICode), out results))
            {
                results = Enumerable.Empty<IReportAccountKPIDetail>();
            }
            return results;
        }

        public IEnumerable<IReportBonusDetail> GetReportBonusDetail(int periodId, int accountId, string bonusCode)
        {
            IEnumerable<IReportBonusDetail> results;
            if (!_earningBonusDetailReporCache.TryGet(Tuple.Create(periodId, accountId, bonusCode), out results))
            {
                results = Enumerable.Empty<IReportBonusDetail>();
            }
            return results;
        }


        public void GetEarningsAmountOnly(int periodId, int accountId, out decimal? totalPeriodEarnings, out decimal? totalYearEanings)
        {
            totalPeriodEarnings = null;
            totalYearEanings = null;
            if (ConnectionProvider.CanConnect())
            {
                using (var connection = ConnectionProvider.GetConnection())
                {
                    using (var command = new SqlCommand(CommissionsConstants.GetObjectNameParsed((CommissionsConstants.EnvironmentList)CommissionsConstants.ConnectionGetEnvironment.EnvironmentCountry, "GetEarningsAmountOnly", "Encore"), connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@AccountID", accountId));
                        command.Parameters.Add(new SqlParameter("@PeriodID", periodId));

                        connection.Open();
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                totalPeriodEarnings = reader.GetNullable<decimal>(0);
                                totalYearEanings = reader.GetNullable<decimal>(1);
                            }
                        }
                    }
                }
            }
        }
    }
}
