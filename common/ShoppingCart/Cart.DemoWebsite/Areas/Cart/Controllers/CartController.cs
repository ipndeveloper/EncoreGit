using System.Linq;
using System.Web.Mvc;
using Cart.Common.Service;
using Cart.DemoWebsite.Areas.Cart.Extensions;
using Cart.DemoWebsite.Areas.Cart.Cart.Models.Interfaces;
using NetSteps.Encore.Core.IoC;

namespace Cart.DemoWebsite.Areas.Cart.Controllers
{
    public class CartController : Controller
    {
        public ActionResult Cart()
        {
			var service = Create.New<ICartService>();
			var carts = service.GetCarts();
			var model = Create.New<ICartModel>().InjectFrom(carts.FirstOrDefault());
            return View(model);
        }
    }
}
