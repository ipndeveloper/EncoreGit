using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;

namespace NetSteps.Data.Entities.Extensions
{
    public class PreOrderExtension
    {
        #region PreOrders (FHP-CSTI)

        public static int CreatePreOrder(PreOrder preOrderModel)
        {
            int preOrderId = DataAccess.ExecWithStoreProcedureSaveIdentity("Core", "upsInsPreOrders",
                new SqlParameter("AccountID", SqlDbType.Int) { Value = preOrderModel.AccountID },
                new SqlParameter("SiteID", SqlDbType.Int) { Value = preOrderModel.SiteID }
                );
            return preOrderId;
        }

        public static int InsPreOrdersDistributor(int AccountID, int SiteID, int OrderID)
        {
            int preOrderId = DataAccess.ExecWithStoreProcedureSaveIdentity("Core", "upsInsPreOrdersDistributor",
                new SqlParameter("AccountID", SqlDbType.Int) { Value = AccountID },
                new SqlParameter("SiteID", SqlDbType.Int) { Value = SiteID },
                new SqlParameter("OrderId", SqlDbType.Int) { Value = OrderID } 
                
                );
            return preOrderId;
        } 

        public static decimal GetCreditLedgerByAccountID(int AccountID)
        {
            decimal periodResult = DataAccess.ExecWithStoreProcedureSaveIdentity("Commissions", "uspGetCreditLedgerByAccountID",
                new SqlParameter("AccountID", SqlDbType.Int) { Value = AccountID });
            return periodResult;
        }

        public static void InsWarehouseMaterialAllocationsOrder(int Quantity, int ProductID, int MaterialId, PreOrder preOrder)
        {
            var result = DataAccess.ExecWithStoreProcedureSave("Core", "uspInsWarehouseMaterialAllocationsOrder",
                new SqlParameter("PreOrderID", SqlDbType.Int) { Value = preOrder.PreOrderId },
                new SqlParameter("Quantity", SqlDbType.Int) { Value = Quantity },
                new SqlParameter("ProductID", SqlDbType.Int) { Value = ProductID },
                new SqlParameter("MaterialId", SqlDbType.Int) { Value = MaterialId },
                new SqlParameter("WarehouseID", SqlDbType.Int) { Value = preOrder.WareHouseID }
                );
        }

        /// <summary>
        /// Método que actualiza en la tabla wareHouseMaterials
        /// </summary>
        /// <param name="productID">Código del producto</param>
        /// <param name="quantity">Cantidad de registrada por producto en la orden</param>
        /// <param name="wareHouseID">Código del WareHouseID</param>
        public static void UpdWareHouseMaterials(int productID, int quantity, int wareHouseID)
        {
            var result = DataAccess.ExecWithStoreProcedureSave("Core", "uspUpdWareHouseMaterials",
                new SqlParameter("WarehouseID", SqlDbType.Int) { Value = wareHouseID },
                new SqlParameter("MaterialID", SqlDbType.Int) { Value = productID },
                new SqlParameter("Quantity", SqlDbType.Int) { Value = quantity }
                );
        }

        /// <summary>
        /// Método que registra en la tabla wareHouseMaterials en base al OrderType = PreOrder CREATE BY FHP
        /// </summary>
        /// <param name="materialID">>Código del Material</param>
        /// <param name="wareHouseID">>Código del WareHouse</param>
        /// <param name="quantity">Cantidad de registrada por producto en la orden</param>
        /// <param name="productID">Código del Producto></param>
        /// <param name="orderID">>Código de la Orden</param>
        public static void InsWareHouseMaterialAllocationPreOrder(int materialID, int wareHouseID, int quantity, int productID, int orderID)
        {
            var result = DataAccess.ExecWithStoreProcedureSave("Core", "uspInsWarehouseMaterialAllocationsPreOrder",
                new SqlParameter("PreOrderID", SqlDbType.Int) { Value = orderID },
                new SqlParameter("Quantity", SqlDbType.Int) { Value = quantity },
                new SqlParameter("ProductID", SqlDbType.Int) { Value = productID },
                new SqlParameter("MaterialId", SqlDbType.Int) { Value = materialID },
                new SqlParameter("WarehouseID", SqlDbType.Int) { Value = wareHouseID }
                );
        }

