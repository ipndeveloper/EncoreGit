using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace NetSteps.Data.Entities.Repositories
{
    public class CreditRequirementsRepository
    {
        public string DeleteCreditRequirementsByAccountID(int accountID)
        {
            string resultado = string.Empty;
            using (SqlConnection connection = new SqlConnection(DataAccess.GetConnectionString("Core")))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "dbo.uspDeleteCreditRequirementsByAccountID";
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
