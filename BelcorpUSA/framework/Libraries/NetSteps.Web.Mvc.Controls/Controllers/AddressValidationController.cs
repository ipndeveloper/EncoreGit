using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AddressValidation.Common;
using AddressValidator.Common;
using NetSteps.Common.Data;
using NetSteps.Encore.Core.IoC;
using NetSteps.Web.Mvc.Controls.Models.AddressValidation;
using NetSteps.Web.Mvc.Restful;

namespace NetSteps.Web.Mvc.Controls.Controllers
{
	public class AddressValidationController : Controller
	{
		public ActionResult Validate(AddressModel model)
		{
			//return this.ProtectedResourceAction(c =>
			//{
			var validationAddress = model.ToValidationAddress();
			var validationResult = Create.New<IAddressValidationResult>();
                if (model.Country != null && model.Country.Equals("US", StringComparison.InvariantCultureIgnoreCase))
			{
				var validator = Create.New<IAddressValidator>();
				validationResult = validator.ValidateAddress(validationAddress);
                    validationResult.ValidAddresses =
                        SanitizeAddressToConformToEncore(validationResult.ValidAddresses)
                        .ToArray();

				if (validationResult.Message.Contains("AmbiguousAddressIndicator"))
				{
					validationResult.Message = "AmbigousMatch";
				}
                    else if (validationResult.Message.Contains("NoCandidatesIndicator"))
                    {
                        validationResult.Message = "NoMatch";
			}
                    else if (validationResult.Message.Contains("ValidAddressIndicator"))
                    {
                        validationResult.Message = "ValidMatch";
                    }
			else
			{
                        validationResult.Message = validationResult.Status.ToString();
                    }
                }
                else
                {
				validationResult.Status = AddressInfoResultState.Success;
				validationResult.ValidAddresses = new List<IValidationAddress>()
                                                          {
                                                              validationAddress
                                                          };
				validationResult.Message = "Success";
			}
			return this.Result_200_OK(validationResult);
			//});
		}

		IEnumerable<IValidationAddress> SanitizeAddressToConformToEncore(IEnumerable<IValidationAddress> validAddresses)
		{
			foreach (var validationAddress in validAddresses)
			{
                if (validationAddress.PostalCode != null)
                {
				validationAddress.PostalCode = validationAddress.PostalCode.Replace("-", string.Empty);
                }

				yield return validationAddress;
			}
		}
	}
}
