using System.Collections.Generic;

namespace NetSteps.Data.Entities.Repositories
{
    public partial interface IUpsWorldshipOrderRepository
    {
        OrderShipment GetOrderShipmentInfo(int orderShipmentID);
        IEnumerable<OrderShipment> GetOrderShipmentsToUpdate();
        OrderShipment GetOrderShipmentWithOrder(int orderShipmentID);
    }
}
