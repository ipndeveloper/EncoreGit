using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Common.Configuration;
using NetSteps.Diagnostics.Utilities;
using NetstepsDataAccess.DataEntities;
using NetSteps.Integrations.Service.GrossRevenue;
using NetSteps.Integrations.Service.ShippedRevenue;
using NetstepsDataAccess.DataEntities.Commissions;
using NetSteps.Integrations.Internals.Security;

namespace NetSteps.Integrations.Service
{
    public static class Repository
    {
        internal static string GetDisbursements(int periodID)
        {
            Disbursements.DisbursementCollection col = new Disbursements.DisbursementCollection();


            // validate that disbursements can be retrieved for this period
            var disbursementRetrievalSucceded = new ObjectParameter("Succeeded", typeof(bool));
            var disbursementRetrievalErrorMessage = new ObjectParameter("ErrorMessage", typeof(string));

            using (NetstepsDataAccess.DataEntities.Commissions.NetStepsCommissionsDBContainer db = new NetstepsDataAccess.DataEntities.Commissions.NetStepsCommissionsDBContainer())
            {
                db.uspIntegrationsValidateDisbursementsRetrieval(periodID, disbursementRetrievalSucceded, disbursementRetrievalErrorMessage);

                if (!Convert.ToBoolean(disbursementRetrievalSucceded.Value))
                    return disbursementRetrievalErrorMessage.Value.ToString();

                col.PeriodID = periodID;
                col.EffectiveDate = (from p in db.Periods
                                     where p.PeriodID == periodID
                                     select p.EndDate).Single();
            }
            // disbursement retrieval succeeded, get disbursements from the database
            col.Disbursement = GetDisbursementDataFromCommissions(periodID);
            // return serialized XML
            return col.Serialize();
        }

