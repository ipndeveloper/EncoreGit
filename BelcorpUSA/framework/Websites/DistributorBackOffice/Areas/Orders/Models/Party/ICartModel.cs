using System.Collections.Generic;
using NetSteps.Encore.Core.IoC;
using NetSteps.Web.Mvc.Models;

namespace DistributorBackOffice.Areas.Orders.Models.Party
{
	public interface ICartModel : IDynamicViewModel
	{
		/// <summary>
		/// A sequence of partial views and models used to compose the Cart view.
		/// </summary>
		IEnumerable<ICartElement> CartElements { get; set; }

		/// <summary>
		/// The Party entity for legacy code.
		/// </summary>
		NetSteps.Data.Entities.Party Party { get; set; }
	}

	[ContainerRegister(typeof(ICartModel), RegistrationBehaviors.Default)]
	public class CartModel : DynamicViewModel, ICartModel
	{
		public IEnumerable<ICartElement> CartElements { get; set; }
		public NetSteps.Data.Entities.Party Party { get; set; }
	}
}
