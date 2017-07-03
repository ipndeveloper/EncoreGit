using System;
using System.Data.SqlClient;

namespace NetSteps.Sql
{
    /// <summary>
    /// Author: John Egbert
    /// Created: 3/26/2010
    /// </summary>
    public static class SqlHelpers
    {
        public static string SqlGetString(string sql, SqlConnection conn)
        {
            SqlCommand sqlCommand = new SqlCommand(sql, conn);
            object value = sqlCommand.ExecuteScalar();
            if (value != null && !string.IsNullOrEmpty(value.ToString()))
                return value.ToString();
            else
                return string.Empty;
        }

        public static int SqlGetInt(string sql, SqlConnection conn)
        {
            SqlCommand sqlCommand = new SqlCommand(sql, conn);
            object value = sqlCommand.ExecuteScalar();
            if (value != null && !string.IsNullOrEmpty(value.ToString()))
                return Convert.ToInt16(value.ToString());
            else
                return 0;
        }

        public static void SqlRun(string sql, SqlConnection conn)
        {
            SqlCommand sqlCommand = new SqlCommand(sql, conn);
            object value = sqlCommand.ExecuteScalar();
        }
    }
}
