using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Common.Interfaces;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Generated;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Encore.Core.IoC;
using NetSteps.Enrollment.Common.Models.Context;
using NetSteps.Security;
using NetSteps.SSO.Common;
using System.Data;
using System.Data.SqlClient;

namespace NetSteps.Data.Entities
{
    public partial class Account : IUser
    {
        /// <summary>
        /// Related entities that can be included by the 'Load' methods (see <see cref="LoadRelationsExtensions"/>).
        /// WARNING: Changes to this list will affect various 'Load' methods, be careful.
        /// </summary>
        [Flags]
        public enum Relations
        {
            // These are bit flags so they must be numbered appropriately (eg. 0, 1, 2, 4, 8, 16)
            // Use bit-shifting for convenience (eg. 0, 1 << 0, 1 << 1, 1 << 2)
            None = 0,
            AccountDevices = 1 << 0,
            AccountPaymentMethods = 1 << 1,
            AccountPaymentMethods_BillingAddress = 1 << 2,
            AccountPhones = 1 << 3,
            AccountProperties = 1 << 4,
            Addresses = 1 << 5,
            CampaignSubscribers = 1 << 6,
            DistributionSubscribers = 1 << 7,
            FileResources = 1 << 8,
            MailAccounts = 1 << 9,
            Notes = 1 << 10,
            User = 1 << 11,
            User_Roles = 1 << 12,
            User_Roles_Functions = 1 << 13,
            Addresses_AddressProperties = 1 << 14,
            AccountContactTags = 1 << 15,

            /// <summary>
            /// The default relations used by the 'LoadFull' methods.
            /// </summary>
            LoadFull = AccountDevices | AccountPaymentMethods | AccountPaymentMethods_BillingAddress
                | AccountPhones | AccountProperties | Addresses | CampaignSubscribers | DistributionSubscribers
                | FileResources | MailAccounts | Notes | User | User_Roles | User_Roles_Functions | Addresses_AddressProperties | AccountContactTags,

            LoadInfoCard = AccountPhones | AccountProperties | MailAccounts
        };

        #region Members
        private int _productPriceTypeID = 0;
        private int _taxPriceTypeID = 0;
        private int _commissionPriceTypeID = 0;
        private string _decryptedTaxNumber = string.Empty;
        private string _cpf = string.Empty;
        private string _pis = string.Empty;

        #endregion

        #region Properties
        // Will be the Culture of the Country on their Main Address if they have one; else their shipping address; else the applications default culture. - JHE
        public CultureInfo AccountCultureInfo
        {
            get
            {
                string countryCultureInfoCode = ApplicationContext.Instance.ApplicationDefaultCultureInfo.Name;

                var mainAddress = Addresses.GetAllByTypeID(ConstantsGenerated.AddressType.Main).FirstOrDefault();
                if (mainAddress != null && mainAddress.CountryID > 0)
                {
                    var country = SmallCollectionCache.Instance.Countries.GetById(mainAddress.CountryID);
                    if (country != null)
                        countryCultureInfoCode = country.CultureInfo;
                }
                if (countryCultureInfoCode.IsNullOrEmpty())
                {
                    var shippingAddress = Addresses.GetAllByTypeID(ConstantsGenerated.AddressType.Shipping).FirstOrDefault();
                    if (shippingAddress != null && shippingAddress.CountryID > 0)
                    {
                        var country = SmallCollectionCache.Instance.Countries.GetById(shippingAddress.CountryID);
                        countryCultureInfoCode = country.CultureInfo;
                    }
                }

                return CultureInfoCache.GetCultureInfo(countryCultureInfoCode);
            }
        }

        public string FullName
        {
            get { return Account.ToFullName(FirstName, string.Empty, LastName, AccountCultureInfo.Name); }
        }

        public int ProductPriceTypeID
        {
            get
            {
                if (_productPriceTypeID == 0)
                {
                    var productPriceType = AccountPriceType.GetPriceType(this.AccountTypeID, ConstantsGenerated.PriceRelationshipType.Products, null);
                    _productPriceTypeID = (productPriceType != null) ? productPriceType.ProductPriceTypeID : 0;
                }
                return _productPriceTypeID;
            }
        }

        public int TaxPriceTypeID
        {
            get
            {
                if (_taxPriceTypeID == 0)
                    _taxPriceTypeID = AccountPriceType.GetPriceType(this.AccountTypeID, ConstantsGenerated.PriceRelationshipType.Taxes, null).ProductPriceTypeID;
                return _taxPriceTypeID;
            }
        }

