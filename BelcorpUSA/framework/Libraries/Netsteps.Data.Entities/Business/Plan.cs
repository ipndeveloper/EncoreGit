using System;
using System.Collections.Generic;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Common.Base;
using System.ComponentModel;
using NetSteps.Data.Entities.Repositories.Interfaces;
using System.Linq;
using System.Transactions;

namespace NetSteps.Data.Entities
{
    /// <summary>
    /// Plan Business Logic
    /// </summary>
    public class Plan
    {
        /// <summary>
        /// Search all Plans
        /// </summary>
        /// <returns>List<PlanSearchData></returns>
        public static List<PlanSearchData> Search()
        {
            return PlanDataAccess.Search();
        }

        /// <summary>
        /// Search Plans without parameters
        /// </summary>
        /// <returns>PaginatedList<PlanSearchData></returns>
        public static PaginatedList<PlanSearchData> Search(FilterPaginatedListParameters<PlanSearchData> searchParams)
        {
            try
            {
                return PlanRepository.Search(searchParams);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        /// <summary>
        /// Updated Enabled field of Plan
        /// </summary>
        /// <param name="planId"></param>
        /// <param name="enabledNow">True: Activate|False: Deactivate</param>
        public static void UpdateEnabledPlan(int planId, bool enabledNow)
        {
            try
            {
                PlanRepository.UpdateEnabledPlan(planId, enabledNow);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        /// <summary>
        /// Save Plan (Create or Update)
        /// </summary>
        /// <param name="plan"></param>
        public static void Save(PlanSearchData plan)
        {
            try
            {
                PlanRepository.Save(plan);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        #region GNH modified
        #region properties

        /// <summary>
        /// Gets or sets plan Id
        /// </summary>
        [Browsable(false)]
        public int PlanId { get; set; }

        /// <summary>
        /// Gets or sets plan code
        /// </summary>
        [Browsable(false)]
        public string PlanCode { get; set; }

        /// <summary>
        /// Gets or sets name
        /// </summary>
        [Browsable(false)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is enabled
        /// </summary>
        [Browsable(false)]
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether plan is default
        /// </summary>
        [Browsable(false)]
        public bool? DefaultPlan { get; set; }

        /// <summary>
        /// Gets or sets Term Name
        /// </summary>
        [Browsable(false)]
        public string TermName { get; set; }
        #endregion

        #endregion

        public static void ChangeStatusShippingOrderTypes(List<int> items, bool enabledNow)
        {
            try
            {
                using (var mainScope = new TransactionScope(TransactionScopeOption.Required))
                {
                    foreach (var item in items)
                    {
                        PlanRepository.ChangeStatusShippingOrderTypes(item, enabledNow);  
                    }
                    mainScope.Complete();
                    mainScope.Dispose();
                }                
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
    }
}
