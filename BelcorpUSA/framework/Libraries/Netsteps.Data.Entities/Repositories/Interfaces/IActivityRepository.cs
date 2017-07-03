namespace NetSteps.Data.Entities.Repositories.Interfaces
{
    using System;
    using System.Collections.Generic;
    using NetSteps.Data.Entities.Dto;

    /// <summary>
    /// Descripcion de la interface
    /// </summary>
    public interface IActivityRepository
    {
        /// <summary>
        /// Gets activity by filters
        /// </summary>
        /// <param name="accountID">Account Id</param>
        /// <param name="periodID">Period Id</param>
        /// <returns>Activity Dto</returns>
        ActivityDto GetByFilters(int accountID, int periodID);

        /// <summary>
        /// Delete Activity
        /// </summary>
        /// <param name="activityID">Activity Id</param>
        void Delete(long activityID);

        /// <summary>
        /// Update Activity
        /// </summary>
        /// <param name="dto"></param>
        void Update(ActivityDto dto);

        /// <summary>
        /// Gets Activities count in Period
        /// </summary>
        /// <param name="accountID">Account ID</param>
        /// <param name="periodID">Period Id</param>
        /// <returns>Activities count</returns>
        int ActivitiesInPeriodLessCurrent(int accountID, int periodID, long currentActivityID);        
    }
}
