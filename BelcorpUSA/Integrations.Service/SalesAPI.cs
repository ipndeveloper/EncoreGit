using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using NetSteps.Integrations.Service.Interfaces;
using NetSteps.Integrations.Service.DataModels;
using NetSteps.Integrations.Internals.Security;
using NetSteps.Data.Entities;
using NetSteps.Diagnostics.Utilities;
using NetSteps.Integrations.Service.Entities;
using NetSteps.Encore.Core;
using System.Diagnostics.Contracts;
using System.Data.Objects;

namespace NetSteps.Integrations.Service
{
	[ServiceBehavior(
						ConcurrencyMode = ConcurrencyMode.Multiple,
						InstanceContextMode = InstanceContextMode.PerCall,
						Name = "salesAPI",
						IncludeExceptionDetailInFaults = true,
						MaxItemsInObjectGraph = 2147483646
					)]
	public class SalesAPI : ISalesAPI
	{
		public bool CreateKitItemValuations(string userName, string password, KitItemValuationModelCollection definitions)
		{
			Contract.Requires(!String.IsNullOrWhiteSpace(userName), "userName is required");
			Contract.Requires(!String.IsNullOrWhiteSpace(password), "password is required");
			Contract.Requires(definitions != null, "definitions is required");
			Contract.Requires(definitions.Any(), "definitions must contain items");

			if (!IntegrationsSecurity.Authenticate(userName, password))
			{
				throw new System.Security.Authentication.AuthenticationException();
			}

			bool result = false;

			var parentSkus = definitions.Select(k => k.ParentSku).Distinct().ToArray();

			foreach (var sku in parentSkus)
			{
				if (definitions.Where(k => k.ParentSku == sku).Sum(k => k.ParticipationPercentage) != 1.0m)
				{
					throw new ArgumentOutOfRangeException("definitions", String.Format("The items for Parent SKU/CUV '{0}' do not add up to 1.0 (100%)", sku));
				}
			}

			using (BelcorpIntegrationsDbContext ctx = new BelcorpIntegrationsDbContext())
			{
				var existingRecords = (from kiv in ctx.KitItemValuations
									   where parentSkus.Contains(kiv.ParentSku)
									   select kiv);
				if (existingRecords.Any())
				{
					foreach (var item in existingRecords)
					{
						ctx.KitItemValuations.Remove(item);
					}
					ctx.SaveChanges();
				}

				foreach (var item in definitions)
				{
					var newItem = ctx.KitItemValuations.Create();
					newItem.ParentSku = item.ParentSku;
					newItem.ChildSku = item.ChildSku;
					newItem.ParticipationPercentage = item.ParticipationPercentage;
					ctx.KitItemValuations.Add(newItem);
				}
				ctx.SaveChanges();
				result = true;
			}

			return result;
		}

