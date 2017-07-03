using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Common.Interfaces;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Security;

namespace nsCore.Controllers
{
    public class AdminController : BaseController
    {
        //private List<NetSteps.Data.Entities.CorporateUser> SiteUsers
        //{
        //    get
        //    {
        //        if (Session["User"] == null)
        //        {
        //            Session["User"] = NetSteps.Data.Entities.CorporateUser.LoadAllFull();
        //        }
        //        return Session["User"] as List<NetSteps.Data.Entities.CorporateUser>;
        //    }
        //    set { Session["User"] = null; }
        //}

        [FunctionFilter("Admin", "~/Accounts")]
        public ActionResult Index()
        {
            return RedirectToAction("Users");
        }

        #region Users
        [FunctionFilter("Admin", "~/Accounts")]
        public ActionResult Users()
        {
            return View();//SiteUsers);
        }

        public ActionResult GetUsers(int page, int pageSize, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection, int? status, int? role, string username)
        {
            //IOrderedEnumerable<NetSteps.Data.Entities.CorporateUser> users;
            //if (orderByDirection == NetSteps.Common.Constants.SortDirection.Ascending)
            //    users = orderBy == "FullName" ? SiteUsers.OrderBy(u => u.FirstName + " " + u.LastName) : SiteUsers.OrderBy(orderBy);
            //else
            //    users = orderBy == "FullName" ? SiteUsers.OrderByDescending(u => u.FirstName + " " + u.LastName) : SiteUsers.OrderByDescending(orderBy);

            PaginatedList<CorporateUserSearchData> users = CorporateUser.SearchUsers(new CorporateUserSearchParameters()
            {
                PageIndex = page,
                PageSize = pageSize,
                OrderBy = orderBy,
                OrderByDirection = orderByDirection,
                Status = status,
                Role = role,
                Username = username
            });

            StringBuilder builder = new StringBuilder();

            int count = 0;
            foreach (CorporateUserSearchData corporateUser in users.Skip(page * pageSize).Take(pageSize))
            {
                builder.Append("<tr class=\"").Append(count % 2 == 0 ? "GridRow" : "GridRowAlt").Append("\"><td><input type=\"checkbox\" class=\"userSelector\" /></td><td><a href=\"").Append("~/Admin/EditUser/".ResolveUrl())
                    .Append(corporateUser.CorporateUserID).Append("\">").Append(corporateUser.FullName).Append("</a></td><td>").Append(corporateUser.Username).Append("</td><td>")
                    .Append(corporateUser.Email).Append("</td><td>").Append(corporateUser.Role).Append("</td><td>").Append(corporateUser.Status).Append("</td><td>")
                    .Append(!corporateUser.LastLogin.HasValue ? "N/A" : corporateUser.LastLogin.ToString()).Append("</td></tr>");
                ++count;
            }

            return Json(new { totalPages = users.TotalPages, page = builder.ToString() });
        }

