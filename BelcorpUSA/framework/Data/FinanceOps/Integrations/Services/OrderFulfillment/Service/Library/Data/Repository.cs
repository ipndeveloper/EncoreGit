using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using log4net;
using NetSteps.Integrations.Service.GrossRevenue;
using NetSteps.Integrations.Service.OrderExport;
using NetSteps.Integrations.Service.ShippedRevenue;
using NetstepsDataAccess.DataEntities;
using NetstepsDataAccess.Security;

namespace NetSteps.Integrations.Service
{
    public static class Repository
    {
        internal static readonly ILog logger = LogManager.GetLogger(typeof(Repository));

        public static string GetGrossRevenue(DateTime fromDate, DateTime toDate, string CountryISOCode)
        {
            FinancialsGrossRevenue rev = new FinancialsGrossRevenue();
            using (NetStepsEntities db = new NetStepsEntities())
            {
                System.Data.Objects.ObjectResult<uspIntegrationsGetGrossRevenueResult> result = db.uspIntegrationsGetGrossRevenue(fromDate, toDate);
                uspIntegrationsGetGrossRevenueResult res = new uspIntegrationsGetGrossRevenueResult();
                res = result.Single(); // database will always only return a single row
                rev.CreditCardRevenue = new GrossRevenue.Money { Currency = GrossRevenue.Currency.USD, Value = res.CreditCardRevenue };
                rev.CashRevenue = new GrossRevenue.Money { Currency = GrossRevenue.Currency.USD, Value = res.CashRevenue };
                rev.GiftCardRevenue = new GrossRevenue.Money { Currency = GrossRevenue.Currency.USD, Value = res.GiftCardRevenue };
                rev.ProductCreditRevenue = new GrossRevenue.Money { Currency = GrossRevenue.Currency.USD, Value = res.ProductCreditRevenue };
                rev.SalesTaxRevenue = new GrossRevenue.Money { Currency = GrossRevenue.Currency.USD, Value = res.SalesTaxRevenue };
                rev.ServiceIncomeRevenue = new GrossRevenue.Money { Currency = GrossRevenue.Currency.USD, Value = res.ServiceIncomeRevenue };
            }
            return rev.Serialize();
        }

        public static string GetShippedRevenue(DateTime fromDate, DateTime toDate, string CountryISOCode)
        {
            FinancialsShippedRevenue rev = new FinancialsShippedRevenue();
            List<ShippedRevenue.OrderItem> items = new List<ShippedRevenue.OrderItem>();
            using (NetStepsEntities db = new NetStepsEntities())
            {
                System.Data.Objects.ObjectResult<uspIntegrationsGetShippedRevenueResult> result = null;
                result = db.uspIntegrationsGetShippedRevenue();

                foreach (uspIntegrationsGetShippedRevenueResult res in result)
                {
                    ShippedRevenue.OrderItem item = new ShippedRevenue.OrderItem();
                    item.ActualPrice = new ShippedRevenue.Money { Currency = ShippedRevenue.Currency.USD, Value = Convert.ToDecimal(res.ActualPrice) };
                    item.QuantityShipped = res.QuantityShipped;
                    item.RetailPrice = new ShippedRevenue.Money { Currency = ShippedRevenue.Currency.USD, Value = res.RetailPrice };
                    item.ShippingCost = new ShippedRevenue.Money { Currency = ShippedRevenue.Currency.USD, Value = res.ShippingCost };
                    item.SKU = res.SKU;
                    item.WholesalePrice = new ShippedRevenue.Money { Currency = ShippedRevenue.Currency.USD, Value = res.WholesalePrice };
                    items.Add(item);
                }
            }

            rev.OrderItem = items;
            return rev.Serialize();
        }

        private static List<OrdersToERP.OrderCustomer> GetOrderCustomersForOrder(int orderID)
        {
            // try parse decmial
            Decimal di;

            List<OrdersToERP.OrderCustomer> list = new List<OrdersToERP.OrderCustomer>();
            using (NetStepsEntities db = new NetStepsEntities())
            {
                var custs = (from c in db.OrderCustomers
                             join a in db.Accounts on c.AccountID equals a.AccountID
                             where c.OrderID == orderID
                             select new
                             {
                                 c.AccountID,
                                 a.AccountTypeID,
                                 c.Account.AccountNumber,
                                 c.OrderCustomerID,
                                 c.Total,
                                 c.Subtotal,
                                 c.ShippingTotal,
                                 c.TaxAmountTotal
                             }).ToList();

                foreach (var cust in custs)
                {
                    OrdersToERP.OrderCustomer xmlCust = new OrdersToERP.OrderCustomer()
                    {
                        AccountType = GetAccountTypeForOrderCustomer(cust.AccountTypeID),
                        CustomerID = cust.AccountNumber,
                        GrandTotal = Decimal.TryParse(cust.Total.ToString(), out di) ? di : new Decimal(),
                        OrderCustomerID = cust.OrderCustomerID,
                        OrderSubTotal = Decimal.TryParse(cust.Subtotal.ToString(), out di) ? di : new Decimal(),
                        ShippingTotal = Decimal.TryParse(cust.ShippingTotal.ToString(), out di) ? di : new Decimal(),
                        TaxTotal = Decimal.TryParse(cust.TaxAmountTotal.ToString(), out di) ? di : new Decimal(),
                        OrderPayment = GetOrderPaymentsForOrderCustomer(cust.OrderCustomerID),
                        OrderItem = GetOrderItemsForOrderCustomer(cust.OrderCustomerID)
                    };
                    list.Add(xmlCust);
                }
            }
            return list;
        }

        private static List<OrdersToERP.OrderItem> GetOrderItemsForOrderCustomer(int orderCustomerID)
        {
            List<OrdersToERP.OrderItem> xmlOrderItems = new List<OrdersToERP.OrderItem>();
            List<NetstepsDataAccess.DataEntities.OrderItem> items = null;

            // get the Orders from the database.
            using (NetStepsEntities db = new NetStepsEntities())
            {
                items = (from o in db.OrderItems
                         where o.OrderCustomerID == orderCustomerID
                         select o).ToList();
            }
            foreach (NetstepsDataAccess.DataEntities.OrderItem item in items)
            {
                DateTime? dateShipped = GetShippedDateForOrderItem(item.OrderItemID);
                OrdersToERP.OrderItem xmlItem = new OrdersToERP.OrderItem()
                {
                    ItemPrice = new OrdersToERP.Money() { Currency = OrdersToERP.Currency.USD, Value = item.ItemPrice },
                    Qty = item.Quantity,
                    SKU = item.SKU,
                    OrderItemID = item.OrderItemID
                };

                bool specifyShippedDate = dateShipped == null ? false : true;

                if (specifyShippedDate)
                {
                    xmlItem.ShippedDateSpecified = true;
                    xmlItem.ShippedDate = Convert.ToDateTime(dateShipped);
                }


                if (item.OrderItemParentTypeID != null)
                {
                    xmlItem.OrderItemParentType = GetOrderItemParentTypeEnumByInt((short)item.OrderItemParentTypeID);
                    xmlItem.OrderItemParentTypeSpecified = true;
                }
                if (item.ParentOrderItemID != null)
                {
                    xmlItem.ParentOrderItemID = Convert.ToInt32(item.ParentOrderItemID);
                    xmlItem.ParentOrderItemIDSpecified = true;
                }
                xmlOrderItems.Add(xmlItem);
            }
            return xmlOrderItems;
        }