		public OrderModelCollection GetSales(string userName, string password, DateTime startDate, DateTime endDate)
		{
			Contract.Requires(!String.IsNullOrWhiteSpace(userName), "userName is required");
			Contract.Requires(!String.IsNullOrWhiteSpace(password), "password is required");
			Contract.Requires(startDate != null, "startDate is required");
			Contract.Requires(endDate != null, "startDate is required");
			Contract.Requires(startDate != endDate, "startDate and endDate cannot be equal");
			Contract.Requires(startDate < endDate, "startDate must be less than endDate");

			if (!IntegrationsSecurity.Authenticate(userName, password))
			{
				throw new System.Security.Authentication.AuthenticationException();
			}

			var result = new OrderModelCollection();
			using (var getSalesActivity = this.TraceActivity(String.Format("Collecting Sales between '{0}' and '{1}'", startDate, endDate)))
			using (NetSteps.Data.Entities.NetStepsEntities nse = new Data.Entities.NetStepsEntities())
			{
				var excludeordertypes = new short[] { 
					(short)Constants.OrderType.AutoshipTemplate };
				//(short)Constants.OrderType.ReplacementOrder};

				var includedOrderStatus = new int[] { 
					(int)Constants.OrderStatus.Paid,
					(int)Constants.OrderStatus.CancelledPaid,
					(int)Constants.OrderStatus.Printed,
					(int)Constants.OrderStatus.Shipped
				};

				//Collect CanceledPaid orders and corresponding returns that occurred on the same day so that they can be omitted from the results.
				var allExcludedOrderIds = (from order in nse.Orders
										   join returns in nse.Orders on new { OrderId = order.OrderID, OrderTypeId = (short)Constants.OrderType.ReturnOrder } equals new { OrderId = returns.ParentOrderID.Value, OrderTypeId = returns.OrderTypeID }
										   where order.OrderStatusID == (int)Constants.OrderStatus.CancelledPaid
											   && !excludeordertypes.Contains(order.OrderTypeID)
											   && order.CompleteDateUTC >= startDate
											   && order.CompleteDateUTC <= endDate
											   && returns.CompleteDateUTC <= endDate
										   select new { CanceledId = order.OrderID, ReturnedId = returns.OrderID }).ToArray();

				var excludedOrderIds = new List<int>();
				excludedOrderIds.Add(0);
				if (allExcludedOrderIds.Any())
				{
					excludedOrderIds.AddRange(allExcludedOrderIds.Select(e => e.CanceledId).Distinct().ToArray());
					excludedOrderIds.AddRange(allExcludedOrderIds.Select(e => e.ReturnedId).Distinct().ToArray());
				}

				var collectOrdersActivity = this.TraceActivity(String.Format("Collecting Orders between '{0}' and '{1}'", startDate, endDate));
				var orders = (from customer in nse.OrderCustomers
							  join account in nse.Accounts on customer.AccountID equals account.AccountID
							  where customer.Order.CompleteDateUTC >= startDate
							  && customer.Order.CompleteDateUTC <= endDate
							  && !excludeordertypes.Contains(customer.Order.OrderTypeID)
							  && includedOrderStatus.Contains(customer.Order.OrderStatusID)
							  && !excludedOrderIds.Contains(customer.Order.OrderID)
							  select new
							  {
								  PartyId = customer.Order.OrderID,
								  ParentPartyId = customer.Order.ParentOrderID,
								  OrderId = customer.OrderCustomerID,
								  OrderType = customer.Order.OrderType.Name,
								  OrderDate = customer.Order.CompleteDateUTC,
								  DistributorAccountID = customer.Order.ConsultantID,
								  AccountID = account.AccountID,
								  LastName = account.LastName,
								  FirstName = account.FirstName,
								  AccountTypeID = account.AccountTypeID,
								  SubTotal = customer.Subtotal,
								  TaxTotal = customer.TaxAmountTotal,
								  PartyTaxOverride = customer.Order.TaxAmountTotalOverride,
								  ShippingTotal = customer.ShippingTotal,
								  PartyShippingTotal = customer.Order.ShippingTotal,
								  HandlingTotal = customer.HandlingTotal,
								  Total = customer.Total,
								  IsHost = customer.OrderCustomerTypeID == (int)Constants.OrderCustomerType.Hostess
							  }).Distinct().ToArray();
				((IDisposable)collectOrdersActivity).Dispose();
				if (orders.Any())
				{
					var consultantIds = orders.Select(o => o.DistributorAccountID).Distinct();
					var collectConsultantActivity = this.TraceActivity(String.Format("Collecting consultants for orders with account ids '{0}'", String.Join(", ", consultantIds)));
					var consultants = (from account in nse.Accounts
									   from address in account.Addresses.DefaultIfEmpty()
									   where consultantIds.Contains(account.AccountID)
									   select new
									   {
										   ID = account.AccountID,
										   LastName = account.LastName,
										   FirstName = account.FirstName,
										   AddressTypeId = address == null ? 999999 : address.AddressTypeID,
										   State = address == null ? String.Empty : address.State ?? String.Empty,
										   Region = address == null ? String.Empty : (address.StateProvince != null ? (address.StateProvince.ShippingRegion != null ? address.StateProvince.ShippingRegion.Name : String.Empty) : String.Empty)
									   })
									   .Distinct()
									   .OrderBy(a => a.AddressTypeId) //Gives precedence to the "Main" address later.
									   .GroupBy(a => a.ID)
									   .ToArray();

					((IDisposable)collectConsultantActivity).Dispose();

					Func<string, int?, int, int> partyIdSelector = (orderType, parentPartyId, partyId) =>
					{
						int val = 0;
						switch (GetBelcorpOrderType(orderType))
						{
							case 4:
							case 5:
							case 6:
							case 7:
							case 8:
							case 9:
								val = partyId;
								break;
							default:
								val = parentPartyId ?? partyId;
								break;
						}
						return val;
					};

					var partyIds = orders.Select(o => partyIdSelector(o.OrderType, o.ParentPartyId, o.PartyId));

					var collecthostActivity = this.TraceActivity(String.Format("Collecting hosts for orders/parties with ids '{0}'", String.Join(", ", partyIds)));
					var hosts = (from customer in nse.OrderCustomers
								 where partyIds.Contains(customer.OrderID)
								 && customer.OrderCustomerTypeID == (int)Constants.OrderCustomerType.Hostess
								 select new { PartyId = customer.OrderID, HostId = customer.AccountID }).ToArray();
					((IDisposable)collecthostActivity).Dispose();

					foreach (var order in orders)
					{
						var collectCCPaymentsActivity = this.TraceActivity(String.Format("Collecting CC Payments for customer id '{0}'", order.OrderId));
						var ccPayments = (from orderpayments in nse.OrderPayments
										  where orderpayments.PaymentTypeID == (short)Constants.PaymentType.CreditCard
										  && orderpayments.OrderPaymentStatusID == (short)Constants.OrderPaymentStatus.Completed
										  && orderpayments.OrderCustomerID == order.OrderId
										  select orderpayments).ToArray();
						((IDisposable)collectCCPaymentsActivity).Dispose();
						decimal ccTotal = 0;
						if (ccPayments.Any())
						{
							ccTotal += ccPayments.Sum(c => c.Amount);
						}

						var partyOrders = (from o in orders
										   where o.PartyId == order.PartyId
										   select o.OrderId);
						decimal? taxOverride = null;
						if (partyOrders.Min() == order.OrderId) //Only execute for the first party order member
						{
							//This should work fine for now, given that Belcorp USA is not using parties... 
							//but in the event it does, and a party level payment is applied,
							//all payments that are not independent will roll up into the first party member, which may appear as an overpayment...
							var collectPartyCCPaymentsActivity = this.TraceActivity(String.Format("Collecting CC Payments for Party id '{0}'", order.PartyId));
							var ccPartyPayments = (from orderpayments in nse.OrderPayments
												   where orderpayments.PaymentTypeID == (short)Constants.PaymentType.CreditCard
												   && orderpayments.OrderPaymentStatusID == (short)Constants.OrderPaymentStatus.Completed
												   && orderpayments.OrderCustomerID == null
												   && orderpayments.OrderID == order.PartyId
												   select orderpayments).ToArray();
							((IDisposable)collectPartyCCPaymentsActivity).Dispose();

							if (ccPartyPayments.Any())
							{
								ccTotal += ccPartyPayments.Sum(c => c.Amount);
							}

							if (order.PartyTaxOverride.HasValue)
							{
								taxOverride = order.PartyTaxOverride;
							}
						}

						var collectLedgerPaymentsActivity = this.TraceActivity(String.Format("Collecting ledger Payments for customer id '{0}'", order.OrderId));
						var ledgerCredits = (from orderpayments in nse.OrderPayments
											 where orderpayments.PaymentTypeID != (short)Constants.PaymentType.CreditCard
											 && orderpayments.OrderPaymentStatusID == (short)Constants.OrderPaymentStatus.Completed
											 && orderpayments.OrderCustomerID == order.OrderId
											 select orderpayments).ToArray();
						((IDisposable)collectLedgerPaymentsActivity).Dispose();
						decimal ledTotal = 0;
						if (ledgerCredits.Any())
						{
							ledTotal = ledgerCredits.Sum(l => l.Amount);
						}

						var consultant = consultants.Where(c => c.Key == order.DistributorAccountID).FirstOrDefault();
						if (consultant != null)
						{
							var consultantInfo = consultant.FirstOrDefault();
							var oModel = new OrderModel()
							{
								PartyID = partyIdSelector(order.OrderType, order.ParentPartyId, order.PartyId),
								OrderID = order.OrderId,
								OrderType = GetBelcorpOrderType(order.OrderType),
								OrderDate = new DateTime(order.OrderDate.GetValueOrDefault().Ticks, DateTimeKind.Utc),
								PartyHostAccountID = hosts.Where(h => h.PartyId == order.PartyId).Select(h => h.HostId).FirstOrDefault(),
								DistributorAccountID = order.DistributorAccountID,
								DistributorName = consultantInfo == null ? "MISSING" : String.Format("{0}, {1}", consultantInfo.LastName, consultantInfo.FirstName),
								DistributorState = consultantInfo == null ? "MISSING" : consultantInfo.State,
								DistributorRegion = consultantInfo == null ? "MISSING" : consultantInfo.Region,
								AccountID = order.AccountID,
								AccountName = String.Format("{0}, {1}", order.LastName, order.FirstName),
								AccountTypeID = order.AccountTypeID == 1 ? 1 : 0, //1 = BA order; 0 = customer order; 
								CCPaymentTotal = Math.Abs(ccTotal),
								LedgerPaymentTotal = Math.Abs(ledTotal),
								SubTotal = Math.Abs(order.SubTotal.GetValueOrDefault()),
								TaxTotal = taxOverride.HasValue ? Math.Abs(taxOverride.Value) : Math.Abs(order.TaxTotal.GetValueOrDefault()),
								ShippingTotal = Math.Abs(Math.Min(order.ShippingTotal.GetValueOrDefault(), order.PartyShippingTotal.GetValueOrDefault())),
								HandlingTotal = Math.Abs(order.HandlingTotal.GetValueOrDefault()),
								Total = Math.Abs(order.Total.GetValueOrDefault())
							};

							decimal cv;
							decimal qv;
							oModel.Items = GetItems(oModel.OrderID, nse, out cv, out qv);
							oModel.TotalCV = cv;
							oModel.TotalQV = qv;

							result.Add(oModel);
						}
						else  //Worst Case Scenario
						{
							var oModel = new OrderModel()
							{
								PartyID = partyIdSelector(order.OrderType, order.ParentPartyId, order.PartyId),
								OrderID = order.OrderId,
								OrderType = GetBelcorpOrderType(order.OrderType),
								OrderDate = new DateTime(order.OrderDate.GetValueOrDefault().Ticks, DateTimeKind.Utc),
								PartyHostAccountID = hosts.Where(h => h.PartyId == order.PartyId).Select(h => h.HostId).FirstOrDefault(),
								DistributorAccountID = order.DistributorAccountID,
								DistributorName = "MISSING",
								DistributorState = "MISSING",
								DistributorRegion = "MISSING",
								AccountID = order.AccountID,
								AccountName = String.Format("{0}, {1}", order.LastName, order.FirstName),
								AccountTypeID = order.AccountTypeID == 1 ? 1 : 0, //1 = BA order; 0 = customer order; 
								CCPaymentTotal = Math.Abs(ccTotal),
								LedgerPaymentTotal = Math.Abs(ledTotal),
								SubTotal = Math.Abs(order.SubTotal.GetValueOrDefault()),
								TaxTotal = taxOverride.HasValue ? Math.Abs(taxOverride.Value) : Math.Abs(order.TaxTotal.GetValueOrDefault()),
								ShippingTotal = Math.Abs(Math.Min(order.ShippingTotal.GetValueOrDefault(), order.PartyShippingTotal.GetValueOrDefault())),
								HandlingTotal = Math.Abs(order.HandlingTotal.GetValueOrDefault()),
								Total = Math.Abs(order.Total.GetValueOrDefault())
							};

							decimal cv;
							decimal qv;
							oModel.Items = GetItems(oModel.OrderID, nse, out cv, out qv);
							oModel.TotalCV = cv;
							oModel.TotalQV = qv;

							result.Add(oModel);
						}
					}
				}
			}

			return result;
		}

