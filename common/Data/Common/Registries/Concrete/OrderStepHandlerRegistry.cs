using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Data.Common.Registries.Concrete
{
	public class OrderStepHandlerRegistry : IOrderStepHandlerRegistry
	{
		/// <summary>
		/// The dictionary of order step handlers registered with a string representation of their types as a key.
		/// </summary>
		private readonly ConcurrentDictionary<string, RegistrationRecord> _orderStepHandlerConstructors = new ConcurrentDictionary<string, RegistrationRecord>();

		private readonly ConcurrentDictionary<string, string> _orderStepHandlerTypeToOrderStepHandlerKindKey = new ConcurrentDictionary<string, string>();

		/// <summary>
		/// A provider registration record.
		/// </summary>
		abstract class RegistrationRecord
		{
			/// <summary>
			/// An untyped provider.
			/// </summary>
			/// <returns></returns>
			public abstract IOrderStepHandler UntypedConstruct();
		}

		/// <summary>
		/// Provides an abstraction for getting an untyped Order Step Handler.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		class RegistrationRecord<T> : RegistrationRecord
			where T : IOrderStepHandler
		{
			public override IOrderStepHandler UntypedConstruct()
			{
				return Create.New<T>();
			}
		}

		public IOrderStepHandler GetOrderStepHandler(string HandlerKindName)
		{
			return _orderStepHandlerConstructors[HandlerKindName].UntypedConstruct();
		}

		public TOrderStepHandler GetOrderStepHandler<TOrderStepHandler>(string HandlerKindName) where TOrderStepHandler : IOrderStepHandler
		{
			return (TOrderStepHandler)_orderStepHandlerConstructors[HandlerKindName].UntypedConstruct();
		}

		public void RegisterHandler<TOrderStepHandler>(string HandlerKindName) where TOrderStepHandler : IOrderStepHandler
		{
			var orderStepHandler = Create.New<TOrderStepHandler>();

			if (!_orderStepHandlerConstructors.TryAdd(HandlerKindName, new RegistrationRecord<TOrderStepHandler>()))
			{
				throw new InvalidOperationException(String.Concat("Order step handler already registered: ", HandlerKindName));
			}
			if (!_orderStepHandlerTypeToOrderStepHandlerKindKey.TryAdd(typeof(TOrderStepHandler).ToString(), HandlerKindName))
			{
				throw new InvalidOperationException(String.Concat("Order step handler type already registered:", typeof(TOrderStepHandler).ToString()));
			}
		}
	}
}
