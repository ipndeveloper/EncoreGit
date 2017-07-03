namespace NetSteps.Data.Entities.Repositories.Interfaces
{
    using System.Collections.Generic;
    using NetSteps.Data.Entities.Dto;

    /// <summary>
    /// Interface for AccountPerformanceData
    /// </summary>
    public interface IAccountPerformanceDataRepository
    {
        /// <summary>
        /// Get all Picked Accounts
        /// </summary>
        /// <param name="titleId">Title Id</param>
        /// <param name="planId">Plan Id</param>
        /// <param name="countryId">Country Id</param>
        /// <returns>List of Account Performance Data DTO</returns>
        IEnumerable<AccountPerformanceDataDto> GetPickedAccounts(int titleId, int planId, int countryId); 
    }
}
