using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NetSteps.Common;
using NetSteps.Common.Configuration;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Common.Validation.NetTiers;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Generated;
using NetSteps.Data.Entities.Interfaces;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Data.Entities.Business.Logic
{
	using NetSteps.Data.Entities.Extensions;

	public partial class PartyBusinessLogic
	{
		public ITotalsCalculator TotalsCalculator { get { return Create.New<ITotalsCalculator>(); } }

		public override List<string> ValidatedChildPropertiesSetByParent(Repositories.IPartyRepository repository)
		{
			return new List<string>() { "OrderID", "AccountID", "AddressID", "OrderItemID", "OrderAdjustmentID" };
		}

		public override void AddValidationRules(Party entity)
		{
			base.AddValidationRules(entity);

			entity.ValidationRules.AddRule(CommonRules.RegexIsMatch,
				new CommonRules.RegexRuleArgs("EviteOrganizerEmail", RegularExpressions.EmailOrEmpty,
					Translation.GetTerm("InvalidEmailErrorMessage", CustomValidationMessages.Email), true));
		}

		public virtual IEnumerable<HostessRewardRule> GetApplicableHostessRewardRules(Party party, int? marketID = null)
		{
			if (party == null)
				return new List<HostessRewardRule>();

			var results = GetPartiallyFilteredApplicableHostessRewardRules(party.Order, marketID);
			results = FilterBookingCreditHostessRewardRules(results, party, marketID);

			return results;
		}

		/// <summary>
		/// Returns the number of guests to use to determine hostess reward eligibility.  Default implementation is as follows: 
		/// Counts the nubmer of customers with non-host reward order items.
		/// Counts the number of online orders that are not returns and are in a commissionable status.
		/// Joins the two collections, and returns the number of distinct account ID's. 
		/// </summary>
		/// <param name="order">The order to calculate the number of eligible customers on</param>
		/// <returns>The number of customers that should be considered when calculating hostess rewards</returns>
		protected virtual int GetEligibleCustomerCountForHostessRewards(Order order)
		{
			var customerIDsWithItems =
				order.OrderCustomers.Where(oc => oc.OrderItems.Any(i => !i.IsHostReward)).Select(oc => oc.AccountID);
			var onlineOrderCustomerIDs =
				order.OnlineChildOrders.Where(
					o => o.OrderTypeID != (int)ConstantsGenerated.OrderType.ReturnOrder && o.IsOrderStatusCommissionable()).
					SelectMany(o => o.OrderCustomers.Select(oc => oc.AccountID));
			var total = customerIDsWithItems.Union(onlineOrderCustomerIDs).Distinct().Count();
			return total;
		}

		protected virtual IList<HostessRewardRule> GetPartiallyFilteredApplicableHostessRewardRules(Order order, int? marketID = null, int? totalBookingCustomers = null)
		{
			if (order == null)
				return new List<HostessRewardRule>();

			NetSteps.Data.Entities.Cache.SmallCollectionCache.HostessRewardRuleCache rules = SmallCollectionCache.Instance.HostessRewardRules;
			var totalHostessRewards = TotalsCalculator.SumHostessRewards(order);
			var totalCustomers = GetEligibleCustomerCountForHostessRewards(order);


			var country = order.GetDefaultShipment().CountryID;
			bool bookingCreditsRulesForChildPartyExist = SmallCollectionCache.Instance.HostessRewardRules.Count(r => r.IsRedeemedAtChildParty) > 0;

			if (totalBookingCustomers == null)
			{
				Party party = null;
				if (order.Parties != null && order.Parties.Count > 0)
					party = order.Parties.FirstOrDefault();

				if (party != null)
				{
					totalBookingCustomers = Party.GetTotalBookedCustomerCountForParty(party.PartyID);
				}
				else
				{
					totalBookingCustomers = 0;
				}
			}

			var results = (country > 0 || marketID.HasValue ? SmallCollectionCache.Instance.HostessRewardRules.Where(r =>
				r.MarketID == (marketID.HasValue ? marketID.Value : SmallCollectionCache.Instance.Countries.GetById(country).MarketID) &&
				DateTime.Now.IsBetween(r.StartDate, r.EndDate)) : new List<HostessRewardRule>()).ToList();



			results = results
				.Where(r =>
						   IsOrderSubtotalRuleWithinAcceptableRange(r, totalHostessRewards, totalCustomers) ||
							   IsCustomerCountRuleForBookingCreditWithinAcceptableRange(r, totalBookingCustomers,
																						totalCustomers,
																						totalHostessRewards) ||
																							IsExclusiveProductsRule(r))
				.ToList();

			return results;
		}

		public bool IsOrderSubtotalRuleWithinAcceptableRange(HostessRewardRule r, decimal totalHostessRewards, decimal totalCustomers)
		{
			return (r.HostessRewardRuleTypeID ==
				(int)Constants.HostessRewardRuleType.OrderSubtotal &&
					IsBetween(IsNullOrLessThan(r.Min, totalHostessRewards),
							  IsNullOrGreaterThan(r.Max, totalHostessRewards)) &&
								  IsNullOrLessThan(r.MinCustomers, totalCustomers) &&
									  IsNullOrLessThan(r.MinOrderSubTotal,
													   totalHostessRewards));
		}

		public bool IsCustomerCountRuleForBookingCreditWithinAcceptableRange(HostessRewardRule r, decimal? totalBookingCustomers, decimal totalCustomers, decimal totalHostessRewards)
		{
			return (r.HostessRewardTypeID == (int)Constants.HostessRewardType.BookingCredit &&
				r.HostessRewardRuleTypeID == (int)Constants.HostessRewardRuleType.CustomerCount &&
					IsNullOrLessThan(r.Min, (decimal)totalBookingCustomers) &&
						IsNullOrGreaterThan(r.Max, totalBookingCustomers.Value) &&
							IsNullOrLessThan(r.MinCustomers, totalCustomers) &&
								IsNullOrLessThan(r.MinOrderSubTotal, totalHostessRewards));
		}

		public bool IsExclusiveProductsRule(HostessRewardRule r)
		{
			return r.HostessRewardRuleTypeID == (int)Constants.HostessRewardRuleType.ExclusiveProducts;
		}

		private bool IsNullOrLessThan(decimal? value, decimal otherValue)
		{
			return (!value.HasValue || otherValue >= value.Value);
		}

		private bool IsNullOrGreaterThan(decimal? value, decimal otherValue)
		{
			return (!value.HasValue || otherValue <= value.Value);
		}

		private bool IsBetween(bool result1, bool result2)
		{
			return result1 && result2;
		}

		protected virtual IList<HostessRewardRule> FilterBookingCreditHostessRewardRules(IList<HostessRewardRule> results, Party party, int? marketID = null)
		{
			bool isChildParty = party.ParentPartyID.HasValue;
			bool bookingCreditsRulesForChildPartyExist = SmallCollectionCache.Instance.HostessRewardRules.Count(r => r.IsRedeemedAtChildParty) > 0;

			if (bookingCreditsRulesForChildPartyExist)
			{
				// Remove all BookingCredit Hostess Rewards Rules from being available at parent parties if they are redeemable at the child party - JHE
				results = results.Where(r => r.HostessRewardTypeID != (int)Constants.HostessRewardType.BookingCredit).ToList();

				if (isChildParty)
				{
					Party parentParty = GetParentPartyFromCache(party);

					var totalHostessRewards = TotalsCalculator.SumHostessRewards(parentParty.Order);

					var parentPartyRules = GetPartiallyFilteredApplicableHostessRewardRules(parentParty.Order);
					parentPartyRules = parentPartyRules.Where(r => r.IsRedeemedAtChildParty).ToList();
					var rule = parentPartyRules.FirstOrDefault(r => r.HostessRewardTypeID == (int)Constants.HostessRewardType.BookingCredit);

					int maxBookingCreditsRedeemable = (rule != null && rule.Products.HasValue) ? rule.Products.Value : 0;
					int totalBookingCreditsRedeemed = Party.GetTotalBookingCreditsRedeemed(parentParty.OrderID);

					var parentPartyHostess = parentParty.Order.GetHostess();
					bool partyContainsHostFromParentParty = party.Order.OrderCustomers.Count(oc => oc.AccountID == parentPartyHostess.AccountID) > 0;

					var parentOrderStatus = SmallCollectionCache.Instance.OrderStatuses.GetById(parentParty.Order.OrderStatusID);
					bool canRedeemBookingCredit = totalBookingCreditsRedeemed < maxBookingCreditsRedeemable && partyContainsHostFromParentParty && parentOrderStatus.IsCommissionable;

					if (canRedeemBookingCredit)
					{
						var partyRules = GetPartiallyFilteredApplicableHostessRewardRules(party.Order, marketID, 1);
						var partyRule = partyRules.FirstOrDefault(r => r.HostessRewardTypeID == (int)Constants.HostessRewardType.BookingCredit &&
							r.IsRedeemedAtChildParty &&
							r.MinOrderSubTotal.IsNullOrLessThan(totalHostessRewards));

						if (partyRule != null && !results.Select(r => r.HostessRewardRuleID).Contains(partyRule.HostessRewardRuleID))
							results.Add(partyRule); // Add back in Rule for child party - JHE
					}
				}
			}
			return results;
		}

		public virtual OrderCustomer GetOrderCustomerForHostessRewardRule(Party party, HostessRewardRule hostessRewardRule)
		{
			var orderCustomer = party.Order.GetHostess();

			if (hostessRewardRule.HostessRewardTypeID == (int)Constants.HostessRewardType.BookingCredit)
			{
				bool isChildParty = party.ParentPartyID.HasValue;
				bool containsBookingCreditsForChildParty = SmallCollectionCache.Instance.HostessRewardRules.Count(r => r.IsRedeemedAtChildParty) > 0;
				if (isChildParty && containsBookingCreditsForChildParty)
				{
					Party parentParty = GetParentPartyFromCache(party);
					var parentPartyHostess = parentParty.Order.GetHostess();
					orderCustomer = party.Order.OrderCustomers.FirstOrDefault(oc => oc.AccountID == parentPartyHostess.AccountID);
				}
			}

			if (hostessRewardRule.HostessRewardTypeID == (int)Constants.HostessRewardType.ExclusiveProduct)
			{
				bool consultantHasCart = party.Order.HasConsultantOnOrder();

				if (consultantHasCart)
				{
					orderCustomer = party.Order.OrderCustomers.FirstOrDefault(oc => oc.AccountID == party.Order.ConsultantID);
				}
				else
				{
					orderCustomer = null;
				}
			}

			return (OrderCustomer)orderCustomer;
		}

		protected virtual Party GetParentPartyFromCache(Party childParty)
		{
			Party parentParty = null;
			if (childParty.ParentPartyID.HasValue && childParty.ParentPartyID.Value > 0)
			{
				string partyOrderKey = string.Format("Party:{0}", childParty.ParentPartyID.Value);
				parentParty = (Party)HttpContext.Current.Cache.Get(partyOrderKey);
				if (parentParty == null)
				{
					// Caching the Parent party for better performance since this method get call frequently - JHE
					parentParty = Party.LoadFull(childParty.ParentPartyID.Value);
					HttpContext.Current.Cache.Insert(partyOrderKey, parentParty, null, DateTime.Now.AddMinutes(1), TimeSpan.Zero);
				}
			}
			return parentParty;
		}

		public virtual DateTime GetMaximumFuturePartyDate(Party party)
		{
			if (party.ParentPartyID.HasValue)
			{
				return DateTime.Now.AddMonths(6); // Default the Max Date of a child party to 6 months out - JHE
			}
			else
				return DateTime.Now.AddYears(50);
		}

		public virtual OrderCustomer GetParentPartyHostess(Party party)
		{
			bool isChildParty = party.ParentPartyID.HasValue;
			if (!isChildParty)
				return null;

			Party parentParty = GetParentPartyFromCache(party);
			var parentPartyHostess = parentParty.Order.GetHostess();

			if (parentPartyHostess != null)
				return party.Order.OrderCustomers.FirstOrDefault(oc => oc.AccountID == parentPartyHostess.AccountID);
			else
				return null;
		}

		/// <summary>
		///add the party information to EventContext and DomainEventQueue table
		/// </summary>
		public void SendReminderToPartyGuests(IEnumerable<Party> parties)
		{
			try
			{
				foreach (var party in parties)
				{
					DomainEventQueueItem.AddPartyGuestOrderReminderEventToQueue(party.PartyID);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}



		/// <summary>
		/// Returns bool if the party has met custom values for required minimums.
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		public virtual bool HasMetTotalAboveMinimumAmount(Party entity)
		{
			return IsOrderTotalAboveMinimumAmount(entity.Order);
		}


		/// <summary>
		/// Throws an exception based on has met total
		/// </summary>
		/// <param name="entity"></param>
		public virtual void IsOrderTotalAboveMinimumAmount(Party entity)
		{
			if (!HasMetTotalAboveMinimumAmount(entity))
			{
				throw new PartyOrderMinimumAmountException();
			}
		}

		public virtual bool IsOrderTotalAboveMinimumAmount(Order entity)
		{
			var isOrderTotalAboveMinimumAmount = true;

			if (OrdersSection.Instance != null)
			{
				var amount = CalculateOrderTotalToCompareToMinimumAmount(entity, ConstantsGenerated.OrderType.OnlineOrder);

				if (entity.OrderTypeID == (short)ConstantsGenerated.OrderType.PartyOrder)
				{
					isOrderTotalAboveMinimumAmount = amount >= OrdersSection.Instance.PartyOrderMinimum;
				}
				else if (entity.OrderTypeID == (int)ConstantsGenerated.OrderType.FundraiserOrder)
				{
					isOrderTotalAboveMinimumAmount = amount >= OrdersSection.Instance.FundraiserOrderMinimum;
				}
			}

			return isOrderTotalAboveMinimumAmount;
		}

		public virtual decimal CalculateOrderTotalToCompareToMinimumAmount(Order entity, ConstantsGenerated.OrderType orderType)
		{
			var orderSubTotal = entity.Subtotal;
			var onlineOrderType = OrderType.LoadAllFull().FirstOrDefault(ot => ot.OrderTypeID == (short)orderType);
			var onlineOrderSubTotal = 0m;

			var onlineOrders = new OrderRepository().LoadOrdersByParentOrderIdAndOrderType(entity.OrderID, onlineOrderType.OrderTypeID).ToList();
			if (onlineOrders.Any())
			{
				onlineOrders = Order.LoadBatch(onlineOrders.Select(o => o.OrderID).ToList());
				onlineOrderSubTotal = onlineOrders.Sum(o => o.Subtotal.ToDecimal());
			}

			return ((orderSubTotal ?? 0) + onlineOrderSubTotal);
		}

		public override Party LoadFull(IPartyRepository repository, int primaryKey)
		{
			return base.LoadFull(repository, primaryKey);
		}
	}
}
