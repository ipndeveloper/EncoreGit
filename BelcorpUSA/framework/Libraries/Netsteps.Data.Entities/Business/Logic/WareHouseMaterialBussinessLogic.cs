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
    /// Logica del Negocio de la clase WareHouseMaterial
    /// </summary>
    public class WareHouseMaterialBussinessLogic
    {
        /// <summary>
        /// Constructor de la clase WareHouseMaterial
        /// </summary>
        private WareHouseMaterialBussinessLogic()
        { 
        }

        /// <summary>
        /// Instancia de la clase WareHouseMaterial
        /// </summary>
        private static WareHouseMaterialBussinessLogic instance;
        /// <summary>
        /// Instancia de la interface repositoryWareHouseMaterial
        /// </summary>
        private static IWareHouseMaterialsRepository repositoryWareHouseMaterial;

        /// <summary>
        /// Instancia de la clase WareHouseMaterial
        /// </summary>
        public static WareHouseMaterialBussinessLogic Instance
        {
            get 
            {
                if (instance == null)
                {
                    instance = new WareHouseMaterialBussinessLogic();
                    repositoryWareHouseMaterial = new WareHouseMaterialsRepository();
                }
                return instance;
            }
        }
        /// <summary>
        /// Permite insertar Datos a la tabla WarehouseMaterialAllocations Create By - FHP
        /// </summary>
        /// <param name="quantity">Cantindad de Productos</param>
        /// <param name="productID">ProductoId a Ingresar</param>
        /// <param name="materialId">Material Id a Ingresar</param>
        /// <returns>Cantidad de filas afectadas</returns>
        public int InsWarehouseMaterialAllocationsOrder(int quantity, int productID, int materialId, int preOrderId, int WareHouseID, int PreOrderId)
        {
            try
            {
                return repositoryWareHouseMaterial.InsWarehouseMaterialAllocationsOrder(quantity, productID, materialId, preOrderId, WareHouseID, PreOrderId);

            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
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

            try
            {
                return repositoryWareHouseMaterial.UpdWareHouseMaterials(materialID, quantity, wareHouseID);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        /// <summary>
        /// Método que registra en la tabla wareHouseMaterials en base al OrderType = PreOrder CREATE BY - FHP
        /// </summary>
        /// <param name="materialID">Código del Material</param> 
        /// <param name="quantity">Cantidad de registrada por producto en la orden</param> 
        /// <returns>Cantidad de filas afectadas</returns>
        public int UpdWarehouseMaterial(int materialID, int quantity, PreOrder preOrder)
        {
            try
            {
                return repositoryWareHouseMaterial.UpdWarehouseMaterial(materialID, quantity, preOrder);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        /// <summary>
        /// Método que retorna el Código del WareHouse CREATE BY - FHP
        /// </summary>
        /// <param name="accountID">Código del Account</param>
        /// <param name="addressID">Código del Address</param>
        /// <returns>Código del WareHouse</returns>
        public int WareHouseByAccountAddress(int accountID, int addressID)
        {
            try
            {
                return repositoryWareHouseMaterial.WareHouseByAccountAddress(accountID, addressID);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
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
        public int InsWareHouseMaterialAllocationPreOrder(int materialID, int wareHouseID, int quantity, int productID, int orderID, int QuantityProduct, bool isClaims)
        {
            try
            {
                return repositoryWareHouseMaterial.InsWareHouseMaterialAllocationPreOrder(materialID, wareHouseID, quantity, productID, orderID, isClaims);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        /// <summary>
        /// Método que elimina WareHouseMaterialsAllocations siempre y cuando sea una PreOrder CREATE BY FHP
        /// </summary>
        /// <param name="materialID">Código del Material</param>
        /// <returns>Cantidad de filas afectadas</returns>
        public int DelWareHouseMaterialsAllocationsByPreOrder(int materialID,PreOrder preOrder)
        {
            try
            {
                return repositoryWareHouseMaterial.DelWareHouseMaterialsAllocationsByPreOrder(materialID, preOrder);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }


        /// <summary>
        /// Método que elimina WareHouseMaterialsAllocations siempre y cuando sea una Order CREATE BY FHP
        /// </summary>
        /// <param name="materialID">Código del Material</param>
        /// <param name="orderID">Código de la Orden</param>
        /// <returns>Cantidad de filas afectadas</returns>
        public int DelWareHouseMaterialsAllocationByOrder(int orderID, int materialID, int wareHouseID)
        {
            try
            {
                return repositoryWareHouseMaterial.DelWareHouseMaterialsAllocationByOrder(orderID, materialID, wareHouseID);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        /// <summary>
        /// Developed By KLC - CSTI
        /// Actualizacion de Saldo (BR-B070)
        /// </summary>
        /// <param name="Material"></param>
        /// <param name="DistributionCenter"></param>
        /// <param name="QuantityOnHand"></param>
        /// <returns></returns>
        public int UpdateSaldo(string Material, string DistributionCenter, decimal QuantityOnHand) 
        {
            try
            {
                return repositoryWareHouseMaterial.UpdateSaldo(Material, DistributionCenter, QuantityOnHand);
            }
            catch (Exception ex)
            {                
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        
        }


    }
}
