using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using NetSteps.Common;
using NetSteps.Common.Base;
using NetSteps.Common.Configuration;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Commissions;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Globalization;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Business.Controls;
using NetSteps.Web.Mvc.Helpers;
using nsCore5.Models;

namespace nsCore.Controllers
{
	public class AccountsController : BaseController
	{
		[FunctionFilter("Accounts", "~/Sites")]
		public ActionResult Index()
		{
			CoreContext.CurrentOrder = null;
			CoreContext.CurrentAccount = null;
			return View();
		}

		public ActionResult Search(string query)
		{
			List<AccountSearchData> accounts = Account.SearchNameAndAccountNumber(query);
			return Json(accounts.Select(a => new { id = a.AccountID, text = a.FullName + " (#" + a.AccountNumber + ")" }));
		}

		public ActionResult SearchActive(string query)
		{
			List<AccountSearchData> accounts = Account.SearchNameAndAccountNumber(query).Where(a => a.AccountStatusID == (int)NetSteps.Data.Entities.Constants.AccountStatus.Active).ToList();
			return Json(accounts.Select(a => new { id = a.AccountID, text = a.FullName + " (#" + a.AccountNumber + ")" }));
		}

		public void SaveStatus(int statusId, int changeReasonId)
		{
			if (CoreContext.CurrentAccount.AccountStatusID != statusId)
			{
				CoreContext.CurrentAccount.AccountStatusChangeReasonID = changeReasonId.ToShortNullable();
				CoreContext.CurrentAccount.AccountStatusID = statusId;
				CoreContext.CurrentAccount.Save();
			}
		}

		#region Overview
		[FunctionFilter("Accounts", "~/Sites")]
		public ActionResult Overview(string id)
		{
			if (!string.IsNullOrEmpty(id))
			{
				CoreContext.CurrentAccount = Account.LoadByAccountNumber(id);
			}
			if (CoreContext.CurrentAccount == null)
				return RedirectToAction("Index");

			// TODO: What are ProxyLinks? - JHE
			var proxyLinkCollection = ProxyLink.LoadAll();
			StringBuilder links = new StringBuilder();

			// Process Template data in links
			ProxyLink.ProcessProxyLinks(proxyLinkCollection, CoreContext.CurrentAccount.AccountID);

			foreach (ProxyLink proxyLink in proxyLinkCollection.Where(pl => pl.Active))
			{
				links.Append(string.Format("<li><a href=\"{0}\" target=\"_blank\" rel=\"external\">{1}</a></li>", proxyLink.URL, proxyLink.DisplayName));
			}
			ViewData["ProxyLinks"] = links.ToString();

			//TODO: finish up the autoship tables and add account autoship types to the account overview - DES
			//ViewData["AutoshipSchedules"] = AutoshipSchedule.GetAllSchedules().Where(s => s.Active).ToList();

			Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);

			return View();
		}

		public ActionResult GetOrderHistory(int page, int pageSize, int? status, string accountNumberOrName, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection, string orderNumber)
		{
			var orders = Order.SearchOrders(new NetSteps.Data.Entities.Business.OrderSearchParameters()
			{
				PageIndex = page,
				PageSize = pageSize,
				OrderStatusID = status,
				AccountNumberOrName = accountNumberOrName,
				OrderNumber = orderNumber,
				OrderBy = orderBy,
				OrderByDirection = orderByDirection,
			});
			if (orders.TotalCount > 0)
			{
				StringBuilder builder = new StringBuilder();
				int count = 0;
				foreach (OrderSearchData order in orders)
				{
					// I broke up the builder.Append calls to make this easier to debug. NDG.
					builder.Append("<tr class=\"").Append(count % 2 == 0 ? "GridRow" : "GridRowAlt")
						.Append(order.IsReturnOrder() ? " ReturnOrder" : "")
						.Append(order.IsAutoshipOrder() ? " AutoShipOrder" : "")
						.Append("\">");
					builder.Append("<td><a href=\"").Append("~/Orders/Details/".ResolveUrl()).Append(order.OrderNumber).Append("\">")
						.Append(order.OrderNumber).Append("</a></td>");
					builder.Append("<td>").Append(!order.DateCreated.HasValue ? "N/A" : order.DateCreated.ToShortDateString()).Append("</td>");
					builder.Append("<td>").Append(order.OrderType).Append("</td>");
					builder.Append("<td>").Append(order.OrderStatus).Append("</td>");
					builder.Append("<td>").Append(!order.DateShipped.HasValue ? "N/A" : order.DateShipped.ToShortDateString()).Append("</td>");
					// I don't know why, but adding a style to these cells causes the grid to not display any data.
					builder.Append("<td>")
						//builder.Append("<td style=\"text-align: right; padding-right: 10px;\">")
						.Append(order.SubTotal.ToString("C")).Append("</td>");
					builder.Append("<td>")
						//builder.Append("<td style=\"text-align: right; padding-right: 10px;\">")
						.Append(order.GrandTotal.ToString("C")).Append("</td>");
					builder.Append("</tr>");
					++count;
				}
				return Json(new { totalPages = orders.TotalPages, page = builder.ToString() });
			}
			else
			{
				return Json(new { totalPages = 0, page = "<tr><td colspan=\"7\">No orders found with that criteria.  Please try again.</td></tr>" });
			}
		}

		[FunctionFilter("Accounts", "~/Sites")]
		public ActionResult StatusHistory(string id)
		{
			if (!string.IsNullOrEmpty(id))
			{
				CoreContext.CurrentAccount = Account.LoadByAccountNumber(id);
			}
			if (CoreContext.CurrentAccount == null)
				return RedirectToAction("Index");

			throw EntityExceptionHelper.GetAndLogNetStepsException("Finish porting this code - JHE");
			//return View(Account.GetStatusChangeHistory(CoreContext.CurrentAccount.AccountID));
		}

		[FunctionFilter("Accounts", "~/Sites")]
		public ActionResult AuditHistory()
		{
			if (CoreContext.CurrentAccount == null)
				return RedirectToAction("Index");

			return View(new PaginatedList<AuditLogRow>());
		}

