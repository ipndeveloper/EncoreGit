using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.SqlClient;
using System.Data;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
using NetSteps.Common.Base;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.EntityModels;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities.Extensions
{
    public partial class ResultTicketSearchExtensions
    {

        //public static List<ResultTicketSearch> SearchForAccount(int id, string valToSearch)
        //{
        //    List<ResultTicketSearch> result = DataAccess.ExecWithStoreProcedureListParam<ResultTicketSearch>("core", "dba_SearchForAccount",
        //        new SqlParameter("AccountID", SqlDbType.Int) { Value = id },
        //         new SqlParameter("ValToSearch", SqlDbType.VarChar) { Value = valToSearch }).ToList();

        //    return result;
        //}
        public static DataTable SearchForAccount(int id, string valToSearch)
        {
            IDbCommand dbCommand = null;

            try
            {
                dbCommand = DataAccess.SetCommand("dba_SearchForAccount", connectionString: EntitiesHelpers.GetAdoConnectionString<NetStepsEntities>());
                DataAccess.AddInputParameter("AccountID", id, dbCommand);
                DataAccess.AddInputParameter("ValToSearch", valToSearch, dbCommand);
                return DataAccess.GetDataTable(dbCommand);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsDataException);
            }
            finally
            {
                DataAccess.Close(dbCommand);
            }
        }
        //AccountBlockingSearchData
    }
}
