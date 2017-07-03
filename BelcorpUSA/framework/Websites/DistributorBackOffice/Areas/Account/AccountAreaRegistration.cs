using System.Web.Mvc;

namespace DistributorBackOffice.Areas.Account
{
    public class AccountAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Account";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Account_default",
                "Account/{controller}/{action}/{id}",
                new { controller = "Landing", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
