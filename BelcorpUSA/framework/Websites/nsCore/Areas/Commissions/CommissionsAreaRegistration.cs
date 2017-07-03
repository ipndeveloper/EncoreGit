using System.Web.Mvc;

namespace nsCore.Areas.Commissions
{
    public class CommissionsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Commissions";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Commissions_default",
                "Commissions/{controller}/{action}/{id}",
                new { controller = "Admin", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