        public int CommissionPriceTypeID
        {
            get
            {
                if (_commissionPriceTypeID == 0)
                    _commissionPriceTypeID = AccountPriceType.GetPriceType(this.AccountTypeID, ConstantsGenerated.PriceRelationshipType.Commissions, null).ProductPriceTypeID;
                return _commissionPriceTypeID;
            }
        }

        public string DecryptedTaxNumber
        {
            get
            {
                if (_decryptedTaxNumber.IsNullOrEmpty())
                    _decryptedTaxNumber = Encryption.DecryptTripleDES(_taxNumber);
                //_decryptedTaxNumber = _taxNumber;

                return _decryptedTaxNumber;
            }
            set
            {
                _decryptedTaxNumber = value ?? string.Empty;

                if (!string.IsNullOrEmpty(value))
                    TaxNumber = Encryption.EncryptTripleDES(value);
                //TaxNumber = value;
            }
        }

        public string MaskedTaxNumber
        {
            get
            {
                return DecryptedTaxNumber.MaskString(4);
            }
        }

        public string MainPhone
        {
            get
            {
                return GetFirstPhone(Constants.PhoneType.Main);
            }
            set
            {
                SetFirstPhone(Constants.PhoneType.Main, value);
            }
        }

        public string CellPhone
        {
            get
            {
                return GetFirstPhone(Constants.PhoneType.Cell);
            }
            set
            {
                SetFirstPhone(Constants.PhoneType.Cell, value);
            }
        }

        public string HomePhone
        {
            get
            {
                return GetFirstPhone(Constants.PhoneType.Home);
            }
            set
            {
                SetFirstPhone(Constants.PhoneType.Home, value);
            }
        }

        public string WorkPhone
        {
            get
            {
                return GetFirstPhone(Constants.PhoneType.Work);
            }
            set
            {
                SetFirstPhone(Constants.PhoneType.Work, value);
            }
        }

        public string Fax
        {
            get
            {
                return GetFirstPhone(Constants.PhoneType.Fax);
            }
            set
            {
                SetFirstPhone(Constants.PhoneType.Fax, value);
            }
        }

        public string TextPhone
        {
            get
            {
                return GetFirstPhone(Constants.PhoneType.Text);
            }
            set
            {
                SetFirstPhone(Constants.PhoneType.Text, value);
            }
        }
        public string PagerPhone
        {
            get
            {
                return GetFirstPhone(Constants.PhoneType.Pager);
            }
            set
            {
                SetFirstPhone(Constants.PhoneType.Pager, value);
            }
        }

        private string GetFirstPhone(Constants.PhoneType phoneType)
        {
            return this.AccountPhones.Count(n => n.PhoneTypeID == (int)phoneType) > 0 ? this.AccountPhones.First(n => n.PhoneTypeID == (int)phoneType).PhoneNumber : string.Empty;
        }
        private void SetFirstPhone(Constants.PhoneType phoneType, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (this.AccountPhones.Count(n => n.PhoneTypeID == (int)phoneType) > 0)
                {
                    AccountPhone phone = this.AccountPhones.First(n => n.PhoneTypeID == (int)phoneType);
                    phone.PhoneNumber = value;
                }
                else
                {
                    AccountPhone newPhone = new AccountPhone();
                    newPhone.StartTracking();
                    newPhone.AccountID = this.AccountID;
                    newPhone.PhoneTypeID = (int)phoneType;
                    newPhone.PhoneNumber = value;
                    this.AccountPhones.Add(newPhone);
                }
            }
        }

        public bool Active
        {
            get
            {
                return AccountStatusID == (int)Constants.AccountStatus.Active;
            }
        }

        //set the account status to active and set the enrollment date to now if empty or resetEnrollmentDate = true  - Scott Wilson
        public void Activate(bool resetEnrollmentDate = false)
        {
            AccountStatusID = (int)Constants.AccountStatus.Active;

            if (!EnrollmentDate.HasValue || resetEnrollmentDate)
                EnrollmentDate = DateTime.Now;
        }

        private AccountSlimSearchData _sponsorInfo;
        public AccountSlimSearchData SponsorInfo
        {
            get
            {
                if ((_sponsorInfo == null && this.SponsorID != null && this.SponsorID > 0) || (_sponsorInfo != null && this.SponsorID != null && this.SponsorID > 0 && _sponsorInfo.AccountID != this.SponsorID))
                    _sponsorInfo = Account.LoadSlim(this.SponsorID.ToInt());
                return _sponsorInfo;
            }
        }

