using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using NetSteps.Data.Entities.Business;
using System.Data.SqlClient;
using System.Data;
using NetSteps.Data.Entities.Repositories.Interfaces;

namespace NetSteps.Data.Entities.Repositories
{
    /// <summary>
    /// Clase Repository que implenta el acceso a Datos Create By - FHP
    /// </summary>
    public class PreOrderRepository : IPreOrder
    {
        /// <summary>
        /// Método genera un nuevo registro a la tabla PreOrders Create By - FHP
        /// </summary>
        /// <param name="accountID">Código del Account</param>
        /// <param name="siteID">Código del Site</param>
        /// <returns>Cantidad de filas afectadas</returns>
        public int CreatePreOrder(int accountID, int siteID)
        {
            return DataAccess.ExecWithStoreProcedureScalar(ConnectionStrings.BelcorpCore, "upsInsPreOrders",
                new SqlParameter("AccountID", SqlDbType.Int) { Value = accountID },
                new SqlParameter("SiteID", SqlDbType.Int) { Value = siteID }
                );
        } 
         
        /// <summary>
        /// Método que obtiene el credito de una Account CREATE BY - FHP
        /// </summary>
        /// <param name="accountID">Código del Account</param>
        /// <returns>Credito disponible</returns>
        public decimal GetCreditLedgerByAccountID(int accountID)
        {
            return Convert.ToDecimal(DataAccess.ExecWithStoreProcedureScalarType<decimal>(ConnectionStrings.BelcorpCommission, "uspGetCreditLedgerByAccountID",
              new SqlParameter("AccountID", SqlDbType.Int) { Value = accountID }));
             
        }

        /// <summary>
        /// Método que registra en la tabla wareHouseMaterials en base al OrderType = Order CREATE BY FHP 
        /// </summary>
        /// <param name="materialID">>Código del Material</param>
        /// <param name="wareHouseID">>Código del WareHouse</param>
        /// <param name="quantity">Cantidad de registrada por producto en la orden</param>
        /// <param name="productID">Código del Producto></param>
        /// <param name="orderID">>Código de la Orden</param> 
        /// <returns>Cantidad de filas afectadas</returns>
        public int InsWarehouseMaterialAllocationOrder(int materialID, int wareHouseID, int quantity, int productID, int orderID)
        {
            return DataAccess.ExecWithStoreProcedureSave(ConnectionStrings.BelcorpCore, "uspInsWarehouseMaterialAllocationsOrder",
               new SqlParameter("OrderID", SqlDbType.Int) { Value = orderID },
               new SqlParameter("Quantity", SqlDbType.Int) { Value = quantity },
               new SqlParameter("ProductID", SqlDbType.Int) { Value = productID },
               new SqlParameter("MaterialId", SqlDbType.Int) { Value = materialID },
               new SqlParameter("WarehouseID", SqlDbType.Int) { Value = wareHouseID }
               );
        }

        /// <summary>
        /// Método que valida la disponibilidad del producto solicitado CREATE BY - FHP 
        /// </summary>
        /// <param name="productoID">Código del producto a consultar</param>
        /// <param name="wareHouseID">Código del WareHouse a consultar</param>
        /// <returns>Lista con cantidades disponibles con el su respectivo Material</returns>
        public List<InventoryCheck> InventoryCheckResult(int productoID, int wareHouseID)
        {
            return DataAccess.ExecWithStoreProcedure<InventoryCheck>(ConnectionStrings.BelcorpCore, "upsAvailabilityValidate",
                 new SqlParameter("ProductoSolicitadoID", SqlDbType.Int) { Value = productoID },
                new SqlParameter("WarehouseID", SqlDbType.Int) { Value = wareHouseID }
                ).ToList(); 
        }

        /// <summary>
        /// Método que valida si el producto es un KIT CREATE BY - FHP
        /// </summary>
        /// <param name="productoID">Código del producto a consultar</param>
        /// <returns>El si el producto es un KIT</returns>
        public bool IsKitProduct(int productoID)
        {
            return Convert.ToBoolean(DataAccess.ExecWithStoreProcedureScalarType<bool>(ConnectionStrings.BelcorpCore, "upsIsProductKit",
                new SqlParameter("ProductoSolicitadoID", SqlDbType.Int) { Value = productoID }
                )); 
        }

