using System.Web.Mvc;

namespace nsCore.Areas.Logistics
{
    public class LogisticsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Logistics";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Logistics_default",
                "Logistics/{controller}/{action}/{id}",
                new { controller = "Logistics", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
