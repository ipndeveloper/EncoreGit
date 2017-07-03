using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using NetSteps.Auth.UI.Common.DataObjects;
using NetSteps.Auth.UI.Common.Enumerations;
using NetSteps.Common.Base;
using NetSteps.Common.Configuration;
using NetSteps.Common.Exceptions;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Data.Entities.Utility;
using NetSteps.Diagnostics.Utilities;
using NetSteps.Encore.Core.IoC;
using NetSteps.Web;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Helpers;
using nsCore.Areas.Accounts.Models.Ledger;

namespace nsCore.Controllers
{
	public class SecurityController : BaseController
	{
		static TraceSource ts = new TraceSource("traceSource");

		[HttpGet]
		[OutputCache(CacheProfile = "DontCache")]
		public virtual ActionResult Login(bool? autoLoggedOut, string returnUrl)
		{
			ViewData["SessionExpired"] = autoLoggedOut.HasValue ? autoLoggedOut.Value : false;
			if (string.IsNullOrEmpty(returnUrl))
			{
				if (ConfigurationManager.AppSettings.AllKeys.Contains("DefaultURL"))
				{
					string defaultUrl = ConfigurationManager.AppSettings["DefaultURL"];
					if (!string.IsNullOrEmpty(defaultUrl))
						returnUrl = defaultUrl.ResolveUrl();
				}
			}
			TempData["ReturnUrl"] = string.IsNullOrEmpty(returnUrl) || returnUrl.Contains("favicon.ico") ? "~/".ResolveUrl() : returnUrl;
			return View();
		}

		[HttpPost]
		[OutputCache(CacheProfile = "DontCache")]
		public virtual ActionResult Login(string username, string password)
		{
			using (var loginTrace = this.TraceActivity(string.Format("do Login for {0}", username)))
			{
				try
				{
					using (var authTrace = this.TraceActivity("get authentication result"))
					{
						IAuthenticationUIResult authResult =
							GetAuthUIService().Authenticate(username, password, ConfigurationManager.GetAppSetting<int>(ConfigurationManager.VariableKey.NSCoreSiteID));
						this.TraceInformation(string.Format("authResult: WasLoginSuccessful - {0}", (authResult != null) ? authResult.WasLoginSuccessful.ToString() : "NULL"));

						if (!authResult.WasLoginSuccessful)
						{
							throw new NetStepsException(authResult.FailureMessage) { PublicMessage = authResult.FailureMessage };
						}

						using (var getUserTrace = this.TraceActivity("setting current user"))
						{
							User user = null;
							user = GetAccountWithCredentails(username, authResult.CredentialTypeID);
							this.TraceInformation(string.Format("got user with UserID {0}", (user != null) ? user.UserID.ToString() : "null"));

							using (var getCurrentUser = this.TraceActivity("load user"))
							{
								var corporateUser = Create.New<ICorporateUserRepository>().LoadByUserIdFull(user.UserID);
								CoreContext.CurrentUser = corporateUser;
							}
						}
					}

					using (var cookieTrace = this.TraceActivity("setting cookie"))
					{
						FormsAuthentication.SetAuthCookie(username, false);

						Response.Cookies.Add(new HttpCookie("AccountID", "1")
						{
							Path = FormsAuthentication.FormsCookiePath
						});
					}

					using (var loginRouteValidationTrace = this.TraceActivity("validating login route"))
					{
						if (!ValidateLoginRoute())
						{
							EntityExceptionHelper.GetAndLogNetStepsException(new Exception(Translation.GetTerm("InsufficentRolesAssignedToLogin", "Insufficient Roles Assigned To Login.")), Constants.NetStepsExceptionType.NetStepsApplicationException);
							ViewData["InvalidLogin"] = true;

							if (WebContext.IsLocalHost)
								ViewData["ErrorMessage"] = Translation.GetTerm("InsufficentRolesAssignedToLogin", "Insufficient Roles Assigned To Login.");

							SignOut();

							return View();
						}
					}

					using (var redirectTrace = this.TraceActivity("determining redirect"))
					{
						if (TempData["ReturnUrl"] == null)
						{
							Uri urlReferrer = Request.UrlReferrer;
							if (urlReferrer == null)
								return Redirect("~/".ResolveUrl());
							if (urlReferrer.AbsolutePath.ToLower().Contains("login"))
							{
								if (urlReferrer.Query.Contains("returnUrl"))
									return Redirect(Server.UrlDecode(urlReferrer.Query.Substring(urlReferrer.Query.IndexOf("returnUrl") + 10)));
								else
									return Redirect("~/".ResolveUrl());
							}
							return Redirect(urlReferrer.PathAndQuery);
						}

						return Redirect(TempData["ReturnUrl"].ToString());
					}
				}
				catch (Exception ex)
				{
					ex.TraceException(ex);
					EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);

					ViewData["InvalidLogin"] = true;

					if (WebContext.IsLocalHost)
					{
						ViewData["ErrorMessage"] = ex.Message;
					}

					return View();
				}
			}
		}


