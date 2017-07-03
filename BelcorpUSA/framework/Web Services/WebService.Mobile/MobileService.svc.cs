using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Web;
using System.Web.Script.Serialization;
using NetSteps.Common.Configuration;
using NetSteps.Common.Exceptions;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Security;
using NetSteps.WebService.Mobile.Models;
using Constants = NetSteps.Data.Entities.Constants;
using NetSteps.Commissions.Common;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.WebService.Mobile
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(MaxItemsInObjectGraph = 2147483647)]
    public class MobileService : IMobileService
    {
        #region Properties
        /// <summary>
        /// This should be used for testing only. Real CMS images can use the string extension ReplaceXMLFileUploadPathToken()
        /// </summary>
        private readonly string FILEUPLOADWEBPATH = ConfigurationManager.GetAppSetting<string>(ConfigurationManager.VariableKey.FileUploadWebPath).AppendForwardSlash();

	    private readonly List<int> COMPLETEDORDERSTATUSES = new List<int>
	    {
            Constants.OrderStatus.Paid.ToInt(),
            Constants.OrderStatus.Printed.ToInt(),
            Constants.OrderStatus.PartiallyShipped.ToInt(),
            Constants.OrderStatus.Shipped.ToInt()
        };

        internal static int _accountid
        {
            get
            {
                var id = 0;
	            if(HttpContext.Current.Items.Contains("accountid"))
	            {
                    int.TryParse(HttpContext.Current.Items["accountid"].ToString(), out id);
	            }

                return id;
            }
        }

        internal static string _accountnumber
        {
            get
            {
                var id = string.Empty;
	            if(HttpContext.Current.Items.Contains("accountnumber"))
	            {
                    id = HttpContext.Current.Items["accountnumber"].ToString();
	            }

                return id;
            }
        }

        internal static Site CurrentSite
        {
            get
            {
                Site value = null;

	            if(HttpContext.Current != null)
	            {
                    value = HttpContext.Current.Items["CurrentSite"] as Site;
	            }

                if (value == null)
                {
                    int siteid;
                    var sitenumber = System.Configuration.ConfigurationManager.AppSettings["nsBackofficeSiteID"];
                    if (!string.IsNullOrEmpty(sitenumber) && int.TryParse(sitenumber, out siteid))
                    {
                        try
                        {
                            value = Site.LoadFull(siteid);
                            HttpContext.Current.Items["CurrentSite"] = value;
                        }
                        catch { }
                    }
                }

                return value;
            }
            set
            {
	            if(HttpContext.Current != null)
	            {
	            	HttpContext.Current.Items["CurrentSite"] = value;
	            }
            }
        }

        internal static Language CurrentLanguage
        {
            get
            {
                Language value = null;

	            if(HttpContext.Current != null)
	            {
                    value = HttpContext.Current.Items["CurrentLanguage"] as Language;
	            }

                return value ?? Language.English;
            }
            set
            {
	            if(HttpContext.Current != null)
	            {
	            	HttpContext.Current.Items["CurrentLanguage"] = value;
	            }
            }
        }

        internal static Account CurrentAccount
        {
            get
            {
                Account value = null;

	            if(HttpContext.Current != null)
	            {
                    value = HttpContext.Current.Items["CurrentAccount"] as Account;
	            }

                if (value == null && _accountid > 0)
                {
                    try
                    {
                        value = Account.Load(_accountid);
                        HttpContext.Current.Items["CurrentAccount"] = value;
                    }
                    catch { }
                }

                return value;
            }
            set
            {
	            if(HttpContext.Current != null)
	            {
                    HttpContext.Current.Items["CurrentAccount"] = value;
				}
			}
        }
        #endregion

        #region Login
        public LoginModel Login(string username, string password)
        {
            var model = new LoginModel { LoginSuccess = false };
            try
            {
                Account account = Account.Authenticate(username, password);
                if (account.AccountID > 0)
                {
                    model.AccountNumber = account.AccountNumber;
                    model.Hash = Encryption.EncryptTripleDES(string.Format("{0}.{1}", account.AccountID, account.AccountNumber));
                    model.ProgramIdentifier = "5477199";
                    model.LoginSuccess = true;
                }
            }
            catch (NetStepsBusinessException excep)
            {
                model.ErrorMessage = excep.PublicMessage;
                model.LoginSuccess = false;
            }
            catch (Exception ex)
            {
                model.ErrorMessage = ex.Message;
                model.LoginSuccess = false;
            }

            return model;
        }

        public bool IsAccountActive(string accountNumber)
        {
            var account = Account.LoadByAccountNumber(accountNumber);
            return account.Active;
        }

        private Language FindLanguageWithCountry(string language, string country)
        {
            var languages = SmallCollectionCache.Instance.Languages;

            Language lang = null;

            if (!language.IsNullOrEmpty())
                switch (language)
                {
                    case "de":
                        lang = languages.FirstOrDefault(l => l.CultureInfo == "de-" + country.ToUpper());
                        break;
                    case "en":
                        if (country == "uk")
                            lang = languages.FirstOrDefault(l => l.CultureInfo == "en-GB");
                        else
                            lang = languages.FirstOrDefault(l => l.CultureInfo == "en-" + country.ToUpper());
                        break;
                    case "fr":
                        if (country == "de")
                            lang = languages.FirstOrDefault(l => l.CultureInfo == "fr");
                        else
                            lang = languages.FirstOrDefault(l => l.CultureInfo == "fr-" + country.ToUpper());
                        break;
                    case "cs":
                        lang = languages.FirstOrDefault(l => l.CultureInfo == "cs-" + country.ToUpper());
                        break;
                    case "dk":
                        lang = languages.FirstOrDefault(l => l.CultureInfo == "da-" + country.ToUpper());
                        break;
                    case "fi":
                        lang = languages.FirstOrDefault(l => l.CultureInfo == "fi-" + country.ToUpper());
                        break;
                    case "se":
                        lang = languages.FirstOrDefault(l => l.CultureInfo == "sv-" + country.ToUpper());
                        break;
                    case "it":
                        lang = languages.FirstOrDefault(l => l.CultureInfo == "it-" + country.ToUpper());
                        break;
                    case "es":
                        lang = languages.FirstOrDefault(l => l.CultureInfo == "es-" + country.ToUpper());
                        break;
                    case "no":
                        lang = languages.FirstOrDefault(l => l.CultureInfo == "no");
                        break;
                    case "pl":
                        lang = languages.FirstOrDefault(l => l.CultureInfo == "pl-" + country.ToUpper());
                        break;
                    case "sk":
                        lang = languages.FirstOrDefault(l => l.CultureInfo == "sk-" + country.ToUpper());
                        break;
                }

            return lang;
        }

        private Site FindSiteWithCountry(string country)
        {
            var baseSites = Site.LoadBaseSites();

            Site site = null;

            //todo: cleaner, nicer
            if (!country.IsNullOrEmpty())
                switch (country)
                {
                    case "at":
                        site = baseSites.FirstOrDefault(s => s.Name.StartsWith("Austria Distributor Back Office"));
                        break;
                    case "au":
                        site = baseSites.FirstOrDefault(s => s.Name.StartsWith("Australia BackOffice"));
                        break;
                    case "ca":
                        site = baseSites.FirstOrDefault(s => s.Name.StartsWith("Canada BackOffice"));
                        break;
                    case "cz":
                        site = baseSites.FirstOrDefault(s => s.Name.StartsWith("Czech Republic Distributor Back Office"));
                        break;
                    case "dk":
                        site = baseSites.FirstOrDefault(s => s.Name.StartsWith("Denmark Distributor Back Office"));
                        break;
                    case "fi":
                        site = baseSites.FirstOrDefault(s => s.Name.StartsWith("Finland Distributor Back Office"));
                        break;
                    case "fr":
                        site = baseSites.FirstOrDefault(s => s.Name.StartsWith("France Distributor Back Office"));
                        break;
                    case "de":
                        site = baseSites.FirstOrDefault(s => s.Name.StartsWith("Germany Distributor Back Office"));
                        break;
                    case "ie":
                        site = baseSites.FirstOrDefault(s => s.Name.StartsWith("Ireland Distributor Back Office"));
                        break;
                    case "it":
                        site = baseSites.FirstOrDefault(s => s.Name.StartsWith("Italy Distributor Back Office"));
                        break;
                    //case "mx":
                    //    site = baseSites.FirstOrDefault(s => s.Name.StartsWith("Mexico Distributor Back Office"));
                    //    break;
                    case "no":
                        site = baseSites.FirstOrDefault(s => s.Name.StartsWith("Norway Distributor Back Office"));
                        break;
                    case "pl":
                        site = baseSites.FirstOrDefault(s => s.Name.StartsWith("Poland Distributor Back Office"));
                        break;
                    case "sk":
                        site = baseSites.FirstOrDefault(s => s.Name.StartsWith("Slovakia Distributor Back Office"));
                        break;
                    case "se":
                        site = baseSites.FirstOrDefault(s => s.Name.StartsWith("Sweden Distributor Back Office"));
                        break;
                    case "ch":
                        site = baseSites.FirstOrDefault(s => s.Name.StartsWith("Switzerland Distributor Back Office"));
                        break;
                    case "uk":
                        site = baseSites.FirstOrDefault(s => s.Name.StartsWith("United Kingdom Distributor Back Office"));
                        break;
                    case "us":
                    default:
                        site = baseSites.FirstOrDefault(s => s.Name.StartsWith("BackOffice Base Site"));
                        break;
                }

            return site;
        }

        public DeviceModel RegisterDevice(string deviceid, Models.DeviceType devicetype, string country, string language, string accountnumber)
        {
            var model = new DeviceModel { registered = false, deviceid = deviceid, devicetype = devicetype };

            Account account = null;
            var deviceChanged = false;
            var accountChanged = false;

            try
            {
                account = Account.LoadByAccountNumber(accountnumber + "-" + country);
            }
            catch { }

            if (account == null)
            {
                account = new Account();
                account.AccountNumber = accountnumber + "-" + country;
                account.Language = FindLanguageWithCountry(language, country);
                account.AccountTypeID = 1;
                account.AccountStatusID = 1;
                account.FirstName = country;
                account.LastName = language;
            }
            else
            {
                account = Account.LoadFull(account.AccountID);
                var newLanguage = FindLanguageWithCountry(language, country);
                if (newLanguage.LanguageID != account.DefaultLanguageID)
                {
                    account.Language = newLanguage;
                    accountChanged = true;
                }
            }

            if (devicetype == Models.DeviceType.BlackBerry)
                devicetype = Models.DeviceType.iOS;

            try
            {
                var device = account.AccountDevices.FirstOrDefault(d => d.DeviceID == deviceid);
                if (device == null)
                {
                    device = new AccountDevice();
                    device.Account = account;
                    device.DeviceID = deviceid;
                    device.DeviceTypeID = (short)devicetype;
                    device.Active = true;
                    account.AccountDevices.Add(device);
                    deviceChanged = true;
                }
                else if (!device.Active)
                {
                    device.Active = true;
                    deviceChanged = true;
                }

                if (deviceChanged && devicetype == Models.DeviceType.Android)
                {
                    account.AccountDevices.Where(d => d.DeviceID != deviceid && d.DeviceTypeID == (short)devicetype && d.Active).Each(d => d.Active = false);
                }
            }
            catch (Exception e)
            {
                NetSteps.Data.Entities.Exceptions.ExceptionLogger.LogException(e, "Failed creating device: " + e.Message, true);
            }

            try
            {
                if (deviceChanged || accountChanged)
                    account.Save();
            }
            catch (Exception e)
            {
                NetSteps.Data.Entities.Exceptions.ExceptionLogger.LogException(e, "Failed saving account: " + e.Message, true);
            }

            model.registered = true;
            model.active = true;

            return model;
        }

        public DeviceModel UpdateDevice(string deviceid, Models.DeviceType devicetype, string country, string language, string accountnumber, bool active)
        {
            var model = new DeviceModel { registered = false, deviceid = deviceid, devicetype = devicetype };

            Account account = null;
            var deviceChanged = false;
            var accountChanged = false;

            try
            {
                account = Account.LoadByAccountNumber(accountnumber + "-" + country);
            }
            catch { }

            if (account == null)
            {
                account = new Account();
                account.AccountNumber = accountnumber + "-" + country;
                account.Language = FindLanguageWithCountry(language, country);
                account.AccountTypeID = 1;
                account.AccountStatusID = 1;
                account.FirstName = country;
                account.LastName = language;
            }
            else
            {
                account = Account.LoadFull(account.AccountID);
                var newLanguage = FindLanguageWithCountry(language, country);
                if (newLanguage.LanguageID != account.DefaultLanguageID)
                {
                    account.Language = newLanguage;
                    accountChanged = true;
                }
            }

            if (devicetype == Models.DeviceType.BlackBerry)
                devicetype = Models.DeviceType.iOS;
            else if (devicetype == Models.DeviceType.iOS)
                devicetype = Models.DeviceType.BlackBerry;

            var device = account.AccountDevices.FirstOrDefault(d => d.DeviceID == deviceid);
            if (device == null)
            {
                device = new AccountDevice();
                device.Account = account;
                device.DeviceID = deviceid;
                device.DeviceTypeID = (short)devicetype;
                device.Active = active;

                if (devicetype == Models.DeviceType.Android)
                {
                    account.AccountDevices.Where(d => d.DeviceID != deviceid && d.DeviceTypeID == (short)devicetype && d.Active).Each(d => d.Active = false);
                }

                account.AccountDevices.Add(device);
                deviceChanged = true;
            }
            //else if(device.Active != active)
            //{
            //    device.Active = active;
            //    deviceChanged = true;
            //}

            //entities is throwing an error if you save the same object twice for some reason
            //dont have time to investigate
            if (deviceChanged || accountChanged)
                account.Save();

            model.registered = true;
            model.active = active;

            return model;
        }

        public TranslationModel GetTerms()
        {
            var result = new TranslationModel();

            result.at["SignIn"] = "Anmelden";

            return result;
        }
        #endregion

        #region Home/News
        [AuthFilter]
        public List<NewsModel> GetNews(string lastUpdate)
        {
            var newsTypes = NewsType.LoadAllFullWithSqlDependency();
	        if(CurrentSite != null && (CurrentSite.News == null || CurrentSite.News.Count == 0))
	        {
                CurrentSite = Site.LoadFull(CurrentSite.SiteID);
	        }

            var result = new List<NewsModel>();
            CurrentSite.News.Where(n => n.Active && n.IsMobile && DateTime.Now.IsBetween(n.StartDate, n.EndDate)).Each(n => result.Add(new NewsModel(n, newsTypes)));

            #region mail/alerts
            //var mailAccount = MailAccount.LoadByAccountID(_accountid);
            //var folders = mailAccount.GetMailFolderCollection();
            //var sentbox = folders.FirstOrDefault(f => f.NameAsEnum == MailFolder.MailFolderNames.SentItems);

            //if (sentbox != null)
            //{
            //    var messages = sentbox.GetMailMessageCollection(mailAccount);
            //    var contactRequests = messages.Where(m => m.Subject == "Contact request").OrderByDescending(m => m.Date).Take(15);
            //    contactRequests.Each(m => result.Add(m));
            //    var hostRequests = messages.Where(m => m.Subject == "Host request").OrderByDescending(m => m.Date).Take(15);
            //    hostRequests.Each(m => result.Add(m));
            //}

            //var failedMessages = MailMessageFail.RetrieveMessages(mailAccount); //later perhaps
            #endregion

            DateTime filterDate = DateTime.MinValue;
	        if(!lastUpdate.IsNullOrEmpty() && DateTime.TryParse(lastUpdate, out filterDate))
	        {
                result = result.Where(n => !n.title.IsNullOrWhiteSpace() && n.isAlert ? DateTime.ParseExact(n.date, "dd-MM-yyyy", null) >= filterDate : true).ToList();
	        }

            result = result.Where(n => n != null && !n.title.IsNullOrWhiteSpace()).OrderBy(n => n.sort).ThenByDescending(n => DateTime.ParseExact(n.date, "dd-MM-yyyy", null)).ToList();
            result.Each(n => n.sort = result.IndexOf(n));
            return result;
        }
        #endregion

        #region Network
        public List<ContactModel> GetCustomersPaginated(int start = 0, int count = 25, string filter = "", string sort = "")
        {
            string filterStr, sortDir, sortProp;
            PrepareFilterSortPagination(ref start, ref count, filter, sort, out filterStr, out sortDir, out sortProp);

            var contacts = new List<ContactModel>();

            DataTable contactData = MobileRepository.MobileAccountSearch(_accountid, Constants.AccountType.RetailCustomer.ToInt(), Constants.AccountType.PreferredCustomer.ToInt());
            foreach (DataRow dataRow in contactData.Rows.AsParallel())
                contacts.Add(dataRow);

            IOrderedEnumerable<ContactModel> sortedContacts;
	        if(sortDir.ToLower() == "asc")
	        {
		        sortedContacts = sortProp.ToLower() == "lastname"
			        ? contacts.OrderBy(c => (!c.lastName.IsNullOrEmpty() ? c.lastName : c.firstName))
			        : contacts.OrderBy(c => c.sortorder.ToInt());
	        }
            else
	        {
                sortedContacts = sortProp.ToLower() == "lastname" ? contacts.OrderByDescending(c => (!c.lastName.IsNullOrEmpty() ? c.lastName : c.firstName)) : contacts.OrderByDescending(c => c.sortorder.ToInt());
	        }
            var filteredContacts = sortedContacts.Where(c => !filterStr.IsNullOrEmpty() ? string.Join(" ", c.firstName, c.lastName).ContainsIgnoreCase(filterStr) : true);

            return filteredContacts.Skip(start).Take(count).ToList();
        }

        public List<ContactModel> GetProspectsPaginated(int start = 0, int count = 25, string filter = "", string sort = "")
        {
            string filterStr, sortDir, sortProp;
            PrepareFilterSortPagination(ref start, ref count, filter, sort, out filterStr, out sortDir, out sortProp);
            var contacts = new List<ContactModel>();

            DataTable contactData = MobileRepository.MobileAccountSearch(_accountid, Constants.AccountType.Prospect.ToInt());
	        foreach(DataRow dataRow in contactData.Rows.AsParallel())
	        {
                contacts.Add(dataRow);
	        }

            IOrderedEnumerable<ContactModel> sortedContacts;
	        if(sortDir.ToLower() == "asc")
	        {
		        sortedContacts = sortProp.ToLower() == "lastname"
			        ? contacts.OrderBy(c => (!c.lastName.IsNullOrEmpty() ? c.lastName : c.firstName))
			        : contacts.OrderBy(c => c.sortorder.ToInt());
	        }
            else
	        {
                sortedContacts = sortProp.ToLower() == "lastname" ? contacts.OrderByDescending(c => (!c.lastName.IsNullOrEmpty() ? c.lastName : c.firstName)) : contacts.OrderByDescending(c => c.sortorder.ToInt());
	        }
            var filteredContacts = sortedContacts.Where(c => !filterStr.IsNullOrEmpty() ? string.Join(" ", c.firstName, c.lastName).ContainsIgnoreCase(filterStr) : true);

            return filteredContacts.Skip(start).Take(count).ToList();
        }
        #endregion

        #region Performance
        public List<ContactModel> GetTeamPaginated(string periodID = null, int start = 0, int count = 25, string filter = "", string sort = "")
        {
            var commissionService = Create.New<ICommissionsService>();

            string filterStr, sortDir, sortProp;
            PrepareFilterSortPagination(ref start, ref count, filter, sort, out filterStr, out sortDir, out sortProp);

	        if(periodID.IsNullOrWhiteSpace())
	        {
                periodID = commissionService.GetCurrentPeriod().PeriodId.ToString();
	        }

            var contacts = new List<ContactModel>();

            var downline = DownlineCache.GetDownline(periodID.ToInt());

            DataTable contactData = MobileRepository.MobileAccountSearch(_accountid, Constants.AccountType.Distributor.ToInt());
	        foreach(DataRow dataRow in contactData.Rows.AsParallel())
	        {
                contacts.Add(dataRow);
	        }

            if (downline != null)
            {
                foreach (var contact in contacts)
                {
                    int accountid;
                    int.TryParse(contact.accountid, out accountid);
                    if (accountid > 0 && downline.Lookup.ContainsKey(accountid))
                    {
                        var contactNode = downline.Lookup[accountid];
                        contact.SetCommissionsProperties(contactNode, ref downline);
                    }
                }
            }

            IOrderedEnumerable<ContactModel> sortedContacts;

            switch (sortProp.ToLower())
            {
                case "pv":
                    sortedContacts = sortDir.Equals("asc", StringComparison.OrdinalIgnoreCase)
                        ? contacts.OrderBy(c => c.pv.ToDecimal())
                        : contacts.OrderByDescending(c => c.pv.ToDecimal());
                    break;
                case "gv":
                    sortedContacts = sortDir.Equals("asc", StringComparison.OrdinalIgnoreCase)
                        ? contacts.OrderBy(c => c.gv.ToDecimal())
                        : contacts.OrderByDescending(c => c.gv.ToDecimal());
                    break;
				//case "lastname":
				//case "name":
                default:
                    sortedContacts = sortDir.Equals("asc", StringComparison.OrdinalIgnoreCase)
                        ? contacts.OrderBy(c => c.lastName).ThenBy(c => c.firstName)
                        : contacts.OrderByDescending(c => c.lastName).ThenByDescending(c => c.firstName);
                    break;
            }
            var filteredContacts = sortedContacts.Where(c => !filterStr.IsNullOrEmpty() ? string.Join(" ", c.firstName, c.lastName).ContainsIgnoreCase(filterStr) : true);

            return filteredContacts.Skip(start).Take(count).ToList();
        }

        public List<ContactModel> GetDownlinePaginated(string periodID = null, int start = 0, int count = 25, string filter = "", string sort = "")
        {
            var commissionService = Create.New<ICommissionsService>();

            var results = new List<ContactModel>();
            string filterStr, sortDir, sortProp;
            PrepareFilterSortPagination(ref start, ref count, filter, sort, out filterStr, out sortDir, out sortProp);

            if (periodID.IsNullOrWhiteSpace())
                periodID = commissionService.GetCurrentPeriod().PeriodId.ToString();

            var downline = DownlineCache.GetDownline(periodID.ToInt());

            var downlineAccounts = new Dictionary<int, dynamic>();
            GetFullDownLineByAccount(_accountid, ref downline, ref downlineAccounts);

            var dl = new List<dynamic>(downlineAccounts.Values);
            if (!filterStr.IsNullOrWhiteSpace())
            {
                dl.RemoveAll(n => !((string)n.FirstName + " " + (string)n.LastName).ContainsIgnoreCase(filterStr));
            }

            switch (sortProp.ToLower())
            {
                case "pv":
                    dl = sortDir.Equals("asc", StringComparison.OrdinalIgnoreCase)
                        ? dl.OrderBy(n => (n.PV > 0 ? n.PV : 0m)).ToList()
                        : dl.OrderByDescending(n => (n.PV > 0 ? n.PV : 0m)).ToList();
                    break;
                case "gv":
                    dl = sortDir.Equals("asc", StringComparison.OrdinalIgnoreCase)
                        ? dl.OrderBy(n => (n.GV > 0 ? n.GV : 0m)).ToList()
                        : dl.OrderByDescending(n => (n.GV > 0 ? n.GV : 0m)).ToList();
                    break;
				//case "lastname":
				//case "name":
                default:
                    dl = sortDir.Equals("asc", StringComparison.OrdinalIgnoreCase)
                        ? dl.OrderBy(n => n.LastName).ThenBy(n => n.FirstName).ToList()
                        : dl.OrderByDescending(n => n.LastName).ThenByDescending(n => n.FirstName).ToList();
                    break;
            }

            dl = dl.Skip(start).Take(count).ToList();

            dl.Each(n =>
            {
                int? accountID = n.AccountID;
                int? sponsorID = n.SponsorID;
                if (accountID.HasValue && sponsorID.HasValue)
                {
                    DataTable contactData = MobileRepository.MobileAccountSearch(sponsorID.Value, Constants.AccountType.Distributor.ToInt(), accountID: accountID.Value);
                    if (contactData != null && contactData.Rows.Count == 1)
                    {
                        var model = (ContactModel)contactData.Rows[0];
                        model.SetCommissionsProperties(n, ref downline);
                        results.Add(model);
                    }
                }
            });

            return results;
        }

        public List<KPIModel> GetPerformance(bool incremental = false)
        {
            var commissionService = Create.New<ICommissionsService>();
            
            var performance = new List<KPIModel>();

            var periods = commissionService.GetPeriodsForAccount(CurrentAccount.AccountID)
                .Where(p => p != null && p.DisbursementFrequency == Commissions.Common.Models.DisbursementFrequencyKind.Monthly && p.StartDateUTC <= DateTime.Now.ToUniversalTime())
                .OrderByDescending(p => p.StartDateUTC)
                .Take(5);

            if (periods.Any())
            {
                DateTime minDate = periods.Min(p => p.StartDateUTC.ToLocalTime()),
                                maxDate = periods.Max(p => p.EndDateUTC.ToLocalTime());

                foreach (var period in periods.Where(p => incremental ? p.IsOpen : true))
                {
                    var downline = DownlineCache.GetDownline(period.PeriodId);

                    var model = new KPIModel
                    {
                        periodid = period.PeriodId,
                        periodname = period.StartDateUTC.ToLocalTime().ToString("MMM yyy") + (period.IsOpen ? "*" : "")
                    };

                    if (downline.Lookup.ContainsKey(_accountid))
                    {
                        var node = downline.Lookup[_accountid];
                        model.pv = (node.PV > 0 ? node.PV : 0m);
                        model.gv = (node.GV > 0 ? node.GV : 0m);

                        int? titleID = (node as dynamic).CurrentTitle as int?,
                                paidAsTitleID = (node as dynamic).PaidAsTitle as int?;
                        model.title = (titleID.HasValue) ? commissionService.GetTitle(titleID.Value).TermName : Translation.GetTerm("N/A");
                        model.paidAsTitle = (paidAsTitleID.HasValue) ? commissionService.GetTitle(paidAsTitleID.Value).TermName : Translation.GetTerm("N/A");

                        if (downline.SponsorshipTree.ContainsKey(_accountid))
                        {
                            model.PersonallySponsoredCount = downline.SponsorshipTree[_accountid].Count();
                            var downlineAccounts = new Dictionary<int, dynamic>();
                            GetFullDownLineByAccount(_accountid, ref downline, ref downlineAccounts);
                            model.DownlineCount = downlineAccounts.Count();
                        }

                        var customWidgets = MobileRepository.MobilePerformanceWidgets(_accountid, period.PeriodId);
                        model.CustomKPIReports = new List<KPIRptWidget>();
                        customWidgets.Each(w => model.CustomKPIReports.Add(w));
                    }

                    performance.Add(model);
                }
            }

            return performance;
        }

        public List<OrderModel> GetPVOrdersPaginated(string periodID = null, int start = 0, int count = 25, string filter = "", string sort = "")
        {
            var commissionService = Create.New<ICommissionsService>();

            var results = new List<OrderModel>();
            string filterStr, sortDir, sortProp;
            PrepareFilterSortPagination(ref start, ref count, filter, sort, out filterStr, out sortDir, out sortProp);

	        if(periodID.IsNullOrWhiteSpace())
	        {
                periodID = commissionService.GetCurrentPeriod().PeriodId.ToString();
	        }

            var period = commissionService.GetPeriod(periodID.ToInt());
            DateTime pStart = period.StartDateUTC, pEnd = period.EndDateUTC;

            string orderby;
            switch (sortProp.ToLower())
            {
                case "name":
                    orderby = "LastName";
                    break;
                case "pv":
                    orderby = "CommissionableTotal";
                    break;
                case "type":
                    orderby = "OrderType.TermName";
                    break;
				//case "date":
                default:
                    orderby = "CompleteDateUTC";
                    break;
            }

            var orders = Order.Search(new OrderSearchParameters
            {
                ConsultantOrCustomerAccountID = CurrentAccount.AccountID,
                PageSize = count,
                PageIndex = start / count,
                WhereClause = o =>
                    (COMPLETEDORDERSTATUSES.Contains(o.OrderStatusID)
                    && o.CommissionDateUTC.HasValue
                    && pStart <= o.CommissionDateUTC
                    && pEnd >= o.CommissionDateUTC),
                OrderBy = orderby,
                OrderByDirection = sortDir.Equals("asc", StringComparison.OrdinalIgnoreCase) ? Common.Constants.SortDirection.Ascending : Common.Constants.SortDirection.Descending
            });

            orders.Each(o => results.Add(o));
            return results;
        }

        public List<OrderModel> GetGVOrdersPaginated(string periodID = null, int start = 0, int count = 25, string filter = "", string sort = "")
        {
            var commissionService = Create.New<ICommissionsService>();

            var results = new List<OrderModel>();
            string filterStr, sortDir, sortProp;
            PrepareFilterSortPagination(ref start, ref count, filter, sort, out filterStr, out sortDir, out sortProp);

            if (periodID.IsNullOrWhiteSpace())
                periodID = commissionService.GetCurrentPeriod().PeriodId.ToString();

            var period = commissionService.GetPeriod(periodID.ToInt());
            DateTime pStart = period.StartDateUTC, pEnd = period.EndDateUTC;

            var downline = DownlineCache.GetDownline(periodID.ToInt());
            var downlineAccounts = new Dictionary<int, dynamic>();
            GetFullDownLineByAccount(_accountid, ref downline, ref downlineAccounts);
            var dlIds = new List<int>(downlineAccounts.Keys);

            string orderby;
            switch (sortProp.ToLower())
            {
                case "name":
                    orderby = "LastName";
                    break;
                case "pv":
                    orderby = "CommissionableTotal";
                    break;
                case "type":
                    orderby = "OrderType.TermName";
                    break;
				//case "date":
                default:
                    orderby = "CompleteDateUTC";
                    break;
            }

            var orders = Order.Search(new OrderSearchParameters
            {
                PageSize = count,
                PageIndex = start / count,
                WhereClause = o =>
                    (COMPLETEDORDERSTATUSES.Contains(o.OrderStatusID)
                    && o.CommissionDateUTC.HasValue
                    && pStart <= o.CommissionDateUTC
                    && pEnd >= o.CommissionDateUTC
                    && dlIds.Contains(o.ConsultantID)),
                OrderBy = orderby,
                OrderByDirection = sortDir.Equals("asc", StringComparison.OrdinalIgnoreCase) ? Common.Constants.SortDirection.Ascending : Common.Constants.SortDirection.Descending
            });

            orders.Each(o => results.Add(o));
            return results;
        }

        /// <summary>
        /// A function to recursively lookup the given account's SponsorshipTree and add it to the result if it isn't already
        /// </summary>
        private void GetFullDownLineByAccount(int accountID, ref Downline downline, ref Dictionary<int, dynamic> downlineAccounts)
        {
            if (downline.SponsorshipTree.ContainsKey(accountID))
            {
                var tree = downline.SponsorshipTree[accountID];
                foreach (var node in tree)
                {
                    if (!downlineAccounts.ContainsKey(node.AccountID))
                    {
                        downlineAccounts.Add(node.AccountID, node);
                        GetFullDownLineByAccount(node.AccountID, ref downline, ref downlineAccounts);
                    }
                }
            }
        }
        #endregion

        #region Orders
        public List<OrderModel> GetOrders(string lastUpdate)
        {
            var orders = new List<OrderModel>();
            DateTime filterDate = DateTime.MinValue;

	        if(!lastUpdate.IsNullOrEmpty())
	        {
                DateTime.TryParse(lastUpdate, out filterDate);
	        }

            var collection = Order.Search(new OrderSearchParameters
            {
                ConsultantOrCustomerAccountID = CurrentAccount.AccountID,
                PageSize = null,
                WhereClause = o =>
                    (o.CompleteDateUTC.HasValue && o.CompleteDateUTC > DateTime.MinValue) ?
                        (o.CompleteDateUTC.Value.Month == filterDate.Month && o.CompleteDateUTC.Value.Year == filterDate.Year)
                    :
                        (o.DateCreatedUTC.Month == filterDate.Month && o.DateCreatedUTC.Year == filterDate.Year)
            });

            collection.Each(orderData =>
            {
                orders.Add(orderData);
            });

            return orders;
        }

        public List<OrderModel> GetPartyOrders()
        {
            var parties = new List<OrderModel>();

            //var pendingParties = new List<PendingOrder>();
            //var orderHistory = PendingOrderAdapter.GetPendingOrders(_accountid, (int?)Constants.OrderStatus.Entered);
            //foreach (var order in orderHistory)
            //{
            //    if (order == null || (order.Total == 0.00m && order.Name.IsNullOrEmpty()))
            //        continue;

            //    pendingParties.Add(order);
            //}

            //var partyInviteSetups = PartyInviteSetup.LoadByAccountId(_accountid);
            //foreach (var partyInviteSetup in partyInviteSetups.Where(pis => pis != null && pis.UseEvites))
            //{
            //    var pendingOrder = pendingParties.Where(o => o.OrderId == partyInviteSetup.OrderID).FirstOrDefault();
            //    if (pendingOrder != null)
            //    {
            //        var partyInvites = PartyInvite.LoadByOrderID(partyInviteSetup.OrderID);

            //        string date = string.Empty, partyDate = string.Empty, partyEndDate = string.Empty;
            //        if (pendingOrder.PartyDate.HasValue)
            //            date = partyDate = pendingOrder.PartyDate.ToShortDateString();
            //        else
            //            partyDate = "N/A";
            //        partyEndDate = pendingOrder.PartyEndDate.ToShortDateString();
            //        if (date.IsNullOrEmpty())
            //            date = pendingOrder.DateCreated.ToShortDateString();
            //        var type = (Constants.OrderType)pendingOrder.OrderTypeID;

            //        var party = new OrderModel
            //        {
            //            date = date,
            //            name = pendingOrder.Name,
            //            type = type.ToString().PascalToSpaced(),
            //            partydate = partyDate,
            //            partyenddate = partyEndDate,
            //            hostess = pendingOrder.Hostess
            //        };

            //        var metadata = OrderMetaData.LoadByOrderID(pendingOrder.OrderId);
            //        var time = string.Empty;
            //        if (pendingOrder.PartyDate.HasValue)
            //            time = pendingOrder.PartyDate.Value.ToShortTimeString();

            //        var partyDetail = new PartyDetailModel
            //        {
            //            inviteCount = partyInvites.Count,
            //            guestsAttending = partyInvites.Count(pi => pi.AttendanceStatus == PartyInvite.AttendanceStatuses.Attending.ToString()),
            //            guestsNotAttending = partyInvites.Count(pi => pi.AttendanceStatus == PartyInvite.AttendanceStatuses.Declined.ToString()),
            //            onlinePurchases = pendingOrder.Total.ToString("0.00"),
            //            address1 = metadata.PartyAddressAddress1,
            //            address2 = metadata.PartyAddressAddress2,
            //            city = metadata.PartyAddressCity,
            //            state = metadata.PartyAddressState,
            //            postalCode = metadata.PartyAddressPostalCode,
            //            date = date,
            //            time = time
            //        };

            //        var guests = new List<ContactModel>();
            //        foreach (var partyInvite in partyInvites)
            //        {
            //            var guest = new ContactModel
            //            {
            //                firstName = partyInvite.Contact.FirstName,
            //                lastName = partyInvite.Contact.LastName,
            //                rsvp = partyInvite.AttendanceStatus == PartyInvite.AttendanceStatuses.Attending.ToString() ? true
            //                    : partyInvite.AttendanceStatus == PartyInvite.AttendanceStatuses.Declined.ToString() ? false : (bool?)null,
            //                customer = partyInvite.Contact.IsPartyCustomer
            //            };
            //            guests.Add(guest);
            //        }

            //        partyDetail.guests = guests;
            //        party.partydetail = partyDetail;
            //        parties.Add(party);
            //    }
            //}

            return parties;
        }
        #endregion

        #region Common Helpers
        private void PrepareFilterSortPagination(ref int start, ref int count, string filter, string sort, out string filterStr, out string sortDir, out string sortProp)
        {
	        if(start < 0)
	        {
                start = 0;
	        }
	        if(count <= 0)
	        {
                count = 25;
	        }

            filterStr = string.Empty;
            if (!filter.IsNullOrEmpty())
                try
                {
                    var serializer = new JavaScriptSerializer();
                    var jsonObj = serializer.Deserialize<dynamic>(filter);
                    filterStr = jsonObj[0]["value"];
                }
				catch (Exception e)
				{
					Data.Entities.Exceptions.ExceptionLogger.LogException(e, true);
				}

            sortDir = "asc";
            sortProp = "lastName";
            try
            {
                var serializer = new JavaScriptSerializer();
                var jsonObj = serializer.Deserialize<dynamic>(sort);
                sortProp = jsonObj[0]["property"];
                sortDir = jsonObj[0]["direction"];
            }
			catch (Exception e)
			{
				Data.Entities.Exceptions.ExceptionLogger.LogException(e, true);
			}
        }
        #endregion

        #region Option Request Hacks
        // Until we find a better way, create a void implementation for each endpoint to handle OPTIONS requests so your normal requests dont get called twice
        public void RegisterDevice_Options() { }
        public void UpdateDevice_Options() { }
        public void GetTerms_Options() { }
        public void GetNews_Options() { }
        public void GetCustomersPaginated_Options() { }
        public void GetProspectsPaginated_Options() { }
        public void GetTeamPaginated_Options() { }
        public void GetDownlinePaginated_Options() { }
        public void GetPerformance_Options() { }
        public void GetOrders_Options() { }
        public void GetPartyOrders_Options() { }
        public void GetPVOrdersPaginated_Options() { }
        public void GetGVOrdersPaginated_Options() { }
        #endregion
    }

    sealed class AuthFilterAttribute : Attribute, IOperationBehavior
    {
        void IOperationBehavior.AddBindingParameters(OperationDescription operationDescription, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {

        }

        void IOperationBehavior.ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
        {

        }

        void IOperationBehavior.ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
        {
            dispatchOperation.ParameterInspectors.Add(new AuthFilterBehavior());
        }

        void IOperationBehavior.Validate(OperationDescription operationDescription)
        {

        }
    }

    internal class AuthFilterBehavior : IParameterInspector
    {
        public void AfterCall(string operationName, object[] outputs, object returnValue, object correlationState)
        {

        }

        public object BeforeCall(string operationName, object[] inputs)
        {
            var decryptedhash = string.Empty;

            if (HttpContext.Current.Request.Params.AllKeys.Contains("hash"))
            {
                string hash = HttpContext.Current.Request.Params["hash"];

                try
                {
                    decryptedhash = Encryption.DecryptTripleDES(hash);
                }
	            catch(Exception e)
				{
					Data.Entities.Exceptions.ExceptionLogger.LogException(e, true);
	            }

                if (!decryptedhash.IsNullOrEmpty())
                {
                    var hasharr = decryptedhash.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                    if (hasharr.Length == 2)
                    {
                        string accountnumber = hasharr[1] + "";
                        int slashIndex = (accountnumber ?? string.Empty).LastIndexOf("\\");
	                    if(slashIndex >= 0)
	                    {
                            accountnumber = accountnumber.Substring(slashIndex + 1);
	                    }

	                    int accountid;
                        if (int.TryParse(hasharr[0], out accountid))
                        {
                            HttpContext.Current.Items["accountid"] = accountid;
                            HttpContext.Current.Items["accountnumber"] = accountnumber;
                            return null;
                        }
                    }
                }
            }

            throw new UnauthorizedAccessException();
        }
    }
}