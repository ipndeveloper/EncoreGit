using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.EntityModels;
using System.Data.SqlClient;
using System.Data;
using NetSteps.Data.Entities.Exceptions;
using System.Reflection;


namespace NetSteps.Data.Entities.Business
{
    public partial class Dispatch
    {
        public static DispatchTable DispatchById(int dispatchID)
        {
            DispatchTable dispatch = DataAccess.ExecWithStoreProcedureListParam<DispatchTable>("Core", "uspDispatchById",
                new SqlParameter("DispatchID", SqlDbType.Int) { Value = dispatchID }).ToList()[0];
           return dispatch;
        }

        public class ListtoDataTableConverter
        {

            public DataTable ToDataTable<T>(List<T> items)
            {
                DataTable dataTable = new DataTable(typeof(T).Name);
                PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo prop in Props)
                {
                    dataTable.Columns.Add(prop.Name);
                }
                foreach (T item in items)
                {
                    var values = new object[Props.Length];
                    for (int i = 0; i < Props.Length; i++)
                    {
                        values[i] = Props[i].GetValue(item, null);
                    }
                    dataTable.Rows.Add(values);
                }
                return dataTable;
            }
        }

        public static int InsertDispatch(DispatchTable entidad)
        {
            try
            {

                var result = DataAccess.ExecWithStoreProcedureScalarType<int>("Core", "uspInsertDispatch",
                    new SqlParameter("DispatchTypeID", SqlDbType.Int) { Value = entidad.DispatchTypeID },
                    new SqlParameter("DispatchStatusType", SqlDbType.Int) { Value = entidad.DispatchStatusType },
                    new SqlParameter("Description", SqlDbType.VarChar, 200) { Value = entidad.Description },
                    new SqlParameter("PeriodStart", SqlDbType.Int) { Value = entidad.PeriodStart },
                    new SqlParameter("PeriodEnd", SqlDbType.Int) { Value = (object)entidad.PeriodEnd ?? DBNull.Value },

                new SqlParameter("DateStart", SqlDbType.DateTime) { Value = (object)entidad.DateStart ?? DBNull.Value },
                new SqlParameter("DateEnd", SqlDbType.DateTime) { Value = (object)entidad.DateEnd ?? DBNull.Value },

                    new SqlParameter("OnlyTime", SqlDbType.Int) { Value = entidad.OnlyTime },
                    new SqlParameter("ListScope", SqlDbType.Int) { Value = entidad.ListScope },
                    new SqlParameter("Termname", SqlDbType.VarChar, 200) { Value = entidad.Termname },
                    new SqlParameter("SortIndex", SqlDbType.Int) { Value = entidad.SortIndex }
                    );
                return result;

            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }



        public static int UpdateDispatch(DispatchTable entidad)
        { 
            var result = DataAccess.ExecWithStoreProcedureScalarType<int>("Core", "uspUpdateDispatch", 
                new SqlParameter("DispatchID", SqlDbType.Int) { Value = entidad.DispatchID },
                new SqlParameter("DispatchTypeID", SqlDbType.Int) { Value = entidad.DispatchTypeID },
                new SqlParameter("DispatchStatusType", SqlDbType.Int) { Value = entidad.DispatchStatusType },
                new SqlParameter("Description", SqlDbType.VarChar, 200) { Value = entidad.Description },
                new SqlParameter("PeriodStart", SqlDbType.Int) { Value = entidad.PeriodStart },
                new SqlParameter("PeriodEnd", SqlDbType.Int) { Value = (object)entidad.PeriodEnd ?? DBNull.Value }, 
                new SqlParameter("DateStart", SqlDbType.DateTime) { Value = (object)entidad.DateStart ?? DBNull.Value },
                new SqlParameter("DateEnd", SqlDbType.DateTime) { Value = (object)entidad.DateEnd ?? DBNull.Value }, 
                new SqlParameter("OnlyTime", SqlDbType.Int) { Value = entidad.OnlyTime },
                new SqlParameter("ListScope", SqlDbType.Int) { Value = entidad.ListScope }, 
                new SqlParameter("Termname", SqlDbType.VarChar, 200) { Value = entidad.Termname },
                new SqlParameter("SortIndex", SqlDbType.Int) { Value = entidad.SortIndex }
                );
            return result;
        }

        /*
        * wv:20160606 Procedimiento para listar los Dispatch x Tipo
        */
        public static List<listdispatchDisplay> listdispatchDisplay()
        {
            return DataAccess.ExecWithStoreProcedureListParam<listdispatchDisplay>("Core", "listdispatchDisplay", 
                new SqlParameter("Status", SqlDbType.Int) { Value = 1 }
                ).ToList();
        }

        public static List<typedispatchDisplay> typedispatchDisplay()
        {
            return DataAccess.ExecWithStoreProcedureListParam<typedispatchDisplay>("Core", "typedispatchDisplay",
                new SqlParameter("Active", SqlDbType.Int) { Value = 1 }
                ).ToList();
        }


        public static string DeleteDispatch(DispatchTable entidad)
        {
            string resultado = string.Empty;

            using (SqlConnection connection = new SqlConnection(DataAccess.GetConnectionString("Core")))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "dbo.uspDeleteDispatch";
                    command.CommandType = CommandType.StoredProcedure;
                    SqlParameter DispatchID = command.Parameters.AddWithValue("@DispatchID", entidad.DispatchID);
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
