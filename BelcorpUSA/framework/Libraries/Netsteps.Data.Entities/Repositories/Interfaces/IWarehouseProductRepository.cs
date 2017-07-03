using System.Collections.Generic;
using NetSteps.Addresses.Common.Models;

namespace NetSteps.Data.Entities.Repositories
{
    public partial interface IWarehouseProductRepository
    {
        List<WarehouseProduct> GetFullInventory();
        List<WarehouseProduct> GetInventoryForWarehouse(int warehouseID);
        WarehouseProduct GetWarehouseProduct(int warehouseID, int productID);
        WarehouseProduct GetWarehouseProduct(IAddress address, int productID);
    }
}
