using System.Web.Mvc;

namespace nsCore.Areas.GeneralLedger
{
    public class GeneralLedgerAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "GeneralLedger";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "GeneralLedger_default",
                "GeneralLedger/{controller}/{action}/{id}",
                new { controller="Holiday", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