        /// <summary>
        /// Método que registra en la tabla wareHouseMaterials en base al OrderType = Order CREATE BY FHP 
        /// </summary>
        /// <param name="materialID">>Código del Material</param>
        /// <param name="wareHouseID">>Código del WareHouse</param>
        /// <param name="quantity">Cantidad de registrada por producto en la orden</param>
        /// <param name="productID">Código del Producto></param>
        /// <param name="orderID">>Código de la Orden</param>
        public static void InsWarehouseMaterialAllocationOrder(int materialID, int wareHouseID, int quantity, int productID, int orderID)
        {
            var result = DataAccess.ExecWithStoreProcedureSave("Core", "uspInsWarehouseMaterialAllocationsOrder",
                new SqlParameter("OrderID", SqlDbType.Int) { Value = orderID },
                new SqlParameter("Quantity", SqlDbType.Int) { Value = quantity },
                new SqlParameter("ProductID", SqlDbType.Int) { Value = productID },
                new SqlParameter("MaterialId", SqlDbType.Int) { Value = materialID },
                new SqlParameter("WarehouseID", SqlDbType.Int) { Value = wareHouseID }
                );
        }

        /// <summary>
        /// Método que valida la disponibilidad del producto solicitado CREATE BY FHP 
        /// </summary>
        /// <param name="productoID">Código del producto a consultar</param>
        /// <param name="wareHouseID">Código del WareHouse a consultar</param>
        /// <returns>Lista con cantidades disponibles con el su respectivo Material</returns>
        public static List<InventoryCheck> InventoryCheckResult(int productoID, int wareHouseID)
        {
            List<InventoryCheck> InventoryCheckResult = DataAccess.ExecWithStoreProcedureListParam<InventoryCheck>("Core", "upsAvailabilityValidate",
                 new SqlParameter("ProductoSolicitadoID", SqlDbType.Int) { Value = productoID },
                new SqlParameter("WarehouseID", SqlDbType.Int) { Value = wareHouseID }
                ).ToList();
            return InventoryCheckResult;
        }

        public static List<DispatchOrderItems> GetProductDispatch(int OrderItemID)
        {
            List<DispatchOrderItems> InventoryCheckResult = DataAccess.ExecWithStoreProcedureListParam<DispatchOrderItems>("Core", "uspGetProductDispatch",
                 new SqlParameter("OrderItemID ", SqlDbType.Int) { Value = OrderItemID } 
                ).ToList();
            return InventoryCheckResult;
        }
        
        /// <summary>
        /// Método que valida si el producto es un KIT CREATE BY FHP
        /// </summary>
        /// <param name="productoID">Código del producto a consultar</param>
        /// <returns>El si el producto es un KIT</returns>
        public static bool IsKitProduct(int productoID)
        {
            bool isKitProductResult = DataAccess.ExecWithStoreProcedureBool("Core", "upsIsProductKit",
                new SqlParameter("ProductoSolicitadoID", SqlDbType.Int) { Value = productoID }
                );
            return isKitProductResult;
        }

        /// <summary>
        /// Método que retorna la existencia de productos CREATE BY FHP
        /// </summary>
        /// <param name="k">Variable que inicializa</param>
        /// <param name="productID">Código del producto a consultar</param>
        /// <param name="materialID">Código del material a consultar</param>
        /// <returns>Objeto de Replacement</returns>
        public static List<Replacement> ReplacementResult(int k, int productID, int materialID)
        {
            List<Replacement> replacementResults = DataAccess.ExecWithStoreProcedureListParam<Replacement>("Core", "upsListIncludeReplacements",
                new SqlParameter("k", SqlDbType.Int) { Value = k },
                new SqlParameter("ProductID", SqlDbType.Int) { Value = productID },
                new SqlParameter("MaterialID", SqlDbType.Int) { Value = materialID }
               ).ToList();
            return replacementResults;
        }

