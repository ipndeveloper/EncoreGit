using System.Web.Mvc;
using NetSteps.Web.Mvc.Attributes;

namespace nsCore.Areas.Sites.Controllers
{
    public class LandingController : BaseSitesController
    {
        /// <summary>
        /// Lists out the base sites and the actions on each
        /// </summary>
        /// <returns></returns>
        [FunctionFilter("Sites", "~/Accounts")]
        public virtual ActionResult Index()
        {
            return View();
        }
    }
}
