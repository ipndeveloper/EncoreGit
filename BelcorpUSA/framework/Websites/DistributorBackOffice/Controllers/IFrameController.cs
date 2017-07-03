using System.Web.Mvc;

namespace DistributorBackOffice.Controllers
{
    public class IFrameController : BaseController
    {
        public virtual ActionResult GoToUrl(string name)
        {
            ViewData["IFrameUrl"] = "http://www.google.com";

            return View();
        }
    }
}
