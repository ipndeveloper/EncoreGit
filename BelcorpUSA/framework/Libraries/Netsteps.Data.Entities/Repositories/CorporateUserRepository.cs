using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Web.Security;
using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class CorporateUserRepository
    {
        #region Members
        protected override Func<NetStepsEntities, IQueryable<CorporateUser>> loadAllFullQuery
        {
            get
            {
                return CompiledQuery.Compile<NetStepsEntities, IQueryable<CorporateUser>>(
                 (context) => from u in context.CorporateUsers
                                               .Include("User")
                                               .Include("User.Roles")
                                               .Include("User.Roles.Functions")
                                               .Include("Sites")
                              select u);
            }
        }
        #endregion

        #region Methods
        public PaginatedList<CorporateUserSearchData> Search(CorporateUserSearchParameters searchParameters)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    PaginatedList<CorporateUserSearchData> results = new PaginatedList<CorporateUserSearchData>(searchParameters);

                    IQueryable<CorporateUser> matchingUsers = context.CorporateUsers;

                    if (searchParameters.Status.HasValue)
                    {
                        matchingUsers = matchingUsers.Where(u => u.User.UserStatusID == searchParameters.Status.Value);
                    }
                    if (searchParameters.Role.HasValue)
                    {
                        matchingUsers = matchingUsers.Where(u => u.User.Roles.Any(r => r.RoleID == searchParameters.Role.Value));
                    }
                    if (!string.IsNullOrEmpty(searchParameters.Username))
                    {
                        matchingUsers = matchingUsers.Where(u => u.User.Username.ToLower().Contains(searchParameters.Username.ToLower()));
                    }

                    if (searchParameters.WhereClause != null)
                        matchingUsers = matchingUsers.Where(searchParameters.WhereClause);


                    if (!string.IsNullOrEmpty(searchParameters.OrderBy))
                    {
                        if (searchParameters.OrderBy == "Name")
                            matchingUsers = matchingUsers.ApplyOrderByFilter(searchParameters.OrderByDirection, u => u.FirstName + " " + u.LastName);
                        else
                            matchingUsers = matchingUsers.ApplyOrderByFilter(searchParameters, context);
                    }
                    else
                        matchingUsers = matchingUsers.ApplyOrderByFilter(searchParameters.OrderByDirection, u => u.UserID);

                    results.TotalCount = matchingUsers.Count();

                    // Apply Paging filter - JHE
                    if (searchParameters.PageSize.HasValue)
                        matchingUsers = matchingUsers.Skip(searchParameters.PageIndex * searchParameters.PageSize.Value).Take(searchParameters.PageSize.Value);

                    
                    var userInfos = matchingUsers.Select(u => new
                                       {
                                           u.CorporateUserID ,
                                           u.UserID ,
                                           FullName = u.FirstName + " " + u.LastName,
                                           u.User.Username,
                                           u.Email,
                                           Roles = u.User.Roles.Select(r => r.RoleID),
                                           UserStatusID = ((u.User == null) ? 0 : u.User.UserStatusID),
                                           LastLogin = u.User.LastLoginUTC
                                       });

                    foreach (var a in userInfos.ToList())
                    {
                        List<string> rolesTranslated = new List<string>();
                        foreach (var roleId in a.Roles.ToList())
                            rolesTranslated.Add(SmallCollectionCache.Instance.Roles.GetById(roleId).GetTerm());

                        results.Add(new CorporateUserSearchData()
                        {
                            CorporateUserID = a.CorporateUserID,
                            UserID = a.UserID,
                            FullName = a.FullName,
                            Username = a.Username,
                            Email = a.Email,
                            Role = rolesTranslated.Join(", "),
                            Status = SmallCollectionCache.Instance.UserStatuses.GetById((short) a.UserStatusID).GetTerm(),
                            LastLogin = a.LastLogin.UTCToLocal()
                        });
                    }

                    return results;
                }
            });
        }

        public CorporateUser LoadByUserIdFull(int userID)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var result = from u in context.CorporateUsers
                                               .Include("User")
                                               .Include("User.Roles")
                                               .Include("User.Roles.Functions")
                                               .Include("Sites")
                                 where u.UserID == userID
                                 select u;
                    return result.FirstOrDefault();
                }
            });
        }

        public void GrantSiteAccess(CorporateUser corporateUser, int siteID)
        {
            ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                // Check the DB for for the UserSite Record just in case of a concurrency issue. - JHE
                using (NetStepsEntities context = CreateContext())
                {
                    var user = (from u in context.CorporateUsers
                                                   .Include("Sites")
                                where u.CorporateUserID == corporateUser.CorporateUserID
                                select u).FirstOrDefault();

                    if (!user.HasAccessToSite(siteID))
                    {
                        Site site = Site.Load(siteID);
                        corporateUser.Sites.Add(site);
                        corporateUser.Save();
                    }
                }
            });
        }

        public void RevokeSiteAccess(CorporateUser corporateUser, int siteID)
        {
            ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                // Check the DB for for the UserSite Record just in case of a concurrency issue. - JHE
                using (NetStepsEntities context = CreateContext())
                {
                    var user = (from u in context.CorporateUsers
                                                   .Include("Sites")
                                where u.CorporateUserID == corporateUser.CorporateUserID
                                select u).FirstOrDefault();

                    if (corporateUser.HasAccessToSite(siteID))
                    {
                        Site site = Site.Load(siteID);
                        corporateUser.Sites.Remove(site);
                        corporateUser.Save();
                    }
                }
            });
        }

        public override PaginatedList<AuditLogRow> GetAuditLog(int primaryKey, AuditLogSearchParameters searchParameters)
        {
            ValidatePrimaryKeyForLoad(primaryKey);

            var user = CorporateUser.LoadFull(primaryKey);

            List<AuditTableValueItem> list = new List<AuditTableValueItem>();
            list.Add(new AuditTableValueItem()
            {
                TableName = EntitySetName,
                PrimaryKey = Convert.ToInt32(primaryKey)
            });

            if (user.User != null && user.User.UserID > 0)
                list.Add(new AuditTableValueItem()
                {
                    TableName = "Users",
                    PrimaryKey = user.User.UserID
                });

            return GetAuditLog(list, searchParameters);
        }

        protected override string GetMeaningfulAuditValue(string tableName, string columnName, string value)
        {
            try
            {
                if (columnName == "PasswordHash")
                    return string.Format("({0})", Translation.GetTerm("encrypted"));
                else
                    return base.GetMeaningfulAuditValue(tableName, columnName, value);
            }
            catch (Exception ex)
            {
                EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsDataException);
                return value;
            }
        }
        #endregion
    }
}