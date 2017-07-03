using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using Belcorp.Policies.Service;
using NetSteps.Data.Entities.Generated;
using NetSteps.Encore.Core.IoC;
using Belcorp.Policies.Service.DTO;
using NetSteps.Data.Entities.Extensions;

namespace nsDistributor.Controllers
{
    public class WelcomeController : BaseController
    {

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!IsLoggedIn)
            {
                if (Request.IsAjaxRequest())
                {
                    filterContext.Result = Json(new { result = false, message = Translation.GetTerm("SessionTimedOut", "Your session has timed out.") });
                }
                else
                {
                    filterContext.Result = Redirect("~/Home");
                }
                return;
            }
            base.OnActionExecuting(filterContext);
        }

        protected virtual bool IsCurrentAccountNull()
        {
            return CoreContext.CurrentAccount.IsNull();
        }

        protected virtual Account GetCurrentAccount()
        {
            return CoreContext.CurrentAccount;
        }

        public virtual ActionResult Index()
        {
            var account = GetCurrentAccount();
            if (IsCurrentAccountNull())
            {
                return Redirect("~/Home");
            }
            var policiesService = Create.New<IPoliciesService>();
            AccountPolicyDetailsDTO modelPolicy = policiesService.AccountPolicyDetail(account.AccountID, (int)ConstantsGenerated.AccountType.Distributor, Account.DefaultLanguageID);/*R2908 - HUNDRED(JAUF)*/
            if (modelPolicy.IsApplicableAccount && !modelPolicy.IsAcceptedPolicy)
            {
                return View();
            }
            else
            {
                return Redirect("~/Home");
            }
        }

        public virtual ActionResult Continue(int? pAccountID)
        {
            if (pAccountID == null)
            {
                return Json(new { result = false, message = Translation.GetTerm("InvalidLoginPWS", "Invalid login.") });
            }
            return Json(new { result = true, returnUrl = Url.Action("Index", "Landing", new { area = "Enroll", pAccountID = pAccountID }) });
        }

    }
}