        private AccountSlimSearchData _enrollerInfo;
        public AccountSlimSearchData EnrollerInfo
        {
            get
            {
                if ((_enrollerInfo == null && this.EnrollerID != null && this.EnrollerID > 0) || (_enrollerInfo != null && this.EnrollerID != null && this.EnrollerID > 0 && _enrollerInfo.AccountID != this.EnrollerID))
                    _enrollerInfo = Account.LoadSlim(this.EnrollerID.ToInt());
                return _enrollerInfo;
            }
        }

        public AccountContactTag AccountContactTag
        {
            get
            {
                return this.AccountContactTags.FirstOrDefault();
            }
            set
            {
                if (this.AccountContactTags.Count == 0)
                    this.AccountContactTags.Add(value);
            }
        }


        private bool _isAccountPublicContactInfoLoaded = false; // To load reduce calls when the Account doesn't have AccountPublicContactInfo - JHE
        private AccountPublicContactInfo _accountPublicContactInfo;
        public AccountPublicContactInfo AccountPublicContactInfo
        {
            get
            {
                if (_accountPublicContactInfo == null && !_isAccountPublicContactInfoLoaded)
                {
                    _accountPublicContactInfo = AccountPublicContactInfo.LoadByAccountID(this.AccountID);
                    _isAccountPublicContactInfoLoaded = true;
                }
                return _accountPublicContactInfo;
            }
            set
            {
                _accountPublicContactInfo = value;
                _accountPublicContactInfo.Account = this;
                _accountPublicContactInfo.AccountID = this.AccountID;
            }
        }

        public bool IsTempAccount { get; set; }

        private bool? _isOptedOut;
        public bool IsOptedOut
        {
            get
            {
                if (_isOptedOut == null)
                {
                    OptOut optout = OptOut.Search(EmailAddress);

                    _isOptedOut = optout.IsNotNull();
                }

                return _isOptedOut.Value;
            }
            set { _isOptedOut = value; }

        }
        #endregion

        #region Methods

