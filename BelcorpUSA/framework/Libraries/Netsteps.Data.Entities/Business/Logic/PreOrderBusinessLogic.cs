using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Repositories.Interfaces;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities.Business.Logic
{
    /// <summary>
    /// Clase Businnes Logic de PreOrder
    /// </summary>
    public class PreOrderBusinessLogic
    {
        /// <summary>
        /// Constructor que inicializa la clase PreOrder
        /// </summary>
        public PreOrderBusinessLogic()
        { 
        }

        /// <summary>
        /// Instancia de la clase PreOrderBusinessLogic
        /// </summary>
        private static PreOrderBusinessLogic instance;
        /// <summary>
        /// Repositorio de la interface  IPreOrder
        /// </summary>
        private static IPreOrder repositoryPreOrder;

        /// <summary>
        /// Instancia de la clase PreOrderBusinessLogic 
        /// </summary>
        public static PreOrderBusinessLogic Instance
        {
            get 
            {
                if (instance == null)
                {
                    instance = new PreOrderBusinessLogic();
                    repositoryPreOrder = new PreOrderRepository();
                }
                return instance;
            }
        }

        /// <summary>
        /// Método genera un nuevo registro a la tabla PreOrders Create By - FHP
        /// </summary>
        /// <param name="accountID">Código del Account</param>
        /// <param name="siteID">Código del Site</param>
        /// <returns>Cantidad de filas afectadas</returns>
        public int CreatePreOrder(int accountID, int siteID)
        {
            try
            {
                return repositoryPreOrder.CreatePreOrder(accountID, siteID);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        /// <summary>
        /// Método que obtiene el credito de una Account CREATE BY - FHP
        /// </summary>
        /// <param name="accountID">Código del Account</param>
        /// <returns>Credito disponible</returns>
        public decimal GetCreditLedgerByAccountID(int accountID)
        {
            try
            {
                return repositoryPreOrder.GetCreditLedgerByAccountID(accountID);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
            
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
            try
            {
                return repositoryPreOrder.InsWarehouseMaterialAllocationOrder(materialID, wareHouseID, quantity, productID, orderID);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        /// <summary>
        /// Método que valida la disponibilidad del producto solicitado CREATE BY FHP 
        /// </summary>
        /// <param name="productoID">Código del producto a consultar</param>
        /// <param name="wareHouseID">Código del WareHouse a consultar</param>
        /// <returns>Lista con cantidades disponibles con el su respectivo Material</returns>
        public List<InventoryCheck> InventoryCheckResult(int productoID, int wareHouseID)
        {
            try
            {
                return repositoryPreOrder.InventoryCheckResult(productoID, wareHouseID);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        /// <summary>
        /// Método que valida si el producto es un KIT CREATE BY FHP
        /// </summary>
        /// <param name="productoID">Código del producto a consultar</param>
        /// <returns>El si el producto es un KIT</returns>
        public bool IsKitProduct(int productoID)
        {
            try
            {
                return repositoryPreOrder.IsKitProduct(productoID);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        /// <summary>
        /// Método que retorna la existencia de productos CREATE BY FHP
        /// </summary>
        /// <param name="k">Variable que inicializa</param>
        /// <param name="productID">Código del producto a consultar</param>
        /// <param name="materialID">Código del material a consultar</param>
        /// <returns>Objeto de Replacement</returns>
        public List<Replacement> ReplacementResult(int k, int productID, int materialID)
        {
            try
            {
                return repositoryPreOrder.ReplacementResult(k, productID, materialID);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }


        /// <summary>
        /// Método que retorna la lista IncludeInventoryCheck CREATE BY - FHP
        /// </summary>
        /// <param name="materialID">Código Material</param>
        /// <param name="wareHouseID">Código WareHouse</param>
        /// <returns>Lista IncludeInventoryCheck</returns>
        public List<IncludeInventoryCheck> IncludeInventoryCheckResult(int materialID, int wareHouseID)
        {
            try
            {
                return repositoryPreOrder.IncludeInventoryCheckResult(materialID, wareHouseID);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        /// <summary>
        /// Método que retorna el credito del producto CREATE BY - FHP
        /// </summary>
        /// <param name="accountID">Código de la cuenta</param>
        /// <returns>Retorna credito del producto</returns>
        public decimal GetProductCreditByAccount(int accountID)
        {
            try
            {
                return repositoryPreOrder.GetProductCreditByAccount(accountID);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }


        /// <summary>
        /// Método que actualiza una preOrder al Create By - FHP
        /// </summary>
        /// <param name="preOrderID">Código de la PreOrder</param>
        /// <param name="orderId">Código de Orden</param>
        /// <returns>Retorna el número de filas afectadas </returns>
        public int UpdatePreOrder(int preOrderID, int orderId)
        {
            try
            {
                return repositoryPreOrder.UpdatePreOrder(preOrderID, orderId);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        /// <summary>
        /// Método que lista las PreOrdenes por Número de Ordenes Create By - FHP
        /// </summary>
        /// <param name="orderNumber"></param>
        /// <returns></returns>
        public List<PreOrder> GetPreOrderByOrderNumber(string orderNumber)
        {
            try
            {
                return repositoryPreOrder.GetPreOrderByOrderNumber(orderNumber);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        /// <summary>
        /// Método que lista los Kit Replacement CREATE BY - FHP
        /// </summary>
        /// <param name="k">Valor entero</param>
        /// <param name="ProductID">Código del Producto</param>
        /// <param name="MaterialID">Código del Material</param>
        /// <returns>Lista de los CheckKitReplacement</returns>
        public List<CheckKitReplacement> CheckKitReplacementResult(int k, int ProductID, int MaterialID)
        {
            try
            {
                return repositoryPreOrder.CheckKitReplacementResult(k, ProductID, MaterialID);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        /// <summary>
        /// Método que lista los Kit Inventory CREATE BY - FHP
        /// </summary>
        /// <param name="MaterialID">Código Material</param>
        /// <returns>Lista de los CheckKitInventory</returns>
        public List<CheckKitInventory> CheckKitInventoryResult(int MaterialID, int wareHouseId)
        {
            try
            {
                return repositoryPreOrder.CheckKitInventoryResult(MaterialID, wareHouseId);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        /// <summary>
        /// Método que lista los ProductosMaterials  CREATE BY - FHP
        /// </summary>
        /// <param name="ProductoSolicitadoID">Código del Producto</param>
        /// <returns>Lista de los ProductMaterials</returns>
        public List<ProductMaterial> ProductMaterials(int ProductoSolicitadoID,int WarehouseID)
        {
            try
            {
                return repositoryPreOrder.ProductMaterials(ProductoSolicitadoID, WarehouseID);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }


        /// <summary>
        /// Método que retorna la lista de InventoryCheckDetail CREATE BY - FHP
        /// </summary>
        /// <param name="MaterialID">Código del Material</param>
        /// <returns>Lista InventoryCheckDetail</returns>
        public List<InventoryCheckDetail> InventoryCheckDetails(int MaterialID,int wareHouseID)
        {
            try
            {
                return repositoryPreOrder.InventoryCheckDetails(MaterialID, wareHouseID);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }


        /// <summary>
        /// Método que retorna lista de los Kits CREATE BY - FHP
        /// </summary>
        /// <param name="ProductoSolicitadoID">Código del Product</param>
        /// <returns>Retorna lista de los Kits</returns>
        public List<ConfigKit> ConfigKits(int ProductoSolicitadoID, int wareHouseID)
        {
            try
            {
                return repositoryPreOrder.ConfigKits(ProductoSolicitadoID, wareHouseID);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        /// <summary>
        /// Método que retorna si es el ProductMaterial esta activo CREATE BY - FHP
        /// </summary>
        /// <param name="ProductID">Código del Producto</param>
        /// <returns>Valor del campo si es activo</returns>
        public int IsActiveProductMaterial(int ProductID)
        {
            try
            {
                return repositoryPreOrder.IsActiveProductMaterial(ProductID);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        /// <summary>
        /// Método que retorna el estado de la orden  CREATE BY - FHP
        /// </summary>
        /// <param name="accountID">Código del Account</param>
        /// <returns>Estado de la orden</returns>
        public List<OrdersStatus> GetStatusOrder(int? accountID)
        {
            try
            {
                return repositoryPreOrder.GetStatusOrder(accountID);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }
        /// <summary>
        /// Método que asigna las PreOrdenes a una Orden
        /// </summary>
        /// <param name="newOrderID">Código de la Orden</param>
        /// <param name="preOrderId">Código de la PreOrder</param>
        /// <returns>Cantidad de filas afectadas</returns>
        public int AsignarReservasAOrden(int newOrderID, int preOrderId)
        {
            try
            {
                return repositoryPreOrder.AsignarReservasAOrden(newOrderID, preOrderId);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

    }
}
