using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using NetSteps.Data.Common.Context;
using NetSteps.Data.Common.Entities;
using NetSteps.Encore.Core.IoC;
using NetSteps.Extensibility.Core;
using NetSteps.OrderAdjustments.Common;
using NetSteps.OrderAdjustments.Common.Exceptions;
using NetSteps.OrderAdjustments.Common.Model;
using NetSteps.Data.Common.Services;

namespace NetSteps.OrderAdjustments.Service
{
	public class OrderAdjustmentHandler : IOrderAdjustmentHandler
	{
		protected internal IOrderAdjustmentProviderManager _providerManager;
		protected internal IDataObjectExtensionProviderRegistry _dataObjectExtensionProviderRegistry;
		protected internal IInventoryService _inventoryService;

		public OrderAdjustmentHandler(IOrderAdjustmentProviderManager providerManager, IDataObjectExtensionProviderRegistry dataObjectExtensionProviderRegistry, IInventoryService inventoryService)
		{
			Contract.Assert(providerManager != null);
			Contract.Assert(dataObjectExtensionProviderRegistry != null);
			Contract.Assert(inventoryService != null);

			_providerManager = providerManager;
			_dataObjectExtensionProviderRegistry = dataObjectExtensionProviderRegistry;
			_inventoryService = inventoryService;
		}

		private bool defaultFilter(IOrderContext orderContext)
		{
			Contract.Assert(orderContext != null);

			return orderContext.ValidOrderStatusIdsForOrderAdjustment.Contains(orderContext.Order.OrderStatusID);
		}

		protected internal IOrderAdjustmentOrderModification createOrderModification(IOrderContext orderContext, IOrderCustomer customer, IOrderAdjustmentProfileOrderModification modification, IOrderAdjustment adjustment)
		{
			Contract.Assert(orderContext != null);
			Contract.Assert(modification != null);
			Contract.Assert(adjustment != null);

			var orderModification = Create.New<IOrderAdjustmentOrderModification>();
			orderModification.PropertyName = modification.Property;
			orderModification.ModificationDecimalValue = modification.ModificationValue;
			orderModification.ModificationDescription = modification.Description;
			orderModification.ModificationOperationID = modification.ModificationOperationID;
			adjustment.AddOrderModification(orderModification);
			customer.AddOrderModification(orderModification);
			return orderModification;

		}

		protected internal void createOrderLineModifications(IOrderContext orderContext, IOrderAdjustmentProfileOrderItemTarget orderLineTarget, IOrderAdjustment adjustment)
		{
			Contract.Assert(orderContext != null);
			Contract.Assert(orderContext.Order != null);
			Contract.Assert(orderContext.Order.OrderAdjustments != null);
			Contract.Assert(orderLineTarget != null);
			Contract.Assert(adjustment != null);

			IOrderCustomer customer = orderContext.Order.OrderCustomers.Where(x => x.AccountID == orderLineTarget.OrderCustomerAccountID).SingleOrDefault();
			if (customer == null)
				return;
			else
			{
				IEnumerable<IOrderItem> targets;
				if (orderLineTarget.Modifications[0].ModificationOperationID == (int)OrderAdjustmentOrderLineOperationKind.AddedItem)
				{
					Contract.Assert(orderLineTarget.Quantity.HasValue && orderLineTarget.Quantity > 0, "OrderAdjustmentHandler cannot add an item with zero or negative quantity.");

					if (canAddItem(orderContext, orderLineTarget.ProductID, orderLineTarget.Quantity.Value))
					{
						var newItem = orderContext.Order.AddItem(customer, orderLineTarget.ProductID, orderLineTarget.Quantity.Value, null, null, null, true);
						ModifyItem(newItem, orderLineTarget, orderLineTarget.Modifications[0], adjustment);
						targets = new IOrderItem[] { newItem };
					}
					else
					{
						var modification = Create.New<IOrderAdjustmentProfileOrderModification>();
						modification.Description = String.Format("{{ Message:Unable to add item,ProductID:{0},Quantity:{1} }}", orderLineTarget.ProductID, orderLineTarget.Quantity);
						modification.ModificationOperationID = (int)OrderAdjustmentOrderOperationKind.Message;
						modification.Property = "UnavailableProduct";
						createOrderModification(orderContext, customer, modification, adjustment);
						targets = new IOrderItem[0];
					}
				}
				else
				{
					targets = customer.AdjustableOrderItems.Where(x => x.ProductID == orderLineTarget.ProductID && x.ParentOrderItem == null);
				}
				foreach (var mod in orderLineTarget.Modifications)
				{
					switch (mod.ModificationOperationID)
					{
						case (int)OrderAdjustmentOrderLineOperationKind.Multiplier:
						case (int)OrderAdjustmentOrderLineOperationKind.FlatAmount:
							if (targets.Count() > 0)
							{
								if (orderLineTarget.Quantity.HasValue && orderLineTarget.Quantity > 0)
								{
									// has maximum quantity
									int quantityLeft = orderLineTarget.Quantity.Value;
									for (int i = 0; i < targets.Count(); i++)
									{
										var item = targets.ElementAt(i);
										if (item.Quantity <= quantityLeft)
										{
											ModifyItem(item, orderLineTarget, mod, adjustment);
											quantityLeft -= item.Quantity;
										}
										else
										{
											orderContext.Order.AddItem(customer, item.ProductID.Value, item.Quantity - quantityLeft, null, null, null, true);
											item.Quantity = quantityLeft;
											ModifyItem(item, orderLineTarget, mod, adjustment);
										}
										if (quantityLeft == 0)
											break;
										Contract.Assert(quantityLeft > 0, "Invalid calculation - cannot have negative quantity.");
									}
								}
								else
								{
									targets.ToList().ForEach(x => ModifyItem(x, orderLineTarget, mod, adjustment));
								}
							}
							break;
						case (int)OrderAdjustmentOrderLineOperationKind.AddedItem:
							break;
					}
				}
			}
		}