		private OrderItemModelCollection GetItems(int customerId, NetStepsEntities nse, out decimal cv, out decimal qv)
		{
			var result = new OrderItemModelCollection();

			var collectOrderitemsActivity = this.TraceActivity(String.Format("Collecting order items for customer id '{0}'", customerId));
			var items = (from oi in nse.OrderItems
						 where oi.OrderCustomerID == customerId
						 && oi.ParentOrderItemID == null
						 select new
						 {
							 ItemId = oi.OrderItemID,
							 SKU = oi.SKU,
							 Quantity = Math.Abs(oi.Quantity),
							 UnitPrice = oi.OrderItemPrices.Where(oip => oip.ProductPriceTypeID == oi.ProductPriceTypeID).FirstOrDefault(),
							 CVPrice = oi.OrderItemPrices.Where(oip => oip.ProductPriceTypeID == (int)Constants.ProductPriceType.CV).FirstOrDefault(),
							 QVPrice = oi.OrderItemPrices.Where(oip => oip.ProductPriceTypeID == (int)Constants.ProductPriceType.QV).FirstOrDefault(),
							 MaterialGroupID = (oi.Product.Properties.Any(pp => pp.ProductPropertyType.Name == "Material Group") ? oi.Product.Properties.FirstOrDefault(pp => pp.ProductPropertyType.Name == "Material Group").PropertyValue : "0"),
							 SAPCode = (oi.Product.Properties.Any(pp => pp.ProductPropertyType.Name == "SAP Code") ? oi.Product.Properties.FirstOrDefault(pp => pp.ProductPropertyType.Name == "SAP Code").PropertyValue : null),
							 OfferType = (oi.Product.Properties.Any(pp => pp.ProductPropertyType.Name == "Offer Type") ? oi.Product.Properties.FirstOrDefault(pp => pp.ProductPropertyType.Name == "Offer Type").PropertyValue : null),
							 ProductType = oi.Product.ProductBase.ProductType.Name,
							 SaleType = (oi.OrderItemType.IsHostessReward || oi.OrderAdjustmentOrderLineModifications.Any(oaolm => oaolm.ModificationOperationID == (int)NetSteps.OrderAdjustments.Common.Model.OrderAdjustmentOrderLineOperationKind.AddedItem) ? InvoiceMaterialGroupSaleType.Gift : InvoiceMaterialGroupSaleType.Regular),
							 HasChildren = oi.ChildOrderItems.Any()
						 }).ToArray();
			((IDisposable)collectOrderitemsActivity).Dispose();

			cv = items.Where(i => i.CVPrice != null).Sum(i => Math.Abs(i.CVPrice.UnitPrice) * i.Quantity);
			qv = items.Where(i => i.QVPrice != null).Sum(i => Math.Abs(i.QVPrice.UnitPrice) * i.Quantity);

			foreach (var item in items)
			{
				InvoiceMaterialGroupProductType prodType;
				if (!Enum.TryParse(item.ProductType, true, out prodType))
				{
					prodType = InvoiceMaterialGroupProductType.Unknown;
				}
				var model = new OrderItemModel()
				{
					ItemID = item.ItemId,
					ItemTotal = Math.Abs(item.UnitPrice.UnitPrice) * item.Quantity,
					MaterialGroupID = item.MaterialGroupID,
					ProductType = (int)prodType,
					Quantity = item.Quantity,
					SaleType = (int)item.SaleType,
					SAPCode = item.SAPCode,
					OfferType = item.OfferType,
					SKU = item.SKU,
					UnitPrice = Math.Abs(item.UnitPrice.UnitPrice)
				};
				if (item.HasChildren)
				{
					model.ChildItems = GetKitItems(item.ItemId, model.SKU, model.UnitPrice, model.Quantity, nse);
				}
				result.Add(model);
			}

			return result;
		}

