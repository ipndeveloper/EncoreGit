using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Extensions;
using NetSteps.Web.Mvc.Helpers;
using NetSteps.Diagnostics.Utilities;
using nsCore.Controllers;
using nsCore.Areas.Admin.Models;
using NetSteps.Data.Entities.Generated;

namespace nsCore.Areas.Admin.Controllers
{
	public class UsersController : BaseController
	{
		[FunctionFilter("Admin-Create and Edit User", "~/Admin/AutoshipSchedules")]
		public virtual ActionResult Index()
		{
			return View();
		}

		[OutputCache(CacheProfile = "PagedGridData")]
		public virtual ActionResult Get(int? status, int? role, string username, int page, int pageSize, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection)
		{
			try
			{
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
				foreach (CorporateUserSearchData corporateUser in users)
				{
					builder.Append("<tr>")
						.AppendCheckBoxCell(value: corporateUser.CorporateUserID.ToString())
						.AppendLinkCell("~/Admin/Users/Edit/" + corporateUser.CorporateUserID, corporateUser.FullName)
						.AppendCell(corporateUser.Username)
						.AppendCell(corporateUser.Email)
						.AppendCell(corporateUser.Role)
						.AppendCell(corporateUser.Status)
						.AppendCell(corporateUser.LastLogin.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo))
						.Append("</tr>");
					++count;
				}
				return Json(new { totalPages = users.TotalPages, page = builder.ToString() });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		public virtual ActionResult ChangeStatus(List<int> items, bool active)
		{
			try
			{
				short statusId = active ? (short)Constants.UserStatus.Active : (short)Constants.UserStatus.Inactive;

				foreach (CorporateUser user in CorporateUser.LoadBatchFull(items))
				{
					if (user.User.UserStatusID != statusId)
					{
						user.User.UserStatusID = statusId;
						user.Save();
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

		[FunctionFilter("Admin-Create and Edit User", "~/Admin/AutoshipSchedules")]
		public virtual ActionResult Edit(int? id)
		{
			try
			{
				var model = new UsersEditModel(id);
				return View(model);
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				throw exception;
			}
		}

		public class SaveCorporateUserResult
		{
			public bool Result { get; set; }
			public string Message { get; set; }

			public CorporateUser CorporateUser { get; set; }
		}

		protected virtual SaveCorporateUserResult SaveCorporateUser(UsersSaveModel model)
		{
			CorporateUser corporateUser = new CorporateUser() { User = new User() };
			corporateUser.StartEntityTracking();
			try
			{
				if (model.userId.HasValue && model.userId.Value > 0)
				{
					corporateUser = CorporateUser.LoadFull(model.userId.Value);
				}

				corporateUser.FirstName = model.firstName;
				corporateUser.LastName = model.lastName;
				corporateUser.User.UserTypeID = (int)Constants.UserType.Corporate;

				if (!NetSteps.Data.Entities.User.IsUsernameAvailable(corporateUser != null ? corporateUser.UserID : 0, model.username))
				{
					return new SaveCorporateUserResult { Result = false, Message = Translation.GetTerm("UsernameInUse") };
				}

				corporateUser.User.Username = model.username;

				if (model.userChangingPassword)
				{
					var result = corporateUser.NewPasswordIsValid(model.password, model.confirmPassword);
					if (result.Success)
					{
						corporateUser.User.Password = model.password;
					}
					else
					{
						return new SaveCorporateUserResult { Result = false, Message = result.Message };
					}
				}

				corporateUser.User.PasswordQuestion = model.passwordQuestion;
				corporateUser.User.PasswordAnswer = model.passwordAnswer;
				corporateUser.Email = model.email;

				if (model.roles != null)
				{
					corporateUser.User.Roles.SyncTo(model.roles, r => r.RoleID, id => Role.Load(id));
				}
				else
				{
					corporateUser.User.Roles.RemoveAll();
				}

				// Reset ConsecutiveFailedLogins if the account is being reactivated - JHE
				if (corporateUser.User.UserStatusID == Constants.UserStatus.LockedOut.ToInt() && model.statusId == Constants.UserStatus.Active.ToInt())
				{
					corporateUser.User.ConsecutiveFailedLogins = 0;
				}

				corporateUser.User.UserStatusID = model.statusId;
				corporateUser.User.DefaultLanguageID = model.defaultLanguageId;

				if (model.hasAccessToAllSites)
				{
					corporateUser.HasAccessToAllSites = true;
					corporateUser.Sites.RemoveAll();
				}
				else
				{
					corporateUser.HasAccessToAllSites = false;

					// Update Site Accessibility - JHE
					if (model.sites != null)
					{
						corporateUser.Sites.SyncTo(model.sites, s => s.SiteID, id => Site.Load(id));
					}
					else
					{
						corporateUser.Sites.RemoveAll();
					}
				}

				corporateUser.Save();

				// Restore Roles to unmodified state for future edits - JHE
				foreach (var role in corporateUser.User.Roles.ToList())
				{
					role.StopTracking();
					role.Users.Clear();
				}

				//SiteUsers.Replace(u => u.CorporateUserID == userId, user);

				return new SaveCorporateUserResult { Result = true, CorporateUser = corporateUser, Message = string.Empty };
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return new SaveCorporateUserResult { Result = false, Message = exception.PublicMessage };
			}
		}

		[HttpPost]
		[FunctionFilter("Admin-Create and Edit User", "~/Admin/AutoshipSchedules")]
		public virtual ActionResult Save(UsersSaveModel model)
		{
			var result = this.SaveCorporateUser(model);
			try
			{
                if (result.Result)
                {
                    var corporateUser = result.CorporateUser;
                    var account = (corporateUser != null) ? Account.GetByUserId(corporateUser.UserID) : null;

                    if (account == null && model.createShoppingAccount)
                    {
                        account = new Account
                        {
                            FirstName = model.firstName,
                            LastName = model.lastName,
                            EmailAddress = model.email,
                            UserID = corporateUser.UserID,
                            AccountTypeID = (int)Constants.AccountType.Employee,
                            AccountStatusID = ((corporateUser.User.UserStatusID == (short)Constants.UserStatus.Active) ? (short)Constants.AccountStatus.Active : (short)Constants.AccountStatus.Terminated),
                            DefaultLanguageID = corporateUser.User.Language != null ? corporateUser.User.Language.LanguageID : 1,
                            DateCreated = DateTime.Now,
                            MarketID = CoreContext.CurrentMarketId,
                            AccountSourceID = (int)ConstantsGenerated.AccountSource.ManuallyEntered
                        };
                        account.Save();
                    }

                    return Json(new { result = true, userId = corporateUser.CorporateUserID, accountId = (account != null) ? account.AccountID.ToString() : string.Empty });
                }
                else
                {
                    return Json(new { result = result.Result, message = result.Message });//, userId = result.CorporateUser.CorporateUserID
                }
			}
			catch (Exception excp)
			{
				excp.TraceException(excp);
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(excp, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[FunctionFilter("Admin-Create and Edit User", "~/Admin/AutoshipSchedules")]
		public virtual ActionResult AuditHistory(int id)
		{
			try
			{
				ViewData["EntityName"] = "CorporateUser";
				ViewData["ID"] = id;

				ViewData["Links"] = new StringBuilder("<a href=\"").Append("~/Admin/Users/Edit/".ResolveUrl()).Append(id).Append("\">")
					.Append(Translation.GetTerm("EditUser", "Edit User"))
					.Append("</a> | ").Append(Translation.GetTerm("AuditHistory", "Audit History")).ToString();

				return View("AuditHistory", "Admin");
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				throw exception;
			}
		}

		[OutputCache(CacheProfile = "AutoCompleteData")]
		public virtual ActionResult Search(string query)
		{
			try
			{
				return Json(NetSteps.Data.Entities.User.SlimSearch(query).ToAJAXSearchResults());
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

        public virtual ActionResult SearchAccount(string query)
        {
            try
            {
                return Json(Account.ListAccountUser(query).ToAJAXSearchResults());
            }
            catch (Exception)
            {
                
                throw;
            }
        }
	}
}