        private static DateTime? GetShippedDateForOrderItem(int orderItemID)
        {
            using (NetStepsEntities db = new NetStepsEntities())
            {
                var d = (from i in db.OrderItems
                         join isp in db.OrderShipmentPackageItems on i.OrderItemID equals isp.OrderItemID
                         join osp in db.OrderShipmentPackages on isp.OrderShipmentPackageID equals osp.OrderShipmentPackageID
                         where i.OrderItemID == orderItemID
                         orderby osp.DateShippedUTC descending
                         select osp.DateShippedUTC).FirstOrDefault();
                if (d == DateTime.MinValue)
                    return null;
                return d;
            }
        }

        public static string GetOrdersForERP(List<string> orderNumbers)
        {
            List<NetstepsDataAccess.DataEntities.Order> orders = null;

            // get the Orders from the database.
            using (NetStepsEntities db = new NetStepsEntities())
            {
                var orderStatusShipped = (from s in db.OrderStatuses
                                          where s.Name.ToLower() == "shipped"
                                          select s.OrderStatusID).Single();

                if (db.Orders.Any(o => orderNumbers.Contains(o.OrderNumber)
                    && o.OrderStatusID != orderStatusShipped))
                    return "Error: an Order was requested that is not in a shipped status";

                orders = (from o in db.Orders
                          where orderNumbers.Contains(o.OrderNumber)
                          select o).ToList();

                if (orders.Count() != orderNumbers.Count())
                    return "Error: Not all Orders requested exist";
            }

            // build XML collection
            OrdersToERP.OrderCollection col = new OrdersToERP.OrderCollection();
            foreach (NetstepsDataAccess.DataEntities.Order o in orders)
            {
                DateTime? dateShipped = GetLatestOrderShipmentDate(o.OrderID);
                OrdersToERP.Order xmlOrder = new OrdersToERP.Order()
                {
                    GrandTotal = o.GrandTotal.HasValue ? o.GrandTotal.Value : 0,
                    OrderNumber = o.OrderNumber,
                    OrderDate = o.CompleteDateUTC ?? o.DateCreatedUTC,
                    OrderStatus = GetOrderStatusByID(o.OrderStatusID),
                    OrderSubTotal = o.Subtotal.HasValue ? o.Subtotal.Value : 0,
                    OrderType = GetOrderTypeByID(o.OrderTypeID),
                    ShippingTotal = (o.ShippingTotalOverride.HasValue) ? o.ShippingTotalOverride.Value : (o.ShippingTotal.HasValue) ? o.ShippingTotal.Value : 0,
                    TaxTotal = (o.TaxAmountTotalOverride.HasValue) ? o.TaxAmountTotalOverride.Value : (o.TaxAmountTotal.HasValue) ? o.TaxAmountTotal.Value : 0,
                    OrderCustomer = GetOrderCustomersForOrder(o.OrderID),
                    OrderPayment = GetOrderPaymentsForOrder(o.OrderID)
                };

                bool shippedSpecified = dateShipped == null ? false : true;

                if (shippedSpecified)
                {
                    xmlOrder.ShippedDateSpecified = true;
                    xmlOrder.ShippedDate = Convert.ToDateTime(dateShipped);
                }
                col.Order.Add(xmlOrder);
            }
            return col.Serialize();
        }

        private static DateTime? GetLatestOrderShipmentDate(int orderID)
        {
            using (NetStepsEntities db = new NetStepsEntities())
            {
                var d = (from s in db.OrderShipments
                         join sp in db.OrderShipmentPackages on s.OrderShipmentID equals sp.OrderShipmentID
                         where s.OrderID == orderID
                         orderby sp.DateShippedUTC descending
                         select sp.DateShippedUTC).FirstOrDefault();
                if (d == DateTime.MinValue)
                    return null;
                return d;
            }
        }

        private static List<OrdersToERP.OrderPayment> GetOrderPaymentsForOrderCustomer(int orderCustomerID)
        {
            List<NetstepsDataAccess.DataEntities.OrderPayment> payments = null;
            using (NetStepsEntities db = new NetStepsEntities())
            {
                payments = (from p in db.OrderPayments
                            where p.OrderCustomerID == orderCustomerID
                            select p).ToList();
            }

            List<OrdersToERP.OrderPayment> xmlPayments = new List<OrdersToERP.OrderPayment>();
            foreach (NetstepsDataAccess.DataEntities.OrderPayment pays in payments)
            {
                OrdersToERP.OrderPayment xmlPay = new OrdersToERP.OrderPayment()
                {
                    PaymentAmount = pays.Amount,
                    PaymentType = GetPaymentTypesByID(pays.PaymentTypeID)
                };
                xmlPayments.Add(xmlPay);
            }
            return xmlPayments;
        }

        private static List<OrdersToERP.OrderPayment> GetOrderPaymentsForOrder(int orderID)
        {
            List<NetstepsDataAccess.DataEntities.OrderPayment> payments = null;
            using (NetStepsEntities db = new NetStepsEntities())
            {
                payments = (from p in db.OrderPayments
                            where p.OrderID == orderID && p.OrderCustomerID == null // TODO: verify logic here that an Order OrderPayment has a null OrderCustomerID
                            select p).ToList();
            }

            List<OrdersToERP.OrderPayment> xmlPayments = new List<OrdersToERP.OrderPayment>();
            foreach (NetstepsDataAccess.DataEntities.OrderPayment pays in payments)
            {
                OrdersToERP.OrderPayment xmlPay = new OrdersToERP.OrderPayment()
                {
                    PaymentAmount = pays.Amount,
                    PaymentType = GetPaymentTypesByID(pays.PaymentTypeID)
                };
                xmlPayments.Add(xmlPay);
            }
            return xmlPayments;
        }

