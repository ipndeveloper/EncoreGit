using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cart.DemoWebsite.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
			return RedirectToRoute("Cart", new { controller = "Cart", action = "Cart" });
        }
    }
}
