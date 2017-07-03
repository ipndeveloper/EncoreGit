using System;
using System.Data.EntityClient;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class ErrorLogRepository
    {
        public PaginatedList<ErrorLog> Search(ErrorLogSearchParameters searchParameters)
        {
            return Search(searchParameters, NetStepsEntities.ConnectionString);
        }
        public PaginatedList<ErrorLog> Search(ErrorLogSearchParameters searchParameters, string connectionString)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                EntityConnection entityConnection = null;
                if (!connectionString.StartsWith("name="))
                    entityConnection = connectionString.GetEntityConnection();
                else
                    entityConnection = new EntityConnection(connectionString);

                using (NetStepsEntities context = new NetStepsEntities(entityConnection))
                {
                    PaginatedList<ErrorLog> results = new PaginatedList<ErrorLog>(searchParameters);

                    var matchingItems = from a in context.ErrorLogs
                                        select a;

                    if (!searchParameters.SessionID.IsNullOrEmpty())
                        matchingItems = from a in matchingItems
                                        where a.SessionID == searchParameters.SessionID
                                        select a;

                    if (!searchParameters.MachineName.IsNullOrEmpty())
                        matchingItems = from a in matchingItems
                                        where a.MachineName.Contains(searchParameters.MachineName)
                                        select a;
                    int errorLogID = 0;
                    if (!searchParameters.Message.IsNullOrEmpty())
                    {
                        if (searchParameters.Message.IsValidInt())
                        {
                            errorLogID = Convert.ToInt32(searchParameters.Message);
                            matchingItems = from a in matchingItems
                                            where (a.Message.Contains(searchParameters.Message) || errorLogID == a.ErrorLogID)
                                            select a;
                        }
                        else
                        {
                            matchingItems = from a in matchingItems
                                            where (a.Message.Contains(searchParameters.Message) ||
                                            a.StackTrace.Contains(searchParameters.Message) ||
                                            a.TargetSite.Contains(searchParameters.Message) ||
                                            a.Referrer.Contains(searchParameters.Message) ||
                                            a.PublicMessage.Contains(searchParameters.Message) ||
                                            a.BrowserInfo.Contains(searchParameters.Message) ||
                                            a.InternalMessage.Contains(searchParameters.Message) ||
                                            a.Source.Contains(searchParameters.Message))
                                            select a;
                        }
                    }

                    if (!searchParameters.StackTrace.IsNullOrEmpty())
                        matchingItems = from a in matchingItems
                                        where a.StackTrace.Contains(searchParameters.StackTrace)
                                        select a;

                    if (searchParameters.ApplicationID.HasValue)
                        matchingItems = from a in matchingItems
                                        where a.ApplicationID == searchParameters.ApplicationID.Value
                                        select a;

                    if (searchParameters.AccountID.HasValue)
                        matchingItems = from a in matchingItems
                                        where a.AccountID == searchParameters.AccountID.Value
                                        select a;

                    if (searchParameters.UserID.HasValue)
                        matchingItems = from a in matchingItems
                                        where a.UserID == searchParameters.UserID.Value
                                        select a;

                    if (searchParameters.OrderID.HasValue)
                        matchingItems = from a in matchingItems
                                        where a.OrderID == searchParameters.OrderID.Value
                                        select a;

                    matchingItems = matchingItems.ApplyDateRangeFilters("LogDateUTC", searchParameters);

                    if (searchParameters.WhereClause != null)
                        matchingItems = matchingItems.Where(searchParameters.WhereClause);

                    matchingItems = matchingItems.ApplyOrderByFilters(searchParameters, a => a.LogDateUTC, context);

                    // TotalCount must be set before applying Pagination - JHE
                    results.TotalCount = matchingItems.Count();

                    matchingItems = matchingItems.ApplyPagination(searchParameters);

                    var errorLogs = from a in matchingItems
                                    select a;

                    foreach (var a in errorLogs.ToList())
                        results.Add(a);

                    return results;
                }
            });
        }

        public PaginatedList<ErrorLog> GetPopularErrors(DateRangeSearchParameters searchParameters)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    PaginatedList<ErrorLog> results = new PaginatedList<ErrorLog>(searchParameters);

                    var matchingItems = from a in context.ErrorLogs
                                        select a;

                    matchingItems = matchingItems.ApplyDateRangeFilters("LogDateUTC", searchParameters);

                    matchingItems = matchingItems.ApplyOrderByFilters(searchParameters, a => a.LogDateUTC, context);

                    results.TotalCount = matchingItems.Count();

                    if (searchParameters.PageSize.HasValue)
                        matchingItems = matchingItems.Skip(searchParameters.PageIndex * searchParameters.PageSize.Value).Take(searchParameters.PageSize.Value);

                    var errorLogs = from a in matchingItems
                                    select a;

                    foreach (var a in errorLogs.ToList())
                        results.Add(a);

                    return results;
                }
            });
        }
    }
}
