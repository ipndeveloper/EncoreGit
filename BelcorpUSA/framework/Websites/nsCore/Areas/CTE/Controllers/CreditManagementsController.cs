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
using nsCore.Areas.Accounts.Controllers;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Web.Mvc.Business.Controllers;
using NetSteps.Data.Entities.Repositories;

namespace nsCore.Areas.CTE.Controllers
{
    public class CreditManagementsController : BaseController
    {

        [FunctionFilter("Accounts", "~/Sites")]
        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult Index(string q, short? status, short? type, int? state, string city, string postalCode, int? country, string email, string coApplicant, int? sponsor, DateTime? startDate, DateTime? endDate, string siteUrl, string phone, int? title)
        {
            try
            {
                AccountSearchParameters searchParams = new AccountSearchParameters();
                try
                {
                    if (!string.IsNullOrEmpty(q) || status.HasValue || type.HasValue || state.HasValue || !string.IsNullOrEmpty(city) || !string.IsNullOrEmpty(postalCode) || country.HasValue || !string.IsNullOrEmpty(email) || !string.IsNullOrEmpty(coApplicant) || sponsor.HasValue || startDate.HasValue || endDate.HasValue || !string.IsNullOrEmpty(siteUrl) || !string.IsNullOrEmpty(phone) || title.HasValue)
                    {
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
                        searchParams.StartDate = startDate;
                        searchParams.EndDate = endDate;
                        searchParams.TitleID = title;
                        searchParams.WhereClause = !string.IsNullOrEmpty(q) ? a => a.AccountNumber.Contains(q) || (a.FirstName + " " + a.LastName).Contains(q) : (Expression<Func<Account, bool>>)null;

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

                return View(new AccountBrowseModel(searchParams));
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }


        

 

        protected virtual void AppendAccountRow(StringBuilder builder, AccountCreditSearchData account)
        {
            builder.Append("<tr>")
                .AppendCheckBoxCell(value: account.AccountID.ToString())
                //.AppendLinkCell("~/Accounts/Overview/Index/" + account.AccountNumber, account.AccountNumber)
                .AppendCell(account.AccountNumber)
                 //.AppendCell("<a href='javascript:void(0)' onclick='PopupSaldoAsignar(" + account.AccountNumber + "," + account.AccountCreditUti + ",1)'>" + account.AccountNumber + "</a>")

                .AppendCell(account.FirstName)
                .AppendCell(account.LastName)
                .AppendCell(account.AccountType)
                .AppendCell(SmallCollectionCache.Instance.AccountStatuses.GetById(account.AccountStatusID).GetTerm())
                //.AppendCell(account.DateEnrolled.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo))
                //.AppendCell(account.EmailAddress)
                //.AppendCell(account.Sponsor.IsNullOrEmpty() ? string.Empty : string.Format("{0} (#{1})", account.Sponsor, account.SponsorAccountNumber))
                //.AppendCell(account.Location)
                  .AppendCell(account.AccountCreditAsg)
                  .AppendCell(account.AccountCreditUti.Replace(".", ",").ToDecimal().ToString("N"))
                  .AppendCell(account.AccountCreditDis.Replace(".", ",").ToDecimal().ToString("N"))
                  .AppendCell(account.AccountCreditEst)
                  .AppendCell(account.AccountCreditAnt.Replace(".", ",").ToDecimal().ToString("N"))
                  .AppendCell(account.AccountCreditFec)
                .Append("</tr>");
        }


        public string GetCulturaFormat(string valor)
        {
            bool correcto = false;
            decimal numero = 0;

            var formatos = new List<System.Globalization.CultureInfo>
            {
                new System.Globalization.CultureInfo("pt-BR"),
                new System.Globalization.CultureInfo("en-US"),
                new System.Globalization.CultureInfo("es-US")

            };

            if (!formatos.Any(x => x.Name == CoreContext.CurrentCultureInfo.Name))
                formatos.Add(CoreContext.CurrentCultureInfo);
            


            if (string.IsNullOrEmpty(valor))
                return numero.ToString("N", CoreContext.CurrentCultureInfo);


            foreach (var item in formatos)
            {

                if (decimal.TryParse(valor, System.Globalization.NumberStyles.AllowDecimalPoint, item, out numero) == true)
                {
                    correcto = true;
                    break;
                }



            }
            if (correcto)
            {
               
                    return numero.ToString("N", CoreContext.CurrentCultureInfo);

            }
            else
                return valor;


        }


        [OutputCache(CacheProfile = "PagedGridData")]
        public virtual ActionResult GetAccounts(int page, int pageSize, short? status, short? type, int? state, string city, string postalCode, int? country, string email, string coApplicant, int? sponsorId, int? title, DateTime? startDate, DateTime? endDate, int? accountID, string siteUrl, string phone, string accountNumberOrName, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection)
        {
            try
            {
                short? c = null;
                if (type.HasValue) c= type.Value;

                NetSteps.Data.Entities.Repositories.AccountCreditRepository rep = new NetSteps.Data.Entities.Repositories.AccountCreditRepository();
                StringBuilder builder = new StringBuilder();
                var accounts =  rep.SearchAccountCredit(new AccountSearchParameters()
                {
                    AccountID = accountID,
                    AccountStatusID = status,
                    AccountTypeID = c,
                    StateProvinceID = state,
                    City = city,
                    PostalCode = postalCode,
                    CountryID = country,
                    SiteUrl = siteUrl,
                    PhoneNumber = phone,
                    Email = email,
                    CoApplicant = coApplicant,
                    SponsorID = sponsorId,
                    CurrentAccountID = ApplicationContext.Instance.CorporateAccountID,
                    StartDate = startDate,
                    EndDate = endDate,
                    TitleID = title,
                    PageIndex = page,
                    PageSize = pageSize,
                    OrderBy = orderBy,
                    OrderByDirection = orderByDirection
                });



                if (accounts.Count > 0)
                {


                    foreach (var account in accounts)
                    {
                        builder.Append((account.AccountCreditEst == "S") ? "<tr  style='color:red'>" : "<tr>");

                        builder
                        .AppendCheckBoxCell(value: account.AccountID.ToString())
                        .AppendCell(account.AccountNumber)
                       
                        .AppendCell("<a href='javascript:void(0)' onclick='PopupAccountCreditLog(" + account.AccountNumber + ")'>" + account.FirstName + "</a>")                       
                        .AppendCell(account.LastName)
                        .AppendCell(account.AccountType)
                        .AppendCell(account.AccountStatus)

                        //.AppendCell(Convert.ToDecimal(account.AccountCreditAsg).ToString("N" , CoreContext.CurrentCultureInfo))
                        //.AppendCell(Convert.ToDecimal(account.AccountCreditUti).ToString("N", CoreContext.CurrentCultureInfo))
                        //.AppendCell(Convert.ToDecimal(account.AccountCreditDis).ToString("N", CoreContext.CurrentCultureInfo))


                        .AppendCell(GetCulturaFormat(account.AccountCreditAsg))
                        .AppendCell(GetCulturaFormat(account.AccountCreditUti))
                        .AppendCell(GetCulturaFormat(account.AccountCreditDis))




                        //.AppendCell(account.AccountCreditAsg.Replace(".", ",").ToDecimal().ToString("N"))
                        //.AppendCell(account.AccountCreditUti.Replace(".", ",").ToDecimal().ToString("N"))
                        //.AppendCell(account.AccountCreditDis.Replace(".", ",").ToDecimal().ToString("N"))
                        .AppendCell(account.AccountCreditEst)
                        .AppendCell(GetCulturaFormat(account.AccountCreditAnt))
                        //.AppendCell(Convert.ToDecimal(account.AccountCreditAnt).ToString("N", CoreContext.CurrentCultureInfo))
                        //.AppendCell(Convert.ToDateTime(account.AccountCreditFec).ToString(CoreContext.CurrentCultureInfo))
                         .AppendCell(GetCulturaFormat(account.AccountCreditFec))
                        .Append("</tr>");

                     
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

       



















        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult BloquearCredit(List<int> items)
        {
            try
            {

                var resp = NetSteps.Data.Entities.Business.CTE.Instance.SaveAccountCredit(items, 1, CoreContext.CurrentUser.UserID);

                return Json(new { result = resp, message = Translation.GetTerm("CreditoBloqueado", "Crédito Bloqueado") });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult DesBloquearCredit(List<int> items)
        {
            try
            {

                var resp = NetSteps.Data.Entities.Business.CTE.Instance.SaveAccountCredit(items, 2, CoreContext.CurrentUser.UserID);

                return Json(new { result = resp, message = Translation.GetTerm("CreditoDesbloqueado", "Crédito Desbloqueado") });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult SaldoCero(List<int> items)
        {
            try
            {

                var resp = NetSteps.Data.Entities.Business.CTE.Instance.SaveAccountCredit(items, 3, CoreContext.CurrentUser.UserID);

                return Json(new { result = resp, message = Translation.GetTerm("SaldoCreditoCero", "Saldo de crédito en cero") });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }


        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult AsignarSaldoAnterior(List<int> items)
        {
            try
            {

                var resp = NetSteps.Data.Entities.Business.CTE.Instance.SaveAccountCredit(items, 4, CoreContext.CurrentUser.UserID);

                return Json(new { result = resp, message = Translation.GetTerm("SaldoActualizado", "Saldo Actualizado a su último movimiento") });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult SetAccountSel(List<int> items)
        {
            Session["AccountsSelect"] = items;
            return Json(new { result = true });
        }
        /// <summary>
        /// Nuevo caso actualziado el saldo  : 19-07-2015
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult UpdateSaldoAsignar(string AccountID, string NewSaldo, string S, int contador = 0)//AccountCreditSearchData param)
        {
            try
            {

                string AccountIDE = "";
                if (S == "1" && Session["AccountsSelect"] != null)
                {
                   
                    var list = (List<int>)Session["AccountsSelect"];
                    contador--;
                    var c = list.Count() - contador;
                    if (c < list.Count())
                        AccountIDE = list[c].ToString();
                    if (contador == 0)
                        Session["AccountsSelect"] = null;

                }

                var param = new AccountCreditSearchData
                {
                    AccountID =  AccountID.ToInt(),
                    AccountCreditAsg = NewSaldo,
                    Site = S.ToInt(),
                    UserID = CoreContext.CurrentUser.UserID
                };
               
                var resp = NetSteps.Data.Entities.Business.CTE.Instance.UpdateSaldoAsignar(param);
                bool _result = true; string msg = "";
                if (resp == 1 || resp == 2)
                {
                    _result = true;
                    msg = Translation.GetTerm("SaldoUpd", "Saldo Actualizado.");
                }
                else
                {
                    _result = false;
                    if (resp == 10)
                        msg = Translation.GetTerm("ValidateCreditYaExiste", "Consultora con Crédito ya asignado.");

                    if (resp == 11)
                        msg = Translation.GetTerm("ValidateCuentaNoExiste", "Consultora no Existe.");

                    if (resp == 12)
                        msg = Translation.GetTerm("ValidateNewSaldoAsig", "No es posible realizar el ajuste al crédito asignado");
                   
                }
                return Json(new { result = _result, message = msg, contador = contador, AccountID = AccountIDE });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }


        public virtual ActionResult ExportAccountCredit
            (int page, int pageSize, short? status, short? type, int? state,
            string city, string postalCode, int? country, string email,
            string coApplicant, int? sponsorId, int? title, DateTime? startDate,
            DateTime? endDate, int? accountID, string siteUrl, string phone,
             string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection
          )
        {
            try
            {

                short? c = null;
                if (type.HasValue) c = type.Value;

                NetSteps.Data.Entities.Repositories.AccountCreditRepository rep = new NetSteps.Data.Entities.Repositories.AccountCreditRepository();
                StringBuilder builder = new StringBuilder();
                var accounts = rep.SearchAccountCredit(new AccountSearchParameters()
                {
                    AccountID = accountID,
                    AccountStatusID = status,
                    AccountTypeID = c,
                    StateProvinceID = state,
                    City = city,
                    PostalCode = postalCode,
                    CountryID = country,
                    SiteUrl = siteUrl,
                    PhoneNumber = phone,
                    Email = email,
                    CoApplicant = coApplicant,
                    SponsorID = sponsorId,
                    CurrentAccountID = ApplicationContext.Instance.CorporateAccountID,
                    StartDate = startDate,
                    EndDate = endDate,
                    TitleID = title,
                    PageIndex = page,
                    PageSize = 20000,
                    OrderBy = orderBy,
                    OrderByDirection = orderByDirection
                });




          


                string fileNameSave = string.Format("{0}.xlsx", Translation.GetTerm("AccountCredits", "AccountCredits"));

                var columns = new Dictionary<string, string>
				{                 
                    {"AccountNumber", Translation.GetTerm("AccountNumber","Account Number")}  ,
                    {"FirstName", Translation.GetTerm("FirstName","First Name")} ,
                    {"LastName", Translation.GetTerm("LastName","Last Name")}  ,
                    {"AccountType", Translation.GetTerm("Type","Type")} ,
                    {"AccountStatus", Translation.GetTerm("Status","Status")}  ,
                    {"AccountCreditAsg", Translation.GetTerm("AccountCreditAsg", "Credit Asig")}  ,
                    {"AccountCreditUti", Translation.GetTerm("AccountCreditUti", "Credit Util")} ,                    
                    {"AccountCreditDis", Translation.GetTerm("AccountCreditDis", "Credit Disponible")} ,
                    {"AccountCreditEst", Translation.GetTerm("AccountCreditEst","Credit Estado")}  ,
                    {"AccountCreditAnt", Translation.GetTerm("AccountCreditAnt", "Credit Anterior")} ,
                    {"AccountCreditFec", Translation.GetTerm("AccountCreditFec", "Credit Date")}  
				};

                return new NetSteps.Web.Mvc.ActionResults.ExcelResult<AccountCreditSearchData>(fileNameSave, accounts, columns, null, columns.Keys.ToArray());
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
            }

        }
        /// <summary>
        ///  Excel Export
        /// </summary>
        /// <returns>Excel File</returns>


        public virtual ActionResult GetAccountCreditModal(
            //int page, int pageSize, 
          int AccountID)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                int count = 0;

              var ListAccountCreditLogs =  AccountCreditRepository.GetAccountCreditLog(AccountID);


              foreach (var log in ListAccountCreditLogs)
                {
                    builder.Append("<tr>");
                    builder
                        .AppendCell(log.FullName)
                        .AppendCell(log.AccountType)
                        .AppendCell(Convert.ToDecimal(log.AccountCreditUti).ToString("N"))
                        .AppendCell(Convert.ToDateTime(log.AccountCreditFec).ToShortDateString())                       
                        .Append("</tr>");
                    ++count;
                }
                
                return Json(new { result = true, Items = builder.ToString() });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

    }
}
