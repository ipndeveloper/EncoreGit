using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Cart.Common.Service
{
	[ContractClass(typeof(CartServiceContracts))]
	public interface ICartService
	{
		IEnumerable<ICart> GetCarts();
	}

	[ContractClassFor(typeof(ICartService))]
	abstract class CartServiceContracts : ICartService
	{
		public IEnumerable<ICart> GetCarts()
		{
			Contract.Ensures(Contract.Result<IEnumerable<ICart>>() != null);
			Contract.Ensures(Contract.Result<IEnumerable<ICart>>().All(c => c.Items != null));
			Contract.Ensures(Contract.Result<IEnumerable<ICart>>().All(c => c.Adjustments != null));

			throw new NotImplementedException();
		}
	}

}
