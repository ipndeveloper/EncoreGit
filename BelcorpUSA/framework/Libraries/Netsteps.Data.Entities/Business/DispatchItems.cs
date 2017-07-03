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
    public class DispatchItems
    {
        public static List<DispatchItemsQuery> DispatchItemsByDispatchID(int dispatchID)
        {
            List<DispatchItemsQuery> dispatch = DataAccess.ExecWithStoreProcedureListParam<DispatchItemsQuery>("Core", "uspDispatchItemsByDispatchID",
                new SqlParameter("DispatchID", SqlDbType.Int) { Value = dispatchID }).ToList();
            return dispatch;
        }

       
        public static int DeleteDispatchItemsbyID(int dispatchID)
        {
            try
            {  
                var result = DataAccess.ExecWithStoreProcedureScalarType<int>("Core", "uspDeleteDispatchItemsbyID" ,
                   new SqlParameter("DispatchID", SqlDbType.Int) { Value = dispatchID } 
                   );
                return result; 
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }
    

        public static int InsertDispatchItems(DispatchItemsTable entidad)
        {
            try
            {
                var result = DataAccess.ExecWithStoreProcedureScalarType<int>("Core", "InsertDispatchItems",
                    new SqlParameter("DispatchID", SqlDbType.Int) { Value = entidad.DispatchID },
                    new SqlParameter("ProductID", SqlDbType.Int) { Value = entidad.ProductID },
                    new SqlParameter("Qty1IOwner", SqlDbType.Int) { Value = entidad.Quantity },
                    new SqlParameter("Qty2Owne", SqlDbType.Int) { Value = (object)entidad.Quantity2 ?? DBNull.Value }
                    );
                return result;
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }
    }
}
