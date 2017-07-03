using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Business;

namespace NetSteps.Data.Entities.Repositories.Interfaces
{
    /// <summary>
    /// Interface que implementan los métodos de PreOrdenes Create by FHP
    /// </summary>
    public interface IPreOrder
    {
        int CreatePreOrder(int accountID, int siteID);
        //int CreatePreOrder(PreOrder preOrderModel); 
        decimal GetCreditLedgerByAccountID(int accountID);
        int InsWarehouseMaterialAllocationOrder(int materialID, int wareHouseID, int quantity, int productID, int orderID);
        List<InventoryCheck> InventoryCheckResult(int productoID, int wareHouseID);
        bool IsKitProduct(int productoID);
        List<Replacement> ReplacementResult(int k, int productID, int materialID);
        List<IncludeInventoryCheck> IncludeInventoryCheckResult(int materialID, int wareHouseID);
        decimal GetProductCreditByAccount(int accountID);
        int UpdatePreOrder(int preOrderID, int orderId); 
        List<PreOrder> GetPreOrderByOrderNumber(string orderNumber);
        List<CheckKitReplacement> CheckKitReplacementResult(int k, int ProductID, int MaterialID);
        List<CheckKitInventory> CheckKitInventoryResult(int MaterialID, int wareHouseID);
        List<ProductMaterial> ProductMaterials(int ProductoSolicitadoID, int WarehouseID);
        List<InventoryCheckDetail> InventoryCheckDetails(int MaterialID, int wareHouseID);
        List<ConfigKit> ConfigKits(int ProductoSolicitadoID, int wareHouseID);
        int IsActiveProductMaterial(int ProductID);
        List<OrdersStatus> GetStatusOrder(int? accountID);
        int AsignarReservasAOrden(int newOrderID, int preOrderId);

    }


}
