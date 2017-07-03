using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Interfaces;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Enrollment.Common.Models.Context;

namespace NetSteps.Data.Entities.Business.Logic.Interfaces
{
	[ContractClass(typeof(Contracts.AccountBusinessLogicContracts))]
    public partial interface IAccountBusinessLogic
    { 
        void GenerateAndSetNewAccountNumber(Account account, bool saveAccount = true);
        void SendGeneratedPasswordEmail(Account account);
        Account LoadByAccountNumberFull(IAccountRepository repository, string accountNumber);
        Account LoadByAccountNumber(IAccountRepository repository, string accountNumber);
        Account LoadInfoCard(IAccountRepository repository, int accountNumber);
        IEnumerable<Account> LoadInfoCardBatch(IAccountRepository repository, List<int> accountNumber);
        void BuildReadOnlyNotesTree(Account account);
        PaginatedList<AccountSearchData> Search(IAccountRepository repository, AccountSearchParameters searchParameters);
        Dictionary<int, string> SlimSearchOnAccountStatuses(IAccountRepository repository, string query, int? accountTypeID = null, int?[] statusIDs = null, int? sponsorID = null);
        Dictionary<int, string> SlimSearchEmail(IAccountRepository repository, string query, int? statusID = null, int? sponsorID = null);
        Dictionary<int, string> DownlineSearch(string query, int periodID, int baseAccountID, List<int> accountTypes, List<int> accountStatuses, int? maxResults, int? maxLevel);
        AccountSlimSearchData LoadSlim(IAccountRepository repository, int accountID);
        AccountSlimSearchData LoadSlimByAccountNumber(IAccountRepository repository, string accountNumber);
        Account Authenticate(IAccountRepository repository, string username, string password);
        IPaginatedList<IAccountLocatorSearchData> AccountLocatorSearch(IAccountRepository repository, IAccountLocatorSearchParameters searchParameters);
        List<AccountSearchData> LoadBatchHeaders(IAccountRepository repository, List<int> accountIDs);
        PaginatedList<AuditLogRow> GetAuditLog(IAccountRepository repository, Account fullyLoadedAccount, AuditLogSearchParameters param);
        Account EnrollRetailCustomer(IAccountRepository repository, int sponsorId, int languageId, string firstName, string lastName, string email, string username, string password, short accountTypeID);
		Account EnrollExpressRetailCustomer(IAccountRepository repository, int sponsorId, int languageId, string firstName, string lastName, string email, string username, short accountTypeID);
		  Account EnrollRetailCustomer(IAccountRepository repository, int sponsorId, int languageId, string firstName, string lastName, string email, string username, string password, int marketID);
        Account EnrollRetailCustomerFromTempAccount(IAccountRepository repository, Account account, int sponsorId, int languageId, string firstName, string lastName, string email, string username, string password);
        AccountPaymentMethod LoadPaymentMethodAndVerifyAccount(IAccountRepository repository, int paymentMethodId, int accountId);
        Address LoadAddressAndVerifyAccount(IAccountRepository repository, int addressID, int accountID);
        List<AccountReport> LoadAccountReports(IAccountRepository repository, int accountID);
        List<AccountReport> LoadCorporateReports(IAccountRepository repository);
        List<Account> ImportProspects(string csvContents);
        void DeleteAccountPaymentMethod(Account account, int paymentMethodId);
        void DeleteAccountAddress(Account account, int addressId);
        string GetIsCommissionQualifiedStatus(bool? isCommissionQualifed, DateTime? autoshipProcessDate);
        string ToFullName(string firstName, string middleName, string lastName, string countryCultureInfoCode);
        string DisplayPhone(string phone, string countryCultureInfoCode);
        bool EnforceUniqueTaxNumber(IAccountRepository repository, Account entity, string countryCultureInfoCode);
        bool IsInUpline(Account parentAccount, Account childAccount);
        Account LoadAccountByEmailAndAccountType(IAccountRepository repository, string email, Generated.ConstantsGenerated.AccountType accountType);
        Account LoadAccountByUserId(IAccountRepository repository, int userId);
		void OnEnrollmentCompleted(IAccountRepository repository, Account account, Order enrollmentOrder);
		void OnEnrollmentCompleted(IEnrollmentContext enrollmentContext);
		void AssignRolesByAccountType(Account account);
        Dictionary<int, string> SlimSearch(IAccountRepository repository, string query);
    }  
      
