using System.Web.Mvc;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Web.Mvc.Diagnostics.Controllers
{
	public class DiagnosticsController : Controller
	{
		public ActionResult Status()
		{
			IHealthChecker healthChecker = Create.New<IHealthChecker>();
			return PerformStatus(healthChecker);
		}

		protected virtual ActionResult PerformStatus(IHealthChecker healthChecker)
		{
			var result = new { Status = healthChecker.PerformHealthCheck() ? "OK" : "NOT_OK" };

			return Json(result, JsonRequestBehavior.AllowGet);
		}
	}
}
