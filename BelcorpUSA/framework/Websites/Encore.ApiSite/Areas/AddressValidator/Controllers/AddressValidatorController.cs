using System.Web.Mvc;
using AddressValidator.Common;
using Encore.ApiSite.Areas.AddressValidator.Models;
using NetSteps.Common.Data;
using NetSteps.Encore.Core.IoC;
using NetSteps.Web.Mvc.Restful;

namespace Encore.ApiSite.Areas.AddressValidator.Controllers
{
    public class AddressValidatorController : Controller
    {
		public IAddressValidator AddressValidator { get { return Create.New<IAddressValidator>(); } }

        [HttpPost]
        public ActionResult ValidateAddress()
        {
            return this.ProtectedResourceAction(c =>
            {
                var model = this.ValidatePostBodyAs<AddressModel>();
                var validationAddress = model.ToValidationAddress();
                var validationResult = AddressValidator.ValidateAddress(validationAddress);
                return this.Result_200_OK(validationResult);
            });
        }
    }
}