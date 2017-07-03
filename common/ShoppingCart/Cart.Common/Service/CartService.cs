using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.IoC;
using System.Diagnostics.Contracts;

namespace Cart.Common.Service
{
	[ContainerRegister(typeof(ICartService), RegistrationBehaviors.IfNoneOther, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
	public class CartService : ICartService
	{
		public IEnumerable<ICart> GetCarts()
		{
			Contract.Ensures(Contract.Result<IEnumerable<ICart>>() != null);
			Contract.Ensures(Contract.Result<IEnumerable<ICart>>().All(r => r.Items != null));
			Contract.Ensures(Contract.Result<IEnumerable<ICart>>().All(r => r.Adjustments != null));

			var results = Create.New<IDummyCartRepo>().GetCarts();

			Contract.Assert(results != null);
			Contract.Assert(results.All(r => r.Items != null));
			Contract.Assert(results.All(r => r.Adjustments != null));
			return results;
		}
	}
}
