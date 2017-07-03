using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Repositories.Interfaces;
using System.Data.SqlClient;
using System.Data;
using NetSteps.Data.Entities.Business;

namespace NetSteps.Data.Entities.Repositories
{
    /// <summary>
    /// Clase de acceso a Datos de la tabla WareHouseMaterials Create By - FHP
    /// </summary>
    public class WareHouseMaterialsRepository : IWareHouseMaterialsRepository
    {

        /// <summary>
        /// Permite insertar Datos a la tabla WarehouseMaterialAllocations Create By - FHP
        /// </summary>
        /// <param name="quantity">Cantindad de Productos</param>
        /// <param name="productID">ProductoId a Ingresar</param>
        /// <param name="materialId">Material Id a Ingresar</param>
        /// <returns>Cantidad de filas afectadas</returns>
        public int InsWarehouseMaterialAllocationsOrder(int quantity, int productID, int materialId, int preOrderId, int WareHouseID, int PreOrderId)
        {
            return DataAccess.ExecWithStoreProcedureSave(ConnectionStrings.BelcorpCore, "uspInsWarehouseMaterialAllocationsOrder",
                 new SqlParameter("OrderId", SqlDbType.Int) { Value = preOrderId },
                 new SqlParameter("PreOrderId", SqlDbType.Int) { Value = PreOrderId },
                 new SqlParameter("Quantity", SqlDbType.Int) { Value = quantity },
                 new SqlParameter("ProductId", SqlDbType.Int) { Value = productID },
                 new SqlParameter("MaterialId", SqlDbType.Int) { Value = materialId },
                 new SqlParameter("WarehouseId", SqlDbType.Int) { Value = WareHouseID }
                 );
        }

        /// <summary>
        /// Método que actualiza en la tabla wareHouseMaterials CREATE BY - FHP
        /// </summary>
        /// <param name="productID">Código del producto</param>
        /// <param name="quantity">Cantidad de registrada por producto en la orden</param>
        /// <param name="wareHouseID">Código del WareHouseID</param>
        /// <returns>Cantidad de filas afectadas</returns>
        public int UpdWareHouseMaterials(int materialID, int quantity, int wareHouseID)
        {
            return DataAccess.ExecWithStoreProcedureScalar(ConnectionStrings.BelcorpCore, "uspUpdWareHouseMaterials",
                new SqlParameter("WarehouseID", SqlDbType.Int) { Value = wareHouseID },
                new SqlParameter("MaterialID", SqlDbType.Int) { Value = materialID },
                new SqlParameter("Quantity", SqlDbType.Int) { Value = quantity }
                );
        }

        /// <summary>
        /// Método que registra en la tabla wareHouseMaterials en base al OrderType = PreOrder CREATE BY - FHP
        /// </summary>
        /// <param name="materialID">Código del Material</param>
        /// <param name="wareHouseID">Código del WareHouse</param>
        /// <param name="quantity">Cantidad de registrada por producto en la orden</param>
        /// <param name="productID">Código del Producto></param>
        /// <param name="orderID">Código de la Orden</param>
        /// <returns>Cantidad de filas afectadas</returns>
        public int InsWareHouseMaterialAllocationPreOrder(int materialID, int wareHouseID, int quantity, int productID, int orderID, bool isClaims)
        {
            return DataAccess.ExecWithStoreProcedureSave(ConnectionStrings.BelcorpCore, "uspInsWarehouseMaterialAllocationsPreOrder",
                new SqlParameter("PreOrderID", SqlDbType.Int) { Value = orderID },
                new SqlParameter("Quantity", SqlDbType.Int) { Value = quantity },
                new SqlParameter("ProductID", SqlDbType.Int) { Value = productID },
                new SqlParameter("MaterialId", SqlDbType.Int) { Value = materialID },
                new SqlParameter("WarehouseID", SqlDbType.Int) { Value = wareHouseID },
                new SqlParameter("IsClaims", SqlDbType.Int) { Value = isClaims } 
                );
        }

        /// <summary>
        /// Método que elimina un WareHouseMaterialsAllocations de la BD. CREATE BY - FHP
        /// </summary>
        /// <param name="materialID">Código del Material</param>
        /// <returns>Cantidad de filas afectadas</returns>
        public int DelWareHouseMaterialsAllocationsXPreOrder(int materialID, PreOrder preOrder)
        {
            return DataAccess.ExecWithStoreProcedureScalar(ConnectionStrings.BelcorpCore, "DelWareHouseMaterialsAllocationsXPreOrder",// uspDelWareHouseMaterialsAllocationsXPreOrder
                new SqlParameter("PreOrderID", SqlDbType.Int) { Value = preOrder.OrderID },
                new SqlParameter("MaterialID", SqlDbType.Int) { Value = materialID },
                new SqlParameter("WarehouseID", SqlDbType.Int) { Value = preOrder.WareHouseID }
                );
        }

