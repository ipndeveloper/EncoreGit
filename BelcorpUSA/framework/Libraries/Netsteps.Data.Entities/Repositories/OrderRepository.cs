using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using NetSteps.Addresses.Common.Models;
using NetSteps.Common.Base;
using NetSteps.Common.Configuration;
using NetSteps.Common.Exceptions;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Common.Threading;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Business.Logic;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Generated;
using NetSteps.Encore.Core.IoC;
using NetSteps.Security;
using System.ComponentModel.DataAnnotations;
using NetSteps.Data.Entities.Dto;
using System.Data.SqlClient;

namespace NetSteps.Data.Entities.Repositories
{
    [ContainerRegister(typeof(NetSteps.Events.Common.Repositories.IOrderRepository), RegistrationBehaviors.IfNoneOther, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class OrderRepository : NetSteps.Events.Common.Repositories.IOrderRepository, IOrderRepository
    {
        protected class OrderTotalDetails
        {
            public decimal Subtotal;

            public decimal GrandTotal;

            public int OrderID;
        }

        protected class CustomOrder
        {
            public int OrderID { get; set; }
            public string OrderNumber { get; set; }
            /*CS.20AGO2016.Inicio*/
            //public string InvoiceNumber { get; set; }
            /*CS.20AGO2016.Fin*/
            public short OrderTypeID { get; set; }
            public int CurrencyID { get; set; }
            public short OrderStatusID { get; set; }
            public int ConsultantID { get; set; }
            public int? ParentOrderID { get; set; }
            public decimal? Subtotal { get; set; }
            public decimal? GrandTotal { get; set; }
            public decimal? CommissionableTotal { get; set; }

            public Account Consultant { get; set; }

            public DateTime? DateCreatedUTC { get; set; }
            public DateTime? CompleteDateUTC { get; set; }
            public DateTime? CommissionDateUTC { get; set; }
            public DateTime? CommissionDate
            {
                get { return CommissionDateUTC.UTCToLocal(TimeZoneInfo.Local); }
                set { CommissionDateUTC = value.LocalToUTC(TimeZoneInfo.Local); }
            }

            public decimal? TotalQV { get; set; } //CGI(CMR)-24/10/2014
        }

        #region Obsolete loadAllFullQuery
        protected override Func<NetStepsEntities, int, IQueryable<Order>> loadFullQuery
        {
            get
            {
                return CompiledQuery.Compile<NetStepsEntities, int, IQueryable<Order>>((context, orderId) => context.Orders
                                                            .Include("Notes")
                                            .Include("OrderAdjustments")
                                            .Include("OrderAdjustments.OrderAdjustmentOrderLineModifications")
                                            .Include("OrderAdjustments.OrderAdjustmentOrderModifications")
                                                            .Include("OrderCustomers")
                                                            .Include("OrderCustomers.OrderItems")
                                            .Include("OrderCustomers.OrderItems.GiftCards")
                                                            .Include("OrderCustomers.OrderItems.OrderItemPrices")
                                                            .Include("OrderCustomers.OrderItems.OrderItemProperties")
                                                            .Include("OrderCustomers.OrderItems.OrderItemProperties.OrderItemPropertyValue")
                                            .Include("OrderCustomers.OrderItems.OrderItemReturns")
                                                            .Include("OrderPayments")
                                                            .Include("OrderAdjustments")
                                                            .Include("OrderAdjustments.OrderAdjustmentOrderModifications")
                                                            .Include("OrderAdjustments.OrderAdjustmentOrderLineModifications")
                                                            .Where(o => o.OrderID == orderId));
            }
        }
        #endregion

        #region Methods
        public Order LoadByOrderNumber(string orderNumber)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var result = context.Orders.FirstOrDefault(o => o.OrderNumber == orderNumber);
                    return result;
                }
            });
        }

        public Order LoadByOrderNumberFull(string orderNumber)
        {
            var order = LoadFullFirstOrDefault(x => x.OrderNumber == orderNumber);

            if (order == null)
                throw new NetStepsDataException("Error loading order. Invalid orderNumber: " + orderNumber);
            else
                return order;
        }

        //@01 AINI BAL
        public Order LoadByID(int orderID)
        {
            var order = LoadFullFirstOrDefault(x => x.OrderID.Equals(orderID));

            if (order == null)
                throw new NetStepsDataException("Error loading order. Invalid orderID: " + orderID);
            else
                return order;
        }

        public List<OrderShipment> LoadOrderShipments(int orderID)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var orderShipments = (from o in context.Orders
                                          join os in context.OrderShipments on o.OrderID equals os.OrderID
                                          where o.OrderID == orderID
                                          orderby os.OrderID descending
                                          select os).ToList();

                    return orderShipments.ToList();
                }
            });
        }

        public IEnumerable<Order> LoadOrderWithShipmentAndPaymentDetails(IEnumerable<string> orderNumbers)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var orders = context.Orders.Include("OrderCustomers")
                                                      .Include("OrderShipments")
                                                      .Include("OrderPayments")
                                                      .Include("OrderCustomers.OrderPayments")
                                                      .Include("OrderCustomers.OrderItems")
                                                      .Include("OrderCustomers.OrderItems.OrderItemProperties")
                                                      .Include("OrderCustomers.OrderItems.GiftCards")
                                                      .Include("OrderCustomers.OrderShipments")
                                                      .Include("OrderAdjustments")
                                                      .Include("OrderAdjustments.OrderAdjustmentOrderModifications")
                                                      .Include("OrderAdjustments.OrderAdjustmentOrderLineModifications")
                                                      .Where(o => orderNumbers.Contains(o.OrderNumber)).ToList();
                    return orders;
                }
            });
        }

        public Order LoadOrderWithPaymentDetails(int orderID)
        {
            return LoadFirstOrDefault(o => o.OrderID == orderID, Order.Relations.LoadPaymentDetails);
        }

        public IEnumerable<Order> LoadOrdersByParentOrderIdAndOrderType(int parentOrderId, short orderTypeId)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (var context = new NetStepsEntities())
                {
                    return context.Orders.Where(o => o.ParentOrderID == parentOrderId && o.OrderTypeID == orderTypeId).ToList();
                }
            });
        }

        public Order LoadWithShipmentDetails(int orderID)
        {
            return LoadFirstOrDefault(x => x.OrderID == orderID, Order.Relations.LoadShipmentDetails);
        }

        public List<Order> LoadBatchWithShipmentDetails(IEnumerable<int> orderIDs)
        {
            return LoadWhere(x => orderIDs.Contains(x.OrderID), Order.Relations.LoadShipmentDetails);
        }

        public PaginatedList<OrderSearchData> Search(OrderSearchParameters searchParameters)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {

             


                    var results = new PaginatedList<OrderSearchData>(searchParameters);

                    var query = this.ApplyPartyFullTotal(context.Orders, searchParameters, context);

                    var matchingItems = ApplyFilters(query, searchParameters, context);

                    results.TotalCount = matchingItems.Count();

                    matchingItems = ApplyOrderBy(matchingItems, searchParameters, context);

                    matchingItems = matchingItems.ApplyPagination(searchParameters);

                    var selectedMatchingItems = matchingItems.Select(o => new
                    {
                        o.OrderID,
                        o.OrderNumber,
                        o.OrderTypeID,
                        o.CurrencyID,
                        o.OrderStatusID,
                        o.Subtotal,
                        o.GrandTotal,
                        o.DateCreatedUTC,
                        o.CompleteDateUTC,
                        o.CommissionDateUTC,
                        o.CommissionableTotal,
                        SponsorFirstName = o.Consultant.FirstName,
                        SponsorLastName = o.Consultant.LastName,
                        SponsorAccountNumber = o.Consultant.AccountNumber,
                        //CGI(CMR)-27/10/2014-Inicio
                        SearchedOrderCustomer_TotalQV = (context.OrderCustomers.Where(oc => oc.OrderID == o.OrderID))
                            //.Where(oc => (searchParameters.ConsultantOrCustomerAccountID ?? searchParameters.CustomerAccountID) != null && oc.AccountID == (searchParameters.ConsultantOrCustomerAccountID ?? searchParameters.CustomerAccountID))
                                .Select(oc => new
                                {
                                    OrderItems_TotalQV = oc.OrderItems
                                        //.Sum(oi => ( (oi.ItemPrice == null ? 0 : oi.ItemPrice) * (oi.Quantity == null ? 0 : (decimal)oi.Quantity)))
                                    .Select(oi => new
                                    {
                                        OrderItemPrices_TotalQV = oi.OrderItemPrices.Any(ip => ip.ProductPriceTypeID == (int)Constants.ProductPriceType.QV) ?
                                            oi.OrderItemPrices
                                            .Where(ip => ip.ProductPriceTypeID == (int)Constants.ProductPriceType.QV)
                                            .Sum(ip => ip.UnitPrice * oi.Quantity)
                                            : 0M
                                    }).FirstOrDefault(),
                                }).FirstOrDefault(),
                        //CGI(CMR)-27/10/2014-Fin
                        SearchedOrderCustomer = (context.OrderCustomers.Where(oc => oc.OrderID == o.OrderID))
                             .Where(oc => (searchParameters.ConsultantOrCustomerAccountID ?? searchParameters.CustomerAccountID) != null && oc.AccountID == (searchParameters.ConsultantOrCustomerAccountID ?? searchParameters.CustomerAccountID))
                             .Select(oc => new
                             {
                                 Total = oc.Total,
                                 Account = context.Accounts
                                      .Where(a => a.AccountID == oc.AccountID)
                                      .Select(a => new
                                      {
                                          FirstName = a.FirstName,
                                          LastName = a.LastName,
                                          AccountNumber = a.AccountNumber
                                      }).FirstOrDefault()
                             }).FirstOrDefault(),

                        FirstOrderCustomer = (context.OrderCustomers.Where(oc => oc.OrderID == o.OrderID))
                             .OrderByDescending(x => x.OrderCustomerTypeID == (int)Constants.OrderCustomerType.Hostess)
                             .Select(oc => new
                             {
                                 Total = oc.Total,
                                 Account = context.Accounts
                                      .Where(a => a.AccountID == oc.AccountID)
                                      .Select(a => new
                                      {
                                          FirstName = a.FirstName,
                                          LastName = a.LastName,
                                          AccountNumber = a.AccountNumber
                                      }).FirstOrDefault()
                             }).FirstOrDefault(),

                        FirstOrderShipment = (context.OrderShipments.Where(oc => oc.OrderID == o.OrderID)).Select(os => new
                        {
                            os.CountryID,
                            FirstOrderShipmentPackage = os.OrderShipmentPackages.Select(p => new
                            {
                                p.DateShippedUTC,
                                p.TrackingNumber,
                                p.ShippingMethod.TrackingNumberBaseUrl,
                                p.ShippingMethodID
                            }).FirstOrDefault()
                        }).FirstOrDefault()
                    });

                    var ordersInfos = selectedMatchingItems.ToList();

                    //INI - GR4172 - Se agrega la columna PeriodId
                    var periods = new List<Tuple<int, string>>();
                    List<int> orderlist = new List<int>();
                    foreach (var item in ordersInfos)
                    {
                        orderlist.Add(item.OrderID);
                    }
                    periods = GetOrderAndPeriods(orderlist);
                    //FIN - GR4172 - Se agrega la columna PeriodId
                    
                    results.AddRange(ordersInfos.Select(o => new OrderSearchData()
                    {
                        FirstName = o.SearchedOrderCustomer != null && o.FirstOrderCustomer == null ? o.SearchedOrderCustomer.Account.FirstName : (o.FirstOrderCustomer != null ? o.FirstOrderCustomer.Account.FirstName : ""),
                        LastName = o.SearchedOrderCustomer != null && o.FirstOrderCustomer == null ? o.SearchedOrderCustomer.Account.LastName : (o.FirstOrderCustomer != null ? o.FirstOrderCustomer.Account.LastName : ""),
                        AccountNumber = o.SearchedOrderCustomer != null && o.FirstOrderCustomer == null ? o.SearchedOrderCustomer.Account.AccountNumber : (o.FirstOrderCustomer != null ? o.FirstOrderCustomer.Account.AccountNumber : ""),
                        Sponsor = string.Format("{0} {1}", o.SponsorFirstName, o.SponsorLastName),
                        SponsorAccountNumber = o.SponsorAccountNumber,
                        OrderID = o.OrderID,
                        OrderNumber = o.OrderNumber,
                        OrderTypeID = o.OrderTypeID,
                        OrderType = SmallCollectionCache.Instance.OrderTypes.GetById(o.OrderTypeID).GetTerm(),
                        CurrencyID = o.CurrencyID,
                        OrderStatusID = o.OrderStatusID,
                        OrderStatus = SmallCollectionCache.Instance.OrderStatuses.GetById(o.OrderStatusID).GetTerm(),
                        Subtotal = o.Subtotal.HasValue ? o.Subtotal.Value : 0m,
                        CustomerTotal = o.SearchedOrderCustomer != null ? o.SearchedOrderCustomer.Total.ToDecimal() : (o.FirstOrderCustomer != null ? o.FirstOrderCustomer.Total.ToDecimal() : (0.00).ToDecimal()),
                        GrandTotal = o.GrandTotal.HasValue ? o.GrandTotal.Value : 0m,
                        DateCreated = o.DateCreatedUTC.UTCToLocal(),

                        CompleteDate = o.CompleteDateUTC.HasValue ? o.CompleteDateUTC : (DateTime?)null,
                        //CompleteDate = o.CompleteDateUTC.HasValue ? o.CompleteDateUTC.UTCToLocal() : (DateTime?)null,
                        CommissionDate = o.CommissionDateUTC.HasValue ? o.CommissionDateUTC.UTCToLocal() : (DateTime?)null,
                        DateShipped = o.FirstOrderShipment != null && o.FirstOrderShipment.FirstOrderShipmentPackage != null ? (DateTime?)o.FirstOrderShipment.FirstOrderShipmentPackage.DateShippedUTC.UTCToLocal() : null,
                        TrackingNumber = o.FirstOrderShipment != null && o.FirstOrderShipment.FirstOrderShipmentPackage != null ? o.FirstOrderShipment.FirstOrderShipmentPackage.TrackingNumber : string.Empty,
                        TrackingUrl = o.FirstOrderShipment != null && o.FirstOrderShipment.FirstOrderShipmentPackage != null && !string.IsNullOrWhiteSpace(o.FirstOrderShipment.FirstOrderShipmentPackage.TrackingNumber) && !string.IsNullOrWhiteSpace(o.FirstOrderShipment.FirstOrderShipmentPackage.TrackingNumberBaseUrl) ? string.Format(o.FirstOrderShipment.FirstOrderShipmentPackage.TrackingNumberBaseUrl, o.FirstOrderShipment.FirstOrderShipmentPackage.TrackingNumber) : string.Empty,
                        MarketID = o.FirstOrderShipment != null ? SmallCollectionCache.Instance.Countries.GetById(o.FirstOrderShipment.CountryID).MarketID : (int?)null,
                        ShippingMethodId = o.FirstOrderShipment != null && o.FirstOrderShipment.FirstOrderShipmentPackage != null ? o.FirstOrderShipment.FirstOrderShipmentPackage.ShippingMethodID : null,
                        CommissionableTotal = o.CommissionableTotal ?? 0m,
                        TotalQV = o.SearchedOrderCustomer_TotalQV == null ? 0 :
                                    o.SearchedOrderCustomer_TotalQV.OrderItems_TotalQV == null ? 0 :
                                        o.SearchedOrderCustomer_TotalQV.OrderItems_TotalQV.OrderItemPrices_TotalQV, //CGI(CMR)-27/10/2014
                        //INI - GR4172 - Se agrega la columna PeriodId
                        PeriodID =  periods.Where(p => p.Item1 == o.OrderID).SingleOrDefault().Item2
                        //FIN - GR4172 - Se agrega la columna PeriodId
                    }));

                    return results;
                }
            });
        }

        protected virtual IQueryable<CustomOrder> ApplyPartyFullTotal(IQueryable<Order> query, OrderSearchParameters searchParameters, NetStepsEntities context)
        {
            // This needs to be done here for now, because the where clause relies on the queryable being for Order (instead of OrderTemp like it is about to become)
            if (searchParameters.WhereClause != null)
            {
                query = query.Where(searchParameters.WhereClause);
            }

            var newQuery = query.Select(q => new CustomOrder
                                                {
                                                    OrderID = q.OrderID,
                                                    OrderNumber = q.OrderNumber,
                                                    OrderTypeID = q.OrderTypeID,
                                                    CurrencyID = q.CurrencyID,
                                                    OrderStatusID = q.OrderStatusID,
                                                    ConsultantID = q.ConsultantID,
                                                    ParentOrderID = q.ParentOrderID,
                                                    Subtotal = q.ChildOrders.Any(x => x.OrderTypeID != (int)Constants.OrderType.ReturnOrder) ? (q.Subtotal ?? 0) + q.ChildOrders.Where(x => x.OrderTypeID != (int)Constants.OrderType.ReturnOrder).Sum(co => co.Subtotal ?? 0) : (q.Subtotal ?? 0),
                                                    GrandTotal = q.ChildOrders.Any(x => x.OrderTypeID != (int)Constants.OrderType.ReturnOrder) ? (q.GrandTotal ?? 0) + q.ChildOrders.Where(x => x.OrderTypeID != (int)Constants.OrderType.ReturnOrder).Sum(co => co.GrandTotal ?? 0) : (q.GrandTotal ?? 0),
                                                    DateCreatedUTC = q.DateCreatedUTC,
                                                    CompleteDateUTC = q.CompleteDateUTC,
                                                    CommissionDateUTC = q.CommissionDateUTC,
                                                    CommissionableTotal = q.CommissionableTotal,
                                                    Consultant = q.Consultant
                                                    //TotalQV = q.GetAllOrderItems().Where(x => x.ProductPriceTypeID == (int)Constants.ProductPriceType.QV).Sum(co => (co.ItemPrice * (decimal)co.Quantity) ) //CGI(CMR)-24/10/2014
                                                });

            return newQuery;
        }

        protected virtual IQueryable<CustomOrder> ConvertToCustomOrderQueryable(IQueryable<Order> query, OrderSearchParameters searchParameters, NetStepsEntities context)
        {
            // This needs to be done here for now, because the where clause relies on the queryable being for Order (instead of OrderTemp like it is about to become)
            if (searchParameters != null && searchParameters.WhereClause != null)
            {
                query = query.Where(searchParameters.WhereClause);
            }

            var newQuery = from q in query
                           select
                           new CustomOrder
                               {
                                   OrderID = q.OrderID,
                                   OrderNumber = q.OrderNumber,
                                   OrderTypeID = q.OrderTypeID,
                                   CurrencyID = q.CurrencyID,
                                   OrderStatusID = q.OrderStatusID,
                                   ConsultantID = q.ConsultantID,
                                   ParentOrderID = q.ParentOrderID,
                                   Subtotal = q.Subtotal,
                                   GrandTotal = q.GrandTotal,
                                   DateCreatedUTC = q.DateCreatedUTC,
                                   CompleteDateUTC = q.CompleteDateUTC,
                                   CommissionDateUTC = q.CommissionDateUTC,
                                   CommissionableTotal = q.CommissionableTotal,
                                   Consultant = q.Consultant
                               };

            return newQuery;
        }

        protected IQueryable<CustomOrder> ApplyFilters(IQueryable<CustomOrder> query, OrderSearchParameters searchParameters, NetStepsEntities context)
        {
            if (searchParameters.ConsultantOrCustomerAccountID.HasValue)
            {
                // Temp IQueryable to join OrderCustomers and Accounts
                var accountQuery = query
                     .Select(o => new
                     {
                         Order = o,
                         OrderCustomersWithAccounts = (context.OrderCustomers.Where(oc => oc.OrderID == o.OrderID)).Select(oc => new
                         {
                             OrderCustomer = oc,
                             Account = context.Accounts.FirstOrDefault(a => a.AccountID == oc.AccountID)
                         })
                     });

                // We do a Union here because putting an OR in the where clause caused timeouts on databases with millions of orders.
                query =
                    // Match consultant
                          query.Where(o => o.ConsultantID == searchParameters.ConsultantOrCustomerAccountID.Value)
                     .Union(
                    // Match customer
                          query.Where(o =>
                              // On order customer searches, don't include parties since there could be multiple order customers

                                //Commnented the following code for the fix of 43865 (Party Order is not loading on Order History or Apply Filter )
                              //As each party order will have multiple order customers and in each customer is associated with AccountId
                              //o.OrderTypeID != (int)Constants.OrderType.PartyOrder &&
                                 (context.OrderCustomers.Where(oc => oc.OrderID == o.OrderID)).Any(oc => oc.AccountID == searchParameters.ConsultantOrCustomerAccountID.Value)))
                     .Union(
                    // Match customer's enroller on enrollment orders
                          accountQuery
                                .Where(x =>
                                     x.Order.OrderTypeID == (short)Constants.OrderType.EnrollmentOrder
                                     && x.OrderCustomersWithAccounts.Any(oca => oca.Account.EnrollerID == searchParameters.ConsultantOrCustomerAccountID.Value))
                                .Select(x => x.Order)
                     );
            }
            else
            {
                if (searchParameters.ConsultantAccountID.HasValue)
                    query = query.Where(o => o.ConsultantID == searchParameters.ConsultantAccountID.Value);

                if (searchParameters.CustomerAccountID.HasValue)
                    query = query.Where(o =>
                        // On order customer searches, don't include parties since there could be multiple order customers
                         o.OrderTypeID != (int)Constants.OrderType.PartyOrder
                         && (context.OrderCustomers.Where(oc => oc.OrderID == o.OrderID)).Any(oc => oc.AccountID == searchParameters.CustomerAccountID.Value));
            }

            if (searchParameters.AutoshipOrderID.HasValue)
            {
                query = from o in query
                        join ao in context.AutoshipOrders on o.ParentOrderID equals ao.TemplateOrderID
                        where ao.AutoshipOrderID == searchParameters.AutoshipOrderID.Value
                        select o;
            }

            if (searchParameters.AutoshipScheduleID.HasValue)
            {
                query = query.Where(o => context.AutoshipOrders.Any(ao => o.ParentOrderID == ao.TemplateOrderID && ao.AutoshipScheduleID == searchParameters.AutoshipScheduleID.Value));
            }

            if (searchParameters.MarketID.HasValue)
            {
                query = query.Where(x => (context.OrderShipments.Where(oc => oc.OrderID == x.OrderID)).Any(os => os.Country.MarketID.Equals(searchParameters.MarketID.Value)));
            }

            if (!string.IsNullOrEmpty(searchParameters.OrderNumber))
                query = query.Where(o => o.OrderNumber.Contains(searchParameters.OrderNumber));

           

            // Account filters
            if (!string.IsNullOrEmpty(searchParameters.CustomerName)
                 || !string.IsNullOrEmpty(searchParameters.CustomerAccountNumber)
                 || searchParameters.AccountTypeID.HasValue)
            {
                // Temp IQueryable to join OrderCustomers and Accounts
                var accountQuery = query
                     .Select(o => new
                     {
                         Order = o,
                         OrderCustomersWithAccounts = (context.OrderCustomers.Where(oc => oc.OrderID == o.OrderID)).Select(oc => new
                         {
                             OrderCustomer = oc,
                             Account = context.Accounts.FirstOrDefault(a => a.AccountID == oc.AccountID)
                         })
                     });

                if (!string.IsNullOrEmpty(searchParameters.CustomerName))
                    accountQuery = accountQuery.Where(x => x.OrderCustomersWithAccounts.Any(oc => oc.Account.FirstName.Contains(searchParameters.CustomerName) || oc.Account.LastName.Contains(searchParameters.CustomerName)));

                if (!string.IsNullOrEmpty(searchParameters.CustomerAccountNumber))
                    accountQuery = accountQuery.Where(x => x.OrderCustomersWithAccounts.Any(oc => oc.Account.AccountNumber.Contains(searchParameters.CustomerAccountNumber)));

                if (searchParameters.AccountTypeID.HasValue)
                    accountQuery = accountQuery.Where(x => x.OrderCustomersWithAccounts.Any(oc => oc.Account.AccountTypeID == searchParameters.AccountTypeID.Value));

                query = accountQuery.Select(x => x.Order);
            }

            if (!string.IsNullOrEmpty(searchParameters.ConsultantName))
            {
                query = query.Where(o => (o.Consultant.FirstName + " " + o.Consultant.LastName).Contains(searchParameters.ConsultantName));
            }

            if (!string.IsNullOrEmpty(searchParameters.ConsultantAccountNumber))
            {
                query = query.Where(o => o.Consultant.AccountNumber.Contains(searchParameters.ConsultantAccountNumber));
            }

            if (searchParameters.CommissionDate.HasValue)
            {
                query = query.Where(o => o.CommissionDate.HasValue && o.CommissionDate.Value > searchParameters.CommissionDate.Value);
            }

            if (!searchParameters.SearchTemplates)
            {
                query = query.Where(o => !(context.OrderTypes.FirstOrDefault(ot => ot.OrderTypeID == o.OrderTypeID).IsTemplate));

            }

            if (searchParameters.OrderStatusID.HasValue)
            {
                query = query.Where(o => o.OrderStatusID == searchParameters.OrderStatusID);
            }

            if (searchParameters.OrderTypeID.HasValue)
            {
                query = query.Where(o => o.OrderTypeID == searchParameters.OrderTypeID);
            }

            if (searchParameters.StartDate.HasValue)
            {
                //// inicio => 18052017 hundred
                ////DateTime startDateUTC = searchParameters.StartDate.Value.StartOfDay().LocalToUTC();
                ////DateTime startDateUTC = searchParameters.StartDate.Value.LocalToUTC();
                //var f = searchParameters.StartDate.Value.Date;


             //  var date = EntityFunctions.CreateDateTime(searchParameters.StartDate.Value.Year, searchParameters.StartDate.Value.Month, searchParameters.StartDate.Value.Day,0,0,0);

                //query = query.Where(a => EntityFunctions.TruncateTime(a.DateCreatedUTC) >= searchParameters.StartDate.Value);
                DateTime startDateUTC = searchParameters.StartDate.Value;
                //DateTime startDateUTC2 = searchParameters.StartDate.Value.LocalToUTC().Date;
                //DateTime startDateUTC = searchParameters.StartDate.Value.StartOfDay().LocalToUTC();
                query = query.Where(a => EntityFunctions.TruncateTime(a.DateCreatedUTC) >= startDateUTC);
            }
            if (searchParameters.EndDate.HasValue)
            {
                // inicio => 18052017 hundred
                //DateTime endDateUTC = searchParameters.EndDate.Value.LocalToUTC();

                // CODIGO ORIGINAL
                //DateTime endDateUTC = searchParameters.EndDate.Value.EndOfDay().LocalToUTC();


                DateTime endDateUTC = searchParameters.EndDate.Value;
                query = query.Where(a => EntityFunctions.TruncateTime(a.DateCreatedUTC) <= endDateUTC);


                //DateTime endDateUTC = searchParameters.EndDate.Value.EndOfDay().LocalToUTC();
                //query = query.Where(a => EntityFunctions.TruncateTime(a.DateCreatedUTC) <= searchParameters.EndDate.Value);
            }

            if (!string.IsNullOrEmpty(searchParameters.CreditCardLastFourDigits))
            {
                query = query.Where(o => (context.OrderPayments.Where(op => op.OrderID == o.OrderID)).Any(op => op.AccountNumberLastFour == searchParameters.CreditCardLastFourDigits));
            }

            if (searchParameters.SearchOpenParties.HasValue && !searchParameters.SearchOpenParties.Value)
            {
                query = query.Where(o => context.Parties.All(p => p.OrderID != o.OrderID) || o.OrderStatusID != (int)Constants.OrderStatus.Pending);
            }

            //NewFilter
            if (searchParameters.PeriodID.HasValue)
            {
                List<int> OrdersListNew = SearchOrdersByPeriod(query.Select(o => o.OrderID).ToList(), searchParameters.PeriodID.ToInt());

                query = query.Where(x => OrdersListNew.Contains(x.OrderID));
            }

            return query;
        }

        protected IQueryable<CustomOrder> ApplyOrderBy(IQueryable<CustomOrder> query, OrderSearchParameters searchParameters, NetStepsEntities context)
        {
            if (!searchParameters.OrderBy.IsNullOrEmpty())
            {
                switch (searchParameters.OrderBy)
                {
                    case "DateShippedUTC":
                        // This code is too slow, I don't think we can sort by DateShippedUTC because of the complexity. - Lundy
                        //query = query.ApplyOrderByFilter(searchParameters.OrderByDirection, o => o.OrderShipments.FirstOrDefault().OrderShipmentPackages.FirstOrDefault().DateShippedUTC);

                        // Just in case someone attempts to sort by DateShippedUTC, this will avoid an exception. - Lundy
                        query = query.OrderBy(o => o.OrderID);
                        break;
                    case "FirstName":
                        var firstNameQuery = query
                             .Select(o => new
                             {
                                 Order = o,
                                 FirstName = (context.OrderCustomers.Where(oc => oc.OrderID == o.OrderID))
                                      .Select(oc => context.Accounts
                                            .Where(a => a.AccountID == oc.AccountID)
                                            .Select(a => a.FirstName)
                                            .FirstOrDefault())
                                      .FirstOrDefault()
                             });
                        query = searchParameters.OrderByDirection == Constants.SortDirection.Ascending
                             ? firstNameQuery.OrderBy(x => x.FirstName).Select(x => x.Order)
                             : firstNameQuery.OrderByDescending(x => x.FirstName).Select(x => x.Order);
                        break;
                    case "LastName":
                        var lastNameQuery = query
                             .Select(o => new
                             {
                                 Order = o,
                                 LastName = (context.OrderCustomers.Where(oc => oc.OrderID == o.OrderID))
                                      .Select(oc => context.Accounts
                                            .Where(a => a.AccountID == oc.AccountID)
                                            .Select(a => a.LastName)
                                            .FirstOrDefault())
                                      .FirstOrDefault()
                             });
                        query = searchParameters.OrderByDirection == Constants.SortDirection.Ascending
                             ? lastNameQuery.OrderBy(x => x.LastName).Select(x => x.Order)
                             : lastNameQuery.OrderByDescending(x => x.LastName).Select(x => x.Order);
                        break;
                    case "OrderNumber":
                        if (ConfigurationManager.GetAppSetting<bool>(ConfigurationManager.VariableKey.OrderNumbersEqualIdentity))
                            query = query.ApplyOrderByFilter(searchParameters.OrderByDirection, o => o.OrderID);
                        else if (ConfigurationManager.GetAppSetting<bool>(ConfigurationManager.VariableKey.OrderNumbersAreNumeric))
                            query = query.ApplyOrderByFilter(searchParameters.OrderByDirection, o => NetStepsEntities.ToBigInt(o.OrderNumber));
                        else
                            query = query.ApplyOrderByFilter(searchParameters, context);
                        break;
                    case "Sponsor":
                        query = searchParameters.OrderByDirection == Constants.SortDirection.Ascending
                             ? query.OrderBy(o => o.Consultant.FirstName).ThenBy(o => o.Consultant.LastName)
                             : query.OrderByDescending(o => o.Consultant.FirstName).ThenByDescending(o => o.Consultant.LastName);
                        break;
                    case "AccountNumber":
                        query = searchParameters.OrderByDirection == Constants.SortDirection.Ascending
                             ? query.OrderBy(o => o.Consultant.AccountNumber)
                             : query.OrderByDescending(o => o.Consultant.AccountNumber);
                        break;
                    default:
                        var newQuery = query.Select(q => new
                            {
                                q.OrderID,
                                q.OrderNumber,
                                q.OrderTypeID,
                                q.CurrencyID,
                                q.OrderStatusID,
                                q.ConsultantID,
                                q.ParentOrderID,
                                q.Subtotal,
                                q.GrandTotal,
                                q.DateCreatedUTC,
                                q.CompleteDateUTC,
                                q.CommissionDateUTC,
                                q.CommissionableTotal,
                                q.Consultant,
                                OrderType = context.OrderTypes.FirstOrDefault(ot => ot.OrderTypeID == q.OrderTypeID),
                                OrderStatus = context.OrderStatuses.FirstOrDefault(os => os.OrderStatusID == q.OrderStatusID)
                                //q.TotalQV //CGI(CMR)-24/10/2014
                            }).ApplyOrderByFilter(searchParameters, context);
                        query = newQuery.Select(q => new CustomOrder
                            {
                                OrderID = q.OrderID,
                                OrderNumber = q.OrderNumber,
                                OrderTypeID = q.OrderTypeID,
                                CurrencyID = q.CurrencyID,
                                OrderStatusID = q.OrderStatusID,
                                ConsultantID = q.ConsultantID,
                                ParentOrderID = q.ParentOrderID,
                                Subtotal = q.Subtotal,
                                GrandTotal = q.GrandTotal,
                                DateCreatedUTC = q.DateCreatedUTC,
                                CompleteDateUTC = q.CompleteDateUTC,
                                CommissionDateUTC = q.CommissionDateUTC,
                                CommissionableTotal = q.CommissionableTotal,
                                Consultant = q.Consultant
                                //TotalQV = q.TotalQV //CGI(CMR)-24/10/2014
                            });
                        break;
                }
            }
            else
                query = query.OrderBy(o => o.OrderID);

            return query;
        }


       
        public int Count(OrderSearchParameters searchParameters)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var query = this.ConvertToCustomOrderQueryable(context.Orders, searchParameters, context);
                    var matchingItems = ApplyFilters(query, searchParameters, context);

                    return matchingItems.Count();
                }
            });
        }

        public PaginatedList<Order> SearchOrders(OrderSearchParameters searchParameters)
        {
            if (searchParameters == null)
                throw new ArgumentNullException();

            return SearchOrders(searchParameters, x => x);
        }

        public PaginatedList<TResult> SearchOrders<TResult>(OrderSearchParameters searchParameters, Expression<Func<Order, TResult>> selector)
        {
            if (searchParameters == null || selector == null)
                throw new ArgumentNullException();

            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var results = new PaginatedList<TResult>(searchParameters);

                    var query = this.ConvertToCustomOrderQueryable(context.Orders, searchParameters, context);

                    query = ApplyFilters(query, searchParameters, context);

                    int totalCount = query.Count();
                    results.TotalCount = totalCount;
                    //results.TotalCount = query.Count();

                    query = ApplyOrderBy(query, searchParameters, context);

                    query = query.ApplyPagination(searchParameters);

                    var selectedQuery = context.Orders.Where(o => query.Any(q => q.OrderID == o.OrderID)).Select(selector);

                    var items = selectedQuery.ToList();
                    results.AddRange(items);

                    return results;
                }
            });
        }

        public List<Order> LoadChildOrders(int parentOrderID, params int[] childOrderTypeIDs)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var childOrders = from o in context.Orders
                                      where o.ParentOrderID == parentOrderID && childOrderTypeIDs.Contains(o.OrderTypeID)
                                      select o;
                    return childOrders.ToList();
                }
            });
        }

        public List<Order> LoadChildOrdersFull(int parentOrderID, params int[] childOrderTypeIDs)
        {
            return LoadFullWhere(o => o.ParentOrderID == parentOrderID && childOrderTypeIDs.Contains(o.OrderTypeID));
        }

        public List<Order> LoadChildOrdersForHostessRewards(int parentOrderID, params int[] childOrderTypeIDs)
        {
            return LoadWhere(o => o.ParentOrderID == parentOrderID && childOrderTypeIDs.Contains(o.OrderTypeID), Order.Relations.LoadChildOrdersForHostessRewards);
        }

        public bool ExistsByOrderNumber(string orderNumber)
        {
            return Any(o => o.OrderNumber == orderNumber);
        }

        public override void Save(Order order)
        {
            if (order.ContainsUnmodifiedDuplicateEntitiesInObjectGraph<OrderShipment>())
            {
                var dup = order.Clone();
                dup.RemoveUnmodifiedDuplicateEntitiesInObjectGraph<OrderShipment>();
                base.Save(dup);
                order.AcceptEntityChanges();
                // TODO: The IDs created by database Identities need to be set back to the 'order' object from the dup object to avoid a problem if the same object is modified and saved again. - JHE
            }
            else
            {
                base.Save(order);
            }

            // Notify commissions in the background
            new RetryWorker<Order>(null,
                 (w, e) => e.Log(Constants.NetStepsExceptionType.NetStepsDataException,
                                 orderID: order.OrderID,
                                 internalMessage: String.Concat("UspInsertOrdersToProcess unable to complete for order: ",
                                 Convert.ToString(order.OrderID))
                                 )
                 )
                 .Fork(NotifyCommissionsOfOrderChanges, order);
        }

        protected void NotifyCommissionsOfOrderChanges(Order order)
        {
            IDbCommand dbCommand = null;

            try
            {
                dbCommand = DataAccess.SetCommand("UspInsertOrdersToProcess", connectionString: NetStepsEntities.CoreConnectionString);
                DataAccess.AddInputParameter("OrderID", order.OrderID, dbCommand);
                DataAccess.ExecuteNonQuery(dbCommand);
            }
            finally
            {
                DataAccess.Close(dbCommand);
            }
        }

        /// <summary>
        /// Query the context to find any child orders for the passed in orderIDs.
        /// Create a new object called "OrderTotalDetails" that will sum up 
        /// all of the subtotal and grand totals for these child orders into a single value.
        /// If no child orders are found an OrderTotalDetails object will still
        /// be created with 0.00 values associated with the orderID.
        /// </summary>
        /// <param name="orderIDs">Int Enumerable of orderIDs to sum up the child order totals.</param>
        /// <returns>An IEnumerable of OrderTotalDetails containing Subtotal and GrandTotal of child orders for each orderID.</returns>
        //private IEnumerable<OrderTotalDetails> GetTotalsForOrderChildOrders(IEnumerable<int> orderIDs)
        //{
        //    var defaults = orderIDs.Select(o => new OrderTotalDetails { OrderID = o }).ToArray();
        //    using (var context = CreateContext())
        //    {
        //        // Base filters
        //        var query = from o in context.Orders
        //                    where o.ParentOrderID.HasValue
        //                    && orderIDs.Contains(o.ParentOrderID.Value)
        //                    group o by o.ParentOrderID into g
        //                    select new
        //                    {
        //                        OrderID = g.Key.Value,
        //                        GrandTotal = g.Sum(x => x.GrandTotal ?? 0),
        //                        Subtotal = g.Sum(x => x.Subtotal ?? 0m)
        //                    };

        //        var result = query.ToArray().Select(x => new OrderTotalDetails { OrderID = x.OrderID, GrandTotal = x.GrandTotal, Subtotal = x.Subtotal });
        //        return result.Union(defaults.Where(o => !result.Select(r => r.OrderID).Contains(o.OrderID)));
        //    }
        //}

        protected override string GetMeaningfulAuditValue(string tableName, string columnName, string value)
        {
            try
            {
                if (columnName == "AccountNumber")
                    return Encryption.DecryptTripleDES(value).MaskString(4);
                else
                    return base.GetMeaningfulAuditValue(tableName, columnName, value);
            }
            catch (Exception ex)
            {
                EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsDataException);
                return value;
            }
        }

        public override PaginatedList<AuditLogRow> GetAuditLog(int primaryKey, AuditLogSearchParameters searchParameters)
        {
            ValidatePrimaryKeyForLoad(primaryKey);

            var order = Order.LoadFull(primaryKey);

            return GetAuditLog(order, searchParameters);
        }

        public virtual PaginatedList<AuditLogRow> GetAuditLog(Order fullyLoadedOrder, AuditLogSearchParameters searchParameters)
        {
            List<AuditTableValueItem> list = new List<AuditTableValueItem>();
            list.Add(new AuditTableValueItem()
            {
                TableName = EntitySetName,
                PrimaryKey = Convert.ToInt32(fullyLoadedOrder.OrderID)
            });

            if (fullyLoadedOrder != null && fullyLoadedOrder.OrderID > 0)
            {
                list.Add(new AuditTableValueItem()
                {
                    TableName = "Orders",
                    PrimaryKey = fullyLoadedOrder.OrderID
                });

                if (fullyLoadedOrder.OrderCustomers != null)
                {
                    foreach (var orderCustomer in fullyLoadedOrder.OrderCustomers)
                    {
                        list.Add(new AuditTableValueItem()
                        {
                            TableName = "OrderCustomers",
                            PrimaryKey = orderCustomer.OrderCustomerID
                        });

                        if (orderCustomer.OrderItems != null)
                        {
                            foreach (var orderItem in orderCustomer.OrderItems)
                            {
                                list.Add(new AuditTableValueItem()
                                {
                                    TableName = "OrderItems",
                                    PrimaryKey = orderItem.OrderItemID
                                });

                                if (orderItem.OrderItemReturns != null)
                                {
                                    foreach (var orderItemReturn in orderItem.OrderItemReturns)
                                    {
                                        list.Add(new AuditTableValueItem()
                                        {
                                            TableName = "OrderItemReturns",
                                            PrimaryKey = orderItemReturn.OrderItemReturnID
                                        });
                                    }
                                }
                            }
                        }

                        if (orderCustomer.OrderPayments != null)
                        {
                            foreach (var orderPayment in orderCustomer.OrderPayments)
                            {
                                list.Add(new AuditTableValueItem()
                                {
                                    TableName = "OrderPayments",
                                    PrimaryKey = orderPayment.OrderPaymentID
                                });
                            }
                        }

                        if (orderCustomer.OrderShipments != null)
                        {
                            foreach (var orderShipment in orderCustomer.OrderShipments)
                            {
                                list.Add(new AuditTableValueItem()
                                {
                                    TableName = "OrderShipments",
                                    PrimaryKey = orderShipment.OrderShipmentID
                                });
                            }
                        }
                    }
                }
            }

            return GetAuditLog(list, searchParameters);
        }

        public List<DateTime> GetCompletedOrderDates(
             int? orderTypeID = null,
             int? parentOrderID = null,
             int? orderCustomerAccountID = null,
             Constants.SortDirection sortDirection = Constants.SortDirection.Ascending,
             int? pageSize = null)
        {
            List<DateTime> results = new List<DateTime>();

            ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (var context = CreateContext())
                {
                    // Base filters
                    var query = from o in context.Orders
                                where o.OrderStatus.IsCommissionable
                                     && o.CompleteDateUTC != null
                                select o;

                    // Conditional filters
                    if (orderTypeID.HasValue)
                    {
                        query = query.Where(o => o.OrderTypeID == orderTypeID.Value);
                    }
                    if (parentOrderID.HasValue)
                    {
                        query = query.Where(o => o.ParentOrderID == parentOrderID.Value);
                    }
                    if (orderCustomerAccountID.HasValue)
                    {
                        query = query.Where(o => o.OrderCustomers.Any(oc => oc.AccountID == orderCustomerAccountID.Value));
                    }

                    // Select
                    var selectedQuery = query.Select(o => o.CompleteDateUTC.Value);

                    // Sort
                    if (sortDirection == Constants.SortDirection.Ascending)
                    {
                        selectedQuery = selectedQuery.OrderBy(x => x);
                    }
                    else
                    {
                        selectedQuery = selectedQuery.OrderByDescending(x => x);
                    }

                    // Paginate
                    if (pageSize.HasValue && pageSize > 0)
                    {
                        selectedQuery = selectedQuery.Take(pageSize.Value);
                    }

                    results.AddRange(selectedQuery);
                }
            });

            return results;
        }

        public IList<OrderCustomer> LoadOrderCustomers(int orderID)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var orderCustomers = (from o in context.Orders
                                          join oc in context.OrderCustomers on o.OrderID equals oc.OrderID
                                          where o.OrderID == orderID
                                          orderby oc.OrderID descending
                                          select oc).ToList();

                    return orderCustomers.ToList();
                }
            });
        }

        public IAddress GetDefaultShippingAddress(int orderId, int orderCustomerId = 0)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (var context = CreateContext())
                {
                    var defaultAddress = (from os in context.OrderShipments
                                          where os.OrderID == orderId
                                               && os.OrderCustomerID == (orderCustomerId > 0 ? orderCustomerId : new int?())
                                          select os).FirstOrDefault()
                                          ??
                                          (from os in context.OrderShipments
                                           where os.OrderID == orderId
                                           select os).FirstOrDefault();

                    return defaultAddress;
                }
            });
        }

        public int GetAccountCompletedOrderCount(int accountId)
        {
            int count = 0;
            ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (var context = CreateContext())
                {
                    var query = from o in context.Orders
                                where o.ConsultantID == accountId && o.OrderStatus.IsCommissionable
                                    && o.CompleteDateUTC != null
                                select o;
                    count = query.Count();
                }
            });
            return count;
        }

        public static List<OrderStatusPartiallyPaidSearchData> ListOrderStatusPartiallyPaid(string OrderNumber, int page, int pageSize, string column, string order)
        {
            try
            {
                List<OrderStatusPartiallyPaidSearchData> result = new List<OrderStatusPartiallyPaidSearchData>();
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@OrderNumber", OrderNumber }, { "@PageSize", pageSize }, { "@PageNumber", page }, { "@Colum", column }, { "@Order", order } };
                SqlDataReader reader = DataAccess.GetDataReader("spListOrderStatusPartiallyPaid", parameters, "Core");

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        result.Add(new OrderStatusPartiallyPaidSearchData()
                        {
                            RowTotal = Convert.ToInt32(reader["RowTotal"]),
                            RowChild = Convert.ToInt32(reader["RowChild"]),
                            OrderID = Convert.ToInt32(reader["OrderID"]),
                            TicketNumber = Convert.ToInt32(reader["TicketNumber"]),
                            Name = Convert.ToString(reader["Name"])
                        });
                    }
                }

                return result;
            }
            catch (Exception ex)
            {

                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static List<OrderStatusShippedSearchData> ListOrderStatusShipped(string OrderNumber, int page, int pageSize, string column, string order)
        {
            try
            {
                List<OrderStatusShippedSearchData> result = new List<OrderStatusShippedSearchData>();
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@OrderNumber", OrderNumber }, { "@PageSize", pageSize }, { "@PageNumber", page }, { "@Colum", column }, { "@Order", order } };
                SqlDataReader reader = DataAccess.GetDataReader("spListOrderStatusShipped", parameters, "Core");

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        result.Add(new OrderStatusShippedSearchData()
                        {
                            RowChild = Convert.ToInt32(reader["RowChild"]),
                            LogDateUTC = Convert.ToDateTime(reader["LogDateUTC"]),
                            Description = Convert.ToString(reader["Description"]),
                            TrackingNumber = Convert.ToInt32(reader["TrackingNumber"]),
                            Name = Convert.ToString(reader["Name"]),
                            RowTotal = Convert.ToInt32(reader["RowTotal"])
                        });
                    }
                }


                return result;
            }
            catch (Exception ex)
            {

                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static List<OrderStatusInvoiceSearchData> ListOrderStatusInvoice(string OrderNumber, int page, int pageSize, string column, string order)
        {
            try
            {
                List<OrderStatusInvoiceSearchData> result = new List<OrderStatusInvoiceSearchData>();
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@OrderNumber", OrderNumber }, { "@PageSize", pageSize }, { "@PageNumber", page }, { "@Colum", column }, { "@Order", order } };
                SqlDataReader reader = DataAccess.GetDataReader("spListOrderStatusInvoice", parameters, "Core");

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        result.Add(new OrderStatusInvoiceSearchData()
                        {
                            RowChild = Convert.ToInt32(reader["RowChild"]),
                            //InvoiceNumber = Convert.ToInt32(reader["InvoiceNumber"]),
                            InvoiceNumber = !reader.IsDBNull(reader.GetOrdinal("InvoiceNumber")) ? Convert.ToInt32(reader["InvoiceNumber"]) : 0,
                            //DateInvoice =  Convert.ToDateTime(reader["DateInvoice"]),
                            DateInvoice = !reader.IsDBNull(reader.GetOrdinal("DateInvoice")) ? Convert.ToDateTime(reader["DateInvoice"]) : DateTime.Now,
                            RowTotal = Convert.ToInt32(reader["RowTotal"])

                        });
                    }
                }


                return result;
            }
            catch (Exception ex)
            {

                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        /// <summary>
        /// Pages the specified query.
        /// </summary>
        /// <typeparam name="T">Generic Type Object</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="query">The Object query where paging needs to be applied.</param>
        /// <param name="pageNum">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="orderByProperty">The order by property.</param>
        /// <param name="isAscendingOrder">if set to <c>true</c> [is ascending order].</param>
        /// <param name="rowsCount">The total rows count.</param>
        /// <returns></returns>
        private static IQueryable<T> PagedResult<T, TResult>(IQueryable<T> query, int pageNum, int pageSize,
                        Expression<Func<T, TResult>> orderByProperty, bool isAscendingOrder, out int rowsCount)
        {
            if (pageSize <= 0) pageSize = 20;

            //Total result count
            rowsCount = query.Count();

            //If page number should be > 0 else set to first page
            if (rowsCount <= pageSize || pageNum <= 0) pageNum = 1;

            //Calculate nunber of rows to skip on pagesize
            int excludedRows = (pageNum - 1) * pageSize;

            query = isAscendingOrder ? query.OrderBy(orderByProperty) : query.OrderByDescending(orderByProperty);

            //Skip the required rows for the current page and take the next records of pagesize count
            return query.Skip(excludedRows).Take(pageSize);
        }
        #endregion

        #region Load Helpers
        public override Order LoadFull(int orderID)
        {
            var order = LoadFullFirstOrDefault(x => x.OrderID == orderID);

            if (order == null)
            {
                throw new NetStepsDataException(string.Format("No Order found with OrderID = {0}.", orderID));
            }

            return order;
        }

        public override List<Order> LoadBatchFull(IEnumerable<int> orderIDs)
        {
            return LoadFullWhere(x => orderIDs.Contains(x.OrderID));
        }

        public virtual Order LoadFullFirstOrDefault(Expression<Func<Order, bool>> predicate)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    return LoadFullFirstOrDefault(predicate, context);
                }
            });
        }

        public virtual Order LoadFullFirstOrDefault(Expression<Func<Order, bool>> predicate, NetStepsEntities context)
        {
            return LoadFirstOrDefault(predicate, Order.Relations.LoadFull, context);
        }

        public virtual Order LoadFirstOrDefault(Expression<Func<Order, bool>> predicate, Order.Relations relations)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    return LoadFirstOrDefault(predicate, relations, context);
                }
            });
        }

        public virtual Order LoadFirstOrDefault(Expression<Func<Order, bool>> predicate, Order.Relations relations, NetStepsEntities context)
        {
            var order = context.Orders
                 .FirstOrDefault(predicate);

            if (order == null)
            {
                return null;
            }

            order.LoadRelations(context, relations);

            return order;
        }

        public virtual List<Order> LoadFullWhere(Expression<Func<Order, bool>> predicate)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    return LoadFullWhere(predicate, context);
                }
            });
        }

        public virtual List<Order> LoadFullWhere(Expression<Func<Order, bool>> predicate, NetStepsEntities context)
        {
            return LoadWhere(predicate, Order.Relations.LoadFull, context);
        }

        public virtual List<Order> LoadWhere(Expression<Func<Order, bool>> predicate, Order.Relations relations)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    return LoadWhere(predicate, relations, context);
                }
            });
        }

        public virtual List<Order> LoadWhere(Expression<Func<Order, bool>> predicate, Order.Relations relations, NetStepsEntities context)
        {
            var orders = context.Orders
                 .Where(predicate)
                 .ToList();

            orders.LoadRelations(context, relations);

            return orders;
        }
        #endregion

        #region IOrderRepository

        public IEnumerable<DateTime> GetNewestCompletedPartyDates(int accountID, int numberToTake)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (var context = CreateContext())
                {
                    IEnumerable<Order> orders = context.Orders.Where(o => o.ConsultantID == accountID
                                    && (o.OrderTypeID == (int)Constants.OrderType.PartyOrder ||
                                        o.OrderTypeID == (int)Constants.OrderType.OnlinePartyOrder)
                                    && (o.OrderStatusID == (int)Constants.OrderStatus.Paid
                                        || o.OrderStatusID == (int)Constants.OrderStatus.PartiallyShipped
                                        || o.OrderStatusID == (int)Constants.OrderStatus.PartyOrderPending
                                        || o.OrderStatusID == (int)Constants.OrderStatus.Printed
                                        || o.OrderStatusID == (int)Constants.OrderStatus.Shipped)).OrderBy(o => o.CompleteDateUTC ?? new DateTime());

                    if (numberToTake < 1)
                    {
                        return orders.Select(o => o.CompleteDateUTC ?? new DateTime()).ToList();
                    }

                    return orders.Select(o => o.CompleteDateUTC ?? new DateTime()).Take(numberToTake).ToList();
                }
            });
        }


        public int? GetRepresentativeAccountIdForOrder(int orderID)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (var context = CreateContext())
                {
                    var order = context.Orders.FirstOrDefault(o => o.OrderID == orderID);
                    if (order != null) return order.ConsultantID;
                    else return default(int?);
                }
            });
        }

        public decimal GetTotal(int orderId, Constants.ProductPriceType productPriceType)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                decimal? result = null;
                using (var context = CreateContext())
                {
                    result = (from o in context.Orders
                              from c in o.OrderCustomers
                              from oi in c.OrderItems
                              from p in oi.OrderItemPrices
                              where o.OrderID == orderId
                                  && p.ProductPriceTypeID == (int)productPriceType
                              select (decimal?)(p.UnitPrice * oi.Quantity))
                              .Where(r => r != null)
                              .Sum();

                    return result.GetValueOrDefault();
                }
            });
        }

        public bool ValidateOrderRulesPreCondition(IOrderRepository repository, int? accountId, out string message)
        {
            var tableOrders = new DynamicModel("Core", "Orders", "OrderID");
            var tableOrderPayments = new DynamicModel("Core", "OrderPayments", "OrderPaymentID");

            var orderCancelledList =
                tableOrders.All(where: @"OrderStatusID = @0 
                                        AND AccountID = @1 
                                        AND CompleteDateUTC = GETDATE()",
                                args: new object[] { (short)ConstantsGenerated.OrderStatus.Cancelled, accountId });

            var cancelledListCount = orderCancelledList.Count();

            var orderPendingList = tableOrders.All(where: @"OrderStatusID = @0 
                                        AND AccountID = @1 
                                        AND CompleteDateUTC = GETDATE()",
                                args: new object[] { (short)ConstantsGenerated.OrderStatus.Pending, accountId });

            var pendingListCount = orderPendingList.Count();

            var orderPaidList = tableOrders.All(where: @"OrderStatusID = @0 
                                        AND AccountID = @1 
                                        AND CompleteDateUTC = GETDATE()",
                                args: new object[] { (short)ConstantsGenerated.OrderStatus.Paid, accountId });

            var paidListCount = orderPaidList.Count();

            var orderCancelledPaidList = tableOrders.All(where: @"OrderStatusID = @0 
                                        AND AccountID = @1 
                                        AND CompleteDateUTC = GETDATE()",
                                args: new object[] { (short)ConstantsGenerated.OrderStatus.CancelledPaid, accountId });

            var cancelledPaidListCount = orderCancelledPaidList.Count();

            var orderCreditCardDeclinedList = tableOrders.All(where: @"OrderStatusID = @0 
                                        AND AccountID = @1 
                                        AND CompleteDateUTC = GETDATE()",
                                args: new object[] { (short)ConstantsGenerated.OrderStatus.CreditCardDeclined, accountId });

            var creditCardDeclinedListCount = orderCreditCardDeclinedList.Count();

            var orderPendingPerPaidConfirmationList = tableOrders.All(where: @"OrderStatusID = @0 
                                        AND AccountID = @1 
                                        AND CompleteDateUTC >= (GETDATE() - 10) AND CompleteDateUTC <= GETDATE()",
                                        args: new object[] { (short)ConstantsGenerated.OrderStatus.Cancelled, accountId });

            var pendingPerPaidConfirmationListCount = orderPendingPerPaidConfirmationList.Count();


            var orderTicketPaymentExpiredList = tableOrderPayments.All(where: @"OrderExpirationStatusID = @0 
                                                                AND AccountNumber = @1 AND OrderPaymentStatusID= @2",
                                                    args: new object[] { (short)ConstantsGenerated.ExpirationStatuses.Expired, accountId, (int)ConstantsGenerated.OrderPaymentStatus.Pending });

            var ticketPaymentExpiredListCount = orderTicketPaymentExpiredList.Count();

            var orderTicketPaymentUnExpiredList = tableOrderPayments.All(where: @"OrderExpirationStatusID = @0 
                                                                AND AccountNumber = @1 AND OrderPaymentStatusID= @2",
                                                    args: new object[] { (short)ConstantsGenerated.ExpirationStatuses.Unexpired, accountId, (int)ConstantsGenerated.OrderPaymentStatus.Pending });

            var ticketPaymentUnExpiredListCount = orderTicketPaymentUnExpiredList.Count();

            var orderNegotiationOriginalList = tableOrderPayments.All(where: @"NegotiationLevelID = @0 
                                                                AND AccountNumber = @1 AND OrderPaymentStatusID= @2",
                                                    args: new object[] { (short)ConstantsGenerated.NegotiationLevel.Original, accountId, (int)ConstantsGenerated.OrderPaymentStatus.Pending });


            var negotiationOriginalListCount = orderNegotiationOriginalList.Count();

            var orderNegotiationFirstList = tableOrderPayments.All(where: @"NegotiationLevelID = @0 
                                                                AND AccountNumber = @1 AND OrderPaymentStatusID= @2",
                                                    args: new object[] { (short)ConstantsGenerated.NegotiationLevel.FirstNegotiation, accountId, (int)ConstantsGenerated.OrderPaymentStatus.Pending });

            var negotiationFirstListCount = orderNegotiationFirstList.Count();

            var orderNegotiationSecondList = tableOrderPayments.All(where: @"NegotiationLevelID = @0 
                                                                AND AccountNumber = @1 AND OrderPaymentStatusID= @2",
                                                    args: new object[] { (short)ConstantsGenerated.NegotiationLevel.SecondNegotiation, accountId, (int)ConstantsGenerated.OrderPaymentStatus.Pending });

            var negotiationSecondListCount = orderNegotiationSecondList.Count();

            var orderNegotiationThirdList = tableOrderPayments.All(where: @"NegotiationLevelID = @0 
                                                                AND AccountNumber = @1",
                                                    args: new object[] { (short)ConstantsGenerated.NegotiationLevel.ThirdNegotiation, accountId });

            var negotiationThirdListCount = orderNegotiationThirdList.Count();

            var orderConfigRules = new OrderRulesConfigurationBusinessLogic();

            var lista = orderConfigRules.GetOrderRulesConfiguration();

            int qtyMaxOrderPerday = 0;
            int qtyMaxOrderPerdayAcumulative = 0;
            int qtyMaxOrderWithoutPayment = 0;
            int qtyMaxOrderWithoutPaymentAcumulative = 0;
            int qtyMaxOfTicketsPayment = 0;
            int qtyMaxOfTicketsPaymentAcumulative = 0;
            int qtyMaxOfTicketsPaymentNegotied = 0;
            int qtyMaxOfTicketsPaymentNegotiedAcumulative = 0;

            foreach (var list in lista)
            {
                string strTermName = list.TermName.ToString();

                string strmessage;
                switch (strTermName)
                {
                    case "MaxOrderPerDay":
                        foreach (var item in list.List)
                        {
                            qtyMaxOrderPerday = item.QuantityMax;

                            var orderStatus = (ConstantsGenerated.OrderStatus)Convert.ToInt16(item.OrderStatusID);

                            switch (orderStatus)
                            {
                                case ConstantsGenerated.OrderStatus.Paid:
                                    qtyMaxOrderPerdayAcumulative += paidListCount;
                                    break;
                                case ConstantsGenerated.OrderStatus.Pending:
                                    qtyMaxOrderPerdayAcumulative += pendingListCount;
                                    break;
                                case ConstantsGenerated.OrderStatus.Cancelled:
                                    qtyMaxOrderPerdayAcumulative += cancelledListCount;
                                    break;
                                case ConstantsGenerated.OrderStatus.CancelledPaid:
                                    qtyMaxOrderPerdayAcumulative += cancelledPaidListCount;
                                    break;
                                case ConstantsGenerated.OrderStatus.CreditCardDeclined:
                                    qtyMaxOrderPerdayAcumulative += creditCardDeclinedListCount;
                                    break;
                                    ;
                            }
                        }

                        if (qtyMaxOrderPerdayAcumulative >= qtyMaxOrderPerday)
                        {
                            strmessage = "You cannot create an order, because you have exceeded the maximum order per day";
                            message = Translation.GetTerm("MaxOrderPerDayMessage", strmessage);
                            return false;
                        }

                        break;
                    case "MaxOrderWithoutPayment":
                        foreach (var item in list.List)
                        {
                            qtyMaxOrderWithoutPayment = item.QuantityMax;

                            var orderStatus = (ConstantsGenerated.OrderStatus)Convert.ToInt16(item.OrderStatusID);

                            switch (orderStatus)
                            {
                                case ConstantsGenerated.OrderStatus.PendingPerPaidConfirmation:
                                    qtyMaxOrderWithoutPaymentAcumulative += pendingPerPaidConfirmationListCount;
                                    break;
                            }
                        }

                        if (qtyMaxOrderWithoutPaymentAcumulative >= qtyMaxOrderWithoutPayment)
                        {
                            strmessage = "You cannot create an order, because you have exceeded the maximum order without payment";
                            message = Translation.GetTerm("MaxOrderWithoutPaymentMessage", strmessage);
                            return false;
                        }

                        break;
                    case "MaxOfTicketsPayment":
                        foreach (var item in list.List)
                        {
                            qtyMaxOfTicketsPayment = item.QuantityMax;

                            var expirationStatus = (ConstantsGenerated.ExpirationStatuses)Convert.ToInt16(item.ExpirationStatusID);

                            switch (expirationStatus)
                            {
                                case ConstantsGenerated.ExpirationStatuses.Expired:
                                    qtyMaxOfTicketsPaymentAcumulative += ticketPaymentExpiredListCount;
                                    break;
                                case ConstantsGenerated.ExpirationStatuses.Unexpired:
                                    qtyMaxOfTicketsPaymentAcumulative += ticketPaymentUnExpiredListCount;
                                    break;
                            }
                        }

                        if (qtyMaxOfTicketsPaymentAcumulative >= qtyMaxOfTicketsPayment)
                        {
                            //TODO: we need to define a description for this message variable
                            strmessage = "It is not meeting the necessary conditions to start a new order";
                            message = Translation.GetTerm("MaxOfTicketPaymentMessage", strmessage);
                            return false;
                        }

                        break;
                    case "MaxOfTicketsPaymentNegotied":
                        foreach (var item in list.List)
                        {
                            qtyMaxOfTicketsPaymentNegotied = item.QuantityMax;

                            var negotiatedLevel = (ConstantsGenerated.NegotiationLevel)Convert.ToInt16(item.NegotiationLevelID);

                            switch (negotiatedLevel)
                            {
                                case ConstantsGenerated.NegotiationLevel.Original:
                                    qtyMaxOfTicketsPaymentNegotiedAcumulative += negotiationOriginalListCount;
                                    break;
                                case ConstantsGenerated.NegotiationLevel.FirstNegotiation:
                                    qtyMaxOfTicketsPaymentNegotiedAcumulative += negotiationFirstListCount;
                                    break;
                                case ConstantsGenerated.NegotiationLevel.SecondNegotiation:
                                    qtyMaxOfTicketsPaymentNegotiedAcumulative += negotiationSecondListCount;
                                    break;
                                case ConstantsGenerated.NegotiationLevel.ThirdNegotiation:
                                    qtyMaxOfTicketsPaymentNegotiedAcumulative += negotiationThirdListCount;
                                    break;
                            }
                        }

                        if (qtyMaxOfTicketsPaymentNegotiedAcumulative >= qtyMaxOfTicketsPaymentNegotied)
                        {
                            //TODO: we need to define a description for this message variable
                            strmessage = "It is not meeting the necessary conditions to start a new order";
                            message = Translation.GetTerm("MaxOfTicketsPaymentNegotiedMessage", strmessage);
                            return false;
                        }

                        break;
                }
            }
            message = "";
            return true;
        }

        #endregion

        /// <summary>
        /// Developed By KLC - CSTI
        /// BR-MLM-003 – ACTUALIZACION DE INDICADORES PERSONALES
        /// </summary>
        /// <param name="OrderID"></param>
        /// <param name="OrderStatusID"></param>
        public static void UpdatePersonalIndicator(int OrderID, short OrderStatusID)
        {
            try
            {
                OrderExtensions.UpdatePersonalIndicator(OrderID, OrderStatusID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
        //Update Status Order 

        public static int spUpdateStatusByOrderNumber(string OrderNumber, int OrderStatusID)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {  { "@OrderNumber", OrderNumber },
                                                                                            { "@OrderStatusID", OrderStatusID }
                                                                                         };

                SqlCommand cmd = DataAccess.GetCommand("spUpdateStatusByOrderNumber", parameters, "Core") as SqlCommand;
                cmd.Connection.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        /// <summary>
        /// Obtiene el Id de la order por filtros
        /// </summary>
        /// <param name="number">Numero de Orden</param>
        /// <param name="idsupport">Id de Ticket de Soporte</param>
        /// <returns>Id de la orden</returns>
        public int OrderIdByFilters(string number, int idsupport)
        {
            using (var context = new EntityDBContext(ConnectionStrings.BelcorpCore))
            {
                var orderNumber = (from o in context.Orders
                                   where (o.OrderNumber == number &&
                                  (o.IDSupportTicket == idsupport || idsupport == 0))
                                   select o.OrderID).FirstOrDefault();

                return orderNumber;
            }
        }

        /// <summary>
        /// Obtiene el support ticket
        /// </summary>
        /// <param name="orderId">Id de la orden</param>
        /// <returns>Id de support ticket</returns>
        public int? IDSupportTicketByOrder(int orderId)
        {
            using (var context = new EntityDBContext(ConnectionStrings.BelcorpCore))
            {
                var supportTicket = (from o in context.Orders
                                     where o.OrderID == orderId
                                     select o.IDSupportTicket).FirstOrDefault();

                return supportTicket;
            }
        }

        /// <summary>
        /// Obtiene el support ticket
        /// </summary>
        /// <param name="orderId">Id de la orden</param>
        /// <returns>Id de support ticket</returns>
        public IEnumerable<OrderDto> OrdersInPeriod(int periodID)
        {
            using (var context = new EntityDBContext(ConnectionStrings.BelcorpCore))
            {
                var accountInPeriod = from r in context.Orders
                                      where r.CompletedPeriodID == periodID
                                      group r by r.AccountID into groupOrders
                                      where groupOrders.Count() > 1
                                      select new
                                      {
                                          AccountId = groupOrders.Key,
                                          TotalOrdersInPeriod = groupOrders.Count()
                                      };

                var ordersInPeriod = from o in context.Orders
                                     join a in accountInPeriod on o.AccountID equals a.AccountId
                                     where o.CompletedPeriodID == periodID
                                     select o;

                var ordersToApply = from op in ordersInPeriod
                                    join oc in context.OrderCustomers on op.OrderID equals oc.OrderID
                                    join oi in context.OrderItems on oc.OrderCustomerID equals oi.OrderCustomerID
                                    join oip in context.OrderItemPrices on oi.OrderItemID equals oip.OrderItemID
                                    where oip.ProductPriceTypeID == 1
                                    select new OrderDto()
                                    {
                                        OrderID = op.OrderID,
                                        RetailTotal = oip.OriginalUnitPrice ?? 0 * oi.Quantity, //orderRetailTotal
                                        AccountID = op.AccountID,
                                        OrderCustomer = (op.AccountID == oc.OrderCustomerID ? 1 : 0)
                                    };

                return ordersToApply.ToList();
            }
        }

        /// <summary>
        /// Obtiene el support ticket
        /// </summary>
        /// <param name="orderId">Id de la orden</param>
        /// <returns>Item1: Number of Pending Orders, Item2: Number of Printed, Shipped, Paid</returns>
        public Tuple<int, int, int> CheckOrdersByAccountID(int AccountID)
        {
            try
            {

                Tuple<int, int, int> result = new Tuple<int, int, int>(0, 0, 0);

                using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[CheckOrdersByAccountID]";
                    cmd.Parameters.AddWithValue("@AccountID", AccountID);
                    cmd.Parameters.AddWithValue("@Pending", result.Item1);
                    cmd.Parameters.AddWithValue("@Others", result.Item2);
                    cmd.Parameters.AddWithValue("@DireccionaPantalla2", result.Item3);
                    cmd.Parameters["@Pending"].Direction = ParameterDirection.Output;
                    cmd.Parameters["@Others"].Direction = ParameterDirection.Output;
                    cmd.Parameters["@DireccionaPantalla2"].Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();
                    result = Tuple.Create(
                          Convert.ToInt32(cmd.Parameters["@Pending"].Value)
                        , Convert.ToInt32(cmd.Parameters["@Others"].Value)
                        , Convert.ToInt32(cmd.Parameters["@DireccionaPantalla2"].Value));
                }

                return result;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public int InsertarOrderTrackings(
                           int OrderCustomerID,
                           Int16 OrderStatuses,
                           DateTime? InitialTackingDateUTC,
                           DateTime? FinalTackingDateUTC,
                           int UserID,
                           string Description
               )
        {
            int SupportTicketFileID = 0;
            SqlParameter op = null;
            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
            {
                connection.Open();

                using (SqlCommand ocom = new SqlCommand())
                {
                    ocom.CommandText = "SpInsOrderTrackings";
                    ocom.Connection = connection;
                    ocom.CommandType = CommandType.StoredProcedure;


                    op = new SqlParameter() { ParameterName = "@OrderCustomerID", Value = OrderCustomerID, SqlDbType = SqlDbType.Int };
                    ocom.Parameters.Add(op);

                    op = new SqlParameter() { ParameterName = "@OrderStatuses", Value = OrderStatuses, SqlDbType = SqlDbType.SmallInt };
                    ocom.Parameters.Add(op);

                    if (!InitialTackingDateUTC.HasValue)
                    {
                        op = new SqlParameter() { ParameterName = "@InitialTackingDateUTC", Value = DBNull.Value, SqlDbType = SqlDbType.DateTime };
                    }
                    else
                    {
                        op = new SqlParameter() { ParameterName = "@InitialTackingDateUTC", Value = InitialTackingDateUTC, SqlDbType = SqlDbType.DateTime };
                    }
                    ocom.Parameters.Add(op);
                    if (!FinalTackingDateUTC.HasValue)
                    {
                        op = new SqlParameter() { ParameterName = "@FinalTackingDateUTC", Value = DBNull.Value, SqlDbType = SqlDbType.DateTime };
                    }
                    else
                    {
                        op = new SqlParameter() { ParameterName = "@FinalTackingDateUTC", Value = FinalTackingDateUTC, SqlDbType = SqlDbType.DateTime };
                    }
                    ocom.Parameters.Add(op);
                    op = new SqlParameter() { ParameterName = "@UserID", Value = UserID, SqlDbType = SqlDbType.Int };
                    ocom.Parameters.Add(op);

                    op = new SqlParameter() { ParameterName = "@Description", Value = Description, SqlDbType = SqlDbType.VarChar };
                    ocom.Parameters.Add(op);

                    int OrderTrackingID = Convert.ToInt32(ocom.ExecuteScalar());

                }
                return SupportTicketFileID;
            }

        }

        public decimal GetItemPriceByIdAndType(int OrderItemID, int ProductPriceTypeID)
        {
            try
            {

                decimal price = 0.00M;

                using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[GetItemPriceByIdAndType]";
                    cmd.Parameters.AddWithValue("@OrderItemID", OrderItemID);
                    cmd.Parameters.AddWithValue("@ProductPriceTypeID", ProductPriceTypeID);
                    cmd.Parameters.AddWithValue("@Price", price);
                    cmd.Parameters["@Price"].Direction = ParameterDirection.Output;
                    cmd.Parameters["@Price"].Precision = 18;
                    cmd.Parameters["@Price"].Scale = 2;
                    cmd.ExecuteNonQuery();
                    price = decimal.Parse(cmd.Parameters["@Price"].Value.ToString());
                }

                return price;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public List<int> SearchOrdersByPeriod(List<int> OrderList, int PeriodID)
        {
            try
            {
                List<int> OrdersListNew = new List<int>();

                DataTable dtOrderIDs = new DataTable();
                dtOrderIDs.Columns.Add("OrderID");

                foreach (var orderID in OrderList)
                {
                    dtOrderIDs.Rows.Add(orderID);
                }

                using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[uspSearchOrdersByPeriod]";
                    cmd.Parameters.AddWithValue("@OrderIDs", dtOrderIDs).SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@PeriodID", PeriodID);

                    cmd.ExecuteNonQuery();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                OrdersListNew.Add(Convert.ToInt32(reader["OrderID"]));
                            }
                        }
                    }
                }

                return OrdersListNew;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public Tuple<string, string, string, string> GetOrderCompleteDateDB(int OrderID)
        {
            try
            {
                Tuple<string, string, string, string> result = new Tuple<string, string, string, string>(string.Empty, string.Empty, string.Empty, string.Empty);

                using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[GetOrderCompleteDateDB]";
                    cmd.Parameters.Add("@OrderID", SqlDbType.Int).Value = OrderID;
                    cmd.Parameters.Add("@Date", SqlDbType.VarChar, 10).Value = result.Item1;
                    cmd.Parameters.Add("@Time", SqlDbType.VarChar, 8).Value = result.Item2;
                    cmd.Parameters.Add("@CreatedPeriodID", SqlDbType.VarChar, 10).Value = result.Item3;
                    cmd.Parameters.Add("@CompletedPeriodID", SqlDbType.VarChar, 10).Value = result.Item4;

                    cmd.Parameters["@Date"].Direction = ParameterDirection.Output;
                    cmd.Parameters["@Time"].Direction = ParameterDirection.Output;
                    cmd.Parameters["@CreatedPeriodID"].Direction = ParameterDirection.Output;
                    cmd.Parameters["@CompletedPeriodID"].Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();
                    result = Tuple.Create(cmd.Parameters["@Date"].Value.ToString(), cmd.Parameters["@Time"].Value.ToString(), cmd.Parameters["@CreatedPeriodID"].Value.ToString(), cmd.Parameters["@CompletedPeriodID"].Value.ToString());
                }

                return result;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public string GetInvoiceNumberByOrderID(int orderID)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {  { "@OrderID", orderID },
                                                                                         };

                SqlCommand cmd = DataAccess.GetCommand("uspGetInvoiceNumberByOrderID", parameters, "Core") as SqlCommand;
                cmd.Connection.Open();
                string InvoiceNumber = cmd.ExecuteScalar().ToString();
                cmd.Connection.Close();
                return InvoiceNumber;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public string GetOrderNumberByInvoiceNumber(string InvoiceNumber)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {  { "@InvoiceNumber", InvoiceNumber },
                                                                                         };

                SqlCommand cmd = DataAccess.GetCommand("uspGetOrderNumberByInvoiceNumber", parameters, "Core") as SqlCommand;
                cmd.Connection.Open();
                string OrderNumber  = cmd.ExecuteScalar().ToString();
                cmd.Connection.Close();
                return OrderNumber;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
        /*CS.20AGO2016.Fin*/

        //INI - GR4172
        public List<Tuple<int, string>> GetOrderAndPeriods(List<int> ListOrderID)
        {
            List<Tuple<int, string>> result = null;
            try
            {
                DataTable dtListOrderID = new DataTable();
                dtListOrderID.Columns.Add("OrderID");

                foreach (var orderID in ListOrderID)
                {
                    dtListOrderID.Rows.Add(orderID);
                }

                Dictionary<string, object> parameters = new Dictionary<string, object>() 
                { 
                    { "@ListOrderID", dtListOrderID }
                };

                SqlDataReader reader = DataAccess.GetDataReader("upsGetOrderAndPeriods", parameters, "Core");

                if (reader.HasRows)
                {
                    result = new List<Tuple<int, string>>();
                    while (reader.Read())
                    {
                        result.Add(Tuple.Create(Convert.ToInt32(reader["OrderId"]), reader["PeriodID"].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }

            return result;
        }
        //FIN - GR4172

    }
}