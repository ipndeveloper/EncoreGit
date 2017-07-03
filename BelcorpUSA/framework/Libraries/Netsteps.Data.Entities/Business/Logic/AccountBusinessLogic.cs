using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using NetSteps.Common.Base;
using NetSteps.Common.Exceptions;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Common.Utility;
using NetSteps.Common.Validation.NetTiers;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Generated;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Enrollment.Common.Models.Context;

namespace NetSteps.Data.Entities.Business.Logic
{
	
    public partial class AccountBusinessLogic
	{
		public override Func<Account, string> GetTitleColumnFunc
		{
			get
			{
				return i => i.FullName;
			}
		}

		public override List<string> ValidatedChildPropertiesSetByParent(Repositories.IAccountRepository repository)
		{
			List<string> list = new List<string>() { "AccountID", "AddressID", "FileResourceID" };
			return list;
		}

		public override void DefaultValues(IAccountRepository repository, Account entity)
		{
			entity.AccountNumber = string.Empty;
		}

		public override void AddValidationRules(Account account)
		{
			// Add additional base validation rules as needed here - JHE
			account.ValidationRules.AddRule(CommonRules.StringRequired, "FirstName");
			account.ValidationRules.AddRule(CommonRules.StringRequired, "LastName");
			account.ValidationRules.AddRule(CommonRules.GreaterThanValue<short?>, new CommonRules.CompareValueRuleArgs<short?>("GenderID", 0));
		}

		public override void CleanDataBeforeSave(IAccountRepository repository, Account entity)
		{
			if (entity != null)
			{
				if (entity.Addresses != null)
					foreach (var address in entity.Addresses)
						address.Trim();

				if (entity.AccountPaymentMethods != null)
				{
					foreach (var accountPaymentMethod in entity.AccountPaymentMethods)
						if (accountPaymentMethod.BillingAddress != null)
							accountPaymentMethod.Trim();
				}

				if (entity.AccountPhones != null)
				{
					foreach (var accountPhone in entity.AccountPhones)
						if (accountPhone.PhoneNumber != null)
							accountPhone.PhoneNumber = accountPhone.PhoneNumber.RemoveNonNumericCharacters();
				}

				if (!entity.DecryptedTaxNumber.IsNullOrEmpty())
					entity.DecryptedTaxNumber = entity.DecryptedTaxNumber.ToCleanString().Replace("-", string.Empty);

				// Make sure that accounts don't get saved null or empty (because we have a unique index on that column) - JHE
				if (entity.AccountNumber.IsNullOrEmpty())
					entity.AccountNumber = "Temp" + Guid.NewGuid().ToString("N");
			}
		}

		/// <summary>
		/// Sets the AccountNumber property for new accounts according to the numbering pattern defined by the client.
		/// Just setting it to the same number as the AccountID by default. - JHE
		/// </summary>
		/// <param name="order"></param>
		public virtual void GenerateAndSetNewAccountNumber(Account account, bool saveAccount = true)
		{
			//logic enforces that in order to save an account the account number cannot be empty
			//if it is when a save happens, then logic inserts a temporary account number
			//if the account number is temporary or empty then set it to our default (AccountID) - Scott Wilson
			if (account.AccountNumber.IsNullOrEmpty() || account.AccountNumber.ToCleanString().ToLower().StartsWith("Temp".ToLower()))
			{
				if (account.AccountID != 0)
					account.AccountNumber = account.AccountID.ToString();
			}
		}

		public virtual void SendGeneratedPasswordEmail(Account account)
		{
			// TODO: Send email with generated password. - JHE
		}

		public virtual string GetIsCommissionQualifiedStatus(bool? isCommissionQualifed, DateTime? autoshipProcessDate)
		{
			if (isCommissionQualifed == true)
			{
				return "Qualified";
			}
			if (isCommissionQualifed != true && autoshipProcessDate.HasValue)
			{
				DateTime today = DateTime.UtcNow.Date;
				int numberOfDays = DateTime.DaysInMonth(today.Year, today.Month);
				if (autoshipProcessDate.Value.Date > today && autoshipProcessDate <= new DateTime(today.Year, today.Month, numberOfDays))
				{
					return "Pending";
				}
			}
			return "UnQualified";
		}

		public override Account LoadFull(IAccountRepository repository, int primaryKey)
		{
			var account = base.LoadFull(repository, primaryKey);
			account.StartEntityTracking();
			account.IsLazyLoadingEnabled = true;

			BuildReadOnlyNotesTree(account);

			return account;
		}

		public Account LoadInfoCard(IAccountRepository repository, int primaryKey)
		{
			return repository.LoadInfoCard(primaryKey);
		}

		public IEnumerable<Account> LoadInfoCardBatch(IAccountRepository repository, List<int> primaryKeys)
		{
			return repository.LoadInfoCardBatch(primaryKeys);
		}

