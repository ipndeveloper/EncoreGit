using System;
using System.Web.Mvc;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Extensions;
using nsDistributor.Areas.Enroll.Controllers;

namespace nsDistributor.Areas.Accounts.Controllers
{
    public abstract class BaseAccountsController : EnrollStepBaseController //BaseController
    {
        protected virtual ActionResult CheckForAccount(string id, object model = null, bool forceLoadAccount = false)
        {
            bool wasAccountLoaded = false;

            if (!string.IsNullOrEmpty(id))
            {
                if (IsCurrentAccountNull() || IsCurrentAccountDifferent(id))
                {
                    ResetCoreContextAccount(id);
                    wasAccountLoaded = true;
                }

                ForceLoadAccount(id, wasAccountLoaded, forceLoadAccount);
            }

            if (IsCurrentAccountNull())
                return Redirect("~/Accounts");

            return View(model);
        }

        private static bool IsCurrentAccountNull()
        {
            return CoreContext.CurrentAccount.IsNull();
        }

        private static bool IsCurrentAccountDifferent(string id)
        {
            return !String.Equals(CoreContext.CurrentAccount.AccountNumber, id);
        }

        private static void ForceLoadAccount(string id, bool isAlreadyLoaded, bool forceLoad)
        {
            if (!isAlreadyLoaded && forceLoad)
                ResetCoreContextAccount(id);

        }

        private static void ResetCoreContextAccount(string id)
        {
            CoreContext.CurrentAccount = Account.LoadForSessionByAccountNumber(id);
        }
    }
}