		public ActionResult GetAccountAuditHistory(int page, int pageSize, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection)
		{
			if (CoreContext.CurrentAccount == null)
				return RedirectToAction("Index");

			int id = CoreContext.CurrentAccount.AccountID;
			PaginatedList<AuditLogRow> auditLogRows = Account.GetAuditLog(id, new PaginatedListParameters()
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

		[FunctionFilter("Accounts", "~/Sites")]
		public ActionResult PoliciesChangeHistory(string id)
		{
			if (!string.IsNullOrEmpty(id))
			{
				CoreContext.CurrentAccount = Account.LoadByAccountNumber(id);
			}
			if (CoreContext.CurrentAccount == null)
				return RedirectToAction("Index");

			return View(AccountPolicy.LoadByAccountID(CoreContext.CurrentAccount.AccountID));
		}
		#endregion

		#region Search Accounts
		[FunctionFilter("Accounts", "~/Sites")]
		public ActionResult SearchAccounts(int? status, int? type, int? state, string email, int? sponsor, DateTime? startDate, DateTime? endDate)
		{
			var searchParams = new AccountSearchParameters()
				{
					AccountStatusID = status,
					AccountTypeID = type,
					StateProvinceID = state,
					Email = email,
					SponsorID = sponsor,
					StartDate = startDate,
					EndDate = endDate
				};
			var accounts = Account.SearchAccounts(searchParams);
			if (accounts.TotalCount == 1)
				return RedirectToAction("Overview", new { id = accounts.First().AccountNumber });
			return View(searchParams);
		}

		public ActionResult GetAccounts(int page, int pageSize, int? status, int? type, int? state, string email, int? sponsorId, DateTime? startDate, DateTime? endDate, string accountNumberOrName, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection)
		{
			if (startDate.HasValue && startDate.Value.Year < 1900)
				startDate = null;
			if (endDate.HasValue && endDate.Value.Year < 1900)
				endDate = null;

			StringBuilder builder = new StringBuilder();
			PaginatedListParameters p = new PaginatedListParameters()
			{
			};
			var accounts = Account.SearchAccounts(new AccountSearchParameters()
			{
				AccountStatusID = status,
				AccountTypeID = type,
				StateProvinceID = state,
				Email = email,
				SponsorID = sponsorId,
				StartDate = startDate,
				EndDate = endDate,
				WhereClause = !string.IsNullOrEmpty(accountNumberOrName) ? a => a.AccountNumber.Contains(accountNumberOrName) || (a.FirstName + " " + a.LastName).Contains(accountNumberOrName) : (Expression<Func<Account, bool>>)null,
				PageIndex = page,
				PageSize = pageSize,
				OrderBy = orderBy,
				OrderByDirection = orderByDirection
			});
			if (accounts.Count > 0)
			{
				int count = 0;
				foreach (AccountSearchData account in accounts)
				{
					builder.Append("<tr class=\"").Append(count % 2 == 0 ? "GridRow" : "GridRowAlt").Append("\">")
						.AppendLinkCell("~/Accounts/Overview/Index" + account.AccountNumber, account.AccountNumber)
						.AppendCell(account.FirstName)
						.AppendCell(account.LastName)
						.AppendCell(account.AccountType)
						.AppendCell(SmallCollectionCache.Instance.AccountStatuses.GetById(account.AccountStatusID).Name)
						.AppendCell(account.DateEnrolled.ToShortDateString())
						.AppendCell(account.EmailAddress)
						.AppendCell(account.Sponsor)
						.AppendCell(account.Location)
						.Append("</tr>");
					++count;
				}
				return Json(new { totalPages = accounts.TotalPages, page = builder.ToString() });
			}
			else
			{
				return Json(new { totalPages = 0, page = "<tr><td colspan=\"9\">There were no records found that meet that criteria.  Please try again.</td></tr>" });
			}
		}
		#endregion

		#region Notes
		[FunctionFilter("Accounts-Notes", "~/Accounts/Overview")]
		public ActionResult AddNote(int? parentId, string subject, string noteText)
		{
			if (string.IsNullOrEmpty(subject) && string.IsNullOrEmpty(noteText))
			{
				return Json(new { result = false, message = "Note not saved.  No subject or text could be found for this note." });
			}

			Note newNote = new Note();
			newNote.StartEntityTracking();
			newNote.DateCreated = DateTime.Now;
			newNote.NotesTypeID = NetSteps.Data.Entities.Constants.NoteType.AccountNotes.ToInt();
			newNote.NoteText = noteText;
			newNote.UserID = ApplicationContext.Instance.CurrentUser.UserID;

			if (parentId.HasValue && parentId.Value > 0)
			{
				var parentNode = CoreContext.CurrentAccount.Notes.FirstOrDefault(an => an.NoteID == parentId.Value);
				if (parentNode == null)
					throw EntityExceptionHelper.GetAndLogNetStepsException("Error: Parent Note not found");
				parentNode.StartEntityTracking();
				parentNode.FollowupNotes.Add(newNote);
				parentNode.Save();
			}
			else
			{
				CoreContext.CurrentAccount.Notes.Add(newNote);
				CoreContext.CurrentAccount.Save();
			}

			if (newNote.NoteID > 0)
				return Json(new { result = true });
			else
				return Json(new { result = false, message = "Note not saved" });
		}

		public ActionResult GetNotes(DateTime startDate, DateTime endDate, string searchCriteria)
		{
			List<Note> filteredList = Note.FilterNotes(startDate, endDate, searchCriteria, CoreContext.CurrentAccount.Notes);
			filteredList.RemoveWhere(n => n.ParentID != null);

			if (filteredList.Count == 0)
			{
				if (string.IsNullOrEmpty(searchCriteria.Trim()))
					return Content("<div style=\"margin-left:10px;\">No notes posted.</div>");
				else
					return Content("<div style=\"margin-left:10px;\">No notes found.</div>");
			}
			return Content(BuildNotes(filteredList, false));
		}

		public string BuildNotes(IList<Note> notes, bool isChild)
		{
			if (notes == null)
				return null;
			StringBuilder builder = new StringBuilder();
			notes.Each((n, i) =>
			{
				if (!isChild)
				{
					builder.Append("<div class=\"AcctNote").Append(i % 2 == 0 ? "" : " Alt").Append("\">").Append("<span class=\"FL NoteTitle\"><b>").Append(n.Subject).Append(" (#").Append(n.NoteID).Append(")</b> (")
						.Append(n.FollowupNotes.Count).Append(" Follow-up(s)) </span><span class=\"FR ExpandNote\"><a style=\"cursor: pointer\" onclick=\"createNewFollowup(").Append(n.NoteID).Append(");\" >Post Follow-up</a>");
					if (n.FollowupNotes.Count > 0)
					{
						builder.Append(" | <a class=\"toggleChildNotes\" style=\"cursor: pointer\">Collapse</a>");
					}
					builder.Append("</span><span class='ClearAll'></span><span class=\"NoteAuthor\">Posted on: ").Append(n.DateCreated.ToShortDateString()).Append("</b><br />").Append("Posted by: #")
					.Append(CoreContext.CurrentAccount.AccountID).Append("</span>").Append(n.NoteText);

					if (n.FollowupNotes.Count > 0)
					{
						builder.Append("<div class=\"ChildNotes\">").Append(BuildNotes(n.FollowupNotes, true)).Append("</div>");
					}
					builder.Append("</div>");
				}
				else
				{
					builder.Append("<div class=\"NoteReply\"><b>").Append(n.Subject).Append("</b> (").Append(n.FollowupNotes.Count).Append(" Follow-up)  <span class=\"FR ExpandNote\"><a style=\"cursor: pointer\" onclick=\"CreateNewFollowup(")
						.Append(n.NoteID).Append(");\" >Post Follow-up</a></span><span class=\"ClearAll\" />").Append("<span class=\"NoteAuthor\">Posted on:")
						.Append(n.DateCreated.ToShortDateString()).Append("<br />Posted by: #").Append(CoreContext.CurrentAccount.AccountID).Append("</span>").Append(n.NoteText);

					if (n.FollowupNotes.Count > 0)
					{
						builder.Append("<div class=\"ChildNotes\">").Append(BuildNotes(n.FollowupNotes, true)).Append("</div>");
					}
					builder.Append("</div>");
				}
			});
			return builder.ToString();
		}
		#endregion

		#region Full Account Record
		[FunctionFilter("Accounts-Create and Edit Account", "~/Accounts/Overview")]
		public ActionResult EditAccount(string id)
		{
			if (!string.IsNullOrEmpty(id))
			{
				CoreContext.CurrentAccount = Account.LoadByAccountNumber(id);
			}
			if (CoreContext.CurrentAccount == null)
				return RedirectToAction("Index");

			return View();
		}

		public ActionResult SaveAccount(int accountId, int accountType, string sponsorAccountNumber, bool applicationOnFile, string username,
			string password, string confirmPassword, bool userChangingPassword, bool generatedPassword, string profileName, string attention, string address1,
			string address2, string address3, string zip, string city, string county, string state, /*string country,*/ string phone, string firstName,
			string lastName, string homePhone, string email, bool isTaxExempt, string ssn, DateTime dob, NetSteps.Data.Entities.Constants.Gender gender)
		{
			try
			{
				Account account = accountId > 0 ? Account.LoadFull(accountId) : new Account();
				account.StartEntityTracking();
				if (!string.IsNullOrEmpty(sponsorAccountNumber) && (account.Sponsor == null || account.Sponsor.AccountNumber != sponsorAccountNumber))
				{
					Account sponsor = Account.LoadByAccountNumber(sponsorAccountNumber);
					account.SponsorID = sponsor.AccountID;
					account.Save();
				}
				account.ReceivedApplication = applicationOnFile;

				if (account.User == null)
				{
					account.User = new NetSteps.Data.Entities.User()
					{
						UserStatusID = NetSteps.Data.Entities.Constants.UserStatus.Active.ToShort(),
						UserTypeID = NetSteps.Data.Entities.Constants.UserType.Distributor.ToShort()
					};
					account.User.Roles.Add(Role.Load(NetSteps.Data.Entities.Constants.Role.LimitedUser.ToInt()));
				}

				// TODO: make sure the Username entered is not taken by someone else - JHE
				account.User.Username = username.ToCleanString();

				if (userChangingPassword)
				{
					var result = NetSteps.Data.Entities.User.NewPasswordIsValid(password, confirmPassword);
					if (result.Success)
						account.User.Password = password.ToCleanString();
					else
						return Json(new { result = false, message = result.Message });
				}

				account.FirstName = firstName.ToCleanString();
				account.LastName = lastName.ToCleanString();
				account.EmailAddress = email.ToCleanString();
				account.IsTaxExempt = isTaxExempt;
				if (!ssn.Contains("*"))
					account.DecryptedTaxNumber = ssn.ToCleanString();
				account.Birthday = dob;
				account.GenderID = gender.ToShortNullable();
				account.MainPhone = homePhone.ToCleanString();
				//account.Save();

				if (generatedPassword)
					Account.SendGeneratedPasswordEmail(account);

				Address address = account.Addresses.GetByTypeID(NetSteps.Data.Entities.Generated.ConstantsGenerated.AddressType.Main);
				if (address == null)
				{
					address = new Address();
					account.Addresses.Add(address);
				}
				address.AttachAddressChangedCheck();
				address.ProfileName = profileName.ToCleanString();
				address.Attention = attention.ToCleanString();
				address.Address1 = address1.ToCleanString();
				address.Address2 = address2.ToCleanString();
				address.Address3 = address3.ToCleanString();
				address.City = city.ToCleanString();
				address.State = state.ToCleanString();
				address.County = county.ToCleanString();
				address.PostalCode = zip.ToCleanString();
				address.CountryID = NetSteps.Data.Entities.Constants.Country.UnitedStates.ToInt();
				address.PhoneNumber = phone.ToCleanString();
				address.AddressTypeID = NetSteps.Data.Entities.Constants.AddressType.Main.ToInt();
				address.LookUpAndSetGeoCode();

				account.Save();

				CoreContext.CurrentAccount = Account.LoadFull(account.AccountID); // Is this necessary? - JHE

				return Json(new { result = true });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}
		#endregion

		#region Site Subscriptions
		[FunctionFilter("Accounts-Site Subscriptions", "~/Accounts/Overview")]
		public ActionResult SiteSubscriptions(string id)
		{
			if (!string.IsNullOrEmpty(id))
			{
				CoreContext.CurrentAccount = Account.LoadByAccountNumber(id);
			}
			if (CoreContext.CurrentAccount == null)
				return RedirectToAction("Index");

			if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["MasterSiteId"]))
			{
				Site masterSite = Site.Load(int.Parse(ConfigurationManager.AppSettings["MasterSiteId"]));
				try
				{
					ViewData["Domains"] = masterSite != null && masterSite.Settings["Domains"] != null ? masterSite.Settings["Domains"].Value.ToString().Split(';') : ConfigurationManager.AppSettings["Domains"].Split(';');
				}
				catch
				{
					ViewData["Domains"] = new string[] { "netsteps.com" };
				}
			}
			else if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["Domains"]))
			{
				ViewData["Domains"] = ConfigurationManager.AppSettings["Domains"].Split(';');
			}

