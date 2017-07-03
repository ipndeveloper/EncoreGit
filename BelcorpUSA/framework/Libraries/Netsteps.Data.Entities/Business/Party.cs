using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NetSteps.Common.Base;
using NetSteps.Common.Configuration;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Common.Services;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Encore.Core.IoC;
using NetSteps.Security;

namespace NetSteps.Data.Entities
{

	public partial class Party
	{
		[NonSerialized]
		private IOrderService _orderService;
		public IOrderService OrderService
		{
			get
			{
				if (_orderService == null)
				{
					_orderService = Create.New<IOrderService>();
				}
				return _orderService;
			}
		}

		#region Members
		/// <summary>
		/// Related entities that can be included by the 'Load' methods (see <see cref="LoadRelationsExtensions"/>).
		/// WARNING: Changes to this list will affect various 'Load' methods, be careful.
		/// </summary>
		[Flags]
		public enum Relations
		{
			// These are bit flags so they must be numbered appropriately (eg. 0, 1, 2, 4, 8, 16)
			// Use bit-shifting for convenience (eg. 0, 1 << 0, 1 << 1, 1 << 2)
			None = 0,
			Address = 1 << 0,
			ChildParties = 1 << 1,
			EmailTemplateTokens = 1 << 2,
			Order = 1 << 3,
			OrderFull = 1 << 4,
			OrderWithCustomers = 1 << 5,
			PartyGuests = 1 << 6,
			PartyRsvps = 1 << 7,

			/// <summary>
			/// The default relations used by the 'LoadFull' methods.
			/// </summary>
			LoadFull =
				 Address
				 | EmailTemplateTokens
				 | OrderFull
				 | PartyGuests
				 | PartyRsvps,

			LoadWithGuests =
				 Address
				 | OrderWithCustomers
				 | PartyGuests
				 | PartyRsvps,

			LoadFullWithChildParties =
				 LoadFull
				 | ChildParties,
		};
		#endregion

		private static List<int> openStatuses;
		public static List<int> OpenStatuses()
		{
			if (openStatuses == null)
			{
				openStatuses = new List<int>();
				openStatuses.Add((int)Constants.OrderStatus.CreditCardDeclined);
				openStatuses.Add((int)Constants.OrderStatus.CreditCardDeclinedRetry);
				openStatuses.Add((int)Constants.OrderStatus.NotSet);
				openStatuses.Add((int)Constants.OrderStatus.DeferredOnlinePayment);
				openStatuses.Add((int)Constants.OrderStatus.PartyOrderPending);
				openStatuses.Add((int)Constants.OrderStatus.Pending);
				openStatuses.Add((int)Constants.OrderStatus.PartiallyPaid);
			}

			return openStatuses;
		}