        private static OrdersToERP.PaymentType GetPaymentTypesByID(int code)
        {
            using (NetStepsEntities db = new NetStepsEntities())
            {
                try
                {
                    string payments = (from c in db.PaymentTypes
                                       where c.PaymentTypeID == code
                                       select c.Name).Single();
                    switch (payments)
                    {
                        case "Cash": return OrdersToERP.PaymentType.Cash;
                        case "Check": return OrdersToERP.PaymentType.Check;
                        case "Credit Card": return OrdersToERP.PaymentType.CreditCard;
                        case "EFT": return OrdersToERP.PaymentType.EFT;
                        case "Gift Card": return OrdersToERP.PaymentType.GiftCard;
                        case "Product Credit": return OrdersToERP.PaymentType.ProductCredit;
                        default: return OrdersToERP.PaymentType.Cash;
                    }
                }
                catch (Exception ex)
                {
                    db.UspErrorLogsInsert(DateTime.Now, String.Empty, "GetPaymentTypesByID method, Repository class: ", ex.Message);
                    return OrdersToERP.PaymentType.Cash;
                }
            }
        }

        public static string GetAccountsForERP(DateTime startDate, DateTime endDate)
        {
            using (NetStepsEntities db = new NetStepsEntities())
            {
                List<uspIntegrationsAccountsToERPResult> res = db.uspIntegrationsAccountsToERP(startDate, endDate).ToList();
                NetSteps.Integrations.Service.AccountsToERP.AccountExportCollection col = new NetSteps.Integrations.Service.AccountsToERP.AccountExportCollection();
                foreach (uspIntegrationsAccountsToERPResult acct in res)
                {
                    NetSteps.Integrations.Service.AccountsToERP.Account xmlAcct = new NetSteps.Integrations.Service.AccountsToERP.Account();
                    xmlAcct.AccountNumber = acct.AccountNumber;
                    xmlAcct.TaxNumber = acct.TaxNumber;

                    if (acct.IsEntity)
                        xmlAcct.EntityType = AccountsToERP.EntityType.Business;
                    else
                        xmlAcct.EntityType = AccountsToERP.EntityType.Personal;

                    xmlAcct.AccountType = GetAccountTypeByName(acct.AccountTypeName);

                    xmlAcct.Address = new NetSteps.Integrations.Service.AccountsToERP.Address
                    {
                        Address1 = acct.Address1,
                        Address2 = acct.Address2,
                        Address3 = acct.Address3,
                        City = acct.City,
                        CountryISOCode = GetCountryByIDForAccountExport(acct.CountryID),
                        Email = acct.EmailAddress,
                        FirstName = acct.FirstName,
                        LastName = acct.LastName,
                        Phone = acct.PhoneNumber,
                        State = acct.State,
                        Zip = acct.PostalCode
                    };
                    col.Account.Add(xmlAcct);
                }
                return col.Serialize();
            }

        }

        private static AccountsToERP.AccountType GetAccountTypeByName(string name)
        {
            switch (name)
            {
                case "Distributor": return AccountsToERP.AccountType.Distributor;
                case "Employee": return AccountsToERP.AccountType.Employee;
                case "Preferred Customer": return AccountsToERP.AccountType.PreferredCustomer;
                case "Prospect": return AccountsToERP.AccountType.Prospect;
                case "Retail Customer": return AccountsToERP.AccountType.RetailCustomer;
                default: return AccountsToERP.AccountType.Distributor;
            }
        }

        private static NetSteps.Integrations.Service.OrdersToERP.OrderStatus GetOrderStatusByID(int code)
        {
            using (NetStepsEntities db = new NetStepsEntities())
            {
                try
                {
                    string status = (from c in db.OrderStatuses
                                     where c.OrderStatusID == code
                                     select c.Name).Single();
                    switch (status)
                    {
                        case "Pending": return OrdersToERP.OrderStatus.Pending;
                        case "Pending Error": return OrdersToERP.OrderStatus.PendingError;
                        case "Paid": return OrdersToERP.OrderStatus.Paid;
                        case "Cancelled": return OrdersToERP.OrderStatus.Cancelled;
                        case "Partially Paid": return OrdersToERP.OrderStatus.PartiallyPaid;
                        case "Printed": return OrdersToERP.OrderStatus.Printed;
                        case "Shipped": return OrdersToERP.OrderStatus.Shipped;
                        case "Credit Card Declined": return OrdersToERP.OrderStatus.CreditCardDeclined;
                        case "Credit Card Declined Retry": return OrdersToERP.OrderStatus.CreditCardDeclinedRetry;
                        default: return OrdersToERP.OrderStatus.Pending;
                    }
                }
                catch (Exception ex)
                {
                    db.UspErrorLogsInsert(DateTime.Now, String.Empty, "GetOrderStatusByID method, Repository class: ", ex.Message);
                    return OrdersToERP.OrderStatus.Pending;
                }
            }
        }

        private static NetSteps.Integrations.Service.OrdersToERP.AccountType GetAccountTypeForOrderCustomer(int code)
        {

            using (NetStepsEntities db = new NetStepsEntities())
            {
                try
                {
                    string accountType = (from c in db.AccountTypes
                                          where c.AccountTypeID == code
                                          select c.Name).Single();
                    switch (accountType)
                    {
                        case "Distributor": return OrdersToERP.AccountType.Distributor;
                        case "Employee": return OrdersToERP.AccountType.Employee;
                        case "Preferred Customer": return OrdersToERP.AccountType.PreferredCustomer;
                        case "Prospect": return OrdersToERP.AccountType.Prospect;
                        case "Retail Customer": return OrdersToERP.AccountType.RetailCustomer;
                        default: return OrdersToERP.AccountType.Distributor;
                    }
                }
                catch (Exception ex)
                {
                    db.UspErrorLogsInsert(DateTime.Now, String.Empty, "GetAccountTypeForOrderCustomer method, Repository class: ", ex.Message);
                    return OrdersToERP.AccountType.Distributor;
                }
            }
        }


        private static NetSteps.Integrations.Service.OrdersToERP.OrderType GetOrderTypeByID(int code)
        {
            using (NetStepsEntities db = new NetStepsEntities())
            {
                try
                {
                    string type = (from c in db.OrderTypes
                                   where c.OrderTypeID == code
                                   select c.Name).Single();
                    switch (type)
                    {
                        case "Autoship Order": return OrdersToERP.OrderType.AutoshipOrder;
                        case "Autoship Template": return OrdersToERP.OrderType.AutoshipTemplate;
                        case "Comp Order": return OrdersToERP.OrderType.CompOrder;
                        case "Online Order": return OrdersToERP.OrderType.OnlineOrder;
                        case "Override Order": return OrdersToERP.OrderType.OverrideOrder;
                        case "Party Order": return OrdersToERP.OrderType.PartyOrder;
                        case "Portal Order": return OrdersToERP.OrderType.PortalOrder;
                        case "Replacement Order": return OrdersToERP.OrderType.ReplacementOrder;
                        case "Return Order": return OrdersToERP.OrderType.ReturnOrder;
                        case "Workstation Order": return OrdersToERP.OrderType.WorkstationOrder;
                        default: return OrdersToERP.OrderType.OnlineOrder;
                    }
                }
                catch (Exception ex)
                {
                    db.UspErrorLogsInsert(DateTime.Now, String.Empty, "GetOrderTypeByID method, Repository class: ", ex.Message);
                    return OrdersToERP.OrderType.OnlineOrder;
                }
            }
        }

