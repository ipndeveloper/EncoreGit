using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using NetSteps.Common.Extensions;
using NetSteps.Common.Interfaces;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Data.Entities.Tax
{
	/// <summary>
	/// An <see cref="ITaxService"/> that delegates to other <see cref="ITaxService"/> instances, based on country.
	/// </summary>
	//[ContainerRegister(typeof(ITaxService), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
	public class MultiCountryTaxService : ITaxService, IDefaultImplementation
	{
		private readonly ConcurrentDictionary<int, ITaxService> _countryTaxCalculators = new ConcurrentDictionary<int, ITaxService>();

		public MultiCountryTaxService()
		{
			LoadDefaultCalculators();
		}

		public MultiCountryTaxService(List<KeyValuePair<int, ITaxService>> countryTaxCalculators)
		{
			Contract.Requires<ArgumentNullException>(countryTaxCalculators != null);
			Contract.Requires<ArgumentException>(countryTaxCalculators.All(kvp => kvp.Value != null));

			LoadDefaultCalculators();

			foreach (var kvp in countryTaxCalculators)
			{
				_countryTaxCalculators.TryAdd(kvp.Key, kvp.Value);
			}
		}

		#region Private Methods
		private void LoadDefaultCalculators()
		{
			// Use BaseTaxService when we don't know the country.
			_countryTaxCalculators[0] = new BaseTaxService();

			// Use USTaxService for USA.
			var usa = Country.GetCountries()
				.FirstOrDefault(c => c.CountryCode.EqualsIgnoreCase("US"));
			if (usa != null)
			{
				_countryTaxCalculators[usa.CountryID] = new USTaxService();
			}
		}

		private ITaxService GetTaxCalculator(Order order)
		{
			Contract.Requires<ArgumentNullException>(order != null);

			return GetTaxCalculator(GetCountryIdForOrder(order));
		}

		private ITaxService GetTaxCalculator(OrderCustomer orderCustomer)
		{
			Contract.Requires<ArgumentNullException>(orderCustomer != null);

			return GetTaxCalculator(GetCountryIdForOrderCustomer(orderCustomer));
		}

		private int GetCountryIdForOrder(Order order)
		{
			Contract.Requires<ArgumentNullException>(order != null);

			var address = order.GetDefaultShipmentNoDefault();
			return address == null
				? 0
				: address.CountryID;
		}

		private int GetCountryIdForOrderCustomer(OrderCustomer orderCustomer)
		{
			Contract.Requires<ArgumentNullException>(orderCustomer != null);

			var address = orderCustomer.OrderShipments.GetDefaultShippingAddress();
			return address == null
				? 0
				: address.CountryID;
		}

		private ITaxService GetTaxCalculator(int countryId)
		{
			ITaxService taxCalculator;
			if (_countryTaxCalculators.TryGetValue(countryId, out taxCalculator))
			{
				return taxCalculator;
			}
			else if (_countryTaxCalculators.TryGetValue(0, out taxCalculator))
			{
				return taxCalculator;
			}
			else
			{
				throw new Exception("Tax calculator not found for country ID: " + countryId);
			}
		}
		#endregion

		#region Public Methods
		public virtual void CalculateTax(OrderCustomer orderCustomer)
		{
			GetTaxCalculator(orderCustomer).CalculateTax(orderCustomer);
		}

		public virtual void CalculateReturnOrderTax(OrderCustomer orderCustomer)
		{
			GetTaxCalculator(orderCustomer).CalculateReturnOrderTax(orderCustomer);
		}

		public virtual void CalculatePartyTax(Order order)
		{
			GetTaxCalculator(order).CalculatePartyTax(order);
		}

		public virtual void FinalizeTax(OrderCustomer customer)
		{
			GetTaxCalculator(customer).FinalizeTax(customer);
		}

		public virtual void CancelTax(OrderCustomer orderCustomer)
		{
			GetTaxCalculator(orderCustomer).CancelTax(orderCustomer);
		}

		public virtual void FinalizePartialReturnTax(OrderCustomer orderCustomer)
		{
			GetTaxCalculator(orderCustomer).FinalizePartialReturnTax(orderCustomer);
		}
		#endregion
	}
}
