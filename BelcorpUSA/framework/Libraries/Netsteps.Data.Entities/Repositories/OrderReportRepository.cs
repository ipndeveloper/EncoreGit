using System.Data.SqlClient;

using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Repositories
{
    public class OrderReportRepository
    {
        private static string _reportConnectionString = string.Empty;
        private  static string GetReportConnectionString()
        {

            _reportConnectionString = EntitiesHelpers.GetAdoConnectionString<NetStepsEntities>();

            return _reportConnectionString;
        }
        public static DataSet OrderSearch(string orderNumber, int LanguageID)
        {
            DataSet dtOrders = new DataSet();
            SqlParameter op = null;
            SqlParameter op1 = null;
            string cadena = GetReportConnectionString();
            using (SqlConnection ocon = new SqlConnection(cadena))
            {
                ocon.Open();
                using (SqlCommand ocom = new SqlCommand())
                {
                    ocom.Connection = ocon;
                    ocom.CommandTimeout = 0;
                    ocom.CommandType = CommandType.StoredProcedure;
                    ocom.CommandText = "USP_SEL_ORDER_PRODUCT";
                    
                    op = new SqlParameter();
                    op.DbType = DbType.String;
                    op.ParameterName = "@orderNumber";
                    op.Value = orderNumber;
                    ocom.Parameters.Add(op);

                    op1 = new SqlParameter();
                    op1.DbType = DbType.Int32;
                    op1.ParameterName = "@LanguageID";
                    op1.Value = LanguageID;
                    ocom.Parameters.Add(op1);

                    using (SqlDataAdapter odata = new SqlDataAdapter())
                    {
                        odata.SelectCommand = ocom;
                        odata.Fill(dtOrders);
                        return dtOrders;
                    }
                }
            }


        }


    }
}
