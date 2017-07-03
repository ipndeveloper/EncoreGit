using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using NetSteps.Accounts.Common.Models;
using NetSteps.Common.Base;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities.Business;

namespace NetSteps.Data.Entities.Repositories
{
	[ContractClass(typeof(Contracts.AccountRepositoryContracts))]
    public partial interface IAccountRepository : ISearchRepository<AccountSearchParameters, PaginatedList<AccountSearchData>>
    {
        Account Authenticate(string username, string password);
        Account LoadByAccountNumberFull(string accountNumber);
        Account LoadByAccountNumber(string accountNumber);
        Account LoadNonProspectByEmailFull(string email);
        Account LoadInfoCard(int accountID);
        IEnumerable<Account> LoadInfoCardBatch(List<int> accountID);
        int? GetNonProspectAccountIDByEmail(string email, bool? returnActive);
        Account LoadByUserIdFull(int userID);
        Account LoadByUserIdFull_(int userID, DateTime BirthDay);
        AccountSlimSearchData LoadSlim(int accountID);
        AccountSlimSearchData LoadSlimByAccountNumber(string accountNumber);
        List<AccountSlimSearchData> LoadBatchSlim(IEnumerable<int> accountIDs);
        List<Account> GetRecent100();
        Dictionary<int, string> SlimSearchOnAccountStatuses(string query, int? accountTypeID, int?[] statusIDs, int? sponsorID);
        Dictionary<int, string> SlimSearchEmail(string query, int? statusID, int? sponsorID);
        IPaginatedList<IAccountLocatorAccountData> AccountLocatorAccountSearch(IAccountLocatorSearchParameters searchParameters);
        IQueryable<Account> GetAccountLocatorBaseQuery(IAccountLocatorSearchParameters searchParameters, NetStepsEntities context);
        IQueryable<IAccountLocatorAccountData> ApplyAccountLocatorAccountGeoQuery(IQueryable<Account> baseQuery, IAccountLocatorSearchParameters searchParameters, NetStepsEntities context);
        IQueryable<IAccountLocatorAccountData> ApplyAccountLocatorAccountNonGeoQuery(IQueryable<Account> baseQuery, IAccountLocatorSearchParameters searchParameters, NetStepsEntities context);
        IQueryable<IAccountLocatorAccountData> SortAccountLocatorQuery(IQueryable<IAccountLocatorAccountData> query, IAccountLocatorSearchParameters searchParameters);
        IList<IAccountLocatorContentData> GetAccountLocatorContent(IEnumerable<int> accountIDs);
        List<AccountSearchData> LoadBatchHeaders(List<int> primaryKeys);
        bool ExistsByAccountNumber(string accountNumber);
        List<AccountContactTag> AccountContactTags(int accountID);
        PaginatedList<AuditLogRow> GetAuditLog(Account fullyLoadedAccount, AuditLogSearchParameters searchParameters);
        AccountPaymentMethod LoadPaymentMethodAndVerifyAccount(int paymentMethodId, int accountId);
        Address LoadAddressAndVerifyAccount(int addressId, int accountId);
        List<AccountReport> LoadAccountReports(int accountID);
        List<AccountReport> LoadCorporateReports();
        bool AccountExists(string email, string currentEmail = null);
        bool IsTaxNumberAvailable(string taxNumber, int? accountID = null);
        bool NonProspectExists(string email, int? ignoreAccountID = null);
        Account GetAccountByEmailAndSponsorID(string email, int sponsorId, bool enableTracking = false, bool? getActive = null);
        Account LoadAccountByEmailAndAccountType(string email, Constants.AccountType accountType);
		Account LoadWithRelationsByAccountID(int accountID, params string[] relations);
		Account LoadWithRelationsByUserID(int userID, params string[] relations);
		Account LoadWithRelationsByEmailAddress(string email, bool includeProspects, params string[] relations);
		Account LoadWithRelationsByAccountNumber(string accountNumber, params string[] relations);
		
