using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Cart.DemoWebsite.Areas.Cart.Cart.Models.Interfaces;
using Cart.DemoWebsite.Areas.Cart.Cart.Models.ModelBinders;

namespace Cart.DemoWebsite
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801
	public class MvcApplication : System.Web.HttpApplication
	{
		

		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();

			ModelBinders.Binders.Add(typeof(ICartModel), new CartModelBinder());

			WebApiConfig.Register(GlobalConfiguration.Configuration);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
		}
	}
}