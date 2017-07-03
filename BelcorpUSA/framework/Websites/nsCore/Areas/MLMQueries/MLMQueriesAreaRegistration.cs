using System.Web.Mvc;

namespace nsCore.Areas.MLMQueries
{
    public class MLMQuerysAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "MLMQueries";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "MLMQueries_default",
                "MLMQueries/{controller}/{action}/{id}",
                new { controller = "MLMQueries", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
