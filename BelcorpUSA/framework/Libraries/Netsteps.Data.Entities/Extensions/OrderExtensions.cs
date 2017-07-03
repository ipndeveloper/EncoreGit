using NetSteps.Commissions.Common;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Data.Entities.Extensions
{
    using System.Linq;

    using NetSteps.Data.Entities.Cache;
    using NetSteps.Commissions.Common.Models;
    using System.Data;
    using System.Collections.Generic;
    using System;
    using System.Data.SqlClient;
    using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
    using NetSteps.Data.Entities.Business;
    using NetSteps.Data.Entities.Dto;
    using NetSteps.Data.Entities.EntityModels;
    using NetSteps.Common.Base;

    /// <summary>
    /// Author: John Egbert
    /// Description: Order Extensions
    /// Created: 03-04-2011
    /// </summary>
    public static class OrderExtensions
    {
        public static void UpdateOrderCustomerAccount(this Order order, Entities.Account account)
        {
            if (order != null)
            {
                OrderCustomer orderCustomer = null;
                if (order.OrderCustomers.Count == 1)
                {
                    orderCustomer = order.OrderCustomers[0];
                }

                if (orderCustomer != null)
                {
                    order.StartEntityTracking();
                    orderCustomer.AccountID = account.AccountID;
                }
            }
        }

        public static bool IsStatusChangeable(this Order order)
        {
            var period = Create.New<ICommissionsService>().GetCurrentPeriod();
            return ApplicationContext.Instance.CurrentUser.HasFunction("Orders-Order Status Overrides")
                && order.IsStatusChangeable_OrderState(period);
        }

        public static bool IsStatusChangeable_OrderState(this Order order, IPeriod currentPeriod)
        {
            return order.CommissionDateUTC <= currentPeriod.EndDateUTC && order.CommissionDateUTC >= currentPeriod.StartDateUTC
                 && (order.OrderStatusID == (int)Constants.OrderStatus.Paid || order.OrderStatusID == (int)Constants.OrderStatus.PartiallyShipped
                            || order.OrderStatusID == (int)Constants.OrderStatus.Shipped || order.OrderStatusID == (int)Constants.OrderStatus.Printed
                            || order.IsPaidInFull());
        }

        /// <summary>
        /// Safely casts a <see cref="NetSteps.Data.Common.Entities.IOrder"/> to an <see cref="Order"/>.
        /// </summary>
        public static Order AsOrder(this Common.Entities.IOrder order)
        {
            return order as Order;
        }

        public static int GeneralParameterVal(int MarketID)
        {
            int generalParameterVal = DataAccess.ExecWithStoreProcedureSaveIdentity("Core", "upsGetGeneralParameterVal",
                new SqlParameter("MarketID", SqlDbType.Int) { Value = MarketID }

             );
            return generalParameterVal;
        }

        public static void InsertKitItemPrices(int OrderID)
        {
            int result = DataAccess.ExecWithStoreProcedureSaveIdentity("Core", "InsertKitItemPrices",
                new SqlParameter("OrderID", SqlDbType.Int) { Value = OrderID }
             );
            //return generalParameterVal;
        }

        public static string GeneralParameterVal(int MarketID, string GeneralParameterCod)
        {
            string generalParameterVal = DataAccess.ExecWithStoreProcedureScalarType<string>("Core", "upsGetGeneralParameterValue",
                new SqlParameter("MarketID", SqlDbType.Int) { Value = MarketID },
                new SqlParameter("GeneralParameterCod", SqlDbType.Char, 3) { Value = GeneralParameterCod }
             );
            if (string.IsNullOrEmpty(generalParameterVal))
            {
                generalParameterVal = "";
            }
            return generalParameterVal.Trim();
        }
         
        public static bool GetStatesByAccountID(int AccountId, string PostalCode)
        {
            var generalParameterVal = DataAccess.ExecWithStoreProcedureScalarType<bool>("Core", "uspGetStatesByAccountID",
                new SqlParameter("AccountId", SqlDbType.Int) { Value = AccountId },
                new SqlParameter("PostalCode", SqlDbType.VarChar) { Value = PostalCode }
             );

            return generalParameterVal;
        }

        public static GeneralParametersTable GetGeneralParametersByCode(int MarketID, string GeneralParameterCod)
        {
            GeneralParametersTable entidad = new GeneralParametersTable();
            entidad = DataAccess.ExecWithStoreProcedureListParam<GeneralParametersTable>("Core", "upsGetGeneralParametersByCode",
                new SqlParameter("MarketID", SqlDbType.Int) { Value = MarketID },
                new SqlParameter("GeneralParameterCod", SqlDbType.Char, 3) { Value = GeneralParameterCod }
            ).ToList()[0];
            return entidad;
        }

        public static void TerminateOrder(int AccountID)
        {
            int result = DataAccess.ExecWithStoreProcedureSaveIdentity("Core", "uspTerminateOrder",
                new SqlParameter("AccountID", SqlDbType.Int) { Value = AccountID }
             );
        }

        public static Dictionary<int, int> GetWarehousePreOrderFromOrderByAccountID(int AccountID)
        {
            Dictionary<int, int> Result = new Dictionary<int, int>();
            SqlParameter[] LstParameter = new SqlParameter[] { new SqlParameter() { SqlDbType = System.Data.SqlDbType.Int, Value = AccountID, ParameterName = "@AccountID" } };
            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
            {
                connection.Open();
                SqlCommand ocom = new SqlCommand("uspGetWarehousePreOrderFromOrderByAccountID", connection);
                ocom.CommandType = CommandType.StoredProcedure;
                ocom.Parameters.AddRange(LstParameter);
                SqlDataReader dr = ocom.ExecuteReader();
                while (dr.Read())
                {
                    Result.Add(Convert.ToInt32(dr["WarehouseID"]), Convert.ToInt32(dr["PreOrderID"]));
                    break;
                }

            }
            return Result;
        }

        public static Dictionary<int, string[]> GetInvoiceKey(DataTable dtOrderNumbers)
        {
            Dictionary<int, string[]> Result = new Dictionary<int, string[]>();
            SqlParameter[] LstParameter = new SqlParameter[] { new SqlParameter() { SqlDbType = System.Data.SqlDbType.Structured, Value = dtOrderNumbers, ParameterName = "@OrderNumbers" } };
            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
            { 
                connection.Open();
                //otr = connection.BeginTransaction();
                SqlCommand ocom = new SqlCommand("spGetInvoiceKey", connection);
                ocom.CommandType = CommandType.StoredProcedure;
                ocom.Parameters.AddRange(LstParameter);
                SqlDataReader dr = ocom.ExecuteReader();

                while (dr.Read())
                {
                    Result.Add(Convert.ToInt32(dr["OrderNumber"]), new string[] { Convert.ToString(dr["InvoiceKey"]), Convert.ToString(dr["DateInvoice"]) });
                }

            }
            return Result;
        }

        //Developed by Wesley Campos S. - CSTI
        public static Order GetOrderItemsInheritance(this Common.Entities.IOrder order, int originalOrderId)
        {
            DataSet ds = new DataSet();
            if (order.OrderCustomers[0].OrderItems.Count > 0) ds = DataAccess.GetDataSet(DataAccess.GetCommand("upsRefreshOrderItems", new Dictionary<string, object>() { { "@ORDERID", originalOrderId } }, "Core"));
            foreach (OrderItem oi in order.OrderCustomers[0].OrderItems)
            {
                if ((int)oi.OrderItemReturns.Count > 0)
                {
                    oi.OrderItemID = (int)oi.OrderItemReturns[0].OriginalOrderItemID;
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        if (Convert.ToInt32(row[0]) == oi.OrderItemID)
                        {
                            if (!string.IsNullOrEmpty(row[1].ToString())) oi.ParentOrderItemID = Convert.ToInt32(row[1]);
                            break;
                        }
                    }
                }
            }
            return order.AsOrder();
        }

        public static bool IsOrderStatusCommissionable(this Order order)
        {
            var collection =
                SmallCollectionCache.Instance.OrderStatuses
                    .Where(x => x.IsCommissionable)
                    .Select(y => y.OrderStatusID);
            return collection.Contains(order.OrderStatusID);
        }

        /// <summary>
        /// Developed By KLC - CSTI
        /// BR-MLM-003 – ACTUALIZACION DE INDICADORES PERSONALES
        /// </summary>
        /// <param name="OrderID"></param>
        /// <param name="OrderStatusID"></param>
        public static void UpdatePersonalIndicator(int OrderID, short OrderStatusID)
        {
            var result = DataAccess.ExecWithStoreProcedureSave("Commissions", "SPOnLineMLM",
                new SqlParameter("OrderID", SqlDbType.Int) { Value = OrderID },
                new SqlParameter("StatusOrderID", SqlDbType.Int) { Value = OrderStatusID }
                );
        }

        public static List<BackOrderedProductData> GetBackOrderedProductData(int accountId)
        {
            return DataAccess.ExecWithStoreProcedureListParam<BackOrderedProductData>("Core", "uspGetBackOrderedProducts",
                new SqlParameter("SelectedAccountID", SqlDbType.Int) { Value = accountId }
                ).ToList();
        }

        /*
        * wv:20160606 Procedimiento para listar los OrderItems procesados a la consultora
        */
        public static List<int> getListDispatchOrderItems(int orderID)
        {
            return DataAccess.ExecWithStoreProcedureListParam<int>("Core", "getListDispatchOrderItems",
                new SqlParameter("orderID", SqlDbType.Int) { Value = orderID }
                ).ToList();
        }

        /*
        * wv:20160606 Procedimiento para listar los Dispatch procesados x Orden
        */
        public static List<getDispatchByOrder> GetDispatchByOrder(int orderID)
        {
            return DataAccess.ExecWithStoreProcedureListParam<getDispatchByOrder>("Core", "GetDispatchByOrder",
                new SqlParameter("orderID", SqlDbType.Int) { Value = orderID }
                ).ToList();
        }

        public static List<getListInfoOrderItems> getListInfoOrderItems(int orderID)
        {
            return DataAccess.ExecWithStoreProcedureListParam<getListInfoOrderItems>("Core", "getListInfoOrderItems",
                new SqlParameter("orderID", SqlDbType.Int) { Value = orderID }
                ).ToList();
        }

        /*
        * wv:20160606 Procedimiento para insertar los Dispatch filtrados para la consultora en la tabla de control
        */
        public static int insertDispatchProducts(int dispatchId, int accountId, int quantity, int orderId, int periodID, int DispatchItemsID, int productId, string name, string sku)
        {
            return DataAccess.ExecWithStoreProcedureScalar(ConnectionStrings.BelcorpCore, "insertDispatchProducts",
                new SqlParameter("dispatchId", SqlDbType.Int) { Value = dispatchId },
                new SqlParameter("accountId", SqlDbType.Int) { Value = accountId },
                new SqlParameter("Quantity", SqlDbType.Int) { Value = quantity },
                new SqlParameter("OrderId", SqlDbType.Int) { Value = orderId },
                new SqlParameter("periodId", SqlDbType.Int) { Value = periodID },
                new SqlParameter("DispatchItemsID", SqlDbType.Int) { Value = DispatchItemsID },
                new SqlParameter("productId", SqlDbType.Int) { Value = productId },
                new SqlParameter("name", SqlDbType.NVarChar) { Value = name },
                new SqlParameter("sku", SqlDbType.NVarChar) { Value = sku }
                );
        }
        /*
        * wv:20160606 Procedimiento para listar los OrderItems procesados a la consultora
        */
        public static List<DisplayDispatch> getListDispatchxAccountxOrder(int orderID)
        {
            return DataAccess.ExecWithStoreProcedureListParam<DisplayDispatch>("Core", "getListDispatchxAccountxOrder",
                new SqlParameter("orderID", SqlDbType.Int) { Value = orderID }
                ).ToList();
        }

        public static List<DispatchNameProducts> GetDispatchProducts(int accountId, int periodId) // wv: 20160523 process Products Dispatch
        {
            return DataAccess.ExecWithStoreProcedureListParam<DispatchNameProducts>("Core", "getDispatchxAccountxPeriod",
                new SqlParameter("PeriodID", SqlDbType.Int) { Value = periodId },
                new SqlParameter("AccountId", SqlDbType.Int) { Value = accountId }
                ).ToList();
        }

        public static List<DispatchProducts> GetOrderItemByDispatch(int OrderID) // wv: 20160523 process Products Dispatch
        {
            return DataAccess.ExecWithStoreProcedureListParam<DispatchProducts>("Core", "GetOrderItemByDispatch",
                new SqlParameter("OrderID", SqlDbType.Int) { Value = OrderID }
                ).ToList();
        }

         



        // -------------------------------------------------------------------------------------------------------------------------------------



        public static int InsOrderCustomerClains(int orderCustomerID, int orderId)
        {
            return DataAccess.ExecWithStoreProcedureScalar(ConnectionStrings.BelcorpCore, "uspInsOrderCustomerClains",
                new SqlParameter("OrderCustomerID", SqlDbType.Int) { Value = orderCustomerID },
                new SqlParameter("OrderID", SqlDbType.Int) { Value = orderId }
                );
        }


        public static int GetQuantityGift(int orderId)
        {
            return DataAccess.ExecWithStoreProcedureScalar(ConnectionStrings.BelcorpCore, "uspGetQuantityGift",
                new SqlParameter("OrderID", SqlDbType.Int) { Value = orderId }
                );
        }

        public static int OrderItemClains(int OrderItemID, int OrderCustomerID, int OrderCustomerNewID, int? OrderItemParentID)
        {
            return DataAccess.ExecWithStoreProcedureScalar(ConnectionStrings.BelcorpCore, "uspOrderItemClains",
                new SqlParameter("OrderItemID", SqlDbType.Int) { Value = OrderItemID },
                new SqlParameter("OrderCustomerID", SqlDbType.Int) { Value = OrderCustomerID },
                new SqlParameter("OrderCustomerNewID", SqlDbType.Int) { Value = OrderCustomerNewID },
                new SqlParameter("OrderItemParentID", SqlDbType.Int) { Value = (object)OrderItemParentID ?? DBNull.Value }
                );
        }

        public static int InsOrderClains(int orderId)
        {
            return DataAccess.ExecWithStoreProcedureScalar(ConnectionStrings.BelcorpCore, "uspInsOrderClains",
                new SqlParameter("OrderId", SqlDbType.Int) { Value = orderId }
                );
        }

        public static List<ProductNameClains> GetProductClains(int accountId)
        {
            return DataAccess.ExecWithStoreProcedureListParam<ProductNameClains>("Core", "uspGetProductClains",
                new SqlParameter("AccountID", SqlDbType.Int) { Value = accountId }
                ).ToList();
        }

        public static List<OrderItemsStock> GetInventoryByClaims(int PreOrderID)
        {
            return DataAccess.ExecWithStoreProcedureListParam<OrderItemsStock>("Core", "uspGetInventoryByClaims",
                new SqlParameter("PreOrderID", SqlDbType.Int) { Value = PreOrderID }
                ).ToList();
        }

        public static List<OrderItemsClaims> GetOrderItemClaimsByAccount(int AccountID)
        {
            return DataAccess.ExecWithStoreProcedureListParam<OrderItemsClaims>("Core", "uspGetOrderItemClaimsByAccount",
                new SqlParameter("AccountID", SqlDbType.Int) { Value = AccountID }
                ).ToList();
        }

        public static int UPDOrderItemclaimsByID(int OrderItemClaimID)
        {
            return DataAccess.ExecWithStoreProcedureScalarType<int>("Core", "uspUPDOrderItemclaimsByID",
                new SqlParameter("OrderItemClaimID", SqlDbType.Int) { Value = OrderItemClaimID }
                );
        }

        public static int GetOrderItemClaims(int ProductID, int OrderClaimID)
        {
            return DataAccess.ExecWithStoreProcedureScalar(ConnectionStrings.BelcorpCore, "uspGetOrderItemClaims",
                new SqlParameter("ProductID", SqlDbType.Int) { Value = ProductID },
                new SqlParameter("OrderClaimID", SqlDbType.Int) { Value = OrderClaimID }
                );
        }

        public static int InsOrderItemByClaims(int OrderItemID, int OrderCustomerID)
        {
            return DataAccess.ExecWithStoreProcedureScalar(ConnectionStrings.BelcorpCore, "uspInsOrderItemByClaims",
                new SqlParameter("OrderItemID", SqlDbType.Int) { Value = OrderItemID },
                new SqlParameter("OrderCustomerID", SqlDbType.Int) { Value = OrderCustomerID }
                );
        }

        public static List<MaterialName> GetMaterialWithMaterialID(int WareHouseID, int PreOrderID)
        {
            return DataAccess.ExecWithStoreProcedureListParam<MaterialName>("Core", "uspGetMaterialWithMaterial",
                new SqlParameter("WareHouseID", SqlDbType.Int) { Value = WareHouseID },
                new SqlParameter("PreOrderID", SqlDbType.Int) { Value = PreOrderID }
                ).ToList();
        }

        public static List<MaterialName> GetProductDetails(int OrderID)
        {
            return DataAccess.ExecWithStoreProcedureListParam<MaterialName>("Core", "uspGetProductDetails",
                new SqlParameter("OrderID", SqlDbType.Int) { Value = OrderID }
                ).ToList();
        }

        public static List<MaterialName> GetWareHouseMaterialsByOrderId(int OrderID, int ProductID)
        {
            return DataAccess.ExecWithStoreProcedureListParam<MaterialName>("Core", "uspGetWareHouseMaterialsByOrderId",
                new SqlParameter("OrderID", SqlDbType.Int) { Value = OrderID },
                new SqlParameter("ProductID", SqlDbType.Int) { Value = ProductID }
                ).ToList();
        }

        public static List<WareHouseMaterialAllocations> GetWareHouseMaterialAllocations(int PreOrderID)
        {
            return DataAccess.ExecWithStoreProcedureListParam<WareHouseMaterialAllocations>("Core", "uspGetWareHouseMaterialAllocations",
                new SqlParameter("PreOrderID", SqlDbType.Int) { Value = PreOrderID }
                ).ToList();
        }

        public static List<MaterialWareHouseMaterial> GetMaterialWareHouseMaterial(int OrderId)
        {
            return DataAccess.ExecWithStoreProcedureListParam<MaterialWareHouseMaterial>("Core", "uspGetMaterialWareHouseMaterial",
                new SqlParameter("OrderId", SqlDbType.Int) { Value = OrderId }
                ).ToList();
        }

        public static List<MaterialWareHouseMaterial> GetMaterialWareHouseMaterialPWS(int OrderId)
        {
            return DataAccess.ExecWithStoreProcedureListParam<MaterialWareHouseMaterial>("Core", "uspGetMaterialWareHouseMaterialPWS",
                new SqlParameter("OrderId", SqlDbType.Int) { Value = OrderId }
                ).ToList();
        }

        public static List<MaterialOrderItem> GetMaterialOrderItem(int OrderCustomerID)
        {
            return DataAccess.ExecWithStoreProcedureListParam<MaterialOrderItem>("Core", "uspGetMaterialOrderItem",
                new SqlParameter("OrderCustomerID", SqlDbType.Int) { Value = OrderCustomerID }
                ).ToList();
        }

        public static List<OrderItemsStock> uspGetOrderItemsStock(int OrderCustomerID)
        {
            return DataAccess.ExecWithStoreProcedureListParam<OrderItemsStock>("Core", "uspGetOrderItemsStock",
                new SqlParameter("OrderCustomerID", SqlDbType.Int) { Value = OrderCustomerID }
                ).ToList();
        }

        public static List<WareHouseMaterialAllocations> GetWareHouseMaterialAllocationsbyProduct(int PreOrderID)
        {
            return DataAccess.ExecWithStoreProcedureListParam<WareHouseMaterialAllocations>("Core", "uspGetWareHouseMaterialAllocationsbyProduct",
                new SqlParameter("PreOrderID", SqlDbType.Int) { Value = PreOrderID }
                ).ToList();
        }

        public static List<PreOrderCondition> GetPreOrderConditions(int AccountID, int LanguageID)
        {
            return DataAccess.ExecWithStoreProcedureListParam<PreOrderCondition>("Core", "uspGetPreOrderConditions",
                new SqlParameter("AccountID", SqlDbType.Int) { Value = AccountID },
                new SqlParameter("LanguageID", SqlDbType.Int) { Value = LanguageID }
                ).ToList();
        }

        public static int GetPreOders(int AccountID, int SiteID)
        {
            return DataAccess.ExecWithStoreProcedureScalarType<int>("Core", "uspGetPreOders",
              new SqlParameter("AccountID", SqlDbType.Int) { Value = AccountID },
              new SqlParameter("SiteID", SqlDbType.Int) { Value = SiteID }
                );
        }

        public static bool ExistPreOrder(int OrderID)
        {
            return DataAccess.ExecWithStoreProcedureScalarType<bool>("Core", "uspExistPreOrder",
              new SqlParameter("OrderID", SqlDbType.Int) { Value = OrderID }
                );
        }

        public static string GetOrderPending(int AccountID, int SiteID)
        {
            return DataAccess.ExecWithStoreProcedureScalarType<string>("Core", "uspGetOrderPending",
              new SqlParameter("AccountID", SqlDbType.Int) { Value = AccountID },
              new SqlParameter("SiteID", SqlDbType.Int) { Value = SiteID }
                );
        }

        public static int GetPreOrderPending(string OrderNumber, int SiteID)
        {
            return DataAccess.ExecWithStoreProcedureScalarType<int>("Core", "uspGetPreOrderPending",
              new SqlParameter("OrderNumber", SqlDbType.VarChar) { Value = OrderNumber },
              new SqlParameter("SiteID", SqlDbType.Int) { Value = SiteID }
                );
        }

        public static int GetPreOrderByOrderID(int? OrderID)
        {
            return DataAccess.ExecWithStoreProcedureScalarType<int>("Core", "uspGetPreOrderByOrderID",
              new SqlParameter("OrderID", SqlDbType.Int) { Value = OrderID }
                );
        }

        public static int GetProOrderUpdate(int AccountID, int SiteID)
        {
            return DataAccess.ExecWithStoreProcedureScalarType<int>("Core", "uspGetProOrderUpdate",
              new SqlParameter("AccountID", SqlDbType.Int) { Value = AccountID },
              new SqlParameter("SiteID", SqlDbType.Int) { Value = SiteID }
                );
        }




        public static bool ExistDispatchItemControl(int AccountID, int OrderID)
        {
            return DataAccess.ExecWithStoreProcedureScalarType<bool>("Core", "ExistDispatchItemControl",
              new SqlParameter("AccountID", SqlDbType.Int) { Value = AccountID },
              new SqlParameter("OrderID", SqlDbType.Int) { Value = OrderID }
                );
        }

        public static int GetShippingMethodID(int ShippingOrderTypeID)
        {
            return DataAccess.ExecWithStoreProcedureScalarType<int>("Core", "uspGetShippingMethodID",
              new SqlParameter("ShippingOrderTypeID", SqlDbType.Int) { Value = ShippingOrderTypeID }
                );
        }

        public static int GetPreOderEdit(int OrderId)
        {
            return DataAccess.ExecWithStoreProcedureScalarType<int>("Core", "uspGetPreOderEdit",
              new SqlParameter("OrderId", SqlDbType.Int) { Value = OrderId }
                );
        }

        public static int DelWareHouseMaterialAllocations(int PreOrderID)
        {
            return DataAccess.ExecWithStoreProcedureScalarType<int>("Core", "uspDelPreOrdersbyId",
              new SqlParameter("PreOrderID", SqlDbType.Int) { Value = PreOrderID }
              );
        }

        public static void UpdWareHouseMaterialAllocations(int OrderID, int PreOrderID)
        {
            DataAccess.ExecWithStoreProcedureSaveIdentity("Core", "uspUpdWareHouseMaterialAllocations",
              new SqlParameter("OrderID", SqlDbType.Int) { Value = OrderID },
              new SqlParameter("PreOrderID", SqlDbType.Int) { Value = PreOrderID }
              );
        }
        //csti-mescobar-20160219-inicio
        public static int GetPeriodByOrderId(string OrderNumber, int AccountId)
        {
            return DataAccess.ExecWithStoreProcedureScalarType<int>("Core", "uspGetPeriodByOrderId",
              new SqlParameter("OrderNumber", SqlDbType.VarChar) { Value = OrderNumber },
              new SqlParameter("AccountId", SqlDbType.Int) { Value = AccountId }
                );
        }
        //csti-mescobar-20160219-fin


        public static string GetGeneralParameterVEA()
        {
            return DataAccess.ExecWithStoreProcedureScalarType<string>("Core", "uspGetGeneralParameterVEA"
                );
        }


        public static int UPDOrderItemProductReturn(int OriginalOrderID, int ReturnOrderID, List<ReturnOrderItemDto> OrderItemList, int ModifiedByUser, bool EsOrdenRetornoCompleta)
        {
            DataTable returnOrderList = new DataTable();
            returnOrderList.Columns.Add("ParentOrderItemID");
            returnOrderList.Columns.Add("OrderItemID");
            returnOrderList.Columns.Add("ProductID");
            returnOrderList.Columns.Add("SKU");
            returnOrderList.Columns.Add("ParentQuantity");
            returnOrderList.Columns.Add("Quantity");
            returnOrderList.Columns.Add("QuantityOrigen");
            returnOrderList.Columns.Add("ItemPrice");
            returnOrderList.Columns.Add("HasComponents");
            returnOrderList.Columns.Add("AllHeader");
            returnOrderList.Columns.Add("IsChild");

            if (OrderItemList != null)
            {
                foreach (ReturnOrderItemDto item in OrderItemList)
                    returnOrderList.Rows.Add(item.ParentOrderItemID, item.OrderItemID, item.ProductID, item.SKU, item.ParentQuantity, item.Quantity, item.QuantityOrigen, item.ItemPrice, item.HasComponents, item.AllHeader, item.IsChild);
            }

            int generalParameterVal = DataAccess.ExecWithStoreProcedureSaveIdentity("Core", "uspUPDOrderItemProductReturn",
                new SqlParameter("OriginalOrderID", SqlDbType.Int) { Value = OriginalOrderID },
                new SqlParameter("ReturnOrderID", SqlDbType.Int) { Value = ReturnOrderID },
                //new SqlParameter("OrderItemList", SqlDbType.Structured) { Value = returnOrderList, TypeName = "ReturnOrderItemDto" },
                new SqlParameter("OrderItemList", SqlDbType.Structured) { Value = returnOrderList, TypeName = "OrderItemListReturn" },
                new SqlParameter("ModifiedByUser", SqlDbType.Int) { Value = ModifiedByUser },
                new SqlParameter("EsOrdenRetornoCompleta", SqlDbType.Bit) { Value = EsOrdenRetornoCompleta }
             );
            return generalParameterVal;
        }

        public static Dictionary<int, string> GetPromotionIDByOrderAdjustmentID(int orderAdjustmentID)
        {
            Dictionary<int, string> Result = new Dictionary<int, string>();

            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
            {
                connection.Open();
                SqlCommand ocom = new SqlCommand("upsGetPromotionIDByOrderAdjustmentID", connection);
                ocom.Parameters.Add("@OrderAdjustmentID", SqlDbType.Int);
                ocom.Parameters["@OrderAdjustmentID"].Value = orderAdjustmentID;
                ocom.CommandType = CommandType.StoredProcedure;
                SqlDataReader dr = ocom.ExecuteReader();

                while (dr.Read())
                {
                    Result.Add(Convert.ToInt32(dr["PromotionID"]), Convert.ToString(dr["Description"]));
                }

            }
            return Result;
        }

        #region GetOrderNumberByStatus

        public static List<int> GetOrderCustomerIdsByStatus(int statusId, int minutesCount)
        {
            var OrderCustomerIdResult = DataAccess.ExecWithStoreProcedureListParam<int>("Core", "GetOrderCustomerIdsByStatus",
                new SqlParameter("StatusId", SqlDbType.Int) { Value = statusId },
                new SqlParameter("MinutesCount", SqlDbType.Int) { Value = minutesCount }
             );
            return OrderCustomerIdResult.ToList();
        }

        #endregion

        #region GetInvoiceNumbersByOrderID

        public static List<decimal> GetInvoiceNumbersByOrderID(int orderId)
        {
            var OrderCustomerIdResult = DataAccess.ExecWithStoreProcedureListParam<decimal>("Core", "SPGetInvoiceNumbersByOrderID",
                new SqlParameter("OrderID", SqlDbType.Int) { Value = orderId }
             );
            return OrderCustomerIdResult.ToList();
        }

        #endregion

        #region GetOrderInvoiceDetail

        public static DataTable GetOrderInvoiceDetail(string invoiceNumber)
        {
            try
            {
                SqlParameter[] LstParameter = new SqlParameter[] 
                {
                    new SqlParameter(){SqlDbType=System.Data.SqlDbType.VarChar,Value=invoiceNumber,ParameterName="@InvoiceNumber"}
                };

                using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand ocom = new SqlCommand())
                    {

                        DataSet dsResult = new DataSet();
                        ocom.Connection = connection;
                        ocom.CommandText = "SPGetOrderInvoiceDetail";
                        ocom.CommandType = CommandType.StoredProcedure;
                        ocom.Parameters.AddRange(LstParameter);
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(ocom);
                        dataAdapter.SelectCommand.ExecuteNonQuery();

                        dataAdapter.Fill(dsResult);

                        return dsResult.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion

        public static PaginatedList<DailyOrderSearchData> SearchDialyOrders(DailyOrderSearchParameters searchParameter)
        {
            object RowsCount;
            List<DailyOrderSearchData> paginatedResult = DataAccess.ExecWithStoreProcedureListParam<DailyOrderSearchData>("Core", "upsDailyOrdersFilter", out RowsCount,
                new SqlParameter("GetAll", SqlDbType.Bit) { Value = (object)searchParameter.GetAll },
                new SqlParameter("LanguageID", SqlDbType.Int) { Value = (object)searchParameter.LanguageID },
                new SqlParameter("AccountName", SqlDbType.VarChar) { Value = (object)searchParameter.AccountName },
                new SqlParameter("AccountNumber", SqlDbType.VarChar) { Value = (object)searchParameter.AccountNumber },
                new SqlParameter("OrderNumber", SqlDbType.VarChar) { Value = (object)searchParameter.OrderNumber },
                new SqlParameter("CompleteDateStart", SqlDbType.Date) { Value = (object)searchParameter.CompleteDateStart ?? DBNull.Value },
                new SqlParameter("CompleteDateEnd", SqlDbType.Date) { Value = (object)searchParameter.CompleteDateEnd ?? DBNull.Value },
                new SqlParameter("SubTotalMin", SqlDbType.Decimal) { Value = (object)searchParameter.SubTotalMin ?? DBNull.Value },
                new SqlParameter("SubTotalMax", SqlDbType.Decimal) { Value = (object)searchParameter.SubTotalMax ?? DBNull.Value },
                new SqlParameter("TotalMin", SqlDbType.Decimal) { Value = (object)searchParameter.GrandTotalMin ?? DBNull.Value },
                new SqlParameter("TotalMax", SqlDbType.Decimal) { Value = (object)searchParameter.GrandTotalMax ?? DBNull.Value },
                new SqlParameter("PageSize", SqlDbType.Int) { Value = searchParameter.PageSize },
                new SqlParameter("PageNumber", SqlDbType.Int) { Value = searchParameter.PageIndex },
                new SqlParameter("Colum", SqlDbType.VarChar) { Value = (object)searchParameter.OrderBy ?? DBNull.Value },
                new SqlParameter("RowsCount", SqlDbType.Int) { Value = 0, Direction = ParameterDirection.Output }
                ).ToList();

            IQueryable<DailyOrderSearchData> matchingItems = paginatedResult.AsQueryable<DailyOrderSearchData>();

            var resultTotalCount = (int)RowsCount;
            return matchingItems.ToPaginatedList<DailyOrderSearchData>(searchParameter, resultTotalCount);
        }
    }
}
