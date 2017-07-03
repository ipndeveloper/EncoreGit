using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Common.Context;
using System.Diagnostics.Contracts;

namespace NetSteps.Data.Common.Registries
{
	[ContractClass(typeof(OrderStepHandlerContract))]
	public interface IOrderStepHandler
	{
		bool VerifyStepCompletion(IOrderStep step);
		bool ValidateStepResponse(IOrderStep step);
	}

	[ContractClassFor(typeof(IOrderStepHandler))]
	public abstract class OrderStepHandlerContract : IOrderStepHandler
	{

		public bool VerifyStepCompletion(IOrderStep step)
		{
			Contract.Requires<ArgumentNullException>(step != null);
			throw new NotImplementedException();
		}

		public bool ValidateStepResponse(IOrderStep step)
		{
			Contract.Requires<ArgumentNullException>(step != null);
			throw new NotImplementedException();
		}
	}
}
