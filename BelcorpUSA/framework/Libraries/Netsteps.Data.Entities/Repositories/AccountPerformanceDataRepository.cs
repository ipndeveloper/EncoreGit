namespace NetSteps.Data.Entities.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using NetSteps.Data.Entities.Repositories.Interfaces;
    using NetSteps.Data.Entities.Dto;
    using System.Data.SqlClient;
    using System.Data;

    /// <summary>
    /// Implementation for IAccountPerformanceDataRepository
    /// </summary>
    public partial class AccountPerformanceDataRepository : IAccountPerformanceDataRepository
    {
        /// <summary>
        /// Get all Picked Accounts
        /// </summary>
        /// <param name="titleId">Title Id</param>
        /// <param name="planId">Plan Id</param>
        /// <param name="countryId">Country Id</param>
        /// <returns>List of Account Performance Data DTO</returns>
        public IEnumerable<AccountPerformanceDataDto> GetPickedAccounts(int titleId, int planId, int countryId)
        {           
            List<AccountPerformanceDataDto> data = DataAccess.ExecWithStoreProcedureListParam<AccountPerformanceDataDto>(ConnectionStrings.BelcorpCommission, "uspAccountsPerformanceByFilter",
            new SqlParameter("TitleId", SqlDbType.Int) { Value = titleId },
            new SqlParameter("PlanId", SqlDbType.Int) { Value = planId },
            new SqlParameter("CountryId", SqlDbType.Int) { Value = countryId }
            ).ToList();

            if (data == null)
                throw new Exception("Selected accounts not found");

            return data;         
        }
    }
}
