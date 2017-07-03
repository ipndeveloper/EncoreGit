using System.Collections.Generic;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities.Business.Logic.Interfaces
{
    public partial interface IUpsWorldshipOrderBusinessLogic
    {
        OrderShipment GetOrderShipmentInfo(IUpsWorldshipOrderRepository repository, int orderShipmentID);
        void SetOrderShipmentTrackingNumber(IUpsWorldshipOrderRepository repository, int orderShipmentID, string trackingNumber);
        IEnumerable<int> GetOrderShipmentIDsToUpdate(IUpsWorldshipOrderRepository repo);
    }
}
