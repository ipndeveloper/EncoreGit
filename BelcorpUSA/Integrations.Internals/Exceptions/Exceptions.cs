using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Data;
//using NetstepsDataAccess.DataEntities;

namespace NetstepsDataAccess.Exceptions
{
    public class Exceptions
    {

        /*public static T Run<T>(ExecutionContext executionContext, Func<T> action)
        {
            try
            {
                if (action != null)
                    return action();
                else
                {
                    using (NetStepsEntities db = new NetStepsEntities())
                    {
                        db.UspErrorLogsInsert(DateTime.Now, "No Action set in ExceptionHandledDataAction", "NetstepsDataAccess.Exceptions",
                            "Action must be set in ExceptionHandledDataAction");
                    }
                    throw new Exception("Action must be set in ExceptionHandledDataAction.");
                }
            }
            catch (OptimisticConcurrencyException ex)
            {
                using (NetStepsEntities db = new NetStepsEntities())
                {
                    db.UspErrorLogsInsert(DateTime.Now, "OptimisticConcurrencyException", "NetstepsDataAccess.Exceptions",
                                ex.Message);
                }
                throw ex;
            }
            catch (Exception ex)
            {
                using (NetStepsEntities db = new NetStepsEntities())
                {
                    db.UspErrorLogsInsert(DateTime.Now, "No Action set in ExceptionHandledDataAction", "NetstepsDataAccess.Exceptions",
                                ex.Message);
                }
                throw ex;
            }
        }*/
    }
}
