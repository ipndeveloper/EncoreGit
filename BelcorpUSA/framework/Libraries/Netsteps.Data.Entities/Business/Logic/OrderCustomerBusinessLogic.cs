using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Generated;
using NetSteps.Data.Entities.Interfaces;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Data.Entities.Business.Logic
{
	public partial class OrderCustomerBusinessLogic
	{
		public IShippingCalculator ShippingCalculator { get { return Create.New<IShippingCalculator>(); } }

		public override void DefaultValues(Repositories.IOrderCustomerRepository repository, OrderCustomer entity)
		{
			entity.FutureBookingDate = null;
			entity.Guid = Guid.NewGuid();
			entity.IsTaxExempt = false;
		}

		public override void CleanDataBeforeSave(Repositories.IOrderCustomerRepository repository, OrderCustomer entity)
		{
			if (entity != null)
			{
				if (entity.OrderShipments != null)
				{
					foreach (var orderShipment in entity.OrderShipments)
						orderShipment.Trim();
				}
			}
		}

		public virtual IEnumerable<ShippingMethodWithRate> GetShippingMethods(OrderCustomer customer)
		{
			if (customer.OrderShipments == null || customer.OrderShipments.Count == 0)
				throw new InvalidOperationException("Cannot get shipping methods when there are no shipments");
			return ShippingCalculator.GetShippingMethodsWithRates(customer, customer.OrderShipments[0]).OrderBy(sm => sm.ShippingAmount);
		}

		/// <summary>
		/// Clear out any orderItems that are hostess rewards from given orderCustomer.
		/// </summary>
		/// <param name="orderCustomer"></param>
		public virtual void ClearHostessRewards(OrderCustomer orderCustomer)
		{
			var order = orderCustomer.Order;
			//Chris Bush says that order customers should never have hostess rewards if they are not hostess.
			//Not sure if this is correct but I'm going with it until we find out otherwise.
			var orderitems = orderCustomer.OrderItems.Where(x => x.IsHostReward).ToList();
			foreach (OrderItem oi in orderitems)
			{
				order.RemoveItem(oi);
			}
		}

		public virtual BasicResponse ValidateHostessRewardItem(OrderCustomer orderCustomer, OrderItem orderItem, Order order, bool includeQuantityInCheck)
		{
			return ValidateHostessRewardItem(orderCustomer, includeQuantityInCheck ? orderItem.Quantity : 0, orderItem.HostessRewardRuleID, order);
		}

		public virtual BasicResponse ValidateHostessRewardItem(OrderCustomer orderCustomer, int quantity, int? hostRewardRuleId, Order order)
		{
			BasicResponse response = new BasicResponse() { Success = true };

			if (hostRewardRuleId.HasValue)
			{
				HostessRewardRule rule = SmallCollectionCache.Instance.HostessRewardRules.GetById(hostRewardRuleId.ToInt());
				response = rule.IsActiveRule();

				if (response.Success)
				{
					response = ValidateHostessRewardTotal(rule, order);
				}

				if (response.Success)
				{
					switch ((Constants.HostessRewardType)rule.HostessRewardTypeID)
					{
						case ConstantsGenerated.HostessRewardType.ExclusiveProduct:
							{
								int numOfRedeemedItems = orderCustomer.GetNumberOfRedeemedItems(hostRewardRuleId);
								int totalPossible = orderCustomer.GetTotalRedeemableItems(hostRewardRuleId.ToShort());

								if (response.Success && quantity + numOfRedeemedItems > totalPossible)
								{
									if (quantity == 0)
									{
										response.Message = Translation.GetTerm("TooManyExclusiveProductsOnOrder", "Too Many Exclusive Products On Order.");
									}
									else if (quantity < 0)
									{
										response.Message = Translation.GetTerm("InvalidQuantityError", "Error adding item to order. Invalid quantity for product: {0}", string.Empty);
									}
									else
									{
										response.Message = Translation.GetTerm("NotEnoughExclusiveProductsLeft", "Not enough exclusive products left.");
									}

									response.Success = false;
								}
								break;
							}
						case ConstantsGenerated.HostessRewardType.FreeItem:
							{
								int numOfRedeemedItems = orderCustomer.GetNumberOfRedeemedItems(hostRewardRuleId);
								int totalPossible = orderCustomer.GetTotalRedeemableItems(hostRewardRuleId.ToShort());

								if (response.Success && quantity + numOfRedeemedItems > totalPossible)
								{
									if (quantity == 0)
									{
										response.Message = Translation.GetTerm("TooManyFreeItemsOnOrder", "Too Many Free Items On Order.");
									}
									else if (quantity < 0)
									{
										response.Message = Translation.GetTerm("InvalidQuantityError", "Error adding item to order. Invalid quantity for product: {0}", string.Empty);
									}
									else
									{
										response.Message = Translation.GetTerm("NotEnoughFreeItemsLeft", "Not enough free items left.");
									}

									response.Success = false;
								}
								break;
							}
						case ConstantsGenerated.HostessRewardType.PercentOff:
						case ConstantsGenerated.HostessRewardType.BookingCredit:
							{
								int numOfRedeemedItems = orderCustomer.GetNumberOfRedeemedItems(hostRewardRuleId);
								int totalPossible = orderCustomer.GetTotalRedeemableItems(hostRewardRuleId.ToShort());

								if (response.Success && quantity + numOfRedeemedItems > totalPossible)
								{
									if (quantity == 0)
									{
										response.Message = Translation.GetTerm("TooManyProductDiscountsOnOrder", "Too Many Product Discounts On Order.");
									}
									else if (quantity < 0)
									{
										response.Message = Translation.GetTerm("InvalidQuantityError", "Error adding item to order. Invalid quantity for product: {0}", string.Empty);
									}
									else
									{
										response.Message = Translation.GetTerm("NotEnoughProductDiscountsLeft", "Not enough product discounts left.");
									}

									response.Success = false;
								}
								break;
							}
						case ConstantsGenerated.HostessRewardType.ItemDiscount:
							{
								int numOfRedeemedItems = orderCustomer.GetNumberOfRedeemedItemsByType(ConstantsGenerated.HostessRewardType.ItemDiscount);
								int totalPossible = orderCustomer.GetTotalRedeemableItems(hostRewardRuleId.ToShort());

								if (response.Success && quantity + numOfRedeemedItems > totalPossible)
								{
									if (quantity == 0)
									{
										response.Message = Translation.GetTerm("TooManyProductDiscountsOnOrder", "Too Many Product Discounts On Order.");
									}
									else if (quantity < 0)
									{
										response.Message = Translation.GetTerm("InvalidQuantityError", "Error adding item to order. Invalid quantity for product: {0}", string.Empty);
									}
									else
									{
										response.Message = Translation.GetTerm("NotEnoughProductDiscountsLeft", "Not enough product discounts left.");
									}

									response.Success = false;
								}
								break;
							}
					}
				}
				else
				{
					response.Success = false;
				}
			}
			else
			{
				response.Success = false;
			}

			if (!response.Success && string.IsNullOrWhiteSpace(response.Message))
			{
				response.Message = Translation.GetTerm("InvalidHostRewardRule", "Invalid Host Reward Rule.");
			}

			return response;
		}

		public virtual BasicResponse ValidateHostessRewardTotal(HostessRewardRule rule, Order order)
		{
			BasicResponse response = new BasicResponse() { Success = false };

			var party = order.Parties.FirstOrDefault();

			// There are scenarios in which the Parties might not have loaded with Order. One example is when Host Rewards are redeemed,
			// cart will be loaded with OrderID and Party details will not be part of Order. In those scenarios, try to get the Party using OrderID.
			if (null == party)
			{
				party = Party.LoadByOrderID(order.OrderID);
				party.Order = order;
			}

			if (party != null)
			{
				var results = party.GetApplicableHostessRewardRules();
				response.Success = results.Select(r => r.HostessRewardRuleID).Contains(rule.HostessRewardRuleID);

				if (!response.Success && string.IsNullOrWhiteSpace(response.Message))
				{
					response.Message = GetRewardRuleTypeSpecificMessage(rule);
				}
			}

			return response;
		}

		public virtual string GetRewardRuleTypeSpecificMessage(HostessRewardRule rule)
		{
			string message = "";
			string rewardType = SmallCollectionCache.Instance.HostessRewardTypes.GetById(rule.HostessRewardTypeID).GetTerm();

			switch (rule.HostessRewardRuleTypeID)
			{
				case (int)Constants.HostessRewardRuleType.CustomerCount:
					message = Translation.GetTerm("ErrorInvalidCustomerCountHostReward", "You are no longer eligible for the {0} at the redeemed level. This reward requires a minimum number of customers: {1}.", rewardType, rule.MinCustomers);
					break;
				case (int)Constants.HostessRewardRuleType.OrderSubtotal:
					message = Translation.GetTerm("ErrorInvalidOrderSubtotalHostReward", "You are no longer eligible for the {0} at the redeemed level. This reward requires an order subtotal between {1} and {2}.", rewardType, rule.Min.ToMoneyString(), rule.Max.ToMoneyString());
					break;
				default:
					message = Translation.GetTerm("InvalidHostRewardRule", "Invalid Host Reward Rule.");
					break;
			}
			return message;
		}

		public virtual BasicResponse ValidateHostessRewards(OrderCustomer orderCustomer)
		{
			BasicResponse response = new BasicResponse() { Success = true };

			if (orderCustomer == null)
				return response;

			int count = 0;

			var orderCustomerWithHostessRewards = orderCustomer.Order.OrderCustomers.Where(oc => oc.OrderItems.Any(h => h.IsHostReward)).ToList();
			if (orderCustomerWithHostessRewards == null || orderCustomerWithHostessRewards.Count == 0)
				return response;

			foreach (var eachOrderCustomer in orderCustomerWithHostessRewards)
			{
				foreach (OrderItem orderItem in eachOrderCustomer.OrderItems.Where(h => h.IsHostReward))
				{
					count++;

					BasicResponse individualResponse = ValidateHostessRewardItem(eachOrderCustomer, orderItem, orderCustomer.Order, false);

					if (!individualResponse.Success)
					{
						//Remove from Order
						response.Success = false;

						string errorMessage = string.Format("{0} {1} {2}", individualResponse.Message, Translation.GetTerm("PleaseRemoveProduct", "Please remove product: "), orderItem.SKU);

						if (count > 1)
						{
							errorMessage = "<br/>" + errorMessage;
						}

						response.Message = response.Message + errorMessage;
					}
				}
			}

			return response;
		}

		/// <summary>
		/// Belcorp wants every item to calculate their shipping total value off of retail.
		/// The change to this method is to change GetAdjustedPrice() to always get the Retail adjusted price
		/// This does not apply to completely free items (Adjustment AddedItem)
		/// </summary>
		/// <param name="kind"></param>
		/// <returns></returns>
		public virtual decimal ParentSubtotalForShippingOrderItemTotalByType(OrderCustomer orderCustomer, Constants.OrderItemType kind)
		{
			var inventory = Create.New<InventoryBaseRepository>();
			var total = 0m;
			foreach (var orderItem in orderCustomer.ParentOrderItems.Where(i => i.OrderItemTypeID == (int)kind
				&& !i.OrderAdjustmentOrderLineModifications.Any(la => la.ModificationOperationID == (int)NetSteps.OrderAdjustments.Common.Model.OrderAdjustmentOrderLineOperationKind.AddedItem)))
			{
				var product = inventory.GetProduct(orderItem.ProductID.Value);
				total += orderItem.ChargeShipping && product.ProductBase.IsShippable ? orderItem.GetOriginalPrice((int)Constants.OrderItemType.Retail) * orderItem.Quantity : 0m;
			}

			return total;
		}
	}
}
