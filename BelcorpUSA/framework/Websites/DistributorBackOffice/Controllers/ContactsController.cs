using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using DistributorBackOffice.Infrastructure;
using DistributorBackOffice.Models.Contacts;
using NetSteps.Common.Configuration;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Common.Serialization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Generated;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Encore.Core.IoC;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.ActionResults;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Helpers;

namespace DistributorBackOffice.Controllers
{
	public class ContactsController : AccountReportController<AccountSearchParameters>
	{
		#region Properties

		public override Constants.AccountReportType AccountReportType
		{
			get
			{
				return ConstantsGenerated.AccountReportType.ContactsReport;
			}
		}

		#endregion

		[OutputCache(CacheProfile = "DontCache")]
		[FunctionFilter("Contacts-New Report", "~/Contacts", Constants.SiteType.BackOffice)]
		public virtual ActionResult NewReport()
		{
			return RedirectToAction("Index", new { accountReportID = 0 });
		}

		[OutputCache(CacheProfile = "DontCache")]
		[FunctionFilter("Contacts", "~/", Constants.SiteType.BackOffice)]
		public virtual ActionResult Index(Constants.AccountType? accountType, int? accountReportID = null)
		{
			CurrentReportParameters = new AccountSearchParameters(); // Reset so filters don't persist when refreshing/coming back to the Contacts tab.
			ViewBag.SelectedReport = accountType.HasValue ? accountType.Value.ToString() : (accountReportID.HasValue ? accountReportID.Value.ToString() : "NA");

			try
			{
				// Reset report if 0 is passed in. - JHE
				if (accountReportID == 0)
				{
					CurrentReportParameters = GetDefaultReportParameters();
					return RedirectToAction("Index");
				}

				if (accountReportID != null && accountReportID > 0)
				{
					// TODO: Check that the report belongs to the current account if the not a 'base' report. - JHE
					var accountReport = CurrentAccountReports.FirstOrDefault(r => r.AccountReportID == accountReportID.ToInt());
					if (accountReport != null)
					{
						CurrentReportParameters = BinarySerializationHelper.Deserialize<AccountSearchParameters>(accountReport.Data);
						//Set the currentAccountID in case the savedReport does not have a currentAccountID or SponsorID set.
						CurrentReportParameters.CurrentAccountID = CurrentAccount.AccountID;

						ViewBag.CurrentAccountReport = CurrentAccountReports.FirstOrDefault(r => r.AccountReportID == accountReportID);
					}
				}

				SetMasterPageViewData();

				if (accountType.HasValue)
				{
					CurrentReportParameters.AccountTypes = new[] { (short)accountType.Value };
				}

				return View();
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
				throw exception;
			}
		}