        public List<PaymentType> GetNonProfilePaymentTypes(IUser user, int orderTypeID)
        {
            try
            {
                using (new ApplicationUsageLogger(new ExecutionContext(this)))
                {
                    int? accountTypeID = null;
                    if (user is Account)
                        accountTypeID = (user as Account).AccountTypeID;

                    var result = SmallCollectionCache.Instance.PaymentTypes
                        .Where(p => p.PaymentTypeID != (int)Constants.PaymentType.CreditCard
                            && p.PaymentTypeID != (int)Constants.PaymentType.EFT
                            && (!string.IsNullOrWhiteSpace(p.FunctionName) ? user.HasFunction(p.FunctionName, accountTypeID: accountTypeID) : false)
                            && (SmallCollectionCache.Instance.PaymentOrderTypes.Any(pot => pot.PaymentTypeID == p.PaymentTypeID && pot.OrderTypeID == orderTypeID))).ToList();

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public List<PaymentType> GetNonProfilePaymentTypes(int accountTypeID, int orderTypeID, int shippingCountryID)
        {
            try
            {
                AccountType accountType = SmallCollectionCache.Instance.AccountTypes.GetById(accountTypeID.ToShort());
                using (new ApplicationUsageLogger(new ExecutionContext(this)))
                {
                    List<PaymentType> result = new List<PaymentType>();
                    var paymentTypes = SmallCollectionCache.Instance.PaymentTypes.Where(p => p.PaymentTypeID != (int)Constants.PaymentType.CreditCard && p.PaymentTypeID != (int)Constants.PaymentType.EFT).ToList();

                    foreach (var paymentType in paymentTypes)
                    {
                        foreach (var role in accountType.Roles)
                        {
                            if (role.HasFunction(paymentType.FunctionName))
                            {
                                if (!result.Contains(paymentType))
                                {
                                    if (SmallCollectionCache.Instance.PaymentOrderTypes.Any(x => x.PaymentTypeID == paymentType.PaymentTypeID
                                        && x.OrderTypeID == orderTypeID && x.CountryID == shippingCountryID))
                                    {
                                        result.Add(paymentType);
                                    }
                                }
                            }
                        }
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static void SendGeneratedPasswordEmail(Account account)
        {
            try
            {
                using (new ApplicationUsageLogger(new ExecutionContext(account)))
                {
                    BusinessLogic.SendGeneratedPasswordEmail(account);
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static PaginatedList<AccountSearchData> Search(AccountSearchParameters searchParameters)
        {
            try
            {
                return BusinessLogic.Search(Repository, searchParameters);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        /// <summary>
        /// Returns the account id as the key and first name, last name, and account number as the value
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static Dictionary<int, string> SlimSearchOnAccountStatuses(string query, int? accountTypeID = null, int?[] statusIDs = null, int? sponsorID = null)
        {
            try
            {
                return BusinessLogic.SlimSearchOnAccountStatuses(Repository, query, accountTypeID, statusIDs, sponsorID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static Dictionary<int, string> SlimSearchEmail(string query, int? statusID = null, int? sponsorID = null)
        {
            try
            {
                return BusinessLogic.SlimSearchEmail(Repository, query, statusID, sponsorID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static List<Account> GetRecent100()
        {
            try
            {
                return Repository.GetRecent100();
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static Account GetByUserId(int userId)
        {
            try
            {
                return BusinessLogic.LoadAccountByUserId(Repository, userId);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static Account LoadByAccountNumberFull(string accountNumber)
        {
            try
            {
                return BusinessLogic.LoadByAccountNumberFull(Repository, accountNumber);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static Account LoadInfoCard(int accountNumber)
        {
            try
            {
                return BusinessLogic.LoadInfoCard(Repository, accountNumber);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static IEnumerable<Account> LoadInfoCardBatch(List<int> accountNumbers)
        {
            try
            {
                return BusinessLogic.LoadInfoCardBatch(Repository, accountNumbers);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static Account LoadByAccountNumber(string accountNumber)
        {
            try
            {
                return BusinessLogic.LoadByAccountNumber(Repository, accountNumber);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static string GetStatusForCommissionQualified(bool? isCommissionQualifed, DateTime? autoshipProcessDate)
        {
            return BusinessLogic.GetIsCommissionQualifiedStatus(isCommissionQualifed, autoshipProcessDate);
        }

        public static Account LoadNonProspectByEmailFull(string email)
        {
            try
            {
                return Repository.LoadNonProspectByEmailFull(email);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static int? GetNonProspectAccountIDByEmail(string email, bool? returnActive = true)
        {
            try
            {
                return Repository.GetNonProspectAccountIDByEmail(email, returnActive);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static AccountSlimSearchData LoadSlim(int accountID)
        {
            try
            {
                return BusinessLogic.LoadSlim(Repository, accountID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static AccountSlimSearchData LoadSlimByAccountNumber(string accountNumber)
        {
            try
            {
                return BusinessLogic.LoadSlimByAccountNumber(Repository, accountNumber);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static List<AccountSlimSearchData> LoadBatchSlim(IEnumerable<int> accountIDs)
        {
            try
            {
                return Repository.LoadBatchSlim(accountIDs);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static Dictionary<int, string> SlimSearch(string query)
        {
            try
            {
                return BusinessLogic.SlimSearch(Repository, query);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public override void Save()
        {
            try
            {
                //Cleaned up the logic here so that base.save() is not called more often than absolutely needed - Scott Wilson

                //If no account id, This is first save
                if (AccountID == 0)
                {
                    //Pre-save in order to generate entity PK ID - Scott Wilson
                    base.Save();
                    //This method checks for an empty or temporary account number before setting a new one
                    //Calling it here immediately after first save so default account number mechanism has
                    //an account id to work with - Scott Wilson
                    BusinessLogic.GenerateAndSetNewAccountNumber(this);
                }

                //Do a final save with valid generated account number - Scott Wilson
                base.Save();
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException, null, this.AccountID.ToIntNullable());
            }
        }

        public static Account Authenticate(string username, string password)
        {
            try
            {
                return BusinessLogic.Authenticate(Repository, username, password);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static IPaginatedList<IAccountLocatorSearchData> AccountLocatorSearch(IAccountLocatorSearchParameters searchParameters)
        {
            return BusinessLogic.AccountLocatorSearch(Repository, searchParameters);
        }

        public static List<AccountSearchData> LoadBatchHeaders(List<int> accountIDs)
        {
            try
            {
                return BusinessLogic.LoadBatchHeaders(Repository, accountIDs);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static int GetCount()
        {
            try
            {
                return Repository.Count();
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        /// <summary>
        /// Use to get an encrypted, URL-encoded Single-Sign-On token for use in URLs. - JHE
        /// </summary>
        public static string GetSingleSignOnToken(int accountID)
        {
            return GetSingleSignOnToken(accountID, true);
        }

        /// <summary>
        /// Generates an encrypted Single-Sign-On token, and optionally URL-encodes the string.
        /// </summary>
        public static string GetSingleSignOnToken(int accountID, bool urlEncode = true)
        {
            var ssoService = Create.New<IAuthenticator>();
            var ssoModel = Create.New<ISingleSignOnTimeStampedModel>();
            ssoModel.DecodedText = accountID.ToString();
            ssoModel.TimeStamp = DateTime.UtcNow;
            ssoService.Encode(ssoModel);
            string singleSignOnToken = ssoModel.EncodedText;

            return urlEncode ? HttpUtility.UrlEncode(singleSignOnToken) : singleSignOnToken;
        }

        /// <summary>
        /// Use to get an decrypted Single-Sign-On token and return the AccountID to Sign-In. - JHE
        /// </summary>
        /// <param name="singleSignOnToken"></param>
        /// <returns></returns>
        public static int GetAccountIdFromSingleSignOnToken(string singleSignOnToken, TimeSpan? expiration = null)
        {
            try
            {
                var ssoService = Create.New<IAuthenticator>();
                var ssoModel = Create.New<ISingleSignOnTimeStampedModel>();
                ssoModel.EncodedText = singleSignOnToken;
                ssoService.Decode(ssoModel);
                int accountID =
                    ssoModel.DecodedText.IsValidInt()
                        && (expiration == null || ssoModel.TimeStamp.Add(expiration.Value) >= DateTime.UtcNow)
                    ? ssoModel.DecodedText.ToInt() : 0;
                return accountID;
            }
            catch (Exception ex)
            {
                EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
                return 0;
            }
        }

        public void BuildReadOnlyNotesTree()
        {
            try
            {
                BusinessLogic.BuildReadOnlyNotesTree(this);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        internal static Account GetRandomRecord()
        {
            try
            {
                return Repository.GetRandomRecord();
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        internal static Account GetRandomRecordFull()
        {
            try
            {
                return Repository.GetRandomRecordFull();
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static PaginatedList<AuditLogRow> GetAuditLog(Account fullyLoadedAccount, int accountID, AuditLogSearchParameters param)
        {
            try
            {
                if (fullyLoadedAccount != null)
                    return BusinessLogic.GetAuditLog(Repository, fullyLoadedAccount, param);
                else
                    return BusinessLogic.GetAuditLog(Repository, accountID, param);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException, null, (fullyLoadedAccount != null) ? fullyLoadedAccount.AccountID : fullyLoadedAccount.AccountID.ToIntNullable());
            }
        }
        public static PaginatedList<AuditLogRow> GetAuditLog(Account fullyLoadedAccount, AuditLogSearchParameters param)
        {
            try
            {
                return BusinessLogic.GetAuditLog(Repository, fullyLoadedAccount, param);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException, null, (fullyLoadedAccount != null) ? fullyLoadedAccount.AccountID : fullyLoadedAccount.AccountID.ToIntNullable());
            }
        }

        [Obsolete("Account now has a MarketID property.")]
        public int? GetMarketID()
        {
            int? marketId = null;
            var mainAddress = this.Addresses.GetDefaultByTypeID(Constants.AddressType.Main);
            if (mainAddress != null)
                marketId = SmallCollectionCache.Instance.Countries.GetById(mainAddress.CountryID).MarketID;
            return marketId;
        }

        public static Account EnrollRetailCustomer(int sponsorID, int languageID, string firstName, string lastName, string email, string username, string password, short accountType)
        {
            try
            {
                return BusinessLogic.EnrollRetailCustomer(Repository, sponsorID, languageID, firstName, lastName, email, username, password, accountType);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static Account EnrollExpressRetailCustomer(int sponsorID, int languageID, string firstName, string lastName, string email, string username, short accountTypeID)
        {
            try
            {
                return BusinessLogic.EnrollExpressRetailCustomer(Repository, sponsorID, languageID, firstName, lastName, email, username, accountTypeID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static Account EnrollRetailCustomerFromTempAccount(Account account, int sponsorID, int languageID, string firstName, string lastName, string email, string username, string password)
        {
            try
            {
                return BusinessLogic.EnrollRetailCustomerFromTempAccount(Repository, account, sponsorID, languageID, firstName, lastName, email, username, password);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static AccountPaymentMethod LoadPaymentMethodAndVerifyAccount(int paymentMethodID, int accountID)
        {
            try
            {
                return BusinessLogic.LoadPaymentMethodAndVerifyAccount(Repository, paymentMethodID, accountID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static Address LoadAddressAndVerifyAccount(int addressID, int accountID)
        {
            try
            {
                return BusinessLogic.LoadAddressAndVerifyAccount(Repository, addressID, accountID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static List<AccountReport> LoadAccountReports(int accountID)
        {
            try
            {
                return BusinessLogic.LoadAccountReports(Repository, accountID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static List<AccountReport> LoadCorporateReports()
        {
            try
            {
                return BusinessLogic.LoadCorporateReports(Repository);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static bool AccountExists(string email, string currentEmail = null)
        {
            try
            {
                if (!string.IsNullOrEmpty(currentEmail) && email.ToLower() == currentEmail.ToLower())
                    return false;
                return Repository.AccountExists(email, currentEmail);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public bool IsTaxNumberAvailable(string taxNumber)
        {
            try
            {
                if (taxNumber.ToCleanString().IsNullOrEmpty())
                    return true;
                return Repository.IsTaxNumberAvailable(taxNumber, this.AccountID.ToIntNullable());
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
        public bool EnforceUniqueTaxNumber(string countryCultureInfoCode = null)
        {
            try
            {
                return BusinessLogic.EnforceUniqueTaxNumber(Repository, this, countryCultureInfoCode);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static bool NonProspectNonExpressAccountExists(string email)
        {
            return NonProspectNonExpressAccountExists(email, 0);

        }

        public static bool NonProspectNonExpressAccountExists(string email, int accountIDToIgnore)
        {
            var accounts = Repository.GetAccountsByEmail(email);
            bool accountExists = accounts.Any(a => a.AccountTypeID != (int)Constants.AccountType.Prospect
                                                && a.AccountID != accountIDToIgnore);

            return accountExists;
        }

        public static bool NonProspectExists(string email, int? ignoreAccountID = null)
        {
            try
            {
                return Repository.NonProspectExists(email, ignoreAccountID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static Account GetAccountByEmailAndSponsorID(string email, int sponsorId, bool enableTracking = false, bool? getActive = null)
        {
            try
            {
                return Repository.GetAccountByEmailAndSponsorID(email, sponsorId, enableTracking, getActive);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public virtual void DeleteAccountPaymentMethod(int paymentMethodId)
        {
            try
            {
                BusinessLogic.DeleteAccountPaymentMethod(this, paymentMethodId);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public virtual void DeleteAccountAddress(int addressId)
        {
            try
            {
                BusinessLogic.DeleteAccountAddress(this, addressId);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public virtual bool IsInUpline(Account childAccount)
        {
            try
            {
                return BusinessLogic.IsInUpline(this, childAccount);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static Dictionary<int, string> DownlineSearch(string query, int periodID, int baseAccountID, List<int> accountTypes, List<int> accountStatuses, int? maxResults = null, int? maxLevel = null)
        {
            try
            {
                return BusinessLogic.DownlineSearch(query, periodID, baseAccountID, accountTypes, accountStatuses, maxResults, maxLevel);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        #endregion

        #region IUser Members

        int IUser.UserID
        {
            get
            {
                if (User == null)
                    return 0;
                else
                    return User.UserID;
            }
            set
            {
                User.UserID = value;
            }
        }

        short IUser.UserTypeID
        {
            get
            {
                return User.UserTypeID;
            }
            set
            {
                User.UserTypeID = value;
            }
        }

        short IUser.UserStatusID
        {
            get
            {
                return User.UserStatusID;
            }
            set
            {
                User.UserStatusID = value;
            }
        }

        string IUser.Username
        {
            get
            {

                return User == null ? null : User.Username;
            }
            set
            {
                User.Username = value;
            }
        }

        string IUser.Password
        {
            set { User.Password = value; }
        }

        string IUser.PasswordHash
        {
            get
            {
                return User.PasswordHash;
            }
            set
            {
                User.PasswordHash = value;
            }
        }

        string IUser.FirstName
        {
            get
            {
                return this.FirstName;
            }
            set
            {
                this.FirstName = value;
            }
        }

        string IUser.LastName
        {
            get
            {
                return this.LastName;
            }
            set
            {
                this.LastName = value;
            }
        }

        string IUser.EmailAddress
        {
            get
            {
                return this.EmailAddress;
            }
            set
            {
                this.EmailAddress = value;
            }
        }

        int IUser.LanguageID
        {
            get
            {
                return this.DefaultLanguageID;
            }
            set
            {
                this.DefaultLanguageID = value;
            }
        }

        bool IUser.HasFunction(string function)
        {
            return User.HasFunction(function);
        }

        public bool HasFunction(string function, bool checkAnonymousRole = true, bool checkWorkstationUserRole = false, int? accountTypeID = null)
        {
            if (checkAnonymousRole)
            {
                Role anonymousRole = ApplicationContext.Instance.AnonymousRole;
                if (anonymousRole != null && anonymousRole.HasFunction(function))
                {
                    return true;
                }
            }

            if (checkWorkstationUserRole)
            {
                Role workstationUserRole = ApplicationContext.Instance.WorkstationUserRole;
                if (workstationUserRole != null && workstationUserRole.HasFunction(function))
                {
                    return true;
                }
            }

            if (User == null)
            {
                return false;
            }

            return User.HasFunction(function, checkAnonymousRole, checkWorkstationUserRole, accountTypeID);
        }

        #endregion

        /// <summary>
        /// Check if account has file resources
        /// </summary>
        /// <param name="primaryKey">account ID</param>
        /// <returns>bool</returns>
        public static bool HasFileResources(int primaryKey)
        {
            try
            {
                AccountRepository accountRepository = new AccountRepository();
                return accountRepository.HasFileResources(primaryKey);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        /// <summary>
        /// Get account properties for particular account
        /// </summary>
        /// <param name="primaryKey">account id</param>
        /// <returns>Account Properties</returns>
        public static Account LoadAccountProperties(int primaryKey)
        {
            try
            {
                AccountRepository accountRepository = new AccountRepository();
                return accountRepository.GetAccountProperties(primaryKey);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        /// <summary>
        /// Get sponsored accounts
        /// </summary>
        /// <param name="primaryKey">accountID</param>
        /// <param name="offset">int - offset</param>
        /// <returns>IEnumerable<Account></returns>
        public static IEnumerable<Account> LoadSponsoredAccount(int primaryKey, int offset)
        {
            try
            {
                AccountRepository accountRepository = new AccountRepository();
                return accountRepository.LoadSponsoredAccount(primaryKey, offset);
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
        public static List<Account> ImportProspects(string csvContents)
        {
            try
            {
                return BusinessLogic.ImportProspects(csvContents);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static string ToFullName(string firstName, string middleName, string lastName, string countryCultureInfoCode)
        {
            try
            {
                return BusinessLogic.ToFullName(firstName, middleName, lastName, countryCultureInfoCode);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static string DisplayPhone(string phone, string countryCultureInfoCode)
        {
            try
            {
                return BusinessLogic.DisplayPhone(phone, countryCultureInfoCode);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static string GenerateUsername(Account account)
        {
            return account.FirstName[0] + account.LastName + account.AccountID;
        }

        public static string GenerateRandomPassword()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }

        public static void LoadAddresses(Account account)
        {
            using (NetStepsEntities context = new NetStepsEntities())
            {
                account.LoadRelations(context, Relations.Addresses);
            }
        }
        //
        // Changed to public so we could load an account with just an e-mail address
        public static Account LoadAccountByEmailAndAccountType(string email, Constants.AccountType accountType)
        {
            return BusinessLogic.LoadAccountByEmailAndAccountType(Repository, email, accountType);
        }

        private static string[] getSessionRelations()
        {
            return new string[] {"AccountPaymentMethods",
				"AccountPaymentMethods.BillingAddress",
				"AccountPhones",
				"AccountProperties",
				"Addresses",
				"CampaignSubscribers",
				"DistributionSubscribers",
				"FileResources",
				"MailAccounts",
				"User",
				"User.Roles",
				"User.Roles.Functions",
				"Addresses.AddressProperties",
				"AccountContactTags"};
        }

        public static Account LoadForSession(int accountID)
        {
            return Repository.LoadWithRelationsByAccountID(accountID, getSessionRelations());
        }

        public static Account LoadForSession(string emailAddress, bool includeProspects)
        {
            return Repository.LoadWithRelationsByEmailAddress(emailAddress, includeProspects,
                getSessionRelations());
        }

        public static Account LoadForSessionByUserID(int userID)
        {
            return Repository.LoadWithRelationsByUserID(userID,
                getSessionRelations());
        }

        public static Account LoadForSessionByAccountNumber(string accountNumber)
        {
            return Repository.LoadWithRelationsByAccountNumber(accountNumber, getSessionRelations());
        }

        internal static bool ExistingRetailAccountCanBeUpdated(Account account)
        {
            return (
                    account != null &&
                    account.AccountTypeID == (int)Constants.AccountType.RetailCustomer &&
                    account.User != null &&
                    account.User.TotalLoginCount == 0 &&
                    account.AccountPaymentMethods.IsNullOrEmpty() &&
                    account.Addresses.IsNullOrEmpty()
                );
        }

        internal static bool ExistingAccountCanBeUpdated(Account account)
        {
            return (
                    account != null &&
                    (account.AccountTypeID == (int)Constants.AccountType.RetailCustomer || account.AccountTypeID == (int)Constants.AccountType.PreferredCustomer) &&
                    account.User != null &&
                    account.User.TotalLoginCount == 0
                );
        }

        public void SaveNote(Note note)
        {
            try
            {
                Repository.SaveNote(AccountID, note);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public void OnEnrollmentCompleted(Order enrollmentOrder)
        {
            try
            {
                BusinessLogic.OnEnrollmentCompleted(Repository, this, enrollmentOrder);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public void OnEnrollmentCompleted(IEnrollmentContext enrollmentContext)
        {
            try
            {
                BusinessLogic.OnEnrollmentCompleted(enrollmentContext);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static void AssignRolesByAccountType(Account account)
        {
            BusinessLogic.AssignRolesByAccountType(account);
        }

        public static void CreateNewDefaultUser(Account account)
        {
            account.User = new User();
            account.User.Username = GenerateUsername(account);
            account.User.Password = GenerateRandomPassword();
            account.User.UserTypeID = (int)Constants.UserType.Distributor;
            account.User.UserStatusID = (int)Constants.UserStatus.Active;
            account.User.DefaultLanguageID = ApplicationContext.Instance.CurrentLanguageID;
        }

        public static Dictionary<string, string> ListAccountUser(string query)
        {
            return AccountExtensions.ListAccountUser(query);
        }

        /// <summary>
        /// Create by FHP
        /// </summary>
        /// <returns>List Id, Name</returns>
        public static Dictionary<string, string> GetAccountTypes()
        {
            try
            {
                return AccountExtensions.GetAccountTypes();
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        /// <summary>
        /// Create by JQP
        /// </summary>
        /// <returns>VOID</returns>
        public static void UnBlockMailAccount(int AccountID, string email)
        {
            try
            {
                AccountExtensions.UnBlockMailAccount(AccountID, email);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static AccountInformacion ListarAccountsInformacionAdicional(int AccountID)
        {
            return new AccountRepository().ListarAccountsInformacionAdicional(AccountID);
        }

        public static DataSet ExistsAccount(DataTable tblAccount)
        {
            DataSet Result = new DataSet();
            using (SqlConnection connection = new SqlConnection(DataAccess.GetConnectionString("Core")))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "dbo.upsExistsAccount";
                    command.CommandType = CommandType.StoredProcedure;

                    SqlParameter P1;
                    P1 = command.Parameters.AddWithValue("@Accounts", tblAccount);
                    P1.SqlDbType = SqlDbType.Structured;
                    P1.TypeName = "dbo.TypeTableInputAccounts";

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(Result);
                }
            }
            return Result;
        }


        public static DataSet ExistsAccountByAccountNumber(DataTable tblAccount)
        {
            DataSet Result = new DataSet();
            using (SqlConnection connection = new SqlConnection(DataAccess.GetConnectionString("Core")))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "dbo.upsExistsAccountByAccountNumber";
                    command.CommandType = CommandType.StoredProcedure;

                    SqlParameter P1;
                    P1 = command.Parameters.AddWithValue("@Accounts", tblAccount);
                    P1.SqlDbType = SqlDbType.Structured;
                    P1.TypeName = "dbo.TypeTableInputAccountByAccountNumber";

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(Result);
                }
            }
            return Result;
        }

        public static List<AccountBlocking> AccountBlockingStatus(int accountId, string blkProcess)
        {
            //List<AccountBlocking> accountResult = new List<AccountBlocking>();
            return DataAccess.ExecWithStoreProcedureListParam<AccountBlocking>("Core", "uspValidateBlocking",
                  new SqlParameter("AccountID", SqlDbType.Int) { Value = accountId }, new SqlParameter("CodProcess", SqlDbType.NVarChar) { Value = blkProcess }
                  ).ToList();
        }

        public static List<AccountPhones> AccountPhonesList(int accountId)
        {
             return DataAccess.ExecWithStoreProcedureListParam<AccountPhones>("Core", "uspAccountPhones",
                  new SqlParameter("AccountID", SqlDbType.Int) { Value = accountId }).ToList();
         }

        public static bool checkField(List<string> listField, string nameField)
        {
            foreach (string sField in listField)
            {
                if (sField.Equals(nameField))
                    return true;
            }
            return false;
        }
    }
}