		private bool canAddItem(IOrderContext context, int productID, int quantity)
		{
            DataAccess objN = new DataAccess();
            //OrderContext.Order.ParentOrderID 
            return objN.Exist(productID, quantity, context.Order.ParentOrderID.Value);

            //var result = _inventoryService.GetProductAvailabilityForOrder(context, productID, quantity);
            //return result.CanAddNormally == quantity;
		}

		private IOrderAdjustmentOrderLineModification ModifyItem(IOrderItem item, IOrderAdjustmentProfileOrderItemTarget orderLineTarget, IOrderAdjustmentProfileOrderLineModification modification, IOrderAdjustment adjustment)
		{
			var lineModification = Create.New<IOrderAdjustmentOrderLineModification>();
			lineModification.ModificationDescription = modification.Description;
			lineModification.ModificationOperationID = modification.ModificationOperationID;
			lineModification.ProductID = orderLineTarget.ProductID;
			lineModification.PropertyName = modification.Property;
			lineModification.ModificationDecimalValue = modification.ModificationValue.HasValue ? modification.ModificationValue.Value : orderLineTarget.Quantity.Value;
			lineModification.MaximumQuantityAffected = orderLineTarget.Quantity;
			adjustment.AddOrderLineModification(lineModification);
			item.AddOrderLineModification(lineModification);
			return lineModification;
		}

		protected internal decimal GetOrderItemAdjustmentDecimal(int operation, decimal modifier, decimal propertyValue)
		{
			switch (operation)
			{
				case (int)OrderAdjustmentOrderLineOperationKind.AddedItem:
					throw new OrderAdjustmentProviderException(OrderAdjustmentProviderException.ExceptionKind.OPERATION_KIND_INVALID, "Order item adjustment operation 'AddedItem' cannot be used for method 'GetOrderItemAdjustmentDecimal'.");
				case (int)OrderAdjustmentOrderLineOperationKind.FlatAmount:
					return -1 * modifier;
				case (int)OrderAdjustmentOrderLineOperationKind.Multiplier:
					return -1 * modifier;
				default:
					throw new OrderAdjustmentProviderException(OrderAdjustmentProviderException.ExceptionKind.PROPERTY_KIND_UNDEFINED, String.Format("Order modification modification property of {0} is not defined.", operation.ToString()));
			}
		}