		[OutputCache(CacheProfile = "PagedGridData")]
		[FunctionFilter("Contacts", "~/", ConstantsGenerated.SiteType.BackOffice)]
		public virtual ActionResult Get(string name, string email, Constants.AccountType? accountType, int? category, int? status, short? source, int page, int? pageSize, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection)
		{
			try
			{
				var builder = new StringBuilder();
				var searchParameters = GetAndSetParameters(name, email, accountType, category, status, source, page, pageSize, orderBy, orderByDirection);
                var accounts = Account.Search(searchParameters); 
				CurrentReportParameters = searchParameters;
				if (accounts.Count > 0)
				{
					int count = 0;
                    foreach (var account in accounts.Where(x => x.AccountStatusID != 3).ToList())//3: BegunEnrollment
					{
                        //CGI(CMR)-14/10/2014-Inicio
                        //var phone = String.Empty;
                        //int[] phoneTypes = new int[] { (int)Constants.PhoneType.Main, (int)Constants.PhoneType.Home, (int)Constants.PhoneType.Cell };
                        //var actPhone = AccountPhone.Repository.Where(ap => ap.AccountID == account.AccountID && !ap.IsPrivate && phoneTypes.Contains(ap.PhoneTypeID)).OrderByDescending(ap => ap.IsDefault).FirstOrDefault();
                        //if (actPhone != null)
                        //{
                        //    NetSteps.Data.Entities.Business.Logic.Interfaces.IAccountBusinessLogic logic = Create.New<NetSteps.Data.Entities.Business.Logic.Interfaces.IAccountBusinessLogic>();
                        //    phone = logic.DisplayPhone(actPhone.PhoneNumber, ApplicationContext.Instance.CurrentCultureInfo ?? ApplicationContext.Instance.ApplicationDefaultCultureInfo.Name);
                        //}
                        //CGI(CMR)-14/10/2014-Fin
						builder.Append("<tr class=\"").Append(count % 2 == 0 ? "GridRow" : "GridRowAlt").Append("\">");

						if (ConfigurationManager.GetAppSetting(ConfigurationManager.VariableKey.AllowExternalEmail, false))
						{
							builder.AppendCheckBoxCell(account.AccountID.ToString(), string.Empty, account.AccountID.ToString(), "contactAccount");
						}

						builder
							.AppendCell(SmallCollectionCache.Instance.AccountTypes.GetById(account.AccountTypeID).GetTerm())
							.AppendCell(account.AccountNumber)
							.AppendLinkCell("~/Contacts/View/" + account.AccountNumber, account.FirstName)
							.AppendCell(account.LastName)
                            //.AppendCell(SmallCollectionCache.Instance.AccountStatuses.GetById(account.AccountStatusID).GetTerm()) //CGI(CMR)-23/10/2014
							.AppendCell(account.IsOptedOut ? string.Empty : account.EmailAddress)
							.AppendCell(
								account.IsOptedOut
								? Translation.GetTerm("AccountHasOptedOut", "Account has opted-out")
								: account.Location)
                             //.AppendCell(phone) //CGI(CMR)-15/10/2014 
                            .AppendCell(account.DateEnrolled.ToString("MM/dd/yyyy")) ////CGI(CMR)-23/10/2014, se agrego MM/dd/yyyy
							.Append("</tr>");
						++count;
					}
					return Json(new { result = true, totalPages = accounts.TotalPages, page = builder.ToString() });
				}

				return Json(new { result = true, totalPages = 0, page = "<tr><td colspan=\"9\">" + Translation.GetTerm("ThereWereNoRecordsFoundThatMeetThatCriteriaPleaseTryAgain", "There were no records found that meet that criteria. Please try again.") + "</td></tr>" });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[OutputCache(CacheProfile = "DontCache")]
		[FunctionFilter("Contacts", "~/", Constants.SiteType.BackOffice)]
		public virtual ActionResult GetOverview(int page, int? pageSize, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection)
		{
			try
			{
				var builder = new StringBuilder();
				var searchParameters = GetAndSetParameters(null, null, null, null, null, null, page, pageSize, orderBy, orderByDirection);
				var accounts = Account.Search(searchParameters);
				CurrentReportParameters = searchParameters;

				if (accounts.Count > 0)
				{
					builder.Append("<ul class=\"lr listNav\">");
					foreach (var account in accounts)
					{
						builder.Append(string.Format("<li><a href=\"{0}\"><span class=\"FL\">{1}</span> <span class=\"lawyer FR\">{2}</span><span class=\"clr\"></span></a></li>", "~/Contacts/View/".ResolveUrl() + account.AccountNumber, account.FullName, account.DateCreated.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo)));
					}
					builder.Append("</ul>");
					return Json(new { result = true, data = builder.ToString() });
				}

				return Json(new { result = true, totalPages = 0, page = "<tr><td colspan=\"9\">" + Translation.GetTerm("ThereWereNoRecordsFoundThatMeetThatCriteriaPleaseTryAgain", "There were no records found that meet that criteria. Please try again.") + "</td></tr>" });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[FunctionFilter("Contacts-Edit Contact", "~/Contacts", Constants.SiteType.BackOffice)]
		public virtual ActionResult Edit(string id)
		{
			try
			{
				SetMasterPageViewData();
				var contact = Account.LoadByAccountNumberFull(id);
				if (contact == null || contact.SponsorID != CurrentAccount.AccountID || contact.AccountTypeID != Constants.AccountType.Prospect.ToInt())
				{
					return RedirectToAction("Index");
				}

				return View(contact);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		[ActionName("View")]
		[FunctionFilter("Contacts", "~/", Constants.SiteType.BackOffice)]
		public virtual ActionResult ViewContact(string id)
		{
			try
			{
				SetMasterPageViewData();

				Account contact = Account.LoadByAccountNumberFull(id);

				if (contact == null || (contact.SponsorID != CurrentAccount.AccountID && contact.AccountID != CurrentAccount.SponsorID))
				{
					return RedirectToAction("Index");
				}

				var mostRecentOrders = Order.Search(new OrderSearchParameters
				{
					CustomerAccountID = contact.AccountID,
					PageSize = 1,
					OrderBy = "CompleteDateUTC",
					OrderByDirection = NetSteps.Common.Constants.SortDirection.Descending
				});

				var cachedDistributionList = DistributionListCacheHelper.GetDistributionListByAccountID(CurrentAccountId).ToList();
				List<DistributionList> groups;
				if (cachedDistributionList.Any())
				{
					groups = cachedDistributionList;
				}
				else
				{
					groups = new List<DistributionList>();
				}

				return View(new ViewContactModel
				{
					Account = contact,
					MostRecentOrder = mostRecentOrders.FirstOrDefault(),
					DistributionLists = groups,
					isDistributor = contact.AccountTypeID == (int)Constants.AccountType.Distributor,
					isProspect = contact.AccountTypeID == (int)Constants.AccountType.Prospect,
					MainAddress = contact.Addresses.GetDefaultByTypeID(ConstantsGenerated.AddressType.Main),
					AccountEmailAddress = CachedData.GetAccountEmailAddress(contact.AccountID, contact)
				});
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		[FunctionFilter("Contacts", "~/", Constants.SiteType.BackOffice)]
		public virtual ActionResult GetPurchases(int accountId, int page, int pageSize, string orderBy, Constants.SortDirection orderByDirection)
		{
			try
			{
				var orders = Order.Search(new OrderSearchParameters
				{
					CustomerAccountID = accountId,
					PageIndex = page,
					PageSize = pageSize,
					OrderBy = orderBy,
					OrderByDirection = orderByDirection
				});

				var builder = new StringBuilder();
				int count = 0;
				foreach (var order in orders)
				{
					builder.Append("<tr").Append(count % 2 == 1 ? " class=\"Alt\"" : "").Append(">")
						.AppendLinkCell("~/Orders/Details/Index/" + order.OrderNumber, order.CompleteDate.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo))
						.AppendCell(SmallCollectionCache.Instance.OrderTypes.GetById(order.OrderTypeID).GetTerm())
						.AppendCell(order.CustomerTotal.ToString(order.CurrencyID))
						.Append("</tr>");
					++count;
				}

				return Json(new { result = true, totalPages = orders.TotalPages, page = builder.ToString() });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[FunctionFilter("Contacts-Create Contact", "~/Contacts", Constants.SiteType.BackOffice)]
		public virtual ActionResult New()
		{
			SetMasterPageViewData();
			return View("Edit", new Account());
		}

		[FunctionFilter("Contacts-Delete Contact", "~/Contacts", Constants.SiteType.BackOffice)]
		public virtual ActionResult Delete(int? accountId)
		{
			try
			{
				if (accountId.HasValue && accountId != 0)
				{
					Account account = Account.LoadFull(accountId.Value);

					// Verify that the contact can be deleted (is a contact of the currently logged in Account, ect..) - JHE
					if (account.AccountTypeID == Constants.AccountType.Prospect.ToInt() && account.SponsorID == CurrentAccount.AccountID)
					{
						account.Delete();
						return Json(new { result = true });
					}

					return Json(new { result = false, message = Translation.GetTerm("ThisAccountCanNotBeDeleted", "This account can not be deleted.") });
				}

				return Json(new { result = false, message = Translation.GetTerm("ErrorDeletingAccount", "Error deleting Account.") });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException, accountID: accountId);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[FunctionFilter("Contacts-Edit Contact", "~/Contacts", Constants.SiteType.BackOffice)]
		public virtual ActionResult Save(int? accountId, string firstName, string middleName, string lastName, DateTime? dob, short? gender,
			string source, int? category, int? status, int? type,
			int? communicationPreference, string homePhone, string cellPhone, string workPhone, string email,
			string address1, string address2, string address3, string postalCode, string city, string county, string state, int country,
			Dictionary<int, bool> newsletterValues)
		{
			try
			{
				Account account;
				if (accountId.HasValue && accountId != 0)
				{
					account = Account.LoadFull(accountId.Value);

					NewsLetterSubscription(newsletterValues, account);
				}
				else
				{
					account = new Account();
					account.StartEntityTracking();
					account.AccountTypeID = (int)Constants.AccountType.Prospect;
					//set the account status to active and set the enrollment date to now - Scott Wilson
					account.Activate();
					account.SponsorID = CurrentAccount.AccountID;
					account.MarketID = CurrentAccount.MarketID;
					account.DateCreated = DateTime.Now;
					account.DefaultLanguageID = CurrentAccount.DefaultLanguageID;
					account.AccountSourceID = (int)ConstantsGenerated.AccountSource.ManuallyEntered;
				}

				account.FirstName = firstName;
				account.MiddleName = middleName;
				account.LastName = lastName;
				account.Birthday = dob;
				account.GenderID = gender;

				var contactTag = account.AccountContactTag;
				if (contactTag == null)
				{
					contactTag = new AccountContactTag();
					contactTag.StartEntityTracking();
					account.AccountContactTag = contactTag;
				}

				contactTag.Source = source;
				contactTag.ContactCategoryID = category;
				contactTag.ContactStatusID = status;

				account.PreferedContactMethodID = communicationPreference;
				account.HomePhone = homePhone;
				account.CellPhone = cellPhone;
				account.WorkPhone = workPhone;
				account.EmailAddress = email;

				if (!string.IsNullOrEmpty(address1))
				{
					Address main = account.Addresses.GetDefaultByTypeID(Constants.AddressType.Main);
					if (main == default(Address))
					{
						main = new Address();
						main.StartEntityTracking();
						main.AddressTypeID = (int)Constants.AddressType.Main;
						account.Addresses.Add(main);
					}

					main.Address1 = address1;
					main.Address2 = address2;
					main.Address3 = address3;
					main.PostalCode = postalCode;
					main.City = city;
					main.County = county;
					main.IsDefault = true;
					main.SetState(state, country);

					main.CountryID = country;
				}

				account.Save();

				if (accountId.HasValue && accountId.Value == 0)
				{
					//
					// Is there a reason we are doing this?!?  When adding a contact, we load all the
					// newletters and subscribe the new user to them, but there isn't a location
					// on the page to unsubscribe yourself.  This also causes major slowness!!!
					//
					NewsLetterSubscription(newsletterValues, account);
					account.Save();
				}

				return Json(new { result = true, accountNumber = account.AccountNumber });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		private List<Campaign> _newsletters;
		protected virtual List<Campaign> GetActiveNewsletters()
		{
			return _newsletters ?? (_newsletters = Create.New<ICampaignRepository>().Where(c => c.Active && c.CampaignTypeID == (int)Constants.CampaignType.Newsletters).ToList());
		}

		public virtual void NewsLetterSubscription(Dictionary<int, bool> newValues, Account account)
		{
			if (newValues == null)
				return;

			foreach (var campaign in GetActiveNewsletters())
			{
				var campaignSubscribed = account.CampaignSubscribers.FirstOrDefault(x => x.CampaignID == campaign.CampaignID);
				if (campaignSubscribed != null)
				{
					bool newsletterExists = account.CampaignSubscribers.Any(x => x.CampaignID == campaign.CampaignID);

					if (newValues.ContainsKey(campaign.CampaignID) && !newValues[campaign.CampaignID])
					{
						// User unchecked a previously checked value. Delete from CampaingSubscribers table
						if (newsletterExists)
						{
							CampaignSubscriber.Delete(campaignSubscribed.CampaignSubscriberID);
						}
					}
				}
				else
				{
					if (newValues.ContainsKey(campaign.CampaignID) && newValues[campaign.CampaignID])
					{
						var newSubscription = new CampaignSubscriber
						{
							CampaignID = campaign.CampaignID,
							AccountID = account.AccountID,
							AddedByAccountID = CurrentAccount.AccountID,
							DateAddedUTC = DateTime.UtcNow
						};
						account.CampaignSubscribers.Add(newSubscription);
					}
				}
			}
		}

		[OutputCache(CacheProfile = "DontCache")]
		[FunctionFilter("Contacts-Export Excel", "~/Contacts", Constants.SiteType.BackOffice)]
		public virtual ActionResult ExportExcel()
		{
			try
			{
				var searchParameters = CurrentReportParameters;
				searchParameters.PageIndex = 0;
				searchParameters.PageSize = null;

				string fileNameSave = string.Format("{0}_{1}.xlsx", Translation.GetTerm("ContactsExport", "Contacts Export"), DateTime.Now.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo));

				var results = Account.Search(searchParameters);

				// TODO: Make these exported columns pull dynamically from user specified 'visible columns' later when functionality is available to users - JHE
				var columns = new Dictionary<string, string>
				{
					{"AccountType", Translation.GetTerm("Type")},
					{"FirstName", Translation.GetTerm("FirstName", "First Name")},
					{"LastName", Translation.GetTerm("LastName", "Last Name")},
					{"AccountStatus", Translation.GetTerm("Status", "Status")},
					{"EmailAddress", Translation.GetTerm("Email", "Email")},
					{"Location", Translation.GetTerm("CityState", "City, State")}
				};

				return new ExcelResult<AccountSearchData>(fileNameSave, results, columns, null, columns.Keys.ToArray());
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
			}
		}

		[HttpGet]
		[FunctionFilter("Contacts-Import Outlook Contacts", "~/Contacts", Constants.SiteType.BackOffice)]
		public virtual ActionResult ImportOutlookContacts()
		{
			SetMasterPageViewData();
			return View();
		}

		[HttpPost]
		[FunctionFilter("Contacts-Import Outlook Contacts", "~/Contacts", Constants.SiteType.BackOffice)]
		public virtual ActionResult ImportOutlookContacts(HttpPostedFileBase file)
		{
			try
			{
				SetMasterPageViewData();

				if (file == null && Request.Files.Count > 0)
				{
					file = Request.Files[0];
				}

				if (file == null)
				{
					TempData["Error"] = Translation.GetTerm("NoFileUploaded", "No file uploaded.");
					return View();
				}

				if (!file.FileName.EndsWith(".csv", StringComparison.InvariantCultureIgnoreCase))
				{
					TempData["Error"] = Translation.GetTerm("InvalidFileTypeCSV", "Invalid file type.  Please upload a CSV file.");
					return View();
				}

				string fileContents = file.GetFileContents();
				var accounts = Account.ImportProspects(fileContents);

				foreach (var account in accounts)
				{
					account.AccountTypeID = (int)Constants.AccountType.Prospect;
					//set the account status to active and set the enrollment date to now - Scott Wilson
					account.Activate();
					account.SponsorID = CurrentAccount.AccountID;
					account.MarketID = CoreContext.CurrentMarketId;
					account.DateCreated = DateTime.Now;
					account.DefaultLanguageID = CurrentAccount.DefaultLanguageID;

					if (string.IsNullOrEmpty(account.LastName))
					{
						account.LastName = Translation.GetTerm("Unknown", "Unknown");
					}

					if (string.IsNullOrEmpty(account.FirstName))
					{
						account.FirstName = Translation.GetTerm("Unknown", "Unknown");
					}

					account.ValidateRecursive();
					account.Save();
				}

				return View(accounts);
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
				TempData["Error"] = exception.PublicMessage;
				SetMasterPageViewData();
				return View();
			}
		}

		#region Helper Methods

		protected virtual List<StaticAccountReport> GetDefaultAccountReports()
		{
			var acctTypes = SmallCollectionCache.Instance.AccountTypes.Where(at => at.Active && ((at.AccountTypeID == Constants.AccountType.RetailCustomer.ToShort())
																							|| (at.AccountTypeID == Constants.AccountType.PreferredCustomer.ToShort())
																							|| (at.AccountTypeID == Constants.AccountType.Prospect.ToShort())));

			var reportList = new List<StaticAccountReport>();
			foreach (AccountType acctType in acctTypes)
			{
				reportList.Add(new StaticAccountReport { AccountType = acctType.TermName, Term = acctType.TermName, TermDefault = acctType.Name.PascalToSpaced() });
			}

			return reportList;
		}
		protected virtual void SetMasterPageViewData()
		{
			ViewData["CurrentAccountReports"] = CurrentAccountReports.Where(r => r.AccountReportTypeID == Constants.AccountReportType.ContactsReport.ToShort()).ToList();
			ViewData["CurrentReportParameters"] = CurrentReportParameters;
			ViewData["DefaultAccountReports"] = GetDefaultAccountReports();
			ViewBag.Newsletters = GetActiveNewsletters();
		}

		protected virtual AccountSearchParameters GetAndSetParameters(string name, string email, Constants.AccountType? accountType, int? category, int? status, short? source, int page, int? pageSize, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection)
		{
			var searchParameters = new AccountSearchParameters
			{
				PageIndex = page,
				PageSize = pageSize,
				OrderBy = orderBy,
				OrderByDirection = orderByDirection,
				Name = name,
				Email = email,
				SponsorID = CurrentAccount.AccountID,
				CurrentAccountID = CurrentAccount.AccountID,
				ContactCategoryID = category,
				AccountSourceID = source,
				AccountTypes = GetSearchableAccountTypes(accountType),
				ExcludedAccountStatuses = new[]
                {
                    (short)Constants.AccountStatus.BegunEnrollment
				}
			};

			if (status != null && status > 0)
			{
				searchParameters.AccountStatusID = (short)status;
			}

			return searchParameters;
		}

		protected virtual short[] GetSearchableAccountTypes(Constants.AccountType? accountType)
		{
			if (accountType.HasValue)
			{
				return new[] { (short)accountType.Value };
			}

			return SmallCollectionCache.Instance.AccountTypes.Where(x => x.Active && x.AccountTypeID != (int)Constants.AccountType.Employee).Select(y => y.AccountTypeID).ToArray();
		}

		#endregion

		#region AccountListValues
		[OutputCache(CacheProfile = "DontCache")]
		[FunctionFilter("Accounts-Account List Values", "~/Contacts", Constants.SiteType.BackOffice)]
		public virtual ActionResult GetAccountListValues(short type)
		{
			var list = new List<object>();
			foreach (var accountListValue in CoreContext.GetCurrentAccountListValuesByType(type))
			{
				list.Add(new
				{
					value = accountListValue.AccountListValueID,
					name = accountListValue.IsCorporate ? accountListValue.GetTerm() : accountListValue.Value
				});
			}
			return Json(new { result = true, items = list });
		}

		[OutputCache(CacheProfile = "DontCache")]
		[FunctionFilter("Accounts-Edit Account List Values", "~/Contacts", Constants.SiteType.BackOffice)]
		public virtual ActionResult EditAccountListValues(short listValueTypeId)
		{
			ViewBag.ListValueTypeID = listValueTypeId;
			ViewBag.ListValueType = SmallCollectionCache.Instance.ListValueTypes.GetById(listValueTypeId).GetTerm();
			return PartialView(AccountListValue.LoadListValuesByTypeAndAccountID(CurrentAccount.AccountID, listValueTypeId));
		}

		[FunctionFilter("Accounts-Delete Account List Values", "~/Contacts", Constants.SiteType.BackOffice)]
		public virtual ActionResult DeleteValue(short type, int listValueId)
		{
			try
			{
				// If new/unsaved item - JHE
				if (listValueId == 0)
				{
					return Json(new { result = true });
				}

				var currentAccountListValues = AccountListValue.LoadListValuesByTypeAndAccountID(CurrentAccount.AccountID, type);
				var accountListValue = currentAccountListValues.FirstOrDefault(a => a.AccountListValueID == listValueId);
				if (accountListValue != null)
				{
					if (accountListValue.IsCorporate)
					{
						// Duplicate list and save on Account not Corporate - JHE
						var newList = AccountListValue.CloneListForAccountOverrides(CurrentAccount.AccountID, type);
						newList.RemoveAll(i => i.Value.ToCleanString().ToLower() == accountListValue.Value.ToCleanString().ToLower());

						AccountListValue.SaveBatch(newList);
						Session["CurrentAccountListValues"] = null;
					}
					else if (accountListValue.AccountID == CurrentAccount.AccountID && !accountListValue.IsCorporate)
					{
						accountListValue.Delete();
						Session["CurrentAccountListValues"] = null;
					}
				}

				return Json(new { result = true, items = GetAccountListValuesHtml(CoreContext.GetCurrentAccountListValuesByType(type)) });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);

				if (exception.Message.ContainsIgnoreCase("This object is currently referenced by another object and cannot be deleted."))
					exception.PublicMessage = Translation.GetTerm("ThisListValueCanNotBeDeletedItsisCurrentlyBeingUsedByOneOrMoreOfYourContacts", "This list value can not be deleted. Its is currently being used by one or more of your Contacts.");

				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[FunctionFilter("Accounts-Edit Account List Values", "~/Contacts", Constants.SiteType.BackOffice)]
		public virtual ActionResult SaveValues(short type, List<AccountListValue> listValues)
		{
			try
			{
				// Clean values first - JHE
				foreach (var value in listValues)
				{
					value.Value = value.Value.ToCleanString();
				}

				var listToSave = new List<AccountListValue>();
				var currentAccountListValues = AccountListValue.LoadListValuesByTypeAndAccountID(CurrentAccount.AccountID, type);
				//bool isUsingCorpDefaults = currentAccountListValues.Any(a => a.IsCorporate);
				bool containsNewValues = listValues.Any(lv => lv.AccountListValueID == 0);

				bool listItemChanged = false;
				if (containsNewValues)
				{
					listItemChanged = true;
				}
				else
				{
					foreach (var listValue in listValues)
					{
						var accountListValue = currentAccountListValues.FirstOrDefault(a => a.AccountListValueID == listValue.AccountListValueID);
						if (accountListValue != null && accountListValue.Value.ToCleanString().ToLower() != listValue.Value.ToLower())
						{
							listItemChanged = true;
							break;
						}
					}
				}

				foreach (var listValue in listValues)
				{
					if (listValue.AccountListValueID == 0)
					{
						var newAccountListValue = new AccountListValue(CurrentAccount.AccountID, type, listValue.Value);
						listToSave.Add(newAccountListValue);
					}
					else
					{
						AccountListValue changedItem = null;
						var accountListValue = currentAccountListValues.FirstOrDefault(a => a.AccountListValueID == listValue.AccountListValueID);
						if (accountListValue != null)
						{
							bool valueChanged = accountListValue.Value.ToCleanString().ToLower() != listValue.Value.ToLower();
							if (valueChanged)
							{
								if (accountListValue.IsCorporate)
								{
									changedItem = new AccountListValue(CurrentAccount.AccountID, type, listValue.Value);
								}
								else
								{
									changedItem = accountListValue;
									changedItem.Value = listValue.Value;
								}
							}
							else
							{
								if (accountListValue.IsCorporate && listItemChanged)
								{
									changedItem = new AccountListValue(CurrentAccount.AccountID, type, listValue.Value);
								}
							}
							if (changedItem != null)
							{
								listToSave.Add(changedItem);
							}
						}
					}
				}

				if (listToSave.Count > 0)
				{
					AccountListValue.SaveBatch(listToSave);
					Session["CurrentAccountListValues"] = null;
				}

				return Json(new { result = true, items = GetAccountListValuesHtml(CoreContext.GetCurrentAccountListValuesByType(type)) });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		#region Helper Methods

		protected virtual string GetAccountListValuesHtml(List<AccountListValue> items)
		{
			var itemHtml = new StringBuilder();
			foreach (var item in items)
			{
				itemHtml.Append(GetAccountListValueHtml(item));
			}

			return itemHtml.ToString();
		}

		protected virtual object GetAccountListValueHtml(AccountListValue accountListValue)
		{
			return string.Format("<li class=\"pad5 icon-24\"><div class=\"FL\"><input type=\"text\" name=\"listValue{0}\" value=\"{1}\" class=\"pad5 listValue\" /></div><a href=\"javascript:void(0);\" class=\"FR IconLink Delete\" title=\"{2}\"><span class=\"UI-icon icon-x\"></span></a><span class=\"clr\"></span></li>",
				accountListValue.AccountListValueID,
				accountListValue.IsCorporate ? accountListValue.GetTerm() : accountListValue.Value,
				Translation.GetTerm("Delete"));
		}

		#endregion

		#endregion

		#region Email Checked

		[OutputCache(CacheProfile = "DontCache")]
		[FunctionFilter("Performance-Email Downline", "~/Performance", Constants.SiteType.BackOffice)]
		public virtual ActionResult ContactsEmail(List<int> accountIDs)
		{
			JsonResult result;

			try
			{
				if (accountIDs != null && accountIDs.Count > 0)
				{
					TempData["EmailDownline"] = accountIDs;

					result = Json(new { result = true });
				}
				else
				{
					result = Json(new { result = false, message = Translation.GetTerm("No_Customers_Selected", "No customers selected.") });
				}

			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
				result = Json(new { result = false, message = exception.PublicMessage });
			}

			return result;
		}

		#endregion
	}
}