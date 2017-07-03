using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Enrollment.Common.Models.Context;
using NetSteps.Web.Mvc.Extensions;
using nsDistributor.Areas.Enroll.Models.Agreements;
using NetSteps.Enrollment.Common.Models.Config;
using NetSteps.Common.Base;

using NetSteps.Encore.Core.IoC;
using NetSteps.Data.Entities.Generated;
using Belcorp.Policies.Service;
using Belcorp.Policies.Service.DTO;

namespace nsDistributor.Areas.Enroll.Controllers
{
	public class AgreementsController : EnrollStepBaseController
	{
		#region Actions
        public virtual ActionResult Index(int? pAccountID)
		{
			var models = new List<AgreementModel>();
            /*CS.Inicio.16Junio.2016*/
            Session["AccountInfoPolicy"] = pAccountID;
            if (pAccountID != null)
            {
                var policiesService = Create.New<IPoliciesService>();
                AccountPolicyDetailsDTO modelPolicy = policiesService.AccountPolicyDetail((int)pAccountID, (int)ConstantsGenerated.AccountType.Distributor, Account.DefaultLanguageID);/*R2908 - HUNDRED(JAUF)*/
                if (modelPolicy.IsApplicableAccount && !modelPolicy.IsAcceptedPolicy)
                {
                    SetEnrollingAccount((int)pAccountID);
                }
                else
                {
                    _enrollmentContext.EnrollmentComplete = true;
                    _enrollmentContext.Clear();
                    return Redirect("~/Home");
                }
            }
            /*CS.Fin.16Junio.2016*/
			var account = GetEnrollingAccount();
			try
			{
				var policies = GetPolicies();
				var accountPolicies = GetAccountPolicies(account);
				Index_LoadValues(models, policies, accountPolicies);
				Index_LoadResources(models, policies);
			}
			catch (Exception ex)
			{
				AddErrorToTempData(ex.Log(accountID: account != null ? (int?)account.AccountID : null).PublicMessage);
				return RedirectToStep(_enrollmentContext.EnrollmentConfig.Steps.PreviousItem);
			}

			return View(models);
		}

		[HttpPost, ValidateAntiForgeryToken]
		public virtual ActionResult Index(List<AgreementModel> models, IEnrollmentContext enrollmentContext)
		{
            var policiesService = Create.New<IPoliciesService>();
            AccountPolicyDetailsDTO modelPolicy = null;
            if (Session["AccountInfoPolicy"] == null)
            {
                if (models == null)
                {
                    models = new List<AgreementModel>();
                }

                if (!ModelState.IsValid)
                {
                    Index_LoadResources(models, GetPolicies());
                    return View(models);
                }

                var account = GetEnrollingAccount();
                try
                {
                    /*CS.10JUN2016.Inicio*/
                    modelPolicy = policiesService.AccountPolicyDetail(account.AccountID, (int)ConstantsGenerated.AccountType.Distributor, account.DefaultLanguageID);/*R2908 - HUNDRED(JAUF)*/
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
                    Index_UpdateAccount(models, account);
                    //account.Save();
                    /*CS.10JUN2016.Fin*/
                }
                catch (Exception ex)
                {
                    AddErrorToViewData(ex.Log(accountID: account != null ? (int?)account.AccountID : null).PublicMessage);
                    Index_LoadResources(models, GetPolicies());
                    return View(models);
                }
                return StepCompleted();
            }
            else
            {
                /*ITG 2833(JCT) - Inicio*/
                var account = GetEnrollingAccount();
                modelPolicy = policiesService.AccountPolicyDetail(account.AccountID, (int)ConstantsGenerated.AccountType.Distributor, account.DefaultLanguageID);/*R2908 - HUNDRED(JAUF)*/
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
                        _enrollmentContext.EnrollmentComplete = true;
                        _enrollmentContext.Clear();
                        return Redirect("~/Home");
                        /*ITG 2833(JCT) - Fin*/
                    }
                    catch (Exception ex)
                    {
                        AddErrorToViewData(ex.Log(accountID: account != null ? (int?)account.AccountID : null).PublicMessage);
                        Index_LoadResources(models, GetPolicies());
                        return View(models);
                    }
                }
                else
                {
                    _enrollmentContext.EnrollmentComplete = true;
                    _enrollmentContext.Clear();
                    return Redirect("~/Home");
                }
            }
            /*CS:18/MAR/2016.Inicio*/
            //return RedirectToStep(_enrollmentContext.EnrollmentConfig.Steps.NextItem);/*CS.10JUN2016.Comentado*/
            /*CS:18/MAR/2016.Fin*/

            return Redirect("~/Home");//Eliminar
		}

        #region Private
        /*CS:10JUN2016.Inicio*/
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
        /*CS:10JUN2016.Fin*/
        #endregion

        /*CS:18/MAR/2016.Inicio*/
        protected virtual OrderedList<IEnrollmentStepSectionConfig> GetSections()
        {
            return _enrollmentContext.EnrollmentConfig.Steps.CurrentItem.Sections;
        }

        protected virtual ActionResult SectionCompleted()
        {
            var sections = GetSections();

            sections.CurrentItem.Completed = true;

            if (!string.IsNullOrWhiteSpace(_enrollmentContext.ReturnUrl))
            {
                string returnUrl = _enrollmentContext.ReturnUrl;
                _enrollmentContext.ReturnUrl = null;
                return Redirect(returnUrl);
            }

            if (sections.HasNextItem)
            {
                return RedirectToAction(sections.NextItem.Action);
            }
            else
            {
                return StepCompleted();
            }
        }
        /*CS:18/MAR/2016.Fin*/
		#endregion

		#region Private
		protected virtual List<Policy> GetPolicies()
		{
			if (_enrollmentContext.MarketID == 0)
			{
				return new List<Policy>();
			}

			// Order by date ascending is important, otherwise we will need to add SortIndex to the table
			var policies = Policy.GetPolicies(_enrollmentContext.AccountTypeID, _enrollmentContext.MarketID)
				.Where(x => x.Active)
				.OrderBy(x => x.DateReleasedUTC)
				.ToList();
			
			//Trim down by language if possible.  If none are found for this language, return all active policies for the market as usual
			if (policies.Any(x => x.LanguageID == _enrollmentContext.LanguageID))
			{
				return policies.Where(x => x.LanguageID == _enrollmentContext.LanguageID).ToList();
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

		protected virtual void Index_UpdateAccount(List<AgreementModel> models, Account account)
		{
			var accountPolicies = GetAccountPolicies(account);

			foreach (var model in models.Where(x => x.Accepted && accountPolicies.All(ap => ap.PolicyID != x.PolicyID)))
			{
				account.AccountPolicies.Add(new AccountPolicy
				{
					AccountID = account.AccountID,
					PolicyID = model.PolicyID.Value,
					DateAcceptedUTC = DateTime.UtcNow
				});
			}
		}
		#endregion
	}
}