        /// <summary>
        /// Método que retorna la existencia de productos CREATE BY - FHP
        /// </summary>
        /// <param name="k">Variable que inicializa</param>
        /// <param name="productID">Código del producto a consultar</param>
        /// <param name="materialID">Código del material a consultar</param>
        /// <returns>Objeto de Replacement</returns>
        public List<Replacement> ReplacementResult(int k, int productID, int materialID)
        {
            return DataAccess.ExecWithStoreProcedureListParam<Replacement>(ConnectionStrings.BelcorpCore, "upsListIncludeReplacements",
               new SqlParameter("k", SqlDbType.Int) { Value = k },
               new SqlParameter("ProductID", SqlDbType.Int) { Value = productID },
               new SqlParameter("MaterialID", SqlDbType.Int) { Value = materialID }
              ).ToList(); 
        }

        /// <summary>
        /// Método que retorna la lista IncludeInventoryCheck CREATE BY - FHP
        /// </summary>
        /// <param name="materialID">Código Material</param>
        /// <param name="wareHouseID">Código WareHouse</param>
        /// <returns>Lista IncludeInventoryCheck</returns>
        public List<IncludeInventoryCheck> IncludeInventoryCheckResult(int materialID, int wareHouseID)
        {
            return DataAccess.ExecWithStoreProcedureListParam<IncludeInventoryCheck>(ConnectionStrings.BelcorpCore, "uspIsDisponibleReplacement",
                new SqlParameter("MaterialID", SqlDbType.Int) { Value = materialID },
                new SqlParameter("WarehouseID", SqlDbType.Int) { Value = wareHouseID }
               ).ToList(); 
        }

        /// <summary>
        /// Método que retorna el credito del producto CREATE BY - FHP
        /// </summary>
        /// <param name="accountID">Código de la cuenta</param>
        /// <returns>Retorna credito del producto</returns>
        public decimal GetProductCreditByAccount(int accountID)
        {
            return DataAccess.ExecWithStoreProcedureDecimal(ConnectionStrings.BelcorpCore, "uspProductCreditByAccount",
                new SqlParameter("AccountId", SqlDbType.Int) { Value = accountID }
               );
        }


        /// <summary>
        /// Método que actualiza una preOrder al Create By - FHP
        /// </summary>
        /// <param name="preOrderID">Código de la PreOrder</param>
        /// <param name="orderId">Código de Orden</param>
         /// <returns>Retorna el número de filas afectadas </returns>
        public int UpdatePreOrder(int preOrderID, int orderId)
        {
            return DataAccess.ExecWithStoreProcedureScalar(ConnectionStrings.BelcorpCore, "uspUpdPreOrders",
               new SqlParameter("PreOrderID", SqlDbType.Int) { Value = preOrderID },
               new SqlParameter("OrderId", SqlDbType.Int) { Value = orderId }
               );
        }

        /// <summary>
        /// Método que lista las PreOrdenes por Número de Ordenes Create By - FHP
        /// </summary>
        /// <param name="orderNumber"></param>
        /// <returns></returns>
        public List<PreOrder> GetPreOrderByOrderNumber(string orderNumber)
        {
            return DataAccess.ExecWithStoreProcedure<PreOrder>(ConnectionStrings.BelcorpCore, "uspListPreOrderByOrderNumber",
                new SqlParameter("OrderNumber", SqlDbType.VarChar) { Value = orderNumber }).ToList();
        }
        
        /// <summary>
        /// Método que lista los Kit Replacement
        /// </summary>
        /// <param name="k">Valor entero</param>
        /// <param name="ProductID">Código del Producto</param>
        /// <param name="MaterialID">Código del Material</param>
        /// <returns>Lista de los CheckKitReplacement</returns>
        public List<CheckKitReplacement> CheckKitReplacementResult(int k, int ProductID, int MaterialID)
        {
            return DataAccess.ExecWithStoreProcedure<CheckKitReplacement>(ConnectionStrings.BelcorpCore, "upsListIncludeReplacements",
                new SqlParameter("k", SqlDbType.Int) { Value = k },
                new SqlParameter("ProductID", SqlDbType.Int) { Value = ProductID },
                new SqlParameter("MaterialID", SqlDbType.Int) { Value = MaterialID }
               ).ToList(); 
        }


