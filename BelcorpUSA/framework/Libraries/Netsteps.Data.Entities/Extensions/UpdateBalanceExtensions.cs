using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Business.Logic;

namespace NetSteps.Data.Entities.Extensions
{
    public class UpdateBalanceExtensions
    {
        /*
         * Developed By KLC - CSTI
         * BR-CC-014 - INTERESES Y  MULTAS
        */
        public static List<CTEUpdateBalance> UpdateBalance(int NumDocument)
        {
            List<CTEUpdateBalance> UpdateBalanceResult = DataAccess.ExecWithStoreProcedureListParam<CTEUpdateBalance>("Core", "spUpdateBalance",
                new SqlParameter("NumDocument", SqlDbType.Int) { Value = NumDocument }
               ).ToList();
            return UpdateBalanceResult;
        }
        private static string _reportConnectionString = string.Empty;
        private static string GetReportConnectionString()
        {

            _reportConnectionString = EntitiesHelpers.GetAdoConnectionString<NetStepsEntities>();

            return _reportConnectionString;
        }
        public static List<CTEUpdateBalance> UpdateBalancesPayments(CTEUpdateBalance dat){

       
            try
            {
                List<CTEUpdateBalance> lst = new List<CTEUpdateBalance>();
                CTEUpdateBalance obj = null;
                SqlParameter op = null;
                string cadena = GetReportConnectionString();
                using (SqlConnection ocon = new SqlConnection(cadena))
                {

                    ocon.Open();
                    using (SqlCommand ocom = new SqlCommand())
                    {
                        ocom.Connection = ocon;
                        ocom.CommandTimeout = 0;
                        ocom.CommandType = CommandType.StoredProcedure;
                        ocom.CommandText = "spUpdateBalancePayment";

                        op = new SqlParameter();
                        op.DbType = DbType.Int32;
                        op.ParameterName = "@NumDocument";
                        op.Value = dat.NumDocument;
                        ocom.Parameters.Add(op);

                        op = new SqlParameter();
                        op.DbType = DbType.Decimal;
                        op.ParameterName = "@InteresCalculado";
                        op.Value = dat.InteresCalculado;
                        ocom.Parameters.Add(op);

                        op = new SqlParameter();
                        op.DbType = DbType.Decimal;
                        op.ParameterName = "@MultaCalculada";
                        op.Value = dat.MultaCalulada;
                        ocom.Parameters.Add(op);


                        using (IDataReader iReader = ocom.ExecuteReader())
                        {
                            while (iReader.Read())
                            {
                                obj = new CTEUpdateBalance();

                                
                                    obj.FinancialAmount = Convert.ToDecimal(iReader["FinancialAmount"]);
                                    obj.TotalAmount = Convert.ToDecimal(iReader["TotalAmount"]);
                               
                                lst.Add(obj);
                            }
                            return lst;
                        }


                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        
        }

    }
}
