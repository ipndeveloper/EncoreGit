using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NetSteps.Addresses.Common.Models;
using NetSteps.Common.Configuration;
using NetSteps.Common.Exceptions;
using NetSteps.Common.Extensions;
using NetSteps.Common.Reflection;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.STOWebServices;
using NetSteps.Data.Entities.Tax.SalesTaxOfficeIntegration.Codes;

namespace NetSteps.Data.Entities.Tax.SalesTaxOfficeIntegration
{
	static internal class DataTypes
	{
		//public const string BaseTypes = "01";
		public const string Exemptions = "02";
		//public const string TaxTypes = "03";
		//public const string CustomerUsageType = "04";
		//public const string LiabilityTypes = "05";
		//public const string Provider = "06";
		//public const string TaxAuthorities = "07";
		//public const string TaxCategories = "08";
		//public const string TransactionTypes = "09";
		//public const string AuthorityTypes = "10";
		//public const string PassFlag = "11";
		//public const string PassType = "12";
		//public const string FreightOnBoard = "13";
		//public const string MailOrder = "14";
		//public const string DeliveryBy = "15";
		//public const string SolicitedOutside = "16";
		//public const string CanRejectDelivery = "17";
		//public const string ShipFromPOB = "18";
		//public const string ProductGroups = "19";
		//public const string GroupItems = "20";
		//public const string Entities = "21";
		//public const string Divisions = "22";
		//public const string Countries = "23";
		//public const string States = "24";
		public const string HalfPrice_UseTax_IDSuffix = "%";
		public const string HostessCredit_UseTax_IDSuffix = "hc";
		public const string DirectShipID = "Shipping";
		public const string DirectHandlingID = "Handling";
		public const string PartyShipID = "PartyShipping";
		public const int MaxPostalCodeLengthCA = 7;
		public const int MaxPostalCodeLengthUS = 5;
	}

	/// <summary>
	/// Usage:
	///     In order to get the tax amount for an order, call CalculateTax() for each orderCustomer.
	///     This puts the order into Pending status on the CCH system.
	///     FinalizeTax() must be called for the order before CCH will report the taxes.
	/// </summary>
	[Serializable]
	public class SalesTaxOfficeCalculator : BaseTaxService, ITaxService
	{
		protected readonly string divisionId = ConfigurationManager.AppSettings["StoDivisionID"];

		protected readonly string entityId = ConfigurationManager.AppSettings["StoEntityID"];

		public SalesTaxOfficeCalculator()
		{

		}

		///--------------------------------------------------------------------
		/// <summary>
		/// This will calculate the tax for the order customer.
		/// </summary>
		/// <exception cref="TaxException"/>
		///--------------------------------------------------------------------
		public override void CalculateTax(OrderCustomer orderCustomer)
		{
			CalculateTax(orderCustomer.Order, orderCustomer);
		}

