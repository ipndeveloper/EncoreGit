namespace NetSteps.Data.Entities.Extensions
{
	using System;
	using System.Collections.Generic;
	using System.Data.Objects;
	using System.Diagnostics.Contracts;
	using System.Linq;
	using NetSteps.Common.Extensions;

	/// <summary>
	/// Methods that break down the huge EF eager loads into simple, fast queries.
	/// Any additions or changes should be benchmarked for optimal performance.
	/// We use EF "relationship fixup" heavily, so don't be alarmed by all the .ToList()
	/// calls that appear to do nothing. EF is automatically wiring up the object graph.
	/// The basic rule is avoid JOINs unless they clearly do not affect SQL performance. - Lundy
	/// </summary>
	internal static class LoadRelationsExtensions
	{
		#region Account
		/// <summary>
		/// Adds related objects to an account.
		/// Uses a series of small SELECTs for better SQL performance.
		/// </summary>
		public static void LoadRelations(
			this Account account,
			NetStepsEntities context,
			Account.Relations relations = Account.Relations.LoadFull)
		{
			Contract.Requires<ArgumentNullException>(account != null);
			Contract.Requires<ArgumentNullException>(context != null);

			LoadRelations(new[] { account }, context, relations);
		}

		/// <summary>
		/// Adds related objects to a collection of accounts.
		/// Uses a series of small SELECTs for better SQL performance.
		/// </summary>
		public static void LoadRelations(
			this IList<Account> accounts,
			NetStepsEntities context,
			Account.Relations relations = Account.Relations.LoadFull)
		{
			Contract.Requires<ArgumentNullException>(accounts != null);
			Contract.Requires<ArgumentNullException>(context != null);
			Contract.Requires<ArgumentException>(accounts.All(x => x != null));

			if (!accounts.Any())
			{
				return;
			}

			EnsureAllEntitiesAreAttachedToContext(accounts, context, "accounts");

			var accountIDs = accounts
				.Select(a => a.AccountID)
				.ToList();

			// AccountDevices
			if (relations.HasFlag(Account.Relations.AccountDevices))
			{
				context.AccountDevices
					.Where(ad => accountIDs.Contains(ad.AccountID))
					.ToList();
			}

			// AccountPaymentMethods
			// AccountPaymentMethods.BillingAddress
			if (relations.HasFlag(Account.Relations.AccountPaymentMethods)
				|| relations.HasFlag(Account.Relations.AccountPaymentMethods_BillingAddress))
			{
				ObjectQuery<AccountPaymentMethod> query = context.AccountPaymentMethods;

				if (relations.HasFlag(Account.Relations.AccountPaymentMethods_BillingAddress))
					query = query.Include("BillingAddress");

				query
					.Where(apm => accountIDs.Contains(apm.AccountID))
					.ToList();
			}

			// AccountPhones
			if (relations.HasFlag(Account.Relations.AccountPhones))
			{
				context.AccountPhones
					.Where(ap => accountIDs.Contains(ap.AccountID))
					.ToList();
			}

			// AccountProperties
			if (relations.HasFlag(Account.Relations.AccountProperties))
			{
				context.AccountProperties
					.Where(ap => accountIDs.Contains(ap.AccountID))
					.ToList();
			}

			// Addresses
			// Addresses.AddressProperties
			if (relations.HasFlag(Account.Relations.Addresses))
			{
				var addresses = context.Addresses
					.Where(ad => ad.Accounts.Any(a => accountIDs.Contains(a.AccountID)))
					.ToList();

				accounts.LoadManyToMany(
					a => a.Addresses,
					a => a.AccountID,
					ad => ad.AddressID,
					addresses,
					context.Accounts
						.Where(a => accountIDs.Contains(a.AccountID))
						.SelectMany(a => a.Addresses.Select(ad => new ParentKeyChildKey<int, int> { ParentKey = a.AccountID, ChildKey = ad.AddressID }))
				);

				if (relations.HasFlag(Account.Relations.Addresses_AddressProperties))
				{
					addresses.LoadRelations(context, Address.Relations.AddressProperties);
				}
			}

			// CampaignSubscribers
			if (relations.HasFlag(Account.Relations.CampaignSubscribers))
			{
				context.CampaignSubscribers
					.Where(cs => accountIDs.Contains(cs.AccountID))
					.ToList();
			}

			// AccountContactTags
			if (relations.HasFlag(Account.Relations.AccountContactTags))
			{
				context.AccountContactTags
					.Where(ac => ac.AccountID != null && accountIDs.Contains(ac.AccountID.Value))
					.ToList();

			}

			// DistributionSubscribers
			if (relations.HasFlag(Account.Relations.DistributionSubscribers))
			{
				context.DistributionSubscribers
					.Where(ds => ds.AccountID != null && accountIDs.Contains(ds.AccountID.Value))
					.ToList();
			}

			// FileResources
			if (relations.HasFlag(Account.Relations.FileResources))
			{
				accounts.LoadManyToMany(
					a => a.FileResources,
					a => a.AccountID,
					fr => fr.FileResourceID,
					context.FileResources
						.Where(fr => fr.Accounts.Any(a => accountIDs.Contains(a.AccountID))),
					context.Accounts
						.Where(a => accountIDs.Contains(a.AccountID))
						.SelectMany(a => a.FileResources.Select(fr => new ParentKeyChildKey<int, int> { ParentKey = a.AccountID, ChildKey = fr.FileResourceID }))
				);
			}

			// MailAccounts
			if (relations.HasFlag(Account.Relations.MailAccounts))
			{
				context.MailAccounts
					.Where(ma => accountIDs.Contains(ma.AccountID))
					.ToList();
			}

			// Notes
			if (relations.HasFlag(Account.Relations.Notes))
			{
				accounts.LoadManyToMany(
					a => a.Notes,
					a => a.AccountID,
					n => n.NoteID,
					context.Notes
						.Where(n => n.Accounts.Any(a => accountIDs.Contains(a.AccountID))),
					context.Accounts
						.Where(a => accountIDs.Contains(a.AccountID))
						.SelectMany(a => a.Notes.Select(n => new ParentKeyChildKey<int, int> { ParentKey = a.AccountID, ChildKey = n.NoteID }))
				);
			}

			if (relations.HasFlag(Account.Relations.User)
				|| relations.HasFlag(Account.Relations.User_Roles)
				|| relations.HasFlag(Account.Relations.User_Roles_Functions))
			{
				var userIDs = accounts
					.Where(a => a.UserID != null)
					.Select(a => a.UserID.Value)
					.Distinct()
					.ToList();

				if (userIDs.Any())
				{
					// User
					var users = context.Users
						.Where(u => userIDs.Contains(u.UserID))
						.ToList();

					// User.Roles
					if (relations.HasFlag(Account.Relations.User_Roles)
						|| relations.HasFlag(Account.Relations.User_Roles_Functions))
					{
						var roles = context.Roles
							.Where(r => r.Users.Any(u => userIDs.Contains(u.UserID)))
							.ToList();

						users.LoadManyToMany(
							u => u.Roles,
							u => u.UserID,
							r => r.RoleID,
							roles,
							context.Users
								.Where(u => userIDs.Contains(u.UserID))
								.SelectMany(u => u.Roles.Select(r => new ParentKeyChildKey<int, int> { ParentKey = u.UserID, ChildKey = r.RoleID }))
						);

						if (relations.HasFlag(Account.Relations.User_Roles_Functions))
						{
							if (roles.Any())
							{
								// User.Roles.Functions
								var roleIDs = roles
									.Select(r => r.RoleID);

								roles.LoadManyToMany(
									r => r.Functions,
									r => r.RoleID,
									f => f.FunctionID,
									context.Functions
										.Where(f => f.Roles.Any(r => roleIDs.Contains(r.RoleID))),
									context.Roles
										.Where(r => roleIDs.Contains(r.RoleID))
										.SelectMany(r => r.Functions.Select(f => new ParentKeyChildKey<int, int> { ParentKey = r.RoleID, ChildKey = f.FunctionID }))
								);
							}
						}
					}
				}
			}
		}
		#endregion

		#region Address
		/// <summary>
		/// Adds related objects to a collection of autoship orders.
		/// Uses a series of small SELECTs for better SQL performance.
		/// </summary>
		public static void LoadRelations(
			this IList<Address> addresses,
			NetStepsEntities context,
			Address.Relations relations = Address.Relations.LoadFull)
		{
			Contract.Requires<ArgumentNullException>(addresses != null);
			Contract.Requires<ArgumentNullException>(context != null);
			Contract.Requires<ArgumentException>(addresses.All(x => x != null));

			if (!addresses.Any())
			{
				return;
			}

			EnsureAllEntitiesAreAttachedToContext(addresses, context, "addresses");

			// AddressType
			if (relations.HasFlag(Address.Relations.AddressType))
			{
				var addressTypeIDs = addresses
					.Select(a => a.AddressTypeID)
					.Distinct()
					.ToList();

				context.AddressTypes
					.Where(at => addressTypeIDs.Contains(at.AddressTypeID))
					.ToList();
			}

			// AddressProperties
			if (relations.HasFlag(Address.Relations.AddressProperties))
			{
				// Move this out of the "if" block in the future if needed by other queries.
				var addressIDs = addresses
					.Select(ao => ao.AddressID)
					.ToList();

				context.AddressProperties
					.Where(ap => addressIDs.Contains(ap.AddressID))
					.ToList();
			}
		}
		#endregion

		#region AutoshipOrders
		/// <summary>
		/// Adds related objects to an autoship order.
		/// Uses a series of small SELECTs for better SQL performance.
		/// </summary>
		public static void LoadRelations(
			this AutoshipOrder autoshipOrder,
			NetStepsEntities context,
			AutoshipOrder.Relations relations = AutoshipOrder.Relations.LoadFull)
		{
			Contract.Requires<ArgumentNullException>(autoshipOrder != null);
			Contract.Requires<ArgumentNullException>(context != null);

			LoadRelations(new[] { autoshipOrder }, context, relations);
		}

		/// <summary>
		/// Adds related objects to a collection of autoship orders.
		/// Uses a series of small SELECTs for better SQL performance.
		/// </summary>
		public static void LoadRelations(
			this IList<AutoshipOrder> autoshipOrders,
			NetStepsEntities context,
			AutoshipOrder.Relations relations = AutoshipOrder.Relations.LoadFull)
		{
			Contract.Requires<ArgumentNullException>(autoshipOrders != null);
			Contract.Requires<ArgumentNullException>(context != null);
			Contract.Requires<ArgumentException>(autoshipOrders.All(x => x != null));

			if (!autoshipOrders.Any())
			{
				return;
			}

			EnsureAllEntitiesAreAttachedToContext(autoshipOrders, context, "autoshipOrders");

			var autoshipOrderIDs = autoshipOrders
				.Select(ao => ao.AutoshipOrderID)
				.ToList();

			// Account
			if (relations.HasFlag(AutoshipOrder.Relations.Account))
			{
				var accountIDs = autoshipOrders
					.Select(ao => ao.AccountID)
					.Distinct()
					.ToList();

				context.Accounts
					.Where(a => accountIDs.Contains(a.AccountID))
					.ToList();
			}

			// OrderLoadFull
			if (relations.HasFlag(AutoshipOrder.Relations.OrderLoadFull))
			{
				var orderIDs = autoshipOrders
					.Select(ao => ao.TemplateOrderID)
					.Distinct()
					.ToList();

				var orders = context.Orders
					.Where(o => orderIDs.Contains(o.OrderID))
					.ToList();

				orders.LoadRelations(
					context,
					Order.Relations.LoadFullForAutoshipOrder
				);
			}

			if (relations.HasFlag(AutoshipOrder.Relations.Sites))
			{
				// Sites
				var sites = context.Sites
					.Where(s => s.AutoshipOrderID != null && autoshipOrderIDs.Contains(s.AutoshipOrderID.Value))
					.ToList();

				var siteIDs = sites
					.Select(s => s.SiteID)
					.ToList();

				if (siteIDs.Any())
				{
					context.SiteUrls
						.Where(su => siteIDs.Contains(su.SiteID.Value))
						.ToList();
				}
			}
		}
		#endregion

		#region Order
		/// <summary>
		/// Adds related objects to an order.
		/// Uses a series of small SELECTs for better SQL performance.
		/// </summary>
		public static void LoadRelations(
			this Order order,
			NetStepsEntities context,
			Order.Relations relations = Order.Relations.LoadFull)
		{
			Contract.Requires<ArgumentNullException>(order != null);
			Contract.Requires<ArgumentNullException>(context != null);

			LoadRelations(new[] { order }, context, relations);
		}

		/// <summary>
		/// Adds related objects to a collection of orders.
		/// Uses a series of small SELECTs for better SQL performance.
		/// </summary>
		public static void LoadRelations(
			this IList<Order> orders,
			NetStepsEntities context,
			Order.Relations relations = Order.Relations.LoadFull)
		{
			Contract.Requires<ArgumentNullException>(orders != null);
			Contract.Requires<ArgumentNullException>(context != null);
			Contract.Requires<ArgumentException>(orders.All(x => x != null));

			if (!orders.Any())
			{
				return;
			}

			EnsureAllEntitiesAreAttachedToContext(orders, context, "orders");

			var orderIDs = orders
				.Select(o => o.OrderID)
				.ToList();

			// Notes
			if (relations.HasFlag(Order.Relations.Notes))
			{
				orders.LoadManyToMany(
					o => o.Notes,
					o => o.OrderID,
					n => n.NoteID,
					context.Notes
						.Where(n => n.Orders.Any(o => orderIDs.Contains(o.OrderID))),
					context.Orders
						.Where(o => orderIDs.Contains(o.OrderID))
						.SelectMany(o => o.Notes.Select(n => new ParentKeyChildKey<int, int> { ParentKey = o.OrderID, ChildKey = n.NoteID }))
				);
			}

			if (relations.HasFlag(Order.Relations.OrderAdjustments)
				|| relations.HasFlag(Order.Relations.OrderAdjustments_OrderAdjustmentOrderLineModifications)
				|| relations.HasFlag(Order.Relations.OrderAdjustments_OrderAdjustmentOrderModifications))
			{
				// OrderAdjustments
				var orderAdjustments = context.OrderAdjustments
					.Where(oa => orderIDs.Contains(oa.OrderID))
					.ToList();

				if (relations.HasFlag(Order.Relations.OrderAdjustments_OrderAdjustmentOrderLineModifications)
					|| relations.HasFlag(Order.Relations.OrderAdjustments_OrderAdjustmentOrderModifications))
				{
					var orderAdjustmentIDs = orderAdjustments
						.Select(oa => oa.OrderAdjustmentID)
						.ToList();

					if (orderAdjustmentIDs.Any())
					{
						// OrderAdjustments_OrderAdjustmentOrderLineModifications
						if (relations.HasFlag(Order.Relations.OrderAdjustments_OrderAdjustmentOrderLineModifications))
						{
							context.OrderAdjustmentOrderLineModifications
								.Where(oaolm => orderAdjustmentIDs.Contains(oaolm.OrderAdjustmentID))
								.ToList();
						}

						// OrderAdjustments_OrderAdjustmentOrderModifications
						if (relations.HasFlag(Order.Relations.OrderAdjustments_OrderAdjustmentOrderModifications))
						{
							context.OrderAdjustmentOrderModifications
								.Where(oaom => orderAdjustmentIDs.Contains(oaom.OrderAdjustmentID))
								.ToList();
						}
					}
				}
			}

			if (relations.HasFlag(Order.Relations.OrderCustomers)
				|| relations.HasFlag(Order.Relations.OrderCustomers_OrderItems)
				|| relations.HasFlag(Order.Relations.OrderCustomers_OrderItems_GiftCards)
				|| relations.HasFlag(Order.Relations.OrderCustomers_OrderItems_OrderItemMessages)
				|| relations.HasFlag(Order.Relations.OrderCustomers_OrderItems_OrderItemPrices)
				|| relations.HasFlag(Order.Relations.OrderCustomers_OrderItems_OrderItemProperties)
				|| relations.HasFlag(Order.Relations.OrderCustomers_OrderItems_OrderItemProperties_OrderItemPropertyValue)
				|| relations.HasFlag(Order.Relations.OrderCustomers_OrderItems_OrderItemReturns))
			{
				// OrderCustomers
				var orderCustomers = context.OrderCustomers
					.Where(oc => orderIDs.Contains(oc.OrderID))
					.ToList();

				var orderCustomerIDs = orderCustomers
					.Select(oc => oc.OrderCustomerID)
					.ToList();

				if (relations.HasFlag(Order.Relations.OrderCustomers_OrderItems)
					|| relations.HasFlag(Order.Relations.OrderCustomers_OrderItems_GiftCards)
					|| relations.HasFlag(Order.Relations.OrderCustomers_OrderItems_OrderItemMessages)
					|| relations.HasFlag(Order.Relations.OrderCustomers_OrderItems_OrderItemPrices)
					|| relations.HasFlag(Order.Relations.OrderCustomers_OrderItems_OrderItemProperties)
					|| relations.HasFlag(Order.Relations.OrderCustomers_OrderItems_OrderItemProperties_OrderItemPropertyValue)
					|| relations.HasFlag(Order.Relations.OrderCustomers_OrderItems_OrderItemReturns))
				{
					// OrderCustomers_OrderItems
					var orderItems = context.OrderItems
						.Where(oi => orderCustomerIDs.Contains(oi.OrderCustomerID))
						.ToList();

					var orderItemIDs = orderItems
						.Select(oi => oi.OrderItemID)
						.ToList();

					// OrderCustomers_OrderItems_GiftCards
					if (relations.HasFlag(Order.Relations.OrderCustomers_OrderItems_GiftCards))
					{
						context.GiftCards
							.Where(gc => gc.OriginOrderItemID != null && orderItemIDs.Contains(gc.OriginOrderItemID.Value))
							.ToList();
					}

					// Uncomment when this gets to SVN trunk
					// OrderCustomers_OrderItems_OrderItemMessages
					if (relations.HasFlag(Order.Relations.OrderCustomers_OrderItems_OrderItemMessages))
					{
						context.OrderItemMessages
							 .Where(oim => orderItemIDs.Contains(oim.OrderItemID))
							 .ToList();
					}

					// OrderCustomers_OrderItems_OrderItemPrices
					if (relations.HasFlag(Order.Relations.OrderCustomers_OrderItems_OrderItemPrices))
					{
						context.OrderItemPrices
							.Where(oip => orderItemIDs.Contains(oip.OrderItemID))
							.ToList();
					}

					// OrderCustomers_OrderItems_OrderItemProperties
					// OrderCustomers_OrderItems_OrderItemProperties_OrderItemPropertyValue
					if (relations.HasFlag(Order.Relations.OrderCustomers_OrderItems_OrderItemProperties)
						|| relations.HasFlag(Order.Relations.OrderCustomers_OrderItems_OrderItemProperties_OrderItemPropertyValue))
					{
						ObjectQuery<OrderItemProperty> query = context.OrderItemProperties;

						if (relations.HasFlag(Order.Relations.OrderCustomers_OrderItems_OrderItemProperties_OrderItemPropertyValue))
						{
							query = query.Include("OrderItemPropertyValue");
						}

						query
							.Where(oip => orderItemIDs.Contains(oip.OrderItemID))
							.ToList();
					}

					// OrderCustomers_OrderItems_OrderItemReturns
					if (relations.HasFlag(Order.Relations.OrderCustomers_OrderItems_OrderItemReturns))
					{
						context.OrderItemReturns
							.Where(oir => orderItemIDs.Contains(oir.OrderItemID))
							.ToList();
					}
				}
			}

			// OrderPayments
			if (relations.HasFlag(Order.Relations.OrderPayments)
				|| relations.HasFlag(Order.Relations.OrderPayments_OrderPaymentResults))
			{
				var orderPayments = context.OrderPayments
					.Where(op => orderIDs.Contains(op.OrderID))
					.ToList();

				if (orderPayments.Any())
				{
					var orderPaymentIDs = orderPayments
						.Select(op => op.OrderPaymentID)
						.ToList();

					context.OrderPaymentResults
						.Where(opr => orderPaymentIDs.Contains(opr.OrderPaymentID))
						.ToList();
				}
			}

			if (relations.HasFlag(Order.Relations.OrderShipments)
				|| relations.HasFlag(Order.Relations.OrderShipments_OrderShipmentPackages)
				|| relations.HasFlag(Order.Relations.OrderShipments_OrderShipmentPackages_OrderShipmentPackageItems))
			{
				// OrderShipments
				var orderShipments = context.OrderShipments
					.Where(os => orderIDs.Contains(os.OrderID))
					.ToList();

				if (relations.HasFlag(Order.Relations.OrderShipments_OrderShipmentPackages)
					|| relations.HasFlag(Order.Relations.OrderShipments_OrderShipmentPackages_OrderShipmentPackageItems))
				{
					var orderShipmentIDs = orderShipments
						.Select(os => os.OrderShipmentID)
						.ToList();

					// OrderShipments_OrderShipmentPackages
					var orderShipmentPackages = context.OrderShipmentPackages
						.Where(osp => orderShipmentIDs.Contains(osp.OrderShipmentID))
						.ToList();

					if (relations.HasFlag(Order.Relations.OrderShipments_OrderShipmentPackages_OrderShipmentPackageItems))
					{
						var orderShipmentPackageIDs = orderShipmentPackages
							.Select(osp => osp.OrderShipmentPackageID)
							.ToList();

						// OrderShipments_OrderShipmentPackages_OrderShipmentPackageItems
						context.OrderShipmentPackageItems
							.Where(ospi => orderShipmentPackageIDs.Contains(ospi.OrderShipmentPackageID))
							.ToList();
					}
				}
			}

			// AutoshipOrders
			if (relations.HasFlag(Order.Relations.AutoshipOrders))
			{
				context.AutoshipOrders
					.Where(ao => orderIDs.Contains(ao.TemplateOrderID))
					.ToList();
			}
		}
		#endregion

		#region Party
		/// <summary>
		/// Adds related objects to a party.
		/// </summary>
		public static Party LoadRelations(
			this Party party,
			NetStepsEntities context,
			Party.Relations relations = Party.Relations.LoadFull)
		{
			Contract.Requires<ArgumentNullException>(party != null);
			Contract.Requires<ArgumentNullException>(context != null);

			new[] { party }.LoadRelations(context, relations);

			return party;
		}

		/// <summary>
		/// Adds related objects to a collection of parties.
		/// </summary>
		public static IEnumerable<Party> LoadRelations(
			this IEnumerable<Party> parties,
			NetStepsEntities context,
			Party.Relations relations = Party.Relations.LoadFull)
		{
			Contract.Requires<ArgumentNullException>(parties != null);
			Contract.Requires<ArgumentNullException>(context != null);
			Contract.Requires<ArgumentException>(parties.All(x => x != null));

			if (!parties.Any())
			{
				return parties;
			}

			EnsureAllEntitiesAreAttachedToContext(parties, context, "parties");

			// The context passed to each loader.
			var loaderContext = new PartyLoaderContext(parties, context, p => p.PartyID);

			// Iterate through loaders and execute any specified in the 'relations' variable.
			foreach (var partyLoader in _partyLoaders)
			{
				if (relations.HasFlag(partyLoader.Key))
				{
					partyLoader.Value(loaderContext);
				}
			}

			if (relations.HasFlag(Party.Relations.Order)
				|| relations.HasFlag(Party.Relations.OrderFull)
				|| relations.HasFlag(Party.Relations.OrderWithCustomers))
			{
				var orders = Parties_LoadOrders(loaderContext);
				if (relations.HasFlag(Party.Relations.OrderFull))
				{
					orders.LoadRelations(context, Order.Relations.LoadFullForParty);
				}
				else if (relations.HasFlag(Party.Relations.OrderWithCustomers))
				{
					orders.LoadRelations(context, Order.Relations.LoadWithCustomersForParty);
				}
			}

			return parties;
		}

		private interface IPartyLoaderContext : ILoaderContext<Party, int> { }

		private class PartyLoaderContext : LoaderContext<Party, int>, IPartyLoaderContext
		{
			public PartyLoaderContext(IEnumerable<Party> entities, NetStepsEntities dataContext, Func<Party, int> idResolver)
				: base(entities, dataContext, idResolver)
			{ }
		}

		private static readonly IDictionary<Party.Relations, Action<IPartyLoaderContext>> _partyLoaders =
			new Dictionary<Party.Relations, Action<IPartyLoaderContext>>
			{
				{ Party.Relations.Address, Parties_LoadAddresses },
				{ Party.Relations.ChildParties, Parties_LoadChildParties },
				{ Party.Relations.EmailTemplateTokens, Parties_LoadEmailTemplateTokens },
				{ Party.Relations.PartyGuests, Parties_LoadPartyGuests },
				{ Party.Relations.PartyRsvps, Parties_LoadPartyRsvps },
			};

		private static void Parties_LoadAddresses(IPartyLoaderContext loaderContext)
		{
			var addressIDs = loaderContext.Entities
				.Where(p => p.AddressID.HasValue)
				.Select(p => p.AddressID.Value)
				.Distinct()
				.ToList();

			if (addressIDs.Any())
			{
				loaderContext.DataContext.Addresses
					.Where(a => addressIDs.Contains(a.AddressID))
					.ToList();
			}
		}

		private static void Parties_LoadChildParties(IPartyLoaderContext loaderContext)
		{
			var childParties = loaderContext.DataContext.Parties
				.Where(p => loaderContext.EntityIds.Contains(p.ParentPartyID.Value))
				.ToList();

			var childPartyOrderIDs = childParties
				.Select(p => p.OrderID)
				.Distinct()
				.ToList();

			if (childPartyOrderIDs.Any())
			{
				loaderContext.DataContext.Orders
					.Where(o => childPartyOrderIDs.Contains(o.OrderID))
					.ToList()
					.LoadRelations(loaderContext.DataContext, Order.Relations.LoadWithCustomersForParty);
			}
		}

		private static void Parties_LoadEmailTemplateTokens(IPartyLoaderContext loaderContext)
		{
			loaderContext.DataContext.EmailTemplateTokens
				.Where(ett => loaderContext.EntityIds.Contains(ett.PartyID.Value))
				.ToList();
		}

		private static void Parties_LoadPartyGuests(IPartyLoaderContext loaderContext)
		{
			loaderContext.DataContext.PartyGuests
				.Where(pg => loaderContext.EntityIds.Contains(pg.PartyID))
				.ToList();
		}

		private static void Parties_LoadPartyRsvps(IPartyLoaderContext loaderContext)
		{
			loaderContext.DataContext.PartyRsvps
				.Where(pr => loaderContext.EntityIds.Contains(pr.PartyID))
				.ToList();
		}

		private static List<Order> Parties_LoadOrders(IPartyLoaderContext loaderContext)
		{
			var orderIDs = loaderContext.Entities
				.Select(p => p.OrderID)
				.Distinct()
				.ToList();

			if (!orderIDs.Any())
			{
				return new List<Order>();
			}

			return loaderContext.DataContext.Orders
				.Where(o => orderIDs.Contains(o.OrderID) || (o.ParentOrderID.HasValue && orderIDs.Contains(o.ParentOrderID.Value)))
				.ToList();
		}
		#endregion

		#region Site

		/// <summary>
		/// Adds related objects to a site.
		/// </summary>
		public static Site LoadRelations(
			this Site site,
			NetStepsEntities context,
			Site.Relations relations = Site.Relations.LoadFull)
		{
			Contract.Requires<ArgumentNullException>(site != null);
			Contract.Requires<ArgumentNullException>(context != null);

			LoadRelations(new[] { site }, context, relations);

			return site;
		}

		/// <summary>
		/// Adds related objects to a collection of sites.
		/// </summary>
		public static IList<Site> LoadRelations(
			this IList<Site> sites,
			NetStepsEntities context,
			Site.Relations relations = Site.Relations.LoadFull)
		{
			Contract.Requires<ArgumentNullException>(sites != null);
			Contract.Requires<ArgumentNullException>(context != null);
			Contract.Requires<ArgumentException>(sites.All(x => x != null));

			if (!sites.Any())
			{
				return sites;
			}

			EnsureAllEntitiesAreAttachedToContext(sites, context, "sites");

			// The context passed to each loader.
			var loaderContext = new SiteLoaderContext
			{
				DataContext = context,
				Entities = sites,
				EntityIds = sites
					.Select(s => s.SiteID)
					.ToList(),
				HtmlSectionIDs = new List<int>()
			};

			// Iterate through loaders and execute any specified in the 'relations' variable.
			foreach (var siteLoader in _siteLoaders)
			{
				if (relations.HasFlag(siteLoader.Key))
				{
					siteLoader.Value(loaderContext);
				}
			}

			// Load all referenced content in one call.
			if (relations.HasFlag(Site.Relations.HtmlSections)
				|| loaderContext.HtmlSectionIDs.Any())
			{
				var includeSiteContents = relations.HasFlag(Site.Relations.HtmlSections);

				Sites_LoadHtmlSectionContents(
					loaderContext,
					includeSiteContents
				);
			}

			return sites;
		}

		private interface ISiteLoaderContext : ILoaderContext<Site, int>
		{
			/// <summary>
			/// A list of all HtmlSectionIDs loaded so we can get their content in a single query at the end of the loads.
			/// </summary>
			IList<int> HtmlSectionIDs { get; }
		}

		private class SiteLoaderContext : LoaderContext<Site, int>, ISiteLoaderContext
		{
			/// <summary>
			/// A list of all HtmlSectionIDs loaded so we can get their content in a single query at the end of the loads.
			/// </summary>
			public IList<int> HtmlSectionIDs { get; set; }
		}

		private static readonly IDictionary<Site.Relations, Action<ISiteLoaderContext>> _siteLoaders =
			new Dictionary<Site.Relations, Action<ISiteLoaderContext>>
			{
				{ Site.Relations.Account, Sites_LoadAccounts },
				{ Site.Relations.Archives, Sites_LoadArchives },
				{ Site.Relations.CalendarEvents, Sites_LoadCalendarEvents },
				{ Site.Relations.HtmlSections, Sites_LoadHtmlSections },
				{ Site.Relations.Languages, Sites_LoadLanguages },
				{ Site.Relations.Navigations, Sites_LoadNavigations },
				{ Site.Relations.News, Sites_LoadNews },
				{ Site.Relations.Pages, Sites_LoadPages },
				{ Site.Relations.SiteSettingValues, Sites_LoadSiteSettingValues },
				{ Site.Relations.SiteSettings, Sites_LoadSiteSettings },
				{ Site.Relations.SiteType, Sites_LoadSiteTypes },
				{ Site.Relations.SiteUrls, Sites_LoadSiteUrls },
				{ Site.Relations.SiteWidgets, Sites_LoadSiteWidgets },
			};

		private static void Sites_LoadAccounts(ISiteLoaderContext loaderContext)
		{
			var accountIDs = loaderContext.Entities
				.Where(s => s.AccountID.HasValue)
				.Select(s => s.AccountID.Value)
				.Distinct()
				.ToList();

			if (accountIDs.Any())
			{
				var accounts = loaderContext.DataContext.Accounts
					.Where(a => accountIDs.Contains(a.AccountID))
					.ToList();

				var userIDs = accounts
					.Where(a => a.UserID.HasValue)
					.Select(a => a.UserID.Value)
					.Distinct()
					.ToList();

				loaderContext.DataContext.Users
					.Where(u => userIDs.Contains(u.UserID))
					.ToList();
			}
		}

		private static void Sites_LoadArchives(ISiteLoaderContext loaderContext)
		{
			var archives = loaderContext.DataContext.Archives
				.Where(a => a.Sites.Any(s => loaderContext.EntityIds.Contains(s.SiteID)))
				.ToList();

			loaderContext.Entities.LoadManyToMany(
				s => s.Archives,
				s => s.SiteID,
				a => a.ArchiveID,
				archives,
				loaderContext.DataContext.Sites
					.Where(s => loaderContext.EntityIds.Contains(s.SiteID))
					.SelectMany(s => s.Archives.Select(a => new ParentKeyChildKey<int, int> { ParentKey = s.SiteID, ChildKey = a.ArchiveID }))
			);

			var archiveIDs = archives
				.Select(a => a.ArchiveID)
				.ToList();

			archives.LoadManyToMany(
				a => a.Translations,
				a => a.ArchiveID,
				dt => dt.DescriptionTranslationID,
				loaderContext.DataContext.DescriptionTranslations
					.Where(dt => dt.Archives
						.Any(a => archiveIDs.Contains(a.ArchiveID))
					),
				loaderContext.DataContext.Archives
					.Where(a => archiveIDs.Contains(a.ArchiveID))
					.SelectMany(a => a.Translations.Select(dt => new ParentKeyChildKey<int, int> { ParentKey = a.ArchiveID, ChildKey = dt.DescriptionTranslationID }))
			);
		}

		private static void Sites_LoadCalendarEvents(ISiteLoaderContext loaderContext)
		{
			var calendarEvents = loaderContext.DataContext.CalendarEvents
				.Include("HtmlSection")
				.Where(ce => ce.Sites.Any(s => loaderContext.EntityIds.Contains(s.SiteID)))
				.ToList();

			loaderContext.Entities.LoadManyToMany(
				s => s.CalendarEvents,
				s => s.SiteID,
				ce => ce.CalendarEventID,
				calendarEvents,
				loaderContext.DataContext.Sites
					.Where(s => loaderContext.EntityIds.Contains(s.SiteID))
					.SelectMany(s => s.CalendarEvents.Select(ce => new ParentKeyChildKey<int, int> { ParentKey = s.SiteID, ChildKey = ce.CalendarEventID }))
			);

			loaderContext.HtmlSectionIDs.AddRange(calendarEvents
				.Where(ce => ce.HtmlSectionID.HasValue)
				.Select(ce => ce.HtmlSectionID.Value)
			);
		}

		private static void Sites_LoadHtmlSections(ISiteLoaderContext loaderContext)
		{
			var htmlSections = loaderContext.DataContext.HtmlSections
				.Where(hs => hs.Sites.Any(s => loaderContext.EntityIds.Contains(s.SiteID)))
				.ToList();

			loaderContext.Entities.LoadManyToMany(
				s => s.HtmlSections,
				s => s.SiteID,
				hs => hs.HtmlSectionID,
				htmlSections,
				loaderContext.DataContext.Sites
					.Where(s => loaderContext.EntityIds.Contains(s.SiteID))
					.SelectMany(s => s.HtmlSections.Select(hs => new ParentKeyChildKey<int, int>
					{
						ParentKey = s.SiteID,
						ChildKey = hs.HtmlSectionID
					}))
			);

			loaderContext.HtmlSectionIDs.AddRange(htmlSections
				.Select(hs => hs.HtmlSectionID)
			);

			// Load HtmlSectionChoices without HtmlElements (they will load at the end of this method).
			loaderContext.DataContext.HtmlSectionChoices
				.Include("HtmlContent")
				.Where(hsc => loaderContext.EntityIds.Contains(hsc.SiteID))
				.ToList();
		}

		private static void Sites_LoadLanguages(ISiteLoaderContext loaderContext)
		{
			loaderContext.Entities.LoadManyToMany(
				s => s.Languages,
				s => s.SiteID,
				l => l.LanguageID,
				loaderContext.DataContext.Languages
					.Where(l => l.Sites.Any(s => loaderContext.EntityIds.Contains(s.SiteID)) || l.Sites1.Any(sl => loaderContext.EntityIds.Contains(sl.SiteID))),
				loaderContext.DataContext.Sites
					.Where(s => loaderContext.EntityIds.Contains(s.SiteID))
					.SelectMany(s => s.Languages.Select(l => new ParentKeyChildKey<int, int> { ParentKey = s.SiteID, ChildKey = l.LanguageID }))
			);
		}

		private static void Sites_LoadNavigations(ISiteLoaderContext loaderContext)
		{
			var navigations = loaderContext.DataContext.Navigations
				.Where(n => loaderContext.EntityIds.Contains(n.SiteID.Value))
				.ToList();

			var navigationIDs = navigations
				.Select(n => n.NavigationID)
				.ToList();

			loaderContext.DataContext.NavigationTranslations
				.Where(nt => navigationIDs.Contains(nt.NavigationID))
				.ToList();
		}

		private static void Sites_LoadNews(ISiteLoaderContext loaderContext)
		{
			var news = loaderContext.DataContext.News
				.Include("HtmlSection")
				.Where(n => n.Sites.Any(s => loaderContext.EntityIds.Contains(s.SiteID)))
				.ToList();

			loaderContext.Entities.LoadManyToMany(
				s => s.News,
				s => s.SiteID,
				n => n.NewsID,
				news,
				loaderContext.DataContext.Sites
					.Where(s => loaderContext.EntityIds.Contains(s.SiteID))
					.SelectMany(s => s.News.Select(n => new ParentKeyChildKey<int, int> { ParentKey = s.SiteID, ChildKey = n.NewsID }))
			);

			loaderContext.HtmlSectionIDs.AddRange(news
				.Where(n => n.HtmlSectionID.HasValue)
				.Select(n => n.HtmlSectionID.Value)
			);
		}

		private static void Sites_LoadPages(ISiteLoaderContext loaderContext)
		{
			var pages = loaderContext.DataContext.Pages
				.Where(p => loaderContext.EntityIds.Contains(p.SiteID.Value))
				.ToList();

			if (pages.Any())
			{
				var pageIDs = pages
					.Select(p => p.PageID)
					.ToList();

				var pageHtmlSections = loaderContext.DataContext.HtmlSections
					.Where(hs => hs.Pages.Any(p => pageIDs.Contains(p.PageID)))
					.ToList();

				pages.LoadManyToMany(
					p => p.HtmlSections,
					p => p.PageID,
					hs => hs.HtmlSectionID,
					pageHtmlSections,
					loaderContext.DataContext.Pages
						.Where(p => pageIDs.Contains(p.PageID))
						.SelectMany(p => p.HtmlSections.Select(hs => new ParentKeyChildKey<int, int> { ParentKey = p.PageID, ChildKey = hs.HtmlSectionID }))
				);

				loaderContext.HtmlSectionIDs.AddRange(pageHtmlSections
					.Select(hs => hs.HtmlSectionID)
				);

				loaderContext.DataContext.PageTranslations
					.Where(pt => pageIDs.Contains(pt.PageID))
					.ToList();

				var pageTypeIDs = pages
					.Select(p => p.PageTypeID)
					.Distinct()
					.ToList();

				var pageTypes = loaderContext.DataContext.PageTypes
					.Where(pt => pageTypeIDs.Contains(pt.PageTypeID))
					.ToList();

				pageTypes.LoadManyToMany(
					pt => pt.Layouts,
					pt => pt.PageTypeID,
					l => l.LayoutID,
					loaderContext.DataContext.Layouts
						.Where(l => l.PageTypes.Any(pt => pageTypeIDs.Contains(pt.PageTypeID))),
					loaderContext.DataContext.PageTypes
						.Where(pt => pageTypeIDs.Contains(pt.PageTypeID))
						.SelectMany(pt => pt.Layouts.Select(l => new ParentKeyChildKey<int, int> { ParentKey = pt.PageTypeID, ChildKey = l.LayoutID }))
				);
			}
		}

		private static void Sites_LoadSiteSettingValues(ISiteLoaderContext loaderContext)
		{
			loaderContext.DataContext.SiteSettingValues
				.Where(ssv => loaderContext.EntityIds.Contains(ssv.SiteID))
				.ToList();
		}

		private static void Sites_LoadSiteSettings(ISiteLoaderContext loaderContext)
		{
			loaderContext.DataContext.SiteSettings
				.Where(ss => loaderContext.EntityIds.Contains(ss.BaseSiteID.Value))
				.ToList();
		}

		private static void Sites_LoadSiteTypes(ISiteLoaderContext loaderContext)
		{
			var siteTypeIDs = loaderContext.Entities
				.Select(s => s.SiteTypeID)
				.Distinct()
				.ToList();

			var siteTypes = loaderContext.DataContext.SiteTypes
					  .Where(st => siteTypeIDs.Contains(st.SiteTypeID))
					  .ToList();

			siteTypes.LoadManyToMany(
			  st => st.NavigationTypes,
			  st => st.SiteTypeID,
			  nt => nt.NavigationTypeID,
			  loaderContext.DataContext.NavigationTypes.Where(nt => nt.SiteTypes.Any(st => siteTypeIDs.Contains(st.SiteTypeID))),
			  loaderContext.DataContext.SiteTypes.Where(st => siteTypeIDs.Contains(st.SiteTypeID)).SelectMany(st => st.NavigationTypes.Select(nt => new ParentKeyChildKey<int, int> { ParentKey = st.SiteTypeID, ChildKey = nt.NavigationTypeID }))
			);
		}

		private static void Sites_LoadSiteUrls(ISiteLoaderContext loaderContext)
		{
			loaderContext.DataContext.SiteUrls
				.Where(su => loaderContext.EntityIds.Contains(su.SiteID.Value))
				.ToList();
		}

		private static void Sites_LoadSiteWidgets(ISiteLoaderContext loaderContext)
		{
			var siteWidgets = loaderContext.DataContext.SiteWidgets
				.Where(sw => loaderContext.EntityIds.Contains(sw.SiteID))
				.ToList();

			if (siteWidgets.Any())
			{
				var widgetIDs = siteWidgets
					.Select(sw => sw.WidgetID)
					.Distinct()
					.ToList();

				loaderContext.DataContext.Widgets
					.Where(w => widgetIDs.Contains(w.WidgetID))
					.ToList();
			}
		}

		private static void Sites_LoadHtmlSectionContents(ISiteLoaderContext loaderContext, bool includeSiteContents)
		{
			var htmlSectionIDs = loaderContext.HtmlSectionIDs
				.Distinct()
				.ToList();

			// The queries are a little different depending on whether 'includeSiteContents' is true.
			if (includeSiteContents)
			{
				loaderContext.DataContext.HtmlSectionContents
			.Include("HtmlContent")
			.Where(hsc =>
				loaderContext.EntityIds.Contains(hsc.SiteID.Value)
			)
			.ToList();

				loaderContext.DataContext.HtmlElements
					.Where(he =>
						he.HtmlContent.HtmlSectionContents.Any(hsc =>
							loaderContext.EntityIds.Contains(hsc.SiteID.Value)
						)
						|| he.HtmlContent.HtmlSectionChoices.Any(hsc =>
							loaderContext.EntityIds.Contains(hsc.SiteID)
						)
					)
					.ToList();
			}
			else if (htmlSectionIDs.Any())
			{
				loaderContext.DataContext.HtmlSectionContents
					.Include("HtmlContent")
					.Where(hsc =>
						htmlSectionIDs.Contains(hsc.HtmlSectionID) && loaderContext.EntityIds.Contains(hsc.SiteID.Value)
					)
					.ToList();

				loaderContext.DataContext.HtmlElements
					.Where(he =>
						he.HtmlContent.HtmlSectionContents.Any(hsc =>
							htmlSectionIDs.Contains(hsc.HtmlSectionID) && loaderContext.EntityIds.Contains(hsc.SiteID.Value)
						)
					)
					.ToList();
			}
		}

		#endregion

		#region ProductBase

		/// <summary>
		/// Adds related objects to a ProductBase.
		/// </summary>
		public static ProductBase LoadRelations(
			this ProductBase productBase,
			NetStepsEntities context,
			ProductBase.Relations relations = ProductBase.Relations.LoadFull)
		{
			Contract.Requires<ArgumentNullException>(productBase != null);
			Contract.Requires<ArgumentNullException>(context != null);

			new[] { productBase }.LoadRelations(context, relations);

			return productBase;
		}

		/// <summary>
		/// Adds related objects to a collection of ProductBases.
		/// </summary>
		public static IEnumerable<ProductBase> LoadRelations(
			this IEnumerable<ProductBase> productBases,
			NetStepsEntities context,
			ProductBase.Relations relations = ProductBase.Relations.LoadFull)
		{
			Contract.Requires<ArgumentNullException>(productBases != null);
			Contract.Requires<ArgumentNullException>(context != null);
			Contract.Requires<ArgumentException>(productBases.All(x => x != null));

			productBases.LoadRelations(context, relations, "productBases", _productBaseLoaders, pb => pb.ProductBaseID);

			return productBases;
		}

		private static readonly IDictionary<ProductBase.Relations, Action<ILoaderContext<ProductBase, int>>> _productBaseLoaders =
			new Dictionary<ProductBase.Relations, Action<ILoaderContext<ProductBase, int>>>
			{
				{ProductBase.Relations.Categories, ProductBases_LoadCategories}
				, {ProductBase.Relations.Translations, ProductBases_LoadTranslations}
				, {ProductBase.Relations.Products, ProductBases_LoadProducts}
				, {ProductBase.Relations.ProductsFull, ProductBases_LoadProductsFull}
				, {ProductBase.Relations.ProductBaseProperties, ProductBases_LoadProductBaseProperties}
				, {ProductBase.Relations.ExcludedStateProvinces, ProductBases_LoadExcludedStateProvinces}
				, {ProductBase.Relations.ProductTypes, ProductBases_LoadProductTypes}
				 
			};


		private static void ProductBases_LoadCategories(ILoaderContext<ProductBase, int> loaderContext)
		{
			var dataContext = loaderContext.DataContext;
			var prodBaseIds = loaderContext.EntityIds.Sort();
			var prodBases = loaderContext.Entities;

			var categories = dataContext.Categories
				.Where(cat => cat.ProductBases.Any(pb => prodBaseIds.Contains(pb.ProductBaseID))).ToArray();

			prodBases.LoadManyToMany(
				pb => pb.Categories,
				pb => pb.ProductBaseID,
				cat => cat.CategoryID,
				categories,
				dataContext.ProductBases
					.Where(pb => prodBaseIds.Contains(pb.ProductBaseID))
					.SelectMany(pb => pb.Categories.Select(cat => new ParentKeyChildKey<int, int>
					{
						ParentKey = pb.ProductBaseID,
						ChildKey = cat.CategoryID
					}))
			);

			var categoryIds = categories.Select(cat => cat.CategoryID).Sort().ToArray();
			dataContext.CategoryTranslations.Where(ct => categoryIds.Contains(ct.CategoryID)).ToArray();
		}

		private static void ProductBases_LoadTranslations(ILoaderContext<ProductBase, int> loaderContext)
		{
			var dataContext = loaderContext.DataContext;
			var prodBaseIds = loaderContext.EntityIds.Sort();
			var prodBases = loaderContext.Entities;

			var translations = dataContext.DescriptionTranslations
				.Where(desc => desc.ProductBases.Any(pb => prodBaseIds.Contains(pb.ProductBaseID))).ToArray();

			prodBases.LoadManyToMany(
				pb => pb.Translations,
				pb => pb.ProductBaseID,
				desc => desc.DescriptionTranslationID,
				translations,
				dataContext.ProductBases
					.Where(pb => prodBaseIds.Contains(pb.ProductBaseID))
					.SelectMany(pb => pb.Translations.Select(desc => new ParentKeyChildKey<int, int>
					{
						ParentKey = pb.ProductBaseID,
						ChildKey = desc.DescriptionTranslationID
					}))
			);
		}

		private static void ProductBases_LoadExcludedStateProvinces(ILoaderContext<ProductBase, int> loaderContext)
		{
			var dataContext = loaderContext.DataContext;
			var prodBaseIds = loaderContext.EntityIds.Sort();
			var prodBases = loaderContext.Entities;

			var excludedStateProvinces = dataContext.StateProvinces
				.Where(sp => sp.ExcludedProductBases.Any(pb => prodBaseIds.Contains(pb.ProductBaseID))).ToArray();

			prodBases.LoadManyToMany(
				pb => pb.ExcludedStateProvinces,
				pb => pb.ProductBaseID,
				sp => sp.StateProvinceID,
				excludedStateProvinces,
				dataContext.ProductBases
					.Where(pb => prodBaseIds.Contains(pb.ProductBaseID))
					.SelectMany(pb => pb.ExcludedStateProvinces.Select(sp => new ParentKeyChildKey<int, int>
					{
						ParentKey = pb.ProductBaseID,
						ChildKey = sp.StateProvinceID
					}))
			);
		}

		private static void ProductBases_LoadProducts(ILoaderContext<ProductBase, int> loaderContext)
		{
			loaderContext.DataContext.Products.Where(p => loaderContext.EntityIds.Contains(p.ProductBaseID)).ToArray();
		}

		private static void ProductBases_LoadProductsFull(ILoaderContext<ProductBase, int> loaderContext)
		{
			var dataContext = loaderContext.DataContext;
			var prodBaseIds = loaderContext.EntityIds.Sort();
			var prodBases = loaderContext.Entities;

			var prods = dataContext.Products.Where(p => loaderContext.EntityIds.Contains(p.ProductBaseID)).ToArray();

			prodBases.LoadManyToMany(
				pb => pb.Products,
				pb => pb.ProductBaseID,
				p => p.ProductID,
				prods,
				dataContext.ProductBases
					.Where(pb => prodBaseIds.Contains(pb.ProductBaseID))
					.SelectMany(pb => pb.Products.Select(p => new ParentKeyChildKey<int, int>
					{
						ParentKey = pb.ProductBaseID,
						ChildKey = p.ProductID
					}))
			);

			if (prods != null && prods.Any())
				prods.LoadRelations(dataContext, Product.Relations.LoadForBase);
		}

		private static void ProductBases_LoadProductBaseProperties(ILoaderContext<ProductBase, int> loaderContext)
		{
			var dataContext = loaderContext.DataContext;
			var prodBaseIds = loaderContext.EntityIds.Sort();

			var prodBaseProps = dataContext.ProductBaseProperties.Where(pbp => prodBaseIds.Contains(pbp.ProductBaseID)).ToArray();

			if (prodBaseProps != null && prodBaseProps.Any())
			{
				var propTypeIds = prodBaseProps.Select(pbp => pbp.ProductPropertyTypeID).Distinct().Sort().ToArray();
				dataContext.ProductPropertyTypes.Where(ppt => propTypeIds.Contains(ppt.ProductPropertyTypeID)).ToArray();
				dataContext.ProductPropertyValues.Where(ppv => propTypeIds.Contains(ppv.ProductPropertyTypeID)).ToArray();
				dataContext.ProductBasePropertyValues.Where(pbpv => loaderContext.EntityIds.Contains(pbpv.ProductBaseID)).ToArray();
			}
		}

		private static void ProductBases_LoadProductTypes(ILoaderContext<ProductBase, int> loaderContext)
		{
			var dataContext = loaderContext.DataContext;
			var prodBaseIds = loaderContext.EntityIds.Sort();
			var prodTypeIds = loaderContext.Entities.Select(pb => pb.ProductTypeID).ToArray();

			if (prodTypeIds != null && prodTypeIds.Any())
			{
				var prodTypes = dataContext.ProductTypes.Where(pt => prodTypeIds.Contains(pt.ProductTypeID)).ToArray();
				if (prodTypes != null && prodTypes.Any())
				{
					var prodPropTypes = dataContext.ProductTypes.Where(pt => prodTypeIds.Contains(pt.ProductTypeID))
						.SelectMany(pt => pt.ProductPropertyTypes)
						.DistinctBy(ppt => ppt.ProductPropertyTypeID)
						.ToArray();

					prodTypes.LoadManyToMany(
						pt => pt.ProductPropertyTypes,
						pt => pt.ProductTypeID,
						ppt => ppt.ProductPropertyTypeID,
						prodPropTypes,
						dataContext.ProductTypes
							.Where(pt => prodTypeIds.Contains(pt.ProductTypeID))
							.SelectMany(pt => pt.ProductPropertyTypes.Select(ppt => new ParentKeyChildKey<int, int>
							{
								ParentKey = pt.ProductTypeID,
								ChildKey = ppt.ProductPropertyTypeID
							}))
						);
				}
			}
		}

		#endregion

		#region Product

		/// <summary>
		/// Adds related objects to a ProductBase.
		/// </summary>
		public static Product LoadRelations(
			this Product product,
			NetStepsEntities context,
			Product.Relations relations = Product.Relations.LoadFull)
		{
			Contract.Requires<ArgumentNullException>(product != null);
			Contract.Requires<ArgumentNullException>(context != null);

			new[] { product }.LoadRelations(context, relations);

			return product;
		}

		/// <summary>
		/// Adds related objects to a collection of Products.
		/// </summary>
		public static IList<Product> LoadRelations(
			this IList<Product> products,
			NetStepsEntities context,
			Product.Relations relations = Product.Relations.LoadFull)
		{
			Contract.Requires<ArgumentNullException>(products != null);
			Contract.Requires<ArgumentNullException>(context != null);
			Contract.Requires<ArgumentException>(products.All(x => x != null));

			products.LoadRelations(context, relations, "products", _productLoaders, p => p.ProductID);

			return products;
		}

		private static readonly IDictionary<Product.Relations, Action<ILoaderContext<Product, int>>> _productLoaders =
			new Dictionary<Product.Relations, Action<ILoaderContext<Product, int>>>
			{
				{Product.Relations.ProductBase, Products_LoadProductBase}
				, {Product.Relations.Prices, Products_LoadPrices}
				, {Product.Relations.Files, Products_LoadFiles}
				, {Product.Relations.Properties, Products_LoadProperties}
				, {Product.Relations.CatalogItems, Products_LoadCatalogItems}
				, {Product.Relations.WarehouseProducts, Products_LoadWarehouseProducts}
				, {Product.Relations.ChildProductRelations, Products_LoadChildProductRelations}
				, {Product.Relations.ParentProductRelations, Products_LoadParentProductRelations}
				, {Product.Relations.DynamicKits, Products_LoadDynamicKits}
				, {Product.Relations.Translations, Products_LoadTranslations}
				, {Product.Relations.ProductVariantInfo, Products_LoadProductVariantInfo}
				, {Product.Relations.ExcludedShippingMethods, Products_LoadExcludedShippingMethods}				 
			};

		private static void Products_LoadProductBase(ILoaderContext<Product, int> loaderContext)
		{
			var dataContext = loaderContext.DataContext;

			var prodBaseIds = loaderContext.Entities.Select(p => p.ProductBaseID).Sort().ToArray();
			var prodBases = dataContext.ProductBases.Where(pb => prodBaseIds.Contains(pb.ProductBaseID)).ToArray();
			if (prodBases != null && prodBases.Any())
				prodBases.LoadRelations(dataContext, ProductBase.Relations.LoadForProducts);
		}

		private static void Products_LoadPrices(ILoaderContext<Product, int> loaderContext)
		{
			var dataContext = loaderContext.DataContext;
			var productIds = loaderContext.EntityIds.Sort();

			var prodPrices = dataContext.ProductPrices.Where(pp => productIds.Contains(pp.ProductID)).ToArray();
			if (prodPrices != null && prodPrices.Any())
			{
				var priceTypeIds = prodPrices.Select(pp => pp.ProductPriceTypeID).Distinct().Sort().ToArray();
				dataContext.ProductPriceTypes.Where(ppt => priceTypeIds.Contains(ppt.ProductPriceTypeID)).ToArray();
			}
		}

		private static void Products_LoadFiles(ILoaderContext<Product, int> loaderContext)
		{
			var dataContext = loaderContext.DataContext;
			var productIds = loaderContext.EntityIds.Sort();

			var prodFiles = dataContext.ProductFiles.Where(pf => productIds.Contains(pf.ProductID)).ToArray();
			if (prodFiles != null && prodFiles.Any())
			{
				var fileTypeIds = prodFiles.Select(pf => pf.ProductFileTypeID).Distinct().Sort().ToArray();
				dataContext.ProductFileTypes.Where(pft => fileTypeIds.Contains(pft.ProductFileTypeID)).ToArray();
			}
		}

		private static void Products_LoadProperties(ILoaderContext<Product, int> loaderContext)
		{
			var dataContext = loaderContext.DataContext;
			var productIds = loaderContext.EntityIds.Sort();

			var prodProps = dataContext.ProductProperties.Where(pp => productIds.Contains(pp.ProductID)).ToArray();
			if (prodProps != null && prodProps.Any())
			{
				var propTypeIds = prodProps.Select(pp => pp.ProductPropertyTypeID).Distinct().Sort().ToArray();
				var propValIds = prodProps.Where(pp => pp.ProductPropertyValueID.HasValue).Select(pp => pp.ProductPropertyValueID.Value).Distinct().Sort().ToArray();
				dataContext.ProductPropertyTypes.Where(ppt => propTypeIds.Contains(ppt.ProductPropertyTypeID)).ToArray();
				dataContext.ProductPropertyValues.Where(ppv => propValIds.Contains(ppv.ProductPropertyValueID)).ToArray();
			}
		}

		private static void Products_LoadCatalogItems(ILoaderContext<Product, int> loaderContext)
		{
			var dataContext = loaderContext.DataContext;
			var productIds = loaderContext.EntityIds.Sort();

			var catItems = dataContext.CatalogItems.Where(ci => productIds.Contains(ci.ProductID)).ToArray();
			if (catItems != null && catItems.Any())
			{
				var catalogIds = catItems.Select(ci => ci.CatalogID).Distinct().Sort().ToArray();
				var catalogs = dataContext.Catalogs.Where(c => catalogIds.Contains(c.CatalogID)).ToArray();

				var storefronts = dataContext.StoreFronts
					.Where(sf => sf.Catalogs.Any(c => catalogIds.Contains(c.CatalogID))).ToArray();

				catalogs.LoadManyToMany(
				c => c.StoreFronts,
				c => c.CatalogID,
				sf => sf.StoreFrontID,
				storefronts,
				dataContext.Catalogs
					.Where(c => catalogIds.Contains(c.CatalogID))
					.SelectMany(c => c.StoreFronts.Select(sf => new ParentKeyChildKey<int, int>
					{
						ParentKey = c.CatalogID,
						ChildKey = sf.StoreFrontID
					}))
				);

				var translations = dataContext.DescriptionTranslations
					.Where(desc => desc.Catalogs.Any(c => catalogIds.Contains(c.CatalogID))).ToArray();

				catalogs.LoadManyToMany(
					c => c.Translations,
					c => c.CatalogID,
					desc => desc.DescriptionTranslationID,
					translations,
					dataContext.Catalogs
						.Where(c => catalogIds.Contains(c.CatalogID))
						.SelectMany(cat => cat.Translations.Select(desc => new ParentKeyChildKey<int, int>
						{
							ParentKey = cat.CatalogID,
							ChildKey = desc.DescriptionTranslationID
						}))
				);
			}
		}

		private static void Products_LoadWarehouseProducts(ILoaderContext<Product, int> loaderContext)
		{
			var dataContext = loaderContext.DataContext;
			var productIds = loaderContext.EntityIds.Sort();

			var wareProds = dataContext.WarehouseProducts.Where(wp => productIds.Contains(wp.ProductID)).ToArray();
			if (wareProds != null && wareProds.Any())
			{
				var wareIds = wareProds.Select(wp => wp.WarehouseID).Distinct().Sort().ToArray();
				dataContext.Warehouses.Where(w => wareIds.Contains(w.WarehouseID)).ToArray();
			}
		}

		private static void Products_LoadChildProductRelations(ILoaderContext<Product, int> loaderContext)
		{
			var dataContext = loaderContext.DataContext;
			var productIds = loaderContext.EntityIds.Sort();

			var children = dataContext.ProductRelations.Where(pr => productIds.Contains(pr.ParentProductID)).ToArray();
			if (children != null && children.Any())
			{
				var relTypeIds = children.Select(c => c.ProductRelationsTypeID).Distinct().Sort().ToArray();
				var childProdIds = children.Select(c => c.ChildProductID).Distinct().Sort().ToArray();

				dataContext.ProductRelationsTypes.Where(prt => relTypeIds.Contains(prt.ProductRelationTypeID)).ToArray();
				dataContext.Products.Where(p => childProdIds.Contains(p.ProductID)).ToArray();
			}
		}

		private static void Products_LoadParentProductRelations(ILoaderContext<Product, int> loaderContext)
		{
			var dataContext = loaderContext.DataContext;
			var productIds = loaderContext.EntityIds.Sort();

			var parents = dataContext.ProductRelations.Where(pr => productIds.Contains(pr.ChildProductID)).ToArray();
			if (parents != null && parents.Any())
			{
				var relTypeIds = parents.Select(c => c.ProductRelationsTypeID).Distinct().Sort().ToArray();
				var parentsProdIds = parents.Select(c => c.ParentProductID).Distinct().Sort().ToArray();

				dataContext.ProductRelationsTypes.Where(prt => relTypeIds.Contains(prt.ProductRelationTypeID)).ToArray();
				dataContext.Products.Where(p => parentsProdIds.Contains(p.ProductID)).ToArray();
			}
		}

		private static void Products_LoadDynamicKits(ILoaderContext<Product, int> loaderContext)
		{
			var dataContext = loaderContext.DataContext;
			var productIds = loaderContext.EntityIds.Sort();
			var products = loaderContext.Entities;

			dataContext.DynamicKitGroupRules.Where(dkg => dkg.ProductID.HasValue && productIds.Contains(dkg.ProductID.Value)).ToArray();
			var dynKits = dataContext.DynamicKits.Where(dk => productIds.Contains(dk.ProductID)).ToArray();
			if (dynKits != null && dynKits.Any())
			{
				var dynKitIds = dynKits.Select(dk => dk.DynamicKitID).Distinct().Sort().ToArray();
				var dynKitGroups = dataContext.DynamicKitGroups.Where(dkg => dynKitIds.Contains(dkg.DynamicKitID)).ToArray();
				var dynKitGroupIds = dynKitGroups.Select(dk => dk.DynamicKitGroupID).Distinct().Sort().ToArray();

				var dynKitGroupRules = dataContext.DynamicKitGroupRules.Where(dkgr => dynKitGroupIds.Contains(dkgr.DynamicKitGroupID)).ToArray();

				var translations = dataContext.DescriptionTranslations
					.Where(desc => desc.DynamicKitGroups.Any(dkg => dynKitGroupIds.Contains(dkg.DynamicKitGroupID))).ToArray();

				dynKitGroups.LoadManyToMany(
					dkg => dkg.Translations,
					dkg => dkg.DynamicKitGroupID,
					desc => desc.DescriptionTranslationID,
					translations,
					dataContext.DynamicKitGroups
						.Where(dkg => dynKitGroupIds.Contains(dkg.DynamicKitGroupID))
						.SelectMany(dkg => dkg.Translations.Select(desc => new ParentKeyChildKey<int, int>
						{
							ParentKey = dkg.DynamicKitGroupID,
							ChildKey = desc.DescriptionTranslationID
						}))
				);
			}
		}

		private static void Products_LoadTranslations(ILoaderContext<Product, int> loaderContext)
		{
			var dataContext = loaderContext.DataContext;
			var productIds = loaderContext.EntityIds.Sort();
			var products = loaderContext.Entities;

			var translations = dataContext.DescriptionTranslations
				.Where(desc => desc.Products.Any(p => productIds.Contains(p.ProductID))).ToArray();

			products.LoadManyToMany(
				p => p.Translations,
				p => p.ProductID,
				desc => desc.DescriptionTranslationID,
				translations,
				dataContext.Products
					.Where(p => productIds.Contains(p.ProductID))
					.SelectMany(p => p.Translations.Select(desc => new ParentKeyChildKey<int, int>
					{
						ParentKey = p.ProductID,
						ChildKey = desc.DescriptionTranslationID
					}))
			);
		}

		private static void Products_LoadProductVariantInfo(ILoaderContext<Product, int> loaderContext)
		{
			loaderContext.DataContext.ProductVariantInfos.Where(pvi => loaderContext.EntityIds.Contains(pvi.ProductID)).ToArray();
		}

		private static void Products_LoadExcludedShippingMethods(ILoaderContext<Product, int> loaderContext)
		{
			var dataContext = loaderContext.DataContext;
			var productIds = loaderContext.EntityIds.Sort();
			var products = loaderContext.Entities;

			var shippingMethods = dataContext.ShippingMethods
				.Where(sm => sm.ExcludedProducts.Any(p => productIds.Contains(p.ProductID))).ToArray();

			products.LoadManyToMany(
					p => p.ExcludedShippingMethods,
					p => p.ProductID,
					sm => sm.ShippingMethodID,
					shippingMethods,
					dataContext.Products
						.Where(p => productIds.Contains(p.ProductID))
						.SelectMany(p => p.ExcludedShippingMethods.Select(sm => new ParentKeyChildKey<int, int>
						{
							ParentKey = p.ProductID,
							ChildKey = sm.ShippingMethodID
						}))
				);
		}

		#endregion

		#region Helpers
		/// <summary>
		/// Adds related objects to a collection of entities.
		/// </summary>
		public static IEnumerable<T> LoadRelations<T, E, TKey>(
			this IEnumerable<T> entities
			, NetStepsEntities context
			, E relations
			, string entitiesName
			, IDictionary<E, Action<ILoaderContext<T, TKey>>> loaders
			, Func<T, TKey> idResolver)
			where T : class, IObjectWithChangeTracker
			where E : struct, IComparable, IConvertible, IFormattable
		{
			Contract.Requires<ArgumentNullException>(entities != null);
			Contract.Requires<ArgumentNullException>(context != null);
			Contract.Requires<ArgumentException>(typeof(E).IsEnum);
			Contract.Requires<ArgumentException>(entities.All(ent => ent != null));

			if (!entities.Any())
			{
				return entities;
			}

			EnsureAllEntitiesAreAttachedToContext(entities, context, entitiesName);

			// The context passed to each loader.
			var loaderContext = new LoaderContext<T, TKey>(entities, context, idResolver);

			// Iterate through loaders and execute any specified in the 'relations' variable.
			if (loaders != null && loaders.Count > 0)
			{
				foreach (var loader in loaders)
				{
					var loaderKey = Convert.ToInt32(loader.Key);
					var relationFlags = Convert.ToInt32(relations);

					if ((loaderKey & relationFlags) == loaderKey)
					{
						loader.Value(loaderContext);
					}
				}
			}

			return entities;
		}

		public interface ILoaderContext<T, TKeyType>
		{
			NetStepsEntities DataContext { get; }
			IEnumerable<T> Entities { get; }
			IEnumerable<TKeyType> EntityIds { get; }
		}

		public class LoaderContext<T, TKeyType> : ILoaderContext<T, TKeyType>
		{
			public NetStepsEntities DataContext { get; set; }
			public IEnumerable<T> Entities { get; set; }
			public IEnumerable<TKeyType> EntityIds { get; internal set; }

			public LoaderContext() { }

			public LoaderContext(IEnumerable<T> entities, NetStepsEntities dataContext, Func<T, TKeyType> idResolver)
			{
				DataContext = dataContext;
				Entities = entities;
				EntityIds = entities.Select(idResolver).ToArray();
			}
		}

		/// <summary>
		/// A helper class for the LoadManyToMany method.
		/// </summary>
		public class ParentKeyChildKey<TParentKey, TChildKey>
		{
			public TParentKey ParentKey { get; set; }
			public TChildKey ChildKey { get; set; }
		}

		/// <summary>
		/// Wires up child entities that have a many-to-many relation to their parents.
		/// Adds child entities to their parents' collection and EF fixup automatically
		/// makes the corresponding change to the child's collection.
		/// </summary>
		public static void LoadManyToMany<TParent, TChild, TParentKey, TChildKey>(
			this IEnumerable<TParent> parents,
			Func<TParent, IList<TChild>> childCollectionSelector,
			Func<TParent, TParentKey> parentKeySelector,
			Func<TChild, TChildKey> childKeySelector,
			IEnumerable<TChild> children,
			IEnumerable<ParentKeyChildKey<TParentKey, TChildKey>> parentChildKeys)
		{
			Contract.Requires<ArgumentNullException>(parents != null);
			Contract.Requires<ArgumentNullException>(childCollectionSelector != null);
			Contract.Requires<ArgumentNullException>(parentKeySelector != null);
			Contract.Requires<ArgumentNullException>(childKeySelector != null);
			Contract.Requires<ArgumentNullException>(children != null);
			Contract.Requires<ArgumentNullException>(parentChildKeys != null);

			if (!parents.Any())
			{
				return;
			}

			// This will execute the children query (if necessary).
			var childrenDict = children
				.ToDictionary(childKeySelector);

			if (!childrenDict.Any())
			{
				return;
			}

			// This will execute the parentChildKeys query BEFORE calling GroupBy().
			var parentChildKeysDict = parentChildKeys
				.GroupBy(x => x.ParentKey)
				.ToDictionary(x => x.Key, g => g.Select(x => x.ChildKey));

			foreach (var parent in parents)
			{
				var parentKey = parentKeySelector(parent);
				IEnumerable<TChildKey> childKeys;
				if (parentChildKeysDict.TryGetValue(parentKey, out childKeys))
				{
					foreach (var childKey in childKeys)
					{
						TChild child;
						if (childrenDict.TryGetValue(childKey, out child))
						{
							childCollectionSelector(parent).Add(child);
						}
					}
				}
			}
		}

		/// <summary>
		/// Throws an exception if any entities are not attached to the given <see cref="ObjectContext"/>.
		/// </summary>
		private static void EnsureAllEntitiesAreAttachedToContext(
			IEnumerable<object> entities,
			ObjectContext context,
			string entitiesName)
		{
			Contract.Requires<ArgumentNullException>(entities != null);
			Contract.Requires<ArgumentNullException>(context != null);
			Contract.Requires<ArgumentNullException>(entitiesName != null);
			Contract.Requires<ArgumentNullException>(entitiesName.Length > 0);

			if (!context.IsAttachedToAll(entities))
			{
				throw new ArgumentException(string.Format("One or more of the {0} are not attached to the object context.", entitiesName));
			}
		}
		#endregion
	}
}
