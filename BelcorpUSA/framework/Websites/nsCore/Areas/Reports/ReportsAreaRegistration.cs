using System.Web.Mvc;

namespace nsCore.Areas.Reports
{
    public class ReportAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Reports";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Reports_default",
                "Reports/{controller}/{action}/{id}",
                new { controller = "Reports", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