		private OrderKitItemModelCollection GetKitItems(int parentItemId, string parentSku, decimal parentValue, int parentQty, NetStepsEntities nse)
		{
			IDictionary<string, KitItemValuation> valuations;
			using (BelcorpIntegrationsDbContext ctx = new BelcorpIntegrationsDbContext())
			{
				var vals = ctx.KitItemValuations.Where(k => k.ParentSku == parentSku);
				if (vals.Any())
				{
					valuations = vals.ToDictionary(k => k.ChildSku);
				}
				else
				{
					valuations = new Dictionary<string, KitItemValuation>();
				}
			}

			var result = new OrderKitItemModelCollection();
			var collectOrderitemsActivity = this.TraceActivity(String.Format("Collecting order item kit children for order item id '{0}'", parentItemId));
			var items = (from oi in nse.OrderItems
						 where oi.ParentOrderItemID == parentItemId
						 select new
						 {
							 ItemID = oi.OrderItemID,
							 SKU = oi.SKU,
							 Quantity = oi.Quantity,
							 MaterialGroupID = (oi.Product.Properties.Any(pp => pp.ProductPropertyType.Name == "Material Group") ? oi.Product.Properties.FirstOrDefault(pp => pp.ProductPropertyType.Name == "Material Group").PropertyValue : "0"),
							 SAPCode = (oi.Product.Properties.Any(pp => pp.ProductPropertyType.Name == "SAP Code") ? oi.Product.Properties.FirstOrDefault(pp => pp.ProductPropertyType.Name == "SAP Code").PropertyValue : null),
							 OfferType = (oi.Product.Properties.Any(pp => pp.ProductPropertyType.Name == "Offer Type") ? oi.Product.Properties.FirstOrDefault(pp => pp.ProductPropertyType.Name == "Offer Type").PropertyValue : null),
							 ProductType = oi.Product.ProductBase.ProductType.Name,
							 SaleType = InvoiceMaterialGroupSaleType.Regular
						 }).ToArray();
			((IDisposable)collectOrderitemsActivity).Dispose();

			foreach (var item in items)
			{
				InvoiceMaterialGroupProductType prodType;
				if (!Enum.TryParse(item.ProductType.Trim(), true, out prodType))
				{
					prodType = InvoiceMaterialGroupProductType.Unknown;
				}
				var unitPrice = 0.0m;
				if (valuations.ContainsKey(item.SKU))
				{
					unitPrice = Math.Round((parentValue * valuations[item.SKU].ParticipationPercentage) / (item.Quantity / parentQty), 3);
				}
				result.Add(new OrderKitItemModel()
				{
					ItemID = item.ItemID,
					ItemTotal = Math.Round(unitPrice * item.Quantity, 3),
					MaterialGroupID = item.MaterialGroupID,
					ProductType = (int)prodType,
					Quantity = item.Quantity,
					SaleType = (int)item.SaleType,
					SAPCode = item.SAPCode,
					OfferType = item.OfferType,
					SKU = item.SKU,
					UnitPrice = unitPrice
				});
			}

			return result;
		}

