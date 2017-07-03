using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using NetSteps.Common.Globalization;

namespace NetSteps.Data.Entities.Extensions
{
    public class PostalCodeExtesions
    {
        public static List<PostalCodeData> GetPostaCode(string PostalCode, int Country)
        {
           var Results= DataAccess.ExecWithStoreProcedureListParam<PostalCodeData>("Core", "uspGetPostaCodeByCountry",
                new SqlParameter("PostalCode", SqlDbType.VarChar) { Value = PostalCode },
                new SqlParameter("CountryID", SqlDbType.Int) { Value = Country }
                ).ToList();

            return Results;
        }

        public static List<PostalCodeData> GetPostaCodeByAccountID(string PostalCode, int Country, int AccountID, int AddressID = 0)
        {
            var Results = DataAccess.ExecWithStoreProcedureListParam<PostalCodeData>("Core", "uspGetPostaCodeByCountry",
                 new SqlParameter("PostalCode", SqlDbType.VarChar) { Value = PostalCode },
                 new SqlParameter("CountryID", SqlDbType.Int) { Value = Country },
                 new SqlParameter("AccountID", SqlDbType.Int) { Value = AccountID },
                 new SqlParameter("AddressID", SqlDbType.Int) { Value = AddressID }
                 ).ToList();

            return Results;
        }
    }
}