	namespace Contracts
	{
		[ContractClassFor(typeof(IAccountBusinessLogic))]
		internal abstract class AccountBusinessLogicContracts : IAccountBusinessLogic
		{
			void IAccountBusinessLogic.GenerateAndSetNewAccountNumber(Account account, bool saveAccount)
			{
				throw new NotImplementedException();
			}

			void IAccountBusinessLogic.SendGeneratedPasswordEmail(Account account)
			{
				throw new NotImplementedException();
			}

			Account IAccountBusinessLogic.LoadByAccountNumberFull(IAccountRepository repository, string accountNumber)
			{
				throw new NotImplementedException();
			}

			Account IAccountBusinessLogic.LoadByAccountNumber(IAccountRepository repository, string accountNumber)
			{
				throw new NotImplementedException();
			}

			Account IAccountBusinessLogic.LoadInfoCard(IAccountRepository repository, int accountNumber)
			{
				throw new NotImplementedException();
			}

			System.Collections.Generic.IEnumerable<Account> IAccountBusinessLogic.LoadInfoCardBatch(IAccountRepository repository, System.Collections.Generic.List<int> accountNumber)
			{
				throw new NotImplementedException();
			}

			void IAccountBusinessLogic.BuildReadOnlyNotesTree(Account account)
			{
				throw new NotImplementedException();
			}

			NetSteps.Common.Base.PaginatedList<AccountSearchData> IAccountBusinessLogic.Search(IAccountRepository repository, AccountSearchParameters searchParameters)
			{
				throw new NotImplementedException();
			}

			System.Collections.Generic.Dictionary<int, string> IAccountBusinessLogic.SlimSearchOnAccountStatuses(IAccountRepository repository, string query, int? accountTypeID, int?[] statusIDs, int? sponsorID)
			{
				throw new NotImplementedException();
			}

			System.Collections.Generic.Dictionary<int, string> IAccountBusinessLogic.SlimSearchEmail(IAccountRepository repository, string query, int? statusID, int? sponsorID)
			{
				throw new NotImplementedException();
			}

			System.Collections.Generic.Dictionary<int, string> IAccountBusinessLogic.DownlineSearch(string query, int periodID, int baseAccountID, System.Collections.Generic.List<int> accountTypes, System.Collections.Generic.List<int> accountStatuses, int? maxResults, int? maxLevel)
			{
				throw new NotImplementedException();
			}

			AccountSlimSearchData IAccountBusinessLogic.LoadSlim(IAccountRepository repository, int accountID)
			{
				throw new NotImplementedException();
			}

			AccountSlimSearchData IAccountBusinessLogic.LoadSlimByAccountNumber(IAccountRepository repository, string accountNumber)
			{
				throw new NotImplementedException();
			}

			Account IAccountBusinessLogic.Authenticate(IAccountRepository repository, string username, string password)
			{
				throw new NotImplementedException();
			}

			NetSteps.Common.Base.IPaginatedList<IAccountLocatorSearchData> IAccountBusinessLogic.AccountLocatorSearch(IAccountRepository repository, IAccountLocatorSearchParameters searchParameters)
			{
				Contract.Requires<ArgumentNullException>(repository != null);
				Contract.Requires<ArgumentNullException>(searchParameters != null);

				throw new NotImplementedException();
			}

			System.Collections.Generic.List<AccountSearchData> IAccountBusinessLogic.LoadBatchHeaders(IAccountRepository repository, System.Collections.Generic.List<int> accountIDs)
			{
				throw new NotImplementedException();
			}

			NetSteps.Common.Base.PaginatedList<AuditLogRow> IAccountBusinessLogic.GetAuditLog(IAccountRepository repository, Account fullyLoadedAccount, AuditLogSearchParameters param)
			{
				throw new NotImplementedException();
			}

