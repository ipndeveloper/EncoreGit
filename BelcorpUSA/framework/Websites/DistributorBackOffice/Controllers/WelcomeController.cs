using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DistributorBackOffice.Areas.Account.Controllers;
using NetSteps.Encore.Core.IoC;
using Belcorp.Policies.Service;
using NetSteps.Data.Entities.Generated;
using Belcorp.Policies.Service.DTO;
using NetSteps.Common.Globalization;

namespace DistributorBackOffice.Controllers
{
    public class WelcomeController : BaseAccountsController
    {
        public virtual ActionResult Index()
        {
            var account = CurrentAccount;
            var policiesService = Create.New<IPoliciesService>();
            AccountPolicyDetailsDTO modelPolicy = policiesService.AccountPolicyDetail(account.AccountID, (int)ConstantsGenerated.AccountType.Distributor, account.DefaultLanguageID);/*R2908 - HUNDRED(JAUF)*/
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
                return Json(new { result = false, message = Translation.GetTerm("YourSessionHasTimedOutPleaseRefreshthePage", "Your session has timed out.  Please refresh the page.") });
            }
            return Json(new { result = true, returnUrl = Url.Action("Index", "Agreements") });
        }

    }
}
