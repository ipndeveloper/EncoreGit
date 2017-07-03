using System;
using System.Collections.Generic;
using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Data.Entities.Extensions;
using System.Globalization;

/*
 * @01 20150811 BR-MLM-007 CSTI JMO: Added GetThreeNextPeriods Method
 */

namespace NetSteps.Data.Entities
{
    /// <summary>
    /// Period Business Logic
    /// </summary>
    public class Periods
    {
        /// <summary>
        /// Search all Periods
        /// </summary>
        /// <returns>List<PeriodSearchData></returns>
        public static List<PeriodSearchData> Search()
        {
            return PeriodDataAccess.Search();
        }

        /// <summary>
        /// Search Periods with filter & pagination parameters
        /// </summary>
        /// <param name="searchParams"></param>
        /// <returns></returns>
        public static PaginatedList<PeriodSearchData> Search(PeriodSearchParameters searchParams)
        {
            try
            {
                return PeriodsRepository.Search(searchParams);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        /// <summary>
        /// Save Period (Create or Update)
        /// </summary>
        /// <param name="period"></param>
        public static void Save(PeriodSearchData period)
        {
            try
            {
                PeriodsRepository.Save(period);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        /// <summary>
        /// Get Plans 
        /// </summary>
        /// <returns>(Key: PlanID, Value: Plan name)</returns>
        public static Dictionary<string, string> GetPlans()
        {
            try
            {
                return PeriodsRepository.GetPlans();
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        /// <summary>
        /// Get Plans 
        /// </summary>
        /// <returns>(Key: PlanID, Value: Plan name)</returns>
        public static Dictionary<string, string> GetAllPeriods()
        {
            try
            {
                return PeriodsRepository.GetAllPeriods();
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
        /// <summary>
        /// Get StartDate, Creating Mode
        /// </summary>
        /// <param name="planId"></param>
        /// <returns>StartDate (datime)</returns>
        public static DateTime? GetStartDateCreatingMode(int planId)
        {
            return PeriodsRepository.GetStartDateCreatingMode(planId);
        }

        /// <summary>
        /// Default values to Save operation
        /// </summary>
        /// <param name="period"></param>
        public static void SetDefaultValuesToSave(PeriodSearchData period)
        {
            period.ClosedDate = null;
            period.EarningsViewable = null;
            period.BackOfficeDisplayStartDate = null;
            period.DisbursementsProcessed = null;

            period.StartDateUTC = period.StartDate.ToUniversalTime();
            period.EndDateUTC = period.EndDate.ToUniversalTime();
        }

        /// <summary>
        /// Default values to presentation a new Period
        /// </summary>
        /// <param name="period"></param>
        /// <param name="id"></param>
        public static void SetDefaultValuesToViewNewPeriod(PeriodSearchData period, int? id)
        {
            if (!id.HasValue)
            {
                period.PlanID = 1;
                period.StartDate = DateTime.Today;
                period.LockDate = DateTime.Today.AddDays(30);
                period.EndDate = DateTime.Today.AddDays(30);
            }
        }

        /// <summary>
        /// Default values to filters
        /// </summary>
        /// <param name="planId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static PeriodSearchParameters SetDefaultValuesToFilters(int? planId, DateTime? startDate, DateTime? endDate)
        {
            return new PeriodSearchParameters()
            {
                PlanID = planId.HasValue ? planId.Value : 1,
                StartDate = startDate.HasValue ? startDate.Value : DateTime.Today.AddMonths(-12),
                EndDate = endDate.HasValue ? endDate.Value : DateTime.Today.AddMonths(12),
                PageIndex = 0,
                PageSize = 15,
                OrderByDirection = NetSteps.Common.Constants.SortDirection.Ascending
            };
        }

        /// <summary>
        /// Verify overlapping (StartDate, EndDate)
        /// </summary>
        /// <param name="planId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static bool VerifyOverlapDate(int? periodId, int planId, DateTime startDate, DateTime endDate)
        {
            bool verify = false;
            if (periodId.HasValue) // Update mode
            {
                verify = PeriodsRepository.VerifyOverlapDate(periodId, planId, startDate, endDate);
            }
            else
            {
                verify = PeriodsRepository.VerifyOverlapStartDate(planId, startDate);
            }
            return verify;
        }

        public static int VerifyInputGeneralRules(PeriodSearchData period, int? periodId)
        {
            int verify = 0; //0: cumple con todas las validaciones, >0: no cumple algunas reglas de validacion
            IFormatProvider culture = new CultureInfo("es-Es", true);
            DateTime today = Convert.ToDateTime(string.Format("{0}/{1}/{2}", DateTime.Today.Day, DateTime.Today.Month, DateTime.Today.Year), culture);
            if ((period.LockDate.Day != period.EndDate.Day || period.LockDate.Month != period.EndDate.Month || period.LockDate.Year != period.EndDate.Year) || period.EndDate <= period.StartDate) verify = 1;
            if (period.EndDate < today) verify = 2;
            return verify;
        }

        public static Dictionary<int, bool> GetNextPeriodsByAccountType(int AccountTypeID, int Offset, int? OrderID, bool IncludeThisCurrentCampaign)
        {
            return PeriodExtensions.GetNextPeriodsByAccountType(AccountTypeID, Offset, OrderID, IncludeThisCurrentCampaign);
        }

        /// <summary>
        /// Crearte by FHP
        /// </summary>
        /// <param name="date">Fecha a validar</param>
        /// <returns>Periodo en base a la fecha enviada</returns>
        public static Dictionary<int, bool> GetPeriodByDate(DateTime date)
        {
            try
            {
                return PeriodExtensions.GetPeriodByDate(date);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        /// <summary>
        /// Create by FHP
        /// </summary>
        /// <returns>Lista Code, Name del Period</returns>
        public static Dictionary<string, string> GetPeriods()
        {
            try
            {
                return PeriodExtensions.GetPeriods();
            }
            catch (Exception ex)
            {

                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static DateTime GetDatePeriod(int period)
        {
            try
            {
                return PeriodExtensions.GetDatePeriod(period);
            }
            catch (Exception ex)
            {

                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }



        #region [@01 A01]

        /// <summary>
        /// Gets Three Next Periods
        /// </summary>
        /// <param name="PeriodID"></param>
        /// <returns>(Key: PeriodID, Value: Description)</returns>
        public static Dictionary<string, string> GetThreeNextPeriods(int PeriodID)
        {
            try
            {
                return PeriodsRepository.GetThreeNextPeriods(PeriodID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static Dictionary<string, string> GetThreeNextPeriodsReactiveAccount(int PeriodID, int AccountID)
        {
            try
            {
                return PeriodsRepository.GetThreeNextPeriodsReactiveAccount(PeriodID, AccountID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
        /// <summary>
        /// Gets Open Period
        /// </summary>
        /// <returns>PeriodID</returns>
        public static int GetOpenPeriodID()
        {
            try
            {
                return PeriodsRepository.GetOpenPeriodID();
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }


        /// <summary>
        /// Gets Period  by Id
        /// </summary>
        /// <returns>PeriodSearchData</returns>
        public static PeriodSearchData GetPeriodbyID(int periodoId)
        {
            try
            {
                return PeriodsRepository.GetPeriodbyID(periodoId);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        #endregion
    }
}
