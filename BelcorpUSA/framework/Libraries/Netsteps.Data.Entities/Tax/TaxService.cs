using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using NetSteps.Addresses.Common.Models;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Encore.Core.IoC;
using NetSteps.Taxes.Common;
using NetSteps.Taxes.Common.Models;

namespace NetSteps.Data.Entities.Tax
{
	/// <summary>
	/// The future default <see cref="ITaxService"/> implementation.
	/// This service contains logic for applying taxes to orders, but it uses an injected <see cref="ITaxCalculator"/>
	/// to perform the actual tax calculations. Third-party tax providers need only provide an <see cref="ITaxCalculator"/>
	/// implementation and the rest of the tax-related infrastructure can still be shared in this service.
	/// 
	/// TODO: We need to finish porting <see cref="BaseTaxService"/> into this class and into a default <see cref="ITaxCalculator"/>
	/// so that this can be the default implementation. Until then, <see cref="BaseTaxService"/> is still the default
	/// and this class derives from it in order to keep the code DRY.
	/// </summary>
	public class TaxService : BaseTaxService
	{
		private readonly Lazy<ITaxCalculator> _taxCalculatorFactory;
		public virtual ITaxCalculator TaxCalculator { get { return _taxCalculatorFactory.Value; } }

		private readonly Lazy<InventoryBaseRepository> _inventoryFactory;
		protected virtual InventoryBaseRepository Inventory { get { return _inventoryFactory.Value; } }

		/// <summary>
		/// Initializes a new instance of the <see cref="TaxService"/> class.
		/// </summary>
		/// <param name="taxCalculatorFactory">An <see cref="ITaxCalculator"/> factory for lazy instantiation.</param>
		/// <param name="inventoryFactory">An <see cref="InventoryBaseRepository"/> factory for lazy instantiation.</param>
		public TaxService(
			Func<ITaxCalculator> taxCalculatorFactory,
			Func<InventoryBaseRepository> inventoryFactory)
		{
			Contract.Requires<ArgumentNullException>(taxCalculatorFactory != null);
			Contract.Requires<ArgumentNullException>(inventoryFactory != null);

			_taxCalculatorFactory = new Lazy<ITaxCalculator>(taxCalculatorFactory);
			_inventoryFactory = new Lazy<InventoryBaseRepository>(inventoryFactory);
		}

		#region ITaxService Methods
		public override void CalculateTax(OrderCustomer customer)
		{
			Contract.Requires<ArgumentNullException>(customer != null);
			Contract.Requires<ArgumentException>(customer.Order != null);

			CallTaxCalculator(customer, TaxCalculator.CalculateTax);
		}

		public override void CalculatePartyTax(Order order)
		{
			// already handled when CalculateTax is called for the party Hostess (as long as CalculatePartyTax has already been called)
		}

		public override void FinalizeTax(OrderCustomer customer)
		{
			CallTaxCalculator(customer, TaxCalculator.FinalizeTax);
		}
		#endregion

		#region Helpers
		protected virtual void CallTaxCalculator(OrderCustomer customer, Func<ITaxOrder, ITaxCalculationResult> calculatorFunc)
		{
			Contract.Requires<ArgumentNullException>(customer != null);
			Contract.Requires<ArgumentException>(customer.Order != null);
			Contract.Requires<ArgumentNullException>(calculatorFunc != null);

			var order = customer.Order;
			var shippingTaxAddress = (customer.OrderShipments.GetDefaultShippingAddress() ?? order.OrderShipments.GetDefaultShippingAddress()).ToTaxAddress();
			var taxOrder = CreateTaxOrder(order, shippingTaxAddress);
			var taxCustomer = CreateTaxCustomer(customer, shippingTaxAddress);
			taxOrder.Customers.Add(taxCustomer);

			var customerShippingTotal = customer.ShippingTotal ?? 0m;
			if (customerShippingTotal > 0m)
			{
				var shippingLineItem = CreateNonProductTaxOrderItem(TaxApplicabilityKind.Shipping, customerShippingTotal, 1, default(ITaxAddress), shippingTaxAddress);
				taxCustomer.Items.Add(shippingLineItem);
			}

			if (customer.IsHostess)
			{
				var partyShippingTotal = order.PartyShipmentTotal ?? 0m;
				if (partyShippingTotal > 0m)
				{
					var shippingLineItem = CreateNonProductTaxOrderItem(TaxApplicabilityKind.OrderShipping, partyShippingTotal, 1, default(ITaxAddress), shippingTaxAddress);
					taxCustomer.Items.Add(shippingLineItem);
				}
			}

			foreach (var item in customer.ParentOrderItems)
			{
				var itemOriginAddress = GetOrderItemOrigin(item).ToTaxAddress();
					
				var taxOrderItem = CreateTaxOrderItem(item, itemOriginAddress, shippingTaxAddress);
				taxCustomer.Items.Add(taxOrderItem);
			}

			////////
			var taxResult = calculatorFunc(taxOrder);
			////////

			if (taxResult == null || taxResult.Status == TaxCalculationState.Faulted)
			{
				throw taxResult.Fault.ToBusinessException();
			}
			else if (taxResult.Status == TaxCalculationState.Succeeded)
			{
				ApplyTaxCalculationToCustomer(customer, taxResult.Order);
				return;
			}
			// else (skipped) <- no order items were provided to get tax data for
		}

