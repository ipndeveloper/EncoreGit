namespace NetSteps.Data.Entities.Repositories
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using NetSteps.Data.Entities.Dto;
    using NetSteps.Data.Entities.Repositories.Interfaces;
    using NetSteps.Data.Entities.Utility;
    using NetSteps.Data.Entities.EntityModels;

    /// <summary>
    /// Implementacion de la interface Inteface
    /// </summary>
    public partial class ActivityRepository : IActivityRepository
    {
        /// <summary>
        /// Gets activity by filters
        /// </summary>
        /// <param name="accountID">Account Id</param>
        /// <param name="periodID">Period Id</param>
        /// <returns>Activity Dto</returns>
        public ActivityDto GetByFilters(int accountID, int periodID)
        {
            using (var context = new EntityDBContext(ConnectionStrings.BelcorpCore))
            {
                var data = from a in context.Activities
                           where a.AccountID == accountID && a.PeriodID == periodID
                           select a;

                if (data == null)
                    throw new Exception("Activities not found");

                return TableToDto(data.FirstOrDefault());
            }
        }

        /// <summary>
        /// Delete Activity
        /// </summary>
        /// <param name="activityID">Activity Id</param>
        public void Delete(long activityID)
        {
            using (var context = new EntityDBContext(ConnectionStrings.BelcorpCore))
            {
                var data = (from a in context.Activities
                            where a.ActivityID == activityID
                            select a).FirstOrDefault();

                context.Activities.Remove(data);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Update Activity
        /// </summary>
        /// <param name="dto"></param>
        public void Update(ActivityDto dto)
        {
            using (var context = new EntityDBContext(ConnectionStrings.BelcorpCore))
            {
                var data = (from a in context.Activities
                            where a.ActivityID == dto.ActivityID
                            select a).FirstOrDefault();

                data = DtoToTable(dto);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Gets Activities count in Period
        /// </summary>
        /// <param name="accountID">Account ID</param>
        /// <param name="periodID">Period Id</param>
        /// <returns>Activities count</returns>
        public int ActivitiesInPeriodLessCurrent(int accountID, int periodID, long currentActivityID)
        {
            using (var context = new EntityDBContext(ConnectionStrings.BelcorpCore))
            {
                var data = (from a in context.Activities
                            where a.AccountID == accountID
                            && a.PeriodID == periodID
                            && a.ActivityID != currentActivityID
                            select a).Count();
                return data;
            }
        }

        /// <summary>
        /// Map from table to dto
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        ActivityDto TableToDto(ActivityTable table)
        {
            if (table == null)
                return null;

            return new ActivityDto()
            {
                ActivityID = table.ActivityID,
                ActivityStatusID = table.ActivityStatusID,
                PeriodID = table.PeriodID,
                IsQualified = table.IsQualified,
                AccountID = table.AccountID,
                AccountConsistencyStatusID = table.AccountConsistencyStatusID,      //INI-FIN - GR_Encore-07
                HasContinuity = table.HasContinuity                                 //INI-FIN - GR_Encore-07
            };
        }

        /// <summary>
        /// Map from dto To table
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ActivityTable DtoToTable(ActivityDto dto)
        {
            if (dto == null)
                return null;

            return new ActivityTable()
            {
                ActivityID = dto.ActivityID,
                ActivityStatusID = dto.ActivityStatusID,
                PeriodID = dto.PeriodID,
                IsQualified = dto.IsQualified,
                AccountID = dto.AccountID,
                HasContinuity = dto.HasContinuity,                              //INI-FIN - GR_Encore-07
                AccountConsistencyStatusID = dto.AccountConsistencyStatusID     //INI-FIN - GR_Encore-07
            };
        }

        public string DeleteActivitiesByAccountID(int accountID)
        {
            string resultado = string.Empty;
            using (SqlConnection connection = new SqlConnection(DataAccess.GetConnectionString("Core")))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "dbo.uspDeleteActivitiesByAccountID";
                    command.CommandType = CommandType.StoredProcedure;
                    SqlParameter DispatchID = command.Parameters.AddWithValue("@AccountID", accountID);
                    SqlParameter Error = command.Parameters.AddWithValue("@Error", "");
                    Error.Direction = ParameterDirection.Output;

                    SqlDataReader dr = command.ExecuteReader();
                    resultado = Error.Value.ToString();
                }
            }
            return resultado;
        }
    }
}