			public Account EnrollRetailCustomer(IAccountRepository repository, int sponsorId, int languageId, string firstName, string lastName, string email, string username, string password, short accountTypeID)
			{
				throw new NotImplementedException();
			}

			public Account EnrollExpressRetailCustomer(IAccountRepository repository, int sponsorId, int languageId, string firstName, string lastName, string email, string username, short accountTypeID)
			{
				throw new NotImplementedException();
			}

			public Account EnrollRetailCustomer(IAccountRepository repository, int sponsorId, int languageId, string firstName, string lastName, string email, string username, string password, int marketID)
			{
				throw new NotImplementedException();
			}

			Account IAccountBusinessLogic.EnrollRetailCustomerFromTempAccount(IAccountRepository repository, Account account, int sponsorId, int languageId, string firstName, string lastName, string email, string username, string password)
			{
				throw new NotImplementedException();
			}

			AccountPaymentMethod IAccountBusinessLogic.LoadPaymentMethodAndVerifyAccount(IAccountRepository repository, int paymentMethodId, int accountId)
			{
				throw new NotImplementedException();
			}

			Address IAccountBusinessLogic.LoadAddressAndVerifyAccount(IAccountRepository repository, int addressID, int accountID)
			{
				throw new NotImplementedException();
			}

			System.Collections.Generic.List<AccountReport> IAccountBusinessLogic.LoadAccountReports(IAccountRepository repository, int accountID)
			{
				throw new NotImplementedException();
			}

			System.Collections.Generic.List<AccountReport> IAccountBusinessLogic.LoadCorporateReports(IAccountRepository repository)
			{
				throw new NotImplementedException();
			}

			System.Collections.Generic.List<Account> IAccountBusinessLogic.ImportProspects(string csvContents)
			{
				throw new NotImplementedException();
			}

			void IAccountBusinessLogic.DeleteAccountPaymentMethod(Account account, int paymentMethodId)
			{
				throw new NotImplementedException();
			}

			void IAccountBusinessLogic.DeleteAccountAddress(Account account, int addressId)
			{
				Contract.Requires<ArgumentNullException>(account != null);

				throw new NotImplementedException();
			}

			string IAccountBusinessLogic.GetIsCommissionQualifiedStatus(bool? isCommissionQualifed, DateTime? autoshipProcessDate)
			{
				throw new NotImplementedException();
			}

			string IAccountBusinessLogic.ToFullName(string firstName, string middleName, string lastName, string countryCultureInfoCode)
			{
				throw new NotImplementedException();
			}

			string IAccountBusinessLogic.DisplayPhone(string phone, string countryCultureInfoCode)
			{
				throw new NotImplementedException();
			}

			bool IAccountBusinessLogic.EnforceUniqueTaxNumber(IAccountRepository repository, Account entity, string countryCultureInfoCode)
			{
				throw new NotImplementedException();
			}

			bool IAccountBusinessLogic.IsInUpline(Account parentAccount, Account childAccount)
			{
				throw new NotImplementedException();
			}

			Account IAccountBusinessLogic.LoadAccountByEmailAndAccountType(IAccountRepository repository, string email, Generated.ConstantsGenerated.AccountType accountType)
			{
				throw new NotImplementedException();
			}

			Account IAccountBusinessLogic.LoadAccountByUserId(IAccountRepository repository, int userId)
			{
				throw new NotImplementedException();
			}

			public void OnEnrollmentCompleted(IAccountRepository repository, Account account, Order enrollmentOrder)
			{
				throw new NotImplementedException();
			}

			public void OnEnrollmentCompleted(IEnrollmentContext enrollmentContext)
			{
				throw new NotImplementedException();
			}

			void IAccountBusinessLogic.AssignRolesByAccountType(Account account)
			{
				throw new NotImplementedException();
			}

			Func<Account, int> IBusinessEntityLogic<Account, int, IAccountRepository>.GetIdColumnFunc
			{
				get { throw new NotImplementedException(); }
			}

			Action<Account, int> IBusinessEntityLogic<Account, int, IAccountRepository>.SetIdColumnFunc
			{
				get { throw new NotImplementedException(); }
			}