        /// <summary>
        /// Método que retorna la disponibilidad del producto. CREATE BY FHP
        /// </summary>
        /// <param name="materialID">Código del material a consultar</param>
        /// <param name="wareHouseID">Código del WareHouse a consultar</param>
        /// <returns></returns>
        public static List<IncludeInventoryCheck> IncludeInventoryCheckResult(int materialID, int wareHouseID)
        {
            List<IncludeInventoryCheck> includeInventoryCheckResult = DataAccess.ExecWithStoreProcedureListParam<IncludeInventoryCheck>("Core", "uspIsDisponibleReplacement",
                new SqlParameter("MaterialID", SqlDbType.Int) { Value = materialID },
                new SqlParameter("WarehouseID", SqlDbType.Int) { Value = wareHouseID }
               ).ToList();
            return includeInventoryCheckResult;
        }

        /// <summary>
        /// Método que retorna el credito del producto CREATE BY FHP
        /// </summary>
        /// <param name="accountID">Código de la cuenta</param>
        /// <returns>Retorna credito del producto</returns>
        public static decimal GetProductCreditByAccount(int accountID)
        {
            return DataAccess.ExecWithStoreProcedureDecimal("Commissions", "uspProductCreditByAccount",
                new SqlParameter("AccountId", SqlDbType.Int) { Value = accountID }
               );
        }

        /// <summary>
        /// Método que retorna el credito del producto CREATE BY FHP
        /// </summary>
        /// <param name="accountID">Código de la cuenta</param>
        /// <returns>Retorna credito del producto</returns>
        public static decimal GetProductCreditByAccountDet(int OrderNumber)
        {
            return DataAccess.ExecWithStoreProcedureDecimal("Core", "uspGetProductCreditDet",
                new SqlParameter("OrderNumber", SqlDbType.Int) { Value = OrderNumber }
               );
        }
        public static IEnumerable<int> GetProductByCategory(int CategoryID)
        {
            IEnumerable<int> inventoryCheckDetail = DataAccess.ExecWithStoreProcedureListParam<int>("Core", "uspGetProductByCategory",
                 new SqlParameter("CategoryID", SqlDbType.Int) { Value = CategoryID }
                ).ToList();
            return inventoryCheckDetail;
        }


        public static int GetPhaseCategory(int CategoryID)
        {
            return DataAccess.ExecWithStoreProcedureScalarType<int>("Core", "uspGetPhaseCategory",
                new SqlParameter("CategoryID", SqlDbType.Int) { Value = CategoryID }
                );
        }

        /// <summary>
        /// Método que actualiza una preOrder al
        /// </summary>
        /// <param name="preOrderID">Código de la PreOrder</param>
        /// <param name="orderId">Código de Orden</param>
        public static void UpdatePreOrder(int preOrderID, int orderId)
        {
            var result = DataAccess.ExecWithStoreProcedureSave("Core", "uspUpdPreOrders",
                new SqlParameter("PreOrderID", SqlDbType.Int) { Value = preOrderID },
                new SqlParameter("OrderId", SqlDbType.Int) { Value = orderId }
                );
        }

        public static List<PreOrder> GetPreOrderByOrderNumber(string orderNumber)
        {
            return DataAccess.ExecWithStoreProcedureListParam<PreOrder>("Core", "uspListPreOrderByOrderNumber",
                new SqlParameter("OrderNumber", SqlDbType.VarChar) { Value = orderNumber }).ToList();
        }

        public static List<CheckKitReplacement> CheckKitReplacementResult(int k, int ProductID, int MaterialID)
        {
            List<CheckKitReplacement> replacementResult = DataAccess.ExecWithStoreProcedureListParam<CheckKitReplacement>("Core", "upsListIncludeReplacements",
                new SqlParameter("k", SqlDbType.Int) { Value = k },
                new SqlParameter("ProductID", SqlDbType.Int) { Value = ProductID },
                new SqlParameter("MaterialID", SqlDbType.Int) { Value = MaterialID }
               ).ToList();
            return replacementResult;
        }