		public static PaginatedList<PartySearchData> Search(PartySearchParameters searchParams)
		{
			try
			{
				return Repository.Search(searchParams);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static List<PartySearchData> GetOpenParties(int accountID)
		{
			return GetOpenParties(accountID, OpenStatuses());
		}

		public static List<PartySearchData> GetOpenParties(int accountID, List<int> orderStatuses = null)
		{
			try
			{
				return Repository.GetOpenParties(accountID, orderStatuses);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static bool HasHostedParties(int accountID)
		{
			try
			{
				return Repository.HasHostedParties(accountID);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static List<Party> GetHostedParties(int accountID)
		{
			try
			{
				return Repository.GetHostedParties(accountID);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		/// <summary>
		/// Method to get the list of parties on specific date
		/// </summary>
		/// <param name="partyEventDate">Party EventDate for which the party list should be fetched</param>
		/// <returns></returns>
		public static List<Party> GetHostedPartiesByDate(DateTime partyEventDate)
		{
			try
			{
				return Repository.GetHostedPartiesByDate(partyEventDate);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		/// <summary>
		/// Method added for adding the party guests info to DomainEventQueue and EventContext table
		/// </summary>
		/// <param name="parties">list of parties </param>
		public static void SendReminderToPartyGuests(IEnumerable<Party> parties)
		{
			try
			{
				BusinessLogic.SendReminderToPartyGuests(parties);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static Party LoadWithGuests(int partyID)
		{
			try
			{
				Party party = Repository.LoadWithGuests(partyID);
				if (party != null)
				{
					party.StartEntityTracking();
					party.EnableLazyLoadingRecursive();
				}
				return party;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static Party LoadFullWithChildParties(int partyID)
		{
			try
			{
				Party party = Repository.LoadFullWithChildParties(partyID);
				if (party != null)
				{
					party.StartEntityTracking();
					party.EnableLazyLoadingRecursive();
				}
				return party;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(
					 ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static Party LoadByOrderID(int orderID)
		{
			try
			{
				Party party = Repository.LoadByOrderID(orderID);
				if (party != null)
				{
					party.StartEntityTracking();
					party.EnableLazyLoadingRecursive();
				}
				return party;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static Party LoadFullByOrderID(int orderID)
		{
			try
			{
				Party party = Repository.LoadFullByOrderID(orderID);
				if (party != null)
				{
					party.StartEntityTracking();
					party.EnableLazyLoadingRecursive();
				}
				return party;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		private static void RefreshOrderItem(OrderItem oldItem, OrderItem newItem)
		{
			newItem.Guid = oldItem.Guid;
			foreach (OrderItem item in oldItem.ChildOrderItems)
			{
				OrderItem refreshedChildItem = newItem.ChildOrderItems.FirstOrDefault(x => x.OrderItemID == item.OrderItemID);
				if (refreshedChildItem != null)
				{
					RefreshOrderItem(item, refreshedChildItem);
				}
			}
		}

		public static Party Refresh(Party currentParty)
		{
			Party refreshedParty = Party.LoadFullByOrderID(currentParty.OrderID);
			foreach (OrderCustomer customer in currentParty.Order.OrderCustomers)
			{
				OrderCustomer refreshedCustomer = refreshedParty.Order.OrderCustomers.FirstOrDefault(x => x.OrderCustomerID == customer.OrderCustomerID);
				if (refreshedCustomer != null)
				{
					refreshedCustomer.Guid = customer.Guid;
					foreach (OrderItem item in customer.OrderItems)
					{
						OrderItem refreshedItem = refreshedCustomer.OrderItems.FirstOrDefault(x => x.OrderItemID == item.OrderItemID);
						if (refreshedItem != null)
						{
							RefreshOrderItem(item, refreshedItem);
						}
					}

					foreach (OrderPayment payment in customer.OrderPayments)
					{
						OrderPayment refreshedPayment = refreshedCustomer.OrderPayments.FirstOrDefault(x => x.OrderPaymentID == payment.OrderPaymentID);
						if (refreshedPayment != null)
						{
							refreshedPayment.Guid = payment.Guid;
						}
					}

					foreach (OrderShipment shipment in customer.OrderShipments)
					{
						OrderShipment refreshedShipment = refreshedCustomer.OrderShipments.FirstOrDefault(x => x.OrderShipmentID == shipment.OrderShipmentID);
						if (refreshedShipment != null)
						{
							refreshedShipment.Guid = shipment.Guid;
						}
					}
				}
			}

			foreach (OrderPayment payment in currentParty.Order.OrderPayments)
			{
				OrderPayment refreshedPayment = refreshedParty.Order.OrderPayments.FirstOrDefault(x => x.OrderPaymentID == payment.OrderPaymentID);
				if (refreshedPayment != null)
				{
					refreshedPayment.Guid = payment.Guid;
				}
			}

			foreach (OrderShipment shipment in currentParty.Order.OrderShipments)
			{
				OrderShipment refreshedShipment = refreshedParty.Order.OrderShipments.FirstOrDefault(x => x.OrderShipmentID == shipment.OrderShipmentID);
				if (refreshedShipment != null)
				{
					refreshedShipment.Guid = shipment.Guid;
				}
			}

			return refreshedParty;
		}

		public static bool IsParty(int orderID)
		{
			try
			{
				return Repository.IsParty(orderID);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public override void Save()
		{
			try
			{
				base.StartEntityTracking();

				if (this.Order == null && this.OrderID == 0)
					throw new Exception("Error saving Party. The Order on Party must be set.");

				bool updateOrder = false;
				bool updateAccounts = false;
				if (this.Order != null)
				{
					if (this.Order.CurrencyID == 0)
					{
						OrderService.SetCurrencyID(Order);
					}

					//New Party set the orderNumber
					if (this.OrderID == 0 || this.Order.OrderNumber.IsNullOrEmpty())
					{
						OrderService.GenerateAndSetNewOrderNumber(Order, false);
					}

					updateOrder = this.Order.OrderNumber.IsNullOrEmpty() || this.Order.OrderNumber.StartsWith("Temp");
				}

				// For Testing to check the number of objects in Party object graph - JHE
				//List<IObjectWithChangeTracker> allTrackerItems = new List<IObjectWithChangeTracker>();
				//IObjectWithChangeTrackerExtensions.GetAllChangeTrackerItems(this, allTrackerItems, true, true);
				//var dups = this.FindDuplicateEntitiesInObjectGraph();

				base.Save();

				if (updateOrder || updateAccounts)
				{
					if (this.Order != null)
					{
						base.StartEntityTracking();
						OrderService.GenerateAndSetNewOrderNumber(Order, false);
						base.Save();
					}
				}
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static string EncryptPartyID(int partyID)
		{
			try
			{
				string obfuscatedPartyID = Encryption.EncryptTripleDES(partyID.ToString());
				return HttpUtility.UrlEncode(obfuscatedPartyID);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static int DecryptPartyID(string encryptedPartyID)
		{
			try
			{
				string decryptedPartyIDString = Encryption.DecryptTripleDES(encryptedPartyID);
				int partyID = decryptedPartyIDString.IsValidInt() ? decryptedPartyIDString.ToInt() : 0;
				return partyID;
			}
			catch (Exception ex)
			{
				EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
				return 0;
			}
		}

		public virtual bool HasMetTotal()
		{
			return BusinessLogic.HasMetTotalAboveMinimumAmount(this);
		}

		public IEnumerable<HostessRewardRule> GetApplicableHostessRewardRules(int? marketID = null)
		{
			try
			{
				return BusinessLogic.GetApplicableHostessRewardRules(this, marketID);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException, this.OrderID.ToIntNullable(), null);
			}
		}

		public static int GetTotalBookingCreditsRedeemed(int parentPartyOrderID)
		{
			try
			{
				return Repository.GetTotalBookingCreditsRedeemed(parentPartyOrderID);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException, null, null);
			}
		}

		public static int GetTotalBookedCustomerCountForParty(int partyID)
		{
			try
			{
				return Repository.GetTotalBookedCustomerCountForParty(partyID);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException, null, null);
			}
		}

		public OrderCustomer GetOrderCustomerForHostessRewardRule(HostessRewardRule hostessRewardRule)
		{
			try
			{
				return BusinessLogic.GetOrderCustomerForHostessRewardRule(this, hostessRewardRule);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException, this.OrderID.ToIntNullable(), null);
			}
		}

		public DateTime GetMaximumFuturePartyDate()
		{
			try
			{
				return BusinessLogic.GetMaximumFuturePartyDate(this);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException, this.OrderID.ToIntNullable(), null);
			}
		}

		public OrderCustomer GetParentPartyHostess()
		{
			try
			{
				return BusinessLogic.GetParentPartyHostess(this);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException, this.OrderID.ToIntNullable(), null);
			}
		}

		public virtual BasicResponse ValidatePartyForSubmission()
		{
			BusinessLogic.IsOrderTotalAboveMinimumAmount(this);
			var orderItems = from orderCustomer in Order.OrderCustomers
							 from orderItem in orderCustomer.OrderItems
							 select orderItem;

			var childPartyItems = from childParty in ChildParties
								  from childOrderCustomer in childParty.Order.OrderCustomers
								  from childOrderItem in childOrderCustomer.OrderItems
								  select childOrderItem;


			if (!orderItems.Any() && !childPartyItems.Any())
			{
				return new BasicResponse()
				{
					Success = false,
					Message = Translation.GetTerm("EmptyPartyCart", "There are no guests with any items in their carts.  Please remove these guests or add items to their cart to continue.")
				};
			}

			return new BasicResponse() { Success = true };
		}

		public virtual bool HasReachedMinimumPartySubtotal
		{
			get
			{
				if (MinimumPartyTotal == 0)
					return true;
				var currentTotal = CalculatePartyTotal();
				return currentTotal >= MinimumPartyTotal;
			}
		}

		public virtual bool IsOpen
		{
			get
			{
				var order = Order ?? new OrderRepository().Load(OrderID);
				return OpenStatuses().Contains(order.OrderStatusID);
			}
		}

		public virtual decimal MinimumPartyTotal
		{
			get
			{
				var partyMinimumAmount = 0m;
				if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["PartyOrderMinimum"]))
					partyMinimumAmount = decimal.Parse(ConfigurationManager.AppSettings["PartyOrderMinimum"]);
				return partyMinimumAmount;
			}
		}

		public virtual decimal CalculatePartyTotal()
		{
			if (Order == null)
			{
				return 0;
			}
			var orderSubTotal = Order.Subtotal;
			var onlineOrderSubTotal = new OrderRepository().LoadOrdersByParentOrderIdAndOrderType(Order.OrderID, (short)Constants.OrderType.OnlineOrder).Sum(o => o.Subtotal.ToDecimal());
			return ((orderSubTotal ?? 0) + onlineOrderSubTotal);
		}
	}
}