        private static NetSteps.Integrations.Service.AccountsToERP.Country GetCountryByIDForAccountExport(int? code)
        {
            using (NetStepsEntities db = new NetStepsEntities())
            {
                try
                {
                    if (code == null)
                        return NetSteps.Integrations.Service.AccountsToERP.Country.USA; // default to USA
                    int countryID = code.Value;
                    string country = (from c in db.Countries
                                      where c.CountryID == countryID
                                      select c.CountryCode3).Single();
                    switch (country)
                    {
                        case "AUS": return NetSteps.Integrations.Service.AccountsToERP.Country.AUS;
                        case "BEL": return NetSteps.Integrations.Service.AccountsToERP.Country.BEL;
                        case "CAN": return NetSteps.Integrations.Service.AccountsToERP.Country.CAN;
                        case "GBR": return NetSteps.Integrations.Service.AccountsToERP.Country.GBR;
                        case "IRL": return NetSteps.Integrations.Service.AccountsToERP.Country.IRL;
                        case "NLD": return NetSteps.Integrations.Service.AccountsToERP.Country.NLD;
                        case "SWE": return NetSteps.Integrations.Service.AccountsToERP.Country.SWE;
                        case "USA": return NetSteps.Integrations.Service.AccountsToERP.Country.USA;
                        default: return NetSteps.Integrations.Service.AccountsToERP.Country.USA;
                    }
                }
                catch (Exception ex)
                {
                    string codeNotFoundMessage = string.Format("(Code not found for code(countryID): '{0}')", code.HasValue ? code.Value.ToString() : "null");
                    db.UspErrorLogsInsert(DateTime.Now, String.Empty, "GetCountryByID method, Repository class: " + codeNotFoundMessage, ex.Message);
                    return NetSteps.Integrations.Service.AccountsToERP.Country.USA;
                }
            }
        }

        //public List<NetSteps.OrderFulfillment.Order> GetOrdersFromRepositoryToFulfill()
        //{
        //    List<NetSteps.OrderFulfillment.Order> xmlOrders = new List<OrderFulfillment.Order>();
        //    using (NetStepsEntities db = new NetStepsEntities())
        //    {
        //        List<NetstepsDataAccess.DataEntities.Order> oList = db.uspSelectLogisticsProviderOrders().ToList();
        //        foreach (NetstepsDataAccess.DataEntities.Order o in oList)
        //        {
        //            NetSteps.OrderFulfillment.Order xmlOrder = new OrderFulfillment.Order();
        //            List<OrderPayment> payments = (from d in db.OrderPayments
        //                                           where d.OrderID == o.OrderID
        //                                           select d).ToList();
        //            List<Address> addresses = new List<Address>();
        //            foreach (OrderPayment p in payments)
        //            {
        //                Address a = new Address();
        //                a.Address1 = p.BillingAddress1;
        //                a.Address2 = p.BillingAddress2;
        //                a.City = p.BillingCity;
        //                a.Company = p.BillingName;
        //                a.CountryISOCode = GetCountryByID(p.BillingCountryID);
        //                a.FirstName = p.BillingFirstName;
        //                a.LastName = p.BillingLastName;
        //                a.Phone = p.BillingPhoneNumber;
        //                a.State = p.BillingState;
        //                a.Zip = p.BillingPostalCode;
        //                addresses.Add(a);
        //            }
        //            xmlOrder.Billing = addresses;
        //            IDs id = new IDs();
        //            id.PrimaryID = o.OrderID;
        //            xmlOrder.IDs = id;
        //            xmlOrder.Status = OrderStatus.Paid;
        //            xmlOrder.Type = OrderFulfillment.OrderType.OnlineOrder;
        //            xmlOrder.OrderCustomer = GetOrderCustomersByOrderID(o.OrderID);
        //            xmlOrders.Add(xmlOrder);
        //        }
        //    }
        //    return xmlOrders;
        //}

        //private List<NetSteps.OrderFulfillment.OrderCustomer> GetOrderCustomersByOrderID(int orderID)
        //{
        //    List<NetSteps.OrderFulfillment.OrderCustomer> custs = new List<OrderFulfillment.OrderCustomer>();
        //    using (NetStepsEntities db = new NetStepsEntities())
        //    {
        //        List<NetstepsDataAccess.DataEntities.OrderCustomer> linqCusts = (from c in db.OrderCustomers
        //                                                                         where c.OrderID == orderID
        //                                                                         select c).ToList();

        //        foreach (NetstepsDataAccess.DataEntities.OrderCustomer c in linqCusts)
        //        {
        //            NetSteps.OrderFulfillment.OrderCustomer cust = new OrderFulfillment.OrderCustomer();
        //            IDs id = new IDs();
        //            id.PrimaryID = c.OrderCustomerID;
        //            cust.IDs = id;
        //            List<OrderCustomerShipping> shipping = new List<OrderCustomerShipping>();
        //            List<Address> addresses = new List<Address>();
        //            // List<OrderCustomerWarehouse> ware = new List<OrderCustomerWarehouse>();
        //            OrderCustomerShipping ship = new OrderCustomerShipping();
        //            foreach (NetstepsDataAccess.DataEntities.OrderShipment s in c.OrderShipments.AsEnumerable())
        //            {
        //                // test data
        //                ship.ShippingMethod = "2 Day Ground";
        //                ship.ShippingProvider = "FedEx";
        //                Address a = new Address();
        //                a.Address1 = s.Address1;
        //                a.Address2 = a.Address2;
        //                a.City = a.City;
        //                a.Company = s.Name;
        //                a.CountryISOCode = GetCountryByID(s.CountryID);
        //                a.Email = s.Email;
        //                a.FirstName = s.FirstName;
        //                a.LastName = s.LastName;
        //                a.Phone = s.DayPhone;
        //                a.State = s.State;
        //                a.Zip = s.PostalCode;
        //                addresses.Add(a);
        //            }
        //            ship.Address = addresses;
        //            shipping.Add(ship);
        //            cust.Shipping = shipping;
        //            cust.OrderItem = GetOrderItemsByOrderCustomerID(c.OrderCustomerID);
        //            custs.Add(cust);
        //        }
        //    }
        //    return custs;
        //}

