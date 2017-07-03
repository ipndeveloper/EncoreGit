using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Data.Common.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Generated;
using NetSteps.Data.Entities.Interfaces;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Encore.Core.IoC;
using NetSteps.OrderAdjustments.Common;
using NetSteps.Promotions.Common;

namespace NetSteps.Data.Entities.Business.Logic
{
	public partial class OrderBusinessLogic
	{
		public InventoryBaseRepository Inventory { get { return Create.New<InventoryBaseRepository>(); } }
		public ITotalsCalculator TotalsCalculator { get { return Create.New<ITotalsCalculator>(); } }
		public IShippingCalculator ShippingCalculator { get { return Create.New<IShippingCalculator>(); } }
		public IProductRepository ProductRepository { get { return Create.New<IProductRepository>(); } }
		public IOrderRepository OrderRepository { get { return Create.New<IOrderRepository>(); } }
		public IOrderAdjustmentService OrderAdjustmentService { get { return Create.New<IOrderAdjustmentService>(); } }
		public IOrderItemRepository OrderItemRepository { get { return Create.New<IOrderItemRepository>(); } }
		public IOrderAdjustmentHandler OrderAdjustmentHandler { get { return Create.New<IOrderAdjustmentHandler>(); } }
		public IPromotionService PromotionService { get { return Create.New<IPromotionService>(); } }

		public virtual void BuildReadOnlyNotesTree(Order order)
		{
			// Hookup read-only tree of notes - JHE
			foreach (var note in order.Notes.ToList())
				note.FollowupNotes = order.Notes.Where(n => n.ParentID == note.NoteID).ToList();
		}

		public virtual string GetPopupMessageForOrderDetail(Repositories.IOrderRepository repository, Order entity)
		{
			// This is allows a business an opportunity to have a popup message appear in the nsCore when an order is pulled up in 'detail' view - JHE

			return string.Empty;
		}

		public virtual PaginatedList<AuditLogRow> GetAuditLog(Repositories.IOrderRepository repository, Order fullyLoadedOrder, AuditLogSearchParameters param)
		{
			try
			{
				return repository.GetAuditLog(fullyLoadedOrder, param);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException, (fullyLoadedOrder != null) ? fullyLoadedOrder.OrderID : fullyLoadedOrder.OrderID.ToIntNullable(), null);
			}
		}

