using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.EntityModels;
using System.Data.SqlClient;
using System.Data;
using NetSteps.Data.Entities.Exceptions;
namespace NetSteps.Data.Entities.Business
{
    public class DispatchItemsList
    {
        public static List<DispatchsItemsListTable> DispatchListItemsByDispatchListID(int dispatchListID)
        {
            List<DispatchsItemsListTable> dispatch = DataAccess.ExecWithStoreProcedureListParam<DispatchsItemsListTable>("Core", "uspDispatchListItemsByDispatchListID",
                new SqlParameter("DispatchListID", SqlDbType.Int) { Value = dispatchListID }).ToList();
            return dispatch;
        }

       
        public static int DeleteDispatchListItemsbyID(int dispatchID)
        {
            try
            {
                int Result = 0;
                using (SqlConnection connection = new SqlConnection(DataAccess.GetConnectionString("Core")))
                {
                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "dbo.uspDeleteDispatchListItemsbyID";
                        command.CommandType = CommandType.StoredProcedure;

                        SqlParameter P1;
                        P1 = command.Parameters.AddWithValue("@DispatchListID", dispatchID);
                        Result = command.ExecuteNonQuery();
                    }
                }
                return Result;
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }


        public static int InsertDispatchListItems(DispatchsItemsParameters entidad)
        {
            try
            {
                var insert = DataAccess.ExecWithStoreProcedureScalarType<int>("Core", "uspInstDispatchListItems",
               new SqlParameter("AccountNumber", SqlDbType.VarChar) { Value = entidad.AccountNumber },
               new SqlParameter("DispatchListId", SqlDbType.Int) { Value = entidad.DispatchListID });

                return insert;
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }
    }
}

