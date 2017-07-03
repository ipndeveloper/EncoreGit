using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities.Business.Logic.Interfaces
{
	[ContractClass(typeof(Contracts.OrderShipmentBusinessLogicContracts))]
    public partial interface IOrderShipmentBusinessLogic
    {
        IEnumerable<Order> GetOrderShippingDetails(IEnumerable<string> orderNumbers);
        IPaginatedList<OrderShippingSearchData> Search(IOrderShipmentRepository repository, OrderShipmentSearchParameters searchParameters);
        string GetBaseTrackUrl(int shippingMethodID, string trackingNumber);
        IList<ShippingMethodWithRate> FilterOutExcludedShippingMethods(Order order, IEnumerable<ShippingMethodWithRate> shippingMethods, IProductRepository productRepository, out IList<int> productIdsWithExclusions);
        short CalculateOrderShipmentStatus(OrderShipment orderShipment);
    }

	namespace Contracts
	{
		[ContractClassFor(typeof(IOrderShipmentBusinessLogic))]
		abstract class OrderShipmentBusinessLogicContracts : IOrderShipmentBusinessLogic
		{
			public IEnumerable<Order> GetOrderShippingDetails(IEnumerable<string> orderNumbers)
			{
				throw new NotImplementedException();
			}

			public IPaginatedList<OrderShippingSearchData> Search(IOrderShipmentRepository repository, OrderShipmentSearchParameters searchParameters)
			{
				throw new NotImplementedException();
			}

			public string GetBaseTrackUrl(int shippingMethodID, string trackingNumber)
			{
				throw new NotImplementedException();
			}

			public IList<ShippingMethodWithRate> FilterOutExcludedShippingMethods(Order order, IEnumerable<ShippingMethodWithRate> shippingMethods, IProductRepository productRepository, out IList<int> productIdsWithExclusions)
			{
				throw new NotImplementedException();
			}

			public short CalculateOrderShipmentStatus(OrderShipment orderShipment)
			{
				Contract.Requires<ArgumentNullException>(orderShipment != null);

				throw new NotImplementedException();
			}

			public Func<OrderShipment, int> GetIdColumnFunc
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			public Action<OrderShipment, int> SetIdColumnFunc
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			public Func<OrderShipment, string> GetTitleColumnFunc
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			public Action<OrderShipment, string> SetTitleColumnFunc
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			public void DefaultValues(IOrderShipmentRepository repository, OrderShipment entity)
			{
				throw new NotImplementedException();
			}

			public OrderShipment Load(IOrderShipmentRepository repository, int primaryKey)
			{
				throw new NotImplementedException();
			}

			public OrderShipment LoadFull(IOrderShipmentRepository repository, int primaryKey)
			{
				throw new NotImplementedException();
			}

			public List<OrderShipment> LoadAll(IOrderShipmentRepository repository)
			{
				throw new NotImplementedException();
			}

			public List<OrderShipment> LoadAllFull(IOrderShipmentRepository repository)
			{
				throw new NotImplementedException();
			}

			public void Save(IOrderShipmentRepository repository, OrderShipment entity)
			{
				throw new NotImplementedException();
			}

			public void Delete(IOrderShipmentRepository repository, OrderShipment entity)
			{
				throw new NotImplementedException();
			}

			public void Delete(IOrderShipmentRepository repository, int primaryKey)
			{
				throw new NotImplementedException();
			}

			public PaginatedList<AuditLogRow> GetAuditLog(IOrderShipmentRepository repository, int primaryKey, AuditLogSearchParameters param)
			{
				throw new NotImplementedException();
			}

			public void StartEntityTracking(OrderShipment entity)
			{
				throw new NotImplementedException();
			}

			public void StartEntityTrackingAndEnableLazyLoading(OrderShipment entity)
			{
				throw new NotImplementedException();
			}

			public void StopEntityTracking(OrderShipment entity)
			{
				throw new NotImplementedException();
			}

			public void AcceptChanges(OrderShipment entity, List<IObjectWithChangeTracker> allTrackerItems = null)
			{
				throw new NotImplementedException();
			}

			public void Validate(OrderShipment entity)
			{
				throw new NotImplementedException();
			}

			public bool IsValid(OrderShipment entity)
			{
				throw new NotImplementedException();
			}

			public void AddValidationRules(OrderShipment entity)
			{
				throw new NotImplementedException();
			}

			public List<string> ValidatedChildPropertiesSetByParent(IOrderShipmentRepository repository)
			{
				throw new NotImplementedException();
			}

			public void CleanDataBeforeSave(IOrderShipmentRepository repository, OrderShipment entity)
			{
				throw new NotImplementedException();
			}

			public void UpdateCreatedFields(IOrderShipmentRepository repository, OrderShipment entity)
			{
				throw new NotImplementedException();
			}
		}
	}
}