        /// <summary>
        /// Método que registra en la tabla wareHouseMaterials en base al OrderType = PreOrder CREATE BY - FHP
        /// </summary>
        /// <param name="materialID">Código del Material</param> 
        /// <param name="quantity">Cantidad de registrada por producto en la orden</param> 
        /// <returns>Cantidad de filas afectadas</returns>
        public int UpdWarehouseMaterial(int materialID, int quantity, PreOrder preOrder)
        {
            return DataAccess.ExecWithStoreProcedureScalar(ConnectionStrings.BelcorpCore, "uspUpdWarehouseMaterial",
                new SqlParameter("MaterialID", SqlDbType.Int) { Value = materialID },
                new SqlParameter("WarehouseID", SqlDbType.Int) { Value = preOrder.WareHouseID },
                new SqlParameter("Quantity", SqlDbType.Int) { Value = quantity }
                );
        }
        
        /// <summary>
        /// Método que retorna el Código del WareHouse CREATE BY - FHP
        /// </summary>
        /// <param name="accountID">Código del Account</param>
        /// <param name="addressID">Código del Address</param>
        /// <returns>Código del WareHouse</returns>
        public int WareHouseByAccountAddress(int accountID, int addressID)
        {
            return DataAccess.ExecWithStoreProcedureScalar(ConnectionStrings.BelcorpCore, "uspWareHouseByAccountAddress",
                new SqlParameter("AccountID", SqlDbType.Int) { Value = accountID },
                new SqlParameter("AddressID", SqlDbType.Int) { Value = addressID }
                );  
        }
         
        /// <summary>
        /// Método que elimina WareHouseMaterialsAllocations siempre y cuando sea una PreOrder CREATE BY FHP
        /// </summary>
        /// <param name="materialID">Código del Material</param>
        /// <returns>Cantidad de filas afectadas</returns>
        public int DelWareHouseMaterialsAllocationsByPreOrder(int materialID, PreOrder preOrder)
        {
            return DataAccess.ExecWithStoreProcedureScalar(ConnectionStrings.BelcorpCore, "DelWareHouseMaterialsAllocationsByPreOrder",// uspDelWareHouseMaterialsAllocationsXPreOrder
                 new SqlParameter("PreOrderID", SqlDbType.Int) { Value = preOrder.OrderID },
                 new SqlParameter("MaterialID", SqlDbType.Int) { Value = materialID },
                 new SqlParameter("WarehouseID", SqlDbType.Int) { Value = preOrder.WareHouseID }
                 );
        }

        /// <summary>
        /// Método que elimina WareHouseMaterialsAllocations siempre y cuando sea una Order CREATE BY FHP
        /// </summary>
        /// <param name="materialID">Código del Material</param>
        /// <param name="orderID">Código de la Orden</param>
        /// <returns>Cantidad de filas afectadas</returns>
        public int DelWareHouseMaterialsAllocationByOrder(int orderID, int materialID, int wareHouseID)
        {
            return DataAccess.ExecWithStoreProcedureScalar(ConnectionStrings.BelcorpCore, "DelWareHouseMaterialsAllocationsXPreOrder",//uspDelWareHouseMaterialsAllocationXOrder
                new SqlParameter("OrderID", SqlDbType.Int) { Value = orderID },
                new SqlParameter("MaterialID", SqlDbType.Int) { Value = materialID },
                new SqlParameter("WarehouseID", SqlDbType.Int) { Value = wareHouseID }
                );
        }

        /// <summary>
        /// Developed By KLC - CSTI
        /// Proceso de Actualización de Saldo  (BR-B070)
        /// </summary>
        /// <param name="Material"></param>
        /// <param name="DistributionCenter"></param>
        /// <param name="QuantityOnHand"></param>
        /// <returns></returns>
        public int UpdateSaldo(string Material, string DistributionCenter, decimal QuantityOnHand)
        {
            return DataAccess.ExecWithStoreProcedureSave(ConnectionStrings.BelcorpCore, "UpdateSaldo",
                new SqlParameter("Material", SqlDbType.VarChar) { Value = Material },
                new SqlParameter("DistributionCenter", SqlDbType.VarChar) { Value = DistributionCenter },
                new SqlParameter("QuantityOnHand", SqlDbType.Decimal) { Value = QuantityOnHand }
                );
        } 
    }
}