        public static List<CheckKitInventory> CheckKitInventoryResult(int MaterialID, int wareHouseID)
        {
            List<CheckKitInventory> checkKitInventoryResult = DataAccess.ExecWithStoreProcedureListParam<CheckKitInventory>("Core", "uspIsDisponibleReplacement",
                new SqlParameter("MaterialID", SqlDbType.Int) { Value = MaterialID },
                new SqlParameter("WarehouseID", SqlDbType.Int) { Value = wareHouseID }
               ).ToList();
            return checkKitInventoryResult;
        }

        public static List<ProductMaterial> ProductMaterials(int ProductoSolicitadoID)
        {
            List<ProductMaterial> configKit = DataAccess.ExecWithStoreProcedureListParam<ProductMaterial>("Core", "uspProductMaterials",
                 new SqlParameter("ProductID", SqlDbType.Int) { Value = ProductoSolicitadoID }
                ).ToList();
            return configKit;
        }

        public static List<ProductPromotion> GetPromotionByProductID(int ProductID)
        {
            return DataAccess.ExecWithStoreProcedureListParam<ProductPromotion>("Core", "uspListPromotionByProductID",
                 new SqlParameter("ProductID", SqlDbType.Int) { Value = ProductID }
                ).ToList();
        }

        public static List<InventoryCheck> GetMaterial(int ProductID, int PreOrderId)
        {
            return DataAccess.ExecWithStoreProcedureListParam<InventoryCheck>("Core", "uspGetMaterial",
                 new SqlParameter("ProductID", SqlDbType.Int) { Value = ProductID },
                 new SqlParameter("PreOrderId", SqlDbType.Int) { Value = PreOrderId }
                ).ToList();
        }
         
         

        public static void DelWareHouseMaterialsAllocationsXPreOrder(int ProductID, int PreOrderId)
        {
            var result = DataAccess.ExecWithStoreProcedureSave("Core", "DelWareHouseMaterialsAllocationsByPreOrders",// uspDelWareHouseMaterialsAllocationsXPreOrder
                new SqlParameter("PreOrderID", SqlDbType.Int) { Value = PreOrderId },
                new SqlParameter("ProductID", SqlDbType.Int) { Value = ProductID } 
                );
        }

        public static void updOrderAmounts(int OrderId)
        {
            var result = DataAccess.ExecWithStoreProcedureSave("Core", "uspUpdOrderAmounts",// uspDelWareHouseMaterialsAllocationsXPreOrder
                new SqlParameter("OrderId", SqlDbType.Int) { Value = OrderId }
                );
        }
        public static void udpOrderPending(int OrderId, bool EsOrderRetonorTotal = true)
        {
            var result = DataAccess.ExecWithStoreProcedureSave("Core", "uspUdpOrderPending",// uspDelWareHouseMaterialsAllocationsXPreOrder
                new SqlParameter("OrderID", SqlDbType.Int) { Value = OrderId },
                new SqlParameter("EsOrderRetonorTotal", SqlDbType.Bit) { Value = EsOrderRetonorTotal }
                );
        }


        public static void InsOrderItemsPending(int OrderCustomerID, int OrderItemID, int ParentOrderItemID, decimal ItemPrice)
        {
            var result = DataAccess.ExecWithStoreProcedureSave("Core", "uspInsOrderItemsPending",// uspDelWareHouseMaterialsAllocationsXPreOrder
                new SqlParameter("OrderCustomerID", SqlDbType.Int) { Value = OrderCustomerID },
                new SqlParameter("OrderItemID", SqlDbType.Int) { Value = OrderItemID },
                new SqlParameter("ParentOrderItemID", SqlDbType.Int) { Value = ParentOrderItemID },
                new SqlParameter("ItemPrice", SqlDbType.Decimal) { Value = ItemPrice }
                );
        }