		public void ApplyAdjustments(IOrderContext orderContext, IEnumerable<IOrderAdjustmentProfile> adjustmentProfiles, Predicate<IOrderContext> orderValidityFilter, Func<IOrderContext, IEnumerable<IOrderAdjustmentProfile>, IEnumerable<IOrderAdjustmentProfile>> orderAdjustmentValidityFilter, bool stripExistingAdjustments)
		{
			Contract.Assert(orderContext != null);
			Contract.Assert(orderContext.Order != null);
			Contract.Assert(orderContext.Order.OrderAdjustments != null);
			Contract.Assert(adjustmentProfiles != null);
			Contract.Assert(orderValidityFilter != null);
			Contract.Assert(orderAdjustmentValidityFilter != null);

			if (!orderValidityFilter(orderContext))
				throw new OrderAdjustmentProviderException(OrderAdjustmentProviderException.ExceptionKind.ORDER_INVALID_FOR_ADJUSTMENT_APPLICATION, "Order validity filter for the order returned false.");

			if (stripExistingAdjustments)
				orderContext.Order.ClearAdjustments();

			adjustmentProfiles = orderAdjustmentValidityFilter(orderContext, adjustmentProfiles);

			//using (TransactionScope scope = new TransactionScope())
			//{
			Dictionary<IOrderAdjustment, IOrderAdjustmentProfile> adjustmentProfileMapper = new Dictionary<IOrderAdjustment, IOrderAdjustmentProfile>();

			// don't mess with non-order adjustment steps
			var nonAdjustmentSteps = orderContext.InjectedOrderSteps.Where(x => !typeof(IOrderAdjustmentOrderStep).IsAssignableFrom(x.GetType())).ToList();
			orderContext.InjectedOrderSteps.Clear();
			nonAdjustmentSteps.ForEach(step => orderContext.InjectedOrderSteps.Add(step));

			foreach (IOrderAdjustmentProfile adjustmentProfile in adjustmentProfiles)
			{
				var newAdjustment = Create.New<IOrderAdjustment>();
				newAdjustment.ExtensionProviderKey = adjustmentProfile.ExtensionProviderKey;
				newAdjustment.Description = adjustmentProfile.Description;
				IOrderAdjustmentProvider provider = _providerManager.GetProvider(adjustmentProfile.ExtensionProviderKey);
				newAdjustment.Extension = provider.CreateOrderAdjustmentDataObjectExtension(adjustmentProfile);
				adjustmentProfileMapper.Add(newAdjustment, adjustmentProfile);
				foreach (IOrderAdjustmentProfileOrderModification orderModification in adjustmentProfile.OrderModifications)
				{
					foreach (var accountID in adjustmentProfile.AffectedAccountIDs)
					{
						var orderCustomer = orderContext.Order.OrderCustomers.FirstOrDefault(cust => cust.AccountID == accountID);
						Contract.Assert(orderCustomer != null);
						createOrderModification(orderContext, orderCustomer, orderModification, newAdjustment);
					}
				}
				foreach (IOrderAdjustmentProfileOrderItemTarget orderLineTarget in adjustmentProfile.OrderLineModificationTargets)
				{
					createOrderLineModifications(orderContext, orderLineTarget, newAdjustment);
				}
				// create a new set of OrderAdjustmentOrderSteps
				var orderAdjustmentSteps = new List<IOrderAdjustmentOrderStep>();

				foreach (var injectedOrderStep in adjustmentProfile.AddedOrderSteps)
				{
					var existing = orderContext.InjectedOrderSteps.SingleOrDefault(x => x.OrderStepReferenceID == injectedOrderStep.OrderStepReferenceID);
					if (existing != null)
					{
						orderAdjustmentSteps.Add((IOrderAdjustmentOrderStep)existing);
						newAdjustment.AddOrderStep(existing);
					}
					else
					{
						orderAdjustmentSteps.Add(injectedOrderStep);
						newAdjustment.AddOrderStep(injectedOrderStep);
					}
				}

				orderAdjustmentSteps.ForEach(step => orderContext.InjectedOrderSteps.Add(step));

				orderContext.Order.AddOrderAdjustment(newAdjustment);
			}
			//}
		}