		protected virtual ITaxOrder CreateTaxOrder(Order order, ITaxAddress shippingTaxAddress)
		{
			Contract.Requires<ArgumentNullException>(order != null);

			var taxOrder = Create.New<ITaxOrder>();
			taxOrder.Customers = new List<ITaxCustomer>();
			taxOrder.OrderID = Convert.ToString(order.OrderID);
			if (shippingTaxAddress != null && shippingTaxAddress.Country != null)
			{
				taxOrder.CountryCode = shippingTaxAddress.Country.Code;
			}

			return taxOrder;
		}

		protected virtual ITaxCustomer CreateTaxCustomer(OrderCustomer customer, ITaxAddress shippingAddress)
		{
			Contract.Requires<ArgumentNullException>(customer != null);

			var taxCustomer = Create.New<ITaxCustomer>();
			taxCustomer.CustomerID = GetTaxKey(customer);
			taxCustomer.IsTaxExempt = customer.IsTaxExempt ?? customer.AccountInfo != null ? customer.AccountInfo.IsTaxExempt ?? false : false;
			taxCustomer.Items = new List<ITaxOrderItem>();
			taxCustomer.ShippingAddress = shippingAddress;
			return taxCustomer;
		}

		protected virtual ITaxOrderItem CreateTaxOrderItem(OrderItem item, ITaxAddress itemOriginAddress, ITaxAddress itemShippingAddress)
		{
			Contract.Requires<ArgumentNullException>(item != null);
			Contract.Requires<ArgumentException>(item.ProductID > 0);

			var product = Inventory.GetProduct(item.ProductID.Value);

			var taxOrderItem = Create.New<ITaxOrderItem>();
			taxOrderItem.Taxes = new List<ITaxOrderItemTaxes>();
			taxOrderItem.Applicability = TaxApplicabilityKind.Price;
			taxOrderItem.ItemID = GetTaxKey(item);
			taxOrderItem.OriginAddress = itemOriginAddress;
			taxOrderItem.ShippingAddress = itemShippingAddress;
			taxOrderItem.UnitPrice = item.GetAdjustedPrice(item.ProductPriceTypeID ?? (int)Constants.ProductPriceType.Retail);
			taxOrderItem.Quantity = item.Quantity;
			taxOrderItem.ShippingFee = item.GetAdjustedPrice((int)Constants.ProductPriceType.ShippingFee);
			taxOrderItem.HandlingFee = item.GetAdjustedPrice((int)Constants.ProductPriceType.HandlingFee);
			taxOrderItem.ProductCode = item.SKU;
			taxOrderItem.ChargeTax = product.ProductBase.ChargeTax;

			// new promotions and OrderItem changes in the works that will probably replace this and GCY isn't using it anyway, but leaving here for a reminder that vertex supports it
			//taxOrderItem.DiscountKind = item.Discount.HasValue ? DiscountKind.Amount : item.DiscountPercent.HasValue ? DiscountKind.Percent : DiscountKind.None;
			//taxOrderItem.Discount = item.Discount.HasValue ? item.Discount.Value : item.DiscountPercent.HasValue ? item.DiscountPercent.Value : 0m;

			return taxOrderItem;
		}

