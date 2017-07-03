using System.Web.Mvc;
using NetSteps.Web.Mvc.Attributes;
using nsCore.Controllers;

namespace nsCore.Areas.Support.Controllers
{
    public class AdminController : BaseController
    {
        [FunctionFilter("Support", "~/Accounts")]
        public virtual ActionResult Index()
        {
            return View();
        }
    }
}