		public virtual void CalculateTax(Order order, OrderCustomer orderCustomer)
		{
			orderCustomer.ResetOrderCustomerTaxes();

			var shipment = orderCustomer.Order.OrderShipments.FirstOrDefault(x => x.OrderCustomer == orderCustomer) ?? orderCustomer.Order.GetDefaultShipment();
			if (shipment == null || String.IsNullOrEmpty(shipment.PostalCode))
			{
				return;
			}

			Address defaultShippingAddress = null;
			OrderShipment orderShipment = orderCustomer.Order.OrderShipments.FirstOrDefault(x => x.OrderCustomer == orderCustomer) ?? orderCustomer.Order.GetDefaultShipment();
			if (orderShipment == null)
			{
				defaultShippingAddress = orderCustomer.Order.Consultant.Addresses.FirstOrDefault(x => x.AddressTypeID == (short)Constants.AddressType.Shipping);
				if (defaultShippingAddress != null)
				{
					orderShipment = new OrderShipment();
					Reflection.CopyPropertiesDynamic<IAddress, IAddress>(defaultShippingAddress, orderShipment);
				}
			}
			else
			{
				defaultShippingAddress = CopyAddressFromOrderShipment(orderShipment);
			}
			Warehouse warehouse = Warehouse.FindNearestByAddress(defaultShippingAddress);

			STOWebServices.Order salesOrder = ConvertOrderCustomerToOrder(orderCustomer, TransactionType.SalesTax, warehouse);

			if (salesOrder == null || !salesOrder.LineItems.Any())
			{
				return;
			}

			STOWebServices.Order purchaseOrder = ConvertOrderCustomerToOrder(orderCustomer, TransactionType.UseTax, warehouse);

			// Sales tax computation.
			//
			TaxResponse salesTaxResponse = null;

			if (salesOrder.LineItems.Any())
			{
				// Invoke the request to STO, then call the extenstion to
				// obtain the transaction status information.
				//
				salesTaxResponse = STOCalculateRequest(salesOrder);

				TransactionStatus transactionStatus = salesTaxResponse.ToTransactionStatus();

				if (transactionStatus == TransactionStatus.FailureNoTransactionId)
				{
					// Reset transaction ID and try again.
					orderCustomer.SalesTaxTransactionNumber = "0";
					CalculateTax(orderCustomer);

					return;
				}
				else if (transactionStatus == TransactionStatus.FailureInProgressUpdateable)
				{
					if (salesTaxResponse.Messages.Any(x => x.Code == 6012))
					{
						STOFullReturnRequest(Convert.ToUInt32(orderCustomer.SalesTaxTransactionNumber));
					}

					// Reset transaction ID and try again.
					orderCustomer.SalesTaxTransactionNumber = "0";
					CalculateTax(orderCustomer);

					return;
				}
				else if (transactionStatus == TransactionStatus.Failure)
				{
					// This will throw the TaxException which should be
					// picked up by the calling chain to appropriately
					// handle the tax issue.
					throw new TaxException(GetErrorMessage(salesTaxResponse.Messages));
				}

				// To get to this point is to have all the sales-tax processing
				// succeed.
				orderCustomer.SalesTaxTransactionNumber = salesTaxResponse.TransactionID.ToString();
				orderCustomer.TaxGeocode = SalesTaxOfficeGeoCode.LookupGeoCode(salesOrder.NexusInfo.ShipToAddress);
			}

			// Use tax computation.
			TaxResponse purchaseTaxResponse = null;

			if (purchaseOrder.LineItems.Any())
			{
				purchaseTaxResponse = STOCalculateRequest(purchaseOrder);

				TransactionStatus transactionStatus = purchaseTaxResponse.ToTransactionStatus();

				if (transactionStatus == TransactionStatus.FailureNoTransactionId)
				{
					// Reset transaction ID and try again.
					orderCustomer.UseTaxTransactionNumber = "0";
					CalculateTax(orderCustomer);

					return;
				}
				else if (transactionStatus == TransactionStatus.FailureInProgressUpdateable)
				{
					if (purchaseTaxResponse.Messages.Any(x => x.Code == 6012))
					{
						STOFullReturnRequest(Convert.ToUInt32(orderCustomer.UseTaxTransactionNumber));
					}

					// Reset transaction ID and try again.
					orderCustomer.UseTaxTransactionNumber = "0";
					CalculateTax(orderCustomer);

					return;
				}
				else if (transactionStatus == TransactionStatus.Failure)
				{
					throw new TaxException(GetErrorMessage(purchaseTaxResponse.Messages));
				}

				// To get to this point is to have all the use-tax procesing
				// succeed.
				orderCustomer.UseTaxTransactionNumber = purchaseTaxResponse.TransactionID.ToString();
			}

			SetOrderCustomerTaxesFromTaxResponses(orderCustomer, new List<TaxResponse> { salesTaxResponse, purchaseTaxResponse });
		}

		public override void FinalizePartialReturnTax(OrderCustomer orderCustomer)
		{
			PartialReturnOrder partialReturnOrder = ConvertOrderCustomerToPartialReturnOrder(orderCustomer);
			TaxResponse refundTaxResponse = null;
			refundTaxResponse = STOPartialReturnRequest(partialReturnOrder);

			TransactionStatus transactionStatus = refundTaxResponse.ToTransactionStatus();
		}

		///--------------------------------------------------------------------
		/// <summary>
		/// This will call the STO services to calculate the tax on the
		/// order.
		/// </summary>
		///--------------------------------------------------------------------
		protected TaxResponse STOCalculateRequest(STOWebServices.Order order)
		{
			TaxResponse taxResponse;
			STOServiceBindingClient service = new STOServiceBindingClient();

			try
			{
				taxResponse = service.CalculateRequest(entityId, divisionId, order);
			}
			catch (TimeoutException)
			{
				// Try again, the server might have been idle, and the app pool shut down
				//   NOTE: Will remove this sleep once a retry-method is written to re-attempt connects.
				Thread.Sleep(2000);
				taxResponse = service.CalculateRequest(entityId, divisionId, order);
			}
			catch (Exception e)
			{
				EntityExceptionHelper.GetAndLogNetStepsException(e.Message);
				List<Message> messages = new List<Message>();
				messages.Add(new Message() { Code = 0, Info = e.Message, TransactionStatus = (Int32)TransactionStatus.Failure, Severity = 0, Source = 0 });
				taxResponse = new TaxResponse()
				{
					TransactionStatus = (Int32)TransactionStatus.Failure,
					Messages = messages.ToArray()
				};
			}
			finally
			{
				service.Close();
			}

			return CheckAndCorrectTaxRates(taxResponse);
		}

