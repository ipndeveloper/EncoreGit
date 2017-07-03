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
using System.Collections.Generic;
using System.Data;
using NetSteps.Data.Entities.Business.Logic;
namespace DistributorBackOffice.Areas.Account.Controllers
{
    public class AccountsController : Controller
    {
        //
        // GET: /Account/Accounts/

        public ActionResult Index()
        {
            return View();
        }
        public virtual ActionResult Search(string query)
        {
            try
            {
                //Dictionary<int, string> accounts = new Dictionary<int, string>();
                //accounts.Add(1, "Francisco");
                //accounts.Add(2, "Jose");
                //accounts.Add(3, "Dayana");
                //accounts.Add(4, "Silvia");

                //return Json(accounts);
                var result = AccountCache.GetAccountSearchByTextResults(query);
                return Json(result.ToAJAXSearchResults(),JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

         
    }
}