        public static void UpdTotalsPending(int OrderID, int OrderParent, bool esOrdenTotal)
        {
            var result = DataAccess.ExecWithStoreProcedureSave("Core", "uspTotalsPending",// uspDelWareHouseMaterialsAllocationsXPreOrder
                new SqlParameter("OrderID", SqlDbType.Int) { Value = OrderID },
                new SqlParameter("OrderParent", SqlDbType.Int) { Value = OrderParent },
                new SqlParameter("EsOrdenTotal", SqlDbType.Bit) { Value = esOrdenTotal } 
                );
        }

        public static int uspGetStatusOrder(int ParentOrderID)
        { 
            return DataAccess.ExecWithStoreProcedureScalarType<int>("Core", "uspGetStatusOrder",
                new SqlParameter("ParentOrderID", SqlDbType.Int) { Value = ParentOrderID } 
                );
        }

        public static int UpdWarehouseMaterial(int MaterialID, int Quantity, int wareHouseID)
        {
            return DataAccess.ExecWithStoreProcedureSave("Core", "uspUpdWarehouseMaterial",
                new SqlParameter("MaterialID", SqlDbType.Int) { Value = MaterialID },
                new SqlParameter("WarehouseID", SqlDbType.Int) { Value = wareHouseID },
                new SqlParameter("Quantity", SqlDbType.Int) { Value = Quantity }
                );
        }

        //public static int GetValueURL()
        //{
        //    return DataAccess.ExecWithStoreProcedureScalarType<int>("Core", "uspGetValueURL",null);
        //}
        public static List<decimal> GetValueURL()
        {
            return DataAccess.ExecWithStoreProcedure<decimal>(ConnectionStrings.BelcorpCommission, "uspGetValueURL",
                 new SqlParameter("Name", SqlDbType.VarChar) { Value = "'URLUsa'" }
                ).ToList();
        }
        



        public static List<CheckKitInventory> GetWareHouseMaterialAllocations(int PreOrderId, int ProductID)
        {
            return DataAccess.ExecWithStoreProcedure<CheckKitInventory>(ConnectionStrings.BelcorpCore, "GetWareHouseMaterialAllocations",
                 new SqlParameter("PreOrderId", SqlDbType.Int) { Value = PreOrderId },
                 new SqlParameter("ProductID", SqlDbType.Int) { Value = ProductID }
                ).ToList();
        }

        public static void UpdWarehouseMaterial(int PreOrderId)
        {
            var result = DataAccess.ExecWithStoreProcedureSave("Core", "uspDelPreOrdersbyId",
                new SqlParameter("PreOrderID", SqlDbType.Int) { Value = PreOrderId } 
                );
        }

        public static void DelWareHouseMaterialsAllocationXOrder(int OrderID, int MaterialID, int wareHouseID)
        {
            var result = DataAccess.ExecWithStoreProcedureSave("Core", "uspDelWareHouseMaterialsAllocationXOrder",//uspDelWareHouseMaterialsAllocationXOrder
                new SqlParameter("OrderID", SqlDbType.Int) { Value = OrderID },
                new SqlParameter("MaterialID", SqlDbType.Int) { Value = MaterialID },
                new SqlParameter("WarehouseID", SqlDbType.Int) { Value = wareHouseID }
                );
        }



        public static List<InventoryCheckDetail> InventoryCheckDetails(int MaterialID)
        {
            List<InventoryCheckDetail> inventoryCheckDetail = DataAccess.ExecWithStoreProcedureListParam<InventoryCheckDetail>("Core", "uspInventoryCheckDetail",
                 new SqlParameter("MaterialID", SqlDbType.Int) { Value = MaterialID }
                ).ToList();
            return inventoryCheckDetail;
        }

