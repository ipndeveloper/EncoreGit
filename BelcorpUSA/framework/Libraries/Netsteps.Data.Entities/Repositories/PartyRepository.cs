using System;
using System.Collections.Generic;
using System.Data;
using System.Data.EntityClient;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using NetSteps.Common.Base;
using NetSteps.Common.Configuration;
using NetSteps.Common.Exceptions;
using NetSteps.Common.Extensions;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using EventRepository = global::NetSteps.Events.Common.Repositories;

namespace NetSteps.Data.Entities.Repositories
{
	using NetSteps.Orders.Common.Models;
    using NetSteps.Encore.Core.IoC;
    
    [ContainerRegister(typeof(EventRepository.IPartyRepository), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
	public partial class PartyRepository : EventRepository.IPartyRepository
	{
		protected override Func<NetStepsEntities, IQueryable<Party>> loadAllFullQuery
		{
			get
			{
				return CompiledQuery.Compile<NetStepsEntities, IQueryable<Party>>(
					(context) => context.Parties.Include("Address")
											.Include("EmailTemplateTokens")
											.Include("PartyGuests")
											.Include("PartyRsvps")
											.Include("Order")
											.Include("Order.Notes")
											.Include("Order.OrderPayments")
											.Include("Order.OrderPayments.OrderPaymentResults")
											.Include("Order.OrderCustomers")
                                            .Include("Order.ChildOrders")
						//.Include("Order.OrderCustomers.Account")
											.Include("Order.OrderAdjustments")
											.Include("Order.OrderCustomers.OrderPayments")
											.Include("Order.OrderCustomers.OrderPayments.OrderPaymentResults")
						//.Include("Order.OrderCustomers.OrderPayments.Order")
											.Include("Order.OrderCustomers.OrderItems")
											.Include("Order.OrderCustomers.OrderItems.OrderItemPrices")
											.Include("Order.OrderCustomers.OrderItems.OrderItemReturns")
											.Include("Order.OrderCustomers.OrderItems.OrderItemProperties")
											.Include("Order.OrderCustomers.OrderItems.OrderItemProperties.OrderItemPropertyValue")
											.Include("Order.OrderCustomers.OrderItems.GiftCards")
						//.Include("Order.OrderCustomers.OrderItems.Product")
						//.Include("Order.OrderCustomers.OrderItems.OrderShipmentItems")
											.Include("Order.OrderCustomers.OrderShipments")
											.Include("Order.OrderCustomers.OrderShipments.OrderShipmentPackages")
											.Include("Order.OrderShipments")
											.Include("Order.OrderShipments.OrderShipmentPackages")
											.Include("Order.OrderAdjustments.OrderAdjustmentOrderModifications")
											.Include("Order.OrderAdjustments.OrderAdjustmentOrderLineModifications"));
			}
		}

		[EdmFunction("ZriiDbModel.Store", "ToBigInt")]
		public static long ToBigInt(string value)
		{
			throw new NotSupportedException("Direct calls are not supported.");
		}

		public PaginatedList<PartySearchData> Search(PartySearchParameters searchParams)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					PaginatedList<PartySearchData> results = new PaginatedList<PartySearchData>(searchParams);
					IQueryable<Party> parties = context.Parties;

					if (searchParams.OrderStatusID.HasValue)
						parties = parties.Where(p => p.Order.OrderStatusID == searchParams.OrderStatusID.Value);
					else if (searchParams.OrderStatuses != null)
						parties = parties.Where(p => searchParams.OrderStatuses.Contains(p.Order.OrderStatusID));

					if (searchParams.ExcludedOrderTypes != null && searchParams.ExcludedOrderTypes.Any())
						parties = parties.Where(p => !searchParams.ExcludedOrderTypes.Contains(p.Order.OrderTypeID));

					if (searchParams.AcountID.HasValue)
						parties = parties.Where(p => p.Order.ConsultantID == searchParams.AcountID.Value);

					if (searchParams.NumberOfOpenDays.HasValue)
						parties = parties.Where(p => p.Order.DateCreated.AddDays(searchParams.NumberOfOpenDays.Value).EndOfDay() <= DateTime.Today.EndOfDay());

					if (searchParams.WhereClause != null)
						parties = parties.Where(searchParams.WhereClause);

					results.TotalCount = parties.Count();

					if (!string.IsNullOrEmpty(searchParams.OrderBy))
					{
						switch (searchParams.OrderBy)
						{
							case "OrderNumber":
							case "Order.OrderNumber":
								if (ConfigurationManager.GetAppSetting<bool>(ConfigurationManager.VariableKey.OrderNumbersEqualIdentity))
									parties = parties.ApplyOrderByFilter(searchParams.OrderByDirection, p => p.OrderID);
								else if (ConfigurationManager.GetAppSetting<bool>(ConfigurationManager.VariableKey.OrderNumbersAreNumeric))
									parties = parties.ApplyOrderByFilter(searchParams.OrderByDirection, p => ToBigInt(p.Order.OrderNumber));
								else
									parties = parties.ApplyOrderByFilter(searchParams, context);
								break;
							default:
								parties = parties.ApplyOrderByFilter(searchParams, context);
								break;
						}
					}
					else
						parties = parties.OrderBy(p => p.PartyID);

					var selectedParties = parties.Select(p => new
					{
						p.PartyID,
						p.Name,
						p.StartDateUTC,
						p.OrderID,
						p.Order.OrderNumber,
						p.Order.OrderStatusID,
						p.Order.CurrencyID,
						p.Order.GrandTotal,
						p.UseEvites,
						Hostess = p.Order.OrderCustomers
							.Where(oc => oc.OrderCustomerTypeID == (int)Constants.OrderCustomerType.Hostess)
							.Select(oc => context.Accounts
								.Where(a => a.AccountID == oc.AccountID)
								.Select(a => new
								{
									a.FirstName,
									a.LastName
								}).FirstOrDefault()
							).FirstOrDefault()
					});

					if (!string.IsNullOrEmpty(searchParams.OrderBy))
					{
						switch (searchParams.OrderBy)
						{
							case "HostessName":
							case "HostName":
								selectedParties = searchParams.OrderByDirection == Constants.SortDirection.Ascending
									? selectedParties.OrderBy(p => p.Hostess.FirstName).ThenBy(p => p.Hostess.LastName)
									: selectedParties.OrderByDescending(p => p.Hostess.FirstName).ThenByDescending(p => p.Hostess.LastName);
								break;
						}
					}

					selectedParties = selectedParties.ApplyPagination(searchParams);

					var partyInfos = selectedParties.ToList();

					results.AddRange(partyInfos.Select(p => new PartySearchData()
					{
						PartyID = p.PartyID,
						Name = p.Name,
						OrderID = p.OrderID,
						OrderNumber = p.OrderNumber,
						OrderStatusID = p.OrderStatusID,
						StartDate = p.StartDateUTC.UTCToLocal(),
						CurrencyID = p.CurrencyID,
						Total = p.GrandTotal,
						UseEvites = p.UseEvites,
						HostFirstName = p.Hostess != null ? p.Hostess.FirstName : "",
						HostLastName = p.Hostess != null ? p.Hostess.LastName : ""
					}));

					return results;
				}
			});
		}

		public List<PartySearchData> GetOpenParties(int accountID, List<int> statuses)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					var openParties = context.Parties.Where(p => p.ShowOnPWS && (!statuses.Any() || statuses.Contains(p.Order.OrderStatusID)) && p.Order.ConsultantID == accountID);

					var partyInfos = openParties.OrderByDescending(p => p.StartDateUTC).Select(p => new
					{
						p.PartyID,
						p.Name,
						p.StartDateUTC,
						p.OrderID,
						p.Order.OrderNumber,
						p.Order.OrderStatusID,
						p.Order.CurrencyID,
						p.Order.GrandTotal,
						p.UseEvites,
						Hostess = p.Order.OrderCustomers
							.Where(oc => oc.OrderCustomerTypeID == (int)Constants.OrderCustomerType.Hostess)
							.Select(oc => context.Accounts
								.Where(a => a.AccountID == oc.AccountID)
								.Select(a => new
								{
									a.FirstName,
									a.LastName
								}).FirstOrDefault()
							).FirstOrDefault()
					}).ToList();

					return partyInfos.Select(p => new PartySearchData()
					{
						PartyID = p.PartyID,
						Name = p.Name,
						OrderID = p.OrderID,
						OrderNumber = p.OrderNumber,
						OrderStatusID = p.OrderStatusID,
						StartDate = p.StartDateUTC.UTCToLocal(),
						CurrencyID = p.CurrencyID,
						Total = p.GrandTotal,
						UseEvites = p.UseEvites,
						HostFirstName = p.Hostess != null ? p.Hostess.FirstName : "",
						HostLastName = p.Hostess != null ? p.Hostess.LastName : ""
					}).ToList();
				}
			});
		}

		public bool HasHostedParties(int accountID)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					return context.Parties.Any(p => p.Order.OrderCustomers.Any(oc => oc.AccountID == accountID && oc.OrderCustomerTypeID == (int)Constants.OrderCustomerType.Hostess));
				}
			});
		}

		public List<Party> GetHostedParties(int accountID)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					return context.Parties.Include("PartyGuests").Include("PartyRsvps").Include("Order").Where(p => p.Order.OrderCustomers.Any(oc => oc.AccountID == accountID && oc.OrderCustomerTypeID == (int)Constants.OrderCustomerType.Hostess)).ToList();
				}
			});
		}

		/// <summary>
		/// Method to list out the properties greater than offSet date
		/// </summary>
		/// <param name="daysOffSet">date time</param>
		/// <returns>returns the party list whoose start date is less than dayoffSet date</returns>
		public List<Party> GetHostedPartiesByDate(DateTime dateOffSet)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					//Take the parties oldern than 4(configurable) days.ie all the parties happend exactly 4 days back  and should b greater than 5 day back
					var recordsPickEndTime = dateOffSet.Date.AddDays(-1).Date.Midnight();
					var recordsPickStartTime = dateOffSet.Date.Midnight();
					var newPartiesList = context.Parties.Where(p => p.StartDateUTC >= recordsPickEndTime && p.StartDateUTC <= recordsPickStartTime).ToList();
					return newPartiesList;
				}
			});
		}

		public Party LoadWithGuests(int partyID)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					return context.Parties.Include("PartyGuests")
						.Include("PartyRsvps")
						.Include("Order")
						.Include("Order.OrderCustomers")
						.Include("Address")
						.FirstOrDefault(p => p.PartyID == partyID);
				}
			});
		}

		public Party LoadFullWithChildParties(int partyID)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					return context.Parties.Include("Address")
										.Include("EmailTemplateTokens")
										.Include("PartyGuests")
										.Include("PartyRsvps")
										.Include("ChildParties")
										.Include("ChildParties.Order")
										.Include("ChildParties.Order.OrderCustomers")
										.Include("Order")
										.Include("Order.Notes")
										.Include("Order.OrderPayments")
										.Include("Order.OrderPayments.OrderPaymentResults")
										.Include("Order.OrderCustomers")
										.Include("Order.OrderAdjustments")
										.Include("Order.OrderCustomers.OrderPayments")
										.Include("Order.OrderCustomers.OrderPayments.OrderPaymentResults")
										.Include("Order.OrderCustomers.OrderItems")
										.Include("Order.OrderCustomers.OrderItems.OrderItemPrices")
										.Include("Order.OrderCustomers.OrderItems.OrderItemReturns")
										.Include("Order.OrderCustomers.OrderItems.OrderItemProperties")
										.Include("Order.OrderCustomers.OrderItems.OrderItemProperties.OrderItemPropertyValue")
										.Include("Order.OrderCustomers.OrderItems.GiftCards")
										.Include("Order.OrderCustomers.OrderShipments")
										.Include("Order.OrderCustomers.OrderShipments.OrderShipmentPackages")
										.Include("Order.OrderShipments")
										.Include("Order.OrderShipments.OrderShipmentPackages")
										.Include("Order.OrderAdjustments.OrderAdjustmentOrderModifications")
										.Include("Order.OrderAdjustments.OrderAdjustmentOrderLineModifications")
										.FirstOrDefault(p => p.PartyID == partyID);
				}
			});
		}

		public Party LoadFullByOrderID(int orderID)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					return loadAllFullQuery(context).FirstOrDefault(p => p.OrderID == orderID);
				}
			});
		}

		public bool IsParty(int orderID)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					return context.Parties.Any(p => p.OrderID == orderID);
				}
			});
		}

		public override void Save(Party party)
		{
			base.Save(party);

			NotifyCommissionsOfOrderChanges(party.Order);
		}

		protected void NotifyCommissionsOfOrderChanges(Order order)
		{
			if (order != null)
			{
				IDbCommand dbCommand = null;

				try
				{
					dbCommand = DataAccess.SetCommand("UspInsertOrdersToProcess", connectionString: GetConnectionString());
					DataAccess.AddInputParameter("OrderID", order.OrderID, dbCommand);
					DataAccess.ExecuteNonQuery(dbCommand);
				}
				finally
				{
					DataAccess.Close(dbCommand);
				}
			}
		}

		protected static string _connectionString = string.Empty;
		internal static string GetConnectionString()
		{
			if (_connectionString.IsNullOrEmpty())
			{
				using (NetStepsEntities context = CreateContext())
				{
					IDbConnection conn = (context.Connection as EntityConnection).StoreConnection;
					_connectionString = conn.ConnectionString;
				}
			}

			return _connectionString;
		}

		public int GetTotalBookingCreditsRedeemed(int parentPartyOrderID)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					var parentPartyInfo = context.Parties.Where(p => p.OrderID == parentPartyOrderID).Select(p => new
					{
						p.PartyID,
						p.ParentPartyID
					}).FirstOrDefault();
					var parentPartyId = parentPartyInfo.PartyID;
					bool isChildParty = parentPartyInfo.ParentPartyID.HasValue;
					bool bookingCreditsRulesForChildPartyExist = SmallCollectionCache.Instance.HostessRewardRules.Count(r => r.IsRedeemedAtChildParty) > 0;
					int bookingCreditItemTypeId = Constants.OrderItemType.BookingCredit.ToShort();

					int total = 0;
					if (bookingCreditsRulesForChildPartyExist && isChildParty)
					{
						var results = context.OrderItems.Where(oi =>
							oi.OrderCustomer.Order.Parties.Any(p => p.ParentPartyID == parentPartyId) &&
							oi.OrderItemTypeID == bookingCreditItemTypeId &&
							oi.OrderCustomer.Order.OrderStatus.IsCommissionable).ToList();
						total = results.Sum(oi => oi.Quantity);
					}
					else
					{
						var results = context.OrderItems.Where(oi =>
							oi.OrderCustomer.Order.Parties.Any(p => p.PartyID == parentPartyId) &&
							oi.OrderItemTypeID == bookingCreditItemTypeId &&
							oi.OrderCustomer.Order.OrderStatus.IsCommissionable).ToList();
						total = results.Sum(oi => oi.Quantity);
					}
					return total;
				}
			});
		}

		public int GetTotalBookedCustomerCountForParty(int partyID)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					int total = context.Parties.Count(p => p.ParentPartyID == partyID);
					return total;
				}
			});
		}

		public Party LoadByOrderID(int orderID)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					return context.Parties.FirstOrDefault(p => p.OrderID == orderID);
				}
			});
		}

		#region Load Helpers
		public override Party LoadFull(int partyID)
		{
			var party = FirstOrDefaultFull(x => x.PartyID == partyID);

			if (party == null)
			{
				throw new NetStepsDataException(string.Format("No Party found with PartyID = {0}.", partyID));
			}

			return party;
		}

		public override List<Party> LoadBatchFull(IEnumerable<int> partyIDs)
		{
			return WhereFull(x => partyIDs.Contains(x.PartyID));
		}

		public override List<Party> LoadAllFull()
		{
			return WhereFull(x => true);
		}

		public virtual Party FirstOrDefaultFull(Expression<Func<Party, bool>> predicate)
		{
			Contract.Requires<ArgumentNullException>(predicate != null);

			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					return FirstOrDefaultFull(predicate, context);
				}
			});
		}

		public virtual Party FirstOrDefaultFull(Expression<Func<Party, bool>> predicate, NetStepsEntities context)
		{
			Contract.Requires<ArgumentNullException>(predicate != null);
			Contract.Requires<ArgumentNullException>(context != null);

			return FirstOrDefault(predicate, Party.Relations.LoadFull, context);
		}

		public virtual Party FirstOrDefault(Expression<Func<Party, bool>> predicate, Party.Relations relations)
		{
			Contract.Requires<ArgumentNullException>(predicate != null);

			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					return FirstOrDefault(predicate, relations, context);
				}
			});
		}

		public virtual Party FirstOrDefault(Expression<Func<Party, bool>> predicate, Party.Relations relations, NetStepsEntities context)
		{
			Contract.Requires<ArgumentNullException>(predicate != null);
			Contract.Requires<ArgumentNullException>(context != null);

			var party = context.Parties
				.FirstOrDefault(predicate);

			if (party == null)
			{
				return null;
			}

			party.LoadRelations(context, relations);

			return party;
		}

		public virtual List<Party> WhereFull(Expression<Func<Party, bool>> predicate)
		{
			Contract.Requires<ArgumentNullException>(predicate != null);

			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					return WhereFull(predicate, context);
				}
			});
		}

		public virtual List<Party> WhereFull(Expression<Func<Party, bool>> predicate, NetStepsEntities context)
		{
			Contract.Requires<ArgumentNullException>(predicate != null);
			Contract.Requires<ArgumentNullException>(context != null);

			return Where(predicate, Party.Relations.LoadFull, context);
		}

		public virtual List<Party> Where(Expression<Func<Party, bool>> predicate, Party.Relations relations)
		{
			Contract.Requires<ArgumentNullException>(predicate != null);

			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					return Where(predicate, relations, context);
				}
			});
		}

		public virtual List<Party> Where(Expression<Func<Party, bool>> predicate, Party.Relations relations, NetStepsEntities context)
		{
			Contract.Requires<ArgumentNullException>(predicate != null);
			Contract.Requires<ArgumentNullException>(context != null);

			var parties = context.Parties
				.Where(predicate)
				.ToList();

			parties.LoadRelations(context, relations);

			return parties;
		}
		#endregion
		public IParty GetPartyByPartyID(int partyID)
		{
			return ExceptionHandledDataAction.Run(
				new ExecutionContext(this),
				() =>
					{
						using (var context = CreateContext())
						{
							Party party = context.Parties.FirstOrDefault(p => p.PartyID == partyID);
							if (party == null)
							{
								throw new Exception(string.Format("No party for PartyID: {0}", partyID));
							}

							party.Order = context.Orders.Include("OrderCustomers").FirstOrDefault(o => o.OrderID == party.OrderID);
							if (party.Order == null)
							{
								throw new Exception(string.Format("PartyID: {0} has no order.", partyID));
							}

							return party;
						}
					});
		}

		public int GetDistributorIDFromParty(IParty party)
		{
			return ExceptionHandledDataAction.Run(
				new ExecutionContext(this),
				() =>
					{
						using (var context = CreateContext())
						{
							var order = context.Orders.FirstOrDefault(o => party.OrderID == o.OrderID);
							if (order == null)
							{
								throw new Exception(string.Format("There is no order for orderID: {0}", party.OrderID));
							}
							return order.ConsultantID;
						}
					});
			;
		}
	}
}
