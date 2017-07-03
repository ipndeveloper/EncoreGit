using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace NetSteps.Data.Entities.Repositories
{
    public class AccountPropertiesRepository
    {
        public string DeleteAccountPropertiesByAccountID(int accountID)
        {
            string resultado = string.Empty;
            using (SqlConnection connection = new SqlConnection(DataAccess.GetConnectionString("Core")))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "dbo.uspDeleteAccountPropertiesByAccountID";
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
