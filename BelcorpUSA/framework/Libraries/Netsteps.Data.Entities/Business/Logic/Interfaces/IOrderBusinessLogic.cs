using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business.Interfaces;
using NetSteps.Data.Entities.Generated;
using NetSteps.Data.Entities.PaymentGateways;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Data.Common.Entities;
using System.Linq;

namespace NetSteps.Data.Entities.Business.Logic.Interfaces
{
	[ContractClass(typeof(Contracts.OrderBusinessLogicContracts))]
	public partial interface IOrderBusinessLogic
	{
        Order LoadByOrderNumber(Repositories.IOrderRepository repository, string orderNumber);
        Order LoadByOrderNumberFull(Repositories.IOrderRepository repository, string orderNumber);
        void BuildReadOnlyNotesTree(Order account);
        List<IOrder> LoadChildOrders(Repositories.IOrderRepository repository, int parentOrderID, params int[] childOrderTypeIDs);
		List<IOrder> LoadChildOrdersFull(Repositories.IOrderRepository repository, int parentOrderID, params int[] childOrderTypeIDs);
		bool ExistsByOrderNumber(Repositories.IOrderRepository repository, string orderNumber);
		PaginatedList<OrderSearchData> Search(Repositories.IOrderRepository repository, OrderSearchParameters orderSearchParameters);
		int Count(Repositories.IOrderRepository repository, OrderSearchParameters searchParameters);

		string GetPopupMessageForOrderDetail(Repositories.IOrderRepository repository, Order entity);
		PaginatedList<AuditLogRow> GetAuditLog(Repositories.IOrderRepository repository, Order fullyLoadedOrder, AuditLogSearchParameters param);

		Order LoadWithShipmentDetails(IOrderRepository repository, int orderID, bool enableChangeTracking = false);
		List<Order> LoadBatchWithShipmentDetails(IOrderRepository repository, IEnumerable<int> orderIDs, bool enableChangeTracking = false);

		Order LoadOrderWithPaymentDetails(Repositories.IOrderRepository repository, int orderID);
		IEnumerable<Order> LoadOrderWithShipmentAndPaymentDetails(Repositories.IOrderRepository repository, IEnumerable<string> orderNumbers);
        bool ValidateOrderRulesPreCondition(IOrderRepository repository, int? accountId, out string message);
	}
	namespace Contracts
	{
		[ContractClassFor(typeof(IOrderBusinessLogic))]
		internal abstract class OrderBusinessLogicContracts : IOrderBusinessLogic
		{
            Order IOrderBusinessLogic.LoadByOrderNumber(IOrderRepository repository, string orderNumber)
            {
                throw new NotImplementedException();
            }

            Order IOrderBusinessLogic.LoadByOrderNumberFull(IOrderRepository repository, string orderNumber)
            {
                throw new NotImplementedException();
            }

            void IOrderBusinessLogic.BuildReadOnlyNotesTree(Order account)
            {
                throw new NotImplementedException();
            }

            List<IOrder> IOrderBusinessLogic.LoadChildOrders(IOrderRepository repository, int parentOrderID, params int[] childOrderTypeIDs)
            {
                throw new NotImplementedException();
            }

			List<IOrder> IOrderBusinessLogic.LoadChildOrdersFull(IOrderRepository repository, int parentOrderID, params int[] childOrderTypeIDs)
			{
				throw new NotImplementedException();
			}

			bool IOrderBusinessLogic.ExistsByOrderNumber(IOrderRepository repository, string orderNumber)
			{
				throw new NotImplementedException();
			}

			PaginatedList<OrderSearchData> IOrderBusinessLogic.Search(IOrderRepository repository, OrderSearchParameters orderSearchParameters)
			{
				throw new NotImplementedException();
			}

			int IOrderBusinessLogic.Count(IOrderRepository repository, OrderSearchParameters searchParameters)
			{
				throw new NotImplementedException();
			}

			string IOrderBusinessLogic.GetPopupMessageForOrderDetail(IOrderRepository repository, Order entity)
			{
				throw new NotImplementedException();
			}

			PaginatedList<AuditLogRow> IOrderBusinessLogic.GetAuditLog(IOrderRepository repository, Order fullyLoadedOrder, AuditLogSearchParameters param)
			{
				throw new NotImplementedException();
			}

			Order IOrderBusinessLogic.LoadWithShipmentDetails(IOrderRepository repository, int orderID, bool enableChangeTracking)
			{
				throw new NotImplementedException();
			}

			List<Order> IOrderBusinessLogic.LoadBatchWithShipmentDetails(IOrderRepository repository, IEnumerable<int> orderIDs, bool enableChangeTracking)
			{
				throw new NotImplementedException();
			}

			Order IOrderBusinessLogic.LoadOrderWithPaymentDetails(IOrderRepository repository, int orderID)
			{
				throw new NotImplementedException();
			}

			Func<Order, int> Entities.Interfaces.IBusinessEntityLogic<Order, int, IOrderRepository>.GetIdColumnFunc
			{
				get { throw new NotImplementedException(); }
			}

