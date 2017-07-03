using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Exceptions;
using NetSteps.Data.Entities.Business.Logic.Interfaces;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Data.Entities.Business.Logic
{
	[ContainerRegister(typeof(IUpsWorldshipOrderBusinessLogic), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class UpsWorldshipOrderBusinessLogic : IUpsWorldshipOrderBusinessLogic
    {
        public OrderShipment GetOrderShipmentInfo(IUpsWorldshipOrderRepository repository, int orderShipmentID)
        {
            return repository.GetOrderShipmentInfo(orderShipmentID);
        }

        public void SetOrderShipmentTrackingNumber(IUpsWorldshipOrderRepository repository, int orderShipmentID, string trackingNumber)
        {
            OrderShipment os = repository.GetOrderShipmentWithOrder(orderShipmentID);
            if (os != null)
            {
                os.StartTracking();
                os.TrackingNumber = trackingNumber;
                os.DateShipped = DateTime.UtcNow.Date;
                os.OrderShipmentStatusID = (int)Constants.OrderShipmentStatus.Shipped;
                os.Save();

                bool allShipped = os.Order.OrderShipments.All(s => s.DateShipped.HasValue);
                os.Order.OrderStatusID = allShipped ? (short)Constants.OrderStatus.Shipped : (short)Constants.OrderStatus.PartiallyShipped;
                os.Order.Save();
            }
            else
            {
                throw new NetStepsDataException("Order Shipment not found for orderShipmentID: " + orderShipmentID.ToString());
            }
        }

        public IEnumerable<int> GetOrderShipmentIDsToUpdate(IUpsWorldshipOrderRepository repository)
        {
            return repository.GetOrderShipmentsToUpdate().Select(os => os.OrderShipmentID);   
        }
    }
}