		protected virtual User GetAccountWithCredentails(string identifier, int credentialTypeID)
		{
			switch ((LoginCredentialTypes)credentialTypeID)
			{
				case LoginCredentialTypes.Username:
				case LoginCredentialTypes.CorporateUsername:
					return Create.New<IUserRepository>().GetByUsername(identifier);
				case LoginCredentialTypes.Email:
					return Account.LoadNonProspectByEmailFull(identifier).User;
				case LoginCredentialTypes.AccountId:
					return Account.LoadFull(int.Parse(identifier)).User;
				default:
					throw new NetStepsException(Translation.GetTerm("Login_UnableToLocateAcount", "Unable to locate account"));
			}
		}

		private bool ValidateLoginRoute()
		{
			if (!CoreContext.CurrentUser.HasFunction("Accounts"))
			{
				string newTempData = string.Empty;

				if (CoreContext.CurrentUser.HasFunction("Sites"))
				{
					newTempData = "/Sites";
				}
				else if (CoreContext.CurrentUser.HasFunction("Orders"))
				{
					newTempData = "/Orders";
				}
				else if (CoreContext.CurrentUser.HasFunction("Products"))
				{
					newTempData = "/Products";
				}
				else if (CoreContext.CurrentUser.HasFunction("Commissions"))
				{
					newTempData = "/Commissions";
				}
				else if (CoreContext.CurrentUser.HasFunction("Communication"))
				{
					newTempData = "/Communication";
				}
				else if (CoreContext.CurrentUser.HasFunction("Support"))
				{
					newTempData = "/Support";
				}
				else if (CoreContext.CurrentUser.HasFunction("Admin-Create and Edit User"))
				{
					newTempData = "/Admin";
				}

				if (string.IsNullOrWhiteSpace(newTempData))
				{
					return false;
				}
				else
				{
					TempData["ReturnUrl"] = newTempData;
				}
			}

			return true;
		}

		[OutputCache(CacheProfile = "DontCache")]
		public virtual ActionResult Logout()
		{
			SignOut();

			return RedirectToAction("Login");
		}

		[OutputCache(CacheProfile = "DontCache")]
		private void SignOut()
		{

			try
			{
				Session.Clear();
				Session.Abandon();
			}
			catch
			{
				CoreContext.CurrentUser = null;
			}
			FormsAuthentication.SignOut();
		}

		[OutputCache(CacheProfile = "DontCache")]
		public virtual ActionResult SetMarket(int marketId)
		{
			CoreContext.CurrentMarketId = marketId;

			if (Request.UrlReferrer != null && Request.UrlReferrer.Segments.ContainsIgnoreCase("Sites/"))
				return Json(new { redirectURL = Url.Content("~/Sites/") });
			else
				return null;
		}

		[OutputCache(CacheProfile = "DontCache")]
		public virtual void SetLanguage(int languageId)
		{
			CoreContext.CurrentLanguageID = languageId;
		}

		[OutputCache(CacheProfile = "DontCache")]
		public virtual void ExpireSmallCollectionCache()
		{
			SmallCollectionCache.Instance.ExpireAllCache();
		}

		[OutputCache(CacheProfile = "DontCache")]
		public virtual void ExpireTranslationCache()
		{
			CachedData.Translation.ExpireCache();
		}

		#region Authorization


		private Dictionary<string, string> _authorizedViews;

		protected virtual Dictionary<string, string> AuthorizedViews
		{
			get
			{
				if (_authorizedViews == null)
				{
					_authorizedViews = new Dictionary<string, string>()
					{
						{ "Accounts-Add Ledger Entry", "~/Areas/Accounts/Views/Ledger/EntryForm.ascx" },
						{ "Accounts-Change Sponsor", "~/Areas/Accounts/Views/Edit/ChangeSponsor.ascx" },
						{ "Orders-Change Commission Consultant", "~/Areas/Orders/Views/Shared/ChangeCommissionConsultant.ascx" },
						{ "Orders-Change Attached Party", "~/Areas/Orders/Views/Shared/ChangeAttachedParty.ascx" },
						{ "Accounts-Change Commission Consultant", "~/Areas/Accounts/Views/Autoships/ChangeCommissionConsultant.ascx" },
						{ "Orders-Override Order", "~/Areas/Orders/Views/OrderEntry/Overrides.ascx" }
					};
				}
				return _authorizedViews;
			}
		}