			Func<Account, string> IBusinessEntityLogic<Account, int, IAccountRepository>.GetTitleColumnFunc
			{
				get { throw new NotImplementedException(); }
			}

			Action<Account, string> IBusinessEntityLogic<Account, int, IAccountRepository>.SetTitleColumnFunc
			{
				get { throw new NotImplementedException(); }
			}

			void IBusinessEntityLogic<Account, int, IAccountRepository>.DefaultValues(IAccountRepository repository, Account entity)
			{
				throw new NotImplementedException();
			}

			Account IBusinessEntityLogic<Account, int, IAccountRepository>.Load(IAccountRepository repository, int primaryKey)
			{
				throw new NotImplementedException();
			}

			Account IBusinessEntityLogic<Account, int, IAccountRepository>.LoadFull(IAccountRepository repository, int primaryKey)
			{
				throw new NotImplementedException();
			}

			System.Collections.Generic.List<Account> IBusinessEntityLogic<Account, int, IAccountRepository>.LoadAll(IAccountRepository repository)
			{
				throw new NotImplementedException();
			}

			System.Collections.Generic.List<Account> IBusinessEntityLogic<Account, int, IAccountRepository>.LoadAllFull(IAccountRepository repository)
			{
				throw new NotImplementedException();
			}

			void IBusinessEntityLogic<Account, int, IAccountRepository>.Save(IAccountRepository repository, Account entity)
			{
				throw new NotImplementedException();
			}

			void IBusinessEntityLogic<Account, int, IAccountRepository>.Delete(IAccountRepository repository, Account entity)
			{
				throw new NotImplementedException();
			}

			void IBusinessEntityLogic<Account, int, IAccountRepository>.Delete(IAccountRepository repository, int primaryKey)
			{
				throw new NotImplementedException();
			}

			NetSteps.Common.Base.PaginatedList<AuditLogRow> IBusinessEntityLogic<Account, int, IAccountRepository>.GetAuditLog(IAccountRepository repository, int primaryKey, AuditLogSearchParameters param)
			{
				throw new NotImplementedException();
			}

			void IBusinessEntityLogic<Account, int, IAccountRepository>.StartEntityTracking(Account entity)
			{
				throw new NotImplementedException();
			}

			void IBusinessEntityLogic<Account, int, IAccountRepository>.StartEntityTrackingAndEnableLazyLoading(Account entity)
			{
				throw new NotImplementedException();
			}

			void IBusinessEntityLogic<Account, int, IAccountRepository>.StopEntityTracking(Account entity)
			{
				throw new NotImplementedException();
			}

			void IBusinessEntityLogic<Account, int, IAccountRepository>.AcceptChanges(Account entity, System.Collections.Generic.List<IObjectWithChangeTracker> allTrackerItems)
			{
				throw new NotImplementedException();
			}

			void IBusinessEntityLogic<Account, int, IAccountRepository>.Validate(Account entity)
			{
				throw new NotImplementedException();
			}

			bool IBusinessEntityLogic<Account, int, IAccountRepository>.IsValid(Account entity)
			{
				throw new NotImplementedException();
			}

			void IBusinessEntityLogic<Account, int, IAccountRepository>.AddValidationRules(Account entity)
			{
				throw new NotImplementedException();
			}

			System.Collections.Generic.List<string> IBusinessEntityLogic<Account, int, IAccountRepository>.ValidatedChildPropertiesSetByParent(IAccountRepository repository)
			{
				throw new NotImplementedException();
			}

			void IBusinessEntityLogic<Account, int, IAccountRepository>.CleanDataBeforeSave(IAccountRepository repository, Account entity)
			{
				throw new NotImplementedException();
			}

			void IBusinessEntityLogic<Account, int, IAccountRepository>.UpdateCreatedFields(IAccountRepository repository, Account entity)
			{
				throw new NotImplementedException();
			}

            System.Collections.Generic.Dictionary<int, string> IAccountBusinessLogic.SlimSearch(IAccountRepository repository, string query)
            {
                throw new NotImplementedException();
            }
		}
	}
}
