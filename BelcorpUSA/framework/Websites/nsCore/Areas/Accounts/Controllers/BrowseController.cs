using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using NetSteps.Common.Exceptions;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Extensions;
using NetSteps.Web.Mvc.Helpers;
using nsCore.Areas.Accounts.Models.Browse;
using System.Collections.Generic;
using System.Data;
using NetSteps.Data.Entities.Business.Logic;
using System.Xml;
using System.Globalization;

namespace nsCore.Areas.Accounts.Controllers
{
    public class BrowseController : BaseAccountsController
    {
        [OutputCache(CacheProfile = "AutoCompleteData")]
        public virtual ActionResult Search(string query)
        {            
            try
            {
                return Json(AccountCache.GetAccountSearchByTextResults(query).ToAJAXSearchResults());
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "AutoCompleteData")]
        public virtual ActionResult SearchOnAccountStatuses(string query)
        {
            try
            {
                int?[] filterStatusIDs = new int?[] 
                    {
                        (int)Constants.AccountStatus.Active,
                        (int)Constants.AccountStatus.BegunEnrollment,
                        (int)Constants.AccountStatus.Imported,
                        (int)Constants.AccountStatus.NotSet
                    };
                return Json(Account.SlimSearchOnAccountStatuses(query, null, filterStatusIDs,null).ToAJAXSearchResults());
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "AutoCompleteData")]
        public virtual ActionResult SearchActive(string query)
        {
            try
            {
                return Json(AccountCache.GetAccountSearchByTextAndAccountStatusResults(query, (int)Constants.AccountStatus.Active).ToAJAXSearchResults());
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "AutoCompleteData")]
        public virtual ActionResult SearchDistributors(string query)
        {
            try
            {
                return Json(AccountCache.GetAccountSearchByTextAndAccountTypeResults(query, (int)Constants.AccountType.Distributor).ToAJAXSearchResults());
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "AutoCompleteData")]
        public virtual ActionResult SearchActiveDistributorsCEP(string query)
        {
            try
            {
               
               Dictionary<int, string> dcAccount=null;
               if (HttpContext.Session["EsBusquedaCep"] != null)
               {
                   Boolean EsBusquedaCep = false;
                   EsBusquedaCep = (Boolean)HttpContext.Session["EsBusquedaCep"];
                   if (!EsBusquedaCep)
                   {
                       dcAccount = AccountCache.GetAccountSearchByTextAccountTypeAndAccountStatusResults(query, (int)Constants.AccountType.Distributor, (int)Constants.AccountStatus.Active);
                   }
                   else
                   {
                       Site oSiteActual = CurrentSite;
                       dcAccount = AccountSponsorBusinessLogic.Instance.ListarCuentasPorCodigoPostal(query, oSiteActual.MarketID);
                   }
               }
               else {
                   dcAccount = AccountCache.GetAccountSearchByTextAccountTypeAndAccountStatusResults(query, (int)Constants.AccountType.Distributor, (int)Constants.AccountStatus.Active);
               }
                
              
                 return Json(dcAccount.ToAJAXSearchResults());
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        /// <summary>
        /// mediante esta metodo se realiza la busqueda de una cuenta por medio del  Codigo Cep |AccountID
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [OutputCache(CacheProfile = "AutoCompleteData")]
        public virtual ActionResult SearchActiveDistributors(string query)
        {
            try
            {
                Dictionary<int, string> dcAccount = null;
                dcAccount = AccountCache.GetAccountSearchByTextAccountTypeAndAccountStatusResults(query, (int)Constants.AccountType.Distributor, (int)Constants.AccountStatus.Active);
                return Json(dcAccount.ToAJAXSearchResults());
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

          [HttpPost]
        public ActionResult AsignarTipoBusqueda( bool EsBusquedaCep)
        {
            try
            {
                HttpContext.Session["EsBusquedaCep"] = EsBusquedaCep;
                return Json(new { resultado=1});
            }
            catch (Exception ex)
            {

                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }


       
         
        //CAMBIO ENCORE-4
        #region Anterior
        //[FunctionFilter("Accounts", "~/Sites")]
        //[OutputCache(CacheProfile = "DontCache")]
        //public virtual ActionResult Index(string q, short? status, short? type, int? state, string city, string postalCode, int? country, string email, string coApplicant, int? sponsor, DateTime? startDate, DateTime? endDate, string siteUrl, string phone, int? title)
        //{
        //    try
        //    {
        //        AccountSearchParameters searchParams = new AccountSearchParameters();                
        //        try
        //        {
        //            if (!string.IsNullOrEmpty(q) || status.HasValue || type.HasValue || state.HasValue || !string.IsNullOrEmpty(city) || !string.IsNullOrEmpty(postalCode) || country.HasValue || !string.IsNullOrEmpty(email) || !string.IsNullOrEmpty(coApplicant) || sponsor.HasValue || startDate.HasValue || endDate.HasValue || !string.IsNullOrEmpty(siteUrl) || !string.IsNullOrEmpty(phone) || title.HasValue)
        //            {
        //                ViewData["q"] = q;
        //                searchParams.AccountStatusID = status;
        //                searchParams.AccountTypes = type.HasValue ? new[] { type.Value } : null;
        //                searchParams.StateProvinceID = state;
        //                searchParams.City = city;
        //                searchParams.PostalCode = postalCode;
        //                searchParams.CountryID = country;
        //                searchParams.SiteUrl = siteUrl;
        //                searchParams.PhoneNumber = phone;
        //                searchParams.Email = email;
        //                searchParams.CoApplicant = coApplicant;
        //                searchParams.SponsorID = sponsor;
        //                searchParams.CurrentAccountID = ApplicationContext.Instance.CorporateAccountID;
        //                searchParams.StartDate = startDate;
        //                searchParams.EndDate = endDate;
        //                searchParams.TitleID = title;
        //                searchParams.WhereClause = !string.IsNullOrEmpty(q) ? a => a.AccountNumber.Contains(q) || (a.FirstName + " " + a.LastName).Contains(q) : (Expression<Func<Account, bool>>)null;

        //                var accounts = Account.Search(searchParams);
        //                if (accounts.TotalCount == 1)
        //                {
        //                    return RedirectToAction("Index", "Overview", new { id = accounts.First().AccountNumber });
        //                }

        //                if (accounts.Any(a => a.AccountNumber == q))
        //                {
        //                    return RedirectToAction("Index", "Overview", new { id = q });
        //                }
        //            }
        //        }
        //        catch (NetStepsDataException ex)
        //        {
        //            //Catch Invalid Account Number exceptions and send user to the browse page showing no results
        //            EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
        //        }

        //        return View(new AccountBrowseModel(searchParams));
        //    }
        //    catch (Exception ex)
        //    {
        //        var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
        //        throw exception;
        //    }
          //}
        #endregion
          [FunctionFilter("Accounts", "~/Sites")]
          [OutputCache(CacheProfile = "DontCache")]
          public virtual ActionResult Index(string q, short? status, short? type, int? state, string city, string postalCode, int? country, string email, string coApplicant, int? sponsor, DateTime? startDate, DateTime? endDate, string siteUrl, string phone, int? title, string firstName, string lastName, string ssn, string gender)
          {
              try
              {
                  AccountSearchParameters searchParams = new AccountSearchParameters();
                  //ENCORE_4 Parametrizable
                  int iCountry = System.Configuration.ConfigurationManager.AppSettings["EnvironmentCountry"].ToInt();
                  var countryDate = SmallCollectionCache.Instance.Countries.GetById(iCountry);

                  try
                  {
                      if (!string.IsNullOrEmpty(q) || status.HasValue || type.HasValue || state.HasValue || !string.IsNullOrEmpty(city) || !string.IsNullOrEmpty(postalCode) || country.HasValue || !string.IsNullOrEmpty(email) || !string.IsNullOrEmpty(coApplicant) || sponsor.HasValue || startDate.HasValue || endDate.HasValue || !string.IsNullOrEmpty(siteUrl) || !string.IsNullOrEmpty(phone) || title.HasValue || !string.IsNullOrEmpty(firstName) || !string.IsNullOrEmpty(lastName) || !string.IsNullOrEmpty(ssn) || !string.IsNullOrEmpty(gender))
                      {

                          //IFormatProvider culture = new CultureInfo(countryDate.CultureInfo, true);
                                                   
                          ViewData["q"] = q;
                          searchParams.AccountStatusID = status;
                          searchParams.AccountTypes = type.HasValue ? new[] { type.Value } : null;
                          searchParams.StateProvinceID = state;
                          searchParams.City = city;
                          searchParams.PostalCode = postalCode;
                          searchParams.CountryID = country;
                          searchParams.SiteUrl = siteUrl;
                          searchParams.PhoneNumber = phone;
                          searchParams.Email = email;
                          searchParams.CoApplicant = coApplicant;
                          searchParams.SponsorID = sponsor;
                          searchParams.CurrentAccountID = ApplicationContext.Instance.CorporateAccountID;
                          if (startDate.HasValue)
                          {
                              string sStartDate = Convert.ToDateTime(startDate).ToString("yyyy/MM/dd");
                              //searchParams.StartDate = Convert.ToDateTime(sStartDate, culture);
                              searchParams.StartDate = Convert.ToDateTime(sStartDate, CoreContext.CurrentCultureInfo);
                          }

                          if (endDate.HasValue)
                          {
                              string sEndDate = Convert.ToDateTime(endDate).ToString("yyyy/MM/dd");
                             // searchParams.EndDate = Convert.ToDateTime(sEndDate, culture);
                              searchParams.EndDate = Convert.ToDateTime(sEndDate, CoreContext.CurrentCultureInfo);
                              
                          }
                          
                          searchParams.TitleID = title;
                          searchParams.WhereClause = !string.IsNullOrEmpty(q) ? a => a.AccountNumber.Contains(q) || (a.FirstName + " " + a.LastName).Contains(q) : (Expression<Func<Account, bool>>)null;
                          searchParams.FirstName = firstName;
                          searchParams.LastName = lastName;
                          searchParams.SSN = ssn;
                          searchParams.gender = gender;


                          var accounts = Account.Search(searchParams);
                          if (accounts.TotalCount == 1)
                          {
                              return RedirectToAction("Index", "Overview", new { id = accounts.First().AccountNumber });
                          }

                          if (accounts.Any(a => a.AccountNumber == q))
                          {
                              return RedirectToAction("Index", "Overview", new { id = q });
                          }
                      }
                  }
                  catch (NetStepsDataException ex)
                  {
                      //Catch Invalid Account Number exceptions and send user to the browse page showing no results
                      EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                  }

                  //ENCORE_4 Parametrizable
                  ViewData["countryID"] = countryDate.CountryCode3;

                  return View(new AccountBrowseModel(searchParams));
              }
              catch (Exception ex)
              {
                  var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                  throw exception;
              }
          }

        [OutputCache(CacheProfile = "PagedGridData")]
          public virtual ActionResult GetAccounts(int page, int pageSize, short? status, short? type, int? state, string city, string postalCode, int? country, string email, string coApplicant, int? sponsorId, int? title, DateTime? startDate, DateTime? endDate, string siteUrl, string phone, string accountNumberOrName, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection, string numCPF, string firstName, string lastName, string ssn, string gender)
        {
            try
            {
                if (country == null) country = System.Configuration.ConfigurationManager.AppSettings["EnvironmentCountry"].ToInt();

                //ENCORE_4 Parametrizable
                var countryDate = SmallCollectionCache.Instance.Countries.GetById(country.ToInt());
                IFormatProvider culture = new CultureInfo(countryDate.CultureInfo, true);
                string sStartDate = null, sEndDate=null;

                if (startDate.HasValue)
                {
                    sStartDate = Convert.ToDateTime(startDate).ToString("yyyy/MM/dd");
                }

                if (endDate.HasValue)
                {
                    sEndDate = Convert.ToDateTime(endDate).ToString("yyyy/MM/dd");
                }
                
                StringBuilder builder = new StringBuilder();
                var accounts = Account.Search(new AccountSearchParameters()
                {
                    AccountStatusID = status,
                    AccountTypes = type.HasValue ? new[] { type.Value } : null,
                    StateProvinceID = state,
                    City = city,
                    PostalCode = postalCode,
                    CountryID = country,
                    SiteUrl = siteUrl,
                    PhoneNumber = phone,
                    Email = email,
                    CoApplicant = coApplicant,
                    CPF = numCPF,
                    SponsorID = sponsorId,
					CurrentAccountID = ApplicationContext.Instance.CorporateAccountID,
                    StartDate = Convert.ToDateTime(sStartDate, culture).StartOfDay(),//startDate.StartOfDay(),
                    EndDate = Convert.ToDateTime(sEndDate, culture).EndOfDay(),//endDate.EndOfDay(),
                    TitleID = title,
                    WhereClause = !string.IsNullOrEmpty(accountNumberOrName) ? a => a.AccountNumber.Contains(accountNumberOrName) || (a.FirstName + " " + a.LastName).Contains(accountNumberOrName) : (Expression<Func<Account, bool>>)null,
                    PageIndex = page,
                    PageSize = pageSize,
                    OrderBy = orderBy,
                    OrderByDirection = orderByDirection,
                    FirstName = firstName,
                    LastName = lastName,
                    SSN = ssn,
                    gender = gender

                });
                if (accounts.Count > 0)
                {
                    int count = 0;
                    foreach (AccountSearchData account in accounts)
                    {
                        AppendAccountRow(builder, account);
                        ++count;
                    }
                    return Json(new { totalPages = accounts.TotalPages, page = builder.ToString() });
                }
                else
                {
                    return Json(new { totalPages = 0, page = "<tr><td colspan=\"9\">" + Translation.GetTerm("ThereWereNoRecordsFoundThatMeetThatCriteriaPleaseTryAgain", "There were no records found that meet that criteria. Please try again.") + "</td></tr>" });
                }
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        //FIN DE CAMBIO ENCORE-4
        protected virtual void AppendAccountRow(StringBuilder builder, AccountSearchData account)
        {
            builder.Append("<tr>")
                .AppendLinkCell("~/Accounts/Overview/Index/" + account.AccountNumber, account.AccountNumber)
                .AppendCell(account.FirstName)
                .AppendCell(account.LastName)
                .AppendCell(account.AccountType)
                .AppendCell(SmallCollectionCache.Instance.AccountStatuses.GetById(account.AccountStatusID).GetTerm())
                .AppendCell(account.DateEnrolled.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo))
                .AppendCell(account.EmailAddress)
                .AppendCell(account.Sponsor.IsNullOrEmpty() ? string.Empty : string.Format("{0} (#{1})", account.Sponsor, account.SponsorAccountNumber))
                .AppendCell(account.Location)
                .AppendCell(account.Gender)
                .Append("</tr>");
        }

        [OutputCache(CacheProfile = "AutoCompleteData")]
        public virtual ActionResult SearchCity(string query)
        {
            try
            {
                return Json(TaxCache.SearchCity(query).ToDictionary(t => t).ToAJAXSearchResults());
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "AutoCompleteData")]
        public virtual ActionResult SearchPostalCode(string query)
        {
            try
            {
                return Json(TaxCache.SearchPostalCode(query).ToDictionary(t => t).ToAJAXSearchResults());
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
    }
}