        public ActionResult ChangeUserStatus(List<int> items, bool active)
        {
            try
            {
                short statusId = active ? (short)Constants.UserStatus.Active : (short)Constants.UserStatus.Inactive;

                foreach (CorporateUser user in CorporateUser.LoadBatch(items))
                {
                    if (user.User.UserStatusID != statusId)
                    {
                        user.User.UserStatusID = statusId;
                        user.User.Save();
                    }
                }
                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [FunctionFilter("Admin-Create and Edit User", "~/Admin/Users")]
        [HttpGet]
        public ActionResult EditUser(int? id)
        {
            CorporateUser user;
            if (id.HasValue && id.Value > 0)
                user = CorporateUser.LoadFull(id.Value);//SiteUsers.First(u => u.CorporateUserID == id);
            else if (TempData["User"] != null)
                user = TempData["User"] as CorporateUser;
            else
                user = new CorporateUser() { User = new User() };
            return View(user);

        }

        [FunctionFilter("Admin-Create and Edit User", "~/Admin/Users")]
        [HttpPost]
        public ActionResult EditUser(int? userId, string firstName, string lastName, string username, string password, string passwordQuestion, string passwordAnswer, string confirmPassword, bool? hasAccessToAllSitesCheckbox, bool userChangingPassword, string email, short statusId, int defaultLanguageID)
        {
            CorporateUser corporateUser = new CorporateUser() { User = new User() };
            try
            {
                if (userId.HasValue && userId.Value > 0)
                    corporateUser = CorporateUser.LoadFull(userId.Value);

                corporateUser.FirstName = firstName;
                corporateUser.LastName = lastName;
                corporateUser.User.Username = username;

                if (userChangingPassword)
                {
                    var result = corporateUser.NewPasswordIsValid(password, confirmPassword);
                    if (result.Success)
                        corporateUser.User.Password = password;
                    else
                    {
                        ViewData["Error"] = result.Message;
                        return View(corporateUser);
                    }
                }
                corporateUser.User.PasswordQuestion = passwordQuestion;
                corporateUser.User.PasswordAnswer = passwordAnswer;
                corporateUser.Email = email;

                List<int> userRoles = new List<int>();
                foreach (string key in Request.Params.AllKeys.Where(k => k.StartsWith("roleAccess")))
                {
                    if (bool.Parse(Request.Params[key]))
                    {
                        int roleID = int.Parse(Regex.Replace(key, @"\D", ""));
                        userRoles.Add(roleID);
                    }
                }
                corporateUser.User.Roles.SyncTo(userRoles ?? new List<int>(), r => r.RoleID, id => Role.Load(id));


                // Reset ConsecutiveFailedLogins if the account is being reactivated - JHE
                if (corporateUser.User.UserStatusID == Constants.UserStatus.LockedOut.ToInt() && statusId == Constants.UserStatus.Active.ToInt())
                    corporateUser.User.ConsecutiveFailedLogins = 0;
                corporateUser.User.UserStatusID = statusId;

                corporateUser.User.DefaultLanguageID = defaultLanguageID;

                if (hasAccessToAllSitesCheckbox.ToBool())
                {
                    corporateUser.HasAccessToAllSites = true;
                    foreach (var site in corporateUser.Sites.ToList())
                        corporateUser.Sites.Remove(site);
                }
                else
                {
                    corporateUser.HasAccessToAllSites = false;

                    // Update Site Accessibility - JHE
                    List<int> accessibleSitesIds = new List<int>();
                    foreach (string key in Request.Params.AllKeys.Where(k => k.StartsWith("siteAccess")))
                    {
                        if (bool.Parse(Request.Params[key]))
                        {
                            int siteId = int.Parse(Regex.Replace(key, @"\D", ""));
                            accessibleSitesIds.Add(siteId);
                        }
                    }
                    corporateUser.Sites.SyncTo(accessibleSitesIds ?? new List<int>(), s => s.SiteID, id => Site.Load(id));
                }
                corporateUser.Save();

                //SiteUsers.Replace(u => u.CorporateUserID == userId, user);

                TempData["SavedUser"] = true;
                return RedirectToAction("Users");
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                ViewData["Error"] = exception.PublicMessage;
                return View(corporateUser);
            }
        }

        [FunctionFilter("Admin", "~/Accounts")]
        public ActionResult UserAuditHistory(int? id)
        {
            ViewData["CorporateUserID"] = id.ToString();
            Session["CurrentCorporateUserID"] = id;
            return View(new PaginatedList<AuditLogRow>());
        }

        public ActionResult GetUserAuditHistory(int page, int pageSize, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection)
        {
            int id = Session["CurrentCorporateUserID"].ToString().ToInt();
            PaginatedList<AuditLogRow> auditLogRows = CorporateUser.GetAuditLog(id, new PaginatedListParameters()
            {
                PageIndex = page,
                PageSize = pageSize,
                OrderBy = orderBy,
                OrderByDirection = orderByDirection,
            });

            StringBuilder builder = new StringBuilder();

            int count = 0;
            foreach (AuditLogRow auditLogRow in auditLogRows)
            {
                StringBuilder rows = new StringBuilder();
                rows.Append(string.Format("<td>{0}</td>", auditLogRow.DateChanged));
                rows.Append(string.Format("<td>{0}</td>", auditLogRow.TableName));
                rows.Append(string.Format("<td>{0}</td>", auditLogRow.ColumnName));
                rows.Append(string.Format("<td>{0}</td>", auditLogRow.OldValue));
                rows.Append(string.Format("<td>{0}</td>", auditLogRow.NewValue));
                rows.Append(string.Format("<td>{0}</td>", auditLogRow.Username));
                rows.Append(string.Format("<td>{0}</td>", auditLogRow.ApplicationName));

                builder.Append(string.Format("<tr class=\"{0}\">{1}</tr>", count % 2 == 0 ? "GridRow" : "GridRowAlt", rows));
                ++count;
            }

            return Json(new { totalPages = auditLogRows.TotalPages, page = builder.ToString() });
        }

        #endregion

        #region Roles
        [FunctionFilter("Admin", "~/Accounts")]
        public ActionResult Roles()
        {
            SmallCollectionCache.Instance.Roles.ExpireCache();
            return View();
        }

        [FunctionFilter("Admin-Create and Edit Role", "~/Admin/Roles")]
        public ActionResult EditRole(int? id)
        {
            return View(id.HasValue ? Role.LoadFull(id.Value) : new Role() { Functions = new TrackableCollection<Function>() });
        }

        [FunctionFilter("Admin-Create and Edit Role", "~/Admin/Roles")]
        public ActionResult SaveRole(string name, List<int> functionIds, int roleId = 0)
        {
            try
            {
                Role role;
                if (roleId > 0)
                    role = Role.LoadFull(roleId);
                else
                    role = new Role();

                role.Name = name;
                role.Functions.SyncTo(functionIds ?? new List<int>(), f => f.FunctionID, id => SmallCollectionCache.Instance.Functions.GetById(id));
                role.Save();

                SmallCollectionCache.Instance.Roles.ExpireCache();
                // Reload Roles (CurrentUser) if CurrentUser is affected by the edit Role. - JHE

                throw new NotImplementedException("Finish porting this functionality if needed. - JHE");
                //if ((CoreContext.CurrentUser as CorporateUser).User.RoleID == roleId)
                //    CoreContext.CurrentUser = CorporateUser.LoadFull((CoreContext.CurrentUser as CorporateUser).CorporateUserID);
                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
        #endregion

        #region List Types
        [FunctionFilter("Admin", "~/Accounts")]
        public ActionResult ListTypes()
        {
            return View();
        }

        [FunctionFilter("Admin-Create and Edit List Value", "~/Admin/ListTypes")]
        public ActionResult ListValues(NetSteps.Data.Entities.Constants.EditableListTypes id)
        {
            ViewData["ListType"] = id.ToString();

            if (id == Constants.EditableListTypes.AccountStatusChangeReason)
                return View(SmallCollectionCache.Instance.AccountStatusChangeReasons.ToIListValueList());
            else if (id == Constants.EditableListTypes.ArchiveType)
                return View(SmallCollectionCache.Instance.ArchiveTypes.ToIListValueList());
            else if (id == Constants.EditableListTypes.NewsType)
                return View(SmallCollectionCache.Instance.NewsTypes.ToIListValueList());
            else if (id == Constants.EditableListTypes.ReturnReasons)
                return View(SmallCollectionCache.Instance.ReturnReasons.ToIListValueList());
            else if (id == Constants.EditableListTypes.ReturnTypes)
                return View(SmallCollectionCache.Instance.ReturnTypes.ToIListValueList());
            else if (id == Constants.EditableListTypes.SiteStatusChangeReason)
                return View(SmallCollectionCache.Instance.SiteStatusChangeReasons.ToIListValueList());

            else if (id == Constants.EditableListTypes.CalendarEventType)
            {
                var list = AccountListValue.LoadCorporateListValuesByType(Constants.ListValueType.CalendarEventType.ToInt());
                return View(list.ToIListValueList());
            }
            else if (id == Constants.EditableListTypes.CalendarCategory)
            {
                var list = AccountListValue.LoadCorporateListValuesByType(Constants.ListValueType.CalendarCategory.ToInt());
                return View(list.ToIListValueList());
            }
            else if (id == Constants.EditableListTypes.CalendarPriority)
            {
                var list = AccountListValue.LoadCorporateListValuesByType(Constants.ListValueType.CalendarPriority.ToInt());
                return View(list.ToIListValueList());
            }
            else if (id == Constants.EditableListTypes.CalendarStatus)
            {
                var list = AccountListValue.LoadCorporateListValuesByType(Constants.ListValueType.CalendarStatus.ToInt());
                return View(list.ToIListValueList());
            }
            else if (id == Constants.EditableListTypes.CalendarColorCoding)
            {
                var list = AccountListValue.LoadCorporateListValuesByType(Constants.ListValueType.CalendarColorCoding.ToInt());
                return View(list.ToIListValueList());
            }
            else if (id == Constants.EditableListTypes.TaskStatus)
            {
                var list = AccountListValue.LoadCorporateListValuesByType(Constants.ListValueType.TaskStatus.ToInt());
                return View(list.ToIListValueList());
            }
            else if (id == Constants.EditableListTypes.TaskType)
            {
                var list = AccountListValue.LoadCorporateListValuesByType(Constants.ListValueType.TaskType.ToInt());
                return View(list.ToIListValueList());
            }
            else if (id == Constants.EditableListTypes.TaskPriority)
            {
                var list = AccountListValue.LoadCorporateListValuesByType(Constants.ListValueType.TaskPriority.ToInt());
                return View(list.ToIListValueList());
            }
            else if (id == Constants.EditableListTypes.TaskCategory)
            {
                var list = AccountListValue.LoadCorporateListValuesByType(Constants.ListValueType.TaskCategory.ToInt());
                return View(list.ToIListValueList());
            }
            else if (id == Constants.EditableListTypes.ContactCategory)
            {
                var list = AccountListValue.LoadCorporateListValuesByType(Constants.ListValueType.ContactCategory.ToInt());
                return View(list.ToIListValueList());
            }
            else if (id == Constants.EditableListTypes.ContactStatus)
            {
                var list = AccountListValue.LoadCorporateListValuesByType(Constants.ListValueType.ContactStatus.ToInt());
                return View(list.ToIListValueList());
            }
            else if (id == Constants.EditableListTypes.ContactType)
            {
                var list = AccountListValue.LoadCorporateListValuesByType(Constants.ListValueType.ContactType.ToInt());
                return View(list.ToIListValueList());
            }
            else
                return View();
        }

        [FunctionFilter("Admin-Create and Edit List Value", "~/Admin/ListTypes")]
        public ActionResult DeleteListValue(NetSteps.Data.Entities.Constants.EditableListTypes type, int listValueId)
        {
            try
            {
                if (type == Constants.EditableListTypes.AccountStatusChangeReason)
                {
                    AccountStatusChangeReason.Delete(listValueId.ToShort());
                    return Json(new { result = true });
                }
                else if (type == Constants.EditableListTypes.ArchiveType)
                {
                    ArchiveType.Delete(listValueId.ToShort());
                    return Json(new { result = true });
                }
                else if (type == Constants.EditableListTypes.NewsType)
                {
                    NewsType.Delete(listValueId.ToShort());
                    return Json(new { result = true });
                }
                else if (type == Constants.EditableListTypes.ReturnReasons)
                {
                    ReturnReason.Delete(listValueId.ToShort());
                    return Json(new { result = true });
                }
                else if (type == Constants.EditableListTypes.ReturnTypes)
                {
                    ReturnType.Delete(listValueId.ToShort());
                    return Json(new { result = true });
                }
                else if (type == Constants.EditableListTypes.SiteStatusChangeReason)
                {
                    SiteStatusChangeReason.Delete(listValueId.ToShort());
                    return Json(new { result = true });
                }
                else if (type == Constants.EditableListTypes.CalendarEventType ||
                    type == Constants.EditableListTypes.CalendarCategory ||
                    type == Constants.EditableListTypes.CalendarPriority ||
                    type == Constants.EditableListTypes.CalendarStatus ||
                    type == Constants.EditableListTypes.CalendarColorCoding ||
                    type == Constants.EditableListTypes.TaskStatus ||
                    type == Constants.EditableListTypes.TaskType ||
                    type == Constants.EditableListTypes.TaskPriority ||
                    type == Constants.EditableListTypes.TaskCategory ||
                    type == Constants.EditableListTypes.TaskStatus ||
                    type == Constants.EditableListTypes.ContactCategory ||
                    type == Constants.EditableListTypes.ContactStatus ||
                    type == Constants.EditableListTypes.ContactType)
                {
                    AccountListValue.Delete(listValueId);
                    return Json(new { result = true });
                }
                else
                    return Json(new { result = false });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [FunctionFilter("Admin-Create and Edit List Value", "~/Admin/ListTypes")]
        public ActionResult SaveListValues(NetSteps.Data.Entities.Constants.EditableListTypes type, Dictionary<int, string> listValues)
        {
            try
            {
                foreach (var listValue in listValues)
                {
                    IListValue lv;

                    if (type == Constants.EditableListTypes.AccountStatusChangeReason)
                    {
                        lv = SmallCollectionCache.Instance.AccountStatusChangeReasons.GetById(listValue.Key.ToShort());

                        if (lv != null)
                            lv = new AccountStatusChangeReason();
                        lv.Title = listValue.Value;
                        (lv as AccountStatusChangeReason).Save();
                    }
                    else if (type == Constants.EditableListTypes.ArchiveType)
                    {
                        lv = SmallCollectionCache.Instance.ArchiveTypes.GetById(listValue.Key.ToShort());

                        if (lv == null)
                            lv = new ArchiveType();
                        lv.Title = listValue.Value;
                        (lv as ArchiveType).Save();
                    }
                    else if (type == Constants.EditableListTypes.NewsType)
                    {
                        lv = SmallCollectionCache.Instance.NewsTypes.GetById(listValue.Key.ToShort());

                        if (lv != null)
                            lv = new NewsType();
                        lv.Title = listValue.Value;
                        (lv as NewsType).Save();
                    }
                    else if (type == Constants.EditableListTypes.ReturnReasons)
                    {
                        lv = SmallCollectionCache.Instance.ReturnReasons.GetById(listValue.Key.ToShort());

                        if (lv != null)
                            lv = new ReturnReason();
                        lv.Title = listValue.Value;
                        (lv as ReturnReason).Save();
                    }
                    else if (type == Constants.EditableListTypes.ReturnTypes)
                    {
                        lv = SmallCollectionCache.Instance.ReturnTypes.GetById(listValue.Key.ToShort());

                        if (lv != null)
                            lv = new ReturnType();
                        lv.Title = listValue.Value;
                        (lv as ReturnType).Save();
                    }
                    else if (type == Constants.EditableListTypes.SiteStatusChangeReason)
                    {
                        lv = SmallCollectionCache.Instance.SiteStatusChangeReasons.GetById(listValue.Key.ToShort());

                        if (lv != null)
                            lv = new SiteStatusChangeReason();
                        lv.Title = listValue.Value;
                        (lv as SiteStatusChangeReason).Save();
                    }
                    else if (type == Constants.EditableListTypes.CalendarEventType ||
                        type == Constants.EditableListTypes.CalendarCategory ||
                        type == Constants.EditableListTypes.CalendarPriority ||
                        type == Constants.EditableListTypes.CalendarStatus ||
                        type == Constants.EditableListTypes.CalendarColorCoding ||
                        type == Constants.EditableListTypes.TaskStatus ||
                        type == Constants.EditableListTypes.TaskType ||
                        type == Constants.EditableListTypes.TaskPriority ||
                        type == Constants.EditableListTypes.TaskCategory ||
                        type == Constants.EditableListTypes.TaskStatus ||
                        type == Constants.EditableListTypes.ContactCategory ||
                        type == Constants.EditableListTypes.ContactStatus ||
                        type == Constants.EditableListTypes.ContactType)
                    {
                        if (listValue.Key > 0)
                            lv = AccountListValue.Load(listValue.Key);
                        else
                        {
                            Constants.ListValueType listType = type.ToString().ToEnum<Constants.ListValueType>(Constants.ListValueType.NotSet);
                            lv = new AccountListValue() { ListValueTypeID = listType.ToShort() };
                        }

                        lv.Title = listValue.Value;
                        (lv as AccountListValue).IsCorporate = true;
                        (lv as AccountListValue).Active = true;
                        (lv as AccountListValue).IsEditable = true;
                        (lv as AccountListValue).Save();
                    }
                }

                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
        #endregion

        #region Autoship Schedules
        //[FunctionFilter("Admin", "~/Accounts")]
        //public ActionResult AutoshipSchedules()
        //{
        //    return View(NetSteps.Objects.Business.AutoshipSchedule.GetAllSchedules());
        //}

        ////[FunctionFilter("Admin-Create and Edit Schedule", "~/Admin/AutoshipSchedules")]
        //public ActionResult EditSchedule(int? id)
        //{
        //    NetSteps.Objects.Business.AutoshipSchedule schedule;
        //    if (id.HasValue)
        //    {
        //        schedule = new NetSteps.Objects.Business.AutoshipSchedule(id.Value);
        //        schedule.Load();
        //    }
        //    else if (TempData["Schedule"] != null)
        //    {
        //        schedule = TempData["Schedule"] as NetSteps.Objects.Business.AutoshipSchedule;
        //    }
        //    else
        //    {
        //        schedule = new NetSteps.Objects.Business.AutoshipSchedule();
        //    }

        //    return View(schedule);
        //}

        ////[FunctionFilter("Admin-Create and Edit Schedule", "~/Admin/AutoshipSchedules")]
        //public ActionResult SaveSchedule(int? scheduleId, string scheduleName, int intervalTypeId, int runDay, bool active)
        //{
        //    NetSteps.Objects.Business.AutoshipSchedule schedule = null;
        //    if (scheduleId.HasValue && scheduleId.Value > 0)
        //    {
        //        schedule = new NetSteps.Objects.Business.AutoshipSchedule(scheduleId.Value);
        //        schedule.Load();
        //    }
        //    else
        //    {
        //        schedule = new NetSteps.Objects.Business.AutoshipSchedule();
        //    }

        //    schedule.ScheduleName = scheduleName;
        //    schedule.IntervalTypeId = intervalTypeId;
        //    schedule.MaxIntervalDay = runDay;
        //    schedule.Active = active;
        //    try
        //    {

        //        schedule.Save();

        //        TempData["SavedSchedule"] = true;
        //        return RedirectToAction("AutoshipSchedules");
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["Schedule"] = schedule;
        //        TempData["Error"] = ex.Message;
        //        return RedirectToAction("EditSchedule");
        //    }
        //}
        #endregion

        public ActionResult ConfigAdmin()
        {
            return View();
        }

        public ActionResult EncryptPaymentGatewayConfig()
        {
            ConfigurationEncryption.ProtectSectionGroup("PaymentGateways");
            return View("ConfigAdmin");
        }

        public ActionResult UnEncryptPaymentGatewayConfig()
        {
            ConfigurationEncryption.UnProtectSectionGroup("PaymentGateways");
            return View("ConfigAdmin");
        }
    }
}