        //private static List<NetSteps.Integrations.Service.Fulfillment.OrderItem> GetOrderItemsByOrderCustomerID(int orderCustomerID)
        //{
        //    int ti;
        //    List<NetSteps.Integrations.Service.Fulfillment.OrderItem> items = new List<Integrations.Service.Fulfillment.OrderItem>();
        //    using (NetStepsEntities db = new NetStepsEntities())
        //    {
        //        List<NetstepsDataAccess.DataEntities.OrderItem> linqItems = (from i in db.OrderItems
        //                                                                     where i.OrderCustomerID == orderCustomerID
        //                                                                     select i).ToList();
        //        foreach (NetstepsDataAccess.DataEntities.OrderItem item in linqItems.AsEnumerable())
        //        {
        //            NetSteps.Integrations.Service.Fulfillment.OrderItem i = new Integrations.Service.Fulfillment.OrderItem();
        //            IDs id = new IDs();
        //            Money m = new Money();
        //            m.Currency = Currency.USD;
        //            m.Value = item.ItemPrice;
        //            id.PrimaryID = item.OrderItemID;
        //            i.IDs = id;
        //            i.ItemPrice = m;
        //            i.ItemWeight = WeightSystem.lbs;
        //            i.ProductID = Int32.TryParse(item.ProductID.ToString(), out ti) ? ti : new int();
        //            i.Qty = item.Quantity;
        //            i.Sku = item.SKU;
        //            items.Add(i);
        //        }
        //    }
        //    return items;
        //}

        private static Fulfillment.Country GetCountryByID(int? code)
        {
            using (NetStepsEntities db = new NetStepsEntities())
            {
                try
                {
                    if (code == null)
                        return Fulfillment.Country.US; // default to USA
                    int countryId = code.Value;
                    string country = (from c in db.Countries
                                      where c.CountryID == countryId
                                      select c.CountryCode3).Single();
                    switch (country)
                    {
                        case "US": return Fulfillment.Country.US;
                        case "CA": return Fulfillment.Country.CA;
                        case "AU": return Fulfillment.Country.AU;
                        case "GB": return Fulfillment.Country.GB;
                        case "IE": return Fulfillment.Country.IE;
                        case "SE": return Fulfillment.Country.SE;
                        case "NL": return Fulfillment.Country.NL;
                        case "BE": return Fulfillment.Country.BE;
                        case "AT": return Fulfillment.Country.AT;
                        case "CZ": return Fulfillment.Country.CZ;
                        case "DK": return Fulfillment.Country.DK;
                        case "FI": return Fulfillment.Country.FI;
                        case "FR": return Fulfillment.Country.FR;
                        case "DE": return Fulfillment.Country.DE;
                        case "IT": return Fulfillment.Country.IT;
                        case "NO": return Fulfillment.Country.NO;
                        case "PL": return Fulfillment.Country.PL;
                        case "SK": return Fulfillment.Country.SK;
                        case "CH": return Fulfillment.Country.CH;
                        default: return Fulfillment.Country.US;
                    }
                }
                catch (Exception ex)
                {
                    string codeNotFoundMessage = string.Format("(Code not found for code(countryID): '{0}')", code.HasValue ? code.Value.ToString() : "null");
                    db.UspErrorLogsInsert(DateTime.Now, String.Empty, "GetCountryByID method, Repository class: " + codeNotFoundMessage, ex.Message);
                    return Fulfillment.Country.US;
                }
            }
        }

        // deprecated
        public static string GetOrders()
        {
            List<uspIntegrationsOrderExportResult> list = null;
            using (NetStepsEntities db = new NetStepsEntities())
                list = db.uspIntegrationsOrderExport().ToList();

            if (list.Count > 0)
            {
                // apply group by clause
                var GroupedList = (from l in list
                                   group l by new { l.AccountID, l.DateCreatedUTC, l.OrderNumber }
                                       into grp
                                       select new { grp.Key.AccountID, grp.Key.DateCreatedUTC, grp.Key.OrderNumber }).ToList();

                // Create Order Collection XML
                OrderExportCollection col = new OrderExportCollection();
                // Create List of Orders for XML
                List<OrderExport.Order> orders = new List<OrderExport.Order>();

                foreach (var g in GroupedList)
                {
                    // create order items list
                    List<OrderExport.OrderItem> listOfItems = new List<OrderExport.OrderItem>();

                    // get items for this Order
                    var items = from i in list
                                where i.OrderNumber == g.OrderNumber
                                select i;

                    // add each item for this order to the list
                    foreach (var item in items)
                    {
                        OrderExport.OrderItem orderItem = new OrderExport.OrderItem()
                        {
                            OrderItemID = item.OrderItemID,
                            Quantity = item.Quantity,
                            SKU = item.SKU
                        };
                        listOfItems.Add(orderItem);
                    }

                    // create an Order and add the list of items.
                    OrderExport.Order o = new OrderExport.Order()
                    {
                        CustomerID = g.AccountID,
                        OrderDate = g.DateCreatedUTC,
                        OrderNumber = g.OrderNumber,
                        OrderItem = listOfItems
                    };
                    orders.Add(o);
                }

                // add Order to XML Order Collection and return Serialized XML
                col.Order = orders;
                return col.Serialize();
            }
            return null;
        }

        internal static string UpdateInventory(string data)
        {
            InventoryResponse.InventoryCollection responseCol = new InventoryResponse.InventoryCollection();
            using (NetStepsEntities db = new NetStepsEntities())
            {
                var inventoryUpdateSucceded = new ObjectParameter("Succeeded", typeof(bool));
                var inventoryUpdateErrorMessage = new ObjectParameter("ErrorMessage", typeof(string));
                List<uspIntegrationsUpdateInventoryResult> res = db.uspIntegrationsUpdateInventory(IntegrationsSecurity.ModifiedByUserID(),
                    data, inventoryUpdateSucceded, inventoryUpdateErrorMessage).ToList();

                if (!Convert.ToBoolean(inventoryUpdateSucceded.Value))
                    return inventoryUpdateErrorMessage.Value.ToString();

                foreach (uspIntegrationsUpdateInventoryResult r in res)
                {
                    InventoryResponse.WarehouseProduct product = new InventoryResponse.WarehouseProduct();
                    product.Succeeded = true;
                    product.SKU = r.SKU;
                    product.WarehouseID = r.WarehouseID;
                    responseCol.WarehouseProduct.Add(product);
                }
            }
            return responseCol.Serialize();
        }

        internal static string GetDisbursements(int periodID)
        {
            Disbursements.DisbursementCollection disbursementCollection = new Disbursements.DisbursementCollection();

            // validate that disbursements can be retrieved for this period
            var disbursementRetrievalSucceded = new ObjectParameter("Succeeded", typeof(bool));
            var disbursementRetrievalErrorMessage = new ObjectParameter("ErrorMessage", typeof(string));

            using (NetstepsDataAccess.DataEntities.Commissions.NetStepsCommissionsDBContainer db = new NetstepsDataAccess.DataEntities.Commissions.NetStepsCommissionsDBContainer())
            {
                db.uspIntegrationsValidateDisbursementsRetrieval(periodID, disbursementRetrievalSucceded, disbursementRetrievalErrorMessage);

                if (!Convert.ToBoolean(disbursementRetrievalSucceded.Value))
                    return disbursementRetrievalErrorMessage.Value.ToString();

                disbursementCollection.PeriodID = periodID;
                disbursementCollection.EffectiveDate = (from p in db.Periods
                                                        where p.PeriodID == periodID
                                                        select p.EndDate).Single();
            }

            // disbursement retrieval succeeded, get disbursements from the database
            disbursementCollection.Disbursement = GetDisbursementDataFromCommissions(periodID);

            // return serialized XML
            return disbursementCollection.Serialize();
        }

