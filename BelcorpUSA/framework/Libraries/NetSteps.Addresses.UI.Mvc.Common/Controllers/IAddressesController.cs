using System;
using System.Diagnostics.Contracts;
using System.Web.Mvc;
using NetSteps.Addresses.UI.Common.Models;

namespace NetSteps.Addresses.UI.Mvc.Common.Controllers
{
	[ContractClass(typeof(Contracts.AddressesControllerContract))]
	public interface IAddressesController : IController
	{
        //ActionResult AddressTemplate(int marketId);
        //ActionResult AddressEditor(string countryCode);
        //JsonResult ScrubAddress(IAddressUIModel fromAddress);
	}

	namespace Contracts
	{
		[ContractClassFor(typeof(IAddressesController))]
		abstract class AddressesControllerContract : IAddressesController
		{
			public ActionResult AddressTemplate(int marketId)
			{
				Contract.Requires(marketId > 0);

				throw new NotImplementedException();
			}

			public ActionResult AddressEditor(string countryCode)
			{
				Contract.Requires(!String.IsNullOrWhiteSpace(countryCode));

				throw new NotImplementedException();
			}

			public void Execute(System.Web.Routing.RequestContext requestContext)
			{
				throw new NotImplementedException();
			}
			
			public JsonResult ScrubAddress(IAddressUIModel fromAddress)
			{
				Contract.Requires(fromAddress != null);

				throw new NotImplementedException();
			}
		}

	}
}
