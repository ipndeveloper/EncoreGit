using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Extensions;
using System.Data.SqlClient;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Utility;
using System.Data;

/*
 * @01 20150811 BR-MLM-007 CSTI JMO: Added GetThreeNextPeriods Method
 */

namespace NetSteps.Data.Entities.Repositories
{
    public class PeriodsRepository
    {
        public static List<PeriodSearchData> Search()
        {
            return PeriodDataAccess.Search();
        }

        public static PaginatedList<PeriodSearchData> Search(PeriodSearchParameters searchParams)
        {
            // Apply filters
            var periods = PeriodDataAccess.Search().FindAll(x => x.PlanID == (searchParams.PlanID.HasValue ? searchParams.PlanID.Value : x.PlanID) &&
                                                              (x.StartDate >= (searchParams.StartDate.HasValue ? searchParams.StartDate.Value : x.StartDate) &&
                                                              x.StartDate <= (searchParams.EndDate.HasValue ? searchParams.EndDate.Value : x.StartDate)));

            // Apply pagination
            IQueryable<PeriodSearchData> matchingItems = periods.AsQueryable<PeriodSearchData>();

            var resultTotalCount = matchingItems.Count();
            matchingItems = matchingItems.ApplyPagination(searchParams);

            return matchingItems.ToPaginatedList<PeriodSearchData>(searchParams, resultTotalCount);
        }

        public static void Save(PeriodSearchData period)
        {
            if (period.PeriodID == 0)
            {
                PeriodDataAccess.Insert(period);
            }
            else
            {
                PeriodDataAccess.Update(period);
            }
        }

        public static Dictionary<string, string> GetPlans()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            
            foreach (var item in Plan.Search())
            {
                dictionary.Add(item.PlanID.ToString(), item.Name);
            }

            return dictionary;
        }

        public static Dictionary<string, string> GetAllPeriods()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            foreach (var item in PeriodDataAccess.Search())
            {
                dictionary.Add(item.PeriodID.ToString(), item.Description);
            }
            return dictionary;
        }

        public static DateTime? GetStartDateCreatingMode(int planId)
        {
            return PeriodDataAccess.GetStartDateCreatingMode(planId);
        }

        public static bool VerifyOverlapStartDate(int planId, DateTime startDate)
        {
            return PeriodDataAccess.Search().FindAll(x => x.PlanID == planId && x.StartDate >= startDate).Count() > 0 ? false : true;
        }

        public static bool VerifyOverlapDate(int? periodId, int planId, DateTime startDate, DateTime endDate)
        {
            return PeriodDataAccess.VerifyOverlapDate(periodId.Value, planId, startDate, endDate);
        }

        //Developed by BAL - CSTI - AINI
        public static List<CatalogPeriod> SearchCatalogPeriods(int catalogId)
        {
            return PeriodDataAccess.SearchCatalogPeriods(catalogId);
        }

        public static void SaveCatalogPeriods(int catalogId, string[] periods)
        {
            using (SqlConnection connection = new SqlConnection(DataAccess.GetConnectionString("Core")))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction("CatalogPeriodsTransaction");

                try
                {
                    PeriodDataAccess.DeleteCatalogPeriods(catalogId, connection, transaction);

                    if (!periods[0].IsNullOrEmpty())
                    {
                        foreach (var periodId in periods)
                        {
                            PeriodDataAccess.SaveCatalogPeriod(catalogId, Convert.ToInt32(periodId), connection, transaction);
                        }
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
                }
            }
        }
        //Developed by BAL - CSTI - AFIN

        #region [@01 A01]

        /// <summary>
        /// Gets Three Next Periods
        /// </summary>
        /// <param name="PeriodID"></param>
        /// <returns>(Key: PeriodID, Value: Description)</returns>
        public static Dictionary<string, string> GetThreeNextPeriods(int PeriodID)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            foreach (var item in PeriodDataAccess.GetThreeNextPeriods(PeriodID))
            {
                dictionary.Add(item.PeriodID.ToString(), item.Description);
            }
            return dictionary;
        }
        public static Dictionary<string, string> GetThreeNextPeriodsReactiveAccount(int PeriodID, int AccountID)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            foreach (var item in PeriodDataAccess.GetThreeNextPeriodsReactiveAccount(PeriodID, AccountID))
            {
                dictionary.Add(item.PeriodID.ToString(), item.Description);
            }
            return dictionary;
        }

        
        /// <summary>
        /// Gets Open Period
        /// </summary>
        /// <returns>PeriodID</returns>
        public static int GetOpenPeriodID()
        {
            return PeriodDataAccess.GetOpenPeriodID();
        }

        /// <summary>
        /// Gets previous period from specific period
        /// </summary>
        /// <param name="periodId">Period Id</param>
        /// <param name="levelDown">Level Down</param>
        /// <returns>Period ID</returns>
        public static int GetPreviousPeriod(int periodId, int levelDown)
        {
            int previousPeriod = 0;
            
            using (var context = new EntityDBContext(ConnectionStrings.BelcorpCommission))
            {
                var result = context.Database.SqlQuery<int>(string.Format(PreviousPeriodFunctionName, periodId, levelDown));

                previousPeriod = result.First();
                return previousPeriod;
            }           
        }


        /// <summary>
        /// Gets Period by Id
        /// </summary>
        /// <returns>PeriodSearchData</returns>
        public static PeriodSearchData GetPeriodbyID(int periodoId)
        {
            return PeriodDataAccess.GetPeriodbyID(periodoId);
        }

        private const string PreviousPeriodFunctionName = "select dbo.sfnLkpPreviousPeriodID({0}, {1});";
        #endregion
    }
}