		[OutputCache(CacheProfile = "DontCache")]
		public virtual ActionResult Authorize(string username, string password, string function)
		{
			try
			{
				function = function.ToCleanString();

				CorporateUser user;
				if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
				{
					try
					{
						user = CorporateUser.Authenticate(username, password);
					}
					catch (Exception ex)
					{
						var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
						return Json(new { result = false, message = exception.PublicMessage });
					}
				}
				else
				{
					user = CoreContext.CurrentUser as CorporateUser;
				}

				if (user == null || !user.HasFunction(function))
				{
					// Create a new one if it doesn't Exist - JHE
					Function.CreateFunction(function);

					return Json(new { result = false, message = Translation.GetTerm("UserDoesNotHavePermissions", "User does not have permissions.") });
				}

				object model = null;
				if (function == "Accounts-Add Ledger Entry")
				{
					model = new LedgerEntryFormModel();
				}
				else if (function == "Orders-Change Commission Consultant")
				{
					var orderContext = OrderContextSessionProvider.Get(HttpContext.Session);
					model = orderContext.Order;
				}

				return Json(new { result = true, form = AuthorizedViews.ContainsKey(function) ? RenderPartialToString(AuthorizedViews[function], model) : null });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}
		#endregion

		[OutputCache(CacheProfile = "PagedGridData")]
		public virtual ActionResult GetAuditHistory(string entityName, int entityId, string columnName, string tableName, int? pk, DateTime? startDate, DateTime? endDate, int page, int pageSize, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection, string loadedEntitySessionVarKey = null)
		{
			try
			{
				if (startDate.HasValue && startDate.Value.Year < 1900)
					startDate = null;
				if (endDate.HasValue && endDate.Value.Year < 1900)
					endDate = null;

				PaginatedList<AuditLogRow> auditLogRows = null;
				if (!loadedEntitySessionVarKey.IsNullOrEmpty() && Session[loadedEntitySessionVarKey] != null)
				{
					auditLogRows = AuditHelper.GetAuditLog(Session[loadedEntitySessionVarKey], entityName, entityId, new AuditLogSearchParameters()
					{
						PK = pk.HasValue && pk.Value != 0 ? pk : null,
						ColumnName = columnName.ToCleanString(),
						TableName = tableName.ToCleanString(),
						StartDate = startDate,
						EndDate = endDate,
						PageIndex = page,
						PageSize = pageSize,
						OrderBy = orderBy,
						OrderByDirection = orderByDirection,
					});
				}
				else
				{
					auditLogRows = AuditHelper.GetAuditLog(entityName, entityId, new AuditLogSearchParameters()
					{
						PK = pk.HasValue && pk.Value != 0 ? pk : null,
						ColumnName = columnName.ToCleanString(),
						TableName = tableName.ToCleanString(),
						StartDate = startDate,
						EndDate = endDate,
						PageIndex = page,
						PageSize = pageSize,
						OrderBy = orderBy,
						OrderByDirection = orderByDirection,
					});
				}

				StringBuilder builder = new StringBuilder();

				int count = 0;
				foreach (AuditLogRow auditLogRow in auditLogRows)
				{
					builder.Append("<tr>")
						.AppendCell(auditLogRow.DateChanged.ToString("F", CoreContext.CurrentCultureInfo))
						.AppendCell(auditLogRow.PK.ToString())
						.AppendCell(auditLogRow.TableName)
						.AppendCell(auditLogRow.ColumnName)
                        .AppendCell(GetNumberByCulture(auditLogRow.OldValue))
                        .AppendCell(GetNumberByCulture(auditLogRow.NewValue))
						.AppendCell(auditLogRow.Username)
						.AppendCell(auditLogRow.ApplicationName)
						.Append("</tr>");
					++count;
				}

				return Json(new { totalPages = auditLogRows.TotalPages, page = builder.ToString() });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

        private string GetNumberByCulture(string valor)
        {
            
            decimal numero;
            DateTime fecha;
            var culture = CoreContext.CurrentCultureInfo;

           
                
           
            if (System.Web.WebPages.StringExtensions.IsDecimal(valor) == true)
            {
                return GetCulturaFormat(valor, true);
            }
            else if (System.Web.WebPages.StringExtensions.IsDateTime(valor) == true)
            {
                return GetCulturaFormat(valor, false);
            }
            else
            {
                return valor;
            }

        }

        public string GetCulturaFormat(string valor , bool esDecimal)
        {
            var fomatos = new List<System.Globalization.CultureInfo>
            {
                new System.Globalization.CultureInfo("pt-BR"),
                new System.Globalization.CultureInfo("en-US"),
                new System.Globalization.CultureInfo("es-US")

            };
            bool correcto = false;
            decimal numero=0;
            DateTime fecha = DateTime.Now ;

          
            foreach (var item in fomatos)
            {
                if (esDecimal)
                {
                    if (decimal.TryParse(valor, System.Globalization.NumberStyles.AllowDecimalPoint, item, out numero) == true)
                    {
                        correcto = true;
                        break;
                    }
                }
                else
                {
                    if (DateTime.TryParse(valor, item, System.Globalization.DateTimeStyles.None, out fecha) == true)
                    {
                        correcto = true;
                        break;
                    }

                }
               
            }
            if (correcto)
            {
                if (esDecimal)
                    return numero.ToString("N", CoreContext.CurrentCultureInfo);
                else
                    return fecha.ToString(CoreContext.CurrentCultureInfo);
            }
            else
               return valor;
        
          
        }
	}
}