		public virtual Order LoadByOrderNumberFull(Repositories.IOrderRepository repository, string orderNumber)
		{
			try
			{
				var order = repository.LoadByOrderNumberFull(orderNumber);
				if (order != null)
				{
					order.StartEntityTracking();
					order.EnableLazyLoadingRecursive();
				}

				BuildReadOnlyNotesTree(order);

				return order;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public virtual Order LoadByOrderNumber(Repositories.IOrderRepository repository, string orderNumber)
		{
			try
			{
				var order = repository.LoadByOrderNumber(orderNumber);
				if (order != null)
				{
					order.StartEntityTracking();
					order.EnableLazyLoadingRecursive();
				}
				return order;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public virtual bool ExistsByOrderNumber(Repositories.IOrderRepository repository, string orderNumber)
		{
			try
			{
				return repository.ExistsByOrderNumber(orderNumber);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public virtual PaginatedList<OrderSearchData> Search(Repositories.IOrderRepository repository, OrderSearchParameters searchParameters)
		{
			try
			{
				return repository.Search(searchParameters);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public virtual int Count(Repositories.IOrderRepository repository, OrderSearchParameters searchParameters)
		{
			try
			{
				return repository.Count(searchParameters);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public virtual List<IOrder> LoadChildOrders(Repositories.IOrderRepository repository, int parentOrderID, params int[] childOrderTypeIDs)
		{
			try
			{
				var list = repository.LoadChildOrders(parentOrderID, childOrderTypeIDs).Select(o => (IOrder)o).ToList<IOrder>();
				list.Each(item =>
		{
			item.AsOrder().StartEntityTracking();
			item.AsOrder().IsLazyLoadingEnabled = true;
		});
				return list;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public virtual List<IOrder> LoadChildOrdersFull(Repositories.IOrderRepository repository, int parentOrderID, params int[] childOrderTypeIDs)
		{
			try
			{
				var list = repository.LoadChildOrdersFull(parentOrderID, childOrderTypeIDs).Select(o => (IOrder)o).ToList<IOrder>();
				list.Each(item =>
		{
			item.AsOrder().StartEntityTracking();
			item.AsOrder().IsLazyLoadingEnabled = true;
		});
				return list;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public override Order LoadFull(Repositories.IOrderRepository repository, int primaryKey)
		{
			var order = base.LoadFull(repository, primaryKey);
			if (order != null)
			{
				order.StartEntityTracking();
				order.EnableLazyLoadingRecursive();
			}

			BuildReadOnlyNotesTree(order);

			return order;
		}

		public virtual Order LoadWithShipmentDetails(IOrderRepository repository, int orderID, bool enableChangeTracking = false)
		{
			var order = repository.LoadWithShipmentDetails(orderID);
			if (enableChangeTracking)
			{
				order.StartEntityTracking();
			}

			return order;
		}

		public virtual List<Order> LoadBatchWithShipmentDetails(IOrderRepository repository, IEnumerable<int> orderIDs, bool enableChangeTracking = false)
		{
			var orders = repository.LoadBatchWithShipmentDetails(orderIDs);
			if (enableChangeTracking)
			{
				orders.ForEach(x => x.StartEntityTracking());
			}

			return orders;
		}

		public virtual IEnumerable<Order> LoadOrderWithShipmentAndPaymentDetails(IOrderRepository repository, IEnumerable<string> orderNumbers)
		{
			try
			{
				var orders = repository.LoadOrderWithShipmentAndPaymentDetails(orderNumbers);

				return orders;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

	    public bool ValidateOrderRulesPreCondition(IOrderRepository repository, int? accountId, out string message)
	    {
	        try
	        {
	            var isValid = repository.ValidateOrderRulesPreCondition(repository, accountId, out message);
	            return isValid;
	        }
	        catch (Exception ex)
	        {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
	        }
	    }

	    public virtual Order LoadOrderWithPaymentDetails(Repositories.IOrderRepository repository, int orderID)
		{
			try
			{
				var order = repository.LoadOrderWithPaymentDetails(orderID);
				return order;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public override void DefaultValues(IOrderRepository repository, Order order)
		{
			order.OrderNumber = String.Empty;
			order.OrderTypeID = ConstantsGenerated.OrderType.OnlineOrder.ToShort();
			order.OrderStatusID = ConstantsGenerated.OrderStatus.Pending.ToShort();
			order.OrderPendingState = Constants.OrderPendingStates.Open;
			order.DateCreated = DateTime.Now;
			order.CompleteDate = null;
		}

		public override void AddValidationRules(Order order)
		{
			// TODO: Finish this - JHE
		}

		public override void CleanDataBeforeSave(IOrderRepository repository, Order entity)
		{
			if (entity != null && entity.OrderCustomers != null)
			{
				foreach (var orderCustomer in entity.OrderCustomers)
				{
					if (orderCustomer.OrderCustomerID < 0)
						orderCustomer.OrderCustomerID = 0;
					foreach (var orderShipment in orderCustomer.OrderShipments)
						orderShipment.Trim();
					if (orderCustomer.OrderPayments != null)
					{
						foreach (var orderPayment in orderCustomer.OrderPayments)
						{
							orderPayment.AccountNumberLastFour = orderPayment.DecryptedAccountNumber.Length > 4
								? orderPayment.DecryptedAccountNumber.SubstringSafe(orderPayment.DecryptedAccountNumber.Length - 4, 4)
								: orderPayment.DecryptedAccountNumber;
						}
					}
				}
			}
		}

		public override List<string> ValidatedChildPropertiesSetByParent(Repositories.IOrderRepository repository)
		{
			return new List<string>
			{
				"OrderID",
				"OrderCustomerID",
				"OrderItemID",
				"OrderAdjustmentID",
				"OrderShipmentID",
				"OrderShipmentPackageID",
				"OrderShipmentPackageItemID",
				"OrderItemMessageID",
			};
		}

        #region BG singleton
        public static OrderBusinessLogic Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new OrderBusinessLogic();
                    repositoryExtended = new NetSteps.Data.Entities.Repositories.OrderRepository();
                }

                return instance;
            }
        }

        private static OrderBusinessLogic instance;

        private static IOrderRepository repositoryExtended;

        /// <summary>
        /// Obtiene el ticket
        /// </summary>
        /// <param name="orderId">Id de Orden</param>
        /// <returns>Id de la orden</returns>
        public int? IDSupportTicketByOrder(int orderId)
        {
            return repositoryExtended.IDSupportTicketByOrder(orderId);
        }

        /// <summary>
        /// Obtiene el Id de la order por filtros
        /// </summary>
        /// <param name="number">Numero de Orden</param>
        /// <param name="idSupport">Id de Ticket de Soporte</param>
        /// <returns>Id de la orden</returns>
        public int OrderIdByFilters(string number, int idSupport)
        {
            return repositoryExtended.OrderIdByFilters(number, idSupport);
        }

        /// <summary>
        /// Enable opcion cancel on Order Detail View
        /// </summary>
        /// <param name="orderTypeId"></param>
        /// <param name="orderStatusId"></param>
        /// <returns></returns>
        public static bool IsCancelPossible(int orderTypeId, int orderStatusId)
        {
            bool result = false;

            //Si no es una orden de retorno
            if (orderTypeId != Constants.OrderType.ReturnOrder.ToInt())
            {
                switch ((Constants.OrderStatus)orderStatusId)
                {
                    case ConstantsGenerated.OrderStatus.PartiallyPaid:
                    case ConstantsGenerated.OrderStatus.Pending:
                    case ConstantsGenerated.OrderStatus.PendingPerPaidConfirmation:
                        result = true;
                        break;
                }
            }
            else if (orderTypeId == Constants.OrderType.ReturnOrder.ToInt()) //Original logic
            {
                switch ((Constants.OrderStatus)orderStatusId)
                {
                    case ConstantsGenerated.OrderStatus.Pending:
                        result = true;
                        break;
                }
            }

            return result;
        }

        /// <summary>
        /// Enable opcion Cancel Paid Order option on detail View
        /// </summary>
        /// <param name="orderTypeId"></param>
        /// <param name="orderStatusId"></param>
        /// <returns></returns>
        public static bool IsCancelPaidOrderPossible(int orderTypeId, int orderStatusId)
        {
            bool result = false;

            //Si no es una orden de retorno
            if (orderTypeId != Constants.OrderType.ReturnOrder.ToInt())
            {
                switch ((Constants.OrderStatus)orderStatusId)
                {
                    case ConstantsGenerated.OrderStatus.Paid:
                        result = true;
                        break;
                }
            }
            else if (orderTypeId == Constants.OrderType.ReturnOrder.ToInt())//Original
            {
                switch ((Constants.OrderStatus)orderStatusId)
                {
                    case ConstantsGenerated.OrderStatus.Pending:
                        result = true;
                        break;
                }
            }

            return result;
        }


        /// <summary>
        /// Define acciones luego de cancelar o retornar una orden
        /// </summary>
        /// <param name="orderStatusID">Id de la orden</param>
        /// <param name="originalOrderWasCancelled">Determina si la orden fue cancelada</param>
        /// <param name="allowClearAllocation">Permite ejecutar ClearAllocation</param>
        /// <param name="allowApplyCredit">Permite ApplyCredit</param>
        /// <param name="allowSAP">Permite generar interfaz SAP</param>
        /// <param name="allowReverseStatusActivity">Permite ReverStatusActivity</param>
        public static void ActionOnReturnOrCancelOrder(short orderStatusID,
            ref bool originalOrderWasCancelled,
            ref bool allowClearAllocation,
            ref bool allowApplyCredit,
            ref bool allowSAP,
            ref bool allowReverseStatusActivity)
        {
            switch ((Constants.OrderStatus)orderStatusID)
            {
                case Constants.OrderStatus.CancelledPaid:
                    originalOrderWasCancelled = true;
                    allowClearAllocation = true;
                    allowSAP = true;
                    allowReverseStatusActivity = true;
                    allowApplyCredit = true;
                    break;
                case Constants.OrderStatus.Pending:
                    allowClearAllocation = true;
                    break;
                case Constants.OrderStatus.PartiallyPaid:
                    allowClearAllocation = true;
                    allowApplyCredit = true;
                    break;
                case Constants.OrderStatus.Paid:
                    allowClearAllocation = true;
                    allowSAP = true;
                    allowReverseStatusActivity = true;
                    allowApplyCredit = true;
                    break;
                case Constants.OrderStatus.Printed:
                    allowClearAllocation = true;
                    allowSAP = true;
                    allowReverseStatusActivity = true;
                    allowApplyCredit = true;
                    break;
                case Constants.OrderStatus.Invoiced:
                    allowReverseStatusActivity = true;
                    allowApplyCredit = true;
                    break;
                case Constants.OrderStatus.Shipped:
                    allowApplyCredit = true;
                    allowReverseStatusActivity = true;
                    break;
                case Constants.OrderStatus.Delivered:
                    allowReverseStatusActivity = true;
                    allowApplyCredit = true;
                    break;
                case Constants.OrderStatus.Embarked:
                    allowReverseStatusActivity = true;
                    allowApplyCredit = true;
                    break;
                case Constants.OrderStatus.PendingPerPaidConfirmation:
                    allowClearAllocation = true;
                    break;
            }
        }


        /// <summary>
        /// update Account Datos
        /// </summary>
        /// <param name="accountID">Account ID</param>
        /// <param name="orderId">Order Id</param>
        /// <param name="periodID">Period ID</param>
        public int ReverseStatusActivity(int accountID, int orderId, int periodID)
        {
            int status = 0;
            var currentActivity = ActivityBusinessLogic.Instance.GetByFilters(accountID, periodID);
            if (currentActivity == null)
                return status;

            long activityId = currentActivity.ActivityID;
            int ordersInPeriod = ActivityBusinessLogic.Instance.ActivitiesInPeriodLessCurrent(accountID, periodID, activityId);
            if (ordersInPeriod == 0)
            {
                int previousPeriod = PeriodBusinessLogic.Instance.GetPreviousPeriod(periodID, 1);
                var previousActivity = ActivityBusinessLogic.Instance.GetByFilters(accountID, previousPeriod);

                if (previousActivity == null)
                {
                    ActivityBusinessLogic.Instance.Delete(activityId);
                }
                else
                {
                    short statusActivityIdLast = previousActivity.ActivityStatusID;
                    currentActivity.ActivityStatusID = statusActivityIdLast;
                    ActivityBusinessLogic.Instance.Update(currentActivity);
                }

                status = 1;
            }
            else
            {
                status = 0;
            }

            return status;
        }

        /// <summary>
        /// Obtiene las ordenes en el periodo para cada accountId que tengas mas de 1
        /// </summary>
        /// <param name="periodId"></param>
        /// <returns></returns>
        public IEnumerable<Order> GetOrdersInPeriod(int periodId)
        {
            return from r in repositoryExtended.OrdersInPeriod(periodId)
                       select DtoToBo(r);
        }

        Order DtoToBo(NetSteps.Data.Entities.Dto.OrderDto dto)
        {
            if (dto == null)
                return null;

            return new Order() 
            {
                OrderID = dto.OrderID,
                TmpRetailTotal = dto.RetailTotal,
                TmpAccountID = dto.AccountID,
                TmpOrderCustomer = dto.OrderCustomer
            }; 
        }
        #endregion

        public Tuple<int, int, int> CheckOrdersByAccountID(int AccountID)
        {
            try
            {
                return new OrderRepository().CheckOrdersByAccountID(AccountID);
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public decimal GetItemPriceByIdAndType(int OrderItemID, int ProductPriceTypeID)
        {
            return new OrderRepository().GetItemPriceByIdAndType(OrderItemID, ProductPriceTypeID);
        }

        public PaymentConfigurations GetPaymentConfigurationByOrderID(int OrderID)
        {
            return new PaymentConfigurationsRepository().GetPaymentConfigurationByOrderID(OrderID);
        }

        public Tuple<string, string, string, string> GetOrderCompleteDateDB(int OrderID)
        {
            return new OrderRepository().GetOrderCompleteDateDB(OrderID);
        }

        /*CS.20AGO2016.Inicio*/
        public string GetInvoiceNumberByOrderID(int orderID)
        {
            OrderRepository repository = new OrderRepository();
            return repository.GetInvoiceNumberByOrderID(orderID);
        }

        public string GetOrderNumberByInvoiceNumber(string InvoiceNumber)
        {
            OrderRepository repository = new OrderRepository();
            return repository.GetOrderNumberByInvoiceNumber(InvoiceNumber);
        }
        /*CS.20AGO2016.Fin*/
    }
}