        public static List<ConfigKit> ConfigKits(int ProductoSolicitadoID)
        {
            List<ConfigKit> configKit = DataAccess.ExecWithStoreProcedureListParam<ConfigKit>("Core", "uspConfigKit",
                 new SqlParameter("ProductID", SqlDbType.Int) { Value = ProductoSolicitadoID }
                ).ToList();
            return configKit;
        }

        //public static int IsActiveProductMaterial(int ProductID)
        public static List<int> TakeActiveProductMaterial(IEnumerable<int> Products)
        {
            List<int> Result = new List<int>();
            using (SqlConnection connection = new SqlConnection(DataAccess.GetConnectionString("Core")))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "dbo.uspGetActiveProductMaterial";
                    command.CommandType = CommandType.StoredProcedure;

                    SqlParameter P1;
                    P1 = command.Parameters.AddWithValue("@Products", CreateDataTable(Products));
                    P1.SqlDbType = SqlDbType.Structured;
                    P1.TypeName = "dbo.TypeTableInputProducts";

                    SqlParameter P2;
                    P2 = command.Parameters.AddWithValue("@StoreFrontID", NetSteps.Data.Entities.ApplicationContext.Instance.StoreFrontID);

                    SqlDataReader dr;
                    dr = command.ExecuteReader();

                    while (dr.Read()) Result.Add(Convert.ToInt32(dr["ProductID"]));
                }
            }
            return Result;


            //***************************************************************************************************************************************
            //int sf = NetSteps.Data.Entities.ApplicationContext.Instance.StoreFrontID;
            //int isActiveResult = DataAccess.ExecWithStoreProcedureSaveIdentity("Core", "uspIsActiveProductMaterial",
            //                                                                    new SqlParameter("ProductID", SqlDbType.Int) { Value = ProductID },
            //                                                                    new SqlParameter("StoreFrontID", SqlDbType.Int) { Value = sf }
            //                                                                    );