			Site distributorSite = Site.LoadByAccountID(CoreContext.CurrentAccount.AccountID);
			if (distributorSite != null)
				return View(distributorSite.SiteUrls);
			return View(new List<SiteUrl>());
		}

		public ActionResult CheckIfAvailableUrl(string url)
		{
			return Json(new { available = SiteUrl.IsAvailable(url) });
		}

		[FunctionFilter("Accounts-Site Subscriptions", "~/Accounts/Overview")]
		public ActionResult SaveSiteSubscriptions(List<string> urls)
		{
			try
			{
				Site site = Site.LoadByAccountID(CoreContext.CurrentAccount.AccountID);
				List<bool> successes = new List<bool>();
				for (int i = 0; i < urls.Count; i++)
				{
					string url = urls[i];
					if (site.SiteUrls.ElementAt(i).Url != url && SiteUrl.IsAvailable(url))
					{
						site.SiteUrls.ElementAt(i).Url = url;
						site.SiteUrls.ElementAt(i).Save();
						successes.Add(true);
					}
					else if (site.SiteUrls.ElementAt(i).Url == url)
					{
						successes.Add(true);
					}
					else
					{
						successes.Add(false);
					}
				}
				return Json(new { succeeded = true, successes = successes });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}
		#endregion

		#region Billing & Shipping Profiles
		[FunctionFilter("Accounts-Billing and Shipping Profiles", "~/Accounts/Overview")]
		public ActionResult BillingShippingProfiles(string id)
		{
			if (!string.IsNullOrEmpty(id))
			{
				CoreContext.CurrentAccount = Account.LoadByAccountNumber(id);
			}
			if (CoreContext.CurrentAccount == null)
				return RedirectToAction("Index");

			return View();
		}

		public ActionResult LookupZip(int countryId, string zip)
		{
			if (zip.Length == 5)
			{
				return Json(LocationProviders.PostalCodeLookupProvider.LookupPostalCode(countryId, zip).Select(r => new { city = r.City.ToTitleCase(), county = r.County.ToTitleCase(), stateId = r.StateID, state = r.StateAbbreviation }));
			}
			else if (zip.Length == 9)
			{
				string zipPlusFour = zip.Substring(5);
				zip = zip.Substring(0, 5);
				return Json(LocationProviders.PostalCodeLookupProvider.LookupPostalCode(countryId, zip).Select(r => new { city = r.City.ToTitleCase(), county = r.County.ToTitleCase(), stateId = r.StateID, state = r.StateAbbreviation }));
			}
			return Json(new List<NetSteps.Common.Globalization.PostalCodeData>());
		}

		public ActionResult GetAddressControl(int countryId, int addressId)
		{
			Address address = Address.Load(addressId);
			return Content(AddressControl.RenderAddress(address, CoreContext.CurrentLanguageID, SmallCollectionCache.Instance.Countries.First(c => c.CountryID == countryId)));
		}

		#region Shipping Addresses
		public ActionResult AddressModal(int? addressId)
		{
			ViewData["Countries"] = SmallCollectionCache.Instance.Countries;
			return PartialView(addressId.HasValue ? CoreContext.CurrentAccount.Addresses.GetByAddressID(addressId.Value) : new Address());
		}

		public ActionResult GetAddresses()
		{
			StringBuilder builder = new StringBuilder();
			int count = 0;
			foreach (Address address in CoreContext.CurrentAccount.Addresses.Where(a => a.AddressTypeID == NetSteps.Data.Entities.Constants.AddressType.Shipping.ToInt()))
			{
				builder.Append("<div class=\"Profile").Append(count % 2 == 1 ? " Alt" : "").Append("\"><span class=\"FR\"><a href=\"javascript:void(0);\" onclick=\"setDefaultAddress(")
					.Append(address.AddressID).Append(")\"").Append(address.IsDefault ? "style=\"display:none;\"" : "").Append(">Set As Default Address</a><span style=\"margin-left: 5px; margin-right: 5px;\">")
					.Append(address.IsDefault ? "" : "|").Append("</span><a href=\"javascript:void(0);\" class=\"deletePaymentMethod\" onclick=\"deleteAddress(")
					.Append(address.AddressID).Append(")\">Delete Address</a></span><div><b><a title=\"Edit\" style=\"cursor: pointer;\" onclick=\"editAddress(")
					.Append(address.AddressID).Append(");\">").Append(string.IsNullOrEmpty(address.ProfileName) ? "Edit" : address.ProfileName).Append("</a></b><span class=\"isDefault\">")
					.Append(address.IsDefault ? " (default)" : "").Append("</span><br />").Append(address.ToString().ToHtmlBreaks())
					.Append("</div></div>");
				++count;
			}
			return Content(builder.ToString());
		}

		[FunctionFilter("Accounts-Billing and Shipping Profiles", "~/Accounts/Overview")]
		public ActionResult SetDefaultAddress(int addressId)
		{
			try
			{
				Address address = CoreContext.CurrentAccount.Addresses.FirstOrDefault(a => a.AddressID == addressId);
				if (address == null)
					throw EntityExceptionHelper.GetAndLogNetStepsException("That address is not associated with the current account.");
				if (!address.IsDefault)
				{
					address.StartEntityTracking();
					address.IsDefault = true;
					address.Save();
				}
				//Set the in memory objects, since we won't reload them after this operation
				foreach (Address a in CoreContext.CurrentAccount.Addresses.GetAllByTypeID((NetSteps.Data.Entities.Constants.AddressType)address.AddressTypeID))
				{
					if (a.IsDefault && a.AddressID != addressId)
					{
						a.IsDefault = false;
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

		[AcceptVerbs(HttpVerbs.Post)]
		[FunctionFilter("Accounts-Billing and Shipping Profiles", "~/Accounts/Overview")]
		public ActionResult SaveAddress(int? addressId, string profileName, string attention, string addressLine1, string addressLine2,
			string addressLine3, string zip, string city, string state, int countryId, string phone)
		{
			try
			{
				Address address = null;
				if (addressId.HasValue && addressId > 0)
				{
					address = CoreContext.CurrentAccount.Addresses.GetByAddressID(addressId.Value);
				}
				else
				{
					address = new Address();
					CoreContext.CurrentAccount.Addresses.Add(address);
				}

				address.AttachAddressChangedCheck();
				address.Address1 = addressLine1.ToCleanString();
				address.Address2 = addressLine2.ToCleanString();
				address.Address3 = addressLine3.ToCleanString();
				address.ProfileName = profileName.ToCleanString();
				address.Attention = attention.ToCleanString();
				address.AddressTypeID = NetSteps.Data.Entities.Constants.AddressType.Shipping.ToInt();
				address.City = city.ToCleanString();
				address.State = state.ToCleanString();
				address.PostalCode = zip.ToCleanString();
				address.CountryID = countryId;
				address.PhoneNumber = phone.ToCleanString();
				address.LookUpAndSetGeoCode();
				CoreContext.CurrentAccount.Save();

				if (!addressId.HasValue || addressId < 1)
				{
					CoreContext.CurrentAccount.Addresses.Add(address);
				}

				return Json(new { result = true });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[FunctionFilter("Accounts-Billing and Shipping Profiles", "~/Accounts/Overview")]
		public ActionResult DeleteAddress(int addressId)
		{
			try
			{
				Address.Delete(addressId);
				if (CoreContext.CurrentAccount.Addresses.Select(a => a.AddressID).Contains(addressId))
				{
					CoreContext.CurrentAccount.Addresses.Remove(CoreContext.CurrentAccount.Addresses.First(a => a.AddressID == addressId));
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

		#region Payment Methods
		public ActionResult PaymentMethodModal(int? paymentMethodId)
		{
			ViewData["Countries"] = SmallCollectionCache.Instance.Countries;

			AccountPaymentMethod accountPaymentMethod = null;
			if (paymentMethodId.HasValue && paymentMethodId.Value > 0)
				accountPaymentMethod = CoreContext.CurrentAccount.AccountPaymentMethods.GetByAccountPaymentMethodID(paymentMethodId.Value);
			else
				accountPaymentMethod = new AccountPaymentMethod() { BillingAddress = new Address() };

			if (accountPaymentMethod.BillingAddress == null)
				accountPaymentMethod.BillingAddress = new Address()
				{
					AddressTypeID = NetSteps.Data.Entities.Constants.AddressType.Billing.ToInt()
				};

			return PartialView(accountPaymentMethod);
		}

		public ActionResult GetPaymentMethods()
		{
			StringBuilder builder = new StringBuilder();
			int count = 0;
			foreach (AccountPaymentMethod paymentMethod in CoreContext.CurrentAccount.AccountPaymentMethods)
			{
				builder.Append("<div class=\"Profile").Append(count % 2 == 1 ? " Alt" : "").Append("\"><span class=\"FR\"><a href=\"javascript:void(0);\" class=\"defaultPaymentMethod\" onclick=\"setDefaultPaymentMethod(")
					.Append(paymentMethod.AccountPaymentMethodID).Append(")\"").Append(paymentMethod.IsDefault ? "style=\"display:none;\"" : "").Append(">Set As Default Payment Method</a><span style=\"margin-left: 5px; margin-right: 5px;\">")
					.Append(paymentMethod.IsDefault ? "" : "|").Append("</span><a href=\"javascript:void(0);\" class=\"deletePaymentMethod\" onclick=\"deletePaymentMethod(")
					.Append(paymentMethod.AccountPaymentMethodID).Append(")\">Delete Payment Method</a></span><div><b><a title=\"Edit\" style=\"cursor: pointer;\" onclick=\"editPaymentMethod(")
					.Append(paymentMethod.AccountPaymentMethodID).Append(");\">").Append(string.IsNullOrEmpty(paymentMethod.ProfileName) ? "Edit" : paymentMethod.ProfileName).Append("</a></b><span class=\"isDefault\">")
					.Append(paymentMethod.IsDefault ? " (default)" : "").Append("</span><br />").Append(paymentMethod.DecryptedAccountNumber).Append("<br />").Append(paymentMethod.FormatedExpirationDate)
					.Append("<br /></div></div>");
				++count;
			}
			return Content(builder.ToString());
		}

		public ActionResult GetAddress(int? addressId)
		{
			ViewData["Countries"] = SmallCollectionCache.Instance.Countries;
			return PartialView("Address", addressId.HasValue ? CoreContext.CurrentAccount.Addresses.GetByAddressID(addressId.Value) : new Address());
		}

		[FunctionFilter("Accounts-Billing and Shipping Profiles", "~/Accounts/Overview")]
		public ActionResult SetDefaultPaymentMethod(int paymentMethodId)
		{
			try
			{
				AccountPaymentMethod paymentMethod = CoreContext.CurrentAccount.AccountPaymentMethods.Cast<AccountPaymentMethod>().FirstOrDefault(pm => pm.AccountPaymentMethodID == paymentMethodId);
				if (paymentMethod == null)
					throw EntityExceptionHelper.GetAndLogNetStepsException("That payment method is not associated with the current account.");
				if (!paymentMethod.IsDefault)
				{
					paymentMethod.StartEntityTracking();
					paymentMethod.IsDefault = true;
					paymentMethod.Save();
				}
				foreach (AccountPaymentMethod pm in CoreContext.CurrentAccount.AccountPaymentMethods)
				{
					if (pm.IsDefault && pm.AccountPaymentMethodID != paymentMethodId)
					{
						pm.IsDefault = false;
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

		[HttpPost]
		[FunctionFilter("Accounts-Billing and Shipping Profiles", "~/Accounts/Overview")]
		public ActionResult SavePaymentMethod(int? paymentMethodId, string accountName, string nameOnCard, string accountNumber,
			DateTime expDate, string profileName, string attention, string addressLine1, string addressLine2, string addressLine3, string zip,
			string city, string state, int countryId, string phone, int? addressId)
		{
			try
			{
				Account account = CoreContext.CurrentAccount;
				AccountPaymentMethod paymentMethod = null;
				if (paymentMethodId.HasValue && paymentMethodId.Value > 0)
				{
					//paymentMethod = CoreContext.CurrentAccount.AccountPaymentMethods.First(a => a.AccountPaymentMethodID == paymentMethodId.Value);
					paymentMethod = AccountPaymentMethod.LoadFull(paymentMethodId.Value);
				}
				else
				{
					paymentMethod = new AccountPaymentMethod();
					CoreContext.CurrentAccount.AccountPaymentMethods.Add(paymentMethod);
				}
				paymentMethod.ProfileName = accountName.ToCleanString();
				paymentMethod.PaymentTypeID = (int)NetSteps.Data.Entities.Constants.PaymentType.CreditCard;
				if (!accountNumber.Contains("*"))
					paymentMethod.DecryptedAccountNumber = accountNumber;
				paymentMethod.NameOnCard = nameOnCard.ToCleanString();
				paymentMethod.ExpirationDate = expDate;

				Address billingAddress = null;
				if (addressId.HasValue && addressId.Value > 0)
				{
					billingAddress = CoreContext.CurrentAccount.Addresses.GetByAddressID(addressId.Value);
				}
				else
				{
					billingAddress = new Address();
					//CoreContext.CurrentAccount.Addresses.Add(billingAddress);
					paymentMethod.BillingAddress = billingAddress;
				}

				billingAddress.AttachAddressChangedCheck();
				billingAddress.ProfileName = profileName.ToCleanString();
				billingAddress.AddressTypeID = NetSteps.Data.Entities.Constants.AddressType.Billing.ToInt();
				billingAddress.Attention = attention.ToCleanString();
				billingAddress.Address1 = addressLine1.ToCleanString();
				billingAddress.Address2 = addressLine2.ToCleanString();
				billingAddress.Address3 = addressLine3.ToCleanString();
				billingAddress.City = city.ToCleanString();
				billingAddress.State = state.ToCleanString();
				billingAddress.PostalCode = zip.ToCleanString();
				billingAddress.CountryID = countryId;
				billingAddress.PhoneNumber = phone.ToCleanString();
				billingAddress.LookUpAndSetGeoCode();
				//billingAddress.Save();

				//if (!addressId.HasValue)
				//{
				//    CoreContext.CurrentAccount.Addresses.Add(billingAddress);
				//}

				paymentMethod.BillingAddress = billingAddress;

				//if (!paymentMethodId.HasValue)
				//{
				//    CoreContext.CurrentAccount.AccountPaymentMethods.Add(paymentMethod);
				//}

				CoreContext.CurrentAccount.Save();

				return Json(new { result = true });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[FunctionFilter("Accounts-Billing and Shipping Profiles", "~/Accounts/Overview")]
		public ActionResult DeletePaymentMethod(int paymentMethodId)
		{
			try
			{
				AccountPaymentMethod.Delete(paymentMethodId);
				return Json(new { result = true });
			}
			catch (Exception ex)
			{
				return Json(new { result = false, message = ex.Message });
			}
		}
		#endregion
		#endregion

		#region Ledger
		public Dictionary<int, List<AccountLedger>> Ledgers
		{
			get
			{
				if (Session["Ledgers"] == null)
					Session["Ledgers"] = new Dictionary<int, List<AccountLedger>>();
				return Session["Ledgers"] as Dictionary<int, List<AccountLedger>>;
			}
			set { Session["Ledgers"] = value; }
		}

		[FunctionFilter("Accounts-Ledger", "~/Accounts/Overview")]
		public ActionResult Ledger(string id)
		{
			if (!string.IsNullOrEmpty(id))
			{
				CoreContext.CurrentAccount = Account.LoadByAccountNumber(id);
			}
			if (CoreContext.CurrentAccount == null)
				return RedirectToAction("Index");

			if (!Ledgers.ContainsKey(CoreContext.CurrentAccount.AccountID))
			{
				//Ledgers.Add(CoreContext.CurrentAccount.AccountID, NetSteps.Commissions.Business.AccountLedgerEntry.GetAccountLedgerByAccountID(CoreContext.CurrentAccount.AccountID));
				Ledgers.Add(CoreContext.CurrentAccount.AccountID, AccountLedger.LoadByAccountID(CoreContext.CurrentAccount.AccountID));
			}

			return View(Ledgers[CoreContext.CurrentAccount.AccountID]);
		}

		public ActionResult GetLedgerEntries(int page, int pageSize)
		{
			if (!Ledgers.ContainsKey(CoreContext.CurrentAccount.AccountID))
			{
				//Ledgers.Add(CoreContext.CurrentAccount.AccountID, NetSteps.Commissions.Business.AccountLedgerEntry.GetAccountLedgerByAccountID(CoreContext.CurrentAccount.AccountID));
				Ledgers.Add(CoreContext.CurrentAccount.AccountID, AccountLedger.LoadByAccountID(CoreContext.CurrentAccount.AccountID));
			}
			IEnumerable<AccountLedger> entries = Ledgers[CoreContext.CurrentAccount.AccountID].OrderByDescending(le => le.EffectiveDate).ThenByDescending(le => le.EntryID);
			if (entries.Count() > 0)
			{
				StringBuilder builder = new StringBuilder();

				int count = 0;
				foreach (AccountLedger entry in entries.Skip(page * pageSize).Take(pageSize))
				{
					builder.Append("<tr").Append(count % 2 == 1 ? " class=\"Alt\"" : "").Append(">")
						.AppendCell(entry.EntryDescription)
						.AppendCell(entry.LedgerEntryReason.ReasonDescription)
						.AppendCell(entry.LedgerEntryType.TypeDescription)
						.AppendCell(entry.EffectiveDate.ToShortDateString())
						.AppendCell(entry.BonusType.BonusDesc)
						.AppendCell(entry.EntryAmount.ToString("C"))
						.AppendCell(entry.EndingBalance.ToDecimal().ToString("C"))
						.Append("</td></tr>");
					++count;
				}
				return Json(new { resultCount = entries.Count(), entries = builder.ToString() });
			}
			return Json(new { resultCount = 0, entries = "<tr><td colspan=\"7\">No ledger entries.</td></tr>" });
		}

		[FunctionFilter("Accounts-Add Ledger Entry", "~/Accounts/Ledger")]
		public ActionResult AddLedgerEntry(decimal entryAmount, DateTime effectiveDate, string entryDescription, int entryReason, int entryType, int bonusType, string notes, decimal currentEndingBalance)
		{
			try
			{
				AccountLedger newEntry = new AccountLedger()
				{
					EntryAmount = entryAmount,
					EffectiveDate = effectiveDate,
					EntryDescription = entryDescription,
					EntryDate = DateTime.Now,
					AccountID = CoreContext.CurrentAccount.AccountID,
					EntryReasonID = entryReason,
					EntryTypeID = entryType,
					BonusTypeID = bonusType,
					EntryNotes = notes
				};
				newEntry.Save();
				return Json(new { result = true, entryAmount = entryAmount.ToString("C"), endingBalance = (currentEndingBalance + entryAmount).ToString("C") });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}
		#endregion

		#region Disbursement Profiles
		[FunctionFilter("Accounts-Disbursement Profiles", "~/Accounts/Overview")]
		public ActionResult DisbursementProfiles(string id)
		{
			if (!string.IsNullOrEmpty(id))
			{
				CoreContext.CurrentAccount = Account.LoadByAccountNumber(id);
			}
			if (CoreContext.CurrentAccount == null)
				return RedirectToAction("Index");

			return View();
		}

		public enum PaymentPreferences
		{
			Check,
			EFT
		}

		[FunctionFilter("Accounts-Disbursement Profiles", "~/Accounts/Overview")]
		public ActionResult SaveDisbursementProfile(PaymentPreferences preference, bool? useAddressOfRecord, bool? isActive, string payableTo, string address1, string address2, string address3, string city, string state, string zip, bool? agreementOnFile, List<EFTAccount> accounts)
		{
			try
			{
				var profiles = DisbursementProfile.LoadByAccountID(CoreContext.CurrentAccount.AccountID);
				// Disable all profiles to start out with
				foreach (DisbursementProfile profile in profiles)
				{
					if (profile.CheckProfile != null && profile.CheckProfile.DisbursementProfileID > 0)
					{
						profile.CheckProfile.Enabled = false;
						profile.CheckProfile.Save();
					}
					if (profile.EFTProfile != null && profile.EFTProfile.DisbursementProfileID > 0)
					{
						profile.EFTProfile.Enabled = false;
						profile.EFTProfile.Save();
					}
				}

				// Consider changing this functionality (DeleteAll). It could quickly cause Identity keys to run out and isn't very efficient. - JHE
				// Delete all profiles and then recreate the 1 profile we need
				DisbursementProfile.DeleteByAccountID(CoreContext.CurrentAccount.AccountID);
				switch (preference)
				{
					case PaymentPreferences.Check:
						if (!useAddressOfRecord.Value)
						{
							CheckProfile checkProfile = CheckProfile.LoadByAccountID(CoreContext.CurrentAccount.AccountID).FirstOrDefault();

							DisbursementProfile disbursementProfile = new DisbursementProfile();
							disbursementProfile.AccountID = CoreContext.CurrentAccount.AccountID;
							disbursementProfile.DisbursementTypeID = (int)NetSteps.Data.Entities.Constants.DisbursementTypeEnum.Check;
							//disbursementProfile.Percentage = 1;
							disbursementProfile.Save();

							Address address = new Address();
							address.AddressTypeID = NetSteps.Data.Entities.Constants.AddressType.Disbursement.ToInt();
							address.Address1 = address1;
							address.Address2 = address2;
							address.Address3 = address3;
							address.City = city;
							address.State = state;
							address.PostalCode = zip;
							address.CountryID = (int)NetSteps.Data.Entities.Constants.Country.UnitedStates;
							CoreContext.CurrentAccount.Addresses.Add(address);
							CoreContext.CurrentAccount.Save();

							if (checkProfile == default(CheckProfile))
								checkProfile = new CheckProfile();
							checkProfile.DisbursementProfileID = disbursementProfile.DisbursementProfileID;
							checkProfile.Enabled = true;
							checkProfile.AccountID = CoreContext.CurrentAccount.AccountID;
							checkProfile.NameOnCheck = payableTo;
							checkProfile.Percentage = 1;
							checkProfile.AddressID = address.AddressID;
							checkProfile.Save();
						}
						break;
					case PaymentPreferences.EFT:
						var eftProfiles = EFTProfile.LoadByAccountID(CoreContext.CurrentAccount.AccountID);
						for (int i = 0; i < eftProfiles.Count; i++)
						{
							EFTProfile eftProfile = eftProfiles[i];
							EFTAccount account = accounts[i];

							DisbursementProfile disbursementProfile = new DisbursementProfile();
							disbursementProfile.AccountID = CoreContext.CurrentAccount.AccountID;
							disbursementProfile.DisbursementTypeID = (int)NetSteps.Data.Entities.Constants.DisbursementTypeEnum.EFT;
							//disbursementProfile.Percentage = account.PercentToDeposit / (decimal)100;
							disbursementProfile.Save();

							eftProfile.DisbursementProfileID = disbursementProfile.DisbursementProfileID;
							eftProfile.NameOnAccount = account.Name;
							eftProfile.RoutingNumber = account.RoutingNumber.ToString();
							eftProfile.BankAccountNumber = account.AccountNumber;
							eftProfile.BankName = account.BankName;
							eftProfile.BankPhone = account.BankPhone;
							eftProfile.BankAccountTypeID = (int)account.AccountType;
							eftProfile.Percentage = Convert.ToDecimal(account.PercentToDeposit / (double)100);
							eftProfile.EnrollmentFormReceived = agreementOnFile.HasValue ? agreementOnFile.Value : false;
							eftProfile.Enabled = true;
							eftProfile.Save();

							if (!string.IsNullOrEmpty(account.BankAddress1))
							{
								Address bankAddress = Address.Load(eftProfile.BankAddressID.ToInt());
								bankAddress.AddressTypeID = NetSteps.Data.Entities.Constants.AddressType.Disbursement.ToInt();
								bankAddress.Address1 = account.BankAddress1;
								bankAddress.Address2 = account.BankAddress2;
								bankAddress.Address3 = account.BankAddress3;
								bankAddress.City = account.BankCity;
								bankAddress.State = account.BankState;
								bankAddress.PostalCode = account.BankZip;
								bankAddress.CountryID = (int)NetSteps.Data.Entities.Constants.Country.UnitedStates;
								bankAddress.Save();
								CoreContext.CurrentAccount.Addresses.Add(bankAddress);
								CoreContext.CurrentAccount.Save();
							}
						}
						for (int i = eftProfiles.Count; i < accounts.Count; i++)
						{
							EFTAccount account = accounts[i];

							DisbursementProfile disbursementProfile = new DisbursementProfile();
							disbursementProfile.AccountID = CoreContext.CurrentAccount.AccountID;
							disbursementProfile.DisbursementTypeID = (int)NetSteps.Data.Entities.Constants.DisbursementTypeEnum.EFT;
							//disbursementProfile.Percentage = account.PercentToDeposit / (decimal)100;
							disbursementProfile.Save();

							Address bankAddress = new Address();
							if (!string.IsNullOrEmpty(account.BankAddress1))
							{
								bankAddress.AddressTypeID = NetSteps.Data.Entities.Constants.AddressType.Disbursement.ToInt();
								bankAddress.Address1 = account.BankAddress1;
								bankAddress.Address2 = account.BankAddress2;
								bankAddress.Address3 = account.BankAddress3;
								bankAddress.City = account.BankCity;
								bankAddress.State = account.BankState;
								bankAddress.PostalCode = account.BankZip;
								bankAddress.CountryID = (int)NetSteps.Data.Entities.Constants.Country.UnitedStates.ToInt();
								bankAddress.Save();
								CoreContext.CurrentAccount.Addresses.Add(bankAddress);
								CoreContext.CurrentAccount.Save();
							}

							EFTProfile eftProfile = new EFTProfile();
							eftProfile.DisbursementProfileID = disbursementProfile.DisbursementProfileID;
							eftProfile.NameOnAccount = account.Name;
							eftProfile.RoutingNumber = account.RoutingNumber.ToString();
							eftProfile.BankAccountNumber = account.AccountNumber;
							eftProfile.BankName = account.BankName;
							eftProfile.BankPhone = account.BankPhone;
							eftProfile.BankAddressID = bankAddress.AddressID;
							eftProfile.BankAccountTypeID = (int)account.AccountType;
							eftProfile.Percentage = Convert.ToDecimal(account.PercentToDeposit / (double)100);
							eftProfile.EnrollmentFormReceived = agreementOnFile.HasValue ? agreementOnFile.Value : false;
							eftProfile.Enabled = true;
							eftProfile.Save();
						}
						break;
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

		#region Full Order History
		[FunctionFilter("Accounts", "~/Accounts")]
		public ActionResult FullOrderHistory(string id)
		{
			if (!string.IsNullOrEmpty(id))
			{
				CoreContext.CurrentAccount = Account.LoadByAccountNumber(id);
			}
			if (CoreContext.CurrentAccount == null)
				return RedirectToAction("Index");
			return View();
		}

		public ActionResult GetFullOrderHistory(int page, int pageSize, int? status, int? type, DateTime? startDate, DateTime? endDate, string accountNumberOrName, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection, string orderNumber)
		{
			if (startDate.HasValue && startDate.Value.Year < 1900)
				startDate = null;
			if (endDate.HasValue && endDate.Value.Year < 1900)
				endDate = null;

			// TODO: Create a OrderSearchParameters object to pass into these search methods instead of having all these parameters - JHE
			//List<SearchOrderData> orders = Order.SearchOrders(page, pageSize, status, type, startDate, endDate, accountNumberOrName, orderBy, orderByDirection, orderNumber);
			var orders = Order.SearchOrders(new NetSteps.Data.Entities.Business.OrderSearchParameters()
			{
				PageIndex = page,
				PageSize = pageSize,
				OrderStatusID = status,
				OrderTypeID = type,
				StartDate = startDate,
				EndDate = endDate,
				AccountNumberOrName = accountNumberOrName,
				OrderNumber = orderNumber,
				OrderBy = orderBy,
				OrderByDirection = orderByDirection
			});
			if (orders.Count > 0)
			{
				StringBuilder builder = new StringBuilder();
				foreach (OrderSearchData order in orders)
				{
					builder.Append("<tr>")
						.AppendLinkCell("~/Order/OrderDetail/" + order.OrderID, order.OrderNumber)
						.AppendCell(order.FirstName)
						.AppendCell(order.LastName)
						.AppendCell(order.OrderStatus)
						.AppendCell(order.OrderType)
						.AppendCell(!order.CompleteDate.HasValue ? "N/A" : order.CompleteDate.ToShortDateString())
						.AppendCell(!order.DateShipped.HasValue ? "N/A" : order.DateShipped.ToShortDateString())
						.AppendCell(order.SubTotal.ToString("C"), style: "text-align: right; padding-right: 10px;")
						.AppendCell(order.GrandTotal.ToString("C"), style: "text-align: right; padding-right: 10px;")
						.AppendCell(!order.CommissionDate.HasValue ? "N/A" : order.CommissionDate.ToShortDateString())
						.AppendLinkCell("~/Accounts/Overview/Index" + order.SponsorAccountNumber, order.Sponsor)
						.Append("</tr>");
				}
				return Json(new { totalPages = orders.TotalPages, page = builder.ToString() });
			}

			return Json(new { totalPages = 0, page = "<tr><td colspan=\"9\">There were no records found that meet that criteria.  Please try again.</td></tr>" });
		}
		#endregion
	}
}