		/// <summary>
		/// Replaces tax rate in the STO response with a value
		/// that reflects the effective rate due to tax on tax
		/// situations.
		/// </summary>
		/// <param name="taxResponse"></param>
		/// <returns></returns>
		private TaxResponse CheckAndCorrectTaxRates(TaxResponse taxResponse)
		{
			if (taxResponse.LineItemTaxes == null)
				return taxResponse;

			foreach (LineItemTax lineItemTax in taxResponse.LineItemTaxes)
			{
				if (lineItemTax.TaxDetails != null)
				{
					foreach (TaxDetail taxDetail in lineItemTax.TaxDetails)
					{
						//Must round here as STO rounds to 3 decimal places.
						decimal straightTaxAmount = DecimalExtensions.GetRoundedNumber(taxDetail.TaxableAmount * taxDetail.TaxRate, 3);
						bool isTaxAmountTaxOnTax = straightTaxAmount != taxDetail.TaxApplied;
						if (isTaxAmountTaxOnTax)
						{
							taxDetail.TaxRate = taxDetail.TaxApplied / taxDetail.TaxableAmount;
						}
					}
				}
			}

			return taxResponse;
		}

		///--------------------------------------------------------------------
		/// <summary>
		/// This will call the STO service to obtain the full return
		/// response.
		/// </summary>
		///--------------------------------------------------------------------
		protected void STOFullReturnRequest(ulong TransactionID)
		{
			STOServiceBindingClient service = new STOServiceBindingClient();
			try
			{
				TaxResponse taxResponse = service.AttributedFullReturnRequest(
					entityId,
					divisionId,
					TransactionID,
					String.Empty,
					String.Empty,
					DateTime.Now.ToSTOFormattedString(),
					String.Empty);
			}
			catch (Exception e)
			{
				EntityExceptionHelper.GetAndLogNetStepsException(e.Message);
				// We log the exception, but let this continue as the calling
				// process will retry the transactions again.
			}
			finally
			{
				service.Close();
			}
		}

		///--------------------------------------------------------------------
		/// <summary>
		/// This will call the STO service to obtain the full return
		/// response.
		/// </summary>
		///--------------------------------------------------------------------
		private TaxResponse STOPartialReturnRequest(STOWebServices.PartialReturnOrder order)
		{
			STOServiceBindingClient service = new STOServiceBindingClient();
			TaxResponse taxResponse = null;
			try
			{
				taxResponse = service.PartialReturnRequest(
					entityId,
					divisionId,
					order);
			}
			catch (Exception e)
			{
				EntityExceptionHelper.GetAndLogNetStepsException(e.Message);
				// We log the exception, but let this continue as the calling
				// process will retry the transactions again.
			}
			finally
			{
				service.Close();
			}

			return taxResponse;
		}

		public override void CancelTax(OrderCustomer orderCustomer)
		{
			STOFullReturnRequest(Convert.ToUInt64(orderCustomer.SalesTaxTransactionNumber));
		}

		///--------------------------------------------------------------------
		/// <summary>
		/// This will call the STO service to finalize the tax.
		/// </summary>
		/// <exception cref="TaxException"/>
		///--------------------------------------------------------------------
		public override void FinalizeTax(OrderCustomer orderCustomer)
		{
			STOServiceBindingClient service = new STOServiceBindingClient();
			try
			{
				if (!String.IsNullOrEmpty(orderCustomer.SalesTaxTransactionNumber))
				{
					TransactionDetail transactionDetail = service.FinalizeRequest(entityId, divisionId, Convert.ToUInt64(orderCustomer.SalesTaxTransactionNumber));
					TransactionStatus status = transactionDetail.ToTransactionStatus();
					if ((status == TransactionStatus.Failure) || (status == TransactionStatus.Unknown))
					{
						throw new TaxException("Failed to finalize tax to Order");
					}
				}
				if (!String.IsNullOrEmpty(orderCustomer.UseTaxTransactionNumber))
				{
					TransactionDetail transactionDetail = service.FinalizeRequest(entityId, divisionId, Convert.ToUInt64(orderCustomer.UseTaxTransactionNumber));
					TransactionStatus status = transactionDetail.ToTransactionStatus();
					if ((status == TransactionStatus.Failure) || (status == TransactionStatus.Unknown))
					{
						throw new TaxException("Failed to finalize tax to Order");
					}
				}
			}
			catch (Exception e)
			{
				EntityExceptionHelper.GetAndLogNetStepsException(e.Message);
				throw new TaxException(e);
			}
			finally
			{
				service.Close();
			}
		}

