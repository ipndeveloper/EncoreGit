using System;
using System.Collections.Generic;
using System.Linq;
using Avalara.AvaTax.Adapter.TaxService;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.AvataxAPI;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Generated;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Data.Entities.Tax.Avatax
{
	public class AvataxCalculator : BaseTaxService, ITaxService
	{
		private IAvataxAdapter _avataxAdapter;
		public IAvataxAdapter AvataxAdapterInstance
		{
			get { return _avataxAdapter ?? (_avataxAdapter = Create.New<IAvataxAdapter>()); }
		}

		public InventoryBaseRepository Inventory { get { return Create.New<InventoryBaseRepository>(); } }

		public override void CalculateTax(OrderCustomer orderCustomer)
		{
			var order = orderCustomer.Order;

			ValidateTax(order, orderCustomer);

			try
			{
				orderCustomer.TaxableTotal = 0;
				orderCustomer.TaxAmountTotal = 0;

				foreach (OrderItem orderItem in orderCustomer.ParentOrderItems)
				{
					decimal taxablePrice = GetTaxablePriceForOrderItem(order, orderCustomer, orderItem, orderItem.ProductID.ToInt());
					orderItem.Taxes.TaxableTotal = taxablePrice * orderItem.Quantity;
				}

				//Call Avatax if order contains Items
				AvataxCalculationInfo avataxCalculationInfo = new AvataxCalculationInfo();
				if (orderCustomer.ParentOrderItems.Count > 0)
				{
					avataxCalculationInfo = CallAvataxTaxProviderAPI(orderCustomer, false); //GetQuote : Get tax amounts for display
				}

				ResetOrderCustomerTotals(orderCustomer);

				foreach (var orderItem in orderCustomer.ParentOrderItems)
				{
					var product = Inventory.GetProduct(orderItem.ProductID.ToInt());

					if (!product.ProductBase.ChargeTax)
					{
						SetDefaultTaxValues(orderItem);
						continue;
					}
					orderItem.Taxes.TaxAmountCity = orderItem.Taxes.TaxableTotal * orderItem.Taxes.TaxPercentCity;
					orderItem.Taxes.TaxAmountCityLocal = orderItem.Taxes.TaxableTotal * orderItem.Taxes.TaxPercentCityLocal;
					orderItem.Taxes.TaxAmountCounty = orderItem.Taxes.TaxableTotal * orderItem.Taxes.TaxPercentCounty;
					orderItem.Taxes.TaxAmountCountyLocal = orderItem.Taxes.TaxableTotal * orderItem.Taxes.TaxPercentCountyLocal;
					orderItem.Taxes.TaxAmountState = orderItem.Taxes.TaxableTotal * orderItem.Taxes.TaxPercentState;
					orderItem.Taxes.TaxAmountDistrict = orderItem.Taxes.TaxableTotal * orderItem.Taxes.TaxPercentDistrict;
					orderItem.Taxes.TaxAmountCountry = orderItem.Taxes.TaxableTotal * orderItem.TaxPercentCountry.ToDecimal();

					CopyTaxValuesToOrderItem(orderItem);

					// Update the customer's totals
					orderCustomer.TaxableTotal += orderItem.Taxes.TaxableTotal;
					orderCustomer.TaxAmountOrderItems += orderItem.Taxes.TaxAmountTotal;
				}

				// Avatax does the Shipping tax calculation. Just adding the tax should be fine.
				orderCustomer.TaxableTotal += avataxCalculationInfo.ShippingTaxable + avataxCalculationInfo.HandlingTaxable;
				orderCustomer.TaxAmountShipping += avataxCalculationInfo.ShippingTax + avataxCalculationInfo.HandlingTax;


				orderCustomer.TaxAmountTotal = orderCustomer.TaxAmountOrderItems.ToDecimal() + orderCustomer.TaxAmountShipping.ToDecimal();
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException, orderCustomer.OrderID, orderCustomer.AccountID, GetAvataxErrorMessage());
			}
		}

		void SetDefaultTaxValues(OrderItem item)
		{
			item.TaxableTotal = 0;
			item.TaxAmount = 0;
			item.TaxAmountCity = 0;
			item.TaxAmountCityLocal = 0;
			item.TaxAmountCounty = 0;
			item.TaxAmountCountyLocal = 0;
			item.TaxAmountState = 0;
			item.TaxAmountDistrict = 0;
			item.TaxAmountCountry = 0;
			item.TaxPercent = 0;
			item.TaxPercentCity = 0;
			item.TaxPercentCounty = 0;
			item.TaxPercentDistrict = 0;
			item.TaxPercentState = 0;
		}

		protected string GetAvataxErrorMessage()
		{
			return
				 String.Format(
					  "An error occured while processing your order, and is unable to continue at this time.  Please try completing your order again at a later time.");
		}

		public override void CopyTaxValuesToOrderItem(OrderItem orderItem)
		{
			try
			{
				CopyValues(orderItem);

			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException, null, null, GetAvataxErrorMessage());
			}
		}

		private void CopyValues(OrderItem orderItem)
		{
			orderItem.TaxableTotal = orderItem.Taxes.TaxableTotal;
			orderItem.TaxAmount = orderItem.Taxes.TaxAmountTotal;
			orderItem.TaxAmountCity = orderItem.Taxes.TaxAmountCity;
			orderItem.TaxAmountCityLocal = orderItem.Taxes.TaxAmountCityLocal;
			orderItem.TaxAmountCounty = orderItem.Taxes.TaxAmountCounty;
			orderItem.TaxAmountCountyLocal = orderItem.Taxes.TaxAmountCountyLocal;
			orderItem.TaxAmountState = orderItem.Taxes.TaxAmountState;
			orderItem.TaxAmountDistrict = orderItem.Taxes.TaxAmountDistrict;
			//orderItem. orderItem.Taxes.TaxAmountShipping = taxAmountShipping;

			orderItem.TaxPercent = orderItem.Taxes.TaxPercent;
			orderItem.TaxPercentCity = orderItem.Taxes.TaxPercentCity;
			orderItem.TaxPercentCounty = orderItem.Taxes.TaxPercentCounty;
			orderItem.TaxPercentDistrict = orderItem.Taxes.TaxPercentDistrict;
			orderItem.TaxPercentState = orderItem.Taxes.TaxPercentState;
		}

		//Save Invoice in Avatax: Finalize
		public override void FinalizeTax(OrderCustomer orderCustomer)
		{
			if (orderCustomer.OrderItems.Count > 0)
			{
				//Call Avatax to save Invoice - it commits the transaction
				CallAvataxTaxProviderAPI(orderCustomer, true);
			}

			//CallPostTax is required if SaveInvoice does not commit the transactions (parameter commit = false)
			//CallPostTax(orderCustomer);
		}

		public override void CancelTax(OrderCustomer orderCustomer)
		{
			try
			{
				//Retrieve Parent Order - to fetch shipping cost/tax
				//Load() only does not return returnOrderItems - hence using LoadFull()
				Order parentOrder = Order.LoadFull(orderCustomer.Order.ParentOrderID.ToInt());

				foreach (var orderItem in orderCustomer.OrderItems)
				{
					var orderItemReturn = orderItem.OrderItemReturns.FirstOrDefault();
					if (orderItemReturn != null)
					{
						var originalItem =
							parentOrder.OrderCustomers.SelectMany(oc => oc.OrderItems)
								.FirstOrDefault(oi => oi.OrderItemID == orderItemReturn.OriginalOrderItemID);

						// Set the Taxes.TaxableTotal on the returned item to the UNIT price here.
						// This makes things easier in the AvataxAdapter.
                        decimal? itemPrice = orderItem.ItemPrice;
						orderItem.Taxes.TaxableTotal = originalItem != null
							? this.GetTaxablePriceForReturnOrderItem(orderItem, originalItem)
							: itemPrice ?? 0m;
					}
				}

				//Call Avatax for Return Order
				GetTaxResult getTaxResult = AvataxAdapterInstance.ReturnInvoice(orderCustomer.Order, parentOrder, orderCustomer, GetTaxRequestValues(parentOrder.OrderCustomers[0]), true);

				if (!AvataxAdapterInstance.IsGetTaxRequestSuccess(getTaxResult))
				{
					throw EntityExceptionHelper.GetAndLogNetStepsException(AvataxAdapterInstance.GetTaxResultCode(getTaxResult) + " : " + AvataxAdapterInstance.GetTaxResultMessage(getTaxResult), Constants.NetStepsExceptionType.NetStepsBusinessException);
				}

				AvataxAdapterInstance.Dispose();
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public virtual AvataxCalculationInfo CallAvataxTaxProviderAPI(OrderCustomer orderCustomer, bool saveInvoice)
		{
			// Summary = 0, Document = 1, Line = 2, Tax = 3, Diagnostic = 4       
			GetTaxResult getTaxResult = null;
			AvataxCalculationInfo avataxCalculationInfo = new AvataxCalculationInfo();
			if (saveInvoice)
			{
				getTaxResult = AvataxAdapterInstance.SaveInvoice(orderCustomer, (int)NetSteps.Data.Entities.AvataxAPI.Constants.DetailLevel.Tax, true, true, GetTaxRequestValues(orderCustomer), GetLineItemColumnValuesBySKU(orderCustomer));
			}
			else
			{
				getTaxResult = AvataxAdapterInstance.GetQuote(orderCustomer, (int)AvataxAPI.Constants.DetailLevel.Tax, true, GetTaxRequestValues(orderCustomer), GetLineItemColumnValuesBySKU(orderCustomer), avataxCalculationInfo);
				if (getTaxResult == null)
				{
					return new AvataxCalculationInfo();
				}
			}

			//If result code != Success throw the Error code + error/warning message(s) to UI
			if (!AvataxAdapterInstance.IsGetTaxRequestSuccess(getTaxResult))
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(AvataxAdapterInstance.GetTaxResultCode(getTaxResult) + " : " + AvataxAdapterInstance.GetTaxResultMessage(getTaxResult), Constants.NetStepsExceptionType.NetStepsBusinessException);
			}

			//set the values before the GetTaxResult gets re-initialized
			avataxCalculationInfo.TotalAmount = getTaxResult.TotalAmount;
			avataxCalculationInfo.TotalTaxableAmount = getTaxResult.TotalTaxable;
			avataxCalculationInfo.TotalTax = getTaxResult.TotalTax;

			AvataxAdapterInstance.Dispose();
			return avataxCalculationInfo;
		}

		public override void CalculatePartyTax(Order order)
		{
			try
			{
				if (order.OrderTypeID != ConstantsGenerated.OrderType.PartyOrder.ToShort())
					return;

				// get shipping total rate and calculate the tax on the shipping if we should be charging tax on shipping.
				var shipment = order.OrderShipments.FirstOrDefault(os => os.OrderCustomerID == null);

				if (shipment == null || !order.OrderCustomers.Any(oc => oc.OrderShipments.Count == 0)) return;

				// find the highest tax rate 
				var customers = order.OrderCustomers.Where(oc => oc.OrderShipments.Count == 0).ToList();
				var orderItems = customers.SelectMany(oc => oc.OrderItems).ToList();

				if (orderItems.Count == 0)
					return;

				//Avatax call //pass OrderID = 0
				AvataxCalculationInfo avataxCalculationInfo = new AvataxCalculationInfo();
				GetTaxResult getTaxResult = AvataxAdapterInstance.GetQuote(customers.FirstOrDefault(),
															(int)NetSteps.Data.Entities.AvataxAPI.Constants.DetailLevel.Tax, true,
															new Dictionary<string, string>(),
															new Dictionary<string, Dictionary<string, string>>(), avataxCalculationInfo);

				// Avatax returns the shipping tax amount.
				//var taxPercent = orderItems.Max(oi => oi.Taxes.TaxPercent);

				order.PartyShippingTax = avataxCalculationInfo.ShippingTax;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException, null, null, GetAvataxErrorMessage());
			}
		}

		protected override void ValidateTax(Order order, OrderCustomer orderCustomer)
		{
			if (orderCustomer.IsTaxExempt.HasValue && order.IsCommissionable())
				return;

			orderCustomer.IsTaxExempt = orderCustomer.AccountInfo == null ? false : orderCustomer.AccountInfo.IsTaxExempt;
		}

		private void CallPostTax(OrderCustomer orderCustomer, AvataxCalculationInfo avataxCalculationInfo)
		{
			PostTaxResult postTaxResult = AvataxAdapterInstance.PostTax(orderCustomer.OrderID.ToString(), avataxCalculationInfo.TotalAmount, avataxCalculationInfo.TotalTax, orderCustomer.OrderID.ToString(), true);

			if (!AvataxAdapterInstance.IsPostTaxRequestSuccess(postTaxResult))
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(AvataxAdapterInstance.PostTaxResultCode(postTaxResult) + " : " + AvataxAdapterInstance.PostTaxResultMessage(postTaxResult), Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
			else
			{
				CallCommitTax(orderCustomer);
			}
		}

		private void CallCommitTax(OrderCustomer orderCustomer)
		{
			CommitTaxResult commitTaxResult = AvataxAdapterInstance.CommitTax(orderCustomer.OrderID.ToString(), orderCustomer.OrderID.ToString());
			if (!AvataxAdapterInstance.IsCommitTaxRequestSuccess(commitTaxResult))
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(AvataxAdapterInstance.CommitTaxResultCode(commitTaxResult) + " : " + AvataxAdapterInstance.CommitTaxResultMessage(commitTaxResult), Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="orderCustomer"></param>
		/// <returns></returns>
		protected Dictionary<string, string> GetTaxRequestValues(OrderCustomer orderCustomer)
		{
			Dictionary<string, string> returnValue = new Dictionary<string, string>();
			if (orderCustomer != null && orderCustomer.IsTaxExempt.ToBool())
			{
				string entityUseCode = GetEntityUseCode(orderCustomer.AccountID);
				if (!string.IsNullOrEmpty(entityUseCode))
				{
					returnValue.Add(NetSteps.Data.Entities.AvataxAPI.Constants.TaxRequestColumns.CustomerUsageType.ToString(), entityUseCode);
				}
			}

			return returnValue;
		}

		private Dictionary<string, Dictionary<string, string>> GetLineItemColumnValuesBySKU(OrderCustomer orderCustomer)
		{
			//var inventory = Create.New<InventoryBaseRepository>();
			Dictionary<string, Dictionary<string, string>> returnValue = new Dictionary<string, Dictionary<string, string>>();
			//Dictionary<string, string> lineItemColumnValues = new Dictionary<string, string>();

			return returnValue;
		}
		/// <summary>
		/// This needs to be passed for users who are marked as tax exempt.
		/// </summary>
		/// <param name="accountID"></param>
		/// <returns></returns>
		private string GetEntityUseCode(int accountID)
		{
			string returnValue = string.Empty;
			AccountPropertyValue accountPropertyValue = null;

			var accountPropType = SmallCollectionCache.Instance.AccountPropertyTypes.FirstOrDefault(pt => pt.TermName == NetSteps.Data.Entities.AvataxAPI.Constants.AVATAX_ACCOUNTPROPERTY_TYPE_NAME);
			if (accountPropType != null)
			{
				var account = Account.LoadAccountProperties(accountID);
				var accountProperty = account.AccountProperties.FirstOrDefault(ap => ap.AccountPropertyTypeID == accountPropType.AccountPropertyTypeID);
				if (accountProperty != null)
				{
					int accountPropertyValueID = accountProperty.AccountPropertyValueID.Value;
					accountPropertyValue = AccountPropertyValue.Load(accountPropertyValueID);
				}

				returnValue = accountPropertyValue == null ? string.Empty : accountPropertyValue.Value; //default value empty i.e. no exemption
			}
			return returnValue;
		}
	}
}