		Account LoadAccountByUserId(int userId);
		void SaveNote(int accountId, Note note);
        Dictionary<int, string> SearchAccountsByText(string key);
        Dictionary<int, string> SearchAccountsByTextAccountTypeAndSponsorId(string text, int accountTypeId, int sponsorId);
        Dictionary<int, string> SearchAccountsByTextAndAccountType(string text, int accountTypeId);
        Dictionary<int, string> SearchAccountsByTextAndAccountStatus(string text, int accountStatusId);
        Dictionary<int, string> SearchAccountsByTextAccountTypeAndAccountStatus(string text, int accountTypeId, int accountStatusId);
		IQueryable<Account> GetAccountSearchQuery(AccountSearchParameters searchParameters, NetStepsEntities context);
		IQueryable<AccountInfoCache> GetAccountInfoSearchQuery(AccountSearchParameters searchParameters, NetStepsEntities context);
		List<IAccount> GetAccountsByEmail(string email);
        Dictionary<int, string> SlimSearch(string query);
        AccountInformacion ListarAccountsInformacionAdicional(int AccountID);
    }

	namespace Contracts
	{
		[ContractClassFor(typeof(IAccountRepository))]
		internal abstract class AccountRepositoryContracts : IAccountRepository
		{
			Account IAccountRepository.Authenticate(string username, string password)
			{
				throw new System.NotImplementedException();
			}

			Account IAccountRepository.LoadByAccountNumberFull(string accountNumber)
			{
				throw new System.NotImplementedException();
			}

			Account IAccountRepository.LoadByAccountNumber(string accountNumber)
			{
				throw new System.NotImplementedException();
			}

			Account IAccountRepository.LoadNonProspectByEmailFull(string email)
			{
				throw new System.NotImplementedException();
			}

			Account IAccountRepository.LoadInfoCard(int accountID)
			{
				throw new System.NotImplementedException();
			}

			IEnumerable<Account> IAccountRepository.LoadInfoCardBatch(List<int> accountID)
			{
				throw new System.NotImplementedException();
			}

			int? IAccountRepository.GetNonProspectAccountIDByEmail(string email, bool? returnActive)
			{
				throw new System.NotImplementedException();
			}

			Account IAccountRepository.LoadByUserIdFull(int userID)
			{
				throw new System.NotImplementedException();
			}

            Account IAccountRepository.LoadByUserIdFull_(int userID, DateTime BirthDay)
            {
                throw new System.NotImplementedException();
            }

			AccountSlimSearchData IAccountRepository.LoadSlim(int accountID)
			{
				throw new System.NotImplementedException();
			}

			AccountSlimSearchData IAccountRepository.LoadSlimByAccountNumber(string accountNumber)
			{
				throw new System.NotImplementedException();
			}

			List<AccountSlimSearchData> IAccountRepository.LoadBatchSlim(IEnumerable<int> accountIDs)
			{
				throw new System.NotImplementedException();
			}

			List<Account> IAccountRepository.GetRecent100()
			{
				throw new System.NotImplementedException();
			}

			Dictionary<int, string> IAccountRepository.SlimSearchOnAccountStatuses(string query, int? accountTypeID, int?[] statusIDs, int? sponsorID)
			{
				throw new System.NotImplementedException();
			}

			Dictionary<int, string> IAccountRepository.SlimSearchEmail(string query, int? statusID, int? sponsorID)
			{
				throw new System.NotImplementedException();
			}

			IPaginatedList<IAccountLocatorAccountData> IAccountRepository.AccountLocatorAccountSearch(IAccountLocatorSearchParameters searchParameters)
			{
				Contract.Requires<ArgumentNullException>(searchParameters != null);

				throw new System.NotImplementedException();
			}

			IQueryable<Account> IAccountRepository.GetAccountLocatorBaseQuery(IAccountLocatorSearchParameters searchParameters, NetStepsEntities context)
			{
				Contract.Requires<ArgumentNullException>(context != null);
				Contract.Requires<ArgumentNullException>(searchParameters != null);

				throw new System.NotImplementedException();
			}

