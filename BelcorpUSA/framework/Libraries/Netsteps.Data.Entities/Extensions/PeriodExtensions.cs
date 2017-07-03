using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;

namespace NetSteps.Data.Entities.Extensions
{
    /// <summary>
    /// Clase que accedera a la base de datos Create by - FHP
    /// </summary>
    public class PeriodExtensions
    {
        /// <summary>
        /// Método que retorna todos los periodos en base a una fecha - Crearte by - FHP
        /// </summary>
        /// <param name="date">Fecha a validar</param>
        /// <returns>Periodo en base a la fecha enviada</returns>
        //public static int GetPeriodByDate(DateTime date)
        //{
        //    return DataAccess.ExecWithStoreProcedureScalar(ConnectionStrings.BelcorpCore, "uspGetPeriodByDate",
        //         new SqlParameter("Fecha", SqlDbType.Date) { Value = date });
        //}

        public static Dictionary<int, bool> GetPeriodByDate(DateTime date)
        {
            Dictionary<int, bool> result = new Dictionary<int, bool>();
            IDataReader dr = DataAccess.ExecuteReader(DataAccess.GetCommand("uspGetPeriodByDate", new Dictionary<string, object>() { { "Fecha", date } }
                , ConnectionStrings.BelcorpCore));

            while (dr.Read())
            {
                result.Add(Convert.ToInt32(dr["Period"]), Convert.ToBoolean(dr["Bloqueado"]));
            }
            dr.Close();
            return result;
        }

        public static Dictionary<int, bool> GetNextPeriodsByAccountType(int AccountTypeID, int Offset, int? OrderID, bool IncludeThisCurrentCampaign)
        {
            //List<int> result = new List<int>();
            Dictionary<int, bool> result = new Dictionary<int, bool>();
            IDataReader dr = DataAccess.ExecuteReader(DataAccess.GetCommand("uspGetNextPeriodsByAccountType",
                                                      new Dictionary<string, object>() { { "AccountTypeID", AccountTypeID }, 
                                                                                         { "Offset", Offset },
                                                                                         { "OrderID", OrderID },
                                                                                         { "IncludeThisCurrentCampaign", IncludeThisCurrentCampaign }
                                                                                       },
                                                      ConnectionStrings.BelcorpCore));
            while (dr.Read())
            {
                result.Add(Convert.ToInt32(dr["PeriodID"]), Convert.ToBoolean(dr["Selected"]));
            }
            dr.Close();
            return result;
        }

        /// <summary>
        /// Método que retorna todos los periodos Create by - FHP
        /// </summary>
        /// <returns>Lista Code, Name del Period</returns>
        public static Dictionary<string, string> GetPeriods()
        {
            return DataAccess.ExecQueryEntidadDictionary(ConnectionStrings.BelcorpCore, "uspGetPeriod");
        }

        public static Dictionary<string, string> GetPreviousNextPeriods()
        {
            return DataAccess.ExecQueryEntidadDictionary(ConnectionStrings.BelcorpCore, "GetPreviousNextPeriods");
        }

        public static DateTime GetDatePeriod(int period)
        {
            return DataAccess.ExecWithStoreProcedureScalarType<DateTime>(ConnectionStrings.BelcorpCore, "uspGetDatePeriod",
                 new SqlParameter("PeriodID", SqlDbType.Int) { Value = period });
        }


        public static bool PrepareNextCampaign()
        {
            return DataAccess.ExecWithStoreProcedureScalarType<bool>(ConnectionStrings.BelcorpCommission, "uspPrepareNextCampaign");
        }
    }
}
