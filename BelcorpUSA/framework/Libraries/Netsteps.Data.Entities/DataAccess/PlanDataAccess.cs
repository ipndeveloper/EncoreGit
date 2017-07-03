using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities
{
    public static class PlanDataAccess
    {
        public static List<PlanSearchData> Search()
        {
            List<PlanSearchData> result = null;
            try
            {
                SqlDataReader reader = DataAccess.GetDataReader("upsGetPlans", null, "Core");

                if (reader.HasRows)
                {
                    result = new List<PlanSearchData>();
                    while (reader.Read())
                    {
                        result.Add(new PlanSearchData()
                        {
                            PlanID = Convert.ToInt32(reader["PlanID"]),
                            PlanCode = Convert.ToString(reader["PlanCode"]),
                            Name = Convert.ToString(reader["Name"]),
                            Enabled = Convert.ToBoolean(reader["Enabled"]),
                            DefaultPlan = Convert.ToBoolean(reader["DefaultPlan"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
            return result;
        }

        public static void UpdateEnabledPlan(int planId, bool enabledNow)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@SelectedPlanID", planId }, { "@Enabled", enabledNow } };

                SqlCommand cmd = DataAccess.GetCommand("upsUpdPlans_Enabled", parameters, "Core") as SqlCommand;
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static int Insert(PlanSearchData plan)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {  { "@PlanCode", plan.PlanCode }, 
                                                                                            { "@Name", plan.Name },
                                                                                            { "@Enabled", plan.Enabled },
                                                                                            { "@DefaultPlan", plan.DefaultPlan },
                                                                                            { "@TermName", plan.TermName }};


                SqlCommand cmd = DataAccess.GetCommand("upsInsPlan", parameters, "Core") as SqlCommand;
                cmd.Connection.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static void Update(PlanSearchData plan)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {  { "@PlanID", plan.PlanID }, 
                                                                                            { "@PlanCode", plan.PlanCode }, 
                                                                                            { "@Name", plan.Name },
                                                                                            { "@Enabled", plan.Enabled },
                                                                                            { "@DefaultPlan", plan.DefaultPlan },
                                                                                            { "@TermName", plan.TermName }};


                SqlCommand cmd = DataAccess.GetCommand("upsUpdPlan", parameters, "Core") as SqlCommand;
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
    }
}