			IQueryable<IAccountLocatorAccountData> IAccountRepository.ApplyAccountLocatorAccountGeoQuery(IQueryable<Account> baseQuery, IAccountLocatorSearchParameters searchParameters, NetStepsEntities context)
			{
				Contract.Requires<ArgumentNullException>(baseQuery != null);
				Contract.Requires<ArgumentNullException>(context != null);
				Contract.Requires<ArgumentNullException>(searchParameters != null);
				Contract.Requires<ArgumentOutOfRangeException>(searchParameters.Latitude >= GeoLocation.MIN_LAT_DEGREES && searchParameters.Latitude <= GeoLocation.MAX_LAT_DEGREES);
				Contract.Requires<ArgumentOutOfRangeException>(searchParameters.Longitude >= GeoLocation.MIN_LON_DEGREES && searchParameters.Longitude <= GeoLocation.MAX_LON_DEGREES);
				Contract.Requires<ArgumentOutOfRangeException>(searchParameters.MaximumDistance > 0);

				throw new System.NotImplementedException();
			}

			IQueryable<IAccountLocatorAccountData> IAccountRepository.ApplyAccountLocatorAccountNonGeoQuery(IQueryable<Account> baseQuery, IAccountLocatorSearchParameters searchParameters, NetStepsEntities context)
			{
				Contract.Requires<ArgumentNullException>(baseQuery != null);
				Contract.Requires<ArgumentNullException>(context != null);
				Contract.Requires<ArgumentNullException>(searchParameters != null);

				throw new System.NotImplementedException();
			}

			IQueryable<IAccountLocatorAccountData> IAccountRepository.SortAccountLocatorQuery(IQueryable<IAccountLocatorAccountData> query, IAccountLocatorSearchParameters searchParameters)
			{
				Contract.Requires<ArgumentNullException>(query != null);
				Contract.Requires<ArgumentNullException>(searchParameters != null);

				throw new System.NotImplementedException();
			}

			IList<IAccountLocatorContentData> IAccountRepository.GetAccountLocatorContent(IEnumerable<int> accountIDs)
			{
				Contract.Requires<ArgumentNullException>(accountIDs != null);

				throw new System.NotImplementedException();
			}

			List<AccountSearchData> IAccountRepository.LoadBatchHeaders(List<int> primaryKeys)
			{
				throw new System.NotImplementedException();
			}

			bool IAccountRepository.ExistsByAccountNumber(string accountNumber)
			{
				throw new System.NotImplementedException();
			}

			List<AccountContactTag> IAccountRepository.AccountContactTags(int accountID)
			{
				throw new System.NotImplementedException();
			}

			PaginatedList<AuditLogRow> IAccountRepository.GetAuditLog(Account fullyLoadedAccount, AuditLogSearchParameters searchParameters)
			{
				throw new System.NotImplementedException();
			}

			AccountPaymentMethod IAccountRepository.LoadPaymentMethodAndVerifyAccount(int paymentMethodId, int accountId)
			{
				throw new System.NotImplementedException();
			}

			Address IAccountRepository.LoadAddressAndVerifyAccount(int addressId, int accountId)
			{
				throw new System.NotImplementedException();
			}

			List<AccountReport> IAccountRepository.LoadAccountReports(int accountID)
			{
				throw new System.NotImplementedException();
			}

			List<AccountReport> IAccountRepository.LoadCorporateReports()
			{
				throw new System.NotImplementedException();
			}

			bool IAccountRepository.AccountExists(string email, string currentEmail)
			{
				throw new System.NotImplementedException();
			}

			bool IAccountRepository.IsTaxNumberAvailable(string taxNumber, int? accountID)
			{
				throw new System.NotImplementedException();
			}

			bool IAccountRepository.NonProspectExists(string email, int? ignoreAccountID)
			{
				throw new System.NotImplementedException();
			}

			Account IAccountRepository.GetAccountByEmailAndSponsorID(string email, int sponsorId, bool enableTracking, bool? getActive)
			{
				throw new System.NotImplementedException();
			}