        private static List<Disbursements.Disbursement> GetDisbursementDataFromCommissions(int periodID)
        {
            List<Disbursements.Disbursement> xmlDisbursements = new List<Disbursements.Disbursement>();
            List<NetstepsDataAccess.DataEntities.Commissions.Disbursement> linqDisbursements = null;
            using (NetstepsDataAccess.DataEntities.Commissions.NetStepsCommissionsDBContainer db = new NetstepsDataAccess.DataEntities.Commissions.NetStepsCommissionsDBContainer())
            {
                // linq to get disbursements from database
                linqDisbursements = (from d in db.Disbursements
                                     where d.PeriodID == periodID
                                     select d).ToList();
                foreach (NetstepsDataAccess.DataEntities.Commissions.Disbursement linqDisbursement in linqDisbursements)
                {
                    Disbursements.Disbursement xmlDisbursement = new Disbursements.Disbursement();
                    var acctInfo = (from a in db.Accounts
                                    where a.AccountID == linqDisbursement.AccountID
                                    select new { a.AccountNumber, a.FirstName, a.LastName, a.AccountID }).Single(); // will fail if Account doesn't exist
                    xmlDisbursement.DisbursementID = linqDisbursement.DisbursementID;
                    xmlDisbursement.ConsultantID = acctInfo.AccountID;
                    xmlDisbursement.AccountNumber = acctInfo.AccountNumber;
                    xmlDisbursement.Amount = linqDisbursement.Amount;
                    xmlDisbursement.FirstName = acctInfo.FirstName;
                    xmlDisbursement.LastName = acctInfo.LastName;
                    xmlDisbursement.DisbursementDetail = GetDisbursementDetails(linqDisbursement.DisbursementID);
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
                    NetstepsDataAccess.DataEntities.Commissions.Check check = (from c in db.Checks
                                                                              where c.DisbursementDetailID == linqDetail.DisbursementDetailID
                                                                              select c).SingleOrDefault();

                    Disbursements.DisbursementDetail xmlDetail = new Disbursements.DisbursementDetail();
                    xmlDetail.Percentage = Convert.ToDecimal(linqDetail.Percentage); // database incorrectly allows NULLs
                    xmlDetail.DisbursementStatus = Disbursements.DisbursementStatus.Entered; // must be in an entered status if we've made it here.
                    xmlDetail.DisbursementType = GetDisbursementTypeByName(linqDetail.DisbursementType.Name);
                    xmlDetail.DisbursementProfile = GetDisbursementProfile(linqDetail.DisbursementDetailID);
                    xmlDetail.DisbursementDetailID = linqDetail.DisbursementDetailID;
                    if (check != null)
                    {
                        xmlDetail.CheckNumber = check.CheckNumber;
                        xmlDetail.CheckDate = check.CheckIssueDate;
                        xmlDetail.CheckPayee = check.NameOnCheck;
                    }

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
            if (code.ToLower() == "savings")
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
            System.Data.Objects.ObjectResult<uspIntegrationsGetShippedRevenueResult> result = null;
            using (NetStepsEntities db = new NetStepsEntities())
            {
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

        internal static BasicResponse UpdateInventory(string data)
        {
            InventoryResponse.InventoryCollection responseCol = new InventoryResponse.InventoryCollection();
            using (NetStepsEntities db = new NetStepsEntities())
            {
                var inventoryUpdateSucceded = new ObjectParameter("Succeeded", typeof(bool));
                var inventoryUpdateErrorMessage = new ObjectParameter("ErrorMessage", typeof(string));
                List<uspIntegrationsUpdateInventoryResult> res = db.uspIntegrationsUpdateInventory(IntegrationsSecurity.ModifiedByUserID(),
                    data, inventoryUpdateSucceded, inventoryUpdateErrorMessage).ToList();

				if (!Convert.ToBoolean(inventoryUpdateSucceded.Value))
				{
					return new BasicResponse {Success = false, Message = inventoryUpdateErrorMessage.Value.ToString()};
				}

            	foreach (uspIntegrationsUpdateInventoryResult r in res)
                {
                    InventoryResponse.WarehouseProduct product = new InventoryResponse.WarehouseProduct();
                    product.Succeeded = true;
                    product.SKU = r.SKU;
                    product.WarehouseID = r.WarehouseID;
                    responseCol.WarehouseProduct.Add(product);
                }
            }

			return new BasicResponse { Success = true, Message = responseCol.Serialize() };
        }

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
                                    select c.CountryCode).Single();
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

        internal static List<Fulfillment.Order> GetOrdersToFulfill()
        {
            var orders = new List<Fulfillment.Order>();
            using (var db = new NetStepsEntities())
            {
                var ordersList = db.uspIntegrationsGetOrdersToFulfill().ToList();

                (new object()).TraceVerbose(string.Format("GetOrdersToFulfill - processing {0} orders", ordersList.Count));

                foreach (var order in ordersList)
                {
                    var xmlOrder = new Fulfillment.Order();

                    var ship = (from d in db.OrderShipments
                                join cust in db.OrderCustomers on d.OrderCustomerID equals cust.OrderCustomerID into joinedOrderCustomer
                                from djoin in joinedOrderCustomer.DefaultIfEmpty() // make it a left join
                                where d.OrderShipmentID == order.OrderShipmentID
                                select new
                                {
                                    d.Address1,
                                    d.Address2,
                                    d.City,
                                    d.OrderCustomer.Account.EmailAddress,
                                    FirstName = d.FirstName ?? d.OrderCustomer.Account.FirstName,
                                    LastName = d.LastName ?? d.OrderCustomer.Account.LastName,
                                    Name = string.IsNullOrEmpty(d.Name) ?
                                                djoin != null ?
                                                        djoin.Account != null ? djoin.Account.FirstName + djoin.Account.LastName : ""
                                                : ""
                                           : d.Name,
                                    d.DayPhone,
                                    d.State,
                                    d.PostalCode,
                                    d.ShippingMethod,
                                    d.CountryID
                                }).SingleOrDefault();

                    xmlOrder.Shipping.Address = new Fulfillment.Address()
                    {
                        Address1 = ship.Address1,
                        Address2 = ship.Address2,
                        City = ship.City,
                        Email = ship.EmailAddress,
                        FirstName = ship.FirstName,
                        LastName = ship.LastName,
                        Name = ship.Name,
                        Phone = ship.DayPhone,
                        State = ship.State,
                        Zip = ship.PostalCode,
                        CountryISOCode = GetCountryByID(ship.CountryID)
                    };

                    xmlOrder.NumberOfPartyOrders = Convert.ToInt32(order.NumberOfPartyOrders);
                    xmlOrder.OrderDate = order.CompleteDate.HasValue ? order.CompleteDate.Value : new DateTime();

                    // pull shipping method from the database.
                    if (ship.ShippingMethod != null)
                        xmlOrder.Shipping.ShippingMethod = ship.ShippingMethod.Name;

                    //xmlOrder.Shipping.ShippingProvider = "FedEx";

                    var payments = (from d in db.OrderPayments
                                                   where d.OrderID == order.OrderID
                                                   select d).ToList();
                    var addresses = new List<Fulfillment.Address>();
                    foreach (var p in payments)
                    {
                        var a = new Fulfillment.Address();
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
                        a.Email = p.OrderCustomer == null ? null : p.OrderCustomer.Account == null ? null : p.OrderCustomer.Account.EmailAddress;
                        addresses.Add(a);
                    }
                    xmlOrder.Billing = addresses;
                    xmlOrder.OrderNumber = order.OrderNumber;
                    xmlOrder.Status = GetOrderStatusByIDForOrderFulfillment(order.OrderStatusID);
                    xmlOrder.Type = GetOrderTypeByIDForOrderFulfillment(order.OrderTypeID);
                    xmlOrder.OrderItem = GetOrderItemsByOrderShipmentID(order.OrderShipmentID);
                    // add to list
                    orders.Add(xmlOrder);
                }
            }
            return orders;
        }

        private static Fulfillment.OrderType GetOrderTypeByIDForOrderFulfillment(short orderTypeID)
        {
            string typeName;
            using (var db = new NetStepsEntities())
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
                case "Employee Order": return Fulfillment.OrderType.EmployeeOrder;
                case "Business Materials order": return Fulfillment.OrderType.BusinessMaterialsorder;
                case "Hostess Rewards order": return Fulfillment.OrderType.HostessRewardsorder;
                case "Fundraiser Order": return Fulfillment.OrderType.FundraiserOrder;
                case "Party Order - Direct Ship": return Fulfillment.OrderType.PartyOrderDirectShip;
                default: return Fulfillment.OrderType.OnlineOrder;
            }
        }

        private static Fulfillment.OrderStatus GetOrderStatusByIDForOrderFulfillment(short orderStatusID)
        {
            string statusName;
            using (var db = new NetStepsEntities())
            {
                statusName = (from s in db.OrderStatuses
                              where s.OrderStatusID == orderStatusID
                              select s.Name).Single();
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
            var items = new List<Fulfillment.OrderItem>();
        	var connectionString = ConfigurationManager.AppSettings["dbconn"];

			using(var connection = new SqlConnection(connectionString))
			{
			    var cmd = connection.CreateCommand();
			    cmd.CommandText = "uspGetOrderItemsByOrderShipmentIDWithIsShippable";
				cmd.CommandType = CommandType.StoredProcedure;

				var orderShipmentIDParameter = new SqlParameter("@OrderShipmentID", SqlDbType.Int);
				orderShipmentIDParameter.Value = orderShipmentID;
				orderShipmentIDParameter.Direction = ParameterDirection.Input;

				var succeededParameter = new SqlParameter("@Succeeded", SqlDbType.Bit);
				succeededParameter.Direction = ParameterDirection.Output;

				var errorMessageParameter = new SqlParameter("@ErrorMessage", SqlDbType.VarChar);
				errorMessageParameter.Direction = ParameterDirection.Output;
				errorMessageParameter.Size = 5000;

				cmd.Parameters.Add(orderShipmentIDParameter);
				cmd.Parameters.Add(succeededParameter);
				cmd.Parameters.Add(errorMessageParameter);

				connection.Open();
				using (var reader = cmd.ExecuteReader())
				{
					while(reader.Read())
					{
						if(!Convert.ToBoolean(reader["IsShippable"].ToString()))
						{
							continue;
						}

						var xmlItem = new Fulfillment.OrderItem();

						if(reader.GetOrdinal("OrderItemParentType") > -1)
						{
							string orderItemParentType = reader["OrderItemParentType"].ToString();
							if (!string.IsNullOrEmpty(orderItemParentType))
							{
								xmlItem.OrderItemParentType = GetOrderItemParentTypeEnumByString(orderItemParentType);
								xmlItem.ParentOrderItemIDSpecified = true;
							}
						}
					
						if(reader.GetOrdinal("ParentOrderItemID") > -1)
						{
							var parentOrderItemID = reader["ParentOrderItemID"].ToString();
							if (!string.IsNullOrEmpty(parentOrderItemID))
							{
								xmlItem.ParentOrderItemID = Convert.ToInt32(parentOrderItemID);
							}
						}

						xmlItem.OrderItemID = Convert.ToInt32(reader["OrderItemID"].ToString());
						//xmlItem.ItemPrice = new Fulfillment.Money() { Value = Convert.ToDecimal(reader["ItemPrice"].ToString()), Currency = Fulfillment.Currency.USD };
						xmlItem.Qty = Convert.ToInt32(reader["Quantity"].ToString());
						xmlItem.Sku = reader["SKU"].ToString();
						items.Add(xmlItem);
					}

					reader.Close();

					if (!Convert.ToBoolean(cmd.Parameters["@Succeeded"].Value))
					{
						throw new Exception(cmd.Parameters["@ErrorMessage"].Value.ToString());
					}
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
    }
}

