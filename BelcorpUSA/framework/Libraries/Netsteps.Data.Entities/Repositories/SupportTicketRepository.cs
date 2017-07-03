using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Common.Exceptions;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using System.Data.SqlClient;
using System.Data;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
using System.IO;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class SupportTicketRepository : ISupportTicketRepository
    {
        #region Members
        protected override Func<NetStepsEntities, IQueryable<SupportTicket>> loadAllFullQuery
        {
            get
            {
                return CompiledQuery.Compile<NetStepsEntities, IQueryable<SupportTicket>>(
                   (context) => from a in context.SupportTickets
                                               .Include("Account")
                                               .Include("Account.AccountPhones")
                                               .Include("Account.Addresses")
                                select a);
            }
        }
        #endregion

        #region Methods

        public SupportTicket LoadBySupportTicketNumber(string supportTicketNumber)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var result = context.SupportTickets.FirstOrDefault(o => o.SupportTicketNumber == supportTicketNumber);

                    if (result == null)
                        throw new NetStepsDataException("Error loading support ticket. Invalid supportTicketNumber: " + supportTicketNumber);
                    else
                        return result;
                }
            });
        }

        public SupportTicket LoadBySupportTicketNumberFull(string supportTicketNumber)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var result = loadAllFullQuery(context).FirstOrDefault(o => o.SupportTicketNumber == supportTicketNumber);

                    if (result == null)
                        throw new NetStepsDataException("Error loading support ticket. Invalid supportTicketNumber: " + supportTicketNumber);
                    else
                        return result;
                }
            });
        }

        public SupportTicket RequestNewTicket(int assignedUserID)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var result = loadAllFullQuery(context).OrderBy(t => t.SupportTicketPriority.SortIndex).ThenBy(t => t.DateCreatedUTC).FirstOrDefault(t => t.AssignedUserID == null);



                    if (result == null)
                    {
                        return null;
                    }
                    else
                    {
                        result.StartEntityTracking();

                        result.AssignedUserID = assignedUserID;

                        result.SupportTicketStatusID = Constants.SupportTicketStatus.Assigned.ToShort();

                        result.Save();

                        return result;
                    }

                }
            });
        }

        //Anterior
        /*
        public PaginatedList<SupportTicketSearchData> Search(SupportTicketSearchParameters searchParameters)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    PaginatedList<SupportTicketSearchData> results = new PaginatedList<SupportTicketSearchData>(searchParameters);

                    IQueryable<SupportTicket> matchingSupportTickets = context.SupportTickets;

                    if (searchParameters.AccountID.HasValue)
                    {
                        matchingSupportTickets = matchingSupportTickets.Where(u => u.AccountID == searchParameters.AccountID.Value);
                    }

                    if (searchParameters.AssignedUserID.HasValue)
                    {
                        matchingSupportTickets = matchingSupportTickets.Where(u => u.AssignedUserID == searchParameters.AssignedUserID.Value);
                    }

                    if (searchParameters.SupportTicketID.HasValue)
                    {
                        matchingSupportTickets = matchingSupportTickets.Where(u => u.SupportTicketID == searchParameters.SupportTicketID.Value);
                    }

                    if (!string.IsNullOrEmpty(searchParameters.SupportTicketNumber))
                    {
                        matchingSupportTickets = matchingSupportTickets.Where(u => u.SupportTicketNumber.Contains(searchParameters.SupportTicketNumber));
                    }

                    if (!string.IsNullOrEmpty(searchParameters.Title))
                    {
                        matchingSupportTickets = matchingSupportTickets.Where(u => u.Title.Contains(searchParameters.Title));
                    }

                    if (searchParameters.SupportTicketPriorityID.HasValue)
                    {
                        matchingSupportTickets = matchingSupportTickets.Where(u => u.SupportTicketPriorityID == searchParameters.SupportTicketPriorityID);
                    }

                    if (searchParameters.SupportTicketCategoryID.HasValue)
                    {
                        matchingSupportTickets = matchingSupportTickets.Where(u => u.SupportTicketCategoryID == searchParameters.SupportTicketCategoryID);
                    }

                    if (searchParameters.IsVisibleToOwner.HasValue)
                    {
                        matchingSupportTickets = matchingSupportTickets.Where(u => u.IsVisibleToOwner == searchParameters.IsVisibleToOwner);
                    }

                    if (searchParameters.SupportTicketStatusOpen.ToBool())
                    {
                        matchingSupportTickets = matchingSupportTickets.Where(u => u.SupportTicketStatusID != (short)Constants.SupportTicketStatus.Resolved);
                    }
                    else if (searchParameters.SupportTicketStatusID.HasValue)
                    {
                        matchingSupportTickets = matchingSupportTickets.Where(u => u.SupportTicketStatusID == searchParameters.SupportTicketStatusID);
                    }

                    if (searchParameters.StartDate.HasValue)
                    {
                        DateTime startDateUTC = searchParameters.StartDate.Value.StartOfDay().LocalToUTC();
                        matchingSupportTickets = matchingSupportTickets.Where(a => a.DateCreatedUTC >= startDateUTC);
                    }

                    if (searchParameters.EndDate.HasValue)
                    {
                        DateTime endDateUTC = searchParameters.EndDate.Value.EndOfDay().LocalToUTC();
                        matchingSupportTickets = matchingSupportTickets.Where(a => a.DateCreatedUTC <= endDateUTC);
                    }

                    if (searchParameters.WhereClause != null)
                        matchingSupportTickets = matchingSupportTickets.Where(searchParameters.WhereClause);

                    if (!searchParameters.OrderBy.IsNullOrEmpty())
                    {

                        switch (searchParameters.OrderBy)
                        {
                            case "AssignedUsername":

                                var termNA = Translation.GetTerm("NA");

                                matchingSupportTickets = matchingSupportTickets.ApplyOrderByFilter(searchParameters.OrderByDirection, t => t.AssignedUserID == null ? termNA : t.AssignedUser.Username);
                                break;
                            case "FirstName":
                                matchingSupportTickets = matchingSupportTickets.ApplyOrderByFilter(searchParameters.OrderByDirection, t => t.Account.FirstName);
                                break;
                            case "LastName":
                                matchingSupportTickets = matchingSupportTickets.ApplyOrderByFilter(searchParameters.OrderByDirection, t => t.Account.LastName);
                                break;
                            default:
                                matchingSupportTickets = searchParameters.OrderByDirection == Constants.SortDirection.Ascending ? matchingSupportTickets.OrderBy(searchParameters.OrderBy) : matchingSupportTickets.OrderByDescending(searchParameters.OrderBy);
                                break;
                        }

                    }
                    else
                    {
                        matchingSupportTickets = searchParameters.OrderByDirection == Constants.SortDirection.Ascending ? matchingSupportTickets.OrderBy(u => u.SupportTicketNumber) : matchingSupportTickets.OrderByDescending(u => u.SupportTicketNumber);
                    }

                    results.TotalCount = matchingSupportTickets.Count();

                    // Apply Paging filter - JHE
                    if (searchParameters.PageSize.HasValue)
                    {
                        matchingSupportTickets = matchingSupportTickets.Skip(searchParameters.PageIndex * searchParameters.PageSize.Value).Take(searchParameters.PageSize.Value);
                    }

                    var supportTicketInfos = matchingSupportTickets.Select(u => new
                    {
                        u.AccountID,
                        u.SupportTicketNumber,
                        u.AssignedUserID,
                        u.SupportTicketID,
                        u.Title,
                        u.SupportTicketPriorityID,
                        u.SupportTicketStatusID,
                        u.SupportTicketCategoryID,
                        u.DateCreatedUTC,
                        u.DateLastModifiedUTC
                    });

                    foreach (var a in supportTicketInfos.ToList())
                    {
                        var account = a.AccountID == 0 ? null : CachedData.GetAccountSlimSearch(a.AccountID);
                        var assignedUser = a.AssignedUserID == null || a.AssignedUserID == 0 ? null : CachedData.GetUser(a.AssignedUserID.ToInt());

                        results.Add(new SupportTicketSearchData()
                        {
                            DateCreated = a.DateCreatedUTC.UTCToLocal(),
                            DateLastModified = a.DateLastModifiedUTC.UTCToLocal(),
                            AccountNumber = account == null ? Translation.GetTerm("NA") : account.AccountNumber,
                            FirstName = account == null ? Translation.GetTerm("NA") : account.FirstName,
                            LastName = account == null ? Translation.GetTerm("NA") : account.LastName,
                            SupportTicketCategoryID = a.SupportTicketCategoryID.ToShort(),
                            SupportTicketPriorityID = a.SupportTicketPriorityID,
                            SupportTicketStatusID = a.SupportTicketStatusID,
                            SupportTicketID = a.SupportTicketID,
                            SupportTicketNumber = a.SupportTicketNumber,
                            AssignedUsername = assignedUser == null ? Translation.GetTerm("NA") : assignedUser.Username,
                            Title = a.Title
                        });
                    }

                    return results;
                }
            });
        }
        */
        public PaginatedList<SupportTicketSearchData> Search(SupportTicketSearchParameters searchParameters)
        {
           
                    PaginatedList<SupportTicketSearchData> results = new PaginatedList<SupportTicketSearchData>(searchParameters);
                    List<SupportTicketsBE> lstSupportTicketsBE = ObtenerSupportTicketsBE(searchParameters.IsSiteDWS);

                   // IQueryable<SupportTicket> matchingSupportTickets = context.SupportTickets;
                    IEnumerable<SupportTicketsBE> ESupportTicketsBE = lstSupportTicketsBE.AsEnumerable();
                   
                    /*CS.20AG2016.Inicio. Filtrar por Order Number y/o Invoice Number*/
                    if (!string.IsNullOrEmpty(searchParameters.OrderNumber))
                        ESupportTicketsBE = ESupportTicketsBE.Where(u => u.OrderNumber == searchParameters.OrderNumber);

                    if (!string.IsNullOrEmpty(searchParameters.InvoiceNumber))
                        ESupportTicketsBE = ESupportTicketsBE.Where(u => u.InvoiceNumber == searchParameters.InvoiceNumber);
                    /*CS.20AG2016.Fin*/

                    if (searchParameters.AccountID.HasValue)
                    {
                        //  matchingSupportTickets = lstSupportTicketsBE.Where(u => u.AccountID == searchParameters.AccountID.Value);
                        ESupportTicketsBE = ESupportTicketsBE.Where(u => u.AccountID == searchParameters.AccountID.Value);
                    
                    }

                    if (searchParameters.AssignedUserID.HasValue)
                    {

                       // matchingSupportTickets = matchingSupportTickets.Where(u => u.AssignedUserID == searchParameters.AssignedUserID.Value);
                        ESupportTicketsBE = ESupportTicketsBE.Where(u => u.AssignedUserID == searchParameters.AssignedUserID.Value);
                    }

                    if (searchParameters.SupportTicketID.HasValue)
                    {
                  //      matchingSupportTickets = matchingSupportTickets.Where(u => u.SupportTicketID == searchParameters.SupportTicketID.Value);
                        ESupportTicketsBE = ESupportTicketsBE.Where(u => u.SupportTicketID == searchParameters.SupportTicketID.Value);
                    }

                    if (!string.IsNullOrEmpty(searchParameters.SupportTicketNumber))
                    {
                      //  matchingSupportTickets = matchingSupportTickets.Where(u => u.SupportTicketNumber.Contains(searchParameters.SupportTicketNumber));
                        ESupportTicketsBE = ESupportTicketsBE.Where(u => u.SupportTicketNumber.Contains(searchParameters.SupportTicketNumber));
                    }

                    if (!string.IsNullOrEmpty(searchParameters.Title))
                    {
                    //    matchingSupportTickets = matchingSupportTickets.Where(u => u.Title.Contains(searchParameters.Title));
                        ESupportTicketsBE = ESupportTicketsBE.Where(u => u.Title.Contains(searchParameters.Title));
                    }

                    if (searchParameters.SupportTicketPriorityID.HasValue)
                    {
                        //matchingSupportTickets = matchingSupportTickets.Where(u => u.SupportTicketPriorityID == searchParameters.SupportTicketPriorityID);
                        ESupportTicketsBE = ESupportTicketsBE.Where(u => u.SupportTicketPriorityID == searchParameters.SupportTicketPriorityID);
                    }

                    //if (searchParameters.SupportTicketCategoryID.HasValue)
                    //{
                    ////    matchingSupportTickets = matchingSupportTickets.Where(u => u.SupportTicketCategoryID == searchParameters.SupportTicketCategoryID);
                    //    ESupportTicketsBE = ESupportTicketsBE.Where(u => u.SupportTicketCategoryID == searchParameters.SupportTicketCategoryID);
                    //}

                    if (searchParameters.IsVisibleToOwner.HasValue)
                    {
                        //matchingSupportTickets = matchingSupportTickets.Where(u => u.IsVisibleToOwner == searchParameters.IsVisibleToOwner);
                        ESupportTicketsBE = ESupportTicketsBE.Where(u => u.IsVisibleToOwner == searchParameters.IsVisibleToOwner);
                    }

                    if (searchParameters.SupportTicketStatusOpen.ToBool())
                    {
                      //  matchingSupportTickets = matchingSupportTickets.Where(u => u.SupportTicketStatusID != (short)Constants.SupportTicketStatus.Resolved);
                        ESupportTicketsBE = ESupportTicketsBE.Where(u => u.SupportTicketStatusID != (short)Constants.SupportTicketStatus.Resolved);
                    }
                    else if (searchParameters.SupportTicketStatusID.HasValue)
                    {
                        //matchingSupportTickets ESupportTicketsBE matchingSupportTickets.Where(u => u.SupportTicketStatusID == searchParameters.SupportTicketStatusID);
                        ESupportTicketsBE = ESupportTicketsBE.Where(u => u.SupportTicketStatusID == searchParameters.SupportTicketStatusID);
                    }

                    if (searchParameters.StartDate.HasValue)
                    {
                        DateTime startDateUTC = searchParameters.StartDate.Value.StartOfDay().LocalToUTC();
                        //matchingSupportTickets = matchingSupportTickets.Where(a => a.DateCreatedUTC >= startDateUTC);
                        ESupportTicketsBE = ESupportTicketsBE.Where(a => a.DateCreatedUTC >= startDateUTC);
                   
                    }

                    if (searchParameters.EndDate.HasValue)
                    {
                        DateTime endDateUTC = searchParameters.EndDate.Value.EndOfDay().LocalToUTC();
                        //matchingSupportTickets = matchingSupportTickets.Where(a => a.DateCreatedUTC <= endDateUTC);
                        ESupportTicketsBE = ESupportTicketsBE.Where(a => a.DateCreatedUTC <= endDateUTC);
                    }

                    searchParameters.SupportLevelID = searchParameters.SupportLevelID==0 ? null : searchParameters.SupportLevelID;
                    if (searchParameters.SupportLevelID   != null  )
                    {
                        // matchingSupportTickets = matchingSupportTickets.Where(searchParameters.WhereClause);
                        ESupportTicketsBE = ESupportTicketsBE.Where(a => a.SupportLevelID == searchParameters.SupportLevelID);
                    }
                    searchParameters.SupportMotiveID = searchParameters.SupportMotiveID == 0 ? null : searchParameters.SupportMotiveID;
                 
                    if (searchParameters.SupportMotiveID != null)
                    {
                        // matchingSupportTickets = matchingSupportTickets.Where(searchParameters.WhereClause);
                        ESupportTicketsBE = ESupportTicketsBE.Where(a => a.SupportMotiveID == searchParameters.SupportMotiveID);
                    }
                    if (searchParameters.WhereClause != null)
                    {
                        // matchingSupportTickets = matchingSupportTickets.Where(searchParameters.WhereClause);
                      //  ESupportTicketsBE = lstSupportTicketsBE.asquer.Where(searchParameters.WhereClause);
                    }

                    if (!searchParameters.OrderBy.IsNullOrEmpty())
                    {
                         
                        switch (searchParameters.OrderBy)
                        {
                            case "AssignedUsername":

                                var termNA = Translation.GetTerm("NA");

                            ESupportTicketsBE = ESupportTicketsBE.AsQueryable().ApplyOrderByFilter(searchParameters.OrderByDirection, t => t.AssignedUserID == null ? termNA : t.Username);
                            break;

                            case "SupportTicketCategoryID":
                                ESupportTicketsBE = ESupportTicketsBE.AsQueryable().ApplyOrderByFilter(searchParameters.OrderByDirection, t => t.SupportLevelMotive);
                                break;
                            case "FirstName":
                                ESupportTicketsBE = ESupportTicketsBE.AsQueryable().ApplyOrderByFilter(searchParameters.OrderByDirection, t => t.FirstName);
                                break;
                            case "LastName":
                                ESupportTicketsBE = ESupportTicketsBE.AsQueryable().ApplyOrderByFilter(searchParameters.OrderByDirection, t => t.LastName);
                                break;
                            default:
                                ESupportTicketsBE = searchParameters.OrderByDirection == Constants.SortDirection.Ascending ? ESupportTicketsBE.AsQueryable().OrderBy(searchParameters.OrderBy) : ESupportTicketsBE.AsQueryable().OrderByDescending(searchParameters.OrderBy);
                                break;
                        }

                    }
                    else
                    {
                        ESupportTicketsBE = searchParameters.OrderByDirection == Constants.SortDirection.Ascending ? ESupportTicketsBE.AsQueryable().OrderBy(u => u.SupportTicketNumber) : ESupportTicketsBE.AsQueryable().OrderByDescending(u => u.SupportTicketNumber);
                    }

                    results.TotalCount = ESupportTicketsBE.Count();

                    // Apply Paging filter - JHE
                    if (searchParameters.PageSize.HasValue)
                    {
                        ESupportTicketsBE = ESupportTicketsBE.Skip(searchParameters.PageIndex * searchParameters.PageSize.Value).Take(searchParameters.PageSize.Value);
                    }

                    var supportTicketInfos = ESupportTicketsBE.Select(u => new
                    {
                        u.AccountID,
                        u.SupportTicketNumber,
                        u.AssignedUserID,
                        u.SupportTicketID,
                        u.Title,
                        u.OrderNumber,
                        u.State,
                        u.City,
                        u.InvoiceNumber,
                        u.SupportTicketPriorityID,
                        u.SupportTicketStatusID,
                        u.SupportTicketCategoryID,
                        u.DateCreatedUTC,
                        u.DateLastModifiedUTC,
                        u.SupportLevelMotive,
                        u.IsSiteDWS
                    });

                    foreach (var a in supportTicketInfos.ToList())
                    {
                        var account = a.AccountID == 0 ? null : CachedData.GetAccountSlimSearch(a.AccountID);
                        var assignedUser = a.AssignedUserID == null || a.AssignedUserID == 0 ? null : CachedData.GetUser(a.AssignedUserID);

                        results.Add(new SupportTicketSearchData()
                        {
                            DateCreated = a.DateCreatedUTC.UTCToLocal(),
                            DateLastModified = a.DateLastModifiedUTC.UTCToLocal(),
                            AccountNumber = account == null ? Translation.GetTerm("NA") : account.AccountNumber,
                            FirstName = account == null ? Translation.GetTerm("NA") : account.FirstName,
                            LastName = account == null ? Translation.GetTerm("NA") : account.LastName,
                            SupportTicketCategoryID = 0,// a.SupportTicketCategoryID.ToShort(),
                            OrderNumber = a.OrderNumber,
                            State = a.State,
                            City = a.City,
                            InvoiceNumber= a.InvoiceNumber,
                            SupportTicketPriorityID = a.SupportTicketPriorityID,
                            SupportTicketStatusID = a.SupportTicketStatusID,
                            SupportTicketID = a.SupportTicketID,
                            SupportTicketNumber = a.SupportTicketNumber,
                            SupportLevelMotive=a.SupportLevelMotive,
                            AssignedUsername = assignedUser == null ? Translation.GetTerm("NA") : assignedUser.Username,
                            Title = a.Title, 
                            IsSiteDWS= Convert.ToByte(a.IsSiteDWS),
                        });
                    }

                    return results;
            //    }
          // });
        }

         
        public override PaginatedList<AuditLogRow> GetAuditLog(int primaryKey, AuditLogSearchParameters searchParameters)
        {
            ValidatePrimaryKeyForLoad(primaryKey);

            var supportTicket = SupportTicket.LoadFull(primaryKey);

            return GetAuditLog(supportTicket, searchParameters);
        }

        public PaginatedList<AuditLogRow> GetAuditLog(SupportTicket fullyLoadedSupportTicket, AuditLogSearchParameters searchParameters)
        {
            List<AuditTableValueItem> list = new List<AuditTableValueItem>();
            list.Add(new AuditTableValueItem()
            {
                TableName = EntitySetName,
                PrimaryKey = Convert.ToInt32(fullyLoadedSupportTicket.SupportTicketID)
            });

            return GetAuditLog(list, searchParameters);
        }

        #region CSTI -FHP

        public static PaginatedList<SupportTicketSearchDetailsData> SearchDetails(SupportTicketSearchDetailsParameter searchParameter)
        {
            List<SupportTicketSearchDetailsData> paginatedResult = DataAccess.ExecWithStoreProcedureListParam<SupportTicketSearchDetailsData>("Core", "uspListSupportTicket",
                 
                new SqlParameter("TypeConsult", SqlDbType.Int) { Value =  searchParameter.TypeConsult }, 
                new SqlParameter("PriorityID", SqlDbType.Int) { Value = (object)searchParameter.PriorityID ?? DBNull.Value },
                new SqlParameter("CategoryID", SqlDbType.Int) { Value = (object)searchParameter.CategoryID ?? DBNull.Value },
                new SqlParameter("StatusID", SqlDbType.Int) { Value = (object)searchParameter.StatusID ?? DBNull.Value },
                new SqlParameter("SupportTicket", SqlDbType.VarChar) { Value = searchParameter.SupportTicket},
                new SqlParameter("Title", SqlDbType.VarChar) { Value = searchParameter.Title  },
                new SqlParameter("AssignedUserID", SqlDbType.Int) { Value = (object)searchParameter.AssignedUserID ?? DBNull.Value },
                new SqlParameter("ConsultSearchID", SqlDbType.Int) { Value = (object)searchParameter.ConsultSearchID ?? DBNull.Value },
                new SqlParameter("CreateByUserID", SqlDbType.Int) { Value = (object)searchParameter.CreateByUserID ?? DBNull.Value },
                new SqlParameter("TypeUserID", SqlDbType.Int) { Value = (object)searchParameter.TypeUserID ?? DBNull.Value },
                new SqlParameter("TypeConsultID", SqlDbType.Int) { Value = (object)searchParameter.TypeUserID ?? DBNull.Value },
                new SqlParameter("IsConfirmID", SqlDbType.Int) { Value = (object)searchParameter.IsConfirmID ?? DBNull.Value },
                new SqlParameter("CampaignID", SqlDbType.Int) { Value = (object)searchParameter.CampaignID ?? DBNull.Value },
                new SqlParameter("StartDate", SqlDbType.Date) { Value = (object)searchParameter.StartDate ?? DBNull.Value },
                new SqlParameter("EndDate", SqlDbType.Date) { Value = (object)searchParameter.EndDate ?? DBNull.Value }
                ).ToList();

            IQueryable<SupportTicketSearchDetailsData> matchingItems = paginatedResult.AsQueryable<SupportTicketSearchDetailsData>();

            var resultTotalCount = matchingItems.Count();
            matchingItems = matchingItems.ApplyPagination(searchParameter);

            return matchingItems.ToPaginatedList<SupportTicketSearchDetailsData>(searchParameter, resultTotalCount);
        } 

         

        public class AccountTypes
        {
            public int AccountTypeID { get; set; }
            public string Name { get; set; }
        }
        public static Dictionary<string, string> GetAccountTypes()
        {
            List<AccountTypes> AccountTypesResultDicResult = DataAccess.ExecWithStoreProcedureLists<AccountTypes>("Core", "uspGetAccountTypes").ToList();
            Dictionary<string, string> AccountTypesResultDic = new Dictionary<string, string>();
            foreach (var item in AccountTypesResultDicResult)
            {
                AccountTypesResultDic.Add(Convert.ToString(item.AccountTypeID), item.Name);
            }
            return AccountTypesResultDic;
        } 

         

        #endregion
        #endregion

        #region  Gestion de Ticket
        public System.Collections.Generic.List<System.Tuple<int, string, int>> GetLevelSupportLevel(int ParentSupportLevelID, bool IsVisibleToWorkStation)
        {
            System.Collections.Generic.List<System.Tuple<int, string, int>> lstSupportLevel = new List<Tuple<int, string, int>>();
            System.Tuple<int, string, int> tplSupportLevel = null;
            try
            {
                using (SqlDataReader reader = DataAccess.GetDataReader("SpGetAllSupportLevel", new Dictionary<string, object>() {
                { "@ParentSupportLevelID", ParentSupportLevelID } ,
                 { "@IsVisibleToWorkStation", IsVisibleToWorkStation }
                
                }, "Core"))
                {

                    if (reader.HasRows)
                    {

                        while (reader.Read())
                        {

                            tplSupportLevel = System.Tuple.Create<int, string, int>

                                (
                                    Convert.ToInt32(reader["SupportLevelID"]),
                                    reader["Name"].ToString(),
                                    Convert.ToInt32(reader["ParentSupportLevelID"])
                                );
                            lstSupportLevel.Add(tplSupportLevel);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
            return lstSupportLevel;
        
        }

        public System.Collections.Generic.List<System.Tuple<int, string, int, int>> GetLevelSupportLevelIsActive(int ParentSupportLevelID, bool IsVisibleToWorkStation)
        {
            System.Collections.Generic.List<System.Tuple<int, string, int, int >> lstSupportLevel = new List<Tuple<int, string, int, int >>();
            System.Tuple<int, string, int, int> tplSupportLevel = null;
            try
            {
                using (SqlDataReader reader = DataAccess.GetDataReader("SpGetAllSupportLevel", new Dictionary<string, object>() {
                { "@ParentSupportLevelID", ParentSupportLevelID } ,
                 { "@IsVisibleToWorkStation", IsVisibleToWorkStation }
                
                }, "Core"))
                {

                    if (reader.HasRows)
                    {

                        while (reader.Read())
                        {

                            tplSupportLevel = System.Tuple.Create<int, string, int, int>

                                (
                                    Convert.ToInt32(reader["SupportLevelID"]),
                                    reader["Name"].ToString(),
                                    Convert.ToInt32(reader["ParentSupportLevelID"]),
                                     Convert.ToInt32(reader["Active"])
                                );
                            lstSupportLevel.Add(tplSupportLevel);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
            return lstSupportLevel;

        }


      

        public System.Collections.Generic.List<System.Tuple<int, string, int, int>> GetLevelSupportLevelMotiveIsActive(int SupportLevelID, bool IsVisibleToWorkStation)
        {
            System.Collections.Generic.List<System.Tuple<int, string, int, int>> lstSupportLevelMotive = new List<Tuple<int, string, int, int>>();
            System.Tuple<int, string, int, int> tplSupportLevelMotive = null;
            try
            {
                using (SqlDataReader reader = DataAccess.GetDataReader("USP_SELSupportLevelMotive", new Dictionary<string, object>() {
                { "@SupportLevelID", SupportLevelID } ,
                { "@IsVisibleToWorkStation", IsVisibleToWorkStation } 
                
                }, "Core"))
                {

                    if (reader.HasRows)
                    {

                        while (reader.Read())
                        {
                            tplSupportLevelMotive = System.Tuple.Create<int, string, int, int>

                                (
                                    Convert.ToInt32(reader["SupportLevelID"]),
                                    reader["Name"].ToString(),
                                    Convert.ToInt32(reader["SupportMotiveID"]),
                                    Convert.ToInt32(reader["Active"])
                                );
                            lstSupportLevelMotive.Add(tplSupportLevelMotive);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
            return lstSupportLevelMotive;
        }




        public System.Collections.Generic.List<System.Tuple<int, string, int>> GetLevelSupportLevelMotive(int SupportLevelID, bool IsVisibleToWorkStation)
        {
            System.Collections.Generic.List<System.Tuple<int, string, int>> lstSupportLevelMotive = new List<Tuple<int, string, int>>();
            System.Tuple<int, string, int> tplSupportLevelMotive = null;
            try
            {
                using (SqlDataReader reader = DataAccess.GetDataReader("USP_SELSupportLevelMotive", new Dictionary<string, object>() {
                { "@SupportLevelID", SupportLevelID } ,
                { "@IsVisibleToWorkStation", IsVisibleToWorkStation } 
                
                }, "Core"))
                {

                    if (reader.HasRows)
                    {

                        while (reader.Read())
                        {
                            tplSupportLevelMotive = System.Tuple.Create<int, string, int>

                                (
                                    Convert.ToInt32(reader["SupportLevelID"]),
                                    reader["Name"].ToString(),
                                    Convert.ToInt32(reader["SupportMotiveID"])
                                );
                            lstSupportLevelMotive.Add(tplSupportLevelMotive);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
            return lstSupportLevelMotive;
        }

        public List<SupportMotivePropertyTypes> ListarSupportMotivePropertyTypesPorMotivo(int SupportMotiveID, int SupportTicketID, Boolean IsVisibleToWorkStation)
        {
            System.Collections.Generic.List<SupportMotivePropertyTypes> lstSupportMotivePropertyTypes = new List<SupportMotivePropertyTypes>();
            SupportMotivePropertyTypes objSupportMotivePropertyTypes = null;
            try
            {
                using (SqlDataReader reader = DataAccess.GetDataReader("uspListarSupportMotivePropertyTypes", new Dictionary<string, object>() 
                { 
                    { "@SupportMotiveID", SupportMotiveID }, 
                    { "@SupportTicketID", SupportTicketID },
                    { "@IsVisibleToWorkStation", IsVisibleToWorkStation},
                    
                }
                , "Core")) {

                    if (reader.HasRows)
                    {

                        while (reader.Read())
                        {
                            objSupportMotivePropertyTypes = new SupportMotivePropertyTypes();

                            if (!Convert.IsDBNull(reader["DataType"])) objSupportMotivePropertyTypes.DataType = reader["DataType"].ToString();

                            if (!Convert.IsDBNull(reader["Editable"])) objSupportMotivePropertyTypes.Editable = Convert.ToBoolean(reader["Editable"]);

                            if (!Convert.IsDBNull(reader["IsVisibleToWorkStation"])) objSupportMotivePropertyTypes.IsVisibleToWorkStation = Convert.ToBoolean(reader["IsVisibleToWorkStation"]);

                            if (!Convert.IsDBNull(reader["Name"])) objSupportMotivePropertyTypes.Name = reader["Name"].ToString();

                            if (!Convert.IsDBNull(reader["Required"])) objSupportMotivePropertyTypes.Required = Convert.ToBoolean(reader["Required"]);

                            if (!Convert.IsDBNull(reader["SortIndex"])) objSupportMotivePropertyTypes.SortIndex = Convert.ToInt32(reader["SortIndex"]);

                            if (!Convert.IsDBNull(reader["SupportMotiveID"])) objSupportMotivePropertyTypes.SupportMotiveID = Convert.ToInt16(reader["SupportMotiveID"]);

                            if (!Convert.IsDBNull(reader["SupportMotivePropertyTypeID"])) objSupportMotivePropertyTypes.SupportMotivePropertyTypeID = Convert.ToInt32(reader["SupportMotivePropertyTypeID"]);

                            if (!Convert.IsDBNull(reader["TermName"])) objSupportMotivePropertyTypes.TermName = reader["TermName"].ToString();

                            if (!Convert.IsDBNull(reader["PropertyValue"])) objSupportMotivePropertyTypes.PropertyValue = reader["PropertyValue"].ToString();

                            if (!Convert.IsDBNull(reader["SupportTicketsPropertyValueID"])) objSupportMotivePropertyTypes.SupportTicketsPropertyValueID = Convert.ToInt32(reader["SupportTicketsPropertyValueID"]);

                            if (!Convert.IsDBNull(reader["SupportTicketsPropertyID"])) objSupportMotivePropertyTypes.SupportTicketsPropertyID = Convert.ToInt32(reader["SupportTicketsPropertyID"]);

                            if (!Convert.IsDBNull(reader["FieldSolution"])) objSupportMotivePropertyTypes.FieldSolution = Convert.ToBoolean(reader["FieldSolution"]);

                            if (!Convert.IsDBNull(reader["DinamicName"])) objSupportMotivePropertyTypes.DinamicName = reader["DinamicName"].ToString();

                            lstSupportMotivePropertyTypes.Add(objSupportMotivePropertyTypes);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
            return lstSupportMotivePropertyTypes;
        }

        public List<SupportMotivePropertyValues> ListarSupportMotivePropertyValuesPorMotivo(int SupportMotiveID)
        {
            System.Collections.Generic.List<SupportMotivePropertyValues> lstSupportMotivePropertyValues = new System.Collections.Generic.List<SupportMotivePropertyValues>();
            SupportMotivePropertyValues objSupportMotivePropertyValues = null;
            try
            {
                using (SqlDataReader reader = DataAccess.GetDataReader("UspListarSupportMotivePropertyValues", new Dictionary<string, object>() { { "@SupportMotiveID", SupportMotiveID } }, "Core"))
                {

                    if (reader.HasRows)
                    {
                        lstSupportMotivePropertyValues = new List<SupportMotivePropertyValues>();
                        while (reader.Read())
                        {
                            objSupportMotivePropertyValues = new SupportMotivePropertyValues();


                            if (!Convert.IsDBNull(reader["SortIndex"])) objSupportMotivePropertyValues.SortIndex = Convert.ToInt32(reader["SortIndex"]);

                            if (!Convert.IsDBNull(reader["SupportMotivePropertyTypeID"])) objSupportMotivePropertyValues.SupportMotivePropertyTypeID = Convert.ToInt32(reader["SupportMotivePropertyTypeID"]);

                            if (!Convert.IsDBNull(reader["SupportMotivePropertyValueID"])) objSupportMotivePropertyValues.SupportMotivePropertyValueID = Convert.ToInt32(reader["SupportMotivePropertyValueID"]);

                            if (!Convert.IsDBNull(reader["TermName"])) objSupportMotivePropertyValues.TermName = Convert.ToString(reader["TermName"]);

                            if (!Convert.IsDBNull(reader["Value"])) objSupportMotivePropertyValues.Value = Convert.ToString(reader["Value"]);

                            lstSupportMotivePropertyValues.Add(objSupportMotivePropertyValues);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
            return lstSupportMotivePropertyValues;
        }
      
        public List<SupportMotiveTask> ListarSupportMotiveTaskPorMotivo(int SupportMotiveID)
        {
            System.Collections.Generic.List<SupportMotiveTask> lstSupportMotiveTask = new List<SupportMotiveTask>();
            SupportMotiveTask objSupportMotiveTask = null;
            try
            {
                using (SqlDataReader reader = DataAccess.GetDataReader("UspListarSupportMotiveTask", new Dictionary<string, object>() { { "@SupportMotiveID", SupportMotiveID } }, "Core"))
                {

                    if (reader.HasRows)
                    {

                        while (reader.Read())
                        {
                            objSupportMotiveTask = new SupportMotiveTask();


                            if (!Convert.IsDBNull(reader["Link"])) objSupportMotiveTask.Link = reader["Link"].ToString();

                            if (!Convert.IsDBNull(reader["Name"])) objSupportMotiveTask.Name = reader["Name"].ToString();

                            if (!Convert.IsDBNull(reader["SortIndex"])) objSupportMotiveTask.SortIndex = Convert.ToInt32(reader["SortIndex"]);

                            if (!Convert.IsDBNull(reader["SupportMotiveID"])) objSupportMotiveTask.SupportMotiveID = Convert.ToInt16(reader["SupportMotiveID"]);

                            if (!Convert.IsDBNull(reader["SupportMotivePropertyTypeID"])) objSupportMotiveTask.SupportMotivePropertyTypeID = Convert.ToInt32(reader["SupportMotivePropertyTypeID"]);

                            if (!Convert.IsDBNull(reader["SupportMotiveTaskID"])) objSupportMotiveTask.SupportMotiveTaskID = Convert.ToInt32(reader["SupportMotiveTaskID"]);

                            if (!Convert.IsDBNull(reader["TermName"])) objSupportMotiveTask.TermName = reader["TermName"].ToString();

                            lstSupportMotiveTask.Add(objSupportMotiveTask);

                        }
                    }
                }
                 
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
            return lstSupportMotiveTask;
        }


        public SupportTicketsBE ObtenerSupportTicketsBE(int SupportTicketID)
        {
            SupportTicketsBE objSupportTicketsBE =   new  SupportTicketsBE();
            try
            {
                using (SqlDataReader reader = DataAccess.GetDataReader("UspGetSupportTickets", new Dictionary<string, object>() { { "@SupportTicketID", SupportTicketID } }, "Core"))
                {
                    if (reader.HasRows)
                    {
                       if( reader.Read())
                        {
                          if(! Convert.IsDBNull(reader["AccountID"])) objSupportTicketsBE.AccountID=   Convert.ToInt32(reader["AccountID"]);

                          if (!Convert.IsDBNull(reader["AssignedUserID"])) objSupportTicketsBE.AssignedUserID = Convert.ToInt32(reader["AssignedUserID"]);

                          if (!Convert.IsDBNull(reader["BlockUserID"])) objSupportTicketsBE.BlockUserID = Convert.ToInt32(reader["BlockUserID"]);

                          if (!Convert.IsDBNull(reader["CreatedByUserID"])) objSupportTicketsBE.CreatedByUserID = Convert.ToInt32(reader["CreatedByUserID"]);

                          if (!Convert.IsDBNull(reader["DateCloseUTC"])) objSupportTicketsBE.DateCloseUTC = Convert.ToDateTime(reader["DateCloseUTC"]);

                          if (!Convert.IsDBNull(reader["DateCreatedUTC"])) objSupportTicketsBE.DateCreatedUTC = Convert.ToDateTime(reader["DateCreatedUTC"]);

                          if (!Convert.IsDBNull(reader["DateLastModifiedUTC"])) objSupportTicketsBE.DateLastModifiedUTC = Convert.ToDateTime(reader["DateLastModifiedUTC"]);

                          if (!Convert.IsDBNull(reader["IsConfirm"])) objSupportTicketsBE.IsConfirm = Convert.ToBoolean(reader["IsConfirm"]);

                          if (!Convert.IsDBNull(reader["IsVisibleToOwner"])) objSupportTicketsBE.IsVisibleToOwner = Convert.ToBoolean(reader["IsVisibleToOwner"]);

                          if (!Convert.IsDBNull(reader["ModifiedByUserID"])) objSupportTicketsBE.ModifiedByUserID = Convert.ToInt32(reader["ModifiedByUserID"]);


                          if (!Convert.IsDBNull(reader["ScalingUserID"])) objSupportTicketsBE.ScalingUserID = Convert.ToInt32(reader["ScalingUserID"]);

                          if (!Convert.IsDBNull(reader["SupportLevelID"])) objSupportTicketsBE.SupportLevelID = Convert.ToInt32(reader["SupportLevelID"]);

                          if (!Convert.IsDBNull(reader["SupportMotiveID"])) objSupportTicketsBE.SupportMotiveID = Convert.ToInt32(reader["SupportMotiveID"]);

                          if (!Convert.IsDBNull(reader["SupportTicketCategoryID"])) objSupportTicketsBE.SupportTicketCategoryID = Convert.ToInt16(reader["SupportTicketCategoryID"]);

                          if (!Convert.IsDBNull(reader["SupportTicketID"])) objSupportTicketsBE.SupportTicketID = Convert.ToInt32(reader["SupportTicketID"]);

                          if (!Convert.IsDBNull(reader["SupportTicketNumber"])) objSupportTicketsBE.SupportTicketNumber = reader["SupportTicketNumber"].ToString();

                          if (!Convert.IsDBNull(reader["Description"])) objSupportTicketsBE.Description = reader["Description"].ToString();

                          if (!Convert.IsDBNull(reader["Title"])) objSupportTicketsBE.Title = reader["Title"].ToString();

                          if (!Convert.IsDBNull(reader["SupportTicketPriorityID"])) objSupportTicketsBE.SupportTicketPriorityID = Convert.ToInt16(reader["SupportTicketPriorityID"]);

                          if (!Convert.IsDBNull(reader["SupportTicketStatusID"])) objSupportTicketsBE.SupportTicketStatusID = Convert.ToInt16(reader["SupportTicketStatusID"]);

                          if (!Convert.IsDBNull(reader["NotEdit"])) objSupportTicketsBE.NotEdit = Convert.ToBoolean(reader["NotEdit"]);

                          if (!Convert.IsDBNull(reader["BlockUserName"])) objSupportTicketsBE.BlockUserName = reader["BlockUserName"].ToString();

                          if (!Convert.IsDBNull(reader["NameStatuses"])) objSupportTicketsBE.NameStatuses = reader["NameStatuses"].ToString();

                          if (!Convert.IsDBNull(reader["UserNameAsigned"])) objSupportTicketsBE.UserNameAsigned = reader["UserNameAsigned"].ToString();

                          if (!Convert.IsDBNull(reader["UserTypeID"])) objSupportTicketsBE.UserTypeID = Convert.ToInt32(reader["UserTypeID"]);

                          if (!Convert.IsDBNull(reader["IsSiteDWS"])) objSupportTicketsBE.IsSiteDWS = Convert.ToBoolean(reader["IsSiteDWS"]);
                           
                        }
                    }
                    return objSupportTicketsBE;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public byte BloquearSUpportTickets(int SupportTicketID, int BlockUserID)
        { 
          
            SqlParameter op = null;
            byte resultado = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
                {
                    connection.Open();
                    
                    using (SqlCommand ocom = new SqlCommand())
                    {
                        ocom.Connection = connection;
                      
                        ocom.CommandText = "SpBloqueaSupportTickets";
                        ocom.CommandType = CommandType.StoredProcedure;

                        op = new SqlParameter() { ParameterName = "@SupportTicketID", Value = SupportTicketID, SqlDbType = SqlDbType.Int };
                        ocom.Parameters.Add(op);

                        op = new SqlParameter() { ParameterName = "@BlockUserID", Value = BlockUserID, SqlDbType = SqlDbType.Int };
                        ocom.Parameters.Add(op);

                        resultado = (byte)ocom.ExecuteNonQuery();
                       return resultado;
                    }
                }
            }
            catch(  Exception ex )
            {
                throw ex;
            }
        }

        public List<SupportTicketsBE> ObtenerSupportTicketsBE(byte IsSiteDWS)
        {
            SupportTicketsBE objSupportTicketsBE = null;
            List<SupportTicketsBE> lstSupportTicket = new List<SupportTicketsBE>();
             
            try
            {
                using (SqlDataReader reader = DataAccess.GetDataReader("UspGetSupportTicketsGetAll",  new Dictionary<string,object>(){ {"@IsSiteDWS",IsSiteDWS}}, "Core"))
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            objSupportTicketsBE = new SupportTicketsBE();
                            if (!Convert.IsDBNull(reader["AccountID"])) objSupportTicketsBE.AccountID = Convert.ToInt32(reader["AccountID"]);

                            if (!Convert.IsDBNull(reader["AssignedUserID"])) objSupportTicketsBE.AssignedUserID = Convert.ToInt32(reader["AssignedUserID"]);

                            if (!Convert.IsDBNull(reader["BlockUserID"])) objSupportTicketsBE.BlockUserID = Convert.ToInt32(reader["BlockUserID"]);

                            if (!Convert.IsDBNull(reader["CreatedByUserID"])) objSupportTicketsBE.CreatedByUserID = Convert.ToInt32(reader["CreatedByUserID"]);

                            if (!Convert.IsDBNull(reader["DateCloseUTC"])) objSupportTicketsBE.DateCloseUTC = Convert.ToDateTime(reader["DateCloseUTC"]);

                            if (!Convert.IsDBNull(reader["DateCreatedUTC"])) objSupportTicketsBE.DateCreatedUTC = Convert.ToDateTime(reader["DateCreatedUTC"]);

                            if (!Convert.IsDBNull(reader["DateLastModifiedUTC"])) objSupportTicketsBE.DateLastModifiedUTC = Convert.ToDateTime(reader["DateLastModifiedUTC"]);

                            if (!Convert.IsDBNull(reader["IsConfirm"])) objSupportTicketsBE.IsConfirm = Convert.ToBoolean(reader["IsConfirm"]);

                            if (!Convert.IsDBNull(reader["IsVisibleToOwner"])) objSupportTicketsBE.IsVisibleToOwner = Convert.ToBoolean(reader["IsVisibleToOwner"]);

                            if (!Convert.IsDBNull(reader["ModifiedByUserID"])) objSupportTicketsBE.ModifiedByUserID = Convert.ToInt32(reader["ModifiedByUserID"]);
                             

                            if (!Convert.IsDBNull(reader["ScalingUserID"])) objSupportTicketsBE.ScalingUserID = Convert.ToInt32(reader["ScalingUserID"]);

                            if (!Convert.IsDBNull(reader["SupportLevelID"])) objSupportTicketsBE.SupportLevelID = Convert.ToInt32(reader["SupportLevelID"]);

                            if (!Convert.IsDBNull(reader["SupportMotiveID"])) objSupportTicketsBE.SupportMotiveID = Convert.ToInt32(reader["SupportMotiveID"]);

                            if (!Convert.IsDBNull(reader["SupportTicketCategoryID"])) objSupportTicketsBE.SupportTicketCategoryID = Convert.ToInt16(reader["SupportTicketCategoryID"]);

                            if (!Convert.IsDBNull(reader["SupportTicketID"])) objSupportTicketsBE.SupportTicketID = Convert.ToInt32(reader["SupportTicketID"]);

                            if (!Convert.IsDBNull(reader["SupportTicketNumber"])) objSupportTicketsBE.SupportTicketNumber = reader["SupportTicketNumber"].ToString();

                            if (!Convert.IsDBNull(reader["SupportTicketPriorityID"])) objSupportTicketsBE.SupportTicketPriorityID = Convert.ToInt16(reader["SupportTicketPriorityID"]);

                            if (!Convert.IsDBNull(reader["SupportTicketStatusID"])) objSupportTicketsBE.SupportTicketStatusID = Convert.ToInt16(reader["SupportTicketStatusID"]);

                             if (!Convert.IsDBNull(reader["FirstName"])) objSupportTicketsBE.FirstName =  reader["FirstName"].ToString();

                             if (!Convert.IsDBNull(reader["Title"])) objSupportTicketsBE.Title = reader["Title"].ToString();

                            /*CS.20AG2016.Inicio.Nuevas Columnas*/
                             if (!Convert.IsDBNull(reader["OrderNumber"])) objSupportTicketsBE.OrderNumber = reader["OrderNumber"].ToString();
                             if (!Convert.IsDBNull(reader["State"])) objSupportTicketsBE.State = reader["State"].ToString();
                             if (!Convert.IsDBNull(reader["City"])) objSupportTicketsBE.City = reader["City"].ToString();
                             if (!Convert.IsDBNull(reader["InvoiceNumber"])) objSupportTicketsBE.InvoiceNumber = reader["InvoiceNumber"].ToString();
                             /*CS.20AG2016.Fin.Nuevas Columnas*/

                            if (!Convert.IsDBNull(reader["LastName"])) objSupportTicketsBE.LastName =  reader["LastName"].ToString();

                            if (!Convert.IsDBNull(reader["Username"])) objSupportTicketsBE.Username =  reader["Username"].ToString();

                            if (!Convert.IsDBNull(reader["SupportLevelMotive"])) objSupportTicketsBE.SupportLevelMotive = reader["SupportLevelMotive"].ToString();

                            if (!Convert.IsDBNull(reader["IsSiteDWS"])) objSupportTicketsBE.IsSiteDWS = Convert.ToBoolean( reader["IsSiteDWS"]);

                            lstSupportTicket.Add(objSupportTicketsBE);
                        }
                    }
                    return lstSupportTicket;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        public List<SupportTicketGestionBE> ListarSupportTicketGestionBE(int  SupportTicketID)
        {
            SupportTicketGestionBE objSupportTicketGestionBE = null;
            List<SupportTicketGestionBE> lstSupportTicketGestionBE = new List<SupportTicketGestionBE>();

            try
            {
                using (SqlDataReader reader = DataAccess.GetDataReader("SpListarSupportTicketGestion",  new Dictionary<string,object>(){{"@SupportTicketID",SupportTicketID}}, "Core"))
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            objSupportTicketGestionBE = new SupportTicketGestionBE();

                            if (!Convert.IsDBNull(reader["DateCloseUTC"])) objSupportTicketGestionBE.DateCloseUTC = Convert.ToDateTime( reader["DateCloseUTC"]);

                            if (!Convert.IsDBNull(reader["DateCreatedUTC"])) objSupportTicketGestionBE.DateCreatedUTC = Convert.ToDateTime(reader["DateCreatedUTC"]);

                            if (!Convert.IsDBNull(reader["DateLastModifiedUTC"])) objSupportTicketGestionBE.DateLastModifiedUTC = Convert.ToDateTime(reader["DateLastModifiedUTC"]);

                            if (!Convert.IsDBNull(reader["Descripction"])) objSupportTicketGestionBE.Descripction = reader["Descripction"].ToString();

                            if (!Convert.IsDBNull(reader["isCLosed"])) objSupportTicketGestionBE.isCLosed = Convert.ToBoolean(reader["isCLosed"]);

                            if (!Convert.IsDBNull(reader["isInternal"])) objSupportTicketGestionBE.isInternal = Convert.ToBoolean(reader["isInternal"]);

                            if (!Convert.IsDBNull(reader["SupportTicketID"])) objSupportTicketGestionBE.SupportTicketID = Convert.ToInt32(reader["SupportTicketID"]);

                            if (!Convert.IsDBNull(reader["SupportTicketStatusID"])) objSupportTicketGestionBE.SupportTicketStatusID = Convert.ToInt16(reader["SupportTicketStatusID"]);

                            if (!Convert.IsDBNull(reader["UserID"])) objSupportTicketGestionBE.UserID = Convert.ToInt32(reader["UserID"]);

                            if (!Convert.IsDBNull(reader["Username"])) objSupportTicketGestionBE.Username = Convert.ToString(reader["Username"]);

                            if (!Convert.IsDBNull(reader["NameStatus"])) objSupportTicketGestionBE.NameStatus = Convert.ToString(reader["NameStatus"]);

                            if (!Convert.IsDBNull(reader["IsVisibleToOwner"])) objSupportTicketGestionBE.IsVisibleToOwner = Convert.ToBoolean(reader["IsVisibleToOwner"]);



                            lstSupportTicketGestionBE.Add(objSupportTicketGestionBE);
                        }
                    }
                    return lstSupportTicketGestionBE;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public int InsertarSuportTickets(
                    SupportTicketsBE objSupportTicketsBE,
                    List<SupportTicketsPropertyBE> LstSupportTicketsProperty, 
                    List<SupportTicketsFilesBE> LstSupportTicketsFiles         ,
                    List<int> ListaEliminarSupportTicketsFiles,
                    SupportTicketGestionBE objSupportTicketGestionBE
            )
        {
            SqlTransaction otr = null;
            SqlParameter op = null;
            int SupportTicketID = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
                {
                    connection.Open();
                    otr = connection.BeginTransaction();
                    using (SqlCommand ocom = new SqlCommand())
                    {
                        ocom.Connection = connection;
                        ocom.Transaction = otr;
                        ocom.CommandText = "InsUpdSupportTickets";
                        ocom.CommandType = CommandType.StoredProcedure;

                        op=  new SqlParameter(){ParameterName="@SupportTicketID",Value= objSupportTicketsBE.SupportTicketID,SqlDbType=SqlDbType.Int};
                        ocom.Parameters.Add(op);
                        if (objSupportTicketsBE.SupportTicketNumber==null)
                        {
                            op = new SqlParameter() { ParameterName = "@SupportTicketNumber", Value = DBNull.Value, SqlDbType = SqlDbType.VarChar };                  
                        }else{
                             op = new SqlParameter() { ParameterName = "@SupportTicketNumber", Value = objSupportTicketsBE.SupportTicketNumber, SqlDbType = SqlDbType.VarChar };
                        }

                        if (objSupportTicketsBE.IsSiteDWS == null)
                        {
                            op = new SqlParameter() { ParameterName = "@IsSiteDWS", Value = DBNull.Value, SqlDbType = SqlDbType.Bit };                  
                        }else{
                            op = new SqlParameter() { ParameterName = "@IsSiteDWS", Value = objSupportTicketsBE.IsSiteDWS.Value, SqlDbType = SqlDbType.Bit };
                        }
                        
                        ocom.Parameters.Add(op);

                        op = new SqlParameter() { ParameterName = "@AccountID", Value = objSupportTicketsBE.AccountID, SqlDbType = SqlDbType.Int };
                        ocom.Parameters.Add(op);

                        op = new SqlParameter() { ParameterName = "@AssignedUserID", Value = objSupportTicketsBE.AssignedUserID, SqlDbType = SqlDbType.Int };
                        ocom.Parameters.Add(op);

                        op = new SqlParameter() { ParameterName = "@Title", Value = objSupportTicketsBE.Title, SqlDbType = SqlDbType.VarChar };
                        ocom.Parameters.Add(op);

                        op = new SqlParameter() { ParameterName = "@Description", Value = objSupportTicketsBE.Description, SqlDbType = SqlDbType.VarChar };
                        ocom.Parameters.Add(op);

                        op = new SqlParameter() { ParameterName = "@SupportTicketCategoryID", Value = objSupportTicketsBE.SupportTicketCategoryID, SqlDbType = SqlDbType.SmallInt };
                        ocom.Parameters.Add(op);

                        op = new SqlParameter() { ParameterName = "@SupportTicketPriorityID", Value = objSupportTicketsBE.SupportTicketPriorityID, SqlDbType = SqlDbType.SmallInt };
                        ocom.Parameters.Add(op);

                        op = new SqlParameter() { ParameterName = "@SupportTicketStatusID", Value = objSupportTicketsBE.SupportTicketStatusID, SqlDbType = SqlDbType.SmallInt };
                        ocom.Parameters.Add(op);

                        op = new SqlParameter() { ParameterName = "@IsVisibleToOwner", Value = objSupportTicketsBE.IsVisibleToOwner, SqlDbType = SqlDbType.Bit };
                        ocom.Parameters.Add(op);

                        op = new SqlParameter() { ParameterName = "@CreatedByUserID", Value = objSupportTicketsBE.CreatedByUserID, SqlDbType = SqlDbType.Int };
                        ocom.Parameters.Add(op);

                        op = new SqlParameter() { ParameterName = "@ModifiedByUserID", Value = objSupportTicketsBE.ModifiedByUserID, SqlDbType = SqlDbType.Int };
                        ocom.Parameters.Add(op);

                        if (!objSupportTicketsBE.DateCreatedUTC.HasValue )
                        {
                            op = new SqlParameter() { ParameterName = "@DateCreatedUTC", Value = DBNull.Value, SqlDbType = SqlDbType.DateTime };
                        }else
                        {
                        op = new SqlParameter() { ParameterName = "@DateCreatedUTC", Value = objSupportTicketsBE.DateCreatedUTC, SqlDbType = SqlDbType.DateTime };
                        }
                        
                        ocom.Parameters.Add(op);

                        if (!objSupportTicketsBE.DateLastModifiedUTC.HasValue) 
                        {
                            op = new SqlParameter() { ParameterName = "@DateLastModifiedUTC", Value = DBNull.Value, SqlDbType = SqlDbType.DateTime };
                        }else{
                            op = new SqlParameter() { ParameterName = "@DateLastModifiedUTC", Value = objSupportTicketsBE.DateLastModifiedUTC.Value, SqlDbType = SqlDbType.DateTime };
                        }
                        ocom.Parameters.Add(op);

                        op = new SqlParameter() { ParameterName = "@SupportLevelID", Value = objSupportTicketsBE.SupportLevelID, SqlDbType = SqlDbType.Int };
                        ocom.Parameters.Add(op);

                        if (!objSupportTicketsBE.DateCloseUTC.HasValue)
                        {
                            op = new SqlParameter() { ParameterName = "@DateCloseUTC", Value = DBNull.Value, SqlDbType = SqlDbType.DateTime };
                        }
                        else
                        {
                            op = new SqlParameter() { ParameterName = "@DateCloseUTC", Value = objSupportTicketsBE.DateCloseUTC.Value, SqlDbType = SqlDbType.DateTime };
                        }
                        ocom.Parameters.Add(op);

                        op = new SqlParameter() { ParameterName = "@SupportMotiveID", Value = objSupportTicketsBE.SupportMotiveID, SqlDbType = SqlDbType.Int };
                        ocom.Parameters.Add(op);

                        op = new SqlParameter() { ParameterName = "@ScalingUserID", Value = objSupportTicketsBE.ScalingUserID, SqlDbType = SqlDbType.Int };
                        ocom.Parameters.Add(op);

                        op = new SqlParameter() { ParameterName = "@IsConfirm", Value = objSupportTicketsBE.IsConfirm, SqlDbType = SqlDbType.Bit };
                        ocom.Parameters.Add(op);

                        op = new SqlParameter() { ParameterName = "@BlockUserID", Value = objSupportTicketsBE.BlockUserID, SqlDbType = SqlDbType.Int };
                        ocom.Parameters.Add(op);
                       
                        SupportTicketID =Convert.ToInt32( ocom.ExecuteScalar());


                        if (objSupportTicketGestionBE != null)
                        {
                            ocom.Parameters.Clear();
                            ocom.CommandText = "SpInsSupportTicketGestion";

                            objSupportTicketGestionBE.SupportTicketID = SupportTicketID;

                            //registro de SupportTicketGestion
                            op = new SqlParameter() { ParameterName = "@SupportTicketID", Value = objSupportTicketGestionBE.SupportTicketID, SqlDbType = SqlDbType.Int };
                            ocom.Parameters.Add(op);

                            op = new SqlParameter() { ParameterName = "@UserID", Value = objSupportTicketGestionBE.UserID, SqlDbType = SqlDbType.Int };
                            ocom.Parameters.Add(op);

                            op = new SqlParameter() { ParameterName = "@Descripction", Value = objSupportTicketGestionBE.Descripction, SqlDbType = SqlDbType.VarChar };
                            ocom.Parameters.Add(op);

                            op = new SqlParameter() { ParameterName = "@DateCreatedUTC", Value = DBNull.Value, SqlDbType = SqlDbType.DateTime };
                            ocom.Parameters.Add(op);

                            op = new SqlParameter() { ParameterName = "@DateLastModifiedUTC", Value = DBNull.Value, SqlDbType = SqlDbType.DateTime };
                            ocom.Parameters.Add(op);

                            op = new SqlParameter() { ParameterName = "@SupportTicketStatusID", Value = objSupportTicketGestionBE.SupportTicketStatusID, SqlDbType = SqlDbType.SmallInt };
                            ocom.Parameters.Add(op);

                            op = new SqlParameter() { ParameterName = "@isInternal", Value = objSupportTicketGestionBE.isInternal, SqlDbType = SqlDbType.Bit };
                            ocom.Parameters.Add(op);

                            op = new SqlParameter() { ParameterName = "@isCLosed", Value = objSupportTicketGestionBE.isCLosed, SqlDbType = SqlDbType.Bit };
                            ocom.Parameters.Add(op);

                            ocom.ExecuteNonQuery();
                        }

                        ocom.Parameters.Clear();
                        ocom.CommandText = "UspInsSupportTicketsProperty";
                        for (int index = 0; index < LstSupportTicketsProperty.Count;index++ )
                        {
                            ocom.Parameters.Clear();
                            LstSupportTicketsProperty[index].SupportTicketID = SupportTicketID;
                            op = new SqlParameter() { ParameterName = "@SupportMotivePropertyTypeID", Value = LstSupportTicketsProperty[index].SupportMotivePropertyTypeID, SqlDbType = SqlDbType.Int };
                            ocom.Parameters.Add(op);

                            op = new SqlParameter() { ParameterName = "@SupportTicketID", Value = LstSupportTicketsProperty[index].SupportTicketID, SqlDbType = SqlDbType.Int };
                            ocom.Parameters.Add(op);

                            op = new SqlParameter() { ParameterName = "@SupportTicketsPropertyID", Value = LstSupportTicketsProperty[index].SupportTicketsPropertyID, SqlDbType = SqlDbType.Int };
                            ocom.Parameters.Add(op);

                            op = new SqlParameter() { ParameterName = "@SupportTicketsPropertyValueID", Value = LstSupportTicketsProperty[index].SupportTicketsPropertyValueID, SqlDbType = SqlDbType.Int };
                            ocom.Parameters.Add(op);
                            if(LstSupportTicketsProperty[index].PropertyValue==null)
                            {
                                op = new SqlParameter() { ParameterName = "@PropertyValue", Value =DBNull.Value, SqlDbType = SqlDbType.VarChar };
                            }else{
                                op = new SqlParameter() { ParameterName = "@PropertyValue", Value = LstSupportTicketsProperty[index].PropertyValue, SqlDbType = SqlDbType.VarChar };
                            }
                           
                           
                            ocom.Parameters.Add(op);

                            int SupportTicketsPropertyID = Convert.ToInt32(ocom.ExecuteScalar());
                            ocom.Parameters.Clear();
                        }

                          ocom.Parameters.Clear();
                          ocom.CommandText = "UspEliminarArchivos";
                        for (int index = 0; index < ListaEliminarSupportTicketsFiles.Count; index++)
                        {
                            ocom.Parameters.Clear();
                            op = new SqlParameter() { ParameterName = "@SupportTicketFileID", Value = ListaEliminarSupportTicketsFiles[index], SqlDbType = SqlDbType.Int };
                            ocom.Parameters.Add(op);
                            ocom.ExecuteNonQuery();
                        }

                        ocom.CommandText = "UspINsUpdSupportTicketsFiles";
                        ocom.Connection = connection;

                        for (int index = 0; index < LstSupportTicketsFiles.Count; index++)
                        {

                            LstSupportTicketsFiles[index].SupportTicketID = SupportTicketID;
                            ocom.Parameters.Clear();
                            if (!LstSupportTicketsFiles[index].DateCreatedUTC.HasValue)
                            {
                                op = new SqlParameter() { ParameterName = "@DateCreatedUTC", Value = DBNull.Value, SqlDbType = SqlDbType.DateTime };
                            }
                            else
                            {
                                op = new SqlParameter() { ParameterName = "@DateCreatedUTC", Value = LstSupportTicketsFiles[index].DateCreatedUTC, SqlDbType = SqlDbType.DateTime };
                            }
                            ocom.Parameters.Add(op);

                            op = new SqlParameter() { ParameterName = "@SupportTicketFileID", Value = LstSupportTicketsFiles[index].SupportTicketFileID, SqlDbType = SqlDbType.Int };
                            ocom.Parameters.Add(op);

                            op = new SqlParameter() { ParameterName = "@SupportTicketID", Value = LstSupportTicketsFiles[index].SupportTicketID, SqlDbType = SqlDbType.Int };
                            ocom.Parameters.Add(op);



                            op = new SqlParameter() { ParameterName = "@FilePath", Value = LstSupportTicketsFiles[index].FilePath, SqlDbType = SqlDbType.VarChar };
                            ocom.Parameters.Add(op);

                            op = new SqlParameter() { ParameterName = "@UserID", Value = LstSupportTicketsFiles[index].UserID, SqlDbType = SqlDbType.Int };
                            ocom.Parameters.Add(op);

                            int SupportTicketFileID = Convert.ToInt32(ocom.ExecuteScalar());
                            File.WriteAllBytes(Path.Combine(objSupportTicketsBE.RutaDirectorio, SupportTicketFileID + "_" + Path.GetFileName(LstSupportTicketsFiles[index].FilePath)), LstSupportTicketsFiles[index].Content);
                          

                            ocom.Parameters.Clear();
                        }


                     //   op = InsertarArchivos(LstSupportTicketsFiles, op, SupportTicketID, ocom);


                        otr.Commit();
                        return SupportTicketID;

                    }
                }
            }
            catch (Exception ex )
            {
                otr.Rollback();
                throw ex;
            }
        }

        public int InsertarArchivos(List<SupportTicketsFilesBE> LstSupportTicketsFiles, int SupportTicketID)
        {
            int SupportTicketFileID=0;
            SqlParameter op = null;
            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
            {
                connection.Open();

                using (SqlCommand ocom = new SqlCommand())
                {
                    ocom.CommandText = "UspINsUpdSupportTicketsFiles";
                    ocom.Connection = connection;

                    for (int index = 0; index < LstSupportTicketsFiles.Count; index++)
                    {
                       

                        LstSupportTicketsFiles[index].SupportTicketID = SupportTicketID;
                        ocom.Parameters.Clear();
                        if (!LstSupportTicketsFiles[index].DateCreatedUTC.HasValue)
                        {
                            op = new SqlParameter() { ParameterName = "@DateCreatedUTC", Value = DBNull.Value, SqlDbType = SqlDbType.DateTime };
                        }
                        else
                        {
                            op = new SqlParameter() { ParameterName = "@DateCreatedUTC", Value = LstSupportTicketsFiles[index].DateCreatedUTC, SqlDbType = SqlDbType.DateTime };
                        }
                        ocom.Parameters.Add(op);

                        op = new SqlParameter() { ParameterName = "@SupportTicketFileID", Value = LstSupportTicketsFiles[index].SupportTicketFileID, SqlDbType = SqlDbType.Int };
                        ocom.Parameters.Add(op);

                        op = new SqlParameter() { ParameterName = "@SupportTicketID", Value = LstSupportTicketsFiles[index].SupportTicketID, SqlDbType = SqlDbType.Int };
                        ocom.Parameters.Add(op);



                        op = new SqlParameter() { ParameterName = "@FilePath", Value = LstSupportTicketsFiles[index].FilePath, SqlDbType = SqlDbType.VarChar };
                        ocom.Parameters.Add(op);

                        op = new SqlParameter() { ParameterName = "@UserID", Value = LstSupportTicketsFiles[index].UserID, SqlDbType = SqlDbType.Int };
                        ocom.Parameters.Add(op);

                          SupportTicketFileID = Convert.ToInt32(ocom.ExecuteScalar());
                        ocom.Parameters.Clear();
                    }
                    return SupportTicketFileID;
                }

            }
             
        }


        public List<SupportTicketsFilesBE> ObtenerSupportTicketsFilesporSupporMotive(int SupportTicketID)
        {
            System.Collections.Generic.List<SupportTicketsFilesBE> lstSupportTicketsFilesBE = null;
            SupportTicketsFilesBE objSupportTicketsFilesBE = null;
            try
            {
                using (SqlDataReader reader = DataAccess.GetDataReader("UspGetSupportTicketsFiles", new Dictionary<string, object>() { { "@SupportTicketID", SupportTicketID } }, "Core"))
                {

                    if (reader.HasRows)
                    {
                        lstSupportTicketsFilesBE = new List<SupportTicketsFilesBE>();
                        while (reader.Read())
                        {
                            objSupportTicketsFilesBE = new SupportTicketsFilesBE();


                            if (!Convert.IsDBNull(reader["DateCreatedUTC"])) objSupportTicketsFilesBE.DateCreatedUTC = Convert.ToDateTime(reader["DateCreatedUTC"]);

                            if (!Convert.IsDBNull(reader["SupportTicketFileID"])) objSupportTicketsFilesBE.SupportTicketFileID = Convert.ToInt32(reader["SupportTicketFileID"]);

                            if (!Convert.IsDBNull(reader["SupportTicketID"])) objSupportTicketsFilesBE.SupportTicketID = Convert.ToInt32(reader["SupportTicketID"]);

                            if (!Convert.IsDBNull(reader["UserID"])) objSupportTicketsFilesBE.UserID = Convert.ToInt32(reader["UserID"]);

                            if (!Convert.IsDBNull(reader["FilePath"])){
                                objSupportTicketsFilesBE.FilePath = Convert.ToString(reader["FilePath"]);
                                objSupportTicketsFilesBE.Extension=Path.GetExtension(reader["FilePath"].ToString());
                            }



                            lstSupportTicketsFilesBE.Add(objSupportTicketsFilesBE);

                        }
                        return lstSupportTicketsFilesBE;
                    }
                }
                return lstSupportTicketsFilesBE;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
          
        }

        public Dictionary<int, string> GetFileName(int SupportTicketFileID)
        {
            Dictionary<int, string> dcSupportTicketsFiles = null;
             
            try
            {
                using (SqlDataReader reader = DataAccess.GetDataReader("SpGetFileName", new Dictionary<string, object>() { { "@SupportTicketFileID", @SupportTicketFileID} }, "Core"))
                {

                    if (reader.HasRows)
                    {
                        dcSupportTicketsFiles = new  Dictionary<int, string> ();
                        if (reader.Read())
                        {
                            dcSupportTicketsFiles[SupportTicketFileID] = reader["FilePath"].ToString();
                        }
                        
                    }
                }
                return dcSupportTicketsFiles;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }

        }
        //
        #endregion




      
    }
}




 