			Action<Order, int> Entities.Interfaces.IBusinessEntityLogic<Order, int, IOrderRepository>.SetIdColumnFunc
			{
				get { throw new NotImplementedException(); }
			}

			Func<Order, string> Entities.Interfaces.IBusinessEntityLogic<Order, int, IOrderRepository>.GetTitleColumnFunc
			{
				get { throw new NotImplementedException(); }
			}

			Action<Order, string> Entities.Interfaces.IBusinessEntityLogic<Order, int, IOrderRepository>.SetTitleColumnFunc
			{
				get { throw new NotImplementedException(); }
			}

			void Entities.Interfaces.IBusinessEntityLogic<Order, int, IOrderRepository>.DefaultValues(IOrderRepository repository, Order entity)
			{
				throw new NotImplementedException();
			}

			Order Entities.Interfaces.IBusinessEntityLogic<Order, int, IOrderRepository>.Load(IOrderRepository repository, int primaryKey)
			{
				throw new NotImplementedException();
			}

			Order Entities.Interfaces.IBusinessEntityLogic<Order, int, IOrderRepository>.LoadFull(IOrderRepository repository, int primaryKey)
			{
				throw new NotImplementedException();
			}

			List<Order> Entities.Interfaces.IBusinessEntityLogic<Order, int, IOrderRepository>.LoadAll(IOrderRepository repository)
			{
				throw new NotImplementedException();
			}

			List<Order> Entities.Interfaces.IBusinessEntityLogic<Order, int, IOrderRepository>.LoadAllFull(IOrderRepository repository)
			{
				throw new NotImplementedException();
			}

			void Entities.Interfaces.IBusinessEntityLogic<Order, int, IOrderRepository>.Save(IOrderRepository repository, Order entity)
			{
				throw new NotImplementedException();
			}

			void Entities.Interfaces.IBusinessEntityLogic<Order, int, IOrderRepository>.Delete(IOrderRepository repository, Order entity)
			{
				throw new NotImplementedException();
			}

			void Entities.Interfaces.IBusinessEntityLogic<Order, int, IOrderRepository>.Delete(IOrderRepository repository, int primaryKey)
			{
				throw new NotImplementedException();
			}

			PaginatedList<AuditLogRow> Entities.Interfaces.IBusinessEntityLogic<Order, int, IOrderRepository>.GetAuditLog(IOrderRepository repository, int primaryKey, AuditLogSearchParameters param)
			{
				throw new NotImplementedException();
			}

			void Entities.Interfaces.IBusinessEntityLogic<Order, int, IOrderRepository>.StartEntityTracking(Order entity)
			{
				throw new NotImplementedException();
			}

			void Entities.Interfaces.IBusinessEntityLogic<Order, int, IOrderRepository>.StartEntityTrackingAndEnableLazyLoading(Order entity)
			{
				throw new NotImplementedException();
			}

			void Entities.Interfaces.IBusinessEntityLogic<Order, int, IOrderRepository>.StopEntityTracking(Order entity)
			{
				throw new NotImplementedException();
			}

			void Entities.Interfaces.IBusinessEntityLogic<Order, int, IOrderRepository>.AcceptChanges(Order entity, List<IObjectWithChangeTracker> allTrackerItems)
			{
				throw new NotImplementedException();
			}

			void Entities.Interfaces.IBusinessEntityLogic<Order, int, IOrderRepository>.Validate(Order entity)
			{
				throw new NotImplementedException();
			}

			bool Entities.Interfaces.IBusinessEntityLogic<Order, int, IOrderRepository>.IsValid(Order entity)
			{
				throw new NotImplementedException();
			}

			void Entities.Interfaces.IBusinessEntityLogic<Order, int, IOrderRepository>.AddValidationRules(Order entity)
			{
				throw new NotImplementedException();
			}

			List<string> Entities.Interfaces.IBusinessEntityLogic<Order, int, IOrderRepository>.ValidatedChildPropertiesSetByParent(IOrderRepository repository)
			{
				throw new NotImplementedException();
			}

			void Entities.Interfaces.IBusinessEntityLogic<Order, int, IOrderRepository>.CleanDataBeforeSave(IOrderRepository repository, Order entity)
			{
				throw new NotImplementedException();
			}

			void Entities.Interfaces.IBusinessEntityLogic<Order, int, IOrderRepository>.UpdateCreatedFields(IOrderRepository repository, Order entity)
			{
				throw new NotImplementedException();
			}

			public bool ShouldDividePartyShipping()
			{
				throw new NotImplementedException();
			}

			IEnumerable<Order> IOrderBusinessLogic.LoadOrderWithShipmentAndPaymentDetails(IOrderRepository repository, IEnumerable<string> orderNumbers)
			{
				throw new NotImplementedException();
			}


            public bool ValidateOrderRulesPreCondition(IOrderRepository repository, int? accountId, out string message)
            {
                throw new NotImplementedException();
            }
        }

	}
}