			Account IAccountRepository.LoadAccountByEmailAndAccountType(string email, Generated.ConstantsGenerated.AccountType accountType)
			{
				throw new System.NotImplementedException();
			}

			Account IAccountRepository.LoadAccountByUserId(int userId)
			{
				throw new System.NotImplementedException();
			}

			void IAccountRepository.SaveNote(int accountId, Note note)
			{
				throw new System.NotImplementedException();
			}

			Dictionary<int, string> IAccountRepository.SearchAccountsByText(string key)
			{
				throw new System.NotImplementedException();
			}

			Dictionary<int, string> IAccountRepository.SearchAccountsByTextAccountTypeAndSponsorId(string text, int accountTypeId, int sponsorId)
			{
				throw new System.NotImplementedException();
			}

			Dictionary<int, string> IAccountRepository.SearchAccountsByTextAndAccountType(string text, int accountTypeId)
			{
				throw new System.NotImplementedException();
			}

			Dictionary<int, string> IAccountRepository.SearchAccountsByTextAndAccountStatus(string text, int accountStatusId)
			{
				throw new System.NotImplementedException();
			}

			Dictionary<int, string> IAccountRepository.SearchAccountsByTextAccountTypeAndAccountStatus(string text, int accountTypeId, int accountStatusId)
			{
				throw new System.NotImplementedException();
			}

			public IQueryable<Account> GetAccountSearchQuery(AccountSearchParameters searchParameters, NetStepsEntities context)
			{
				throw new NotImplementedException();
			}

			public IQueryable<AccountInfoCache> GetAccountInfoSearchQuery(AccountSearchParameters searchParameters, NetStepsEntities context)
			{
				throw new NotImplementedException();
			}

			public List<IAccount> GetAccountsByEmail(string email)
			{
				throw new NotImplementedException();
			}

			NetSteps.Common.PrimaryKeyInfo IBaseRepository<Account, int>.PrimaryKeyInfo
			{
				get { throw new System.NotImplementedException(); }
			}

			Account IBaseRepository<Account, int>.Load(int primaryKey)
			{
				throw new System.NotImplementedException();
			}

			Account IBaseRepository<Account, int>.LoadFull(int primaryKey)
			{
				throw new System.NotImplementedException();
			}

			List<Account> IBaseRepository<Account, int>.LoadAll()
			{
				throw new System.NotImplementedException();
			}

			List<Account> IBaseRepository<Account, int>.LoadAllFull()
			{
				throw new System.NotImplementedException();
			}

			List<Account> IBaseRepository<Account, int>.LoadBatch(IEnumerable<int> primaryKeys)
			{
				throw new System.NotImplementedException();
			}

			List<Account> IBaseRepository<Account, int>.LoadBatchFull(IEnumerable<int> primaryKeys)
			{
				throw new System.NotImplementedException();
			}

			SqlUpdatableList<Account> IBaseRepository<Account, int>.LoadAllFullWithSqlDependency()
			{
				throw new System.NotImplementedException();
			}

			SqlUpdatableList<Account> IBaseRepository<Account, int>.LoadBatchWithSqlDependency(IEnumerable<int> primaryKeys)
			{
				throw new System.NotImplementedException();
			}

			void IBaseRepository<Account, int>.Save(Account obj)
			{
				throw new System.NotImplementedException();
			}

			void IBaseRepository<Account, int>.SaveBatch(IEnumerable<Account> items)
			{
				throw new System.NotImplementedException();
			}

			void IBaseRepository<Account, int>.Delete(Account obj)
			{
				throw new System.NotImplementedException();
			}

			void IBaseRepository<Account, int>.Delete(int primaryKey)
			{
				throw new System.NotImplementedException();
			}

			void IBaseRepository<Account, int>.DeleteBatch(IEnumerable<int> primaryKeys)
			{
				throw new System.NotImplementedException();
			}

			bool IBaseRepository<Account, int>.Exists(int primaryKey)
			{
				throw new System.NotImplementedException();
			}

			PaginatedList<AuditLogRow> IBaseRepository<Account, int>.GetAuditLog(int primaryKey, AuditLogSearchParameters param)
			{
				throw new System.NotImplementedException();
			}

