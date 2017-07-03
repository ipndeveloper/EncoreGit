using System.Web.Mvc;

namespace Encore.ApiSite.Areas.AddressValidator
{
    public class AddressValidatorAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "AddressValidator";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
               "address_validator",
               "address/validate",
               new { controller="AddressValidator",  action = "ValidateAddress" }
           );
        }
    }
}
