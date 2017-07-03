using System.Web.Mvc;

namespace nsCore.Controllers
{
    [HandleError]
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Orders");
        }

        //[HttpGet]
        //public ActionResult Login()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult Login(string username, string password)
        //{
        //    ApplicationContext.Instance.CurrentUser = NetSteps.Data.Entities.User.Authenticate(username, password);
        //    return Json(new { result = ApplicationContext.Instance.CurrentUser != default(IUser), returnUrl = TempData["ReturnURL"] ?? VirtualPathUtility.ToAbsolute("~/Orders") });
        //}

        //public ActionResult Logout()
        //{
        //    ApplicationContext.Instance.CurrentUser = null;
        //    return RedirectToAction("Login");
        //}
    }
}
