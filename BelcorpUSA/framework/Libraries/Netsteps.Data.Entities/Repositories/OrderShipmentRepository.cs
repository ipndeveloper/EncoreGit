using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Dynamic;
using NetSteps.Common.Base;
using NetSteps.Common.Configuration;
using NetSteps.Common.Extensions;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class OrderShipmentRepository
    {
        protected override Func<NetStepsEntities, IQueryable<OrderShipment>> loadAllFullQuery
        {
            get
            {
                return context => context.OrderShipments
                    .Include("Order")
                    .Include("OrderCustomer")
                    .Include("OrderCustomer.Account")
                    .Include("OrderShipmentPackages")
                    .Include("OrderShipmentPackages.OrderShipmentPackageItems");
            }
        }
        public OrderShipment LoadFullByShipmentID(int shipmentID)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    return loadAllFullQuery(context).FirstOrDefault(os => os.OrderShipmentID == shipmentID);
                }
            });
        }

        public virtual IPaginatedList<OrderShippingSearchData> Search(OrderShipmentSearchParameters searchParameters)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var orderIDs = Search(searchParameters, context);

                    var orders = GetOrderShippingSearchData(orderIDs, context, searchParameters);

                    // Join to maintain the correct sort order.
                    return (from orderID in orderIDs
                            join order in orders on orderID equals order.OrderID
                            select order)
                            .ToPaginatedList(searchParameters, orderIDs.TotalCount);
                }
            });
        }

        public virtual IList<OrderShippingSearchData> GetOrderShippingSearchData(IEnumerable<int> orderIDs)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    return GetOrderShippingSearchData(orderIDs, context);
                }
            });
        }

        protected virtual IList<OrderShippingSearchData> GetOrderShippingSearchData(IEnumerable<int> orderIDs, NetStepsEntities context, OrderShipmentSearchParameters searchParameters = null)
        {
            Contract.Requires<ArgumentNullException>(orderIDs != null);
            Contract.Requires<ArgumentNullException>(context != null);

            var orders = context.Orders
                .Where(x => orderIDs.Contains(x.OrderID))
                // Force a single inner join to Accounts.
                .Select(x => new
                {
                    Order = x,
                    Consultant = x.Consultant
                })
                .Select(x => new OrderShippingSearchData
                {
                    OrderID = x.Order.OrderID,
                    OrderNumber = x.Order.OrderNumber,
                    OrderTypeID = x.Order.OrderTypeID,
                    OrderStatusID = x.Order.OrderStatusID,
                    ConsultantID = x.Order.ConsultantID,
                    ConsultantAccountNumber = x.Consultant.AccountNumber,
                    ConsultantFirstName = x.Consultant.FirstName,
                    ConsultantLastName = x.Consultant.LastName,
                    CompleteDateUTC = x.Order.CompleteDateUTC,
                    CurrencyID = x.Order.CurrencyID,
                })
                .ToList();

			IQueryable<OrderShipment> orderShipmentsQuery;

			if (searchParameters != null && searchParameters.OrderShipmentStatusID != null)
			{
				orderShipmentsQuery = context.OrderShipments
					.Where(os => orderIDs.Contains(os.OrderID) && os.OrderShipmentStatusID == searchParameters.OrderShipmentStatusID);
			}
			else
			{
				orderShipmentsQuery = context.OrderShipments.Where(os => orderIDs.Contains(os.OrderID));
			}

        	var orderShipments =
        		orderShipmentsQuery.Select(
        			os =>
        			new
        				{
        					os.OrderID,
        					os.OrderShipmentID,
        					os.OrderShipmentStatusID,
        					os.FirstName,
        					os.LastName,
        					os.IsWillCall,
        				}).ToList();

            var orderShipmentIDs = orderShipments
                .Select(os => os.OrderShipmentID)
                .ToList();

            var orderShipmentPackages = context.OrderShipmentPackages
                .Where(osp => orderShipmentIDs.Contains(osp.OrderShipmentID))
                .Select(osp => new
                {
                    osp.OrderShipmentID,
                    OrderShipmentPackageID = osp.OrderShipmentPackageID,
                    TrackingNumber = osp.TrackingNumber,
                    DateShippedUTC = osp.DateShippedUTC
                })
                .ToList();

            orders.ForEach(o => o.OrderShipments = orderShipments
                .Where(os => os.OrderID == o.OrderID)
                .OrderBy(os => os.OrderShipmentID)
                .Select(os => new OrderShippingSearchData.OrderShipmentSearchData
                {
                    OrderShipmentID = os.OrderShipmentID,
                    OrderShipmentIndex = orderShipments
                        .Count(x => x.OrderID == o.OrderID && x.OrderShipmentID < os.OrderShipmentID),
                    OrderShipmentStatusID = os.OrderShipmentStatusID,
                    FirstName = os.FirstName,
                    LastName = os.LastName,
                    IsWillCall = os.IsWillCall,
                    OrderShipmentPackages = orderShipmentPackages
                        .Where(osp => osp.OrderShipmentID == os.OrderShipmentID)
                        .OrderBy(osp => osp.OrderShipmentPackageID)
                        .Select(osp => new OrderShippingSearchData.OrderShipmentPackageSearchData
                        {
                            OrderShipmentPackageID = osp.OrderShipmentPackageID,
                            OrderShipmentPackageIndex = orderShipmentPackages
                                .Count(x => x.OrderShipmentID == osp.OrderShipmentID && x.OrderShipmentPackageID < osp.OrderShipmentPackageID),
                            TrackingNumber = osp.TrackingNumber,
                            DateShippedUTC = osp.DateShippedUTC
                        })
                        .ToList()
                })
                .ToList()
            );

            return orders;
        }

        protected virtual IPaginatedList<int> Search(OrderShipmentSearchParameters searchParameters, NetStepsEntities context)
        {
            Contract.Requires<ArgumentNullException>(searchParameters != null);
            Contract.Requires<ArgumentNullException>(context != null);

            var query = context.Orders
                .AsQueryable();

            query = ApplyFilters(query, searchParameters, context);

            var totalCount = query.Count();

            query = ApplyOrderBy(query, searchParameters, context);

            return query
                .ApplyPagination(searchParameters)
                .Select(x => x.OrderID)
                .ToPaginatedList(searchParameters, totalCount);
        }

        protected virtual IQueryable<Order> ApplyFilters(IQueryable<Order> query, OrderShipmentSearchParameters searchParameters, NetStepsEntities context)
        {
            Contract.Requires<ArgumentNullException>(query != null);
            Contract.Requires<ArgumentNullException>(searchParameters != null);
            Contract.Requires<ArgumentNullException>(context != null);

            if (searchParameters.OrderIDs != null && searchParameters.OrderIDs.Any())
            {
                query = query.Where(x => searchParameters.OrderIDs.Contains(x.OrderID));
            }
            else
            {
                // Order Number
                if (!string.IsNullOrWhiteSpace(searchParameters.OrderNumber))
                {
                    query = query.Where(x => x.OrderNumber.Contains(searchParameters.OrderNumber));
                }

                // Order Type
                if (searchParameters.OrderTypeID == null)
                {
                    // Default order type filter - all except templates
                    var excludedOrderTypeIDs = new[] { (short)Constants.OrderType.AutoshipTemplate, (short)Constants.OrderType.ReturnOrder };
                    query = query.Where(x => !excludedOrderTypeIDs.Contains(x.OrderTypeID));
                }
                else
                {
                    query = query.Where(x => x.OrderTypeID == searchParameters.OrderTypeID);
                }

                // Order Status
                if (searchParameters.OrderStatusID == null)
                {
                    // Default order status filter - these are the statuses that can be shipped
                    var orderStatusIDs = new[]
                                {
                                    (short)Constants.OrderStatus.Paid,
                                    (short)Constants.OrderStatus.Printed,
                                    (short)Constants.OrderStatus.PartiallyShipped
                                };

                    query = query.Where(x => orderStatusIDs.Contains(x.OrderStatusID));
                }
                else
                {
                    query = query.Where(x => x.OrderStatusID == searchParameters.OrderStatusID);
                }

                // Order Shipment Status
                if (searchParameters.OrderShipmentStatusID != null)
                {
                    query = query.Where(x => x.OrderShipments.Any(os => os.OrderShipmentStatusID == searchParameters.OrderShipmentStatusID));
                }

                // Start Date
                if (searchParameters.StartDate.HasValue)
                {
                    DateTime startDateUTC = searchParameters.StartDate.Value.Date.LocalToUTC();
                    query = query.Where(a => a.CompleteDateUTC >= startDateUTC);
                }

                // End Date
                if (searchParameters.EndDate.HasValue)
                {
                    DateTime endDateUTC = searchParameters.EndDate.Value.EndOfDay().LocalToUTC();
                    query = query.Where(a => a.CompleteDateUTC <= endDateUTC);
                }

                // Where Clause
                if (searchParameters.WhereClause != null)
                {
                    query = query.Where(searchParameters.WhereClause);
                }
            }

            return query;
        }

        protected virtual IQueryable<Order> ApplyOrderBy(IQueryable<Order> query, OrderShipmentSearchParameters searchParameters, NetStepsEntities context)
        {
            Contract.Requires<ArgumentNullException>(query != null);
            Contract.Requires<ArgumentNullException>(searchParameters != null);
            Contract.Requires<ArgumentNullException>(context != null);

            string orderBy = searchParameters.OrderBy;

            // Default sort
            if (string.IsNullOrWhiteSpace(orderBy))
            {
                orderBy = "OrderNumber";
            }

            switch (orderBy)
            {
                case "OrderNumber":
                    if (ConfigurationManager.GetAppSetting<bool>(ConfigurationManager.VariableKey.OrderNumbersEqualIdentity))
                        query = searchParameters.OrderByDirection == Constants.SortDirection.Ascending
                            ? query.OrderBy(x => x.OrderID)
                            : query.OrderByDescending(x => x.OrderID);
                    else if (ConfigurationManager.GetAppSetting<bool>(ConfigurationManager.VariableKey.OrderNumbersAreNumeric))
                        query = searchParameters.OrderByDirection == Constants.SortDirection.Ascending
                            ? query.OrderBy(x => NetStepsEntities.ToBigInt(x.OrderNumber))
                            : query.OrderByDescending(x => NetStepsEntities.ToBigInt(x.OrderNumber));
                    else
                        query = searchParameters.OrderByDirection == Constants.SortDirection.Ascending
                            ? query.OrderBy(x => x.OrderNumber)
                            : query.OrderByDescending(x => x.OrderNumber);
                    break;
                case "Consultant.FullName":
                    // Force a single inner join to Accounts.
                    // http://stackoverflow.com/questions/9799136/why-does-linq-to-ef-render-certain-contains-as-two-separate-joins
                    var tempQuery = query
                        .Select(x => new { Order = x, x.Consultant });
                    query = (
                        searchParameters.OrderByDirection == Constants.SortDirection.Ascending
                            ? tempQuery.OrderBy(x => x.Consultant.FirstName).ThenBy(x => x.Consultant.LastName)
                            : tempQuery.OrderByDescending(x => x.Consultant.FirstName).ThenByDescending(x => x.Consultant.LastName)
                        )
                        .Select(x => x.Order);
                    break;
                case "OrderStatus":
                    query = query.OrderByLocalizedKind(
                        (int)Constants.LocalizedKindTable.OrderStatuses,
                        searchParameters.LanguageID,
                        o => o.OrderStatusID,
                        searchParameters.OrderByDirection,
                        context
                    );
                    break;
                case "OrderType":
                    query = query.OrderByLocalizedKind(
                        (int)Constants.LocalizedKindTable.OrderTypes,
                        searchParameters.LanguageID,
                        o => o.OrderTypeID,
                        searchParameters.OrderByDirection,
                        context
                    );
                    break;
                default:
                    // We know searchParameters.OrderByString is not blank or it would have
                    // hit the "OrderNumber" case.
                    query = DynamicQueryable.OrderBy(query, searchParameters.OrderByString);
                    break;
            }

            return query;
        }
    }
}