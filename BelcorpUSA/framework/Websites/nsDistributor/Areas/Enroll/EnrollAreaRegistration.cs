using System.Web.Mvc;
using NetSteps.Common.Configuration;
using NetSteps.Data.Entities;

namespace nsDistributor.Areas.Enroll
{
    public class EnrollAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Enroll";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            bool isSubdomain = ConfigurationManager.GetAppSetting<bool>(ConfigurationManager.VariableKey.UrlFormatIsSubdomain, true);
            string prefix = isSubdomain ? string.Empty : "{distributor}/";

            context.MapRoute("Enroll_Landing", prefix + "Enroll/Landing", new { controller = "Landing", action = "Index" });
            context.MapRoute("Enroll_Distributor", prefix + "Enroll/Distributor", new { controller = "Landing", action = "Index", accountTypeID = (short)Constants.AccountType.Distributor });
            context.MapRoute("Enroll_PC", prefix + "Enroll/PC", new { controller = "Landing", action = "Index", accountTypeID = (short)Constants.AccountType.PreferredCustomer });
            context.MapRoute("Enroll_default", prefix + "Enroll/{controller}/{action}/{id}", new { controller = "Landing", action = "Index", id = UrlParameter.Optional });
        }
    }
}