		protected virtual ITaxOrderItem CreateNonProductTaxOrderItem(
			TaxApplicabilityKind taxAvailabilityKind,
			decimal unitPrice,
			int quantity,
			ITaxAddress itemOriginAddress,
			ITaxAddress itemShippingAddress)
		{
			Contract.Requires<InvalidOperationException>(taxAvailabilityKind != TaxApplicabilityKind.Price);

			var taxOrderItem = Create.New<ITaxOrderItem>();
			taxOrderItem.Taxes = new List<ITaxOrderItemTaxes>();
			taxOrderItem.ItemID = "S" + Guid.NewGuid().ToString("N");
			taxOrderItem.ProductCode = taxAvailabilityKind.ToString();
			taxOrderItem.Applicability = taxAvailabilityKind;
			taxOrderItem.OriginAddress = itemOriginAddress;
			taxOrderItem.ShippingAddress = itemShippingAddress;
			taxOrderItem.Quantity = quantity;
			taxOrderItem.UnitPrice = unitPrice;
			taxOrderItem.ChargeTax = true;
			return taxOrderItem;
		}

		protected virtual bool IsTaxInfoAvailable(ITaxAddress taxAddress)
		{
			if (taxAddress == null)
			{
				return false;
			}

			var taxArea = TaxCalculator.LookupTaxArea(taxAddress);

			return taxArea != null && !String.IsNullOrWhiteSpace(taxArea.TaxAreaID);
		}

		protected virtual IAddress GetOrderItemOrigin(OrderItem item)
		{
			if (item.ProductID == null)
			{
				return null;
			}

			var product = Inventory.GetProduct(item.ProductID.Value);
			var warehouseProduct = product.WarehouseProducts.FirstOrDefault();
			if (warehouseProduct == null
				|| warehouseProduct.Warehouse == null
				|| warehouseProduct.Warehouse.AddressID == null)
			{
				return null;
			}

			return Address.LoadFull(warehouseProduct.Warehouse.AddressID.Value);
		}
		
		protected virtual void ApplyTaxCalculationToCustomer(OrderCustomer customer, ITaxOrder taxOrder)
		{
			Contract.Requires<ArgumentNullException>(customer != null);
			Contract.Requires<ArgumentNullException>(taxOrder != null);

			var taxCustomer = taxOrder.Customers.SingleOrDefault(c => c.CustomerID == GetTaxKey(customer));
			if (taxCustomer == default(ITaxCustomer))
			{
				throw new InvalidOperationException(string.Format("Did not receive tax results for order customer id {0}.", GetTaxKey(customer)));
			}

			customer.TaxableTotal = 0m;
			customer.TaxAmountOrderItems = 0m;
			customer.TaxAmountShipping = 0m;
			foreach (var taxOrderItem in taxCustomer.Items)
			{
				switch (taxOrderItem.Applicability)
				{
					case TaxApplicabilityKind.Shipping:
						ProcessShippingTaxes(taxOrderItem, customer);
						break;
					case TaxApplicabilityKind.OrderShipping:
						ProcessOrderShippingTaxes(taxOrderItem, customer);
						break;
					case TaxApplicabilityKind.Price:
					default:
						ProcessProductTaxes(taxOrderItem, customer);
						break;
				}
			}

			customer.TaxAmountTotal = customer.TaxAmountOrderItems + customer.TaxAmountShipping;
		}