        /// <summary>
        /// Método que lista los Kit Inventory CREATE BY - FHP
        /// </summary>
        /// <param name="MaterialID">Código Material</param>
        /// <returns>Lista de los CheckKitInventory</returns>
        public List<CheckKitInventory> CheckKitInventoryResult(int MaterialID, int WareHouseID)
        {
            return DataAccess.ExecWithStoreProcedure<CheckKitInventory>(ConnectionStrings.BelcorpCore, "uspIsDisponibleReplacement",
                new SqlParameter("MaterialID", SqlDbType.Int) { Value = MaterialID },
                new SqlParameter("WarehouseID", SqlDbType.Int) { Value = WareHouseID }
               ).ToList(); 
        }

        /// <summary>
        /// Método que lista los ProductosMaterials CREATE BY - FHP
        /// </summary>
        /// <param name="ProductoSolicitadoID">Código del Producto</param>
        /// <returns>Lista de los ProductMaterials</returns>
        public List<ProductMaterial> ProductMaterials(int ProductoSolicitadoID, int WarehouseID)
        {
            return DataAccess.ExecWithStoreProcedure<ProductMaterial>(ConnectionStrings.BelcorpCore, "uspProductMaterial",
                 new SqlParameter("ProductID", SqlDbType.Int) { Value = ProductoSolicitadoID },
                 new SqlParameter("WarehouseID", SqlDbType.Int) { Value = WarehouseID }
                ).ToList(); 
        }

        /// <summary>
        /// Método que retorna la lista de InventoryCheckDetail CREATE BY - FHP
        /// </summary>
        /// <param name="MaterialID">Código del Material</param>
        /// <returns>Lista InventoryCheckDetail</returns>
        public List<InventoryCheckDetail> InventoryCheckDetails(int MaterialID, int wareHouseID)
        {
            return DataAccess.ExecWithStoreProcedure<InventoryCheckDetail>(ConnectionStrings.BelcorpCore, "uspInventoryCheckDetail",
                 new SqlParameter("MaterialID", SqlDbType.Int) { Value = MaterialID },
                 new SqlParameter("WarehouseID", SqlDbType.Int) { Value = wareHouseID }
                ).ToList(); 
        }

      
        /// <summary>
        /// Método que retorna lista de los Kits CREATE BY - FHP
        /// </summary>
        /// <param name="ProductoSolicitadoID">Código del Product</param>
        /// <returns>Retorna lista de los Kits</returns>
        public List<ConfigKit> ConfigKits(int ProductoSolicitadoID, int wareHouseID)
        {
            return DataAccess.ExecWithStoreProcedure<ConfigKit>(ConnectionStrings.BelcorpCore, "uspConfigKit",
                 new SqlParameter("ProductID", SqlDbType.Int) { Value = ProductoSolicitadoID },
                 new SqlParameter("WarehouseID", SqlDbType.Int) { Value = wareHouseID }
                ).ToList(); 
        }

        /// <summary>
        /// Método que retorna si es el ProductMaterial esta activo CREATE BY - FHP
        /// </summary>
        /// <param name="ProductID">Código del Producto</param>
        /// <returns>Valor del campo si es activo</returns>
        public int IsActiveProductMaterial(int ProductID)
        {
            return DataAccess.ExecWithStoreProcedureScalar(ConnectionStrings.BelcorpCore, "uspIsActiveProductMaterial",
                new SqlParameter("ProductID", SqlDbType.Int) { Value = ProductID }
                );
        }

        /// <summary>
        /// Método que retorna el estado de la orden  CREATE BY - FHP
        /// </summary>
        /// <param name="accountID">Código del Account</param>
        /// <returns>Estado de la orden</returns>
        public List<OrdersStatus> GetStatusOrder(int? accountID)
        {
            return DataAccess.ExecWithStoreProcedure<OrdersStatus>(ConnectionStrings.BelcorpCore, "uspGetStatusOrderId",
                new SqlParameter("AccountID", SqlDbType.Int) { Value = accountID }
               ).ToList();
        }

        /// <summary>
        /// Método que asigna las PreOrdenes a una Orden
        /// </summary>
        /// <param name="newOrderID">Código de la Orden</param>
        /// <param name="preOrderId">Código de la PreOrder</param>
        /// <returns>Cantidad de filas afectadas</returns>
        public int AsignarReservasAOrden(int newOrderID, int preOrderId)
        {
            return DataAccess.ExecWithStoreProcedureScalar(ConnectionStrings.BelcorpCore, "uspAsignarReservasAOrden",
                new SqlParameter("@newOrderID", SqlDbType.Int) { Value = newOrderID },
                new SqlParameter("@preOrderId", SqlDbType.Int) { Value = newOrderID }); 
        } 
    }
}
