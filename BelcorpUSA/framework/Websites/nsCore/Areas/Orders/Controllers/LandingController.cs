using System.Web.Mvc;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Helpers;
using nsCore.Controllers;

namespace nsCore.Areas.Orders.Controllers
{
    public class LandingController : BaseController
    {
        [FunctionFilter("Orders", "~/Accounts")]
        public virtual ActionResult Index()
        {
            CoreContext.CurrentOrder = null;
            CoreContext.CurrentAccount = null;
            return View("Index");
        }
    }
}