		public virtual Account LoadByAccountNumberFull(IAccountRepository repository, string accountNumber)
		{
			try
			{
				var account = repository.LoadByAccountNumberFull(accountNumber);
				account.StartEntityTracking();
				account.IsLazyLoadingEnabled = true;

				BuildReadOnlyNotesTree(account);

				return account;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public virtual Account LoadByAccountNumber(IAccountRepository repository, string accountNumber)
		{
			try
			{
				var account = repository.LoadByAccountNumber(accountNumber);
				account.StartEntityTracking();
				account.IsLazyLoadingEnabled = true;

				return account;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public virtual Account LoadAccountByUserId(IAccountRepository repository, int userId)
		{
			try
			{
				return repository.LoadAccountByUserId(userId);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public virtual void BuildReadOnlyNotesTree(Account account)
		{
			// Hookup read-only tree of notes - JHE
			foreach (var note in account.Notes.ToList())
				note.FollowupNotes = account.Notes.Where(n => n.ParentID == note.NoteID).ToList();
		}

		public virtual PaginatedList<AccountSearchData> Search(IAccountRepository repository, AccountSearchParameters searchParameters)
		{
			try
			{
				return repository.Search(searchParameters);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public virtual Dictionary<int, string> SlimSearchOnAccountStatuses(IAccountRepository repository, string query, int? accountTypeID = null, int?[] statusIDs = null, int? sponsorID = null)
		{
			try
			{
				return repository.SlimSearchOnAccountStatuses(query, accountTypeID, statusIDs, sponsorID);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public virtual Dictionary<int, string> SlimSearchEmail(IAccountRepository repository, string query, int? statusID = null, int? sponsorID = null)
		{
			try
			{
				return repository.SlimSearchEmail(query, statusID, sponsorID);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public virtual AccountSlimSearchData LoadSlim(IAccountRepository repository, int accountID)
		{
			try
			{
				return repository.LoadSlim(accountID);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public virtual AccountSlimSearchData LoadSlimByAccountNumber(IAccountRepository repository, string accountNumber)
		{
			try
			{
				return repository.LoadSlimByAccountNumber(accountNumber);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public virtual Account Authenticate(IAccountRepository repository, string username, string password)
		{
			try
			{
				//var user = User.Authenticate(username, password);
				//var account = repository.LoadByUserIdFull(user.UserID);
				var account = repository.Authenticate(username, password);
				if (account != null)
				{
					account.StartEntityTracking();
					account.IsLazyLoadingEnabled = true;
					account.User.IsLazyLoadingEnabled = true;
				}
				return account;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public virtual IPaginatedList<IAccountLocatorSearchData> AccountLocatorSearch(
			 IAccountRepository repository,
			 IAccountLocatorSearchParameters searchParameters)
		{
			var accountData = repository.AccountLocatorAccountSearch(
				 searchParameters
			);

			var accountIDs = accountData
				 .Select(x => x.AccountID)
				 .ToArray();

			var contentData = repository.GetAccountLocatorContent(
				 accountIDs
			);

			// accountData has already been sorted. This join should keep the same order.
			var resultData = from a in accountData
							 join c in contentData on a.AccountID equals c.AccountID into outer
							 from c in outer.DefaultIfEmpty()
							 select new AccountLocatorSearchData
							 {
								 AccountID = a.AccountID,
								 FirstName = a.FirstName,
								 LastName = a.LastName,
								 City = a.City,
								 State = a.State,
								 CountryID = a.CountryID,
								 Distance = a.Distance,
								 DistanceType = searchParameters.DistanceType,
								 PwsUrl = c == null ? "" : c.PwsUrl,
								 PhotoContent = c == null ? new HtmlString(string.Empty) : c.PhotoContent,
                                 EmailAddress=a.EmailAddress,
                                 PhoneNumber=a.PhoneNumber
							 };

			return resultData
				.Cast<IAccountLocatorSearchData>()
				.ToPaginatedList(
					searchParameters,
					  accountData.TotalCount
				 );
		}

		public virtual List<AccountSearchData> LoadBatchHeaders(IAccountRepository repository, List<int> accountIDs)
		{
			try
			{
				return repository.LoadBatchHeaders(accountIDs);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public virtual PaginatedList<AuditLogRow> GetAuditLog(IAccountRepository repository, Account fullyLoadedAccount, AuditLogSearchParameters param)
		{
			try
			{
				return repository.GetAuditLog(fullyLoadedAccount, param);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException, null, (fullyLoadedAccount != null) ? fullyLoadedAccount.AccountID : fullyLoadedAccount.AccountID.ToIntNullable());
			}
		}

		public Account EnrollRetailCustomer(IAccountRepository repository, int sponsorID, int languageID, string firstName, string lastName, string email, string username, string password, short accountTypeID)
		{
			try
			{
				Account account;
				if (accountTypeID == (short)Constants.AccountType.RetailCustomer || accountTypeID == (short)Constants.AccountType.Distributor)
					account = repository.LoadAccountByEmailAndAccountType(email, Constants.AccountType.RetailCustomer);
				else
					account = repository.LoadAccountByEmailAndAccountType(email, Constants.AccountType.PreferredCustomer);

				var updateExistingAccount = Account.ExistingAccountCanBeUpdated(account);

				if (updateExistingAccount)
					return UpdateExistingRetailCustomer(account, sponsorID, languageID, username, password, firstName, lastName);

				if (Account.NonProspectNonExpressAccountExists(email) && (account == null || account.User != null))
				{
					throw new NetSteps.Common.Exceptions.NetStepsBusinessException("This email is already enrolled.")
					{
						PublicMessage = Translation.GetTerm("EmailAlreadyEnrolled", "This email is already enrolled.")
					};
				}

				if (account == null)
					account = Account.GetAccountByEmailAndSponsorID(email, sponsorID, true, true);

				if (account == null)
				{
					account = new Account { DateCreated = DateTime.Now };
				}

				BaseEnrollInformation(account, repository, sponsorID, languageID, firstName, lastName, email, username, password, (short)Constants.AccountType.RetailCustomer, 0);

				return account;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public Account EnrollExpressRetailCustomer(IAccountRepository repository, int sponsorID, int languageID, string firstName, string lastName, string email, string username, short accountTypeID)
		{
			try
			{
				var account = new Account { DateCreated = DateTime.Now };

				BaseEnrollInformation(account, repository, sponsorID, languageID, firstName, lastName, email, username, string.Empty, accountTypeID, 0);

				return account;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		protected virtual Account UpdateExistingRetailCustomer(Account existingAccount, int sponsorID, int languageID, string username, string password, string firstName, string lastName)
		{
			if (existingAccount == null)
				throw new NetSteps.Common.Exceptions.NetStepsBusinessException("This email is already enrolled.")
				{
					PublicMessage = Translation.GetTerm("EmailAlreadyEnrolled", "This email is already enrolled.")
				};

			existingAccount.FirstName = firstName;
			existingAccount.LastName = lastName;
			existingAccount.SponsorID = sponsorID;
			existingAccount.EnrollerID = sponsorID;
			existingAccount.DefaultLanguageID = languageID;

			existingAccount.User.Username = username;
			existingAccount.User.Password = password;
			existingAccount.User.DefaultLanguageID = languageID;
			existingAccount.User.TotalLoginCount++;
			existingAccount.User.LastLoginUTC = DateTime.UtcNow;

			existingAccount.Save();
			//We need this so that the Roles and Functions and who knows what else is loaded correctly. - SOK
			return Account.LoadFull(existingAccount.AccountID);
		}

		public virtual Account BaseEnrollInformation(Account account, IAccountRepository repository, int sponsorID, int languageID, string firstName, string lastName, string email, string username, string password, short accountTypeID, int marketID)
		{
			if (accountTypeID == (short)Constants.AccountType.Distributor)
				accountTypeID = (short)Constants.AccountType.RetailCustomer;

			account.FirstName = firstName;
			account.LastName = lastName;
			account.EmailAddress = email;
			account.AccountTypeID = accountTypeID;

			//set account status to active, and the enrollment date to now - Scott Wilson
			account.Activate();
			account.DefaultLanguageID = languageID;
			account.EnrollmentDate = DateTime.Now;

			int mrktID = marketID;
			if (marketID < 1)
			{
				Account sponsor = repository.Load(sponsorID);
				if (sponsor != null)
					mrktID = sponsor.MarketID;
			}
			account.MarketID = mrktID;

			account.SponsorID = sponsorID;
			account.EnrollerID = sponsorID;

			account.User = new User();
			account.User.Username = username;
			account.User.Password = password.ToString();
			account.User.UserTypeID = (int)Constants.UserType.Distributor;
			account.User.UserStatusID = (int)Constants.UserStatus.Active;
			account.User.DefaultLanguageID = languageID;


			account.Save();
			//We need this so that the Roles and Functions and who knows what else is loaded correctly. - SOK
			account = Account.LoadFull(account.AccountID);

			return account;
		}

		public virtual Account EnrollRetailCustomer(IAccountRepository repository, int sponsorID, int languageID, string firstName, string lastName, string email, string username, string password, int accountTypeID)
		{
			return EnrollRetailCustomer(repository, sponsorID, languageID, firstName, lastName, email, username, password, accountTypeID, 0);
		}

		public virtual Account EnrollRetailCustomer(IAccountRepository repository, int sponsorID, int languageID, string firstName, string lastName, string email, string username, string password, int accountTypeID, int marketID)
		{
			try
			{
				Account account;
				if (accountTypeID == (short)Constants.AccountType.RetailCustomer || accountTypeID == (short)Constants.AccountType.Distributor)
					account = repository.LoadAccountByEmailAndAccountType(email, Constants.AccountType.RetailCustomer);
				else
					account = repository.LoadAccountByEmailAndAccountType(email, Constants.AccountType.PreferredCustomer);

				var updateExistingAccount = Account.ExistingRetailAccountCanBeUpdated(account);

				if (updateExistingAccount)
					return UpdateExistingRetailCustomer(account, sponsorID, languageID, username, password, firstName, lastName);

				if (Account.NonProspectNonExpressAccountExists(email) && (account == null || account.User != null))
				{
					throw new NetSteps.Common.Exceptions.NetStepsBusinessException("This email is already enrolled.")
					{
						PublicMessage = Translation.GetTerm("EmailAlreadyEnrolled", "This email is already enrolled.")
					};
				}

				if (account == null)
					account = Account.GetAccountByEmailAndSponsorID(email, sponsorID, true, true);

				if (account == null)
				{
					account = new Account { DateCreated = DateTime.Now };
				}

				BaseEnrollInformation(account, repository, sponsorID, languageID, firstName, lastName, email, username, password, (short)Constants.AccountType.RetailCustomer, 0);

				return account;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}


		public virtual Account EnrollRetailCustomerFromTempAccount(IAccountRepository repository, Account account, int sponsorID, int languageID, string firstName, string lastName, string email, string username, string password)
		{
			return EnrollRetailCustomerFromTempAccount(repository, account, sponsorID, languageID, firstName, lastName, email, username, password, 0);
		}

		public virtual Account EnrollRetailCustomerFromTempAccount(IAccountRepository repository, Account account, int sponsorID, int languageID, string firstName, string lastName, string email, string username, string password, int marketID)
		{
			try
			{
				if (account == null)
				{
					account = new Account { DateCreated = DateTime.Now };
				}

				account.FirstName = firstName;
				account.LastName = lastName;
				account.EmailAddress = email;
				account.AccountTypeID = (int)Constants.AccountType.RetailCustomer;
				//set account status to active, and the enrollment date to now - Scott Wilson
				account.Activate();
				account.SponsorID = sponsorID;
				account.EnrollerID = sponsorID;
				account.DefaultLanguageID = languageID;
				account.EnrollmentDate = DateTime.Now;
				//TODO: Figure out if we need to set the EnrollmentDate and possibly any other columns during retail customer enrollment - DES

				account.User = new User();
				account.User.Username = username;
				account.User.Password = password;
				account.User.UserTypeID = (int)Constants.UserType.Distributor;
				account.User.UserStatusID = (int)Constants.UserStatus.Active;
				account.User.DefaultLanguageID = languageID;
				if (account.MarketID < 1)
				{
					int mrktID = marketID;
					if (marketID < 1)
					{
						Account sponsor = repository.Load(sponsorID);
						if (sponsor != null)
							mrktID = sponsor.MarketID;
					}
					account.MarketID = mrktID;
				}

				account.Save();

				//We need this so that the Roles and Functions and who knows what else is loaded correctly. - SOK
				account = Account.LoadFull(account.AccountID);

				return account;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public virtual AccountPaymentMethod LoadPaymentMethodAndVerifyAccount(IAccountRepository repository, int paymentMethodID, int accountID)
		{
			try
			{
				var accountPaymentMethod = repository.LoadPaymentMethodAndVerifyAccount(paymentMethodID, accountID);
				accountPaymentMethod.StartEntityTracking();
				accountPaymentMethod.IsLazyLoadingEnabled = true;
				return accountPaymentMethod;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public virtual Address LoadAddressAndVerifyAccount(IAccountRepository repository, int addressID, int accountID)
		{
			try
			{
				var address = repository.LoadAddressAndVerifyAccount(addressID, accountID);
				address.StartEntityTracking();
				address.IsLazyLoadingEnabled = true;
				return address;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}
		public virtual List<AccountReport> LoadAccountReports(IAccountRepository repository, int accountID)
		{
			try
			{
				var accountReports = repository.LoadAccountReports(accountID);
				foreach (var accountReport in accountReports)
				{
					accountReport.StartEntityTracking();
					accountReport.IsLazyLoadingEnabled = true;
				}
				return accountReports;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public virtual List<AccountReport> LoadCorporateReports(IAccountRepository repository)
		{
			try
			{
				var corporateReports = repository.LoadCorporateReports();
				foreach (var corporateReport in corporateReports)
				{
					corporateReport.StartEntityTracking();
					corporateReport.IsLazyLoadingEnabled = true;
				}
				return corporateReports;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		/// <summary>
		/// Parses the contents of a 'Outlook Contact Export CSV' formatted doc exported file from 
		/// outlook into Accounts to be saved as 'Prospects'. -JHE
		/// </summary>
		/// <param name="csvContents"></param>
		/// <returns></returns>
		public virtual List<Account> ImportProspects(string csvContents)
		{
			try
			{
				List<Account> importedAccounts = new List<Account>();

				int endOfColumnNames = csvContents.IndexOf(Environment.NewLine);

				if (endOfColumnNames <= 0)
					throw new NetStepsBusinessException("Error importing prospect. Invalid data.")
					{
						PublicMessage = Translation.GetTerm("ErrorImportingProspectInvalidData.", "Error importing prospect. Invalid data.")
					};

				#region Get The Column Names

				string header = csvContents.Substring(0, endOfColumnNames);
				string[] ColumnsNames = header.Split(new char[] { ',' });

				Dictionary<string, int> col = new Dictionary<string, int>();

				for (int i = 0; i < ColumnsNames.Length; i++)
					col.Add(ColumnsNames[i].Replace("\"", ""), i);

				#region Column Names
				//First Name
				//Last Name
				//Business Street
				//Business Street 2
				//Business City
				//Business State
				//Business Postal Code
				//Business Country/Region
				//Home Street	
				//Home Street 2	
				//Home City	
				//Home State	
				//Home Postal Code	
				//Home Country/Region	
				//Business Fax	
				//Business Phone	
				//Home Fax	
				//Home Phone	
				//Mobile Phone	
				//E-mail Address
				#endregion

				#endregion

				// Skip past the Column Names Row and the newline at the end of the row
				string rows = csvContents.Substring(endOfColumnNames + 2, csvContents.Length - (endOfColumnNames + 2));
				List<List<string>> parsedRows = OutlookContactsParseHelper.CSVParser(rows);
				List<List<string>> trimmedRows = OutlookContactsParseHelper.RemoveEmptyRows(parsedRows);
				foreach (List<string> rowValues in trimmedRows)
				{
					Account account = new Account();

					string firstname = string.Empty;
					#region Get the First Name

					if (col.ContainsKey("First Name"))
						firstname = !string.IsNullOrEmpty(rowValues[col["First Name"]]) ? rowValues[col["First Name"]] : string.Empty;

					if (string.IsNullOrEmpty(firstname))
					{
						if (col.ContainsKey("Name"))
							firstname = !string.IsNullOrEmpty(rowValues[col["Name"]]) ? rowValues[col["Name"]] : string.Empty;
					}

					#endregion
					account.FirstName = firstname;

					account.LastName = col.ContainsKey("Last Name") ? rowValues[col["Last Name"]] : string.Empty;
					account.WorkPhone = rowValues[col["Business Phone"]];
					account.HomePhone = rowValues[col["Home Phone"]];
					account.CellPhone = rowValues[col["Mobile Phone"]];
					account.Fax = !string.IsNullOrEmpty(rowValues[col["Business Fax"]]) ? rowValues[col["Business Fax"]] : rowValues[col["Home Fax"]];

					if (col.ContainsKey("Birthday"))
					{
						if (!string.IsNullOrEmpty(rowValues[col["Birthday"]]) && rowValues[col["Birthday"]].ToCleanString() != "0/0/00")
							account.Birthday = rowValues[col["Birthday"]].ToCleanString().ToDateTimeNullable();
					}

					string emailaddress = string.Empty;
					#region Get the Email

					if (col.ContainsKey("E-mail Address"))
						emailaddress = !string.IsNullOrEmpty(rowValues[col["E-mail Address"]]) ? rowValues[col["E-mail Address"]] : string.Empty;

					if (string.IsNullOrEmpty(emailaddress))
					{
						if (col.ContainsKey("E-mail 2 Address"))
							emailaddress = !string.IsNullOrEmpty(rowValues[col["E-mail 2 Address"]]) ? rowValues[col["E-mail 2 Address"]] : string.Empty;
					}

					if (string.IsNullOrEmpty(emailaddress))
					{
						if (col.ContainsKey("E-mail 3 Address"))
							emailaddress = !string.IsNullOrEmpty(rowValues[col["E-mail 3 Address"]]) ? rowValues[col["E-mail 3 Address"]] : string.Empty;
					}

					#endregion
					if (Regex.IsMatch(emailaddress, NetSteps.Common.RegularExpressions.EmailOrEmpty))
						account.EmailAddress = emailaddress;

					Address homeAddress = new Address() { AddressTypeID = Constants.AddressType.Main.ToShort() };
					Address businessAddress = new Address() { AddressTypeID = Constants.AddressType.Billing.ToShort() };

					#region Get the Address

					if (col.ContainsKey("Business Street"))
						businessAddress.Address1 = !string.IsNullOrEmpty(rowValues[col["Business Street"]]) ? rowValues[col["Business Street"]] : string.Empty;
					if (col.ContainsKey("Business Street 2"))
						businessAddress.Address2 = !string.IsNullOrEmpty(rowValues[col["Business Street 2"]]) ? rowValues[col["Business Street 2"]] : string.Empty;

					if (col.ContainsKey("Home Street"))
						homeAddress.Address1 = !string.IsNullOrEmpty(rowValues[col["Home Street"]]) ? rowValues[col["Home Street"]] : string.Empty;
					if (col.ContainsKey("Home Street 2"))
						homeAddress.Address2 = !string.IsNullOrEmpty(rowValues[col["Home Street 2"]]) ? rowValues[col["Home Street 2"]] : string.Empty;

					#endregion

					#region Get the City

					if (col.ContainsKey("Business City"))
						businessAddress.City = !string.IsNullOrEmpty(rowValues[col["Business City"]]) ? rowValues[col["Business City"]] : string.Empty;

					if (col.ContainsKey("Home City"))
						homeAddress.City = !string.IsNullOrEmpty(rowValues[col["Home City"]]) ? rowValues[col["Home City"]] : string.Empty;

					#endregion

					int countryID = Constants.Country.UnitedStates.ToInt();
					// TODO: Do a try parse to try and get the country if not USA - JHE
					string country = string.Empty;
					#region Get the Country

					if (col.ContainsKey("Business Country/Region"))
						country = !string.IsNullOrEmpty(rowValues[col["Business Country/Region"]]) ? rowValues[col["Business Country/Region"]] : string.Empty;
					if (String.IsNullOrWhiteSpace(country) || country == "United States of America")
						businessAddress.CountryID = Constants.Country.UnitedStates.ToInt();
					else
					{
						var parsedCountry = SmallCollectionCache.Instance.Countries.FirstOrDefault(c => c.Name.Equals(country, StringComparison.InvariantCultureIgnoreCase) || c.CountryCode == country || c.CountryCode3 == country);
						if (parsedCountry != null)
							businessAddress.CountryID = parsedCountry.CountryID;
						else
							businessAddress.CountryID = Constants.Country.UnitedStates.ToInt();
					}

					if (col.ContainsKey("Home Country/Region"))
						country = !string.IsNullOrEmpty(rowValues[col["Home Country/Region"]]) ? rowValues[col["Home Country/Region"]] : string.Empty;
					if (String.IsNullOrWhiteSpace(country) || country == "United States of America")
						homeAddress.CountryID = Constants.Country.UnitedStates.ToInt();
					else
					{
						var parsedCountry = SmallCollectionCache.Instance.Countries.FirstOrDefault(c => c.Name.Equals(country, StringComparison.InvariantCultureIgnoreCase) || c.CountryCode == country || c.CountryCode3 == country);
						if (parsedCountry != null)
							homeAddress.CountryID = parsedCountry.CountryID;
						else
							homeAddress.CountryID = Constants.Country.UnitedStates.ToInt();
					}

					#endregion

					string state = string.Empty;
					#region Get the State

					if (col.ContainsKey("Business State"))
						state = !string.IsNullOrEmpty(rowValues[col["Business State"]]) ? rowValues[col["Business State"]] : string.Empty;
					businessAddress.SetState(state, countryID);

					if (col.ContainsKey("Home State"))
						state = !string.IsNullOrEmpty(rowValues[col["Home State"]]) ? rowValues[col["Home State"]] : string.Empty;
					homeAddress.SetState(state, countryID);

					#endregion

					#region Get the Postal Code

					if (col.ContainsKey("Business Postal Code"))
						businessAddress.PostalCode = !string.IsNullOrEmpty(rowValues[col["Business Postal Code"]]) ? rowValues[col["Business Postal Code"]] : string.Empty;

					if (col.ContainsKey("Home Postal Code"))
						homeAddress.PostalCode = !string.IsNullOrEmpty(rowValues[col["Home Postal Code"]]) ? rowValues[col["Home Postal Code"]] : string.Empty;

					#endregion

					if (!homeAddress.IsEmpty(true))
						account.Addresses.Add(homeAddress);
					if (!businessAddress.IsEmpty(true))
						account.Addresses.Add(businessAddress);

					if (account.Addresses.Count == 1)
						account.Addresses[0].AddressTypeID = Constants.AddressType.Main.ToShort();

					account.AccountTypeID = Constants.AccountType.Prospect.ToShort();

					importedAccounts.Add(account);
				}

				return importedAccounts;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}


		public virtual void DeleteAccountPaymentMethod(Account account, int paymentMethodId)
		{
			if (account == null)
			{
				throw new ArgumentNullException("account");
			}

			if (!account.AccountPaymentMethods.Any(apm => apm.AccountPaymentMethodID == paymentMethodId))
			{
				throw new NetStepsBusinessException("The payment method cannot be deleted. It does not belong to the specified account.");
			}

			if (AccountPaymentMethod.IsUsedByAnyActiveOrderTemplates(paymentMethodId))
			{
				string errorMessage = "The payment method cannot be deleted. It is used by one or more active subscriptions.";
				var exception = new NetStepsBusinessException(errorMessage);
				exception.PublicMessage = Translation.GetTerm("ErrorCannotDeletePaymentMethodInUse", errorMessage);
				throw exception;
			}

			try
			{
				account.StartEntityTracking();
				var accountPaymentMethod = account.AccountPaymentMethods.First(a => a.AccountPaymentMethodID == paymentMethodId);

				// Delete Address is it is a Billing Address only used by the current billing profile - JHE
				int? billingAddressID = accountPaymentMethod.BillingAddressID;
				if (billingAddressID != null && accountPaymentMethod.BillingAddress != null &&
					 accountPaymentMethod.BillingAddress.AddressTypeID == Constants.AddressType.Billing.ToShort() &&
					 account.AccountPaymentMethods.Count(p => p.BillingAddressID == billingAddressID) <= 1)
				{
					if (accountPaymentMethod.BillingAddress.ChangeTracker.State != ObjectState.Added)
						accountPaymentMethod.BillingAddress.MarkAsDeleted();

					var address = account.Addresses.FirstOrDefault(a => a.AddressID == billingAddressID);
					if (address != null)
						account.Addresses.Remove(address);
				}

				if (accountPaymentMethod.ChangeTracker.State != ObjectState.Added)
					accountPaymentMethod.MarkAsDeleted();
				account.AccountPaymentMethods.Remove(accountPaymentMethod);

				account.Save();
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public virtual void DeleteAccountAddress(Account account, int addressId)
		{
			account.IsNotNull();
			if (!account.Addresses.Any(x => x.AddressID == addressId))
				throw new NetStepsBusinessException("The address cannot be deleted. It does not belong to the specified account.");

			if (Address.IsUsedByAnyActiveOrderTemplates(addressId))
			{
				const string errorMessage = "The address cannot be deleted. It is used by one or more active subscriptions.";
				var exception = new NetStepsBusinessException(errorMessage) { PublicMessage = Translation.GetTerm(string.Join(" ", errorMessage), errorMessage) };
				throw exception;
			}

			try
			{
				if (account.Addresses.Select(a => a.AddressID).Contains(addressId))
				{
					account.StartEntityTracking();
					var address = account.Addresses.First(a => a.AddressID == addressId);
					if (address.ChangeTracker.State != ObjectState.Added)
						address.MarkAsDeleted();
					account.Addresses.Remove(address);
					account.Save();
				}
				else
					throw new Exception(Translation.GetTerm("AddressNotFoundOnCurrentAccount", "Address not found on current Account."));
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public virtual string ToFullName(string firstName, string middleName, string lastName, string countryCultureInfoCode)
		{
			try
			{
				if (countryCultureInfoCode == CountryCultureInfoCode.UnitedStates ||
					 countryCultureInfoCode == CountryCultureInfoCode.Canada)
				{
					return string.Format("{0}{1} {2}", firstName, (!middleName.IsNullOrEmpty()) ? " " + middleName : string.Empty, lastName);
				}
				else if (countryCultureInfoCode == CountryCultureInfoCode.Japan)
					return string.Format("{0} {1}", lastName, firstName);
				else
					return string.Format("{0}{1} {2}", firstName, (!middleName.IsNullOrEmpty()) ? " " + middleName : string.Empty, lastName);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		// http://www.csoft.co.uk/coverage - JHE
		public virtual string DisplayPhone(string phone, string countryCultureInfoCode)
		{
			if (string.IsNullOrWhiteSpace(phone))
			{
				return phone;
			}

			var phoneDigits = Regex.Replace(phone, @"[\D]", string.Empty);

			if (string.IsNullOrEmpty(phoneDigits))
			{
				return phone;
			}

			if (countryCultureInfoCode == CountryCultureInfoCode.UnitedStates
				|| countryCultureInfoCode == CountryCultureInfoCode.Canada)
			{
				if (phoneDigits.Length == 11)
					return Regex.Replace(phoneDigits, @"(.{1})(.{3})(.{3})(.{4})", "$1 ($2) $3-$4");
				else if (phoneDigits.Length == 10)
					return Regex.Replace(phoneDigits, @"(.{3})(.{3})(.{4})", "($1) $2-$3");
				else if (phoneDigits.Length == 7)
					return Regex.Replace(phoneDigits, @"(.{3})(.{4})", "$1-$2");
			}
			else if (countryCultureInfoCode == CountryCultureInfoCode.GreatBritain)
			{
				if (phoneDigits.Length == 11)
					return Regex.Replace(phoneDigits, @"(.{5})(.{3})(.{3})", "$1 $2 $3");
			}
			else if (countryCultureInfoCode == CountryCultureInfoCode.Australia)
			{
				if (phoneDigits.Length == 8)
					return Regex.Replace(phoneDigits, @"(.{4})(.{4})", "$1 $2");
				else if (phoneDigits.Length == 10)
					return Regex.Replace(phoneDigits, @"(.{2})(.{4})(.{4})", "($1) $2 $3");
				else if (phoneDigits.Length == 11)
					return Regex.Replace(phoneDigits, @"(.{2})(.{1})(.{4})(.{4})", "+$1 $2 $3 $4");
			}
			else if (countryCultureInfoCode == CountryCultureInfoCode.Norway)
			{
				if (phoneDigits.Length == 8)
					return Regex.Replace(phoneDigits, @"(.{4})(.{4})", "$1 $2");
				else if (phoneDigits.Length == 10)
					return Regex.Replace(phoneDigits, @"(.{2})(.{4})(.{4})", "+$1 $2 $3");
			}
			else if (countryCultureInfoCode == CountryCultureInfoCode.Belgium)
			{
				if (phoneDigits.Length == 9)
					return Regex.Replace(phoneDigits, @"(.{2})(.{3})(.{4})", "$1 $2 $3");
				else if (phoneDigits.Length == 10)
					return Regex.Replace(phoneDigits, @"(.{2})(.{1})(.{3})(.{4})", "+$1 $2 $3 $4");
			}
			else if (countryCultureInfoCode == CountryCultureInfoCode.Germany)
			{
				if (phoneDigits.Length == 8)
					return Regex.Replace(phoneDigits, @"(.{3})(.{5})", "$1-$2");
				else if (phoneDigits.Length == 11)
					return Regex.Replace(phoneDigits, @"(.{3})(.{3})(.{5})", "($1) / $2-$3");
				else if (phoneDigits.Length == 12)
					return Regex.Replace(phoneDigits, @"(.{2})(.{2})(.{3})(.{5})", "+$1-$2-$3-$4");
				else if (phoneDigits.Length == 14)
					return Regex.Replace(phoneDigits, @"(.{2})(.{2})(.{2})(.{3})(.{5})", "$1-$2-$3-$4-$5");
			}
			else if (countryCultureInfoCode == CountryCultureInfoCode.Denmark)
			{
				if (phoneDigits.Length == 8)
					return Regex.Replace(phoneDigits, @"(.{4})(.{4})", "$1 $2");
			}
			else if (countryCultureInfoCode == CountryCultureInfoCode.France)
			{
				if (phoneDigits.Length == 10)
					return Regex.Replace(phoneDigits, @"(.{2})(.{2})(.{2})(.{2})(.{2})", "$1 $2 $3 $4 $5");
				else if (phoneDigits.Length == 11)
					return Regex.Replace(phoneDigits, @"(.{2})(.{1})(.{2})(.{2})(.{2})(.{2})", "+$1 $2 $3 $4 $5 $6");
			}
			else if (countryCultureInfoCode == CountryCultureInfoCode.NetherLands)
			{
				if (phoneDigits.Length == 8)
					return Regex.Replace(phoneDigits, @"(.{4})(.{4})", "$1-$2");
				else if (phoneDigits.Length == 10)
					return Regex.Replace(phoneDigits, @"(.{3})(.{7})", "$1-$2");
				else if (phoneDigits.Length == 11)
					return Regex.Replace(phoneDigits, @"(.{2})(.{1})(.{8})", "+$1 $2 $3");
				else if (phoneDigits.Length == 12)
					return Regex.Replace(phoneDigits, @"(.{2})(.{2})(.{8})", "+$1 $2 $3");
			}
			else if (countryCultureInfoCode == CountryCultureInfoCode.Sweden)
			{
				if (phoneDigits.Length == 12)
					return Regex.Replace(phoneDigits, @"(.{2})(.{2})(.{8})", "+$1-$2-$3");
			}

			return phone;
		}

		public virtual bool IsInUpline(Account parentAccount, Account childAccount)
		{
			Account currentAccount = childAccount;

			//If these are the same account, return true
			if (childAccount.AccountID == parentAccount.AccountID)
				return true;

			bool shouldContinue = true;
			while (shouldContinue)
			{
				Account currentParent = Account.Load(currentAccount.SponsorID ?? 0);

				if (currentParent.AccountID == parentAccount.AccountID)
					return true;

				if ((currentParent.SponsorID ?? 0) == 0)
					return false;

				currentAccount = currentParent;
			}

			return false;
		}

		public virtual Dictionary<int, string> DownlineSearch(string query, int periodID, int baseAccountID, List<int> accountTypes, List<int> accountStatuses, int? maxResults, int? levelMax)
		{
			Downline downline = DownlineCache.GetDownline(periodID);
			DownlineNode baseNode = downline.LookupNode.ContainsKey(baseAccountID) ? downline.LookupNode[baseAccountID] : null;
			Account baseAccount = Account.Load(baseAccountID);

			//AccountSlimSearchData baseAccount = Account.LoadSlim(baseAccountID);

			//Create return value and initialize it with the baseAccount
			Dictionary<int, string> returnValue = new Dictionary<int, string>();
			returnValue.Add(baseAccount.AccountID, string.Format("{0} (#{1})", baseAccount.FullName, baseAccount.AccountID));

			//List which keeps track of all AccountID's on the current genealogical level
			List<DownlineNode> listOfAccountNodes = new List<DownlineNode>();
			if (baseNode.IsNotNull())
				listOfAccountNodes.Add(baseNode);

			//Iterate through levels until we get enough results, go past the result counter, or reach the bottom hierarchical level
			int currentLevel = 0;
			while (listOfAccountNodes.Count > 0)
			{
				if (levelMax != null && currentLevel >= levelMax)
					return returnValue;

				//Gets next level of accountID's, and populates the listOfAccountNumbers with it
				PaginatedList<DownlineNode> newList = GetAccountsForEnrollerID(listOfAccountNodes);

				//Iterate through new accounts and add them to returnValue
				foreach (var a in newList)
				{
					var nodeData = downline.Lookup[a.AccountID];

					//If the node's name or ID doesn't match the query, pass over it
					if (!(((string)string.Format("{0} {1}", nodeData.FirstName, nodeData.LastName)).ContainsIgnoreCase(query) ||
						 (a.AccountID.ToString()).ContainsIgnoreCase(query) ||
						 ((string)string.Format("{0} {1} (#{2})", nodeData.FirstName, nodeData.LastName, a.AccountID.ToString())).ContainsIgnoreCase(query)))
						continue;

					if (a.AccountTypeID == null || a.AccountStatusID == null)
					{
						//If the account status is not in one of the accountStatuses, pass over it
						Account tempAccount = Account.Load(a.AccountID);
						//AccountSlimSearchData tempAccount = Account.LoadSlim(a.AccountID);
						if (!accountStatuses.Contains(tempAccount.AccountStatusID) || !accountTypes.Contains(tempAccount.AccountTypeID))
							continue;
					}
					else
					{
						if (!accountStatuses.Contains(a.AccountStatusID ?? 0) || !accountTypes.Contains(a.AccountTypeID ?? 0))
							continue;
					}

					//Add the value
					returnValue.Add(a.AccountID, string.Format("{0} {1} (#{2})", nodeData.FirstName, nodeData.LastName, a.AccountID));

					//If we are now over our max result limit, stop searching
					if (maxResults != null && returnValue.Count >= maxResults)
						return returnValue;
				}
				currentLevel++;
			}

			return returnValue;
		}

		/// <summary>
		/// Gets all accounts under each DonwlineNode in nodes. Clears nodes, and loads it up with the newly found results.
		/// Returns a PaginatedList of those newly found results
		/// </summary>
		/// <param name="nodes"></param>
		/// <param name="accountTypes"></param>
		/// <returns></returns>
		public virtual PaginatedList<DownlineNode> GetAccountsForEnrollerID(List<DownlineNode> nodes)
		{
			PaginatedList<DownlineNode> returnValue = new PaginatedList<DownlineNode>();
			foreach (DownlineNode node in nodes)
			{
				returnValue.AddRange(node.Children);
			}

			nodes.Clear();
			nodes.AddRange(returnValue);

			return returnValue;
		}

		public virtual bool EnforceUniqueTaxNumber(IAccountRepository repository, Account entity, string countryCultureInfoCode = null)
		{
			return true;
		}

		public Account LoadAccountByEmailAndAccountType(IAccountRepository repository, string email, ConstantsGenerated.AccountType accountType)
		{
			try
			{
				var account = repository.LoadAccountByEmailAndAccountType(email, accountType);

				return account;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}

		}

		public virtual void AssignRolesByAccountType(Account account)
		{
			if (account.User != null)
			{
				var accountTypeRoles = AccountType.GetRolesByAccountType(account.AccountTypeID);
				foreach (var role in accountTypeRoles)
				{
					if (!account.User.Roles.Any(r => r.RoleID == role.RoleID))
						account.User.Roles.Add(role);
				}
			}
		}

		public virtual void OnEnrollmentCompleted(IAccountRepository repository, Account account, Order enrollmentOrder)
		{
		}

		public virtual void OnEnrollmentCompleted(IEnrollmentContext enrollmentContext)
		{
		}

        public virtual Dictionary<int, string> SlimSearch(IAccountRepository repository, string query)
        {
            try
            {

                return repository.SlimSearch(query);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public virtual CoApplicantSearchParameters GetAccountAdditionalTitulars(int AccountID)
        {
            try
            {
                return new AccountAdditionalTitularsRepository().GetAccountAdditionalTitulars(AccountID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public virtual int InsertAccountAdditionalTitulars(CoApplicantSearchParameters CoApplicantSearchParameters)
        {
            try
            {
                return new AccountAdditionalTitularsRepository().InsertAccountAdditionalTitulars(CoApplicantSearchParameters);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public virtual void InsertAccountAdditionalTitularSuppliedIDs(AccountAdditionalTitularSuppliedIDsParameters AccountAdditionalTitularSuppliedIDsParameters)
        {
            try
            {
                new AccountAdditionalTitularsRepository().InsertAccountAdditionalTitularSuppliedIDs(AccountAdditionalTitularSuppliedIDsParameters);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public virtual void InsertAccountAdditionalPhones(AccountAdditionalPhonesParameters AccountAdditionalPhonesParameters)
        {
            try
            {
                new AccountAdditionalPhonesRepository().InsertAccountAdditionalPhones(AccountAdditionalPhonesParameters);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public virtual int UpdateAccountAdditionalTitulars(CoApplicantSearchParameters CoApplicantSearchParameters)
        {
            try
            {
                return new AccountAdditionalTitularsRepository().UpdateAccountAdditionalTitulars(CoApplicantSearchParameters);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public virtual void UpdateAccountAdditionalTitularSuppliedIDs(AccountAdditionalTitularSuppliedIDsParameters AccountAdditionalTitularSuppliedIDsParameters)
        {
            try
            {
                new AccountAdditionalTitularsRepository().UpdateAccountAdditionalTitularSuppliedIDs(AccountAdditionalTitularSuppliedIDsParameters);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public virtual void UpdateAccountAdditionalPhones(AccountAdditionalPhonesParameters AccountAdditionalPhonesParameters)
        {
            try
            {
                new AccountAdditionalPhonesRepository().UpdateAccountAdditionalPhones(AccountAdditionalPhonesParameters);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public PaginatedList<AccountWithoutSponsorSearchData> GetAccountsWithoutSponsor(AccountWithoutSponsorSearchParameters seachParameters)
        {
            return new AccountRepository().GetAccountsWithoutSponsor(seachParameters);
        }
	}
}