            //return isActiveResult;
        }

        private static DataTable CreateDataTable(IEnumerable<int> ids)
        {
            DataTable table = new DataTable();
            table.Columns.Add("ProductID", typeof(int));
            foreach (int id in ids) table.Rows.Add(id);
            return table;
        }


        public static int WareHouseByAccountAddress(int accountID, int addressID)
        {         
            return DataAccess.ExecWithStoreProcedureSaveIdentity("Core", "uspWareHouseByAccountAddress",
                new SqlParameter("AccountID", SqlDbType.Int) { Value = accountID },
                new SqlParameter("AddressID", SqlDbType.Int) { Value = addressID }
                );
        }

		public static List<MaterialIDs> GetMaterialIdByProductIDPreOrderId(int ProductID, int PreOrderID)
        {
            return DataAccess.ExecWithStoreProcedureListParam<MaterialIDs>("Core", "USP_GetMaterialIdByProductIDPreOrderId",
                new SqlParameter("ProductID", SqlDbType.Int) { Value = ProductID },
                new SqlParameter("PreOrderID", SqlDbType.Int) { Value = PreOrderID }
               ).ToList();
        }
 
        public static int GetIncludeReplacementPriority(int ProductoSolicitadoID, int MaterialID)
        {
            return DataAccess.ExecWithStoreProcedureSaveIdentity("Core", "upsListIncludeReplacementPriority",
                new SqlParameter("ProductoSolicitadoID", SqlDbType.Int) { Value = ProductoSolicitadoID },
                new SqlParameter("MaterialID", SqlDbType.Int) { Value = MaterialID }
                );
        }

        public static List<OrdersStatus> GetStatusOrder(int? accountID)
        {
            return DataAccess.ExecWithStoreProcedureListParam<OrdersStatus>("Core", "uspGetStatusOrderId",
                new SqlParameter("AccountID", SqlDbType.Int) { Value = accountID }
               ).ToList();
        }

        public static string GetOrderNumberByAccount(int AccountID, int OrderTypeID, int SiteID)
        {
            return DataAccess.ExecWithStoreProcedureScalarType<string> ("Core", "uspGetOrderNumberByAccount",
                new SqlParameter("AccountID", SqlDbType.Int) { Value = AccountID },
                new SqlParameter("OrderTypeID", SqlDbType.Int) { Value = OrderTypeID },
                new SqlParameter("SiteID", SqlDbType.Int) { Value = SiteID }
                );
        }
 
        public static void InsWarehouseMaterialAllocationLogs(WarehouseMaterialAllocationLogsParameters parametersLogs)
        {
                DataAccess.ExecWithStoreProcedureSave("Core", "uspInsWarehouseMaterialAllocationLogs",
                new SqlParameter("WareHouseMaterialId", SqlDbType.Int) { Value = parametersLogs.WareHouseMaterialId },
                new SqlParameter("InventoryMovementTypeID", SqlDbType.Int) { Value = parametersLogs.InventoryMovementTypeID },
                new SqlParameter("QuantityAllocatedBefore", SqlDbType.Int) { Value = parametersLogs.QuantityAllocatedBefore },
                new SqlParameter("QuantityMov", SqlDbType.Int) { Value = parametersLogs.QuantityMov },
                new SqlParameter("QuantityAllocatedAfter", SqlDbType.Int) { Value = parametersLogs.QuantityAllocatedAfter },
                new SqlParameter("AverageCost", SqlDbType.Decimal) { Value = parametersLogs.AverageCost },
                new SqlParameter("Description", SqlDbType.VarChar) { Value = parametersLogs.Description },                    
                new SqlParameter("userID", SqlDbType.Int) { Value = parametersLogs.userID },
                new SqlParameter("PreOrderID", SqlDbType.Int) { Value = parametersLogs.PreOrderID }
                );
        }

        public static List<WarehouseMaterialLacksData> GetWarehouseMaterialLacksByPreOrder(int preOrderID, int laguangeId)
        {
            return  DataAccess.ExecWithStoreProcedureListParam<WarehouseMaterialLacksData>("Core", "uspGetWarehouseMaterialLacksByPreOrder",
                new SqlParameter("PreOrderId", SqlDbType.Int) { Value = preOrderID },
                new SqlParameter("LanguageID", SqlDbType.Int) { Value = laguangeId}
                ).ToList();
        }

        public static void InsWarehouseMaterialLacks(WarehouseMaterialLacksParameters parametersLogs)
        {
                DataAccess.ExecWithStoreProcedureSave("Core", "uspInsWarehouseMaterialLacks",
                new SqlParameter("WarehouseMaterialID", SqlDbType.Int) { Value = parametersLogs.WarehouseMaterialID },
                new SqlParameter("PreOrderId", SqlDbType.Int) { Value = parametersLogs.PreOrderId }, 
                new SqlParameter("QuantityLack", SqlDbType.Int) { Value = parametersLogs.QuantityLack },
                new SqlParameter("ProductId", SqlDbType.Int) { Value = parametersLogs.ProductId },
                new SqlParameter("OfferType", SqlDbType.Decimal) { Value = parametersLogs.OfferType },
                new SqlParameter("ProductPriceTypeId", SqlDbType.Int) { Value = parametersLogs.ProductPriceTypeId },
                new SqlParameter("Motive", SqlDbType.VarChar) { Value = parametersLogs.Motive }
                
                );
        }
          
        public static int GetWarehouseMaterialID(int MaterialID, int WarehouseID)
        {
            return DataAccess.ExecWithStoreProcedureScalarType<int>("Core", "uspGetWarehouseMaterialID",
                new SqlParameter("MaterialID", SqlDbType.Int) { Value = MaterialID },
                new SqlParameter("WarehouseID", SqlDbType.Int) { Value = WarehouseID }
                );
        }

        public static int GetMaterialIdByProductId(int ProductID)
        {
            return DataAccess.ExecWithStoreProcedureScalarType<int>("Core", "uspGetMaterialIdByProductId",
                new SqlParameter("ProductID ", SqlDbType.Int) { Value = ProductID }
                );
        }

        public static int GetOfertTypeByMaterialId(int MaterialID, int ParentProductID)
        {
            return DataAccess.ExecWithStoreProcedureScalarType<int>("Core", "uspGetOfertTypeByMaterialId",
                new SqlParameter("MaterialID", SqlDbType.Int) { Value = MaterialID },
                new SqlParameter("ParentProductID", SqlDbType.Int) { Value = ParentProductID }
                );
        } 

        public static List<AddLineValidStock> GetAddLineValidStock(int WarehouseID, int Quantity, int ProductID,int PreOrderID, int AccountTypeID)
        {
            return DataAccess.ExecWithStoreProcedureListParam<AddLineValidStock>("Core", "uspAddLineValidStock",
                new SqlParameter("WarehouseID", SqlDbType.Int) { Value = WarehouseID },
                new SqlParameter("Quantity", SqlDbType.Int) { Value = Quantity },
                new SqlParameter("ProductID", SqlDbType.Int) { Value = ProductID },
                new SqlParameter("PreOrderID", SqlDbType.Int) { Value = PreOrderID },
                new SqlParameter("AccountTypeID", SqlDbType.Int) { Value = AccountTypeID }
               ).ToList();
        }

        public static List<MaterialDTO> GetsMaterialID(int materialID)
        {
            return DataAccess.ExecWithStoreProcedureListParam<MaterialDTO>("Core", "uspGetsMaterialIDByMaterialID",
                new SqlParameter("MaterialID", SqlDbType.Int) { Value = materialID }            
               ).ToList();
        }

        public static int GetProductRelationsTypeID(int ProductID)
        {
            return DataAccess.ExecWithStoreProcedureScalarType<int>("Core", "uspGetProductRelationsTypeID",
                new SqlParameter("ParentProductID", SqlDbType.Int) { Value = ProductID }
               );
        }

		public static int GetMaterialIdPRR(int ProductRelationID)
        {
            return DataAccess.ExecWithStoreProcedureScalarType<int>("Core", "USPGetMaterialIdPRR",
                new SqlParameter("ProductRelationID", SqlDbType.Int) { Value = ProductRelationID }
               );
        }
 
        public static int GetProductIDByMaterialID(int MaterialID)
        {
            return DataAccess.ExecWithStoreProcedureScalarType<int>("Core", "uspGetSKUByMaterialID",
                new SqlParameter("MaterialID", SqlDbType.Int) { Value = MaterialID }
               );
        }


        public static List<KitItemPrice> GetKitItemPrices(int OrderID)
        {
            return DataAccess.ExecWithStoreProcedureListParam<KitItemPrice>("Core", "uspGetKitItemPrices",
                new SqlParameter("OrderID", SqlDbType.Int) { Value = OrderID }              
               ).ToList();
        }

        public static List<MaterialDTO> GetMaterialQuantityByPreOrderID(int preOrderID, int productID)        {
            return DataAccess.ExecWithStoreProcedureListParam<MaterialDTO>("Core", "uspGetQuantityMaterialPreOrder",                
                new SqlParameter("PreOrderID", SqlDbType.Int) { Value = preOrderID },
                new SqlParameter("ProductID", SqlDbType.Int) { Value = productID }                
               ).ToList();
        }

        public static List<MaterialValionDTO> GetKitComposition(int productID)
        {
            return DataAccess.ExecWithStoreProcedureListParam<MaterialValionDTO>("Core", "uspGetKitCompositionValidaton",                
                new SqlParameter("ProductID", SqlDbType.Int) { Value = productID }                
               ).ToList();
        }


        public static List<ParameterCountriesData> GetParameterCountriesByCountyIdStep(int CountryId, int Step)
        {
            return DataAccess.ExecWithStoreProcedureListParam<ParameterCountriesData>("Core", "GetParameterCountriesByCountyIdStep",
                new SqlParameter("CountryId", SqlDbType.Int) { Value = CountryId },
                new SqlParameter("Step", SqlDbType.Int) { Value = Step }
               ).ToList();
        }  


        #endregion
    }
}