		private bool CanTaxBeCalculated(OrderShipment orderShipment)
		{
			return orderShipment != null && IsAddressInformationAvailable(orderShipment);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="orderCustomer"></param>
		/// <param name="transactionType">what types of lineitems should we include?</param>
		/// <returns></returns>
		protected STOWebServices.Order ConvertOrderCustomerToOrder(OrderCustomer orderCustomer, string transactionType, Warehouse warehouse = null)
		{
			Address defaultShippingAddress = null;
			OrderShipment orderShipment = orderCustomer.Order.OrderShipments.FirstOrDefault(x => x.OrderCustomer == orderCustomer) ?? orderCustomer.Order.GetDefaultShipment();
			if (orderShipment == null)
			{
				defaultShippingAddress = orderCustomer.Order.Consultant.Addresses.FirstOrDefault(x => x.AddressTypeID == (short)Constants.AddressType.Shipping);
				if (defaultShippingAddress != null)
				{
					orderShipment = new OrderShipment();
					Reflection.CopyPropertiesDynamic<IAddress, IAddress>(defaultShippingAddress, orderShipment);
				}
			}
			else
			{
				defaultShippingAddress = CopyAddressFromOrderShipment(orderShipment);
			}

			if (!CanTaxBeCalculated(orderShipment as OrderShipment))
			{
				return null;
			}
			string plus4 = null;
			string warehousePlus4 = null;
			if (warehouse == null)
				warehouse = Warehouse.FindNearestByAddress(defaultShippingAddress);
			var warehouseAddress = SmallCollectionCache.Instance.Warehouses.GetById(warehouse.WarehouseID).Address;

			STOWebServices.Order order = new STOWebServices.Order
			{
				CustomerID = orderCustomer.AccountInfo.AccountNumber,
				InvoiceDate = DateTime.Now.ToSTOFormattedString(),
				InvoiceID = orderCustomer.Order.OrderID.ToString(),
				SourceSystem = Environment.MachineName,
				TestTransaction = false,
				finalize = false,
				TransactionID = transactionType == TransactionType.SalesTax ? Convert.ToUInt32(orderCustomer.SalesTaxTransactionNumber) :
					(transactionType == TransactionType.UseTax ? Convert.ToUInt32(orderCustomer.UseTaxTransactionNumber) : 0),
				CustomerType = CustomerType.Retail,
				ProviderType = ProviderType.Retail,
				NexusInfo = new NexusInfo
				{
					ShipFromAddress = new STOWebServices.Address
					{
						City = warehouseAddress.City,
						StateOrProvince = Lookup2DigitState(warehouseAddress.State),
						PostalCode = GetPostalCode(warehouseAddress.CountryID, warehouseAddress.PostalCode, ref warehousePlus4),
						CountryCode = warehouseAddress.CountryID == 2 ? "CA" : "US"
					},
				},
			};

			var postalCode = GetPostalCode(orderShipment.CountryID, orderShipment.PostalCode, ref plus4);

			order.NexusInfo.ShipToAddress = new STOWebServices.Address
			{
				Line1 = orderShipment.Address1,
				City = orderShipment.City,
				StateOrProvince = Lookup2DigitState(orderShipment.State),
				PostalCode = postalCode,
				Plus4 = plus4,
				CountryCode = orderShipment.CountryID == 2 ? "CA" : "US"
			};

			List<LineItem> lineItems = orderCustomer.ParentOrderItems.Where(oi => oi.Quantity > 0 && oi.GetAdjustedPrice((int)oi.ProductPriceTypeID) > 0)
				.SelectMany(oi => ConvertOrderItemToLineItem(oi, orderShipment.State))
				.ToList();

			// Direct Shipping
			if (transactionType == TransactionType.SalesTax && orderCustomer.ShippingTotal > 0)
			{
				lineItems.Add(new LineItem
				{
					ID = DataTypes.DirectShipID,
					AvgUnitPrice = DecimalExtensions.GetRoundedNumber(orderCustomer.ShippingTotal),
					Quantity = 1,
					SKU = "SHIPPING",
					TransactionType = TransactionType.SalesTax,
					ExemptionCode = null, // TODO
				});
			}
			if (transactionType == TransactionType.SalesTax && orderCustomer.HandlingTotal > 0)
			{
				lineItems.Add(new LineItem
				{
					ID = DataTypes.DirectHandlingID,
					AvgUnitPrice = DecimalExtensions.GetRoundedNumber(orderCustomer.HandlingTotal),
					Quantity = 1,
					SKU = "HANDLING",
					TransactionType = TransactionType.SalesTax,
					ExemptionCode = null, // TODO
				});
			}

			order.LineItems = lineItems.Where(li => li != null && (li.AvgUnitPrice > 0 || (li.Amount > 0 && li.Quantity > 0)) && li.TransactionType == transactionType).ToArray();
			return order;
		}

		private STOWebServices.PartialReturnOrder ConvertOrderCustomerToPartialReturnOrder(OrderCustomer orderCustomer)
		{
			List<PartialLineItem> lineItems = orderCustomer.ParentOrderItems.Where(oi => Math.Abs(oi.Quantity) > 0 && Math.Abs(oi.GetAdjustedPrice((int)oi.ProductPriceTypeID)) > 0)
				.SelectMany(oi => ConvertOrderItemToPartialLineItem(oi))
				.ToList();
			ArrayOfPartialLineItem arrayOfPartialLineItems = new ArrayOfPartialLineItem();
			arrayOfPartialLineItems.AddRange(lineItems);

			PartialReturnOrder partialReturnOrder = new PartialReturnOrder()
			{
				OriginalTransactionID = Convert.ToUInt64(orderCustomer.SalesTaxTransactionNumber),
				InvoiceDate = orderCustomer.Order.CompleteDate.HasValue
					? orderCustomer.Order.CompleteDate.Value.ToSTOFormattedString()
					: orderCustomer.Order.DateCreated.ToSTOFormattedString(),
				LineItems = arrayOfPartialLineItems
			};
			return partialReturnOrder;
		}

		private string NormalizePostalCode(string postalCode, Constants.Country countryId)
		{
			string regCA = "[^0-9A-Z]";
			string regUS = "[^0-9]";
			int validMaxLength;
			string doReg = string.Empty;

			if (countryId == Constants.Country.Canada)
			{
				doReg = regCA;
				validMaxLength = DataTypes.MaxPostalCodeLengthCA;
			}
			else
			{
				doReg = regUS;
				validMaxLength = DataTypes.MaxPostalCodeLengthUS;
			}

			Regex reg = new Regex(doReg, RegexOptions.IgnoreCase);
			String normalized = reg.Replace(postalCode, string.Empty);
			validMaxLength = (normalized.Length < validMaxLength ? normalized.Length : validMaxLength);

			return normalized.Substring(0, validMaxLength);
		}

		private List<LineItem> ConvertOrderItemToLineItem(OrderItem orderItem, string state)
		{
			List<LineItem> result = new List<LineItem>();
			//OrderItem ids need to be distinct per order.  If 
			string tempID = orderItem.OrderItemID > 0 ? orderItem.OrderItemID.ToString() : orderItem.Guid.ToString().Substring(0, 10);
			orderItem.TaxNumber = tempID;
			// add OrderItem.PerItemPrice as a sales transaction type for all OrderItemTypes
			result.Add(new LineItem
			{
				ID = tempID,
				ExemptionCode = null, // TODO
				//Must round here, STO will fail at times if numbers are not rounded.
				//AvgUnitPrice = DecimalExtensions.GetRoundedNumber(orderItem.PerItemPrice),
				Quantity = orderItem.Quantity,
				SKU = orderItem.SKU,
				TransactionType = TransactionType.SalesTax,
				Amount = orderItem.GetAdjustedPrice((int)orderItem.ProductPriceTypeID) > 0 ? DecimalExtensions.GetRoundedNumber(orderItem.GetAdjustedPrice((int)orderItem.ProductPriceTypeID) * orderItem.Quantity)
															: DecimalExtensions.GetRoundedNumber(orderItem.GetPreAdjustmentPrice((int)orderItem.ProductPriceTypeID) * orderItem.Quantity)
			});

			switch ((Constants.OrderItemType)orderItem.OrderItemTypeID)
			{
				case Constants.OrderItemType.HostCredit:
					// hostess credit amount as use tax
					NetSteps.Data.Entities.Tax.SalesTaxOfficeIntegration.Codes.HostessCreditItemTaxTreatment hostessCreditTreatment;
					if (StateTaxTreatment.HostessCreditItemTreatment.TryGetValue(state.ToUpperInvariant(), out hostessCreditTreatment))
					{
						decimal taxableHostessCreditUnitPrice = 0;
						switch (hostessCreditTreatment)
						{
							case NetSteps.Data.Entities.Tax.SalesTaxOfficeIntegration.Codes.HostessCreditItemTaxTreatment.Cost:
								taxableHostessCreditUnitPrice = GetWholesaleCost(orderItem);
								break;
							case NetSteps.Data.Entities.Tax.SalesTaxOfficeIntegration.Codes.HostessCreditItemTaxTreatment.SuggestedRetail:
								taxableHostessCreditUnitPrice = GetFullRetailPrice(orderItem);
								break;
							case NetSteps.Data.Entities.Tax.SalesTaxOfficeIntegration.Codes.HostessCreditItemTaxTreatment.NoTax:
							default:
								break;
						}
						taxableHostessCreditUnitPrice = taxableHostessCreditUnitPrice - DecimalExtensions.GetRoundedNumber(orderItem.GetAdjustedPrice());

						string tempCreditID = orderItem.OrderItemID > 0
							? orderItem.OrderItemID + DataTypes.HostessCredit_UseTax_IDSuffix
							: orderItem.Guid.ToString().Substring(0, 10);

						result.Add(new LineItem
						{
							ID = tempCreditID,
							ExemptionCode = null, // TODO
							AvgUnitPrice = taxableHostessCreditUnitPrice,
							Quantity = orderItem.Quantity,
							SKU = orderItem.SKU,
							TransactionType = TransactionType.UseTax
						});
					}
					break;
				default:
					break;
			}

			return result;
		}

		private List<PartialLineItem> ConvertOrderItemToPartialLineItem(OrderItem orderItem)
		{
			List<PartialLineItem> result = new List<PartialLineItem>();
			//OrderItem ids need to be distinct per order.  If 
			// add OrderItem.PerItemPrice as a sales transaction type for all OrderItemTypes

			//In case no orderItem Tax Number was stored
			string tempItemID = orderItem.OrderItemID > 0 ? orderItem.OrderItemID.ToString() : orderItem.Guid.ToString().Substring(0, 10);

			result.Add(new PartialLineItem
			{
				ID = orderItem.TaxNumber ?? tempItemID,
				//Must round here, STO will fail at times if numbers are not rounded.
				//AvgUnitPrice = DecimalExtensions.GetRoundedNumber(orderItem.PerItemPrice),
				Quantity = Math.Abs(orderItem.Quantity),
				Amount = DecimalExtensions.GetRoundedNumber(orderItem.GetAdjustedPrice() * orderItem.Quantity)
			});

			return result;
		}

		protected string GetErrorMessage(IEnumerable<Message> messages)
		{
			StringBuilder errorMessageBuilder = new StringBuilder("Sales Tax Calculation Failed");
			errorMessageBuilder.Append(Environment.NewLine);
			foreach (Message message in messages)
			{
				errorMessageBuilder.AppendFormat("{0} - {1}{2}", message.Code, message.Info, Environment.NewLine);
			}

			return errorMessageBuilder.ToString();
		}

		private decimal GetFullRetailPrice(OrderItem orderItem)
		{
			var result = 0M;
			if (orderItem.OrderItemPrices != null)
			{
				result = orderItem.OrderItemPrices.FirstOrDefault(p => p.ProductPriceTypeID == (int)Constants.ProductPriceType.Retail).OriginalUnitPrice ?? 0;
			}

			return result;

			// Why pull it out of the repository when we have it in the order item?
			//if (!orderItem.ProductID.HasValue)
			//{
			//    return 0;
			//}
			//var inventory = Create.New<InventoryBaseRepository>();
			//Product product = inventory.GetProduct(orderItem.ProductID.Value);
			//ProductPrice productPrice = product.Prices.FirstOrDefault(pp => pp.ProductPriceTypeID == (int)Constants.ProductPriceType.Retail &&
			//    pp.CurrencyID == orderItem.OrderCustomer.Order.CurrencyID);
			//return productPrice == null ? 0 : productPrice.Price;
		}

		internal string GetPostalCode(int country, string postalCode, ref string plus4)
		{
			return GetPostalCode((Constants.Country)country, postalCode, ref plus4);
		}

		internal string GetPostalCode(Constants.Country country, string postalCode, ref string plus4)
		{
			if (country == Constants.Country.UnitedStates && postalCode.Length > 5)
			{
				postalCode = postalCode.Replace("-", "");

				plus4 = postalCode.Substring(5);
			}

			return NormalizePostalCode(postalCode, country);
		}

		private decimal GetTaxTotalForType(LineItemTax lineItemTax, string authorityType, out decimal rate)
		{
			List<TaxDetail> filteredTaxDetails = lineItemTax.TaxDetails.Where(td => td.AuthorityType == authorityType).ToList();
			filteredTaxDetails.ForEach(x => Console.WriteLine("{0} taxed at {1} totaling {2:c} with {3:c} in fees.", x.TaxName, x.TaxRate, x.TaxApplied, x.FeeApplied));
			decimal taxTotalForType = filteredTaxDetails.Sum(td => td.TaxApplied + td.FeeApplied);
			rate = filteredTaxDetails.Sum(td => td.TaxRate);

			return taxTotalForType;
		}

		private decimal GetWholesaleCost(OrderItem orderItem)
		{
			if (!orderItem.ProductID.HasValue)
			{
				return 0;
			}

			return orderItem.GetAdjustedPrice();
		}

		private bool IsAddressInformationAvailable(OrderShipment orderShipment)
		{
			bool cityIsAvailable = !string.IsNullOrEmpty(orderShipment.City);
			bool stateIsAvailable = !string.IsNullOrEmpty(orderShipment.State);
			string plus4 = null;
			bool postalCodeIsAvailable = !string.IsNullOrEmpty(GetPostalCode(orderShipment.CountryID, orderShipment.PostalCode, ref plus4));

			return postalCodeIsAvailable || (cityIsAvailable && stateIsAvailable);
		}

		private void SetOrderCustomerTaxesFromOrderItem(OrderCustomer orderCustomer, OrderItem orderItem)
		{
			orderCustomer.TaxAmountCity += orderItem.Taxes.TaxAmountCity;
			orderCustomer.TaxAmountCounty += orderItem.Taxes.TaxAmountCounty;
			orderCustomer.TaxAmountState += orderItem.Taxes.TaxAmountState;
			orderCustomer.TaxAmountDistrict += orderItem.Taxes.TaxAmountDistrict;
			orderCustomer.TaxAmountTotal += orderItem.Taxes.TaxAmountTotal;
		}

		protected void SetOrderCustomerTaxesFromTaxResponses(OrderCustomer orderCustomer, IEnumerable<TaxResponse> taxResponses)
		{
			foreach (OrderItem orderItem in orderCustomer.OrderItems)
			{
				foreach (LineItemTax lineItemTax in taxResponses.Where(x => x != null && x.LineItemTaxes != null).SelectMany(x => x.LineItemTaxes)
					.Where(x => orderItem.OrderItemID > 0
						? (x.ID.Replace(DataTypes.HalfPrice_UseTax_IDSuffix, "") == orderItem.OrderItemID.ToString())
						: x.ID == orderItem.Guid.ToString().Substring(0, 10)))
				{
					SetOrderItemTaxesFromTaxResponse(orderItem, lineItemTax);
				}
				SetOrderCustomerTaxesFromOrderItem(orderCustomer, orderItem);
			}

			if (orderCustomer.OrderItems.Count > 0)
			{
				orderCustomer.TaxPercentCity = orderCustomer.OrderItems.Max(x => x.Taxes.TaxPercentCity);
				orderCustomer.TaxPercentCounty = orderCustomer.OrderItems.Max(x => x.Taxes.TaxPercentCounty);
				orderCustomer.TaxPercentDistrict = orderCustomer.OrderItems.Max(x => x.Taxes.TaxPercentDistrict);
				orderCustomer.TaxPercentState = orderCustomer.OrderItems.Max(x => x.Taxes.TaxPercentState);
				orderCustomer.TaxPercent = orderCustomer.OrderItems.Max(x => x.Taxes.TaxPercent);
			}

			LineItemTax shippingLineItemTax = taxResponses.Where(x => x != null && x.LineItemTaxes != null).SelectMany(x => x.LineItemTaxes)
				.FirstOrDefault(x => x.ID.Equals(DataTypes.DirectShipID, StringComparison.OrdinalIgnoreCase));
			if (shippingLineItemTax != null)
			{
				decimal rate;
				orderCustomer.TaxAmountCity += GetTaxTotalForType(shippingLineItemTax, AuthorityType.City, out rate);
				orderCustomer.TaxAmountCounty += GetTaxTotalForType(shippingLineItemTax, AuthorityType.County, out rate);
				orderCustomer.TaxAmountState += GetTaxTotalForType(shippingLineItemTax, AuthorityType.State, out rate);
				orderCustomer.TaxAmountDistrict += GetTaxTotalForType(shippingLineItemTax, AuthorityType.ReportingAgency, out rate);
				orderCustomer.TaxAmountTotal += shippingLineItemTax.TotalTaxApplied;
				orderCustomer.TaxAmountShipping += shippingLineItemTax.TotalTaxApplied;
			}

			LineItemTax handlingLineItemTax = taxResponses.Where(x => x != null && x.LineItemTaxes != null).SelectMany(x => x.LineItemTaxes)
				.FirstOrDefault(x => x.ID.Equals(DataTypes.DirectHandlingID, StringComparison.OrdinalIgnoreCase));
			if (handlingLineItemTax != null)
			{
				decimal rate;
				orderCustomer.TaxAmountCity += GetTaxTotalForType(handlingLineItemTax, AuthorityType.City, out rate);
				orderCustomer.TaxAmountCounty += GetTaxTotalForType(handlingLineItemTax, AuthorityType.County, out rate);
				orderCustomer.TaxAmountState += GetTaxTotalForType(handlingLineItemTax, AuthorityType.State, out rate);
				orderCustomer.TaxAmountDistrict += GetTaxTotalForType(handlingLineItemTax, AuthorityType.ReportingAgency, out rate);
				orderCustomer.TaxAmountTotal += handlingLineItemTax.TotalTaxApplied;
				orderCustomer.TaxAmountShipping += handlingLineItemTax.TotalTaxApplied;
			}

			orderCustomer.TaxAmountTotal = orderCustomer.TaxAmountTotal;
		}

		private void SetOrderItemTaxesFromTaxResponse(OrderItem orderItem, LineItemTax lineItemTax)
		{
			decimal rate;

			orderItem.Taxes.TaxAmountCity += GetTaxTotalForType(lineItemTax, AuthorityType.City, out rate);
			orderItem.Taxes.TaxPercentCity = rate;

			orderItem.Taxes.TaxAmountCounty += GetTaxTotalForType(lineItemTax, AuthorityType.County, out rate);
			orderItem.Taxes.TaxPercentCounty = rate;

			orderItem.Taxes.TaxAmountCountry += GetTaxTotalForType(lineItemTax, AuthorityType.Federal, out rate);
			orderItem.Taxes.TaxPercentCountry = rate;

			orderItem.Taxes.TaxAmountCityLocal += GetTaxTotalForType(lineItemTax, AuthorityType.Local, out rate);
			orderItem.Taxes.TaxPercentCountyLocal = rate;

			orderItem.Taxes.TaxAmountState += GetTaxTotalForType(lineItemTax, AuthorityType.State, out rate);
			orderItem.Taxes.TaxPercentState = rate;

			orderItem.Taxes.TaxAmountDistrict += GetTaxTotalForType(lineItemTax, AuthorityType.ReportingAgency, out rate);
			orderItem.Taxes.TaxPercentDistrict = rate;

			orderItem.Taxes.TaxAmountTotal += lineItemTax.TotalTaxApplied;
			orderItem.Taxes.TaxPercent = lineItemTax.TaxDetails.Sum(x => x.TaxRate);

			base.CopyTaxValuesToOrderItem(orderItem);
			//orderItem.Save();
		}

		public List<KeyValuePair<string, string>> GetExemptionCodes()
		{
			var service = new STOServiceBindingClient();
			try
			{

				var values = service.GetDataValues(DataTypes.Exemptions, entityId, divisionId);
				var result = values == null || values.DataDetails == null || !values.DataDetails.Any()
					? new List<KeyValuePair<string, string>>()
					: values.DataDetails.Select(xx => new KeyValuePair<string, string>(xx.Key, xx.Value)).ToList();

				return result;
			}
			finally
			{

				service.Close();
			}
		}

		private string Lookup2DigitState(string state)
		{
			foreach (var item in Constants.StatesAndTerritories_USA_CAN)
			{
				if (string.Equals(item.Key, state, StringComparison.OrdinalIgnoreCase) || string.Equals(item.Value, state, StringComparison.OrdinalIgnoreCase))
				{
					return item.Key;
				}
			}

			return state;
		}

		protected Address CopyAddressFromOrderShipment(OrderShipment shipment)
		{
			Address address = new Address();
			address.Address1 = shipment.Address1;
			address.Address2 = shipment.Address2;
			address.Address3 = shipment.Address3;
			address.City = shipment.City;
			address.County = shipment.County;
			address.State = shipment.State;
			address.PostalCode = shipment.PostalCode;
			address.Country = shipment.Country;
			address.StateProvinceID = shipment.StateProvinceID;

			return address;
		}

	}
}
