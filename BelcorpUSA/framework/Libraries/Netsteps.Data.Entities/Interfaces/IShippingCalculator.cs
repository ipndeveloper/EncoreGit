using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using NetSteps.Data.Entities.Business;

namespace NetSteps.Data.Entities.Interfaces
{
	[ContractClass(typeof(Contracts.ShippingCalculatorContracts))]
	public interface IShippingCalculator
	{
		List<ShippingMethodWithRate> GetShippingMethodsWithRates(OrderCustomer orderCustomer);
		List<ShippingMethodWithRate> GetShippingMethodsWithRates(OrderCustomer orderCustomer, OrderShipment orderShipment);
		List<ShippingMethodWithRate> GetShippingMethodsWithRates(Order order);
		List<ShippingMethodWithRate> GetShippingMethodsWithRates(Order order, OrderShipment orderShipment);

		ShippingMethodWithRate GetLeastExpensiveShippingMethod(OrderCustomer orderCustomer, OrderShipment orderShipment);
		ShippingMethodWithRate GetLeastExpensiveShippingMethod(Order order);

		void CalculateShipping(OrderCustomer orderCustomer);
		void CalculatePartyShipping(Order order);
		void SetPartyShipmentChargeForCustomer(OrderCustomer customer);
		decimal CalculateTotalForShipping(OrderCustomer orderCustomer, OrderShipment orderShipment);
		bool IncludeMaxValuesWhenExceedingAllRangesForShippingRates { get; }
	}

	namespace Contracts
	{
		[ContractClassFor(typeof(IShippingCalculator))]
		abstract class ShippingCalculatorContracts : IShippingCalculator
		{
			public List<ShippingMethodWithRate> GetShippingMethodsWithRates(OrderCustomer orderCustomer)
			{
				Contract.Requires(orderCustomer != null);

				throw new NotImplementedException();
			}

			public List<ShippingMethodWithRate> GetShippingMethodsWithRates(OrderCustomer orderCustomer, OrderShipment orderShipment)
			{
				Contract.Requires(orderCustomer != null);
				Contract.Requires(orderCustomer.Order != null);
				Contract.Requires(orderShipment != null);

				throw new NotImplementedException();
			}

			public List<ShippingMethodWithRate> GetShippingMethodsWithRates(Order order)
			{
				Contract.Requires(order != null);

				throw new NotImplementedException();
			}

			public List<ShippingMethodWithRate> GetShippingMethodsWithRates(Order order, OrderShipment orderShipment)
			{
				Contract.Requires(order != null);

				throw new NotImplementedException();
			}

			public ShippingMethodWithRate GetLeastExpensiveShippingMethod(OrderCustomer orderCustomer, OrderShipment orderShipment)
			{
				throw new NotImplementedException();
			}

			public ShippingMethodWithRate GetLeastExpensiveShippingMethod(Order order)
			{
				throw new NotImplementedException();
			}

			public void CalculateShipping(OrderCustomer orderCustomer)
			{
				Contract.Requires<ArgumentNullException>(orderCustomer != null);

				throw new NotImplementedException();
			}

			public void CalculatePartyShipping(Order order)
			{
				throw new NotImplementedException();
			}

			public void SetPartyShipmentChargeForCustomer(OrderCustomer orderCustomer)
			{
				Contract.Requires(orderCustomer != null);

				throw new NotImplementedException();
			}

			public decimal CalculateTotalForShipping(OrderCustomer orderCustomer, OrderShipment orderShipment)
			{
				Contract.Requires(orderCustomer != null);
				Contract.Requires(orderShipment != null);

				throw new NotImplementedException();
			}

			public bool IncludeMaxValuesWhenExceedingAllRangesForShippingRates
			{
				get
				{
					throw new NotImplementedException();
				}
			}
		}
	}
}