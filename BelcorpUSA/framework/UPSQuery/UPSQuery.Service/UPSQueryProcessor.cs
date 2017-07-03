using System.Collections.Generic;
using System.Linq;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business.Logic.Interfaces;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Encore.Core.IoC;
using UPSQuery.Common;

namespace UPSQuery.Service
{
	[ContainerRegister(typeof(IUPSQueryProcessor), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
	public class UPSQueryProcessor : IUPSQueryProcessor
	{
		protected IUpsWorldshipOrderRepository UPSOrderRepository { get; private set; }

		public UPSQueryProcessor(IUpsWorldshipOrderRepository upsOrderRepository)
        {
			UPSOrderRepository = upsOrderRepository;
        }

		public IUPSQueryProcessorResult GetUPSOrderShipmentInfo(int orderShipmentID)
		{
			var upsOrderBuisnessLogic = Create.New<IUpsWorldshipOrderBusinessLogic>();
			var orderShipments = upsOrderBuisnessLogic.GetOrderShipmentInfo(UPSOrderRepository, orderShipmentID);
			var results = GetUPSQueryPrcessorResultsFromOrderInfos(orderShipments);
			return results;
		}

		public void SetUPSOrderShippingTrackingNumber(int orderShipmentID, string trackingNumber)
		{
			var upsOrderBuisnessLogic = Create.New<IUpsWorldshipOrderBusinessLogic>();
			upsOrderBuisnessLogic.SetOrderShipmentTrackingNumber(UPSOrderRepository, orderShipmentID, trackingNumber);
		}

		public List<int> GetOrderShipmentIDsToUpdate()
		{
			var upsOrderBusinessLogic = Create.New<IUpsWorldshipOrderBusinessLogic>();
			return upsOrderBusinessLogic.GetOrderShipmentIDsToUpdate(UPSOrderRepository).ToList();
		}

		private IUPSQueryProcessorResult GetUPSQueryPrcessorResultsFromOrderInfos(OrderShipment orderShipment)
		{
			if (orderShipment != null)
			{
				var info = Create.Mutation(Create.New<IUPSQueryProcessorResult>(),
					it =>
					{
						it.Address1 = orderShipment.Address1;
						it.Address2 = orderShipment.Address2;
						it.Address3 = orderShipment.Address3;
						it.City = orderShipment.City;
						it.Country = orderShipment.Country == null ? "" : orderShipment.Country.Name;
						it.CustomerName = string.Format("{0} {1}", orderShipment.FirstName, orderShipment.LastName);
						it.PostalCode = orderShipment.PostalCode;
						it.State = orderShipment.State;
						it.Telephone = orderShipment.DayPhone;
					}
				);

				return info;
			}

			return null;
		}
	}
}
