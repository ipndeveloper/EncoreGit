using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Web;
using NetSteps.Accounts.Common.Models;
using NetSteps.Commissions.Common;
using NetSteps.Common.Base;
using NetSteps.Common.Configuration;
using NetSteps.Common.Exceptions;
using NetSteps.Common.Expressions;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Common.Interfaces;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Dto;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Generated;
using NetSteps.Data.Entities.Hierarchy;
using NetSteps.Encore.Core.IoC;
using NetSteps.Security;
using NetSteps.Security.Authentication;
using EventRepository = global::NetSteps.Events.Common.Repositories;
using System.Data.SqlClient;
using NetSteps.Data.Entities.EntityModels;

namespace NetSteps.Data.Entities.Repositories
{
    [ContainerRegister(typeof(EventRepository.IAccountRepository), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class AccountRepository : BaseRepository<Account, int, NetStepsEntities>, IDefaultImplementation, IAccountRepository, EventRepository.IAccountRepository
    {
        #region Obsolete loadAllFullQuery
        [Obsolete("This query should not be used. Use the LoadRelations extension methods instead.")]
        protected override Func<NetStepsEntities, IQueryable<Account>> loadAllFullQuery
        {
            get
            {
                return CompiledQuery.Compile<NetStepsEntities, IQueryable<Account>>(context => context.Accounts
                    .Include("Addresses")
                    .Include("AccountPaymentMethods")
                    .Include("AccountPaymentMethods.BillingAddress")
                    .Include("AccountPhones")
                    .Include("FileResources")
                    .Include("Notes")
                    .Include("User")
                    .Include("User.Roles")
                    .Include("User.Roles.Functions")
                    .Include("MailAccounts")
                    .Include("DistributionSubscribers")
                    .Include("CampaignSubscribers")
                    .Include("AccountDevices")
                    .Include("AccountProperties")
                );
            }
        }
        #endregion

        public Account Authenticate(string username, string password)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var user = context.Accounts
                        .Where(a => a.User.Username == username)
                        .OrderByDescending(a => a.User.UserStatus.Active)
                        .ThenByDescending(a => a.AccountStatus.ReportAsActive)
                        .ThenBy(a => a.EnrollmentDateUTC)
                        .Select(a => a.User)
                        .FirstOrDefault();

                    if (user == null)
                    {
                        throw new NetStepsBusinessException("Invalid credentials.")
                        {
                            PublicMessage = Translation.GetTerm("InvalidCredentials", "Invalid credentials."),
                            IncludeErrorLogMessage = false
                        };
                    }

                    if (user.UserStatusID != Constants.UserStatus.Active.ToInt())
                    {
                        user.LogFailedLogin();
                        throw new NetStepsBusinessException("User is inactive or locked out.")
                        {
                            PublicMessage = Translation.GetTerm("UserIsInactiveOrLockedOut", "User is inactive or locked out."),
                            IncludeErrorLogMessage = false
                        };
                    }

                    if (SimpleHash.VerifyHash(password, SimpleHash.Algorithm.SHA512, user.PasswordHash))
                    {
                        var account = LoadFullFirstOrDefault(a => a.UserID == user.UserID, context);

                        if (account == null)
                        {
                            throw new NetStepsBusinessException("Account does not exist.")
                            {
                                PublicMessage = Translation.GetTerm("AccountDoesNotExist", "Account does not exist."),
                                IncludeErrorLogMessage = false
                            };
                        }

                        if (Container.Root.Registry.IsTypeRegistered<IAuthorizationProviders>())
                        {
                            IAuthorizationProviders authProviders = Create.New<IAuthorizationProviders>();
                            foreach (IAuthorizationProvider provider in authProviders.Providers)
                            {
                                IAuthorizationResult authResult = provider.CheckAuthorization(account);
                                if (!authResult.IsAuthorzied)
                                {
                                    user.LogFailedLogin();
                                    throw new NetStepsBusinessException("Failed Authorization.")
                                    {
                                        PublicMessage = authResult.Message,
                                        IncludeErrorLogMessage = false
                                    };
                                }
                            }
                        }

                        account.User.LogSucessfulLogin();
                        return account;
                    }
                    else
                    {
                        user.LogFailedLogin();
                        throw new NetStepsBusinessException("Invalid credentials.")
                        {
                            PublicMessage = Translation.GetTerm("InvalidCredentials", "Invalid credentials."),
                            IncludeErrorLogMessage = false
                        };
                    }
                }
            });
        }

        public Dictionary<int, string> SearchAccountsByText(string text)
        {
            return SearchAccountsByText(text, 0, 0, 0);
        }

        public Dictionary<int, string> SearchAccountsByTextAccountTypeAndSponsorId(string text, int accountTypeId, int sponsorId)
        {
            return SearchAccountsByText(text, accountTypeId, 0, sponsorId);
        }

        public Dictionary<int, string> SearchAccountsByTextAndAccountType(string text, int accountTypeId)
        {
            return SearchAccountsByText(text, accountTypeId, 0, 0);
        }

        public Dictionary<int, string> SearchAccountsByTextAndAccountStatus(string text, int accountStatusId)
        {
            return SearchAccountsByText(text, 0, accountStatusId, 0);
        }

        public Dictionary<int, string> SearchAccountsByTextAccountTypeAndAccountStatus(string text, int accountTypeId, int accountStatusId)
        {
            return SearchAccountsByText(text, accountTypeId, accountStatusId, 0);
        }

        private Dictionary<int, string> SearchAccountsByText(string text, int accountTypeId, int accountStatusId, int sponsorId)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                var parts = text.Trim().Split(' ').ToList();
                string firstValue = text;
                string secondValue = null;
                if (parts.Count > 1)
                {
                    firstValue = parts.First();
                    parts.RemoveAt(0);
                    secondValue = string.Join(" ", parts);
                }

                using (NetStepsEntities context = CreateContext())
                {
                    var result = FullTextAccountsSearch(context, firstValue, secondValue, accountTypeId, accountStatusId, sponsorId).ToList();
                    return result.ToDictionary(a => a.AccountId, a => a.DisplayText);
                }
            });
        }

        private IEnumerable<AccountSearchResult> FullTextAccountsSearch(NetStepsEntities context, string firstValue, string secondValue, int accountTypeId, int accountStatusId, int sponsorId)
        {
            firstValue = FormatForFullTextSearch(firstValue);
            //secondValue = FormatForFullTextSearch(secondValue);

            return context.ExecuteStoreQuery<AccountSearchResult>("EXECUTE dbo.AccountsSearchUsingFullText {0}, {1}, {2}, {3}, {4}", new object[] { firstValue, secondValue, accountTypeId, accountStatusId, sponsorId });
        }

        private string FormatForFullTextSearch(string fullTextParameter)
        {
            if (string.IsNullOrEmpty(fullTextParameter))
            {
                return null;
            }

            return string.Format("\"{0}*\"", fullTextParameter);
        }

        /// <summary>
        /// Returns the account id as the key and first name, last name, and account number as the value
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Dictionary<int, string> SlimSearchOnAccountStatuses(string query, int? accountTypeID = null, int?[] statusIDs = null, int? sponsorID = null)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    // Trim the search string - JHE
                    query = query.ToCleanString();

                    //return
                    var resultQuery = context.Accounts.Where(a => statusIDs.Contains(a.AccountStatusID)
                        && (!accountTypeID.HasValue || a.AccountTypeID == accountTypeID.Value)
                        && (!sponsorID.HasValue || a.SponsorID == sponsorID)
                        //&& (a.AccountNumber.Contains(query) || (a.FirstName + " " + a.LastName).Contains(query)))

                        /** 6-13-2011: Tenzin - Needs to be exactly the same format as the result.ToDictionary below **/
                        && (a.FirstName + " " + a.LastName + " (#" + a.AccountNumber + ")").Contains(query))
                        .Select(p => new
                        {
                            p.AccountID,
                            p.FirstName,
                            p.LastName,
                            p.AccountNumber
                        });
                    // var q = resultQuery.ToTraceString();
                    var result = resultQuery
                        .ToDictionary(a => a.AccountID, a => string.Format("{0} {1} (#{2})", a.FirstName, a.LastName, a.AccountNumber));

                    return result;
                }
            });
            //return Search(new AccountSearchParameters() { OrderBy = "FirstName", PageSize = null, WhereClause = a => a.AccountNumber.Contains(query) || (a.FirstName + " " + a.LastName).Contains(query) });
        }


        /// <summary>
        /// Returns the account id as the key and first name, last name, and account number as the value
        /// The displayEmail will override the AccountNumber with the Email Address.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Dictionary<int, string> SlimSearchEmail(string query, int? statusID = null, int? sponsorID = null)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                Dictionary<int, string> result;
                using (NetStepsEntities context = CreateContext())
                {
                    var accountTypes = new short[] {
							(short)Constants.AccountType.PreferredCustomer, 
							(short)Constants.AccountType.RetailCustomer, 
							(short)Constants.AccountType.Prospect, 
							(short)Constants.AccountType.Distributor};

                    IQueryable<Account> mySponsorQuery = null;
                    IQueryable<Account> myDownlineQuery = null;
                    IQueryable<Account> acctQuery = (from a in context.Accounts
                                                     where accountTypes.Contains(a.AccountTypeID)
                                                     && (a.AccountNumber.Contains(query) || a.EmailAddress.ToLower().Contains(query) || (a.FirstName + " " + a.LastName).Contains(query))
                                                     select a);
                    IQueryable<Account> baQuery = (from a in context.Accounts
                                                   where a.AccountTypeID == (short)Constants.AccountType.Distributor
                                                     && (a.AccountNumber.Contains(query) || a.EmailAddress.ToLower().Contains(query) || (a.FirstName + " " + a.LastName).Contains(query))
                                                   select a);
                    if (sponsorID.HasValue)
                    {
                        mySponsorQuery = (from a in context.Accounts
                                          where a.AccountID == sponsorID.Value && (a.Sponsor.AccountNumber.Contains(query) || a.Sponsor.EmailAddress.ToLower().Contains(query) || (a.Sponsor.FirstName + " " + a.Sponsor.LastName).Contains(query))
                                          select a.Sponsor);
                        myDownlineQuery = (from h in context.SponsorHierarchies.WhereInHierarchy(sponsorID.Value)
                                           join a in context.Accounts on h.AccountId equals a.AccountID
                                           where (a.AccountNumber.Contains(query) || a.EmailAddress.ToLower().Contains(query) || (a.FirstName + " " + a.LastName).Contains(query))
                                              && a.AccountID != sponsorID.Value //Exclude yourself
                                           select a);
                        acctQuery = acctQuery.Where(a => a.SponsorID == sponsorID.Value);
                    }

                    if (statusID.HasValue)
                    {
                        if (mySponsorQuery != null)
                        {
                            mySponsorQuery = mySponsorQuery.Where(s => s.AccountStatusID == statusID.Value);
                        }
                        if (myDownlineQuery != null)
                        {
                            myDownlineQuery = myDownlineQuery.Where(a => a.AccountStatusID == statusID.Value);
                        }
                        acctQuery = acctQuery.Where(a => a.AccountStatusID == statusID.Value);
                        baQuery = baQuery.Where(a => a.AccountStatusID == statusID.Value);
                    }
                    if (mySponsorQuery != null)
                    {
                        acctQuery = acctQuery.Union(mySponsorQuery);
                    }
                    if (myDownlineQuery != null)
                    {
                        acctQuery = acctQuery.Union(myDownlineQuery);
                    }
                    acctQuery = acctQuery.Union(baQuery);

                    result = acctQuery
                        .Select(a => new
                            {
                                a.AccountID,
                                a.FirstName,
                                a.LastName,
                                a.AccountNumber,
                                a.EmailAddress
                            })
                        .Distinct()
                        .ToDictionary(
                            a => a.AccountID,
                            a => (string)NetSteps.Data.Entities.Mail.MailMessageRecipient.GetAddressFormated(string.Format("{0} {1}", a.FirstName, a.LastName), a.EmailAddress));
                }
                return result;
            });
        }

        public virtual IQueryable<Account> GetAccountSearchQuery(AccountSearchParameters searchParameters, NetStepsEntities context)
        {
            //CurrentAccountID is required to trim down the data and prevent a consultant from seeing everybody if SponsorID is left null.
            if (searchParameters.CurrentAccountID == 0)
            {
                throw new NetStepsDataException("Error Loading Account Search Query.  CurrentAccountID is not set to a valid account.");
            }

            var query = context.Accounts
                .AsQueryable();

            //don't limit if the CurrentAccountID is the CorporateAccountID to allow for GMP searches
            if (searchParameters.CurrentAccountID != ApplicationContext.Instance.CorporateAccountID)
            {
                query = query.Where(x => x.SponsorID == searchParameters.CurrentAccountID);
            }

            if (searchParameters.AccountStatusID.HasValue)
            {
                query = query
                    .Where(a => a.AccountStatusID == searchParameters.AccountStatusID.Value);
            }
            // I don't like the idea of "ExcludedAccountStatuses". I think we should simply replace "short? AccountStatusID" with "short[] AccountStatusIDs". - Lundy
            else if (searchParameters.ExcludedAccountStatuses != null
                && searchParameters.ExcludedAccountStatuses.Any())
            {
                query = query
                    .Where(a => !searchParameters.ExcludedAccountStatuses.Contains(a.AccountStatusID));
            }

            if (searchParameters.AccountTypes != null
                && searchParameters.AccountTypes.Any())
            {
                query = query
                    .Where(a => searchParameters.AccountTypes.Contains(a.AccountTypeID));
            }

            if (searchParameters.StartDate.HasValue)
            {
                DateTime startDateUTC = searchParameters.StartDate.Value.StartOfDay().LocalToUTC();
                query = query
                    .Where(a => a.EnrollmentDateUTC >= startDateUTC);
            }

            if (searchParameters.EndDate.HasValue)
            {
                DateTime endDateUTC = searchParameters.EndDate.Value.EndOfDay().LocalToUTC();
                query = query
                    .Where(a => a.EnrollmentDateUTC <= endDateUTC);
            }

            if (searchParameters.SponsorID.HasValue)
            {
                query = query
                    .Where(a => a.SponsorID == searchParameters.SponsorID.Value);
            }

            if (!string.IsNullOrWhiteSpace(searchParameters.Email))
            {
                string email = searchParameters.Email.Trim();
                query = query
                    .Where(a => a.EmailAddress.Contains(email));
            }

            if (!string.IsNullOrWhiteSpace(searchParameters.CoApplicant))
            {
                string coApplicant = searchParameters.CoApplicant.Trim();
                query = query
                    .Where(a => a.CoApplicant.Contains(coApplicant));
            }

            if (!string.IsNullOrWhiteSpace(searchParameters.Name))
            {
                string name = searchParameters.Name.Trim();
                query = query
                    .Where(a => a.FirstName.Contains(name) || a.LastName.Contains(name));
            }
           
           //ENCORE-4 INICIO
           if (!string.IsNullOrWhiteSpace(searchParameters.CPF)) //BRA
            {
                string CPF = searchParameters.CPF.Trim();

                AccountSuppliedIDsRepository suppliedID = new AccountSuppliedIDsRepository();
                List<AccountSuppliedIDsTable> accountCPFs = suppliedID.GetAccountSuppliedIDsByType(new AccountSuppliedIDsParameters() { IDTypeID = 8, AccountSuppliedIDValue = CPF });

                if (accountCPFs.Count > 0)
                {
                    foreach (var supplied in accountCPFs)
                    {
                        query = query.Where(a => a.AccountID == supplied.AccountID);
                    }
                }
                else
                {
                    query = query.Where(a => a.AccountID == 0);
                }
            }

            if (!string.IsNullOrWhiteSpace(searchParameters.FirstName))
            {
                string firstName = searchParameters.FirstName.Trim();
                query = query
                    .Where(a => a.FirstName.Contains(firstName));
            }

            if (!string.IsNullOrWhiteSpace(searchParameters.LastName))
            {
                string lastName = searchParameters.LastName.Trim();
                query = query
                    .Where(a => a.LastName.Contains(lastName));
            }


            if (!string.IsNullOrWhiteSpace(searchParameters.SSN))
            {
                string ssn = searchParameters.SSN.Trim();
                var ssnEncrypt = Encryption.EncryptTripleDES(ssn);
                query = query
                    .Where(a => a.TaxNumber.Contains(ssnEncrypt));
            }

            if (!string.IsNullOrWhiteSpace(searchParameters.gender))
            {
                if (searchParameters.gender.Trim() != "None")
                {
                    string gender = searchParameters.gender.Trim();
                    query = query
                        .Where(a => a.Gender.TermName.Trim() == gender.Trim());
                }

            }
            //ENCORE-4 FIN

            // Filter on titleID through View of Synonym in Commissions DB - JHE
            if (searchParameters.TitleID.HasValue)
            {
                var currentPeriod = Create.New<ICommissionsService>().GetCurrentPeriod();
                var currentPeriodId = currentPeriod != null ? currentPeriod.PeriodId : 0;

                var titleTypeId = (int)Constants.TitleType.PaidAS;
                var titleId = searchParameters.TitleID.Value;

                var accountTitlesQuery = context.Commissions_AccountTitles_View
                    .Where(t =>
                        t.PeriodID == currentPeriodId
                        && t.TitleTypeID == titleTypeId
                        && t.TitleID == titleId
                    );

                query = query
                    .Join(
                        accountTitlesQuery,
                        a => a.AccountID,
                        t => t.AccountID,
                        (a, t) => a
                    );
            }

            if (searchParameters.ContactCategoryID.HasValue)
            {
                query = query
                    .Where(a => a.AccountContactTags
                        .Any(act => act.ContactCategoryID == searchParameters.ContactCategoryID.Value)
                    );
            }

            if (searchParameters.ContactStatusID.HasValue)
            {
                query = query
                    .Where(a => a.AccountContactTags
                        .Any(act => act.ContactStatusID == searchParameters.ContactStatusID.Value)
                    );
            }

            if (searchParameters.AccountSourceID.HasValue)
            {
                query = query
                    .Where(a => a.AccountSourceID == searchParameters.AccountSourceID.Value);
            }

            if (searchParameters.WhereClause != null)
            {
                query = query.Where(searchParameters.WhereClause);
            }

            return query;
        }

        public virtual IQueryable<AccountInfoCache> GetAccountInfoSearchQuery(AccountSearchParameters searchParameters, NetStepsEntities context)
        {
            var query = context.AccountInfoCache
                        .AsQueryable();

            if (searchParameters.StateProvinceID.HasValue)
            {
                query = query
                    .Where(i => i.StateProvinceID == searchParameters.StateProvinceID.Value);
            }

            if (!string.IsNullOrWhiteSpace(searchParameters.City))
            {
                string city = searchParameters.City.Trim();
                query = query
                    .Where(i => i.City.Contains(city));
            }

            if (!string.IsNullOrWhiteSpace(searchParameters.PostalCode))
            {
                string postalCode = searchParameters.PostalCode.Trim();
                query = query
                    .Where(i => i.PostalCode.Contains(postalCode));
            }

            if (searchParameters.CountryID.HasValue)
            {
                query = query
                    .Where(i => i.CountryID == searchParameters.CountryID.Value);
            }

            if (!string.IsNullOrWhiteSpace(searchParameters.PhoneNumber))
            {
                string phoneNumber = searchParameters.PhoneNumber.RemoveNonNumericCharacters();
                // Ensure we didn't just get an empty string.
                if (!string.IsNullOrEmpty(phoneNumber))
                {
                    query = query
                        .Where(i => i.PhoneNumber.Contains(phoneNumber));
                }
            }

            if (!string.IsNullOrWhiteSpace(searchParameters.SiteUrl))
            {
                string siteUrl = searchParameters.SiteUrl.Trim();
                query = query
                    .Where(i => i.PwsUrl.Contains(siteUrl));
            }

            return query;
        }

        //CGI(CMR)-10102014-Inicio
        private class AccountSearchQuery
        {
            public int AccountID { get; set; }
            public string AccountNumber { get; set; }
            public string AccountNumberSortable { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public short AccountTypeID { get; set; }
            public short AccountStatusID { get; set; }
            public string EmailAddress { get; set; }
            public string CoApplicant { get; set; }
            public DateTime? EnrollmentDateUTC { get; set; }
            public DateTime DateCreatedUTC { get; set; }
            public int MarketID { get; set; }

            public string SponsorAccountNumber { get; set; }
            public string SponsorFirstName { get; set; }
            public string SponsorLastName { get; set; }

            public string City { get; set; }
            public string StateAbbreviation { get; set; }

            public object OptOut { get; set; }
            public string Gender { get; set; } //CAMBIO ENCORE-4
            
        }
        //CGI(CMR)-10102014-Fin

        public virtual PaginatedList<AccountSearchData> Search(AccountSearchParameters searchParameters)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var query = from a in GetAccountSearchQuery(searchParameters, context)
                                join i in GetAccountInfoSearchQuery(searchParameters, context) on a.AccountID equals i.AccountID
                                select new AccountSearchQuery //CGI(CMR)-10/10/2014
                        {
                            AccountID = a.AccountID,
                            AccountNumber = a.AccountNumber,
                            AccountNumberSortable = i.AccountNumberSortable,
                            FirstName = a.FirstName,
                            LastName = a.LastName,
                            AccountTypeID = a.AccountTypeID,
                            AccountStatusID = a.AccountStatusID,
                            EmailAddress = a.EmailAddress,
                            CoApplicant = a.CoApplicant,
                            EnrollmentDateUTC = a.EnrollmentDateUTC,
                            DateCreatedUTC = a.DateCreatedUTC,
                            MarketID = a.MarketID,
                            
                            SponsorAccountNumber = i.SponsorAccountNumber,
                            SponsorFirstName = i.SponsorFirstName,
                            SponsorLastName = i.SponsorLastName,

                            City = i.City,
                            StateAbbreviation = i.StateAbbreviation,
                            Gender = a.Gender.TermName,

                            // I debated whether to do this here or to simply query the OptOut table separately.
                            // For now this does not appear to impact performance, so I'll leave it. - Lundy
                            OptOut = context.OptOuts
                                .Where(o => o.EmailAddress == a.EmailAddress)
                                .Select(o => new
                                {
                                    Placeholder = 1
                                })
                                .FirstOrDefault()
                        };

                    // Get the total count before sorting or pagination.
                    var totalCount = query.Count();

                    string orderBy = searchParameters.OrderBy;

                    // Default sort
                    if (string.IsNullOrWhiteSpace(orderBy))
                    {
                        orderBy = "AccountNumber";
                    }

                    switch (orderBy)
                    {
                        case "AccountNumber":
                            if (ConfigurationManager.GetAppSetting<bool>(ConfigurationManager.VariableKey.AccountNumbersEqualIdentity))
                            {
                                query = query
                                    .Order(searchParameters.OrderByDirection, x => x.AccountID);
                            }
                            else
                            {
                                query = query
                                    .Order(searchParameters.OrderByDirection, x => x.AccountNumberSortable);
                            }
                            break;
                        case "Location":
                            query = query
                                .Order(searchParameters.OrderByDirection, x => x.City)
                                .Then(searchParameters.OrderByDirection, x => x.StateAbbreviation);
                            break;
                        case "Sponsor":
                            query = query
                                .Order(searchParameters.OrderByDirection, x => x.SponsorFirstName)
                                .Then(searchParameters.OrderByDirection, x => x.SponsorLastName);
                            break;
                        case "AccountStatus":
                            query = query.OrderByLocalizedKind(
                                (int)Constants.LocalizedKindTable.AccountStatuses,
                                searchParameters.LanguageID,
                                x => x.AccountStatusID,
                                searchParameters.OrderByDirection,
                                context
                            );
                            break;
                        case "AccountType":
                            query = query.OrderByLocalizedKind(
                                (int)Constants.LocalizedKindTable.AccountTypes,
                                searchParameters.LanguageID,
                                x => x.AccountTypeID,
                                searchParameters.OrderByDirection,
                                context
                            );
                            break;
                        //CGI(CMR)-10/10/2014-Inicio
                        case "ID":
                            query = query.Order(searchParameters.OrderByDirection, x => x.AccountID);
                            break;
                        case "JoinDate":
                            query = query.Order(searchParameters.OrderByDirection, x => x.EnrollmentDateUTC);
                            break;
                        //CGI(CMR)-10/10/2014-Inicio
                        default:
                            // We know searchParameters.OrderByString is not blank or it would have
                            // hit the "AccountNumber" case, so it's safe to use searchParameters.OrderByString.
                            query = DynamicQueryable.OrderBy(query, searchParameters.OrderByString);
                            break;
                    }

                    query = query.ApplyPagination(searchParameters);

                    // Execute the query and apply final select.
                    return query
                        .ToArray()
                        .Select(x => new AccountSearchData
                        {
                            AccountID = x.AccountID,
                            AccountNumber = x.AccountNumber ?? string.Empty,
                            FirstName = x.FirstName ?? string.Empty,
                            LastName = x.LastName ?? string.Empty,
                            FullName = string.Format(
                                "{0} {1}",
                                x.FirstName ?? string.Empty,
                                x.LastName ?? string.Empty
                            ),
                            AccountType = SmallCollectionCache.Instance.AccountTypes.GetById(x.AccountTypeID).GetTerm(),
                            AccountTypeID = x.AccountTypeID,
                            EmailAddress = x.EmailAddress ?? string.Empty,
                            CoApplicant = x.CoApplicant ?? string.Empty,
                            Sponsor = string.Format(
                                "{0} {1}",
                                x.SponsorFirstName ?? string.Empty,
                                x.SponsorLastName ?? string.Empty
                            ),
                            SponsorAccountNumber = x.SponsorAccountNumber ?? string.Empty,
                            DateEnrolled = x.EnrollmentDateUTC.UTCToLocal(),
                            DateCreated = x.DateCreatedUTC.UTCToLocal(),
                            AccountStatusID = x.AccountStatusID,
                            AccountStatus = SmallCollectionCache.Instance.AccountStatuses.GetById(x.AccountStatusID).GetTerm(),
                            Location = (string.IsNullOrEmpty(x.City) && string.IsNullOrEmpty(x.StateAbbreviation))
                                ? string.Empty
                                : string.Format(
                                    "{0}, {1}",
                                    x.City ?? string.Empty,
                                    x.StateAbbreviation ?? string.Empty
                                ),
                            IsOptedOut = x.OptOut != null,
                            MarketID = x.MarketID,
                            Gender = x.Gender,
                            

                        })
                        .ToPaginatedList(searchParameters, totalCount);
                }
            });
        }

        /// <summary>
        /// Check if account has file resources associated
        /// </summary>
        /// <param name="primaryKey">account id</param>
        /// <returns>bool</returns>
        public bool HasFileResources(int primaryKey)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var account = LoadFullFirstOrDefault(a => a.AccountID == primaryKey);
                    return (account.FileResources.Count > 0) ? true : false;
                }
            });
        }

        /// <summary>
        /// Load sponsored accounts
        /// </summary>
        /// <param name="primaryKey">account number</param>
        /// <param name="offset">offset</param>
        /// <returns>IEnumerable<Account></returns>
        public IEnumerable<Account> LoadSponsoredAccount(int primaryKey, int offset)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var account = context.Accounts.Include("AccountProperties")
                        .Where(a => a.SponsorID == primaryKey
                                    && a.AccountStatusID == (decimal)ConstantsGenerated.AccountStatus.Active
                                    && a.AccountTypeID == (decimal)ConstantsGenerated.AccountType.Distributor)
                        .OrderBy(a => a.FirstName).Skip(offset).Take(20).ToList();
                    return account;

                }
            });
        }

        /// <summary>
        /// Account properties for particular account
        /// </summary>
        /// <param name="primaryKey">primaryKey</param>
        /// <returns>Account</returns>
        public Account GetAccountProperties(int primaryKey)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var account = context.Accounts.Include("AccountProperties").Where(ac => ac.AccountID == primaryKey).FirstOrDefault();
                    if (account != null)
                        account.StartEntityTracking();

                    return account;
                }
            });
        }

        public Account LoadByAccountNumber(string accountNumber)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var result = context.Accounts.FirstOrDefault(o => o.AccountNumber == accountNumber);

                    if (result == null)
                        throw new NetStepsDataException("Error loading account. Invalid accountNumber: " + accountNumber);
                    else
                        return result;
                }
            });
        }

        public Account LoadByAccountNumberFull(string accountNumber)
        {
            var account = LoadFullFirstOrDefault(a => a.AccountNumber == accountNumber);

            if (account == null)
                throw new NetStepsDataException("Error loading account. Invalid accountNumber: " + accountNumber);
            else
                return account;
        }

        public Account LoadAccountByUserId(int userId)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    return context.Accounts.FirstOrDefault(o => o.UserID == userId);
                }
            });
        }

        public Account LoadNonProspectByEmailFull(string email)
        {
            var account = LoadFullFirstOrDefault(a =>
                a.EmailAddress == email
                && a.AccountTypeID != (int)Constants.AccountType.Prospect
                && a.AccountStatus.ReportAsActive
            );

            if (account == null)
                throw new NetStepsDataException("Error loading account. Invalid email: " + email);

            return account;
        }

        public int? GetNonProspectAccountIDByEmail(string email, bool? returnActive = true)
        {

            var accountID = returnActive.HasValue ?
                FirstOrDefaultSelect(
                a => a.EmailAddress == email
                    && a.AccountTypeID != (int)Constants.AccountType.Prospect
                    && a.AccountStatus.ReportAsActive == returnActive.Value,
                a => a.AccountID)
            :
                FirstOrDefaultSelect(a => a.EmailAddress == email
                    && a.AccountTypeID != (int)Constants.AccountType.Prospect,
                a => a.AccountID, "AccountStatus.ReportAsActive", true);
            return accountID == 0 ? null : (int?)accountID;
        }

        public Account LoadByUserIdFull(int userID)
        {
            var account = LoadFullFirstOrDefault(a => a.UserID == userID);

            if (account == null)
                throw new NetStepsDataException("Error loading account. Invalid userID: " + userID);

            return account;
        }

        public Account LoadByUserIdFull_(int userID, DateTime BirthDay)
        {
            var account = LoadFullFirstOrDefault(a => a.UserID == userID && a.BirthdayUTC == BirthDay);

            if (account == null)
                throw new NetStepsDataException("Error loading account. Invalid userID: " + userID);

            return account;
        }

        public AccountSlimSearchData LoadSlim(int accountID)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var result = (from a in context.Accounts
                                  where a.AccountID == accountID
                                  select new
                                       {
                                           a.AccountID,
                                           a.MarketID,
                                           a.SponsorID,
                                           a.AccountTypeID,
                                           a.AccountNumber,
                                           a.FirstName,
                                           a.LastName,
                                           a.EmailAddress,
                                           a.TaxNumber,
                                           a.TaxGeocode,
                                           a.IsTaxExempt
                                       }).FirstOrDefault();

                    if (result == null)
                        throw new NetStepsDataException("Error loading account. Invalid accountID: " + accountID);
                    else
                        return new AccountSlimSearchData()
                            {
                                AccountID = result.AccountID,
                                MarketID = result.MarketID,
                                SponsorID = result.SponsorID,
                                AccountTypeID = result.AccountTypeID,
                                AccountNumber = result.AccountNumber,
                                FirstName = result.FirstName,
                                LastName = result.LastName,
                                FullName = result.FirstName + " " + result.LastName,
                                EmailAddress = result.EmailAddress,
                                DecryptedTaxNumber = (!result.TaxNumber.IsNullOrEmpty()) ? Encryption.DecryptTripleDES(result.TaxNumber) : string.Empty,
                                IsTaxExempt = result.IsTaxExempt
                            };
                }
            });
        }

        public AccountSlimSearchData LoadSlimByAccountNumber(string accountNumber)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var result = (from a in context.Accounts
                                  where a.AccountNumber == accountNumber
                                  select new
                                  {
                                      a.AccountID,
                                      a.SponsorID,
                                      a.AccountTypeID,
                                      a.AccountNumber,
                                      a.FirstName,
                                      a.LastName,
                                      a.EmailAddress,
                                      a.TaxNumber
                                  }).FirstOrDefault();

                    if (result == null)
                        throw new NetStepsDataException("Error loading account. Invalid accountNumber: " + accountNumber);
                    else
                        return new AccountSlimSearchData()
                        {
                            AccountID = result.AccountID,
                            SponsorID = result.SponsorID,
                            AccountTypeID = result.AccountTypeID,
                            AccountNumber = result.AccountNumber,
                            FirstName = result.FirstName,
                            LastName = result.LastName,
                            FullName = result.FirstName + " " + result.LastName,
                            EmailAddress = result.EmailAddress,
                            DecryptedTaxNumber = (!result.TaxNumber.IsNullOrEmpty()) ? Encryption.DecryptTripleDES(result.TaxNumber) : string.Empty
                        };
                }
            });
        }

        public List<AccountSlimSearchData> LoadBatchSlim(IEnumerable<int> accountIDs)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var where = ExpressionHelper.MakeWhereInExpression<Account, int>(a => a.AccountID, accountIDs);
                    return context.Accounts.Where(where).Select(a => new AccountSlimSearchData()
                        {
                            AccountID = a.AccountID,
                            SponsorID = a.SponsorID,
                            AccountTypeID = a.AccountTypeID,
                            AccountNumber = a.AccountNumber,
                            FirstName = a.FirstName,
                            LastName = a.LastName,
                            FullName = a.FirstName + " " + a.LastName,
                            EmailAddress = a.EmailAddress
                        }).ToList();
                }
            });
        }

        public List<Account> GetRecent100()
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    return (from o in context.Accounts.Take(100)
                            orderby o.AccountID descending
                            select o).ToList();
                }
            });
        }

        public virtual IPaginatedList<IAccountLocatorAccountData> AccountLocatorAccountSearch(
            IAccountLocatorSearchParameters searchParameters)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var baseQuery = GetAccountLocatorBaseQuery(
                        searchParameters,
                        context
                    );

                    var accountQuery = searchParameters.IsGeoLocationSearch()
                        ? ApplyAccountLocatorAccountGeoQuery(baseQuery, searchParameters, context)
                        : ApplyAccountLocatorAccountNonGeoQuery(baseQuery, searchParameters, context);

                    int totalCount = accountQuery.Count();

                    return SortAccountLocatorQuery(
                            accountQuery,
                            searchParameters
                        )
                        .ApplyPagination(searchParameters)
                        .ToPaginatedList(searchParameters, totalCount);
                }
            });
        }

        public virtual IQueryable<Account> GetAccountLocatorBaseQuery(
            IAccountLocatorSearchParameters searchParameters,
            NetStepsEntities context)
        {
            //var query = ObjectSet<Account>;
            int countryId = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["EnvironmentCountry_Aux"]);
            int marketId = (int)Constants.Market.Brazil;
            if (countryId == (int)Constants.Country.UnitedStates)
            {
                marketId = (int)Constants.Market.UnitedStates;
            }
              var  query = context.Accounts
              .Where(x => x.AccountStatusID == (int)Constants.AccountStatus.Active && x.MarketID == marketId); 

            if (searchParameters.AccountTypeIDs != null && searchParameters.AccountTypeIDs.Any())
            {
                query = query.Where(x => searchParameters.AccountTypeIDs.Contains(x.AccountTypeID));
            }

            if (!string.IsNullOrWhiteSpace(searchParameters.AccountNumber))
            {
                // Case-insensitive, exact match.
                query = query.Where(x => x.AccountNumber == searchParameters.AccountNumber);
            }

            if (!string.IsNullOrWhiteSpace(searchParameters.FirstName))
            {
                // Case-insensitive, exact match.
                query = query.Where(x => x.FirstName == searchParameters.FirstName);
            }

            if (!string.IsNullOrWhiteSpace(searchParameters.LastName))
            {
                // Case-insensitive, exact match.
                query = query.Where(x => x.LastName == searchParameters.LastName);
            }

            if (searchParameters.RequirePwsUrl)
            {
                query = query.Where(x =>
                    x.Sites.Any(s =>
                        s.SiteStatusID == (int)Constants.SiteStatus.Active
                        && s.SiteTypeID == (int)Constants.SiteType.Replicated
                        && s.SiteUrls.Any()
                    )
                );
            }

            return query;
        }

        public virtual IQueryable<IAccountLocatorAccountData> ApplyAccountLocatorAccountGeoQuery(
            IQueryable<Account> baseQuery,
            IAccountLocatorSearchParameters searchParameters,
            NetStepsEntities context)
        {

            int PhoneTypeIDMain = context.PhoneTypes.Where((pt) => pt.Name == "Main").FirstOrDefault().PhoneTypeID;
            var accountQuery = context.AccountInfoCache
                .Where(ad =>
                    ad.Latitude != null
                    && ad.Longitude != null
                    && ad.CountryID == (int)Constants.Country.UnitedStates
                )
                .SelectWithDistance(
                    NetStepsEntities.SelectWithDistance,
                    searchParameters.Latitude.Value,
                    searchParameters.Longitude.Value,
                    searchParameters.DistanceType
                )
                .Select(x => new
                {
                    AccountID = x.GeoCodeItem.AccountID,
                    City = x.GeoCodeItem.City,
                    State = x.GeoCodeItem.StateAbbreviation,
                    CountryID = x.GeoCodeItem.CountryID,
                    Distance = x.Distance
                });

            var query = baseQuery
                .Join(
                    accountQuery,
                    a => a.AccountID,
                    x => x.AccountID,
                    (a, x) => new { Account = a, Extended = x }
                )
                .Select(x => new AccountLocatorAccountData
                {
                    AccountID = x.Account.AccountID,
                    FirstName = x.Account.FirstName,
                    LastName = x.Account.LastName,
                    City = x.Extended.City,
                    State = x.Extended.State,
                    CountryID = x.Extended.CountryID,
                    Distance = x.Extended.Distance,
                    EmailAddress = x.Account.EmailAddress,
                    PhoneNumber = x.Account.AccountPhones
                            .Where((ap) => ap.PhoneTypeID == PhoneTypeIDMain).FirstOrDefault().PhoneNumber

                });

            var commissionService = Create.New<ICommissionsService>();

            var period = commissionService.GetCurrentAndPastPeriods().Where(p => !p.IsOpen).OrderByDescending((Func<NetSteps.Commissions.Common.Models.IPeriod, int>)(p => p.PeriodId)).FirstOrDefault();
            if (period != null)
            {
                var validTitleIDs = this.ValidTitleIDs;
                var titleKinds = commissionService.GetTitleKinds();
                NetSteps.Commissions.Common.Models.ITitleKind paidAsTK;
                if (validTitleIDs != null && validTitleIDs.Any() && titleKinds != null && (paidAsTK = titleKinds.Where(tk => tk.TitleKindCode == "PAT").FirstOrDefault()) != null)
                {
                    var curTitles = commissionService.GetCurrentAccountTitles(period.PeriodId);
                    if (curTitles != null && curTitles.Any())
                    {
                        var ctPats = curTitles.Where(ct => ct.TitleKindId == paidAsTK.TitleKindId && validTitleIDs.Contains(ct.TitleId)).Select(c => c.AccountId).ToArray();
                        query = query.Where(al => ctPats.Contains(al.AccountID));
                    }
                }
            }

            return query;
        }

        IList<int> _validTitleIDs = null;
        private IList<int> ValidTitleIDs
        {
            get
            {
                if (_validTitleIDs == null)
                {
                    ICommissionsService commissionsService = Create.New<ICommissionsService>();
                    var titles = commissionsService.GetTitles();
                    if (titles != null && titles.Any())
                    {
                        var filterTitleCode = this.LocatorTitleCodeFilter;
                        if (!String.IsNullOrWhiteSpace(filterTitleCode))
                        {
                            var filterTitle = titles.Where(x => x.TitleCode == filterTitleCode).FirstOrDefault();
                            if (filterTitle != null)
                            {
                                _validTitleIDs = titles
                                    .Where(x => x.SortOrder >= filterTitle.SortOrder)
                                    .Select(x => x.TitleId)
                                    .ToList();
                            }
                        }
                    }
                }

                return _validTitleIDs;
            }
        }

        string LocatorTitleCodeFilter
        {
            get
            {
                return ConfigurationManager.AppSettings["LocatorTitleCodeFilter"];
            }
        }

        public virtual IQueryable<IAccountLocatorAccountData> ApplyAccountLocatorAccountNonGeoQuery(
            IQueryable<Account> baseQuery,
            IAccountLocatorSearchParameters searchParameters,
            NetStepsEntities context)
        {
            int PhoneTypeIDMain = context.PhoneTypes.Where((pt) => pt.Name == "Main").FirstOrDefault().PhoneTypeID;
            return baseQuery
                .Select(a => new
                {
                    a.AccountID,
                    a.FirstName,
                    a.LastName,
                    a.EmailAddress,
                    PhoneNumber = a.AccountPhones
                           .Where((ap) => ap.PhoneTypeID == PhoneTypeIDMain).FirstOrDefault().PhoneNumber,

                    Address = a.Addresses
                        .Where(ad => ad.AddressTypeID == (int)Constants.AddressType.Main)
                        .Select(ad => new
                        {
                            ad.IsDefault,
                            ad.City,
                            ad.State,
                            ad.CountryID
                        })
                        .OrderByDescending(ad => ad.IsDefault)
                        .FirstOrDefault()
                })
                .Select(a => new AccountLocatorAccountData
                {
                    AccountID = a.AccountID,
                    FirstName = a.FirstName,
                    LastName = a.LastName,
                    City = a.Address.City,
                    State = a.Address.State,
                    CountryID = a.Address.CountryID,
                    EmailAddress = a.EmailAddress,
                    PhoneNumber = a.PhoneNumber
                });
        }

        public virtual IQueryable<IAccountLocatorAccountData> SortAccountLocatorQuery(
            IQueryable<IAccountLocatorAccountData> query,
            IAccountLocatorSearchParameters searchParameters)
        {
            string defaultOrderBy = "FirstName, LastName";
            string orderBy;
            if (string.IsNullOrWhiteSpace(searchParameters.OrderBy))
            {
                orderBy = defaultOrderBy;
            }
            else if (searchParameters.OrderBy.EqualsIgnoreCase("Distance") && !searchParameters.IsGeoLocationSearch())
            {
                orderBy = defaultOrderBy;
            }
            else
            {
                orderBy = searchParameters.OrderByString;
            }

            return DynamicQueryable.OrderBy(query, orderBy);
        }

        public virtual IList<IAccountLocatorContentData> GetAccountLocatorContent(
            IEnumerable<int> accountIDs)
        {
            if (!accountIDs.Any())
            {
                return new List<IAccountLocatorContentData>();
            }

            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var contentData = context.Sites
                        .Where(s =>
                            accountIDs.Contains(s.AccountID.Value)
                            && s.SiteStatusID == (int)Constants.SiteStatus.Active
                            && s.SiteTypeID == (int)Constants.SiteType.Replicated
                            && s.SiteUrls.Any()
                        )
                        .GroupBy(s => s.AccountID, (accountID, s) => s.FirstOrDefault())
                        .Select(s => new
                        {
                            AccountID = s.AccountID.Value,
                            PwsUrl = s.SiteUrls
                                .OrderByDescending(su => su.IsPrimaryUrl)
                                .Select(su => su.Url)
                                .FirstOrDefault(),
                            PhotoContent = s.HtmlSectionContents
                                .Where(hsc =>
                                    hsc.HtmlSection.SectionName == "MyPhoto"
                                    && hsc.HtmlContent.HtmlContentStatusID == (int)Constants.HtmlContentStatus.Production
                                )
                                .Select(hsc => new
                                {
                                    hsc.HtmlContent,
                                    hsc.HtmlContent.HtmlElements
                                })
                                .FirstOrDefault()
                        })
                        .ToList();

                    return contentData
                        .Select(c => new AccountLocatorContentData
                        {
                            AccountID = c.AccountID,
                            PwsUrl = c.PwsUrl,
                            PhotoContent = new HtmlString(c.PhotoContent == null ? string.Empty : c.PhotoContent.HtmlContent.BuildContent())
                        })
                        .Cast<IAccountLocatorContentData>()
                        .ToList();
                }
            });
        }

        public List<AccountSearchData> LoadBatchHeaders(List<int> primaryKeys)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    List<AccountSearchData> results = new List<AccountSearchData>();

                    var entityQuery = GetLoadBatchQuery(primaryKeys, context);

                    var accountInfos = from a in entityQuery
                                       select new
                                       {
                                           a.AccountID,
                                           a.AccountNumber,
                                           a.FirstName,
                                           a.LastName,
                                           AccountType = a.AccountType.Name,
                                           a.AccountTypeID,
                                           a.EmailAddress,
                                           SponsorFirstName = a.Sponsor.FirstName,
                                           SponsorLastName = a.Sponsor.LastName,
                                           SponsorAccountNumber = a.Sponsor.AccountNumber,
                                           MainAddress = a.Addresses.FirstOrDefault(ad => ad.AddressTypeID == (short)Constants.AddressType.Main),
                                           a.EnrollmentDateUTC,
                                           a.AccountStatusID,
                                       };

                    foreach (var a in accountInfos.ToList())
                        results.Add(new AccountSearchData()
                        {
                            AccountID = a.AccountID,
                            AccountNumber = a.AccountNumber,
                            FirstName = a.FirstName,
                            LastName = a.LastName,
                            FullName = string.Format("{0} {1}", a.FirstName, a.LastName),
                            AccountType = SmallCollectionCache.Instance.AccountTypes.GetById(a.AccountTypeID).GetTerm(),
                            AccountTypeID = a.AccountTypeID,
                            EmailAddress = a.EmailAddress,
                            Sponsor = string.Format("{0} {1}", a.SponsorFirstName, a.SponsorLastName),
                            SponsorAccountNumber = a.SponsorAccountNumber,
                            DateEnrolled = a.EnrollmentDateUTC.UTCToLocal(),
                            AccountStatusID = a.AccountStatusID,
                            Location = (a.MainAddress == null) ? string.Empty : string.Format("{0}, {1}", a.MainAddress.State, a.MainAddress.City)
                        });

                    return results;
                }
            });
        }

        public override PaginatedList<AuditLogRow> GetAuditLog(int primaryKey, AuditLogSearchParameters searchParameters)
        {
            ValidatePrimaryKeyForLoad(primaryKey);

            var account = Account.LoadFull(primaryKey);

            return GetAuditLog(account, searchParameters);
        }
        public virtual PaginatedList<AuditLogRow> GetAuditLog(Account fullyLoadedAccount, AuditLogSearchParameters searchParameters)
        {
            var account = fullyLoadedAccount;

            List<AuditTableValueItem> list = new List<AuditTableValueItem>();
            list.Add(new AuditTableValueItem()
            {
                TableName = EntitySetName,
                PrimaryKey = Convert.ToInt32(fullyLoadedAccount.AccountID)
            });

            if (account.User != null && account.User.UserID > 0)
                list.Add(new AuditTableValueItem()
                {
                    TableName = "Users",
                    PrimaryKey = account.User.UserID
                });

            if (account.Addresses != null)
                foreach (var address in account.Addresses)
                    list.Add(new AuditTableValueItem()
                    {
                        TableName = "Addresses",
                        PrimaryKey = address.AddressID
                    });

            if (account.AccountPhones != null)
                foreach (var accountPhone in account.AccountPhones)
                    list.Add(new AuditTableValueItem()
                    {
                        TableName = "AccountPhones",
                        PrimaryKey = accountPhone.AccountPhoneID
                    });

            if (account.AccountPaymentMethods != null)
                foreach (var accountPaymentMethod in account.AccountPaymentMethods)
                    list.Add(new AuditTableValueItem()
                    {
                        TableName = "AccountPaymentMethods",
                        PrimaryKey = accountPaymentMethod.AccountPaymentMethodID
                    });

            return GetAuditLog(list, searchParameters);
        }

        public bool ExistsByAccountNumber(string accountNumber)
        {
            return Any(x => x.AccountNumber == accountNumber);
        }

        public override void Save(Account account)
        {
            if (account.ContainsUnmodifiedDuplicateEntitiesInObjectGraph<Address>())
            {
                var dup = account.Clone();
                dup.RemoveUnmodifiedDuplicateEntitiesInObjectGraph<Address>();
                base.Save(dup);
                account.AcceptEntityChanges();
                // TODO: The IDs created by database Identities need to be set back to the 'account' object from the dup object to avoid a problem if the same object is modified and saved again. - JHE
            }
            else
            {
                base.Save(account);
            }

            if (account.AccountID > 0 && account.AccountTypeID == 1)
            {
                using (var create = Create.SharedOrNewContainer())
                {
                    if (create.Registry.IsTypeRegistered<INetStepsInformer<Account>>())
                    {
                        var accountUpdateInformer = create.New<INetStepsInformer<Account>>();
                        accountUpdateInformer.InformObservers(account);
                    }
                }
            }
        }

        protected override string GetMeaningfulAuditValue(string tableName, string columnName, string value)
        {
            try
            {
                if (columnName == "TaxNumber")
                    return Encryption.DecryptTripleDES(value).MaskString(4);
                else if (columnName == "PasswordHash")
                    return string.Format("({0})", Translation.GetTerm("encrypted"));
                else if (tableName == "AccountPaymentMethods" && columnName == "AccountNumber")
                    return Encryption.DecryptTripleDES(value).MaskString(4);
                else
                    return base.GetMeaningfulAuditValue(tableName, columnName, value);
            }
            catch (Exception ex)
            {
                EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsDataException);
                return value;
            }
        }

        public List<AccountContactTag> AccountContactTags(int accountID)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var accountContactTags = (from a in context.Accounts
                                              join ac in context.AccountContactTags on a.AccountID equals ac.AccountID
                                              where a.AccountID == accountID
                                              orderby ac.AccountID descending
                                              select ac).ToList();

                    return accountContactTags.ToList();
                }
            });
        }

        public AccountPaymentMethod LoadPaymentMethodAndVerifyAccount(int paymentMethodId, int accountId)
        {
            using (NetStepsEntities context = CreateContext())
            {
                var paymentMethod = context.AccountPaymentMethods.Include("BillingAddress").FirstOrDefault(pm => pm.AccountPaymentMethodID == paymentMethodId);

                //if (paymentMethod == default(AccountPaymentMethod))
                //    throw new NetStepsDataException("Error loading payment method.  Invalid payment method id: " + paymentMethodId);
                //if (paymentMethod.AccountID != accountId)
                //    throw new NetStepsDataException("Error loading payment method.  This payment method is not attached to account id: " + accountId);

                return paymentMethod;
            }

            //return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            //{
            //    using (NetStepsEntities context = CreateContext())
            //    {
            //        var paymentMethod = context.AccountPaymentMethods.Include("BillingAddress").FirstOrDefault(pm => pm.AccountPaymentMethodID == paymentMethodId);

            //        if (paymentMethod == default(AccountPaymentMethod))
            //            throw new NetStepsDataException("Error loading payment method.  Invalid payment method id: " + paymentMethodId);
            //        if (paymentMethod.AccountID != accountId)
            //            throw new NetStepsDataException("Error loading payment method.  This payment method is not attached to account id: " + accountId);

            //        return paymentMethod;
            //    }
            //});
        }

        public NonAccountPaymentMethod LoadPaymentTypeAndVerifyUser(int paymentTypeID, NetSteps.Common.Interfaces.IUser user, bool checkAnonymousRole = true, bool checkWorkstationUserRole = false)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    return new NonAccountPaymentMethod();
                }
            });
        }

        public Address LoadAddressAndVerifyAccount(int addressId, int accountId)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var address = context.Addresses.Include("AddressProperties").FirstOrDefault(a => a.AddressID == addressId);

                    if (address == default(Address))
                        throw new NetStepsDataException("Error loading Address.  Invalid address id: " + addressId);

                    var count = context.ExecuteStoreQuery<int>("SELECT COUNT(1) FROM AccountAddresses WHERE AddressID = @p0 AND AccountID = @p1", addressId, accountId);
                    if (count.First() == 0)
                        throw new NetStepsDataException("Error loading Address.  This address is not attached to account id: " + accountId);

                    return address;
                }
            });
        }


        public List<AccountReport> LoadAccountReports(int accountID)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var accountReports = context.AccountReports.Where(a => a.AccountID == accountID && !a.IsCorporate).OrderByDescending(a => a.Name).ToList();

                    return accountReports;
                }
            });
        }

        public List<AccountReport> LoadCorporateReports()
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var corporateReports = context.AccountReports.Where(a => a.IsCorporate).OrderByDescending(a => a.Name).ToList();

                    return corporateReports;
                }
            });
        }
        public bool AccountExists(string email, string currentEmail = null)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    return context.Accounts.Any(a => a.EmailAddress == email && a.AccountStatusID != (int)Constants.AccountStatus.BegunEnrollment);
                }
            });
        }

        public bool IsTaxNumberAvailable(string taxNumber, int? accountID = null)
        {
            if (taxNumber.ToCleanString().IsNullOrEmpty())
                return true;

            using (NetStepsEntities context = CreateContext())
            {
                taxNumber = taxNumber.Replace("-", string.Empty).ToCleanString();
                string encryptedTaxNumber = Encryption.EncryptTripleDES(taxNumber);

                if (accountID != null)
                {
                    var re = context.Accounts.Count(a => a.TaxNumber == encryptedTaxNumber && a.AccountStatusID != (int)Constants.AccountStatus.BegunEnrollment && a.AccountID != accountID) == 0;
                    return context.Accounts.Count(a => a.TaxNumber == encryptedTaxNumber && a.AccountStatusID != (int)Constants.AccountStatus.BegunEnrollment && a.AccountID != accountID) == 0;
                }
                else
                {
                    return context.Accounts.Count(a => a.TaxNumber == encryptedTaxNumber && a.AccountStatusID != (int)Constants.AccountStatus.BegunEnrollment) == 0;
                }
            }

            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    taxNumber = taxNumber.Replace("-", string.Empty).ToCleanString();
                    string encryptedTaxNumber = Encryption.EncryptTripleDES(taxNumber);

                    if (accountID != null)
                        return context.Accounts.Count(a => a.TaxNumber == encryptedTaxNumber && a.AccountStatusID != (int)Constants.AccountStatus.BegunEnrollment && a.AccountID != accountID) == 0;
                    else
                        return context.Accounts.Count(a => a.TaxNumber == encryptedTaxNumber && a.AccountStatusID != (int)Constants.AccountStatus.BegunEnrollment) == 0;

                }
            });
        }

        /// <summary>
        /// Checks for any active, non-prospect accounts with the given email address.
        /// </summary>
        /// <param name="email">The email address to search for</param>
        /// <param name="ignoreAccountID">If specified, ignores the given account in the result set</param>
        /// <returns>True if an active, non-prospect account was found with the given email address</returns>
        public bool NonProspectExists(string email, int? ignoreAccountID = null)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var query = context.Accounts
                        .Where(x =>
                            x.EmailAddress == email
                            && x.AccountTypeID != (int)Constants.AccountType.Prospect
                            && x.AccountStatus.ReportAsActive);

                    if (ignoreAccountID != null)
                    {
                        query = query.Where(x => x.AccountID != ignoreAccountID.Value);
                    }

                    return query.Any();
                }
            });
        }

        public Account GetAccountByEmailAndSponsorID(string email, int sponsorId, bool enableTracking = false, bool? getActive = null)
        {
            var account = getActive ?? false
                ? LoadFullFirstOrDefault(a => a.EmailAddress == email && a.SponsorID == sponsorId)
                : LoadFullFirstOrDefault(a => a.EmailAddress == email && a.SponsorID == sponsorId && a.AccountStatus.ReportAsActive);

            if (account != null && enableTracking)
                account.StartEntityTracking();

            return account;
        }

        public List<IAccount> GetAccountsByEmail(string email)
        {
            using (var context = CreateContext())
            {
                var accounts = context.Accounts
                    .Where(a => a.EmailAddress == email)
                    .ToList();

                return accounts
                        .Select(a => (IAccount)a)
                        .ToList();
            }
        }

        public Account LoadAccountByEmailAndAccountType(string email, Constants.AccountType accountType)
        {
            var account = LoadFullFirstOrDefault(a => a.EmailAddress == email && a.AccountTypeID == (int)accountType);

            if (account != null)
                account.StartEntityTracking();

            return account;
        }

        public override void Delete(Account obj)
        {
            Delete(obj.AccountID);
        }
        public override void Delete(int primaryKey)
        {
            ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var obj = context.Accounts
                                            .Include("Addresses")
                                            .Include("AccountContactTags")
                                            .Include("AccountPaymentMethods")
                                            .Include("AccountPaymentMethods.BillingAddress")
                                            .Include("AccountPhones")
                                            .Include("Notes")
                                            .Include("User")
                                            .Include("User.Roles")
                                            .Include("User.Roles.Functions")
                                            .Include("DistributionSubscribers")
                                            .Include("CampaignSubscribers")
                                            .FirstOrDefault(pb => pb.AccountID == primaryKey);

                    if (obj == default(Account))
                        return;

                    obj.StartEntityTracking();

                    // Check to make sure the Account does not have any orders (can't delete and account with orders) - JHE
                    // TODO: Make sure this works - JHE
                    var ordersCount = (from o in context.Orders
                                       where o.OrderCustomers.Any(oc => oc.AccountID == primaryKey)
                                       select o).Count();
                    if (ordersCount != 0)
                    {
                        var ex = new NetStepsBusinessException(string.Format("Error deleting Account# {0}. Accounts with Orders can not be deleted.", primaryKey));
                        throw ex;
                    }

                    foreach (var accountPaymentMethod in obj.AccountPaymentMethods)
                        context.DeleteObject(accountPaymentMethod.BillingAddress);
                    context.DeleteObjects(obj.AccountPaymentMethods);

                    if (obj.Addresses != null)
                    {
                        obj.Addresses.RemoveAllAndMarkAsDeleted(); // Linker Tables still need to use the Self-Tracking Entities to delete the object - JHE
                    }

                    if (obj.User != null)
                    {
                        if (obj.User.Roles != null)
                            obj.User.Roles.RemoveAll();
                        context.DeleteObject(obj.User);
                    }

                    if (obj.AccountContactTag != null)
                        context.DeleteObject(obj.AccountContactTag);

                    context.DeleteObjects(obj.AccountContactTags);
                    context.DeleteObjects(obj.AccountPhones);
                    context.DeleteObjects(obj.Notes);

                    context.DeleteObjects(obj.DistributionSubscribers);
                    context.DeleteObjects(obj.CampaignSubscribers);

                    context.DeleteObject(obj);

                    Save(obj, context);
                }
            });
        }

        public virtual void SaveNote(int accountId, Note note)
        {
            ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    //assuming we have a valid account by this point.
                    var account = context.Accounts.Single(a => a.AccountID == accountId);
                    note.Accounts.Add(account);
                    note.Save();
                }
            });
        }

        /// <summary>
        /// Loads an account with lots of child objects.
        /// </summary>
        public override Account LoadFull(int accountID)
        {
            var account = LoadFullFirstOrDefault(x => x.AccountID == accountID);

            if (account == null)
            {
                throw new NetStepsDataException(string.Format("No Account found with AccountID = {0}.", accountID));
            }

            return account;
        }

        public virtual Account LoadInfoCard(int accountID)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var account = context.Accounts.Include("AccountPhones").FirstOrDefault(x => x.AccountID == accountID);

                    if (account != null)
                    {
                        var enroller = context.Accounts.FirstOrDefault(a => a.AccountID == account.EnrollerID);
                        account.EnrollerInfo.FirstName = enroller.FirstName;
                        account.EnrollerInfo.LastName = enroller.LastName;
                        account.EnrollerInfo.AccountID = enroller.AccountID;
                    }

                    return account;
                }
            });
        }

        public virtual IEnumerable<Account> LoadInfoCardBatch(List<int> accountIDs)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = new NetStepsEntities())
                {
                    List<Account> accounts = new List<Account>();

                    foreach (int accountID in accountIDs)
                    {
                        accounts.Add(LoadInfoCard(accountID));
                    }

                    return accounts;
                }
            });
        }

        public override List<Account> LoadBatchFull(IEnumerable<int> accountIDs)
        {
            return LoadFullWhere(a => accountIDs.Contains(a.AccountID));
        }

        protected virtual Account LoadFullFirstOrDefault(Expression<Func<Account, bool>> predicate)
        {
            Contract.Requires<ArgumentNullException>(predicate != null);

            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    return LoadFullFirstOrDefault(predicate, context);
                }
            });
        }

        protected virtual Account LoadFullFirstOrDefault(Expression<Func<Account, bool>> predicate, NetStepsEntities context)
        {
            Contract.Requires<ArgumentNullException>(predicate != null);
            Contract.Requires<ArgumentNullException>(context != null);

            var account = context.Accounts
                .FirstOrDefault(predicate);

            if (account == null)
            {
                return null;
            }

            account.LoadRelations(context, Account.Relations.LoadFull);

            return account;
        }

        protected virtual List<Account> LoadFullWhere(Expression<Func<Account, bool>> predicate)
        {
            Contract.Requires<ArgumentNullException>(predicate != null);

            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    return LoadFullWhere(predicate, context);
                }
            });
        }

        protected virtual List<Account> LoadFullWhere(Expression<Func<Account, bool>> predicate, NetStepsEntities context)
        {
            Contract.Requires<ArgumentNullException>(predicate != null);
            Contract.Requires<ArgumentNullException>(context != null);

            var accounts = context.Accounts
                .Where(predicate)
                .ToList();

            accounts.LoadRelations(context, Account.Relations.LoadFull);

            return accounts;
        }

        public IAccount GetAccountByAccountID(int accountID)
        {
            using (var context = CreateContext())
            {
                Account account = context.Accounts.FirstOrDefault(a => a.AccountID == accountID);
                if (account == null)
                {
                    return null;
                }
                account.LoadRelations(context, Account.Relations.User);
                account.LoadRelations(context, Account.Relations.LoadInfoCard);
                return account;
            }
        }

        public int GetChildDistributorCount(int accountID)
        {
            using (var context = CreateContext())
            {
                return context.Accounts.Count(a => a.SponsorID == accountID && a.AccountTypeID == (int)Constants.AccountType.Distributor);
            }
        }

        private ObjectQuery<Account> CreateRelationQuery(NetStepsEntities context, string[] relations)
        {
            var accounts = context.Accounts;
            ObjectQuery<Account> query = accounts;
            foreach (var relation in relations)
            {
                query = query.Include(relation);
            }

            return query;
        }

        public Account LoadWithRelationsByAccountID(int accountID, params string[] relations)
        {
            using (var context = CreateContext())
            {
                var query = CreateRelationQuery(context, relations);
                var returnValue = query.FirstOrDefault(a => a.AccountID == accountID);
                return returnValue;
            }
        }

        public Account LoadWithRelationsByUserID(int userID, params string[] relations)
        {
            using (var context = CreateContext())
            {
                var query = CreateRelationQuery(context, relations);
                var returnValue = query.FirstOrDefault(a => a.UserID == userID);
                return returnValue;
            }
        }

        public Account LoadWithRelationsByEmailAddress(string email, bool includeProspects, params string[] relations)
        {
            using (var context = CreateContext())
            {
                var query = CreateRelationQuery(context, relations);
                var returnValue = query.FirstOrDefault(a => a.EmailAddress == email && a.AccountStatus.ReportAsActive && (includeProspects || a.AccountTypeID != (int)Constants.AccountType.Prospect));
                return returnValue;
            }
        }

        public Account LoadWithRelationsByAccountNumber(string accountNumber, params string[] relations)
        {
            using (var context = CreateContext())
            {
                var query = CreateRelationQuery(context, relations);
                var returnValue = query.FirstOrDefault(q => q.AccountNumber == accountNumber);
                return returnValue;
            }
        }

        public Dictionary<int, string> SlimSearch(string query)
        {
            int accountID = 0;
            try { accountID = int.Parse(query); }
            catch { }
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    return context.Accounts.Where(a =>
                         (a.FirstName.Contains(query)) ||
                         (a.LastName.Contains(query)) ||
                          (a.AccountID == accountID)
                         )
                        .Select(p => new
                        {
                            p.AccountID,
                            p.FirstName
                        })
                        .ToDictionary(a => a.AccountID, a => a.FirstName);
                }
            });
        }
        public AccountInformacion ListarAccountsInformacionAdicional(int AccountID)
        {
            AccountInformacion objAccountInformacion = null;
            try
            {
                using (SqlDataReader reader = DataAccess.GetDataReader("UspListarAccountsInformacionAdicional", new Dictionary<string, object>() { { "@AccountID", AccountID } }, "Core"))
                {

                    if (reader.HasRows)
                    {

                        if (reader.Read())
                        {
                            objAccountInformacion = new AccountInformacion();



                            if (!Convert.IsDBNull(reader["AccountID"])) objAccountInformacion.AccountID = Convert.ToInt32(reader["AccountID"]);

                            if (!Convert.IsDBNull(reader["AccountNumber"])) objAccountInformacion.AccountNumber = reader["AccountNumber"].ToString();

                            if (!Convert.IsDBNull(reader["PhoneNumber"])) objAccountInformacion.PhoneNumber = reader["PhoneNumber"].ToString();

                            if (!Convert.IsDBNull(reader["Address"])) objAccountInformacion.Address = reader["Address"].ToString();

                            if (!Convert.IsDBNull(reader["Address1"])) objAccountInformacion.Address1 = reader["Address1"].ToString();

                            if (!Convert.IsDBNull(reader["Address2"])) objAccountInformacion.Address2 = reader["Address2"].ToString();

                            if (!Convert.IsDBNull(reader["Address3"])) objAccountInformacion.Address3 = reader["Address3"].ToString();

                            if (!Convert.IsDBNull(reader["Attention"])) objAccountInformacion.Attention = reader["Attention"].ToString();

                            if (!Convert.IsDBNull(reader["City"])) objAccountInformacion.City = reader["City"].ToString();

                            if (!Convert.IsDBNull(reader["FirstName"])) objAccountInformacion.FirstName = reader["FirstName"].ToString();

                            if (!Convert.IsDBNull(reader["FirstNameAddress"])) objAccountInformacion.FirstNameAddress = reader["FirstNameAddress"].ToString();

                            if (!Convert.IsDBNull(reader["LastName"])) objAccountInformacion.LastName = reader["LastName"].ToString();

                            if (!Convert.IsDBNull(reader["LastNameAddress"])) objAccountInformacion.LastNameAddress = reader["LastNameAddress"].ToString();

                            if (!Convert.IsDBNull(reader["State"])) objAccountInformacion.State = reader["State"].ToString();

                            if (!Convert.IsDBNull(reader["TaxNumber"])) objAccountInformacion.TaxNumber = reader["TaxNumber"].ToString();

                            if (!Convert.IsDBNull(reader["Birthday"])) objAccountInformacion.Birthday = Convert.ToDateTime(reader["Birthday"]);

                            if (!Convert.IsDBNull(reader["AccTypeName"])) objAccountInformacion.AccountType = reader["AccTypeName"].ToString();

                            if (!Convert.IsDBNull(reader["Enrollment"])) objAccountInformacion.Enrollment = Convert.ToDateTime(reader["Enrollment"]);

                            if (!Convert.IsDBNull(reader["IsLocked"])) objAccountInformacion.IsLocked = Convert.ToBoolean(reader["IsLocked"]);

                            if (!Convert.IsDBNull(reader["AccStatusName"])) objAccountInformacion.StatusName = reader["AccStatusName"].ToString();

                            if (!Convert.IsDBNull(reader["Sponsor"])) objAccountInformacion.Sponsor = reader["Sponsor"].ToString();
                            objAccountInformacion.CareerAsTitle = "";
                            objAccountInformacion.PaidAsTitle = "";

                            TranslationCache term = new TranslationCache();
                            IEnumerable<TitleInformationByAccount> lista;
                            lista = Title.ListGetTitlesByAccount(objAccountInformacion.AccountID);
                            if (lista != null)
                            {
                                if (lista.Count() > 0)
                                {
                                    var queryPT = (from item in lista
                                                   where item.Name == "Paid As Title"
                                                   select item.Titulo);

                                    var queryCT = (from item in lista
                                                   where item.Name == "Career Title"
                                                   select item.Titulo);

                                    foreach (var member in queryCT)
                                        objAccountInformacion.CareerAsTitle = member.ToString();

                                    foreach (var member in queryPT)
                                        objAccountInformacion.PaidAsTitle = member.ToString();
                                }
                            }
                            return objAccountInformacion;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
            return objAccountInformacion ?? new AccountInformacion() { };
        }

        public PaginatedList<AccountWithoutSponsorSearchData> GetAccountsWithoutSponsor(AccountWithoutSponsorSearchParameters searchParameters)
        {
            try
            {
                PaginatedList<AccountWithoutSponsorSearchData> List = new PaginatedList<AccountWithoutSponsorSearchData>();
                List.TotalCount = 0;

                using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[GetAccountsWithoutSponsor]";
                    cmd.Parameters.AddWithValue("@AccountID", searchParameters.AccountID);
                    cmd.Parameters.AddWithValue("@PeriodID", searchParameters.PeriodID);
                    cmd.Parameters.AddWithValue("@PageNumber", searchParameters.PageNumber);
                    cmd.Parameters.AddWithValue("@PageSize", searchParameters.PageSize);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            AccountWithoutSponsorSearchData Account = new AccountWithoutSponsorSearchData();

                            #region [Assign Values]

                            Account.AccountID = Convert.ToInt32(reader["AccountID"]);
                            Account.Name = reader["Name"] != DBNull.Value ? reader["Name"].ToString() : null;
                            Account.AccountStatusTerm = reader["AccountStatusTerm"] != DBNull.Value ? reader["AccountStatusTerm"].ToString() : null;
                            Account.PeriodID = reader["PeriodID"] != DBNull.Value ? reader["PeriodID"].ToString() : null;
                            Account.City = reader["City"] != DBNull.Value ? reader["City"].ToString() : null;
                            Account.State = reader["State"] != DBNull.Value ? reader["State"].ToString() : null;
                            Account.Email = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : null;
                            Account.Address = reader["Address"] != DBNull.Value ? reader["Address"].ToString() : null;
                            Account.Telephone1 = reader["Telephone1"] != DBNull.Value ? reader["Telephone1"].ToString() : null;
                            Account.Telephone2 = reader["Telephone2"] != DBNull.Value ? reader["Telephone2"].ToString() : null;

                            #endregion
                            List.TotalCount = Convert.ToInt32(reader["TotalRecords"]);
                            List.Add(Account);
                        }
                    }
                }

                return List;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
    }
}
