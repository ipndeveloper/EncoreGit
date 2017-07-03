using System.Web.Mvc;

namespace nsCore.Areas.CTE
{
    public class CTEAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "CTE";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "CTE_default",
                "CTE/{controller}/{action}/{id}",
                new { controller = "InterestRules", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