        private static List<Disbursements.Disbursement> GetDisbursementDataFromCommissions(int periodID)
        {
            List<Disbursements.Disbursement> xmlDisbursements = new List<Disbursements.Disbursement>();
            using (NetstepsDataAccess.DataEntities.Commissions.NetStepsCommissionsDBContainer db = new NetstepsDataAccess.DataEntities.Commissions.NetStepsCommissionsDBContainer())
            {
                // linq to get disbursements from database
                List<NetstepsDataAccess.DataEntities.Commissions.Disbursement> linqDisbursements = null;
                linqDisbursements = (from d in db.Disbursements
                                     where d.PeriodID == periodID
                                     select d).ToList();

                foreach (NetstepsDataAccess.DataEntities.Commissions.Disbursement linqDisbursement in linqDisbursements)
                {
                    var acctInfo = (from a in db.Accounts
                                    where a.AccountID == linqDisbursement.AccountID
                                    select new { a.AccountNumber, a.FirstName, a.LastName, a.IsEntity }).Single(); // will fail if Account doesn't exist

                    Disbursements.Disbursement xmlDisbursement = new Disbursements.Disbursement();
                    xmlDisbursement.AccountNumber = acctInfo.AccountNumber;
                    xmlDisbursement.Amount = linqDisbursement.Amount;
                    xmlDisbursement.FirstName = acctInfo.FirstName;
                    xmlDisbursement.LastName = acctInfo.LastName;
                    xmlDisbursement.DisbursementDetail = GetDisbursementDetails(linqDisbursement.DisbursementID);
                    xmlDisbursement.EntityType = (acctInfo.IsEntity) ? Disbursements.EntityType.Business : Disbursements.EntityType.Individual;

                    xmlDisbursements.Add(xmlDisbursement);
                }
            }

            return xmlDisbursements;
        }

        private static List<Disbursements.DisbursementDetail> GetDisbursementDetails(int disbursementID)
        {
            List<Disbursements.DisbursementDetail> xmlDetails = new List<Disbursements.DisbursementDetail>();
            // linq to get disbursement details from database
            List<NetstepsDataAccess.DataEntities.Commissions.DisbursementDetail> linqDetails = null;
            using (NetstepsDataAccess.DataEntities.Commissions.NetStepsCommissionsDBContainer db = new NetstepsDataAccess.DataEntities.Commissions.NetStepsCommissionsDBContainer())
            {
                linqDetails = (from d in db.DisbursementDetails
                               where d.DisbursementID == disbursementID
                               select d).ToList();

                foreach (NetstepsDataAccess.DataEntities.Commissions.DisbursementDetail linqDetail in linqDetails)
                {
                    Disbursements.DisbursementDetail xmlDetail = new Disbursements.DisbursementDetail();
                    xmlDetail.Percentage = Convert.ToDecimal(linqDetail.Percentage); // database incorrectly allows NULLs
                    xmlDetail.DisbursementStatus = Disbursements.DisbursementStatus.Entered; // must be in an entered status if we've made it here.
                    xmlDetail.DisbursementType = GetDisbursementTypeByName(linqDetail.DisbursementType.Name);
                    xmlDetail.DisbursementProfile = GetDisbursementProfile(linqDetail.DisbursementDetailID);
                    xmlDetail.DisbursementDetailID = linqDetail.DisbursementDetailID;

                    // if check disbursement type, populate check number
                    if (linqDetail.DisbursementType != null && linqDetail.DisbursementType.Code.ToUpper() == "CK")
                    {
                        xmlDetail.CheckNumber = (from c in db.Checks
                                                 where c.DisbursementDetailID == linqDetail.DisbursementDetailID
                                                 select c.CheckNumber).FirstOrDefault();
                    }

                    xmlDetails.Add(xmlDetail);
                }
            }
            return xmlDetails;
        }

        private static Disbursements.DisbursementProfile GetDisbursementProfile(int disbursementDetailID)
        {
            Disbursements.DisbursementProfile xmlProfile = new Disbursements.DisbursementProfile();
            NetstepsDataAccess.DataEntities.Commissions.uspIntegrationsGetDisbursementProfileDataResult profileData = null;
            // linq to retrieve disbursement profile data from the database.
            using (NetstepsDataAccess.DataEntities.Commissions.NetStepsCommissionsDBContainer db = new NetstepsDataAccess.DataEntities.Commissions.NetStepsCommissionsDBContainer())
            {
                profileData = db.uspIntegrationsGetDisbursementProfileData(disbursementDetailID).Single(); // database will only return one row.
            }

            xmlProfile.AccountAddress = new Disbursements.DisbursementProfileAccountAddress()
            {
                Address = new Disbursements.Address()
                {
                    Address1 = profileData.AddressLine1,
                    Address2 = profileData.AddressLine2,
                    City = profileData.Locality,
                    CountryISOCode = GetDisbursementCountryByName(profileData.CountryISOCode),
                    State = profileData.StateProvince,
                    Zip = profileData.PostalCode
                }
            };
            xmlProfile.BankAccountNumber = profileData.BankAccountNumber;
            xmlProfile.BankName = profileData.BankName;
            xmlProfile.BankPhone = profileData.BankPhone;
            xmlProfile.EnrollmentFormReceived = Convert.ToBoolean(profileData.EnrollmentFormReceived);
            xmlProfile.NameOnAccount = profileData.NameOnAccount;
            xmlProfile.RoutingNumber = profileData.RoutingNumber;
            xmlProfile.AccountType = GetDisbursementProfileAccountType(profileData.AccountType);
            return xmlProfile;
        }

        private static Disbursements.AccountType GetDisbursementProfileAccountType(string code)
        {
            if (!String.IsNullOrEmpty(code) && code.ToLower() == "savings")
                return Disbursements.AccountType.Savings;
            return Disbursements.AccountType.Checking; // default to Checking

        }

        private static Disbursements.Country GetDisbursementCountryByName(string country)
        {
            switch (country)
            {
                case "US": return Disbursements.Country.US;
                case "CA": return Disbursements.Country.CA;
                case "AU": return Disbursements.Country.AU;
                case "GB": return Disbursements.Country.GB;
                case "IE": return Disbursements.Country.IE;
                case "SE": return Disbursements.Country.SE;
                case "NL": return Disbursements.Country.NL;
                case "BE": return Disbursements.Country.BE;
                case "AT": return Disbursements.Country.AT;
                case "CZ": return Disbursements.Country.CZ;
                case "DK": return Disbursements.Country.DK;
                case "FI": return Disbursements.Country.FI;
                case "FR": return Disbursements.Country.FR;
                case "DE": return Disbursements.Country.DE;
                case "IT": return Disbursements.Country.IT;
                case "NO": return Disbursements.Country.NO;
                case "PL": return Disbursements.Country.PL;
                case "SK": return Disbursements.Country.SK;
                case "CH": return Disbursements.Country.CH;
                default: return Disbursements.Country.US;
            }
        }

