namespace NetSteps.Data.Entities.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using NetSteps.Data.Entities.Dto;
    using NetSteps.Data.Entities.Repositories.Interfaces;
    using System.Data.SqlClient;
    using System.Data;
    using NetSteps.Data.Entities.Exceptions;
    using NetSteps.Data.Entities.Business;
    using NetSteps.Common.Base;
    using System.Configuration;

    public partial class ManualBonusEntryRepository : IManualBonusEntryRepository
    {
        public List<ManualBonusEntrySearchData> ManualBonusEntryValidation(DataTable values)
        {
            try
            {
                List<ManualBonusEntrySearchData> DataBaseErrors = new PaginatedList<ManualBonusEntrySearchData>();

                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Commissions"].ConnectionString))
                {
                    connection.Open();

                    SqlParameter parameter = new SqlParameter();
                    parameter.ParameterName = "@Values";
                    parameter.SqlDbType = SqlDbType.Structured;
                    parameter.Value = values;

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[ManualBonusEntryValidation]";
                    cmd.Parameters.Add(parameter);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                ManualBonusEntrySearchData Row = new ManualBonusEntrySearchData();

                                #region [Assign Values]

                                Row.RowNumber = Convert.ToInt32(reader["RowNumber"]);
                                Row.Period = Convert.ToBoolean(reader["Period"]);
                                Row.BonusType = Convert.ToBoolean(reader["BonusType"]);
                                Row.Account = Convert.ToBoolean(reader["Account"]);
                                Row.BonusAmount = Convert.ToBoolean(reader["BonusAmount"]);
                                Row.Duplicated = Convert.ToBoolean(reader["Duplicated"]);

                                #endregion

                                DataBaseErrors.Add(Row);
                            }
                        }
                    }
                }

                return DataBaseErrors;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public Tuple<int, string> ManualBonusEntryLoad(DataTable values)
        {
            Tuple<int, string> result;
            int code = -1;
            string message = string.Empty;

            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Commissions"].ConnectionString))
                {
                    connection.Open();

                    SqlParameter parameter = new SqlParameter();
                    parameter.ParameterName = "@Values";
                    parameter.SqlDbType = SqlDbType.Structured;
                    parameter.Value = values;

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[ManualBonusEntryLoad]";
                    cmd.Parameters.Add(parameter);

                    code = cmd.ExecuteNonQuery();
                    message = "Executed.";
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            result = new Tuple<int, string>(code, message);
            return result;
        }
    }
}
