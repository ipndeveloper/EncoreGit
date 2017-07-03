using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace NetSteps.Data.Common.Registries
{
	[ContractClass(typeof(OrderStepHandlerRegistryContract))]
	public interface IOrderStepHandlerRegistry
	{
		IOrderStepHandler GetOrderStepHandler(string rewardKindName);
		TOrderStepHandler GetOrderStepHandler<TOrderStepHandler>(string rewardKindName) where TOrderStepHandler : IOrderStepHandler;
		void RegisterHandler<TOrderStepHandler>(string rewardKindName) where TOrderStepHandler : IOrderStepHandler;
	}

	[ContractClassFor(typeof(IOrderStepHandlerRegistry))]
	public abstract class OrderStepHandlerRegistryContract : IOrderStepHandlerRegistry
	{

		public IOrderStepHandler GetOrderStepHandler(string rewardKindName)
		{
			Contract.Requires<ArgumentException>(!String.IsNullOrEmpty(rewardKindName));
			throw new NotImplementedException();
		}

		public TOrderStepHandler GetOrderStepHandler<TOrderStepHandler>(string rewardKindName) where TOrderStepHandler : IOrderStepHandler
		{
			Contract.Requires<ArgumentException>(!String.IsNullOrEmpty(rewardKindName));
			throw new NotImplementedException();
		}

		public void RegisterHandler<TOrderStepHandler>(string rewardKindName) where TOrderStepHandler : IOrderStepHandler
		{
			Contract.Requires<ArgumentException>(!String.IsNullOrEmpty(rewardKindName));
			throw new NotImplementedException();
		}
	}
}