        private static Disbursements.DisbursementType GetDisbursementTypeByName(string disbursementTypeName)
        {
            using (NetStepsEntities db = new NetStepsEntities())
            {
                try
                {
                    if (disbursementTypeName == null)
                        return Disbursements.DisbursementType.Check; // default to check
                    switch (disbursementTypeName.ToLower())
                    {
                        case "check": return Disbursements.DisbursementType.Check;
                        case "eft": return Disbursements.DisbursementType.EFT;
                        default: return Disbursements.DisbursementType.Check;
                    }
                }
                catch (Exception ex)
                {
                    db.UspErrorLogsInsert(DateTime.Now, String.Empty, "GetDisbursementTypeByName method, Repository class: ", ex.Message);
                    return Disbursements.DisbursementType.Check;
                }
            }
        }

        //private static List<Disbursements.Disbursement> GetFakeData()
        //{
        //    List<Disbursements.Disbursement> disbursements = new List<Disbursements.Disbursement>();

        //    // first disbursement
        //    Disbursements.Disbursement d = new Disbursements.Disbursement();
        //    d.AccountNumber = "52";
        //    d.DisbursementType = "Check";
        //    d.FirstName = "Mark";
        //    d.LastName = "Clarke";
        //    d.DisbursementProfile = GetFakeDisbursementProfile(1);

        //    // second disbursement
        //    Disbursements.Disbursement dd = new Disbursements.Disbursement();
        //    dd.AccountNumber = "44";
        //    dd.DisbursementType = "EFT";
        //    dd.FirstName = "Kimberly";
        //    dd.LastName = "Washington";
        //    dd.DisbursementProfile = GetFakeDisbursementProfile(2);
        //    disbursements.Add(d);
        //    disbursements.Add(dd);
        //    return disbursements;
        //}

        //private static List<Disbursements.DisbursementProfile> GetFakeDisbursementProfile(int checkOrEft)
        //{
        //    List<Disbursements.DisbursementProfile> list = new List<Disbursements.DisbursementProfile>();
        //    if (checkOrEft == 1)
        //    {
        //        Disbursements.DisbursementProfile prof = new Disbursements.DisbursementProfile();
        //        prof.BankAccountNumber = "546546465";
        //        prof.BankAddress = new Disbursements.DisbursementProfileBankAddress() {  Address = new Disbursements.Address() { Address1 = "123 Brior Stree" } };
        //        prof.BankName = "Credit Union";
        //        prof.BankPhone = "(492) 789-1544";
        //        prof.NameOnAccount = "Mark Clarke";
        //        prof.RoutingNumber = "44646456454";
        //        list.Add(prof);
        //    }
        //    else if (checkOrEft == 2)
        //    {
        //        Disbursements.DisbursementProfile prof2 = new Disbursements.DisbursementProfile();
        //        prof2.BankAccountNumber = "454546465";
        //        prof2.BankAddress = new Disbursements.DisbursementProfileBankAddress() { Address = new Disbursements.Address() { Address1 = "123 Brior Stree" } };
        //        prof2.BankName = "Bank of America";
        //        prof2.BankPhone = "(492) 789-2000";
        //        prof2.NameOnAccount = "Kimberly Washington";
        //        prof2.RoutingNumber = "54678797";
        //        list.Add(prof2);
        //    }

        //    return list;
        //}

        internal static List<Fulfillment.Order> GetOrdersToFulfill()
        {
            List<Fulfillment.Order> orders = new List<Fulfillment.Order>();
            using (NetStepsEntities db = new NetStepsEntities())
            {
                List<uspIntegrationsGetOrdersToFulfillResult> oList = db.uspIntegrationsGetOrdersToFulfill().ToList();

                logger.Debug(string.Format("GetOrdersToFulfill - processing {0} orders", oList.Count));


                var orderStatuses = db.OrderStatuses.ToList();

                foreach (uspIntegrationsGetOrdersToFulfillResult o in oList)
                {
                    Fulfillment.Order xmlOrder = new Fulfillment.Order();

                    var ship = (from d in db.OrderShipments
                                join cust in db.OrderCustomers on d.OrderCustomerID equals cust.OrderCustomerID into JoinedOrderCustomer
                                from djoin in JoinedOrderCustomer.DefaultIfEmpty() // make it a left join
                                where d.OrderShipmentID == o.OrderShipmentID
                                select new
                                {
                                    d.Address1,
                                    d.Address2,
                                    d.City,
                                    d.Email,
                                    FirstName = String.IsNullOrEmpty(d.FirstName) ? djoin.Account.FirstName : d.FirstName,
                                    LastName = String.IsNullOrEmpty(d.LastName) ? djoin.Account.LastName : d.LastName,
                                    Name = String.IsNullOrEmpty(d.Name) ?
                                                djoin != null ?
                                                        djoin.Account != null ? djoin.Account.FirstName + djoin.Account.LastName : ""
                                                : ""
                                           : d.Name,
                                    d.DayPhone,
                                    d.State,
                                    d.PostalCode,
                                    d.ShippingMethod
                                }).SingleOrDefault();
                    if(ship != null)
                    xmlOrder.Shipping.Address = new Fulfillment.Address()
                    {
                        Address1 = ship.Address1,
                        Address2 = ship.Address2,
                        City = ship.City,
                        Email = ship.Email,
                        FirstName = ship.FirstName,
                        LastName = ship.LastName,
                        Name = ship.Name,
                        Phone = ship.DayPhone,
                        State = ship.State,
                        Zip = ship.PostalCode
                    };

                    xmlOrder.NumberOfPartyOrders = Convert.ToInt32(o.NumberOfPartyOrders);
                    xmlOrder.OrderDate = o.CompleteDate.HasValue ? o.CompleteDate.Value : new DateTime();

                    // pull shipping method from the database.
                    if (ship != null && ship.ShippingMethod != null)
                        xmlOrder.Shipping.ShippingMethod = ship.ShippingMethod.Name;

                    //xmlOrder.Shipping.ShippingProvider = "FedEx";

                    List<OrderPayment> payments = (from d in db.OrderPayments
                                                   where d.OrderID == o.OrderID
                                                   select d).ToList();
                    List<Fulfillment.Address> addresses = new List<Fulfillment.Address>();
                    foreach (OrderPayment p in payments)
                    {
                        Fulfillment.Address a = new Fulfillment.Address();
                        a.Address1 = p.BillingAddress1;
                        a.Address2 = p.BillingAddress2;
                        a.City = p.BillingCity;
                        a.Company = p.BillingName;
                        a.CountryISOCode = GetCountryByID(p.BillingCountryID);
                        a.FirstName = p.BillingFirstName;
                        a.LastName = p.BillingLastName;
                        a.Phone = p.BillingPhoneNumber;
                        a.State = p.BillingState;
                        a.Zip = p.BillingPostalCode;
                        addresses.Add(a);
                    }
                    xmlOrder.Billing = addresses;
                    xmlOrder.OrderNumber = o.OrderNumber;
                    xmlOrder.Status = GetOrderStatusByIDForOrderFulfillment(o.OrderStatusID, orderStatuses);
                    xmlOrder.Type = GetOrderTypeByIDForOrderFulfillment(o.OrderTypeID);
                    if(o.OrderShipmentID > 0)
                    xmlOrder.OrderItem = GetOrderItemsByOrderShipmentID(o.OrderShipmentID);

                    // add to list
                    orders.Add(xmlOrder);
                }
            }
            return orders;
        }