		private int GetBelcorpOrderType(string nsOrderType)
		{
			int result = 0;
			//1 - Normal order (online order, workstation order, portal order, comp order)
			//2 - Glam up order (glam up order, online glam up order) (ns = Party Order)
			//3 - Enrollment order
			//4 - Return order
			//5 - AutoshipTemplate, AutoshipOrder
			//6 - OverriderOrder
			//7 - ReplacementOrder
			//8 - EmployeeOrder
			//9 - BusinessMaterialsOrders
			//10 - FundraiserOrder
			switch (nsOrderType)
			{
				case "Online Order":
				case "Workstation Order":
				case "Portal Order":
				case "Comp Order":
					result = 1;
					break;
				case "Party Order":
				case "Online Party Order":
					result = 2;
					break;
				case "Enrollment Order":
					result = 3;
					break;
				case "Return Order":
					result = 4;
					break;
				case "Autoship Order":
				case "Autoship Template":
					result = 5;
					break;
				case "Override Order":
					result = 6;
					break;
				case "Replacement Order":
					result = 7;
					break;
				case "Employee Order":
					result = 8;
					break;
				case "Business Materials Order":
					result = 9;
					break;
				case "Fundraiser Order":
					result = 10;
					break;
				default: //Hostess Rewards Order, Comp Order
					break;
			}

			return result;
		}

	}
}
