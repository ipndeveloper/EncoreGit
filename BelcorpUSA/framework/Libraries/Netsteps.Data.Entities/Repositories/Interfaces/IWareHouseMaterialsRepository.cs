using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Business;

namespace NetSteps.Data.Entities.Repositories.Interfaces
{
    /// <summary>
    /// Interface que representa WareHouseMaterialsRepository Create By - FHP
    /// </summary>
    public interface IWareHouseMaterialsRepository
    {
        int InsWarehouseMaterialAllocationsOrder(int quantity, int productID, int materialId, int preOrderId, int WareHouseID, int PreOrderId);
        int UpdWareHouseMaterials(int materialID, int quantity, int wareHouseID);
        int InsWareHouseMaterialAllocationPreOrder(int materialID, int wareHouseID, int quantity, int productID, int orderID, bool isClaims);
        int DelWareHouseMaterialsAllocationsByPreOrder(int materialID, PreOrder preOrder);
        int UpdWarehouseMaterial(int materialID, int quantity, PreOrder preOrder);
        int DelWareHouseMaterialsAllocationByOrder(int orderID, int materialID, int wareHouseID);
        int WareHouseByAccountAddress(int accountID, int addressID);

        /// <summary>
        /// Developed By KLC - CSTI
        /// Proceso de Actualización de Saldo  (BR-B070)
        /// </summary>
        /// <param name="Material"></param>
        /// <param name="DistributionCenter"></param>
        /// <param name="QuantityOnHand"></param>
        /// <returns></returns>
        int UpdateSaldo(string Material, string DistributionCenter, decimal QuantityOnHand);
    }
}