        private static Fulfillment.OrderType GetOrderTypeByIDForOrderFulfillment(short orderTypeID)
        {
            string typeName = "";
            using (NetStepsEntities db = new NetStepsEntities())
            {
                typeName = (from t in db.OrderTypes
                            where t.OrderTypeID == orderTypeID
                            select t.Name).Single();
            }
            switch (typeName)
            {
                case "Autoship Order": return Fulfillment.OrderType.AutoshipOrder;
                case "Autoship Template": return Fulfillment.OrderType.AutoshipTemplate;
                case "Comp Order": return Fulfillment.OrderType.CompOrder;
                case "Online Order": return Fulfillment.OrderType.OnlineOrder;
                case "Override Order": return Fulfillment.OrderType.OverrideOrder;
                case "Party Order": return Fulfillment.OrderType.PartyOrder;
                case "Portal Order": return Fulfillment.OrderType.PortalOrder;
                case "Replacement Order": return Fulfillment.OrderType.ReplacementOrder;
                case "Return Order": return Fulfillment.OrderType.ReturnOrder;
                case "Workstation Order": return Fulfillment.OrderType.WorkstationOrder;
                case "Enrollment Order": return Fulfillment.OrderType.EnrollmentOrder;
                default: return Fulfillment.OrderType.OnlineOrder;
            }
        }

        private static Fulfillment.OrderStatus GetOrderStatusByIDForOrderFulfillment(short orderStatusID, List<OrderStatus> orderStatuses)
        {
            string statusName = "";
            using (NetStepsEntities db = new NetStepsEntities())
            {
                try
                {
                    statusName = (from s in orderStatuses
                                  where s.OrderStatusID == orderStatusID
                                  select s.Name).Single();
                }
                catch
                {
                    statusName = (from s in db.OrderStatuses
                                  where s.OrderStatusID == orderStatusID
                                  select s.Name).Single();
                }
            }
            switch (statusName)
            {
                case "Pending": return Fulfillment.OrderStatus.Pending;
                case "Pending Error": return Fulfillment.OrderStatus.PendingError;
                case "Paid": return Fulfillment.OrderStatus.Paid;
                case "Cancelled": return Fulfillment.OrderStatus.Cancelled;
                case "Partially Paid": return Fulfillment.OrderStatus.PartiallyPaid;
                case "Printed": return Fulfillment.OrderStatus.Printed;
                case "Shipped": return Fulfillment.OrderStatus.Shipped;
                case "Credit Card Declined": return Fulfillment.OrderStatus.CreditCardDeclined;
                case "Credit Card Declined Retry": return Fulfillment.OrderStatus.CreditCardDeclinedRetry;
                default: return Fulfillment.OrderStatus.Pending;
            }
        }

        private static List<Fulfillment.OrderItem> GetOrderItemsByOrderShipmentID(int orderShipmentID)
        {
            List<Fulfillment.OrderItem> items = new List<Fulfillment.OrderItem>();
            using (NetStepsEntities db = new NetStepsEntities())
            {
                var procSucceded = new ObjectParameter("Succeeded", typeof(bool));
                var procErrorMessage = new ObjectParameter("ErrorMessage", typeof(string));

                List<uspGetOrderItemsByOrderShipmentIDResult> res = db.uspGetOrderItemsByOrderShipmentID(orderShipmentID, procSucceded, procErrorMessage).ToList();
                if (!Convert.ToBoolean(procSucceded.Value))
                    throw new Exception(procErrorMessage.Value.ToString());

                foreach (uspGetOrderItemsByOrderShipmentIDResult result in res)
                {
                    Fulfillment.OrderItem xmlItem = new Fulfillment.OrderItem();
                    if (!String.IsNullOrEmpty(result.OrderItemParentType))
                    {
                        xmlItem.OrderItemParentType = GetOrderItemParentTypeEnumByString(result.OrderItemParentType);
                        xmlItem.OrderItemParentTypeSpecified = true;
                    }
                    if (result.ParentOrderItemID != null)
                    {
                        xmlItem.ParentOrderItemID = Convert.ToInt32(result.ParentOrderItemID);
                        xmlItem.ParentOrderItemIDSpecified = true;
                    }
                    xmlItem.OrderItemID = result.OrderItemID;
                    xmlItem.ItemPrice = new Fulfillment.Money() { Value = result.ItemPrice, Currency = Fulfillment.Currency.USD };
                    xmlItem.Qty = result.Quantity;
                    xmlItem.Sku = result.SKU;
                    items.Add(xmlItem);
                }
            }
            return items;
        }

        private static Fulfillment.OrderItemParentType GetOrderItemParentTypeEnumByString(string parentType)
        {
            switch (parentType.ToLower())
            {
                case "dynamic kit": return Fulfillment.OrderItemParentType.DynamicKit;
                case "static kit": return Fulfillment.OrderItemParentType.StaticKit;
                default: return Fulfillment.OrderItemParentType.DynamicKit;
            }
        }

        private static OrdersToERP.OrderItemParentType GetOrderItemParentTypeEnumByInt(short parentTypeID)
        {
            using (NetStepsEntities db = new NetStepsEntities())
            {
                int dynamicInt = (from p in db.OrderItemParentTypes
                                  where p.Name.ToLower() == "dynamic kit"
                                  select p.OrderItemParentTypeID).Single();
                int staticInt = (from p in db.OrderItemParentTypes
                                 where p.Name.ToLower() == "static kit"
                                 select p.OrderItemParentTypeID).Single();
                if (parentTypeID == dynamicInt)
                    return OrdersToERP.OrderItemParentType.DynamicKit;
                else if (parentTypeID == staticInt)
                    return OrdersToERP.OrderItemParentType.StaticKit;
                else // default
                    return OrdersToERP.OrderItemParentType.StaticKit;
            }
        }
    }
}

