using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using NetSteps.Common.Configuration;
using DistributorBackOffice.Areas.Account.Controllers;
using NetSteps.Common.Globalization;
using NetSteps.Web.Mvc.Helpers;
using DistributorBackOffice.Models.Agreements;
using NetSteps.Data.Entities;
using NetSteps.Common.Extensions;
using NetSteps.Web.Mvc.Extensions;
using NetSteps.Data.Entities.Generated;
using Belcorp.Policies.Service;
using Belcorp.Policies.Service.DTO;
using NetSteps.Encore.Core.IoC;
using NetSteps.Data.Entities.Extensions;

namespace DistributorBackOffice.Controllers
{
    public class AgreementsController : BaseAccountsController
    {
        public virtual ActionResult Index()
        {
            var account = CurrentAccount;
            var models = new List<AgreementModel>();
            var policiesService = Create.New<IPoliciesService>();
            AccountPolicyDetailsDTO modelPolicy = policiesService.AccountPolicyDetail(account.AccountID, (int)ConstantsGenerated.AccountType.Distributor, account.DefaultLanguageID);/*R2908 - HUNDRED(JAUF)*/
            if (modelPolicy.IsApplicableAccount && !modelPolicy.IsAcceptedPolicy)
            {
                try
                {
                    var policies = GetPolicies(account);
                    var accountPolicies = GetAccountPolicies(account);
                    Index_LoadValues(models, policies, accountPolicies);
                    Index_LoadResources(models, policies);
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index", "Agreements");
                }

                return View(models);
            }
            else
            {
                return Redirect("~/Home");
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult Index(List<AgreementModel> models)
        {
            if (models == null)
            {
                models = new List<AgreementModel>();
            }
            var account = CurrentAccount;
            var policiesService = Create.New<IPoliciesService>();
            AccountPolicyDetailsDTO modelPolicy = policiesService.AccountPolicyDetail(account.AccountID, (int)ConstantsGenerated.AccountType.Distributor, account.DefaultLanguageID);/*R2908 - HUNDRED(JAUF)*/
            if (modelPolicy.IsApplicableAccount && !modelPolicy.IsAcceptedPolicy)
            {
                try
                {
                    modelPolicy.DateAccepted = DateTime.UtcNow;
                    modelPolicy.IPAddress = GetUserIPAddress();
                    modelPolicy.LanguageID = account.DefaultLanguageID;

                    /*R2908 - HUNDRED(JAUF)*/
                    var pAccountPolicyDetail = policiesService.AccountPolicyDetail(account.AccountID, (int)ConstantsGenerated.AccountType.Distributor, account.DefaultLanguageID);
                    string[] pArrayPDFname = pAccountPolicyDetail.FilePath.Split('\\');
                    string cadPDFname = pArrayPDFname[pArrayPDFname.Length - 1];
                    string[] pArrayExtension = cadPDFname.Split('.');
                    string cadExtension = pArrayExtension[pArrayExtension.Length - 1];
                    ExternalMail.SendMailConfirmPolicy(policiesService.GeneratePDFBytes(account.FirstName + ", " + account.LastName + ". " +
                    Translation.GetTerm(account.DefaultLanguageID, "IPAddress", "IP Address") + ": " + modelPolicy.IPAddress, DateTime.Now.ToShortDateString(), pAccountPolicyDetail.FilePath.ToString().Replace("<!--filepath-->", System.Configuration.ConfigurationManager.AppSettings["FileUploadAbsolutePath"].ToString())), cadPDFname, cadExtension, account, pAccountPolicyDetail.LanguageID);


                    policiesService.AddAccountPolicyDetail(modelPolicy);
                    policiesService.Commit();
                    return Redirect("~/Home");
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessagePolicy = ex.Log(accountID: account != null ? (int?)account.AccountID : null).PublicMessage;
                    return View(models);
                }
            }
            else
            {
                return Redirect("~/Home");
            }

        }

        protected static string GetUserIPAddress()
        {
            var context = System.Web.HttpContext.Current;
            string ip = String.Empty;

            if (context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                ip = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            else if (!String.IsNullOrWhiteSpace(context.Request.UserHostAddress))
                ip = context.Request.UserHostAddress;

            if (ip == "::1")
                ip = "127.0.0.1";

            return ip;
        }

        private string MailFooterInfo(AccountPolicyDetailsDTO model, Account account)
        {
            return account.FirstName + ", " + account.LastName + ". " +
                Translation.GetTerm(account.DefaultLanguageID, "IPAddress", "IP Address") + ": " + model.IPAddress + ". " +
                Translation.GetTerm(account.DefaultLanguageID, "Date", "Date") + ": " + model.DateFormatedAccepted;
        }

        protected virtual List<Policy> GetPolicies(Account pAccount)
        {
            if (pAccount.MarketID == 0)
            {
                return new List<Policy>();
            }

            // Order by date ascending is important, otherwise we will need to add SortIndex to the table
            var policies = Policy.GetPolicies(pAccount.AccountTypeID, pAccount.MarketID)
                .Where(x => x.Active)
                .OrderBy(x => x.DateReleasedUTC)
                .ToList();

            //Trim down by language if possible.  If none are found for this language, return all active policies for the market as usual
            if (policies.Any(x => x.LanguageID == pAccount.DefaultLanguageID))
            {
                return policies.Where(x => x.LanguageID == pAccount.DefaultLanguageID).ToList();
            }

            return policies;
        }

        protected virtual List<AccountPolicy> GetAccountPolicies(Account account)
        {
            if (account == null || account.AccountID == 0)
            {
                return new List<AccountPolicy>();
            }

            return AccountPolicy.LoadByAccountID(account.AccountID);
        }

        protected virtual void Index_LoadValues(List<AgreementModel> models, List<Policy> policies, List<AccountPolicy> accountPolicies)
        {
            foreach (var policy in policies)
            {
                models.Add(new AgreementModel().LoadValues(
                    policy.PolicyID,
                    accountPolicies.Any(x => x.PolicyID == policy.PolicyID)
                ));
            }
        }

        protected virtual void Index_LoadResources(List<AgreementModel> models, List<Policy> policies)
        {
            int i = 0;
            foreach (var model in models)
            {
                var policy = policies.FirstOrDefault(x => x.PolicyID == model.PolicyID);
                if (policy != null)
                {
                    model.LoadResources(
                        Translation.GetTerm(policy.TermName, policy.Name),
                        "Agreement" + i,
                        policy.FilePath.ReplaceFileUploadPathToken(),
                        policy.HtmlSection == null ? null
                            : policy.HtmlSection.HtmlSectionContents.FirstOrDefault() == null ? null
                            : policy.HtmlSection.HtmlSectionContents.FirstOrDefault().HtmlContent.GetBody().ToMvcHtmlString()
                    );
                }
                i++;
            }
        }

    }
}
