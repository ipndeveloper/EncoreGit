using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using NetSteps.Addresses.Common.Models;
using NetSteps.Common.Extensions;
using NetSteps.Core.Cache;
using NetSteps.Data.Common.Services;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Generated;
using NetSteps.Data.Entities.Services;
using NetSteps.Encore.Core.Dto;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Data.Entities.Tax
{
	[DTO]
	public interface ITaxCacheKey
	{

		bool IsDetailedKey { get; set; }

		bool UseProvince { get; set; }

		int CountryID { get; set; }

		string PostalCode { get; set; }

		string StateAbbr { get; set; }

		string County { get; set; }

		string City { get; set; }
	}

	/// <summary>
	/// This is the "legacy" tax service formerly known as BaseTaxCalculator.
	/// It is still used by ALL the other <see cref="ITaxService"/> implementations,
	/// but it needs to be replaced by <see cref="TaxService"/> and by a
	/// default implementation of the new <see cref="ITaxCalculator"/> interface.
	/// </summary>
	public class BaseTaxService : ITaxService
	{
		public enum HalfPriceItemTaxTreatment
		{
			NetSellingPrice,
			GrossBeforeCredit,
			NoTax
		}

		public enum HostessCreditItemTaxTreatment
		{
			Cost,
			SuggestedRetail,
			NoTax
		}
		#region Private Fields
		internal static readonly Dictionary<string, HalfPriceItemTaxTreatment> HalfPriceItemTreatment
				= new Dictionary<string, HalfPriceItemTaxTreatment>	{
						{ "AL", HalfPriceItemTaxTreatment.NetSellingPrice },
						{ "AK", HalfPriceItemTaxTreatment.GrossBeforeCredit },
						{ "AZ", HalfPriceItemTaxTreatment.GrossBeforeCredit },
						{ "AR", HalfPriceItemTaxTreatment.GrossBeforeCredit },
						{ "CA", HalfPriceItemTaxTreatment.GrossBeforeCredit },
						{ "CO", HalfPriceItemTaxTreatment.NetSellingPrice },
						{ "CT", HalfPriceItemTaxTreatment.NetSellingPrice },
						{ "DE", HalfPriceItemTaxTreatment.NoTax },
						{ "DC", HalfPriceItemTaxTreatment.NetSellingPrice },
						{ "FL", HalfPriceItemTaxTreatment.NetSellingPrice },
						{ "GA", HalfPriceItemTaxTreatment.GrossBeforeCredit },
						{ "HI", HalfPriceItemTaxTreatment.NetSellingPrice },
						{ "ID", HalfPriceItemTaxTreatment.GrossBeforeCredit },
						{ "IL", HalfPriceItemTaxTreatment.NetSellingPrice },
						{ "IN", HalfPriceItemTaxTreatment.NetSellingPrice },
						{ "IA", HalfPriceItemTaxTreatment.NetSellingPrice },
						{ "KS", HalfPriceItemTaxTreatment.GrossBeforeCredit },
						{ "KY", HalfPriceItemTaxTreatment.NetSellingPrice},
						{ "LA", HalfPriceItemTaxTreatment.GrossBeforeCredit },
						{ "ME", HalfPriceItemTaxTreatment.NetSellingPrice },
						{ "MD", HalfPriceItemTaxTreatment.GrossBeforeCredit },
						{ "MA", HalfPriceItemTaxTreatment.NetSellingPrice },
						{ "MI", HalfPriceItemTaxTreatment.NetSellingPrice },
						{ "MN", HalfPriceItemTaxTreatment.GrossBeforeCredit },
						{ "MS", HalfPriceItemTaxTreatment.NetSellingPrice },
						{ "MO", HalfPriceItemTaxTreatment.NetSellingPrice },
						{ "MT", HalfPriceItemTaxTreatment.NoTax },
						{ "NE", HalfPriceItemTaxTreatment.NetSellingPrice },
						{ "NV", HalfPriceItemTaxTreatment.GrossBeforeCredit },
						{ "NH", HalfPriceItemTaxTreatment.NoTax },
						{ "NJ", HalfPriceItemTaxTreatment.NetSellingPrice },
						{ "NM", HalfPriceItemTaxTreatment.NetSellingPrice },
						{ "NY", HalfPriceItemTaxTreatment.GrossBeforeCredit },
						{ "NC", HalfPriceItemTaxTreatment.GrossBeforeCredit },
						{ "ND", HalfPriceItemTaxTreatment.NetSellingPrice },
						{ "OH", HalfPriceItemTaxTreatment.NetSellingPrice },
						{ "OK", HalfPriceItemTaxTreatment.GrossBeforeCredit },
						{ "OR", HalfPriceItemTaxTreatment.NoTax },
						{ "PA", HalfPriceItemTaxTreatment.GrossBeforeCredit },
						{ "RI", HalfPriceItemTaxTreatment.NetSellingPrice },
						{ "SC", HalfPriceItemTaxTreatment.NetSellingPrice },
						{ "SD", HalfPriceItemTaxTreatment.NetSellingPrice },
						{ "TN", HalfPriceItemTaxTreatment.GrossBeforeCredit },
						{ "TX", HalfPriceItemTaxTreatment.NetSellingPrice },
						{ "UT", HalfPriceItemTaxTreatment.NetSellingPrice },
						{ "VT", HalfPriceItemTaxTreatment.GrossBeforeCredit },
						{ "VA", HalfPriceItemTaxTreatment.NetSellingPrice },
						{ "WA", HalfPriceItemTaxTreatment.NetSellingPrice },
						{ "WV", HalfPriceItemTaxTreatment.GrossBeforeCredit },
						{ "WI", HalfPriceItemTaxTreatment.NetSellingPrice },
						{ "WY", HalfPriceItemTaxTreatment.NetSellingPrice }
					};

		internal static readonly Dictionary<string, HostessCreditItemTaxTreatment> HostessCreditItemTreatment
				= new Dictionary<string, HostessCreditItemTaxTreatment>
						{
								{ "AL", HostessCreditItemTaxTreatment.Cost },
						{ "AK", HostessCreditItemTaxTreatment.NoTax },
						{ "AZ", HostessCreditItemTaxTreatment.Cost },
						{ "AR", HostessCreditItemTaxTreatment.SuggestedRetail },
						{ "CA", HostessCreditItemTaxTreatment.Cost },
						{ "CO", HostessCreditItemTaxTreatment.Cost },
						{ "CT", HostessCreditItemTaxTreatment.Cost },
						{ "DE", HostessCreditItemTaxTreatment.NoTax },
						{ "DC", HostessCreditItemTaxTreatment.Cost },
						{ "FL", HostessCreditItemTaxTreatment.Cost },
						{ "GA", HostessCreditItemTaxTreatment.Cost },
						{ "HI", HostessCreditItemTaxTreatment.Cost },
						{ "ID", HostessCreditItemTaxTreatment.SuggestedRetail },
						{ "IL", HostessCreditItemTaxTreatment.Cost },
						{ "IN", HostessCreditItemTaxTreatment.Cost },
						{ "IA", HostessCreditItemTaxTreatment.Cost },
						{ "KS", HostessCreditItemTaxTreatment.SuggestedRetail },
						{ "KY", HostessCreditItemTaxTreatment.SuggestedRetail },
						{ "LA", HostessCreditItemTaxTreatment.Cost },
						{ "ME", HostessCreditItemTaxTreatment.Cost },
						{ "MD", HostessCreditItemTaxTreatment.Cost },
						{ "MA", HostessCreditItemTaxTreatment.Cost },
						{ "MI", HostessCreditItemTaxTreatment.Cost },
						{ "MN", HostessCreditItemTaxTreatment.Cost },
						{ "MS", HostessCreditItemTaxTreatment.Cost },
						{ "MO", HostessCreditItemTaxTreatment.Cost },
						{ "MT", HostessCreditItemTaxTreatment.NoTax },
						{ "NE", HostessCreditItemTaxTreatment.Cost },
						{ "NV", HostessCreditItemTaxTreatment.Cost },
						{ "NH", HostessCreditItemTaxTreatment.NoTax },
						{ "NJ", HostessCreditItemTaxTreatment.Cost },
						{ "NM", HostessCreditItemTaxTreatment.Cost },
						{ "NY", HostessCreditItemTaxTreatment.SuggestedRetail },
						{ "NC", HostessCreditItemTaxTreatment.Cost },
						{ "ND", HostessCreditItemTaxTreatment.Cost },
						{ "OH", HostessCreditItemTaxTreatment.Cost },
						{ "OK", HostessCreditItemTaxTreatment.SuggestedRetail },
						{ "OR", HostessCreditItemTaxTreatment.NoTax },
						{ "PA", HostessCreditItemTaxTreatment.Cost },
						{ "RI", HostessCreditItemTaxTreatment.Cost },
						{ "SC", HostessCreditItemTaxTreatment.Cost },
						{ "SD", HostessCreditItemTaxTreatment.Cost },
						{ "TN", HostessCreditItemTaxTreatment.Cost },
						{ "TX", HostessCreditItemTaxTreatment.Cost },
						{ "UT", HostessCreditItemTaxTreatment.Cost },
						{ "VT", HostessCreditItemTaxTreatment.Cost },
						{ "VA", HostessCreditItemTaxTreatment.Cost },
						{ "WA", HostessCreditItemTaxTreatment.Cost },
						{ "WV", HostessCreditItemTaxTreatment.SuggestedRetail },
						{ "WI", HostessCreditItemTaxTreatment.Cost },
						{ "WY", HostessCreditItemTaxTreatment.Cost }
						};
		#endregion

		#region Public Methods

		public virtual void CalculateTax(OrderCustomer orderCustomer)
		{
			var order = orderCustomer.Order;
			ValidateTax(order, orderCustomer);

			try
			{
				OrderShipment shipment = orderCustomer.Order.GetDefaultShipmentNoDefault();

				if (shipment == null || !(shipment as IAddress).IsTaxInfoAvailable())
				{
					orderCustomer.TaxableTotal = 0;
					orderCustomer.TaxAmountTotal = 0;

					foreach (OrderItem orderItem in orderCustomer.ParentOrderItems)
					{
						decimal taxablePrice = 0;
						if (orderItem.HasChildOrderItems)
						{
							CalculateKitTaxableTotal(order, orderCustomer, orderItem);
						}
						else
						{
							taxablePrice = GetTaxablePriceForOrderItem(order, orderCustomer, orderItem, orderItem.ProductID.ToInt());
							orderItem.Taxes.TaxableTotal = taxablePrice * orderItem.Quantity;
						}
					}
					return;
				}

				var taxes = GetTaxInfo(shipment);

				ResetOrderCustomerTotals(orderCustomer);

				// Get highest item tax rate for shipping taxes
				decimal taxAmountShipping = 0.00m;

				// Add taxes on shipping
				// If any item is taxable for shipping, shipping tax is applied to the whole order. (May need to modify this to the item level.)
				bool chargeTaxOnShipping = taxes.Any(t => t.ChargeTaxOnShipping == true);

				var inventory = Create.New<InventoryBaseRepository>();

				foreach (OrderItem orderItem in orderCustomer.ParentOrderItems)
				{
					var product = inventory.GetProduct(orderItem.ProductID.ToInt());
					bool itemChargeTaxOnShipping = (chargeTaxOnShipping && product.ProductBase.ChargeTaxOnShipping);
					if (!product.ProductBase.ChargeTaxOnShipping && orderItem.ChildOrderItems.Count == 0)
						chargeTaxOnShipping = itemChargeTaxOnShipping;

					// This account is tax exempt or there is no tax on the item
					if (orderCustomer.IsTaxExempt.ToBool())
					{
						orderItem.Taxes.TaxAmountTotal = 0;
						//continue;
					}
					bool itemIsTaxable = true;

					// If it is a kit with child items, get the taxable value for each individual child item
					// Just don't put the key in the appsettings if you don't want to use this logic
					if (orderItem.ChildOrderItems.Count > 0) // It is a kit
					//item.Product.Relations.ContainsKey(kitChildItemRelationshipId) && !item.Product.BaseProduct.IsTaxedAtChild)
					{
						CalculateKitItemSalesTax(order, orderCustomer, orderItem, taxes, ref taxAmountShipping, ref chargeTaxOnShipping);
					}
					else // It is not a kit 
					{
						itemIsTaxable = !orderCustomer.IsTaxExempt.ToBool() && IsItemTaxable(product.ProductBase.ChargeTax, orderItem, orderCustomer.OrderPayments.Count > 0 ? orderCustomer.OrderPayments[0].BillingState : null);
						//if (orderCustomer.IsTaxExempt.ToBool() || (!product.ProductBase.ChargeTax && !itemChargeTaxOnShipping))
						//continue;

						// Figure out the taxable amount
						decimal taxablePrice;

						taxablePrice = GetTaxablePriceForOrderItem(order, orderCustomer, orderItem, product.ProductID);

						// Set the tax percentages on the order item 
						SetItemTaxes(orderItem, taxes, ref taxAmountShipping, itemIsTaxable);

						if (itemIsTaxable)
						{
							orderItem.Taxes.TaxableTotal = NetSteps.Common.Extensions.DecimalExtensions.GetRoundedNumber(taxablePrice * orderItem.Quantity, 2);
							orderItem.Taxes.TaxAmountTotal = NetSteps.Common.Extensions.DecimalExtensions.GetRoundedNumber(orderItem.Taxes.TaxableTotal * orderItem.Taxes.TaxPercent, 2);
							orderItem.Taxes.TaxAmountCity = NetSteps.Common.Extensions.DecimalExtensions.GetRoundedNumber(orderItem.Taxes.TaxableTotal * orderItem.Taxes.TaxPercentCity, 2);
							orderItem.Taxes.TaxAmountCityLocal = NetSteps.Common.Extensions.DecimalExtensions.GetRoundedNumber(orderItem.Taxes.TaxableTotal * orderItem.Taxes.TaxPercentCityLocal, 2);
							orderItem.Taxes.TaxAmountCounty = NetSteps.Common.Extensions.DecimalExtensions.GetRoundedNumber(orderItem.Taxes.TaxableTotal * orderItem.Taxes.TaxPercentCounty, 2);
							orderItem.Taxes.TaxAmountCountyLocal = NetSteps.Common.Extensions.DecimalExtensions.GetRoundedNumber(orderItem.Taxes.TaxableTotal * orderItem.Taxes.TaxPercentCountyLocal, 2);
							orderItem.Taxes.TaxAmountState = NetSteps.Common.Extensions.DecimalExtensions.GetRoundedNumber(orderItem.Taxes.TaxableTotal * orderItem.Taxes.TaxPercentState, 2);
							orderItem.Taxes.TaxAmountDistrict = NetSteps.Common.Extensions.DecimalExtensions.GetRoundedNumber(orderItem.Taxes.TaxableTotal * orderItem.Taxes.TaxPercentDistrict, 2);
							orderItem.Taxes.TaxAmountShipping = taxAmountShipping;
							orderItem.Taxes.TaxAmountCountry = NetSteps.Common.Extensions.DecimalExtensions.GetRoundedNumber(orderItem.Taxes.TaxableTotal * orderItem.TaxPercentCountry.ToDecimal(), 2);
						}
					}
					CopyTaxValuesToOrderItem(orderItem);

					if (itemIsTaxable)
					{
						// Update the customer's totals
						orderCustomer.TaxableTotal += orderItem.Taxes.TaxableTotal;
						orderCustomer.TaxAmountOrderItems += orderItem.Taxes.TaxAmountTotal;
						orderCustomer.TaxAmountCity += orderItem.Taxes.TaxAmountCity;
						orderCustomer.TaxAmountCounty += orderItem.Taxes.TaxAmountCounty;
						orderCustomer.TaxAmountState += orderItem.Taxes.TaxAmountState;
						orderCustomer.TaxAmountDistrict += orderItem.Taxes.TaxAmountDistrict;
						orderCustomer.TaxAmountCountry += orderItem.Taxes.TaxAmountCountry;
					}
				}
				//);

				// TODO: Finish & TEST - JHE
				if (chargeTaxOnShipping && !(orderCustomer.IsTaxExempt ?? false))
				{
					decimal taxableShippingPrice = orderCustomer.ShippingTotal.ToDecimal();
					orderCustomer.TaxableTotal += taxableShippingPrice;
					orderCustomer.TaxAmountShipping += NetSteps.Common.Extensions.DecimalExtensions.GetRoundedNumber(taxableShippingPrice * taxAmountShipping, 2);
				}

				orderCustomer.TaxAmountTotal = orderCustomer.TaxAmountOrderItems.ToDecimal() + orderCustomer.TaxAmountShipping.ToDecimal();
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		protected virtual void ValidateTax(Order order, OrderCustomer orderCustomer)
		{
			if (!orderCustomer.IsTaxExempt.HasValue || !order.IsCommissionable())
			{
				Account account = orderCustomer.AccountID != 0 ? Account.Load(orderCustomer.AccountID) : null;
				orderCustomer.IsTaxExempt = account != null && account.IsTaxExempt.ToBool();
			}
		}

		public virtual void FinalizeTax(OrderCustomer orderCustomer)
		{

		}

		public virtual void CancelTax(OrderCustomer orderCustomer)
		{

		}

		public virtual void FinalizePartialReturnTax(OrderCustomer orderCustomer)
		{

		}

		public virtual void CalculateReturnOrderTax(OrderCustomer orderCustomer)
		{
			Contract.Requires(orderCustomer != null);
			Contract.Requires(orderCustomer.Order != null);
			Contract.Requires(orderCustomer.Order.IsReturnOrder());

			try
			{
				// Load the original order being returned
				int orderId = orderCustomer.Order.ParentOrderID.ToInt();
				if (orderId == 0)
				{
					throw new Exception("Parent order ID cannot be null for a return order.");
				}

				Order originalOrder = Order.LoadFull(orderId);
				if (String.IsNullOrEmpty(originalOrder.OrderNumber))
				{
					throw new Exception("Original order could not be found for the return order.");
				}

				ResetOrderCustomerTotals(orderCustomer);

				OrderCustomer originalCustomer = originalOrder.OrderCustomers.FirstOrDefault(
						oc => oc.AccountID == orderCustomer.AccountID && oc.OrderItems.Count > 0);

				// Copy tax values / percentages from original items -> if qty != original qty, calc new value from original value
				foreach (OrderItem item in orderCustomer.ParentOrderItems)
				{
					bool hasReturnItems = item.OrderItemReturns.Any();

					OrderItem originalItem = hasReturnItems && IsValidCustomer(originalCustomer)
														? originalCustomer.OrderItems.FirstOrDefault(oi => oi.OrderItemID == item.OrderItemReturns[0].OriginalOrderItemID)
														: null;

					if (originalItem == null || item.OrderItemTypeID == (int)Constants.OrderItemType.Fees)
					{
						// Just don't charge taxes on that item because it's an additional item (restocking fee most likely)
						continue;
					}

					item.TaxNumber = originalItem.TaxNumber;

					bool returningBundleItem = originalItem.ParentOrderItemID != null &&
							!orderCustomer.OrderItems.Any(
									oi =>
											oi.OrderItemReturns != null &&
											oi.ChildOrderItems.Count > 0 &&
											oi.OrderItemReturns[0].OriginalOrderItemID == originalItem.ParentOrderItemID
									);

					OrderItem originalParentOrderItem = null;

					if (returningBundleItem)
					{
						// this item is part of a bundle
						originalParentOrderItem =
								originalCustomer.OrderItems.FirstOrDefault(
										oi => oi.OrderItemID == originalItem.ParentOrderItemID.Value);
                        if (originalParentOrderItem != null)
                        {
                            // Copy tax percentages
                            item.Taxes.TaxPercent = originalParentOrderItem.Taxes.TaxPercent;
                            item.Taxes.TaxPercentCity = originalParentOrderItem.Taxes.TaxPercentCity;
                            item.Taxes.TaxPercentCityLocal = originalParentOrderItem.Taxes.TaxPercentCityLocal;
                            item.Taxes.TaxPercentCounty = originalParentOrderItem.Taxes.TaxPercentCounty;
                            item.Taxes.TaxPercentCountyLocal = originalParentOrderItem.Taxes.TaxPercentCountyLocal;
                            item.Taxes.TaxPercentState = originalParentOrderItem.Taxes.TaxPercentState;
                            item.Taxes.TaxPercentDistrict = originalParentOrderItem.Taxes.TaxPercentDistrict;
                            item.Taxes.TaxPercentCountry = originalParentOrderItem.Taxes.TaxPercentCountry;
                        }
					}
					else
					{
						// Copy tax percentages
						item.Taxes.TaxPercent = originalItem.Taxes.TaxPercent;
						item.Taxes.TaxPercentCity = originalItem.Taxes.TaxPercentCity;
						item.Taxes.TaxPercentCityLocal = originalItem.Taxes.TaxPercentCityLocal;
						item.Taxes.TaxPercentCounty = originalItem.Taxes.TaxPercentCounty;
						item.Taxes.TaxPercentCountyLocal = originalItem.Taxes.TaxPercentCountyLocal;
						item.Taxes.TaxPercentState = originalItem.Taxes.TaxPercentState;
						item.Taxes.TaxPercentDistrict = originalItem.Taxes.TaxPercentDistrict;
						item.Taxes.TaxPercentCountry = originalItem.Taxes.TaxPercentCountry;
					}

                    decimal qtyRatio = 0;
                    if (originalParentOrderItem != null)
                        qtyRatio = returningBundleItem ? Convert.ToDecimal(item.Quantity) / Convert.ToDecimal(originalParentOrderItem.ChildOrderItems.Count) : Convert.ToDecimal(item.Quantity) / Convert.ToDecimal(originalItem.Quantity);
                    else
                        qtyRatio = returningBundleItem ? Convert.ToDecimal(item.Quantity) : Convert.ToDecimal(item.Quantity) / Convert.ToDecimal(originalItem.Quantity);
					if (qtyRatio == 1)
					{
						// Copy tax amounts
						item.Taxes.TaxableTotal = originalItem.Taxes.TaxableTotal;
						item.Taxes.TaxAmountTotal = originalItem.Taxes.TaxAmountTotal;
						item.Taxes.TaxAmountCity = originalItem.Taxes.TaxAmountCity;
						item.Taxes.TaxAmountCityLocal = originalItem.Taxes.TaxAmountCityLocal;
						item.Taxes.TaxAmountCounty = originalItem.Taxes.TaxAmountCounty;
						item.Taxes.TaxAmountCountyLocal = originalItem.Taxes.TaxAmountCountyLocal;
						item.Taxes.TaxAmountState = originalItem.Taxes.TaxAmountState;
						item.Taxes.TaxAmountDistrict = originalItem.Taxes.TaxAmountDistrict;
					}
					else
					{
						// Calc tax amounts
                        if (originalParentOrderItem != null)
                        {
                            item.Taxes.TaxableTotal = returningBundleItem ? originalParentOrderItem.Taxes.TaxableTotal * qtyRatio : originalItem.Taxes.TaxableTotal * qtyRatio;
                            item.Taxes.TaxAmountTotal = NetSteps.Common.Extensions.DecimalExtensions.GetRoundedNumber(item.Taxes.TaxableTotal * item.Taxes.TaxPercent, 2);
                            item.Taxes.TaxAmountCity = NetSteps.Common.Extensions.DecimalExtensions.GetRoundedNumber(item.Taxes.TaxableTotal * item.Taxes.TaxPercentCity, 2);
                            item.Taxes.TaxAmountCityLocal = NetSteps.Common.Extensions.DecimalExtensions.GetRoundedNumber(item.Taxes.TaxableTotal * item.Taxes.TaxPercentCityLocal, 2);
                            item.Taxes.TaxAmountCounty = NetSteps.Common.Extensions.DecimalExtensions.GetRoundedNumber(item.Taxes.TaxableTotal * item.Taxes.TaxPercentCounty, 2);
                            item.Taxes.TaxAmountCountyLocal = NetSteps.Common.Extensions.DecimalExtensions.GetRoundedNumber(item.Taxes.TaxableTotal * item.Taxes.TaxPercentCountyLocal, 2);
                            item.Taxes.TaxAmountState = NetSteps.Common.Extensions.DecimalExtensions.GetRoundedNumber(item.Taxes.TaxableTotal * item.Taxes.TaxPercentState, 2);
                            item.Taxes.TaxAmountDistrict = NetSteps.Common.Extensions.DecimalExtensions.GetRoundedNumber(item.Taxes.TaxableTotal * item.Taxes.TaxPercentDistrict, 2);
                        }
					}

					CopyTaxValuesToOrderItem(item);

					if (item.ParentOrderItemID == null || returningBundleItem)
					{
						// Update the customer's totals
						orderCustomer.TaxableTotal += item.Taxes.TaxableTotal;
						orderCustomer.TaxAmountOrderItems += item.Taxes.TaxAmountTotal;
						orderCustomer.TaxAmountCity += item.Taxes.TaxAmountCity;
						orderCustomer.TaxAmountCounty += item.Taxes.TaxAmountCounty;
						orderCustomer.TaxAmountState += item.Taxes.TaxAmountState;
						orderCustomer.TaxAmountDistrict += item.Taxes.TaxAmountDistrict;
					}
				}

				// BJC -- If the original order charge a tax on shipping then it needs to be returned
				if (IsValidCustomer(originalCustomer) && originalCustomer.TaxAmountShipping > 0 && originalCustomer.ShippingTotal > 0)
				{
					//get count of return items, and count of items on original order - Scott Wilson
					decimal OrderItemQuantity = 0;
					decimal OriginalOrderItemQuantity = 0;

					foreach (var oi in orderCustomer.OrderItems)
					{
						// Again, if it's a fee it's an additional item (restocking fee most likely).
						// Don't include it in the calculation of item quantity on the return order
						// or it will throw off the returned shipping tax.
						if (oi.OrderItemTypeID != (int)Constants.OrderItemType.Fees)
						{
							OrderItemQuantity += oi.Quantity;
						}
					}

					foreach (var oi in originalCustomer.OrderItems)
					{
						OriginalOrderItemQuantity += oi.Quantity;
					}

					//We need to only apply shipping values only as a percentage of items being returned to items on the original order
					//this way we spread the shipping tax that was charged on the original order
					//evenly across all items on the order, and only return an aven portion per item.
					//This is required to correctly calculate return amounts anytime orders are not returned all at once - Scott Wilson
					orderCustomer.TaxableTotal +=
						NetSteps.Common.Extensions.DecimalExtensions.GetRoundedNumber(
						(originalCustomer.ShippingTotal.Value * (OrderItemQuantity / OriginalOrderItemQuantity)), 2);

					orderCustomer.TaxAmountShipping +=
						NetSteps.Common.Extensions.DecimalExtensions.GetRoundedNumber(
						(originalCustomer.TaxAmountShipping.Value * (OrderItemQuantity / OriginalOrderItemQuantity)), 2);
				}

				orderCustomer.TaxAmountTotal = orderCustomer.TaxAmountOrderItems + orderCustomer.TaxAmountShipping;

				// Failsafe: if tax total is > original total, total becomes original total
				if (IsValidCustomer(originalCustomer) && orderCustomer.TaxAmountTotal > originalCustomer.TaxAmountTotal)
					orderCustomer.TaxAmountTotal = originalCustomer.TaxAmountTotal;

				if (IsValidCustomer(originalCustomer) && orderCustomer.Subtotal > originalCustomer.Subtotal)
					orderCustomer.Subtotal = originalCustomer.Subtotal;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		private bool IsValidCustomer(OrderCustomer customer)
		{
			return customer != null;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="order"></param>
		/// <param name="orderCustomer"></param>
		/// <param name="orderItem"></param>
		/// <param name="productId"></param>
		/// <returns>Returns a Unit Price</returns>
		public virtual decimal GetTaxablePriceForOrderItem(Order order, OrderCustomer orderCustomer, OrderItem orderItem, int productId)
		{
			try
			{
				// AdjustedPrice should be set previous to this call with a correctly calculated value. (taking discounts/overrides into consideration) - JHE
				// This means that the tax total for an order will be taxed on the discounted amount and not the full amount. - JHE
				int calculationType = 0;

				if (orderItem.HostessRewardRuleID.HasValue)
				{
					string stateAbr = orderCustomer.OrderShipments.Count > 0 ? orderCustomer.OrderShipments[0].State : order.GetDefaultShipment().State;

					// find out what type of treatment should be used to determine how much tax should be charged.
					switch (HostessRewardRule.Load(orderItem.HostessRewardRuleID.Value).HostessRewardTypeID)
					{
						case (int)ConstantsGenerated.HostessRewardType.HostCredit:
							calculationType = GetHostessCreditCalculationType(stateAbr, calculationType);
							break;
						case (int)ConstantsGenerated.HostessRewardType.PercentOff:
						case (int)ConstantsGenerated.HostessRewardType.ItemDiscount:
							calculationType = GetDiscountedItemCalculationType(stateAbr, calculationType);
							break;
						default:
							calculationType = GetCustomHostessRuleCalculationType();
							break;
					}
				}
				else if (orderItem.OrderAdjustmentOrderLineModifications
					.Any(lm => lm.ModificationOperationID == (int)NetSteps.OrderAdjustments.Common.Model.OrderAdjustmentOrderLineOperationKind.AddedItem))
				{
					//Do not charge tax on Promotionally added (free) items.
					return 0;
				}

				// return the price to charge tax on
				switch (calculationType)
				{
					case 0:
						// Pay on the original taxable price type price
						return this.GetOriginalTaxablePriceTypeValue(order, orderCustomer, orderItem);
					case 1:
						// pay tax on the discounted price -- Cost
						return (orderItem.GetAdjustedPrice()).GetRoundedNumber();
					case 2:
					case 3:
						// pay tax on the retail price -- MSRP
						if (orderItem.ItemPrice > 0)
							return orderItem.ItemPrice;
						throw new Exception("Error calculating tax for order. ItemPrice not set on OrderItem.");
					default:
						// charge no tax
						return 0;
				}
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public virtual decimal GetTaxablePriceForReturnOrderItem(OrderItem returnItem, OrderItem originalItem)
		{
			var originalCustomer = originalItem.OrderCustomer;
			var originalOrder = originalCustomer.Order;
			return this.GetTaxablePriceForOrderItem(originalOrder, originalCustomer, originalItem, originalItem.ProductID ?? 0);
		}

		/// <summary>
		/// Get the taxable price type's original price
		/// </summary>
		/// <param name="order">An Order</param>
		/// <param name="orderCustomer">An OrderCustomer</param>
		/// <param name="orderItem">The OrderItem you want priced</param>
		/// <returns>The original taxable price</returns>
		protected virtual decimal GetOriginalTaxablePriceTypeValue(Order order, OrderCustomer orderCustomer, OrderItem orderItem)
		{
			var priceTypeService = Create.New<IPriceTypeService>();
			var taxablePriceType = priceTypeService.GetPriceType(
				orderCustomer.AccountTypeID,
				(int)ConstantsGenerated.PriceRelationshipType.Taxes,
				ApplicationContext.Instance.StoreFrontID,
				order.OrderTypeID).PriceTypeID;

			return GetOriginalPriceForPriceType(order, orderItem, taxablePriceType);
		}

		/// <summary>
		/// Get the original price for the price type
		/// </summary>
		/// <param name="order"></param>
		/// <param name="orderItem"></param>
		/// <param name="priceTypeID"></param>
		/// <returns></returns>
		private static decimal GetOriginalPriceForPriceType(Order order, OrderItem orderItem, int priceTypeID)
		{
			// First, try to get the original price from the item itself
			var originalPrice = orderItem.OrderItemPrices.FirstOrDefault(oip => oip.ProductPriceTypeID == priceTypeID);
			if (originalPrice != null && originalPrice.OriginalUnitPrice.HasValue)
				return originalPrice.OriginalUnitPrice.Value;

			// If the item doesn't have that price added to it, get it through the product.
			var inventory = Create.New<InventoryBaseRepository>();
			var productPricingService = Create.New<IProductPricingService>();

			var product = inventory.GetProduct(orderItem.ProductID.Value);
			var originalProductPrice = productPricingService.GetPrice(product, priceTypeID, order.CurrencyID);
			return originalProductPrice;
		}

		public virtual int GetCustomHostessRuleCalculationType()
		{
			int calculationType = 0;
			return calculationType;
		}

		private static int GetDiscountedItemCalculationType(string stateAbr, int calculationType)
		{
			if (!HalfPriceItemTreatment.ContainsKey(stateAbr.ToUpper()))
			{
				return 0;
			}

			switch (HalfPriceItemTreatment[stateAbr.ToUpper()])
			{
				case HalfPriceItemTaxTreatment.NetSellingPrice:
					calculationType = 1;
					break;
				case HalfPriceItemTaxTreatment.GrossBeforeCredit:
					calculationType = 3;
					break;
				case HalfPriceItemTaxTreatment.NoTax:
					calculationType = 4;
					break;
				default:
					calculationType = 0;
					break;
			}
			return calculationType;
		}

		private static int GetHostessCreditCalculationType(string stateAbr, int calculationType)
		{
			if (!HostessCreditItemTreatment.ContainsKey(stateAbr.ToUpper()))
			{
				return 0;
			}

			switch (HostessCreditItemTreatment[stateAbr.ToUpper()])
			{
				case HostessCreditItemTaxTreatment.Cost:
					calculationType = 1;
					break;
				case HostessCreditItemTaxTreatment.SuggestedRetail:
					calculationType = 2;
					break;
				case HostessCreditItemTaxTreatment.NoTax:
					calculationType = 4;
					break;
				default:
					calculationType = 0;
					break;
			}
			return calculationType;
		}

		public virtual bool IsItemTaxable(bool defaultChargeTax, OrderItem orderItem, string stateAbbr)
		{
			try
			{
				var inventory = Create.New<InventoryBaseRepository>();

				var product = inventory.GetProduct(orderItem.ProductID.ToInt());
				return product.ProductBase.ChargeTax;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}
		#endregion

		#region Private Methods

		protected virtual void ResetOrderCustomerTotals(OrderCustomer orderCustomer)
		{
			try
			{
				// Reset Customer's tax percentages
				orderCustomer.TaxPercent = 0;
				orderCustomer.TaxPercentCity = 0;
				orderCustomer.TaxPercentCounty = 0;
				orderCustomer.TaxPercentDistrict = 0;
				orderCustomer.TaxPercentState = 0;
				orderCustomer.TaxAmountTotal = 0;
				orderCustomer.TaxAmountCity = 0;
				orderCustomer.TaxAmountCounty = 0;
				orderCustomer.TaxAmountDistrict = 0;
				orderCustomer.TaxAmountState = 0;

				// Reset Other Taxes
				orderCustomer.TaxableTotal = 0;
				orderCustomer.TaxAmountOrderItems = 0;
				orderCustomer.TaxAmountShipping = 0;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		protected virtual void CalculateKitTaxableTotal(Order order, OrderCustomer orderCustomer, OrderItem orderItem)
		{
			try
			{
				decimal taxablePrice = 0;

				// This code works, but we are not ready to calculate tax based on child items. In most cases,
				// tax should be calculated on the parent item's price - not on the child items (see code below).
				// Before we enable this, we need to set up the logic to determine which circumstances will use it. - Lundy
				//
				//if (orderItem.ChildOrderItems.Count > 0)
				//{
				//    foreach (var childOrderItem in orderItem.ChildOrderItems)
				//    {
				//        if (childOrderItem.ChildOrderItems != null && childOrderItem.ChildOrderItems.Count > 0)
				//            CalculateKitTaxableTotal(order, orderCustomer, childOrderItem);
				//        else
				//            childOrderItem.Taxes.TaxableTotal = GetTaxablePriceForOrderItem(order, orderCustomer, childOrderItem, childOrderItem.ProductID.ToInt()) * childOrderItem.Quantity;
				//        taxablePrice += childOrderItem.Taxes.TaxableTotal;
				//    }
				//}
				//orderItem.Taxes.TaxableTotal = taxablePrice;

				// Just calculate tax on the parent item (this item)
				taxablePrice = GetTaxablePriceForOrderItem(order, orderCustomer, orderItem, orderItem.ProductID.ToInt());
				orderItem.Taxes.TaxableTotal = taxablePrice * orderItem.Quantity;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		protected virtual void CalculateKitItemSalesTax(Order order, OrderCustomer orderCustomer, OrderItem orderItem, List<TaxCache> taxRates, ref decimal shippingTaxRate, ref bool chargeTaxOnShipping)
		{
			try
			{
				var inventory = Create.New<InventoryBaseRepository>();

				var product = inventory.GetProduct(orderItem.ProductID.ToInt());
				if (product.ProductBase.ChargeTaxOnShipping)
					chargeTaxOnShipping = true;

				orderItem.Taxes.ResetTotals();

				decimal taxablePrice = GetTaxablePriceForOrderItem(order, orderCustomer, orderItem, orderItem.ProductID.ToInt());
				//var categoryTaxRates = GetTaxRecordForTaxCategory(childOrderItem.Product.TaxCategoryID, taxRates);

				// We May need to change this later if product belongs to multiple categories. - JHE
				// TODO: Change this to work with multiple TaxCategoryIDs from ProductTaxCategories table when necessary - JHE
				int taxCategoryId = product.ProductBase.TaxCategoryID.ToInt();
				var categoryTaxRates = GetTaxRecordForTaxCategory(taxCategoryId, taxRates);


				SetItemTaxes(orderItem, taxRates, ref shippingTaxRate, true);
				if (IsItemTaxable(orderItem.ChargeTax, orderItem, orderCustomer.OrderPayments.Count > 0 ? orderCustomer.OrderPayments[0].BillingState : null))
				{
					orderItem.Taxes.TaxableTotal += orderItem.Taxes.TaxableTotal = NetSteps.Common.Extensions.DecimalExtensions.GetRoundedNumber(taxablePrice * orderItem.Quantity, 2);
					orderItem.Taxes.TaxAmountTotal += orderItem.Taxes.TaxAmountTotal = NetSteps.Common.Extensions.DecimalExtensions.GetRoundedNumber(categoryTaxRates.CombinedSalesTax.ToDecimal() * orderItem.Taxes.TaxableTotal, 2);
					orderItem.Taxes.TaxAmountCity += orderItem.Taxes.TaxAmountCity = NetSteps.Common.Extensions.DecimalExtensions.GetRoundedNumber(categoryTaxRates.CitySalesTax * orderItem.Taxes.TaxableTotal, 2);
					orderItem.Taxes.TaxAmountCityLocal += orderItem.Taxes.TaxAmountCityLocal = NetSteps.Common.Extensions.DecimalExtensions.GetRoundedNumber(categoryTaxRates.CityLocalSales.ToDecimal() * orderItem.Taxes.TaxableTotal, 2);
					orderItem.Taxes.TaxAmountCounty += orderItem.Taxes.TaxAmountCounty = NetSteps.Common.Extensions.DecimalExtensions.GetRoundedNumber(categoryTaxRates.CountySalesTax * orderItem.Taxes.TaxableTotal, 2);
					orderItem.Taxes.TaxAmountCountyLocal += orderItem.Taxes.TaxAmountCountyLocal = NetSteps.Common.Extensions.DecimalExtensions.GetRoundedNumber(categoryTaxRates.CountyLocalSales.ToDecimal() * orderItem.Taxes.TaxableTotal, 2);
					orderItem.Taxes.TaxAmountState += orderItem.Taxes.TaxAmountState = NetSteps.Common.Extensions.DecimalExtensions.GetRoundedNumber(categoryTaxRates.StateSalesTax * orderItem.Taxes.TaxableTotal, 2);
					orderItem.Taxes.TaxAmountDistrict += orderItem.Taxes.TaxAmountDistrict = NetSteps.Common.Extensions.DecimalExtensions.GetRoundedNumber(categoryTaxRates.DistrictSalesTax * orderItem.Taxes.TaxableTotal, 2);
				}
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		protected virtual void SetItemTaxes(OrderItem orderItem, List<TaxCache> taxRates, ref decimal shippingTaxRate, bool isItemTaxable)
		{
			try
			{
				var inventory = Create.New<InventoryBaseRepository>();

				var product = inventory.GetProduct(orderItem.ProductID.ToInt());
				int taxCategoryId = product.ProductBase.TaxCategoryID.ToInt();
				var categoryTaxRates = GetTaxRecordForTaxCategory(taxCategoryId, taxRates);

				if (isItemTaxable)
				{
					orderItem.Taxes.TaxPercent = categoryTaxRates.CombinedSalesTax.ToDecimal();
					orderItem.Taxes.TaxPercentCity = categoryTaxRates.CitySalesTax;
					orderItem.Taxes.TaxPercentCityLocal = categoryTaxRates.CityLocalSales.ToDecimal();
					orderItem.Taxes.TaxPercentCounty = categoryTaxRates.CountySalesTax;
					orderItem.Taxes.TaxPercentCountyLocal = categoryTaxRates.CountyLocalSales.ToDecimal();
					orderItem.Taxes.TaxPercentState = categoryTaxRates.StateSalesTax;
					orderItem.Taxes.TaxPercentDistrict = categoryTaxRates.DistrictSalesTax;
				}

				// Set Shipping Tax Rate at Highest Rate
				if (categoryTaxRates.CombinedSalesTax > shippingTaxRate)
					shippingTaxRate = categoryTaxRates.CombinedSalesTax.ToDecimal();
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		protected virtual TaxCache GetTaxRecordForTaxCategory(int taxCategoryID, List<TaxCache> taxRates)
		{
			try
			{
				if (taxRates.Count == 0)
					throw new Exception("No tax records were found for the requested address.");

				var taxRateResult = taxRates.FirstOrDefault(t => t.TaxCategoryID.ToInt() == taxCategoryID);

				// If we don't find a tax rate for the given category
				if (taxRateResult == null)
				{
					// Then we'll try to find a default tax rate category
					TaxCategory defaultTaxCategory = SmallCollectionCache.Instance.TaxCategories.FirstOrDefault(c => c.IsDefault == "1");

					// If there is a default tax category
					if (defaultTaxCategory != null)
					{
						// Then we'll check to make sure the provided tax category isn't already the default and we'll load the rate for the default category.
						if (defaultTaxCategory.TaxCategoryID != taxCategoryID)
							taxRateResult = GetTaxRecordForTaxCategory(defaultTaxCategory.TaxCategoryID, taxRates);
					}
				}

				// If we don't find a tax rate for either the provided tax category or the default tax category we'll just grab the first in the list.
				if (taxRateResult == null)
					taxRateResult = taxRates[0];

				return taxRateResult;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public virtual void CopyTaxValuesToOrderItem(OrderItem orderItem)
		{
			try
			{
				orderItem.TaxableTotal = orderItem.Taxes.TaxableTotal;
				orderItem.TaxAmount = orderItem.Taxes.TaxAmountTotal;
				orderItem.TaxAmountCity = orderItem.Taxes.TaxAmountCity;
				orderItem.TaxAmountCityLocal = orderItem.Taxes.TaxAmountCityLocal;
				orderItem.TaxAmountCounty = orderItem.Taxes.TaxAmountCounty;
				orderItem.TaxAmountCountyLocal = orderItem.Taxes.TaxAmountCountyLocal;
				orderItem.TaxAmountState = orderItem.Taxes.TaxAmountState;
				orderItem.TaxAmountDistrict = orderItem.Taxes.TaxAmountDistrict;

				orderItem.TaxPercent = orderItem.Taxes.TaxPercent;
				orderItem.TaxPercentCity = orderItem.Taxes.TaxPercentCity;
				orderItem.TaxPercentCounty = orderItem.Taxes.TaxPercentCounty;
				orderItem.TaxPercentDistrict = orderItem.Taxes.TaxPercentDistrict;
				orderItem.TaxPercentState = orderItem.Taxes.TaxPercentState;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public virtual void CopyTaxValuesFromOrderItem(OrderItem orderItem)
		{
			try
			{
				orderItem.Taxes.TaxableTotal = orderItem.TaxableTotal.ToDecimal();
				orderItem.Taxes.TaxAmountTotal = orderItem.TaxAmount.ToDecimal();
				orderItem.Taxes.TaxAmountCity = orderItem.TaxAmountCity.ToDecimal();
				orderItem.Taxes.TaxAmountCityLocal = orderItem.TaxAmountCityLocal.ToDecimal();
				orderItem.Taxes.TaxAmountCounty = orderItem.TaxAmountCounty.ToDecimal();
				orderItem.Taxes.TaxAmountCountyLocal = orderItem.TaxAmountCountyLocal.ToDecimal();
				orderItem.Taxes.TaxAmountState = orderItem.TaxAmountState.ToDecimal();
				orderItem.Taxes.TaxAmountDistrict = orderItem.TaxAmountDistrict.ToDecimal();

				orderItem.Taxes.TaxPercent = orderItem.TaxPercent.ToDecimal();
				orderItem.Taxes.TaxPercentCity = orderItem.TaxPercentCity.ToDecimal();
				orderItem.Taxes.TaxPercentCounty = orderItem.TaxPercentCounty.ToDecimal();
				orderItem.Taxes.TaxPercentDistrict = orderItem.TaxPercentDistrict.ToDecimal();
				orderItem.Taxes.TaxPercentState = orderItem.TaxPercentState.ToDecimal();
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}
		#endregion

		#region Get Tax Rates Methods

		class TaxCacheResolver : DemuxCacheItemResolver<ITaxCacheKey, List<TaxCache>>
		{
			protected override bool DemultiplexedTryResolve(ITaxCacheKey key, out List<TaxCache> value)
			{
				if (key.IsDetailedKey)
				{
					value = TaxCache.LoadByAddress(key.CountryID, key.StateAbbr, key.County, key.City, key.PostalCode);
					var overridenTaxInfo = TaxCache.CheckForOverrides(value);

				}
				else if (key.UseProvince)
				{
					//This is to take care of Canada
					value = TaxCache.LoadByProvince(key.CountryID, key.StateAbbr);
				}
				else
				{
					value = TaxCache.LoadByAddress(key.CountryID, key.PostalCode);
				}

				return value != null;
			}
		}
		ICache<ITaxCacheKey, List<TaxCache>> _cacheTaxCache = new ActiveMruLocalMemoryCache<ITaxCacheKey, List<TaxCache>>("tax-cache", new TaxCacheResolver());

		public virtual List<TaxCache> GetTaxInfo(OrderShipment orderShipment)
		{
			try
			{
				// Load tax information from the db based on the Ship To address
				var taxRates = LoadByAddress(orderShipment.CountryID, orderShipment.State, orderShipment.County, orderShipment.City, orderShipment.PostalCode);

				// Didn't find any sales tax records -- fall back to just postal code
				if (taxRates.Count == 0)
					taxRates = LoadByAddress(orderShipment.CountryID, orderShipment.PostalCode);

				if (taxRates.Count == 0 && orderShipment.CountryID == Constants.Country.Canada.ToInt())
				{
					taxRates = LoadByProvince(orderShipment.CountryID, orderShipment.State);
				}
				// Still no results, check Simpova
				if (taxRates.Count == 0 && orderShipment.CountryID == Constants.Country.UnitedStates.ToInt())
				{
					// TODO: Test this - JHE
					SimpovaTaxRateProvider simpovaTaxRateProvider = new SimpovaTaxRateProvider();
					var rates = simpovaTaxRateProvider.GetTaxInfo(orderShipment.CountryID, orderShipment.State, orderShipment.County, orderShipment.City, orderShipment.PostalCode);
					if (rates.Count > 0)
					{
						ClearCacheItem(orderShipment.CountryID, orderShipment.PostalCode);
						ClearCacheItem(orderShipment.CountryID, orderShipment.State, orderShipment.County, orderShipment.City, orderShipment.PostalCode);
					}

					// Then try and get the rates again - JHE

					// Load tax information from the db based on the Ship To address
					taxRates = LoadByAddress(orderShipment.CountryID, orderShipment.State, orderShipment.County, orderShipment.City, orderShipment.PostalCode);

					// Didn't find any sales tax records -- fall back to just postal code
					if (taxRates.Count == 0)
						taxRates = LoadByAddress(orderShipment.CountryID, orderShipment.PostalCode);
				}

				if (taxRates.Count == 0)
				{
					string errorMessage = string.Format("No tax records were found for the requested address. State: {0} County: {1} City: {2} PostalCode: {3}", orderShipment.State, orderShipment.County, orderShipment.City, orderShipment.PostalCode);
					throw new Exception(errorMessage);
				}

				return taxRates;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public virtual List<TaxCache> LoadByAddress(int countryId, string postalCode)
		{
			ITaxCacheKey key = MakeCacheKey(false, false, countryId, null, null, null, postalCode);
			return _cacheTaxCache.Get(key);
		}

		public virtual List<TaxCache> LoadByProvince(int countryId, string stateAbbr)
		{
			ITaxCacheKey key = MakeCacheKey(false, true, countryId, stateAbbr, null, null, null);
			return _cacheTaxCache.Get(key);
		}

		[Obsolete("Use LoadByAddress, all calls select through the cache.", true)]
		public virtual List<TaxCache> LoadByAddressCached(int countryId, string postalCode)
		{
			return LoadByAddress(countryId, postalCode);
		}

		public virtual List<TaxCache> LoadByAddress(int countryId, string stateAbbr, string county, string city, string postalCode)
		{
			ITaxCacheKey key = MakeCacheKey(true, false, countryId, stateAbbr, county, city, postalCode);
			return _cacheTaxCache.Get(key);
		}

		[Obsolete("Use LoadByAddress, all calls select through the cache.", true)]
		public virtual List<TaxCache> LoadByAddressCached(int countryId, string stateAbbr, string county, string city, string postalCode)
		{
			return LoadByAddress(countryId, stateAbbr, county, city, postalCode);
		}

		public virtual void ClearCacheItem(int countryId, string postalCode)
		{
			ITaxCacheKey key = MakeCacheKey(false, false, countryId, null, null, null, postalCode);
			List<TaxCache> removed;
			_cacheTaxCache.TryRemove(key, out removed);
		}

		public virtual void ClearCacheItem(int countryId, string stateAbbr, string county, string city, string postalCode)
		{
			ITaxCacheKey key = MakeCacheKey(true, false, countryId, stateAbbr, county, city, postalCode);
			List<TaxCache> removed;
			_cacheTaxCache.TryRemove(key, out removed);
		}

		private ITaxCacheKey MakeCacheKey(bool isDetailed, bool useProvince, int countryId, string stateAbbr, string county, string city, string postalCode)
		{
			var key = Create.New<ITaxCacheKey>();
			key.IsDetailedKey = isDetailed;
			key.UseProvince = useProvince;
			key.CountryID = countryId;
			key.StateAbbr = stateAbbr;
			key.County = county;
			key.PostalCode = postalCode;
			key.City = city;
			return key;
		}

		public virtual void CalculatePartyTax(Order order)
		{
			if (order.OrderTypeID != (short)Constants.OrderType.PartyOrder && order.OrderTypeID != (short)Constants.OrderType.FundraiserOrder)
				return;

			// get shipping total rate and calculate the tax on the shipping if we should be charging tax on shipping.
			OrderShipment shipment = order.OrderShipments.Where(os => os.OrderCustomerID == null).FirstOrDefault();

			if (shipment == null || !order.OrderCustomers.Any(oc => !oc.OrderShipments.Any())) return;

			// find the highest tax rate 
			IEnumerable<OrderItem> orderItems = order.OrderCustomers.Where(oc => !oc.OrderShipments.Any()).SelectMany(oc => oc.OrderItems);

			if (!orderItems.Any())
				return;

			decimal taxPercent = orderItems.Max(oi => oi.Taxes.TaxPercent);

			var taxes = GetTaxInfo(shipment);
			bool chargeTaxOnShipping = taxes.Any(t => t.ChargeTaxOnShipping == true);
			if (chargeTaxOnShipping)
			{
				order.PartyShippingTax = (order.PartyShipmentTotal * taxPercent).GetRoundedNumber();
			}
		}
		#endregion
	}
}
