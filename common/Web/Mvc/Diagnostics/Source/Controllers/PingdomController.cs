using System.Web.Mvc;
using System;

namespace NetSteps.Web.Mvc.Diagnostics.Controllers
{
	public class PingdomController : DiagnosticsController
	{
		private const string __pingdomXml = "<pingdom_http_custom_check><status>{0}</status><response_time>{1}</response_time></pingdom_http_custom_check>";
		protected override ActionResult PerformStatus(IHealthChecker healthChecker)
		{
			DateTime start = DateTime.UtcNow;
			bool healthy = healthChecker.PerformHealthCheck();
			string status = healthy ? "OK" : "NOT_OK";
			return Content(String.Format(__pingdomXml, status, (DateTime.UtcNow - start).Milliseconds), "text/xml");
		}
	}
}