			Account IBaseRepository<Account, int>.GetRandomRecord()
			{
				throw new System.NotImplementedException();
			}

			Account IBaseRepository<Account, int>.GetRandomRecordFull()
			{
				throw new System.NotImplementedException();
			}

			int IBaseRepository<Account, int>.Count()
			{
				throw new System.NotImplementedException();
			}

			int IBaseRepository<Account, int>.Count(System.Linq.Expressions.Expression<System.Func<Account, bool>> predicate)
			{
				throw new System.NotImplementedException();
			}

			bool IBaseRepository<Account, int>.Any()
			{
				throw new System.NotImplementedException();
			}

			bool IBaseRepository<Account, int>.Any(System.Linq.Expressions.Expression<System.Func<Account, bool>> predicate)
			{
				throw new System.NotImplementedException();
			}

			List<Account> IBaseRepository<Account, int>.Where(System.Linq.Expressions.Expression<System.Func<Account, bool>> predicate)
			{
				throw new System.NotImplementedException();
			}

			List<Account> IBaseRepository<Account, int>.Where(System.Linq.Expressions.Expression<System.Func<Account, bool>> predicate, IEnumerable<string> includes)
			{
				throw new System.NotImplementedException();
			}

			List<TSelected> IBaseRepository<Account, int>.WhereSelect<TSelected>(System.Linq.Expressions.Expression<System.Func<Account, bool>> predicate, System.Linq.Expressions.Expression<System.Func<Account, TSelected>> selector)
			{
				throw new System.NotImplementedException();
			}

			Account IBaseRepository<Account, int>.FirstOrDefault(System.Linq.Expressions.Expression<System.Func<Account, bool>> predicate)
			{
				throw new System.NotImplementedException();
			}

			Account IBaseRepository<Account, int>.FirstOrDefault(System.Linq.Expressions.Expression<System.Func<Account, bool>> predicate, IEnumerable<string> includes)
			{
				throw new System.NotImplementedException();
			}

			TSelected IBaseRepository<Account, int>.FirstOrDefaultSelect<TSelected>(System.Linq.Expressions.Expression<System.Func<Account, bool>> predicate, System.Linq.Expressions.Expression<System.Func<Account, TSelected>> selector)
			{
				throw new System.NotImplementedException();
			}

			PaginatedList<AccountSearchData> ISearchRepository<AccountSearchParameters, PaginatedList<AccountSearchData>>.Search(AccountSearchParameters searchParams)
			{
				throw new System.NotImplementedException();
			}

			IQueryable<Account> IAccountRepository.GetAccountSearchQuery(AccountSearchParameters searchParameters, NetStepsEntities context)
			{
				throw new NotImplementedException();
			}

			IQueryable<AccountInfoCache> IAccountRepository.GetAccountInfoSearchQuery(AccountSearchParameters searchParameters, NetStepsEntities context)
			{
				throw new NotImplementedException();
			}

			List<IAccount> IAccountRepository.GetAccountsByEmail(string email)
			{
				throw new NotImplementedException();
			}

			Account IAccountRepository.LoadWithRelationsByAccountID(int accountID, params string[] relations)
			{
				throw new NotImplementedException();
			}

			Account IAccountRepository.LoadWithRelationsByUserID(int userID, params string[] relations)
			{
				throw new NotImplementedException();
			}

			Account IAccountRepository.LoadWithRelationsByEmailAddress(string email, bool includeProspects, params string[] relations)
			{
				throw new NotImplementedException();
			}

			#region IAccountRepository Members


			Account IAccountRepository.LoadWithRelationsByAccountNumber(string accountNumber, params string[] relations)
			{
				throw new NotImplementedException();
			}

			#endregion

            Dictionary<int, string> IAccountRepository.SlimSearch(string query)
            {
                throw new System.NotImplementedException();
            }


            public AccountInformacion ListarAccountsInformacionAdicional(int AccountID)
            {
                throw new NotImplementedException();
            }
        }
	}
}
