using System.Web.Mvc;

namespace DistributorBackOffice.Areas.Earnings
{
	public class EarningsAreaRegistration : AreaRegistration
	{
		public override string AreaName
		{
			get
			{
				return "Earnings";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context)
		{
            context.MapRoute("GetDetail", "Earnings/GetBonusDetail", new { controller = "Earnings", action = "GetBonusDetail" });
            context.MapRoute("Download", "Earnings/Download", new { controller = "Earnings", action = "Download" });
			context.MapRoute(
				"Earnings_default",
				"Earnings/{periodId}",
				new { controller = "Earnings", action = "Index", periodId = UrlParameter.Optional }
			);
		}
	}
}