		public IEnumerable<IOrderAdjustmentProfile> GetOrderAdjustments(IOrderContext orderContext)
		{
			Contract.Assert(orderContext != null);
			Contract.Assert(orderContext.Order != null);
			Contract.Assert(orderContext.Order.OrderAdjustments != null);

			List<IOrderAdjustmentProfile> restoredAdjustments = new List<IOrderAdjustmentProfile>();
			foreach (IOrderAdjustment adjustment in orderContext.Order.OrderAdjustments)
			{
				IOrderAdjustmentProvider provider = _providerManager.GetProvider(adjustment.ExtensionProviderKey);
				restoredAdjustments.Add(provider.GetOrderAdjustmentProfile(orderContext, adjustment.OrderAdjustmentID));
			}
			return restoredAdjustments;
		}

		public void RemoveAdjustment(IOrderContext orderContext, IOrderAdjustment adjustment)
		{
			Contract.Assert(orderContext != null);
			Contract.Assert(orderContext.Order != null);
			Contract.Assert(orderContext.Order.OrderAdjustments != null);
			Contract.Assert(adjustment != null);

			IOrderAdjustment retrievedAdjustment = orderContext.Order.OrderAdjustments.FirstOrDefault(x => x == adjustment);

			orderContext.Order.OrderAdjustments.Remove(retrievedAdjustment);
			var adjustmentProvider = _dataObjectExtensionProviderRegistry.RetrieveExtensionProvider(adjustment.ExtensionProviderKey) as IOrderAdjustmentProvider;
			adjustmentProvider.NotifyOfRemoval(orderContext, adjustment);
			if (retrievedAdjustment != default(IOrderAdjustment))
			{
				if (retrievedAdjustment.OrderLineModifications != null)
				{
					foreach (IOrderAdjustmentOrderLineModification lineModification in retrievedAdjustment.OrderLineModifications)
					{
						int subtractQuantity = (int)lineModification.ModificationDecimalValue;
						foreach (IOrderCustomer customer in orderContext.Order.OrderCustomers)
						{
							for (int i = 0; i < customer.OrderItems.Count; i++)
							{
								var item = customer.OrderItems[i];
								foreach (IOrderAdjustmentOrderLineModification mod in item.OrderLineModifications)
								{
									if (mod.ModificationOperationID == (int)OrderAdjustmentOrderLineOperationKind.AddedItem)
									{
										if (item.Quantity <= subtractQuantity)
										{
											orderContext.Order.RemoveItem(item);
										}
										else
										{
											item.Quantity -= subtractQuantity;
										}
									}
								}
							}
						}
					}
				}
				if (retrievedAdjustment.OrderModifications != null)
				{
					List<IOrderAdjustmentOrderModification> orderModifications = retrievedAdjustment.OrderModifications.ToList();
					foreach (IOrderAdjustmentOrderModification orderModification in orderModifications)
					{
						orderModification.MarkAsDeleted();
					}
				}
				retrievedAdjustment.MarkAsDeleted();
			}
		}


		public void RemoveAllAdjustments(IOrderContext orderContext)
		{
			Contract.Assert(orderContext != null);
			Contract.Assert(orderContext.Order != null);
			Contract.Assert(orderContext.Order.OrderAdjustments != null);

			foreach (IOrderAdjustment adjustment in orderContext.Order.OrderAdjustments)
				RemoveAdjustment(orderContext, adjustment);
		}

		public void CommitAdjustments(IOrderContext orderContext)
		{
			Contract.Assert(orderContext != null);
			Contract.Assert(orderContext.Order != null);
			Contract.Assert(orderContext.Order.OrderAdjustments != null);

			foreach (IOrderAdjustment adjustment in orderContext.Order.OrderAdjustments)
			{
				CommitAdjustment(adjustment, orderContext);
			}
		}

		private void CommitAdjustment(IOrderAdjustment adjustment, IOrderContext orderContext)
		{
			Contract.Assert(orderContext != null);
			Contract.Assert(orderContext.Order != null);
			Contract.Assert(orderContext.Order.OrderAdjustments != null);
			Contract.Assert(adjustment != null);

			var adjustmentProvider = _dataObjectExtensionProviderRegistry.RetrieveExtensionProvider(adjustment.ExtensionProviderKey) as IOrderAdjustmentProvider;
			adjustmentProvider.CommitAdjustment(adjustment, orderContext);
		}
	}
}