		protected virtual void ProcessProductTaxes(ITaxOrderItem taxOrderItem, OrderCustomer customer)
		{
			var orderItem = customer.ParentOrderItems.SingleOrDefault(i => taxOrderItem.ItemID == GetTaxKey(i));
			if (orderItem == default(ITaxOrderItem))
			{
				throw new InvalidOperationException(string.Format("Error matching tax results to order item id {0}.", taxOrderItem.ItemID));
			}

			var orderItemTaxes = orderItem.Taxes;

			// Primary jurisdictions...
			orderItem.TaxPercentCity = orderItemTaxes.TaxPercentCity = taxOrderItem.GetEffectiveRateByLevel(JurisdictionLevel.City);
			orderItem.TaxAmountCity = orderItemTaxes.TaxAmountCity = taxOrderItem.GetCalculatedTaxByLevel(JurisdictionLevel.City);
			orderItem.TaxPercentCounty = orderItemTaxes.TaxPercentCounty = taxOrderItem.GetEffectiveRateByLevel(JurisdictionLevel.County, JurisdictionLevel.Parish /*LA*/, JurisdictionLevel.Borough /*AK*/);
			orderItem.TaxAmountCounty = orderItemTaxes.TaxAmountCounty = taxOrderItem.GetCalculatedTaxByLevel(JurisdictionLevel.County, JurisdictionLevel.Parish /*LA*/, JurisdictionLevel.Borough /*AK*/);
			orderItem.TaxPercentState = orderItemTaxes.TaxPercentState = taxOrderItem.GetEffectiveRateByLevel(JurisdictionLevel.State, JurisdictionLevel.Province, JurisdictionLevel.Territory);
			orderItem.TaxAmountState = orderItemTaxes.TaxAmountState = taxOrderItem.GetCalculatedTaxByLevel(JurisdictionLevel.State, JurisdictionLevel.Territory, JurisdictionLevel.Province);
			orderItem.TaxPercentCountry = orderItemTaxes.TaxPercentCountry = taxOrderItem.GetEffectiveRateByLevel(JurisdictionLevel.Country);
			orderItem.TaxAmountCountry = orderItemTaxes.TaxAmountCountry = taxOrderItem.GetCalculatedTaxByLevel(JurisdictionLevel.Country);

			// Extraneous jurisdictions...
			orderItemTaxes.TaxPercentCityLocal = orderItemTaxes.TaxPercentCountyLocal = taxOrderItem.GetEffectiveRateByLevel(JurisdictionLevel.LocalImprovementDistrict);
			orderItem.TaxAmountCityLocal = orderItem.TaxAmountCountyLocal = orderItemTaxes.TaxAmountCityLocal = orderItemTaxes.TaxAmountCountyLocal = taxOrderItem.GetCalculatedTaxByLevel(JurisdictionLevel.LocalImprovementDistrict);
			orderItem.TaxPercentDistrict = orderItemTaxes.TaxPercentDistrict = taxOrderItem.GetEffectiveRateByLevel(JurisdictionLevel.District);
			orderItem.TaxAmountDistrict = orderItemTaxes.TaxAmountDistrict = taxOrderItem.GetCalculatedTaxByLevel(JurisdictionLevel.District);

			// update item totals
			orderItem.TaxableTotal = taxOrderItem.GetTaxableAmount();
			orderItem.TaxAmount = taxOrderItem.GetCalculatedTax();

			// add to customer totals
			customer.TaxableTotal += orderItem.TaxableTotal;
			customer.TaxAmountOrderItems += orderItem.TaxAmount;
		}

		protected virtual void ProcessShippingTaxes(ITaxOrderItem taxOrderItem, OrderCustomer customer)
		{
			var shippingTax = taxOrderItem.GetCalculatedTax();
			customer.TaxAmountShipping += shippingTax;
		}

		protected virtual void ProcessOrderShippingTaxes(ITaxOrderItem taxOrderItem, OrderCustomer customer)
		{
			var shippingTaxableAmount = taxOrderItem.GetTaxableAmount();
			var shippingTax = taxOrderItem.GetCalculatedTax();
			var order = customer.Order;
			order.TaxableTotal += shippingTaxableAmount;
			order.TaxAmountShipping += shippingTax;
			order.TaxAmountTotal += shippingTax;
			order.GrandTotal += shippingTax;
		}

		protected virtual string GetTaxKey(OrderCustomer customer)
		{
			return customer.OrderCustomerID > 0 ? Convert.ToString(customer.OrderCustomerID) : "G" + customer.Guid.ToString("N");
		}

		protected virtual string GetTaxKey(OrderItem orderItem)
		{
			return orderItem.OrderItemID > 0 ? Convert.ToString(orderItem.OrderItemID) : "G" + orderItem.Guid.ToString("N");
		}
		#endregion
	}
}
