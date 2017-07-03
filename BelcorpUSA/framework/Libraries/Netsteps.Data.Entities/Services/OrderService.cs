using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetSteps.Bundles.Common.Models;
using NetSteps.Commissions.Common;
using NetSteps.Common.Base;
using NetSteps.Common.Collections;
using NetSteps.Common.Configuration;
using NetSteps.Common.Exceptions;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Common.Validation.NetTiers;
using NetSteps.Data.Common.Context;
using NetSteps.Data.Common.Entities;
using NetSteps.Data.Common.Models;
using NetSteps.Data.Common.Services;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Business.Interfaces;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Generated;
using NetSteps.Data.Entities.Interfaces;
using NetSteps.Data.Entities.PaymentGateways;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Diagnostics.Utilities;
using NetSteps.Encore.Core.IoC;
using NetSteps.GiftCards.Common;
using NetSteps.OrderAdjustments.Common;
using NetSteps.OrderAdjustments.Common.Model;
using NetSteps.Promotions.Common;
using NetSteps.Promotions.Common.Model;
using NetSteps.Promotions.Plugins.Common.Qualifications;
using NetSteps.Data.Entities.TokenValueProviders;
using NetSteps.Data.Entities.Mail;
using System.IO;
using System.Net.Mail;
using System.Text.RegularExpressions;
using NetSteps.Common.EldResolver;
using System.Transactions;
using NetSteps.Data.Entities.Business.Common;
using NetSteps.Data.Entities.Business.HelperObjects;
using NetSteps.OrderAdjustments.Common.Exceptions;
using NetSteps.Data.Entities.Business.Logic;
using NetSteps.Data.Entities.Context;
using NetSteps.Data.Entities.EntityModels;
using NetSteps.Extensibility.Core;
using NetSteps.Web;
using Newtonsoft.Json;

namespace NetSteps.Data.Entities.Services
{
    public class OrderService : IOrderService
    {
        protected InventoryBaseRepository inventoryRepository;
        protected ITotalsCalculator totalsCalculator;
        protected IShippingCalculator shippingCalculator;
        protected IOrderRepository orderRepository;
        protected IWarehouseMaterialAllocationRepository WarehouseMaterialAllocationRepository;
        protected IWarehouseMaterialRepository WarehouseMaterialRepository;
        protected IMaterialRepository MaterialRepository;

        public OrderService(InventoryBaseRepository repo)
        {

            inventoryRepository = repo ?? Create.New<InventoryBaseRepository>();
            totalsCalculator = Create.New<ITotalsCalculator>();
            shippingCalculator = Create.New<IShippingCalculator>();
            WarehouseMaterialAllocationRepository = Create.New<IWarehouseMaterialAllocationRepository>();
            WarehouseMaterialRepository = Create.New<IWarehouseMaterialRepository>();
            orderRepository = Create.New<IOrderRepository>();
            MaterialRepository = Create.New<IMaterialRepository>();
        }

        public OrderService()
            : this(null)
        {
        }

        /// <summary>
        /// Loads the specified order by its OrderID.
        /// </summary>
        /// <param name="orderID">The order ID.</param>
        /// <returns></returns>
        public Common.Entities.IOrder Load(int orderID)
        {
            return Order.LoadFull(orderID);
        }

        public void IsKitItemPricesAlive(int kitItemPricesID)
        {
            var o = KitItemPrice.Load(kitItemPricesID);
            var p = (KitItemPrice)o;
            var c = new KitItemPrice()
            {
                OrderItemID = p.OrderItemID,
                UnitPrice = p.UnitPrice,
                KitItemPriceID = p.KitItemPriceID
            };

        }

        /// <summary>
        /// Updates quantities of order items within the order.
        /// </summary>
        /// <param name="orderContext">The order context.</param>
        /// <param name="itemsToUpdate">The items to update.</param>
        public virtual IEnumerable<IOrderItem> UpdateOrderItemQuantities(IOrderContext orderContext, IEnumerable<IOrderItemQuantityModification> itemsToUpdate)
        {
            Contract.Requires<ArgumentNullException>(orderContext != null);
            var results = new List<IOrderItem>();
            using (TransactionScope scope = new TransactionScope())
            {
                var inventoryRepository = Create.New<InventoryBaseRepository>();

                foreach (var updateItem in itemsToUpdate)
                {
                    IOrderItem current = null;
                    var product = inventoryRepository.GetProductwithoutCache(updateItem.ProductID);

                    if (updateItem.ExistingItem != null)
                    {
                        current = updateItem.ExistingItem;
                    }
                    else if (updateItem.ModificationKind == OrderItemQuantityModificationKind.SetToQuantity || !product.RequiresCustomization())
                    {
                        current = updateItem.Customer.OrderItems.FirstOrDefault(item => item.ProductID == updateItem.ProductID
                            && item.ParentOrderItem == null
                            && !item.OrderLineModifications.Any(m => m.ModificationOperationID == (int)OrderAdjustmentOrderLineOperationKind.AddedItem));
                    }

                    if (product.IsDynamicKit())
                    {
                        switch (updateItem.ModificationKind)
                        {
                            case OrderItemQuantityModificationKind.SetToQuantity:
                                if (updateItem.Quantity > 0)
                                    throw new InvalidOperationException("Cannot set dynamic kit quantity to a number greater than 1.");
                                current.Quantity = updateItem.Quantity == 0 ? 0 : 1;
                                break;
                            case OrderItemQuantityModificationKind.Add:
                                current = CreateOrderItem(orderContext, updateItem);
                                break;
                            case OrderItemQuantityModificationKind.Delete:
                                current.Quantity = 0;
                                break;
                            default:
                                // this adjustment type is not currently supported.
                                throw new NotImplementedException();
                        }
                    }
                    else if (product.IsStaticKit())
                    {
                        switch (updateItem.ModificationKind)
                        {
                            case OrderItemQuantityModificationKind.Add:
                                current = CreateOrderItem(orderContext, updateItem);
                                break;
                            case OrderItemQuantityModificationKind.Delete:
                                current.Quantity = 0;
                                break;
                            case OrderItemQuantityModificationKind.SetToQuantity:
                                if (current.Quantity != updateItem.Quantity)
                                {
                                    current.Quantity = updateItem.Quantity;

                                    if (current.ProductID != null)
                                    {
                                        var queryQuantities = PreOrderExtension.GetMaterialQuantityByPreOrderID(orderContext.Order.PreorderID, current.ProductID.Value);
                                        foreach (var item in ((OrderItem)current).ChildOrderItems)
                                        {
                                            var quantity = queryQuantities.First(o => o.MaterialID == item.MaterialID).Quantity;
                                            if (quantity != null)
                                                item.Quantity = quantity.Value;

                                        }
                                    }
                                }

                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        switch (updateItem.ModificationKind)
                        {
                            case OrderItemQuantityModificationKind.SetToQuantity:
                                if (current == null)
                                    current = CreateOrderItem(orderContext, updateItem);
                                else
                                {
                                    if (current.Quantity < updateItem.Quantity)
                                    {
                                        ((OrderCustomer)updateItem.Customer).NeedsAutoBundling = true;
                                    }
                                    current.Quantity = updateItem.Quantity;
                                }
                                break;
                            case OrderItemQuantityModificationKind.Add:
                                if (current == null)
                                {
                                    current = CreateOrderItem(orderContext, updateItem);
                                }
                                else
                                {
                                    current.Quantity += updateItem.Quantity;
                                    ((OrderCustomer)updateItem.Customer).NeedsAutoBundling = true;
                                }
                                break;
                            case OrderItemQuantityModificationKind.Delete:
                                if (current != null)
                                    current.Quantity = 0;
                                break;
                            default:
                                // this adjustment type is not currently supported.
                                throw new NotImplementedException();
                        }
                    }
                    results.Add(current);
                }
                scope.Complete();
            }
            return results;
        }

        public IEnumerable<IOrderItem> UpdateOrderItemProperties(IOrderContext orderContext, IEnumerable<IOrderItemPropertyModification> itemsToUpdate)
        {
            Contract.Requires<ArgumentNullException>(orderContext != null);
            Contract.Requires<ArgumentNullException>(itemsToUpdate != null);

            List<IOrderItem> results = new List<IOrderItem>();

            foreach (var updateItem in itemsToUpdate)
            {
                IOrderItem current = updateItem.ExistingItem;

                if (current != null)
                {
                    foreach (var prop in updateItem.Properties)
                    {
                        current.AddOrUpdateOrderItemProperty(prop.Key, prop.Value);
                    }

                    results.Add(current);
                }
            }

            return results;
        }

        /// <summary>
        /// Splits order items within the order into multiple line items.
        /// </summary>
        /// <param name="orderContext">The order context.</param>
        /// <param name="targetOrderItem">The target order item.</param>
        /// <param name="NewOrderItemQuantities">The new order item quantities.</param>
        /// <returns></returns>
        public virtual IEnumerable<IOrderItem> SplitOrderItem(IOrderContext orderContext, IOrderItem targetOrderItem, IEnumerable<int> newOrderItemQuantities)
        {
            Contract.Requires<ArgumentNullException>(orderContext != null);
            Contract.Assert(
                targetOrderItem.Quantity == newOrderItemQuantities.Sum(),
                "Cannot split order item into a set of items whose summed quantities are not equal to the original item's quantity.");

            var updatedItems = new List<IOrderItem>();

            // we have to get the order customer. This should be refactored so as to not be depended on the concrete data object.
            var customer = ((OrderItem)targetOrderItem).OrderCustomer;

            for (int i = 0; i < newOrderItemQuantities.Count(); i++)
            {
                if (i == 0)
                {
                    targetOrderItem.Quantity = newOrderItemQuantities.ElementAt(i);
                    updatedItems.Add(targetOrderItem);
                }
                else
                {
                    var mod = Create.New<IOrderItemQuantityModification>();
                    mod.Customer = customer;
                    mod.Quantity = newOrderItemQuantities.ElementAt(i);
                    mod.ProductID = targetOrderItem.ProductID.Value;
                    updatedItems.Add(CreateOrderItem(orderContext, mod));
                }
            }

            return updatedItems;
        }

        /// <summary>
        /// Updates the order and order totals.
        /// </summary>
        /// <param name="orderContext">The order context.</param>
        public virtual void UpdateOrder(IOrderContext orderContext)
        {
            Contract.Requires<ArgumentNullException>(orderContext != null);
            /*CS.19MAY2016.Inicio.Validad Order*/
            if (orderContext.Order == null)
                return;
            /*CS.19MAY2016.Fin*/
            if (orderContext.Order.AsOrder().IsCommissionable())
                return;

            orderContext.Order.AsOrder().StartEntityTracking();

            ClearExistingOrderAdjustments(orderContext);
            ConsolidateOrderItems(orderContext);
            ClearDeletedItems(orderContext);

            // Ensure that whatever called the update has been calculated before updating adjustments.
            // Don't calculate shipping and taxes on the first calculate call.
            var pendingState = orderContext.Order.AsOrder().OrderPendingState;
            orderContext.Order.AsOrder().OrderPendingState = Constants.OrderPendingStates.Open;
            CalculateTotals(orderContext.Order);

            orderContext.Order.AsOrder().OrderPendingState = pendingState;

            if (this.ShouldApplyAdjustmentsToOrder(orderContext.Order))
            {
                UpdateOrderAdjustments(orderContext);
            }

            ClearDeletedItems(orderContext);

            if (ShouldAutoBundleOrder(orderContext.Order))
            {
                foreach (var customer in orderContext.Order.AsOrder().OrderCustomers)
                {
                    if (customer.NeedsAutoBundling)
                    {
                        AutoBundleItemsForCustomer(orderContext, customer);
                        customer.NeedsAutoBundling = false;
                    }
                }
            }

            CalculateTotals(orderContext.Order);
        }

        /// <summary>
        /// Submits the order and commits order adjustments.
        /// </summary>
        /// <param name="orderContext">The order context.</param>
        public virtual BasicResponse SubmitOrder(IOrderContext orderContext)
        {
            var response = SubmitOrder(orderContext.Order.AsOrder());
            if (response.Success)
            {
                try
                {
                    var orderAdjustmentService = Create.New<IOrderAdjustmentService>();
                    orderAdjustmentService.CommitOrderAdjustments(orderContext);
                }
                catch (Exception ex)
                {
                    string errorMessage = ex.Log(orderID: orderContext.Order != null ? orderContext.Order.OrderID.ToIntNullable() : null).PublicMessage;
                    response.Message = Translation.GetTerm("ErrorCommittingOrderAdjustments", "Order submission was successful, but an error occurred while finalizing the order: {0}", new[] { errorMessage });
                }
            }

            return response;
        }

        protected virtual BasicResponse SubmitOrder(Order order)
        {
            try
            {
                BasicResponse response = new BasicResponse();

                #region Validation
                var productIDs = new List<int>();
                foreach (var customer in order.OrderCustomers)
                {
                    if (customer.IsHostess)
                    {
                        BasicResponse rewardResponse = customer.ValidateHostessRewards();
                        if (!rewardResponse.Success)
                        {
                            return rewardResponse;
                        }
                    }

                    OrderShipment orderShipment = customer.OrderShipments.FirstOrDefault();
                    if (orderShipment == null)
                        orderShipment = order.OrderShipments.FirstOrDefault();

                    // TODO: Maybe virtual items don't need a warehouse??? - Lundy
                    if (orderShipment == null)
                    {
                        throw new NetStepsException("Cannot check warehouse inventory levels because the shipping address was not found.")
                        {
                            PublicMessage = Translation.GetTerm("CannotCheckWarehouseInventoryLevels", "Cannot update warehouse inventory levels because the shipping address was not found.")
                        };
                    }

                    foreach (var orderItem in customer.OrderItems)
                    {
                        if (orderItem.ProductID.HasValue && !productIDs.Contains(orderItem.ProductID.Value))
                            productIDs.Add(orderItem.ProductID.ToInt());

                        var product = inventoryRepository.GetProduct(orderItem.ProductID.Value);

                        IEnumerable<string> excluded;
                        if (!product.AssertCustomerCanOrderProduct(customer, out excluded))
                        {
                            //Disallow Products from being shipped to Excluded State/Provinces
                            response.Message = Translation.GetTerm("CustomerCannotOrderProduct", "The product ({0}: {1}) could not be added to the cart due to specific restrictions: item cannot be ordered in the state(s) of {2}.", Translation.GetTerm("sku"), ((Product)product).SKU, String.Join(", ", excluded));
                            response.Success = false;
                            return response;
                        }

                        // Ignore Inventory for Non-Shippable products (they are virtual) - JHE
                        if (!product.ProductBase.IsShippable)
                            break;

                        if (product.IsVariantTemplate)
                        {
                            response.Message = Translation.GetTerm("TheProductYourTriedToOrderIsAVariantTemplate", "The product you tried to order ({0}) is a variant template. Please select a product variant.", product.Translations.Name());
                            response.Success = false;
                            return response;
                        }

                        if (!IsDynamicKitValid(orderItem))
                        {
                            response.Message = Translation.GetTerm("TheBundleYouTriedToOrderIsNotComplete", "The bundle you tried to order ({0}) is not complete.", product.Translations.Name());
                            response.Success = false;
                            return response;
                        }

                        if (!IsStaticKitValid(orderItem))
                        {
                            response.Message = Translation.GetTerm("TheKitYouTriedToOrderIsNotComplete", "The kit you tried to order ({0}) is not complete.", product.Translations.Name());
                            response.Success = false;
                            return response;
                        }

                        if (orderItem.ProductID.HasValue)
                        {
                            if (!product.Active)
                            {
                                response.Message = Translation.GetTerm("TheProductYouOrderedIsNotActive", "The product you ordered ({0}) is not active.", product.Translations.Name());
                                response.Success = false;
                                return response;
                            }
                            //Validacion de Stock Disponible 
                            //InventoryLevels inventoryLevel = Product.CheckStock(orderItem.ProductID.Value, orderShipment);
                            //switch (product.ProductBackOrderBehaviorID)
                            //{
                            //    case (int)Constants.ProductBackOrderBehavior.AllowBackorder:
                            //    case (int)Constants.ProductBackOrderBehavior.AllowBackorderInformCustomer:
                            //    //    if (inventoryLevel.IsOutOfStock)
                            //    //    {
                            //    //        orderItem.WasBackordered = true;
                            //    //    }
                            //    //    break;
                            //    //case (int)Constants.ProductBackOrderBehavior.ShowOutOfStockMessage:
                            //    //case (int)Constants.ProductBackOrderBehavior.Hide:
                            //    //    if (inventoryLevel.IsOutOfStock)
                            //    //    {
                            //    //        response.Message = Translation.GetTerm("TheProductYouTriedToOrderIsOutOfStock", "The product you tried to order ({0}) is out of stock.", product.Translations.Name());
                            //    //        response.Success = false;
                            //    //        return response;
                            //    //    }

                            //    //    int currentQTYOrderedForProduct = GetCurrentQTYOrderedForProduct(order, product.ProductID);
                            //    //    if (inventoryLevel.QuantityAvailable.HasValue && currentQTYOrderedForProduct > inventoryLevel.QuantityAvailable)
                            //    //    {
                            //    //        response.Message = Translation.GetTerm("TheProductYouOnlyHasXUnitsInStock", "The product you ordered ({0}) only has {1} unit(s) left in stock", product.Translations.Name(), inventoryLevel.QuantityAvailable.Value);
                            //    //        response.Success = false;
                            //    //        return response;
                            //    //    }
                            //        break;
                            //}
                        }
                    }
                }
                #endregion

                response.Success = true;
                order.StartEntityTracking();

                if (!IsOrderValidForSubmission(order, response))
                {
                    return response;
                }

                #region PrepareToSubmittal

                BasicResponse submitResponse = PrepareOrderForSubmittal(order);
                if (!submitResponse.Success)
                {
                    return submitResponse;
                }

                #endregion

                order.Save();
                order.IsTemplate = true;
                var result = SubmitPayments(order);

                // POINT OF NO RETURN - The credit card may have been charged. Everything below this point should be wrapped in a try-catch.

                try
                {
                    order.StartEntityTracking(); // Because this gets turned off from orderPayment saving. - JHE
                }
                catch (Exception ex)
                {
                    // Log and continue
                    ex.Log(orderID: order != null ? order.OrderID.ToIntNullable() : null);
                }

                // If there are enough successfull payments to cover the order, let it complete normally - JHE
                if (!result.Success && CanChangeToPaidStatus(order))
                {
                    result.Success = true;
                }

                if (!result.Success)
                {
                    // If the order is not fully paid we need to call calculate totals to update the balance.
                    // We are setting the OrderPendingState to Open to ensure taxes and shipping are not recalculated.
                    // Since all we are doing is updating the balance to reflect the failed payments we are bypassing the OrderService.
                    order.OrderPendingState = Constants.OrderPendingStates.Open;
                    CalculateTotals(order);
                    order.OrderPendingState = Constants.OrderPendingStates.Quote;

                    if (order.OrderPayments.Any(op => op.OrderPaymentStatusID == (short)Constants.OrderPaymentStatus.Completed))
                    {
                        order.OrderStatusID = (short)Constants.OrderStatus.PartiallyPaid;
                    }
                    else if (result.FailureCount > 0)
                    {
                        order.OrderStatusID = (short)Constants.OrderStatus.CreditCardDeclined;
                    }


                    order.Save();

                    response.Success = false;
                    response.Message = result.Success
                        ? Translation.GetTerm("ErrorPaymentAmountNotEqualToGrandTotal", "One or more payments were successful, but the total payment amount did not equal the grand total.")
                        : result.Message;
                    return response;
                }

                // Successful order completion
                response.Success = true;
                try
                {
                    ChangeToPaidStatus(order);
                    DataAccess.GetCommand("ups_GetItemValuation", new Dictionary<string, object>() { { "@orderID", order.OrderID } }, "Core");

                }
                catch (Exception ex)
                {
                    string errorMessage = ex.Log(orderID: order != null ? order.OrderID.ToIntNullable() : null).PublicMessage;
                    response.Message = Translation.GetTerm("ErrorChangingOrderToPaidStatus", "Payment was successful, but an error occurred while finalizing the order: {0}", new[] { errorMessage });
                }
                return response;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException, order != null ? order.OrderID.ToIntNullable() : null);
            }
        }

        public virtual bool CanChangeToPaidStatus(IOrder order)
        {
            // Order templates can always change to "Paid" because they don't actually process payments and "Paid" is their final status.
            if (order.AsOrder().IsTemplate)
                return true;

            if (order.OrderTypeID == (int)Constants.OrderType.ReplacementOrder && (order.GrandTotal ?? 0) == 0)
                return true;

            int totalOrderItems = order.OrderCustomers.SelectMany(oc => oc.OrderItems).Count();
            if (totalOrderItems == 0)
                return false;

            // If not fully paid, return false.
            if (!IsPaidInFull(order))
                return false;

            // If status is cancelled, return false.
            if (order.OrderStatusID == (short)Constants.OrderStatus.Cancelled
                || order.OrderStatusID == (short)Constants.OrderStatus.CancelledPaid)
            {
                return false;
            }

            // If status is already commissionable, return false.
            var orderStatus = SmallCollectionCache.Instance.OrderStatuses.GetById(order.OrderStatusID);
            if (orderStatus.IsCommissionable || orderStatus.Name == "Cancelled" || orderStatus.Name == "Cancelled Paid")
                return false;

            // All clear, return true.
            return true;
        }

        public virtual void ChangeToPaidStatus(IOrder order)
        {
            try
            {
                var orderStatusID = (short)Constants.OrderStatus.Paid;

                if (!order.AsOrder().IsTemplate)
                {
                    // TODO: Re-visit this later; this loop of OrderShipments and OrderCustomers does not seem to be accurate. - JHE
                    foreach (var orderShipment in order.AsOrder().OrderShipments)
                    {
                        var containsShippableItems = true;
                        if (orderShipment.OrderCustomerID.HasValue && order.AsOrder().OrderCustomers.Select(oc => oc.OrderCustomerID).Contains(orderShipment.OrderCustomerID.Value))
                            containsShippableItems = order.AsOrder().OrderCustomers.FirstOrDefault(oc => oc.OrderCustomerID == orderShipment.OrderCustomerID.Value).ContainsShippableItems();
                        else
                            containsShippableItems = order.AsOrder().OrderCustomers.ContainsShippableItems();

                        if (!containsShippableItems)
                            orderShipment.OrderShipmentStatusID = (short)ConstantsGenerated.OrderShipmentStatus.Shipped;
                    }

                    // Mark order 'Shipped' if it contains no shippable items - JHE
                    bool orderContainsShippableItems = order.AsOrder().OrderCustomers.ContainsShippableItems();
                    if (!orderContainsShippableItems)
                        orderStatusID = (short)Constants.OrderStatus.Shipped;
                }

                order.OrderStatusID = orderStatusID;
                order.AsOrder().CompleteDate = order.AsOrder().CompleteDate ?? DateTime.Now;
                order.AsOrder().CommissionDate = order.AsOrder().CommissionDate ?? DateTime.Now;
                FinalizeCommissionableValue(order.AsOrder());
                order.Save();

                try
                {
                    if (!order.AsOrder().IsTemplate && order.OrderTypeID != Constants.OrderType.ReturnOrder.ToShort() && order.OrderTypeID != Constants.OrderType.ReplacementOrder.ToShort())
                    {
                        order.AsOrder().FinalizeTax();
                    }
                }
                catch (Exception ex)
                {
                    // Log and continue
                    ex.Log(orderID: order != null ? order.OrderID.ToIntNullable() : null);
                }

                try
                {
                    if (order.OrderTypeID != (short)Constants.OrderType.PartyOrder && !order.AsOrder().IsTemplate)
                    {
                        //						DomainEventQueueItem.AddOrderCompletedEventToQueue(order.OrderID);
                    }
                }
                catch (Exception ex)
                {
                    // Log and continue
                    ex.Log(orderID: order != null ? order.OrderID.ToIntNullable() : null);
                }

                try
                {
                    //Return orders shouldn't be changing the on hand inventory allotment.
                    if (!order.AsOrder().IsTemplate && (order.OrderTypeID != (Int32)Constants.OrderType.ReturnOrder))
                    {
                        UpdateInventoryLevels(order);
                    }
                }
                catch (Exception ex)
                {
                    // Log and continue
                    ex.Log(orderID: order != null ? order.OrderID.ToIntNullable() : null);
                }

                try
                {
                    var gcService = Create.New<IGiftCardService>();
                    gcService.GenerateGiftCardCodesForAllPurchasedGiftCards(order);
                }
                catch (Exception ex)
                {
                    // Log and continue
                    ex.Log(orderID: order != null ? order.OrderID.ToIntNullable() : null);
                }

                //SendMailOrderComplete
                string SendMailCondition;
                string SendMailConfiguration = ConfigurationManager.AppSettings["SendMailOrderComplete"].ToString();
                SendMailCondition = SendMailConfiguration == null ? "1" : SendMailConfiguration;
                if (SendMailCondition == "1")
                {
                    OnOrderSuccessfullyCompleted(order);
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        protected virtual BasicResponse PrepareOrderForSubmittal(Order order)
        {
            return new BasicResponse { Success = true };
        }

        /// <summary>
        /// Clears the existing order adjustments.
        /// </summary>
        /// <param name="orderContext">The order context.</param>
        protected virtual void ClearExistingOrderAdjustments(IOrderContext orderContext)
        {
            Contract.Requires<ArgumentNullException>(orderContext != null);

            orderContext.Order.ClearAdjustments();
        }

        /// <summary>
        /// Consolidates the order items.
        /// </summary>
        /// <param name="orderContext">The order context.</param>
        protected virtual void ConsolidateOrderItems(IOrderContext orderContext)
        {
            Contract.Requires<ArgumentNullException>(orderContext != null);

            foreach (IOrderCustomer customer in orderContext.Order.OrderCustomers)
            {
                // we're going to merely cast to an OrderCustomer TEMPORARILY so that we have access to its components.
                var orderCustomerRef = (OrderCustomer)customer;
                var rootItems = orderCustomerRef.ParentOrderItems;
                foreach (var orderItem in rootItems)
                {
                    var matches = rootItems.Except(new OrderItem[] { orderItem }) // remove the one we're testing against
                        .Where(otherItem => otherItem.ProductID == orderItem.ProductID && orderItem.CanBeCombinedWith(otherItem)).ToList();
                    if (matches.Any())
                    {
                        matches.Add(orderItem);
                        CombineOrderItems(orderContext, matches);
                    }
                }
            }
        }

        /// <summary>
        /// Updates the order adjustments for the order.
        /// </summary>
        /// <param name="orderContext">The order context.</param>
        protected virtual void UpdateOrderAdjustments(IOrderContext orderContext)
        {
            Contract.Requires<ArgumentNullException>(orderContext != null);

            using (var updateOrderAdustmentsTracer = this.TraceActivity("OrderService::UpdateOrderAdjustments"))
            {
                try
                {
                    orderContext.CurrentActivity = AccountExtensions.GetActivityInfoForCurrentUser(orderContext.Order.OrderCustomers[0].AccountID);  //INI-FIN - GR_Encore-07
                    var orderAdjustmentService = Create.New<IOrderAdjustmentService>();
                    var orderAdjustmentHandler = Create.New<IOrderAdjustmentHandler>();
                    var adjustments = orderAdjustmentService.GetApplicableOrderAdjustments(orderContext);

                    orderAdjustmentHandler.ApplyAdjustments(orderContext, adjustments);

                    foreach (var itm in orderContext.Order.AsOrder().GetAllOrderItems()
                        .Where(itm => itm.OrderAdjustmentOrderLineModifications
                            .Any(x => x.ModificationOperationID == (int)OrderAdjustmentOrderLineOperationKind.AddedItem)))
                    {
                        itm.HasChanges = false;
                    }
                }
                catch (Exception excp)
                {
                    excp.TraceException(excp);
                    throw;
                }
            }
        }
        protected internal IOrderAdjustmentProviderManager _providerManager;

        public void ApplyAdjustments(IOrderContext orderContext, IEnumerable<IOrderAdjustmentProfile> adjustmentProfiles, Predicate<IOrderContext> orderValidityFilter, Func<IOrderContext, IEnumerable<IOrderAdjustmentProfile>, IEnumerable<IOrderAdjustmentProfile>> orderAdjustmentValidityFilter, bool stripExistingAdjustments)
        {
            _providerManager = Create.New<IOrderAdjustmentProviderManager>();
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


        private bool canAddItem(IOrderContext context, int productID, int quantity)
        {
            int WareHouse = 0;
            foreach (var item in context.Order.AsOrder().OrderShipments)
            {
                WareHouse = PreOrderExtension.WareHouseByAccountAddress(context.Order.AsOrder().OrderCustomers[0].AccountID, item.SourceAddressID.Value);
            }
            return Exist(productID, quantity, WareHouse);
        }

        public bool Exist(int productId, int quantity, int wareHouseID)
        {
            List<InventoryCheck> InventoryCheck = PreOrderBusinessLogic.Instance.InventoryCheckResult(productId, wareHouseID);
            List<ProductRelations> objProductRelations = new List<ProductRelations>();
            foreach (var inventoryCheck in InventoryCheck)
            {
                if (quantity > inventoryCheck.Available)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return false;

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
                        // Order.WareHouseByAccountAddress(Convert.ToInt32(Session["AccountID"]), shippingAddressId);

                        var newItem = orderContext.Order.AddItem(customer, orderLineTarget.ProductID, orderLineTarget.Quantity.Value, null, null, null, true);
                        ModifyItem(newItem, orderLineTarget, orderLineTarget.Modifications[0], adjustment);
                        targets = new IOrderItem[] { newItem };
                    }
                    else
                    {
                        var modification = Create.New<IOrderAdjustmentProfileOrderModification>(); modification.Description = "" + orderLineTarget.ProductID;
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


        /// <summary>
        /// Reajusta los descuentos de acuerdo al tipo de descuento que este activo
        /// </summary>
        /// <param name="orderContext"></param>
        /// <returns></returns>
        public IOrderContext ReadjusteOrderLineModification(IOrderContext orderContext)
        {
            int promotionId = 0;
            bool cumulative = false;
            Order orderTmp = orderContext.Order.AsOrder();
            switch (NetSteps.Data.Entities.Business.Logic.PromoPromotionTypeConfigurationsLogic.Instance.GetActiveID())
            {
                case (int)NetSteps.Data.Entities.Generated.ConstantsGenerated.PromotionEngineType.DiscountType1:
                    orderTmp = NetSteps.Data.Entities.Business.Logic.PromoPromotionLogic.Instance.ApplyPromotionType1(orderTmp, out promotionId, out cumulative);
                    break;

                case (int)NetSteps.Data.Entities.Generated.ConstantsGenerated.PromotionEngineType.DiscountType2:
                    orderTmp = NetSteps.Data.Entities.Business.Logic.PromoPromotionLogic.Instance.ApplyPromotionType2(orderTmp, out promotionId, out cumulative);
                    break;

                case (int)NetSteps.Data.Entities.Generated.ConstantsGenerated.PromotionEngineType.DiscountType4:
                    orderTmp = NetSteps.Data.Entities.Business.Logic.PromoPromotionLogic.Instance.ApplyPromotionType4(orderTmp, out promotionId, out cumulative);
                    break;
            };

            orderContext.Order = orderTmp;
            return orderContext;
        }

        public virtual void AutoBundleItems(IOrderContext orderContext)
        {
            Contract.Requires<ArgumentNullException>(orderContext != null);
            Contract.Assert(orderContext.Order != null);

            // If the order isn't in a status that should be bundled, then it will not be bundled!
            if (orderContext.Order.AsOrder().IsCommissionable())
            {
                return;
            }

            foreach (var customer in orderContext.Order.AsOrder().OrderCustomers)
            {
                AutoBundleItemsForCustomer(orderContext, customer);
            }
        }

        /// <summary>
        /// Automatically bundles items in the shopping cart.
        /// </summary>
        /// <param name="orderContext">The order context.</param>
        protected virtual void AutoBundleItemsForCustomer(IOrderContext orderContext, OrderCustomer customer)
        {
            Contract.Requires<ArgumentNullException>(customer != null);

            // If the order isn't in a status that should be bundled, then it will not be bundled!
            if (customer.Order.IsCommissionable() || customer.IsTooBigForBundling())
            {
                return;
            }

            try
            {
                var staticKitsBefore = customer.OrderItems
                    .Where(oi => oi.ProductID.HasValue && inventoryRepository.GetProduct(oi.ProductID.Value).IsDynamicKit())
                    .OrderBy(oi => oi.ProductID)
                    .Select(oi => new { oi.ProductID, oi.Quantity })
                    .ToList();

                var resultSet = new List<Product>();
                Parallel.ForEach(orderContext.SortedDynamicKitProducts.Select(kp => (Product)kp), (kitProduct) =>
                    {
                        kitProduct.OrderItems.Clear();
                        if (customer.Order.CanBeDynamicKitUpSold(customer, kitProduct, 0))
                        {
                            lock (resultSet)
                            {
                                resultSet.Add(kitProduct);
                            }
                        }
                    });

                resultSet = resultSet.OrderByDescending(rs => rs.DynamicKits[0].DynamicKitGroups.Sum(kg => kg.MinimumProductCount)).ToList();
                for (int i = 0; i < resultSet.Count; i++)
                {
                    while (customer.Order.CanBeDynamicKitUpSold(customer, resultSet[i], 0))
                    {
                        resultSet[i].OrderItems.Clear();
                        ImplementDynamicKit(orderContext, customer, resultSet[i]);
                    }
                }

                var staticKitsAfter = customer.OrderItems
                    .Where(oi => oi.ProductID.HasValue && inventoryRepository.GetProduct(oi.ProductID.Value).IsDynamicKit())
                    .OrderBy(oi => oi.ProductID)
                    .Select(oi => new { oi.ProductID, oi.Quantity })
                    .ToList();

                if (staticKitsBefore.Count != staticKitsAfter.Count)
                {
                    RemoveInvalidKitsForCustomer(customer);
                    return;
                }

                for (int i = 0; i < staticKitsBefore.Count; i++)
                {
                    if (staticKitsBefore[i].ProductID != staticKitsAfter[i].ProductID || staticKitsBefore[i].Quantity != staticKitsAfter[i].Quantity)
                    {
                        RemoveInvalidKitsForCustomer(customer);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Log(accountID: customer.Order.OrderCustomers[0].AccountID, orderID: customer.OrderID);
                throw;
            }

        }

        private void RemoveInvalidKitsForCustomer(OrderCustomer customer)
        {
            var bundleComponent = Create.New<Bundles.Common.IBundleComponent>();

            var dynamicKits = customer.OrderItems.Where(oi => inventoryRepository.GetProduct(oi.ProductID ?? 0).IsDynamicKit()).ToList();
            foreach (var kit in dynamicKits)
            {
                var childItems = customer.OrderItems.Where(oi => oi.ParentOrderItem == kit);
                bool isValidKit = bundleComponent.IsValidKit(TranslateOrderItemToBundleItem(childItems), kit.ProductID ?? 0);

                if (!isValidKit)
                {
                    RemoveKit(childItems, customer, kit);
                }
            }

            var staticKits = customer.OrderItems.Where(oi => inventoryRepository.GetProduct(oi.ProductID ?? 0).IsStaticKit()).ToList();
            foreach (var kit in staticKits)
            {
                var childItems = customer.OrderItems.Where(oi => oi.ParentOrderItem == kit);
                if (!childItems.Any())
                {
                    RemoveKit(childItems, customer, kit);
                }
            }
        }

        public void RemoveInvalidKits(IOrderContext orderContext)
        {
            Contract.Requires<ArgumentNullException>(orderContext != null);
            Contract.Requires<ArgumentNullException>(orderContext.Order != null);

            foreach (var customer in orderContext.Order.AsOrder().OrderCustomers)
            {
                RemoveInvalidKitsForCustomer(customer);
            }
        }

        private void RemoveKit(IEnumerable<OrderItem> childItems, OrderCustomer customer, OrderItem kitToRemove)
        {
            foreach (OrderItem item in childItems)
            {
                item.ParentOrderItemID = null;
                item.ParentOrderItem = null;
            }

            customer.Order.RemoveItem(customer, kitToRemove);
        }

        private List<IBundleItem> TranslateOrderItemToBundleItem(IEnumerable<OrderItem> orderItems)
        {
            var returnValue = new List<IBundleItem>();

            foreach (OrderItem item in orderItems)
            {
                var toAdd = Create.New<IBundleItem>();
                toAdd.HasAdjustmentOrderLineModifications = item.OrderAdjustmentOrderLineModifications.Any();
                toAdd.IsDynamicKit = false;
                toAdd.IsHostReward = item.IsHostReward;
                toAdd.IsParentStaticKit = false;
                toAdd.IsStaticKit = false;
                toAdd.ProductID = item.ProductID ?? 0;

                var tempProduct = inventoryRepository.GetProduct(item.ProductID ?? 0);
                if (tempProduct.ProductBase == null)
                {
                    tempProduct.ProductBase = ProductBase.Load(tempProduct.ProductBaseID);
                }
                toAdd.ProductTypeID = tempProduct.ProductBase.ProductTypeID;
                toAdd.Quantity = item.Quantity;

                returnValue.Add(toAdd);
            }

            return returnValue;
        }

        /// <summary>
        /// Calculates the totals.
        /// </summary>
        /// <param name="orderContext">The order context.</param>
        protected virtual void CalculateTotals(IOrder order)
        {
            Contract.Requires<ArgumentNullException>(order != null);

            totalsCalculator.CalculateOrder(order.AsOrder());
        }

        /// <summary>
        /// Combines multiple combineable order item quantities into one item.
        /// </summary>
        /// <param name="orderContext">The order context.</param>
        /// <param name="itemsToCombine">The items to combine.</param>
        protected virtual void CombineOrderItems(IOrderContext orderContext, IEnumerable<OrderItem> itemsToCombine)
        {
            Contract.Requires<ArgumentNullException>(orderContext != null);
            Contract.Requires<ArgumentNullException>(itemsToCombine != null);
            Contract.Requires<ArgumentOutOfRangeException>(itemsToCombine.Any());

            var first = itemsToCombine.First();
            for (int i = 1; i < (itemsToCombine.Count()); i++)
            {

                var itemToBeRemoved = itemsToCombine.ElementAt(i);
                if (first.ItemPriceActual.Equals(itemToBeRemoved.ItemPriceActual)) //Luis Peña V. - CSTI
                {
                    Contract.Assert(first.CanBeCombinedWith(itemToBeRemoved), "Attempted to combine order items but they were incompatible.");
                    first.Quantity += itemToBeRemoved.Quantity;
                    itemToBeRemoved.Quantity = 0;
                }
            }
        }

        /// <summary>
        /// Removes any items with quantity 0.
        /// </summary>
        /// <param name="orderContext">The order context.</param>
        protected virtual void ClearDeletedItems(IOrderContext orderContext)
        {
            Contract.Requires<ArgumentNullException>(orderContext != null);

            var order = orderContext.Order.AsOrder();

            foreach (var customer in order.OrderCustomers)
            {
                int pointer = 0;
                while (pointer < customer.OrderItems.Count())
                {
                    var item = customer.OrderItems.ElementAt(pointer);
                    if (item.Quantity == 0 || !item.ProductPriceTypeID.HasValue)
                    {
                        order.RemoveItem(customer, item);
                    }
                    else
                    {
                        // increment pointer only if the order item has not been removed. (i.e. we still want to look at that position)
                        pointer++;
                    }
                }
            }
        }

        /// <summary>
        /// Creates the order item.
        /// </summary>
        /// <param name="orderContext">The order context.</param>
        /// <param name="updateItem">The update item.</param>
        /// <returns></returns>
        protected virtual IOrderItem CreateOrderItem(IOrderContext orderContext, IOrderItemQuantityModification updateItem)
        {
            Contract.Requires<ArgumentNullException>(orderContext != null);
            Contract.Requires<ArgumentNullException>(updateItem != null);

            var product = inventoryRepository.GetProductwithoutCache(updateItem.ProductID);

            return AddItem(orderContext.Order, updateItem.Customer, product, updateItem.Quantity, (short)ConstantsGenerated.OrderItemType.Retail);
        }

        public virtual bool ImplementDynamicKit(IOrderContext orderContext, OrderCustomer customer, Product kitProduct)
        {
            Contract.Requires(orderContext != null);
            Contract.Requires(customer != null);

            if (kitProduct == null)
                return false;

            // create and add item - refactor to not have to cast to OrderItem
            var addedKitItem = (OrderItem)AddKitProduct(orderContext, customer, kitProduct);

            //TODO: Fix this vagueness
            var parentGuid = addedKitItem != null ? addedKitItem.Guid.ToString("N") : "";
            // we clone the dynamic kits in order to get a fresh order item set each time we populate it.
            var dynamicKit = kitProduct.DynamicKits[0];
            int requiredNumberOfProducts = dynamicKit.DynamicKitGroups.Sum(g => g.MinimumProductCount);

            // determine possible kit items
            /* possible items can't be:
             *      a host reward item
             *      an item with existing adjustments
             *      in a kit with greater count of items
             */
            var possibleKitItems =
                customer.OrderItems.Where(
                    item =>
                    !item.IsHostReward && IsInLesserDynamicKit(item, requiredNumberOfProducts)
                    && !item.OrderAdjustmentOrderLineModifications.Any()).ToList();

            // group products dictionary (contains any order items for which this group is satisfied).
            var groupProductsDictionary = new Dictionary<DynamicKitGroup, List<IOrderItem>>();
            foreach (var group in dynamicKit.DynamicKitGroups)
            {
                groupProductsDictionary[group] = possibleKitItems.Where(item => CanBeAddedToDynamicKitGroup(item, group)).Cast<IOrderItem>().ToList();
            }

            CompleteKitGroups(orderContext, addedKitItem, groupProductsDictionary);

            // don't forget to dissolve the lesser absorbed kits.

            return true;
        }

        private IOrderItem AddKitProduct(IOrderContext orderContext, IOrderCustomer orderCustomer, IProduct kitProduct)
        {
            Contract.Requires(orderContext != null);
            Contract.Requires(orderCustomer != null);
            Contract.Requires(kitProduct != null);
            Contract.Ensures(Contract.Result<IOrderItem>() != null);

            var addedKitItemModification = Create.New<IOrderItemQuantityModification>();
            addedKitItemModification.Customer = orderCustomer;
            addedKitItemModification.ModificationKind = OrderItemQuantityModificationKind.Add;
            // this should not be cast - refactor when kitProduct has ProductID
            addedKitItemModification.ProductID = ((Product)kitProduct).ProductID;
            addedKitItemModification.Quantity = 1;
            UpdateOrderItemQuantities(orderContext, new IOrderItemQuantityModification[] { addedKitItemModification });
            var addedKitItem =
                orderCustomer.OrderItems.LastOrDefault(
                    oi =>
                    oi.ProductID.Value == ((Product)kitProduct).ProductID
                    && !orderCustomer.OrderItems.Any(possibleChildItem => possibleChildItem.ParentOrderItem == oi));

            return addedKitItem;
        }

        /// <summary>
        /// Completes the kit groups.
        /// </summary>
        /// <param name="orderContext">The order context.</param>
        /// <param name="kit">The kit.</param>
        /// <param name="groupProducts">The group products.</param>
        /// <returns></returns>
        private void CompleteKitGroups(IOrderContext orderContext, IOrderItem kit, Dictionary<DynamicKitGroup, List<IOrderItem>> groupProducts)
        {
            Contract.Requires(orderContext != null);
            Contract.Requires(kit != null);
            Contract.Requires(groupProducts != null);

            var groupAssignedProducts = new Dictionary<DynamicKitGroup, List<IOrderItem>>();
            while (groupProducts.Keys.Any(group => !groupAssignedProducts.ContainsKey(group) || group.MinimumProductCount > groupAssignedProducts[group].Sum(orderItem => orderItem.Quantity)))
            {
                // find an unsatisfied group
                var unsatisfiedGroup =
                    groupProducts.Keys.First(
                        group =>
                        !groupAssignedProducts.ContainsKey(group)
                            || group.MinimumProductCount > groupAssignedProducts[group].Sum(orderItem => orderItem.Quantity));
                if (!groupAssignedProducts.ContainsKey(unsatisfiedGroup))
                {
                    groupAssignedProducts.Add(unsatisfiedGroup, new List<IOrderItem>());
                }
                var currentCount = groupAssignedProducts[unsatisfiedGroup].Sum(orderItem => orderItem.Quantity);
                foreach (var orderItem in groupProducts[unsatisfiedGroup])
                {
                    if (orderItem.Quantity <= unsatisfiedGroup.MinimumProductCount - currentCount)
                    {
                        // We are removing and readding the item here to address an issue with EF throwing a key constraint if the
                        // child item was added to the context before the parent item.
                        groupAssignedProducts[unsatisfiedGroup].Add(orderItem);
                        var updateItem = Create.New<IOrderItemQuantityModification>();
                        updateItem.Customer = ((OrderItem)orderItem).OrderCustomer;
                        updateItem.ModificationKind = OrderItemQuantityModificationKind.Add;
                        updateItem.ProductID = orderItem.ProductID.Value;
                        updateItem.Quantity = orderItem.Quantity;
                        RemoveOrderItem(updateItem.Customer, orderItem);
                        var newItem = CreateOrderItem(orderContext, updateItem);
                        ((OrderItem)newItem).DynamicKitGroupID = unsatisfiedGroup.DynamicKitGroupID;
                        ((OrderItem)newItem).ParentOrderItem = (OrderItem)kit;
                    }
                    else
                    {
                        var quantityToAttach = unsatisfiedGroup.MinimumProductCount - currentCount;
                        var splitItems = SplitOrderItem(orderContext, orderItem, new int[] { orderItem.Quantity - quantityToAttach, quantityToAttach }).ToList();
                        groupAssignedProducts[unsatisfiedGroup].Add((OrderItem)splitItems[1]);
                        ((OrderItem)splitItems[1]).ParentOrderItem = (OrderItem)kit;
                        ((OrderItem)splitItems[1]).DynamicKitGroupID = unsatisfiedGroup.DynamicKitGroupID;
                    }
                    currentCount = groupAssignedProducts[unsatisfiedGroup].Sum(currentKitItem => currentKitItem.Quantity);
                }
            }
        }

        public virtual bool CanBeAddedToDynamicKitGroup(IOrderItem item, DynamicKitGroup group)
        {
            int productID = item.ProductID.Value;
            Product product = Create.New<InventoryBaseRepository>().GetProduct(productID);

            //dynamic kit cannot be added to another dynamic kit
            if (!product.IsDynamicKit())
            {
                if (group.DynamicKitGroupRules.Any(r => (r.ProductTypeID == product.ProductBase.ProductTypeID || r.ProductID == productID) && r.Include))
                {
                    if (!group.DynamicKitGroupRules.Any(r => (r.ProductID == productID) && !r.Include))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool IsInLesserDynamicKit(OrderItem item, int requiredNumberOfProducts)
        {
            Contract.Requires(item != null);

            var inventoryRepository = Create.New<InventoryBaseRepository>();
            //only try to upgrade meaning existing item is not a dynamic kit, not currently part of dynamic kit, or is part of one that has fewer items
            if (!inventoryRepository.GetProduct(item.ProductID.Value).IsDynamicKit() &&
                (item.ParentOrderItem == null || !inventoryRepository.GetProduct(item.ParentOrderItem.ProductID.Value).IsDynamicKit() ||
                 requiredNumberOfProducts > inventoryRepository.GetProduct(item.ParentOrderItem.ProductID.Value).DynamicKits[0].DynamicKitGroups.Sum(g => g.MinimumProductCount)))
            {
                //don't break up static kit
                if (item.ParentOrderItem == null || !inventoryRepository.GetProduct(item.ParentOrderItem.ProductID.Value).IsStaticKit())
                    return true;

                return false;
            }

            return false;
        }

        /// <summary>
        /// Adds child order items to bundle item already in the cart.
        /// </summary>
        /// <param name="orderContext">The order context.</param>
        /// <param name="bundleItem">The bundle item.</param>
        /// <param name="itemsToAdd">The items to add.</param>
        public virtual void AddOrderItemsToOrderBundle(IOrderContext orderContext, IOrderItem bundleItem, IEnumerable<IOrderItemQuantityModification> itemsToAdd, int dynamicKitGroupID)
        {
            Contract.Requires<ArgumentNullException>(orderContext != null);

            var inventoryRepository = Create.New<InventoryBaseRepository>();
            foreach (var updateItem in itemsToAdd)
            {
                var current = updateItem.Customer.OrderItems.FirstOrDefault(item => item.ProductID == updateItem.ProductID && item.ParentOrderItem == bundleItem);
                var product = inventoryRepository.GetProduct(updateItem.ProductID);
                if (product.IsDynamicKit())
                {
                    throw new InvalidOperationException("Can't add a dynamic kit as a child of a dynamic kit.");
                }

                switch (updateItem.ModificationKind)
                {
                    case OrderItemQuantityModificationKind.SetToQuantity:
                        if (current == null)
                        {
                            var newItem = CreateOrderItem(orderContext, updateItem);
                            ((OrderItem)newItem).DynamicKitGroupID = dynamicKitGroupID;
                            ((OrderItem)newItem).ParentOrderItem = (OrderItem)bundleItem;
                        }
                        else
                        {
                            current.Quantity = updateItem.Quantity;
                        }
                        break;
                    case OrderItemQuantityModificationKind.Add:
                        if (current == null)
                        {
                            var newItem = CreateOrderItem(orderContext, updateItem);
                            ((OrderItem)newItem).DynamicKitGroupID = dynamicKitGroupID;
                            ((OrderItem)newItem).ParentOrderItem = (OrderItem)bundleItem;
                        }
                        else
                        {
                            current.Quantity += updateItem.Quantity;
                        }
                        break;
                    case OrderItemQuantityModificationKind.Delete:
                        if (current != null)
                        {
                            current.Quantity = 0;
                        }
                        break;
                    default:
                        // this adjustment type is not currently supported.
                        throw new NotImplementedException();
                }
            }
        }



        //created overloaded method to allow for passing in a OrderItemUpdateInfo object instead of only a product
        //this allows for user customization of skus
        protected virtual OrderItem GetDuplicateOrderItemOnOrder(OrderCustomer orderCustomer, Product product, short orderItemTypeID, string parentGuid = null, int? dynamicKitGroupId = null)
        {
            return orderCustomer.OrderItems.FirstOrDefault(oi => oi.ProductID == product.ProductID && oi.OrderItemTypeID == orderItemTypeID
                && !product.IsDynamicKit() && oi.DynamicKitGroupID == dynamicKitGroupId && ((parentGuid == null && oi.ParentOrderItem == null)
                || (oi.ParentOrderItem != null && oi.ParentOrderItem.Guid.ToString("N") == parentGuid)));
        }

        protected virtual OrderItem GetDuplicateOrderItemOnOrder(OrderCustomer orderCustomer, int productID, short orderItemTypeID, string parentGuid = null, int? dynamicKitGroupId = null)
        {
            Product product = inventoryRepository.GetProduct(productID);
            return GetDuplicateOrderItemOnOrder(orderCustomer, product, orderItemTypeID, parentGuid, dynamicKitGroupId);
        }

        public int GetAccountOrderCount(int accountID)
        {
            throw new NotImplementedException();
        }

        public void AttachToParty(IOrder order, int partyID)
        {
            order.ParentOrderID = partyID;
        }

        private void ValidateItemQuantity(int quantity, int productID, short orderTypeID)
        {
            Product product = inventoryRepository.GetProduct(productID);

            if (quantity == 0)
            {
                throw new NetStepsBusinessException("Error adding item to order. There is no quantity for product: " + product.Translations.Name())
                {
                    PublicMessage = Translation.GetTerm("NoQuantityError", "Error adding item to order. There is no quantity for product: {0}", product.Translations.Name())
                };
            }

            if (quantity < 0)
            {
                throw new NetStepsBusinessException("Error adding item to order. Invalid quantity for product: " + product.Translations.Name())
                {
                    PublicMessage = Translation.GetTerm("InvalidQuantityError", "Error adding item to order. Invalid quantity for product: {0}", product.Translations.Name())
                };
            }

            if (orderTypeID != (short)Constants.OrderType.ReturnOrder)
            {
                if (product.IsDynamicKit() || product.IsStaticKit())
                {
                    foreach (var item in product.ChildProductRelations)
                    {
                        var inventoryLevel = Product.CheckStock(item.ChildProductID);
                        if (inventoryLevel.IsOutOfStock && (product.ProductBackOrderBehaviorID == (int)Constants.ProductBackOrderBehavior.Hide || product.ProductBackOrderBehaviorID == (int)Constants.ProductBackOrderBehavior.ShowOutOfStockMessage))
                        {
                            throw new NetStepsBusinessException("Error adding item to order. One or more child products are out of stock: {0}" + product.Translations.Name())
                            {
                                PublicMessage = Translation.GetTerm("ChildProductsOutOfStock", "Error adding item to order. One or more child products are out of stock: {0}", product.Translations.Name())
                            };
                        }
                    }
                }
                else
                {
                    var inventoryLevel = Product.CheckStock(productID);
                    if (inventoryLevel.IsOutOfStock && (product.ProductBackOrderBehaviorID == (int)Constants.ProductBackOrderBehavior.Hide || product.ProductBackOrderBehaviorID == (int)Constants.ProductBackOrderBehavior.ShowOutOfStockMessage))
                    {
                        throw new NetStepsBusinessException("Error adding item to order. Item out of stock: {0}" + product.Translations.Name())
                        {
                            PublicMessage = Translation.GetTerm("ItemOutOfStock", "Error adding item to order. Item is out of stock: {0}", product.Translations.Name())
                        };
                    }
                }
            }
        }

        /// <summary>
        /// Processes the payments (charged Credit Cards, ect..) on the order. - JHE
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public virtual SubmitPaymentResponse SubmitPayments(Order order)
        {
            try
            {
                SubmitPaymentResponse response = new SubmitPaymentResponse();
                response.FailureCount = 0; // Must be 0 for all payments for success.

                List<OrderPayment> processedPayments = new List<OrderPayment>();

                // Do not process payments on a template order
                //order.IsTemplate = true;
                if (!order.IsTemplate)
                {
                    response.Success = true;
                    return response;
                }

                // Fail Safe: if there are not customers on the order then don't process.
                if (order.OrderCustomers.Count == 0)
                    throw new NetStepsApplicationException()
                    {
                        OrderID = order.OrderID,
                        PublicMessage = "There are no customers on the order, can't process payments."
                    };

                List<OrderCustomer> orderCustomersToProcess = ValidateEmptyCarts(order);

                SubmitCreditCardPayments(orderCustomersToProcess, processedPayments, order, response);

                SubmitAllRemainingPayments(orderCustomersToProcess, processedPayments, order, response);

                response.Success = this.IsPaidInFull(order);
                if (!response.Success)
                {
                    response.Message = Translation.GetTerm(
                        "TheOrderCouldNotBeSubmitted:ThereisStillAnUnpaidBalanceAdditionalInfo",
                        "The order could not be submitted: There is still an unpaid balance. Additional Information: {0}", response.Message);
                }


                return response;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException, order != null ? order.OrderID.ToIntNullable() : null);
            }
        }

        public virtual void SubmitCreditCardPayments(List<OrderCustomer> orderCustomersToProcess, List<OrderPayment> processedPayments,
                                                                                            Order order, SubmitPaymentResponse response)
        {
            Func<OrderPayment, bool> creditCardPayment = p => p.PaymentTypeID == Constants.PaymentType.CreditCard.ToInt();

            ProcessPaymentsAtOrderCustomerLevel(orderCustomersToProcess, processedPayments, order, response, creditCardPayment);

            ProcessPaymentsAtOrderLevel(orderCustomersToProcess, processedPayments, order, response, creditCardPayment);
        }

        public virtual void SubmitAllRemainingPayments(List<OrderCustomer> orderCustomersToProcess, List<OrderPayment> processedPayments,
                                                                                            Order order, SubmitPaymentResponse response)
        {
            if (IsCreditCardPaymentsSuccessfull(response))
            {
                Func<OrderPayment, bool> nonCreditCardPayment = p => p.PaymentTypeID != (int)Constants.PaymentType.CreditCard;

                ProcessPaymentsAtOrderCustomerLevel(orderCustomersToProcess, processedPayments, order, response, nonCreditCardPayment);

                ProcessPaymentsAtOrderLevel(orderCustomersToProcess, processedPayments, order, response, nonCreditCardPayment);
            }
        }

        public virtual void ProcessPaymentsAtOrderCustomerLevel(List<OrderCustomer> orderCustomersToProcess, List<OrderPayment> processedPayments, Order order, SubmitPaymentResponse response,
            Func<OrderPayment, bool> paymentTypeToProcess)
        {
            foreach (OrderCustomer orderCustomer in orderCustomersToProcess)
            {
                foreach (OrderPayment orderPayment in orderCustomer.OrderPayments)
                {
                    if (paymentTypeToProcess(orderPayment) && !processedPayments.Contains(orderPayment))
                    {
                        SubmitPayment(response, order, orderPayment, orderCustomer);
                        processedPayments.Add(orderPayment);
                    }
                }
            }
        }

        public virtual void ProcessPaymentsAtOrderLevel(List<OrderCustomer> orderCustomersToProcess, List<OrderPayment> processedPayments, Order order, SubmitPaymentResponse response,
             Func<OrderPayment, bool> paymentTypeToProcess)
        {
            foreach (OrderPayment orderPayment in order.OrderPayments)
            {
                if (paymentTypeToProcess(orderPayment) && !processedPayments.Contains(orderPayment))
                {
                    SubmitPayment(response, order, orderPayment);
                    processedPayments.Add(orderPayment);
                }
            }
        }

        public virtual void SubmitPayment(SubmitPaymentResponse response, Order order, OrderPayment orderPayment, OrderCustomer orderCustomer = null)
        {
            if (!orderPayment.ChangeTracker.ChangeTrackingEnabled)
                orderPayment.StartTracking();

            List<OrderPaymentResult> duplicateOrderPaymentResults = OrderPayment.LoadOrderPaymentResultsByOrderPaymentID(orderPayment.OrderPaymentID);

            //ProPay and Auth.Net both use empty and 1 to represent successful payments.
            //Anything else should be considered a fail.
            if (duplicateOrderPaymentResults.Any(x => (String.IsNullOrWhiteSpace(x.ResponseReasonCode) || x.ResponseReasonCode == "1") && String.IsNullOrWhiteSpace(x.ErrorMessage))) //Empty or 1 = success
            {
                return;
            }

            //ProPay and Auth.Net both use empty and 1 to represent successful payments.
            //Anything else should be considered a fail.
            if (duplicateOrderPaymentResults.Any(x => (String.IsNullOrWhiteSpace(x.ResponseReasonCode) || x.ResponseReasonCode == "1") && String.IsNullOrWhiteSpace(x.ErrorMessage))) //Empty or 1 = success
            {
                return;
            }

            // Already been submitted, don't re-submit.
            if (orderPayment.OrderPaymentStatusID != (int)Constants.OrderPaymentStatus.Completed &&
                    orderPayment.OrderPaymentStatusID != (int)Constants.OrderPaymentStatus.Cancelled)
            {
                // If deferred then save deferred amount
                if (orderCustomer != null && orderPayment.IsDeferred)
                    orderPayment.DeferredAmount = orderCustomer.DeferredBalance;

                Constants.GatewayAuthorizationStatus responseStatus = Constants.GatewayAuthorizationStatus.NotSet;

                //IPaymentGateway gateway = PaymentGateways.Payments.GetPaymentGateway(order, orderPayment);
                //if (gateway != null)
                //{
                //    var authResponse = gateway.Charge(orderPayment);
                //    response.Success = authResponse.Success;
                //    if (!response.Success)
                //        response.Message += "\n" + authResponse.Message;
                //    responseStatus = authResponse.GatewayAuthorizationStatus;
                //}
                //else
                //{
                //    response.Message = "Error finding gateway.  GetPaymentGateway returned null.";
                //    response.Success = false;
                //}

                if (!response.Success)
                {
                    if (responseStatus == Constants.GatewayAuthorizationStatus.Declined)
                        orderPayment.OrderPaymentStatusID = (int)Constants.OrderPaymentStatus.Declined;
                    else
                        orderPayment.OrderPaymentStatusID = (int)Constants.OrderPaymentStatus.Pending;

                    response.FailureCount++; // Add 1 failure.
                    if (response.Message.IsNullOrEmpty())
                    {
                        switch (responseStatus)
                        {
                            case Constants.GatewayAuthorizationStatus.Declined:
                                response.Message = "GatewayAuthorizationStatus: Declined";
                                break;
                            case Constants.GatewayAuthorizationStatus.Error:
                                response.Message = "GatewayAuthorizationStatus: Error";
                                break;
                            default:
                                response.Message = "GatewayAuthorizationStatus: Unknown";
                                break;
                        }
                    }

                    if (order.OrderTypeID == (short)Constants.OrderType.AutoshipOrder)
                    {
                        DomainEventQueueItem.AddAutoshipCreditCardFailedEventToQueue(order.OrderID);
                    }
                }
                else
                {
                    // Fail safe: If the payment type is credit card and data are missing
                    // then the credit card may not have been authorized. Or it might be authorized.
                    // We don't know at this point.
                    if (ConfigurationManager.GetAppSetting<bool>("EnableFailSafeCreditCardCode", true) && orderPayment.PaymentTypeID == Constants.PaymentType.CreditCard.ToInt())
                    {
                        List<OrderPaymentResult> orderPaymentResults = OrderPayment.LoadOrderPaymentResultsByOrderPaymentID(orderPayment.OrderPaymentID);

                        if (orderPaymentResults == null || orderPaymentResults.Count == 0)
                        {
                            orderPayment.OrderPaymentStatusID = (int)Constants.OrderPaymentStatus.Pending;
                            response.FailureCount++;
                            response.Message += "\nThe gateway returned a successful authorization but the results are missing. Verify with the processor.";
                        }
                        else if (orderPayment.TransactionID.IsNullOrEmpty())
                        {
                            orderPayment.OrderPaymentStatusID = (int)Constants.OrderPaymentStatus.Pending;
                            response.FailureCount++;
                            response.Message += "\nThe gateway returned a successful authorization but the transaction ID is missing. Verify with the processor.";
                        }
                        else
                            orderPayment.OrderPaymentStatusID = (int)Constants.OrderPaymentStatus.Completed;
                    }
                    else
                        orderPayment.OrderPaymentStatusID = (int)Constants.OrderPaymentStatus.Completed;

                    if (orderPayment.PaymentTypeID == (int)Constants.PaymentType.EFT)
                    {
                        orderPayment.NachaClassType = GetNachaClassType();
                    }
                }
                orderPayment.Save();
            }
        }

        public virtual string GetNachaClassType()
        {
            return "WEB";
        }

        private bool IsCreditCardPaymentsSuccessfull(SubmitPaymentResponse response)
        {
            return response.FailureCount == 0;
        }

        public virtual List<OrderCustomer> ValidateEmptyCarts(Order order)
        {
            // Don't require a current party Hostess to add items to order - JHE
            // Don't require a future hostess to add items to order - cpaulsen
            List<OrderCustomer> orderCustomersToProcess = order.OrderCustomers
                .Where(oc => !(oc.OrderItems.Count == 0
                    && (oc.OrderCustomerTypeID == Constants.OrderCustomerType.Hostess.ToShort() || oc.IsBookingCredit))).ToList();

            // Don't require a parent party Hostess to add items to order (if they are redeeming a booking credit) - JHE
            Party party = null;
            if (order.Parties != null && order.Parties.Count > 0)
            {
                party = order.Parties.FirstOrDefault();
                var parentPartyHostess = party.GetParentPartyHostess();
                if (parentPartyHostess != null && parentPartyHostess.OrderItems.Any(oi => oi.OrderItemTypeID == Constants.OrderItemType.BookingCredit.ToInt()))
                    orderCustomersToProcess = orderCustomersToProcess.Where(oc => oc.AccountID != parentPartyHostess.AccountID).ToList();
            }

            foreach (OrderCustomer orderCustomer in orderCustomersToProcess)
            {
                if (orderCustomer.OrderItems.Count == 0)
                    throw new NetStepsApplicationException(String.Format("There are no items for a customer({0}), can't process payments.", orderCustomer.AccountInfo.FullName))
                    {
                        AccountID = orderCustomer.AccountID.ToIntNullable(),
                        OrderID = order.OrderID,
                        PublicMessage = Translation.GetTerm(String.Format("There are no items for a customer({0}), can't process payments.", orderCustomer.AccountInfo.FullName))
                    };
            }

            return orderCustomersToProcess;
        }

        public void DetachFromParty(IOrder order)
        {
            order.ParentOrderID = null;
        }

        public List<string> GetActivePromotionCodes(int accountID)
        {
            var promoService = Create.New<IPromotionService>();
            var account = accountID != 0 ? Account.LoadSlim(accountID) : new Business.AccountSlimSearchData();
            Predicate<IPromotion> hasCouponCode = p => p.PromotionQualifications.Any(q => q.Value.ExtensionProviderKey == QualificationExtensionProviderKeys.PromotionCodeProviderKey);
            Predicate<IPromotion> validForAccount = p => p.PromotionQualifications.All(q => q.Value.ValidFor("AccountID", accountID));
            Predicate<IPromotion> validForAccountType = p => p.PromotionQualifications.All(q => q.Value.ValidFor("AccountTypeID", account.AccountTypeID));
            var promos = promoService.GetPromotions(p => hasCouponCode(p) && validForAccount(p) && validForAccountType(p));

            return promos.Select(p => p.PromotionQualifications.Values.OfType<IPromotionCodeQualificationExtension>().SingleOrDefault().PromotionCode).ToList();
        }

        protected bool ShouldApplyAdjustmentsToOrder(IOrder order)
        {
            // 104907: Due to an issue brought up in this ticket, we are explicitly restricting return orders from getting order adjustments.			
            switch (order.OrderTypeID)
            {
                case (int)Constants.OrderType.ReturnOrder:
                case (int)Constants.OrderType.AutoshipTemplate:
                case (int)Constants.OrderType.AutoshipOrder:
                case (int)Constants.OrderType.ReplacementOrder:
                    return false;
            }

            return true;
        }

        protected bool ShouldAutoBundleOrder(IOrder order)
        {
            if (!ApplicationContext.Instance.UseDefaultBundling)
            {
                return false;
            }

            switch (order.OrderTypeID)
            {
                case (int)Constants.OrderType.ReturnOrder:
                case (int)Constants.OrderType.AutoshipTemplate:
                case (int)Constants.OrderType.AutoshipOrder:
                case (int)Constants.OrderType.ReplacementOrder:
                    return false;
            }

            return true;
        }

        #region Add/Remove OrderItems

        //created overloaded method to allow for passing in a OrderItemUpdateInfo object instead of only a product
        //this allows for user customization of skus
        public virtual IOrderItem AddItem(IOrder order, IOrderCustomer orderCustomer, IProduct product, int quantity, short orderItemTypeID,
            int? hostRewardRuleId = null, string parentGuid = null, int? dynamicKitGroupId = null)
        {

            var ProductID = ((Product)product).ProductID;

            if (((Order)order).IsCommissionable())
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(new Exception("Unable to add items to orders in a commissionable status."), Constants.NetStepsExceptionType.NetStepsBusinessException, order.OrderID.ToIntNullable());
            }

            try
            {
                #region Validations

                if (((Product)product).IsVariantTemplate)
                {
                    throw new NetStepsBusinessException("Error adding item to order. Product: " + ((Product)product).Translations.Name() + " is a product variant template. Please select a variant product.")
                    {
                        PublicMessage = Translation.GetTerm("ProductDoesNotQualifyForTheBundleError", "Error adding item to order. Product: {0} is a product variant template. Please select a variant product.", ((Product)product).Translations.Name())
                    };
                }

                if (orderCustomer == null || !order.OrderCustomers.Any(oc => oc.AccountID == orderCustomer.AccountID))
                    throw new NetStepsBusinessException("Error Adding item to Order. Order customer doesn't exist on order yet. Please add OrderCustomer to order first.");

                IEnumerable<string> excluded;
                if (!((Product)product).AssertCustomerCanOrderProduct((OrderCustomer)orderCustomer, out excluded))
                {
                    //Disallow Products from being shipped to Excluded State/Provinces
                    var msg = Translation.GetTerm("CustomerCannotOrderProduct", "The product ({0}: {1}) could not be added to the cart due to specific restrictions: item cannot be ordered in the state(s) of {2}.", Translation.GetTerm("sku"), ((Product)product).SKU, String.Join(", ", excluded));
                    throw new NetStepsBusinessException(msg)
                        {
                            PublicMessage = msg
                        };
                }

                // JWL, 2012-01-20, If product has a dynamic kit with a dynamic pricing type, allow zero price items to be added
                if (!((Product)product).ContainsPrice(orderCustomer.AccountTypeID, order.CurrencyID, order.OrderTypeID) && !((Product)product).IsDynamicallyPricedKit())
                    throw new NetStepsBusinessException("Error adding item to order. There is no price for product: " + ((Product)product).Translations.Name())
                    {
                        PublicMessage = Translation.GetTerm("NoPriceError", "Error adding item to order. There is no price for product: {0}", ((Product)product).Translations.Name())
                    };


                //ValidateItemQuantity(quantity, ((Product)product).ProductID, order.OrderTypeID);


                var result = this.IsProductValid(order, product, quantity);
                if (!result.Success)
                    throw new NetStepsBusinessException(result.Message);

                if (!parentGuid.IsNullOrEmpty() && !((Order)order).IsReturnOrder())
                {
                    var valResult = this.CanBeAddedToDynamicKit(orderCustomer, product, quantity, parentGuid, dynamicKitGroupId);
                    if (!valResult.Success)
                    {
                        throw new NetStepsBusinessException(valResult.Message)
                        {
                            PublicMessage = valResult.Message
                        };
                    }
                }

                // Validate Hostess Reward Items if not a return order
                if (!((Order)order).IsReturnOrder())
                {
                    switch (orderItemTypeID)
                    {
                        case (short)ConstantsGenerated.OrderItemType.BookingCredit:
                        case (short)ConstantsGenerated.OrderItemType.PercentOff:
                            var response = ((OrderCustomer)orderCustomer).ValidateHostessRewardItem(quantity, hostRewardRuleId, ((OrderCustomer)orderCustomer).Order);
                            if (!response.Success)
                            {
                                throw new NetStepsBusinessException(response.Message)
                                {
                                    PublicMessage = string.Format(response.Message)
                                };
                            }
                            break;
                        default:
                            break;
                    }
                }
                
                #endregion

                order.CalculationsDirty = true;
                var CurrentProductSKU = ((Product)product).SKU;
                decimal? participationPercentage = null;
                int? materialID;
				int? materialIDs;
                var productName = ((Product)product).Name;
				ProductRelationRepository productRelationRepo = new ProductRelationRepository();
                if (!parentGuid.IsNullOrEmpty() && !((Order)order).IsReturnOrder())// Kit Child
                {
                    materialID = ((Product)product).ProductID;
                    materialIDs = ((Product)product).ProductID;
                    var productID = ((OrderCustomer)orderCustomer).OrderItems.GetByGuid(parentGuid).ProductID;
                    if (productID != null) ProductID = (int)productID;
                    var queryMaterial = PreOrderExtension.GetsMaterialID(materialID.Value);
                    //Valid Stock
                    WarehouseMaterialRepository wareHouseMaterialRepository = new WarehouseMaterialRepository();
                    var existInventory = wareHouseMaterialRepository.FirstOrDefault(x =>x.MaterialID.Value == materialID.Value && x.WarehouseID.Value == order.WareHouseID);
                    var queryWMA = WarehouseMaterialAllocationRepository.Where(o => (o.PreOrderID == order.PreorderID && o.ProductID == ProductID));                       
                    if (existInventory.QuantityAllocated >= existInventory.QuantityOnHand)
                    {
                        var warehouseMaterialID = wareHouseMaterialRepository.FirstOrDefault(p => p.MaterialID == existInventory.MaterialID).WarehouseMaterialID;
                        bool exist = false;
                        foreach (var item in queryWMA)
                        {
                            if (item.WarehouseMaterialID == warehouseMaterialID)
                            {
                                //exist
                                exist = true;
                            }
                        }
                        //Validar si ya estan en WareHouseMaterialAllocations 
                        if (exist == false)
                        {
                            //No hay stock identificar en productRelationReplacement
                            var MaterialId = PreOrderExtension.GetMaterialIdPRR(productRelationRepo.FirstOrDefault(l => l.ParentProductID == ProductID && l.MaterialID == materialID.Value).ProductRelationID);
                            materialID = MaterialId;
                            queryMaterial = PreOrderExtension.GetsMaterialID(materialID.Value);
                        }


                        //var q = WarehouseMaterialAllocationRepository.Where(o => (o.PreOrderID == order.PreorderID && o.ProductID == ProductID && o.WarehouseMaterialID == warehouseMaterialID)).FirstOrDefault().WarehouseMaterialID;
                        //if (q == null)
                        //{
                          
                        //} 
                    }
                    if (queryMaterial.Any())
                    {
                        CurrentProductSKU = queryMaterial.First().SKU;
                        productName = queryMaterial.First().Name;
                    }
                    var queryProductRelation = productRelationRepo.FirstOrDefault(l => l.ParentProductID == ProductID && l.MaterialID == materialIDs.Value);
                    participationPercentage = queryProductRelation.ParticipationPercentage;
                }
                else
                {
                    Constants.ProductRelationType queryIsKit = (Constants.ProductRelationType)PreOrderExtension.GetProductRelationsTypeID(ProductID);
                    materialID = null;
                    if (queryIsKit == Constants.ProductRelationType.Adjunct)
                    {
                        var queryWMA = WarehouseMaterialAllocationRepository.FirstOrDefault(o => (o.PreOrderID == order.PreorderID && o.ProductID == ((Product)product).ProductID));
                        if (queryWMA == null)
                        {
                            if ((order.ParentOrderID == null || order.ParentOrderID == 1) && ((order.OrderStatusID == (int)ConstantsGenerated.OrderStatus.Pending)
                                || (order.OrderStatusID == (int)ConstantsGenerated.OrderStatus.NotSet)))
                            {

                                var objAddLineValidStock = PreOrderExtension.GetAddLineValidStock(order.WareHouseID, quantity, ((Product)product).ProductID, order.PreorderID, order.OrderCustomers[0].AccountTypeID);
                                if (objAddLineValidStock.Any())
                                {
                                    materialID = objAddLineValidStock.First().materialID;
                                }
                                else
                                {
                                    throw new NetStepsBusinessException("Error Adding item to Order. Not warehouse allocations found");
                                }
                            }
                        }
                        else
                        {
                            materialID = WarehouseMaterialRepository.FirstOrDefault(p => p.WarehouseMaterialID == queryWMA.WarehouseMaterialID).MaterialID;
                        }
                    }
                }


                var orderItem = new OrderItem()
                {
                    OrderCustomerID = ((OrderCustomer)orderCustomer).OrderCustomerID,
                    ProductID = ProductID,
                    OrderItemTypeID = orderItemTypeID,
                    Quantity = quantity,
                    ProductName = productName,
                    SKU = CurrentProductSKU,
                    ChargeShipping = ((Product)product).ProductBase.ChargeShipping,
                    ChargeTax = ((Product)product).ProductBase.ChargeTax,
                    //if the item type is anything other than retail or not set, then it is a discount item. In which case we have to use retail pricing.
                    ProductPriceTypeID = ((orderItemTypeID == (short)ConstantsGenerated.OrderItemType.Retail) ||
                        (orderItemTypeID == (short)ConstantsGenerated.OrderItemType.NotSet)) ? ((OrderCustomer)orderCustomer).ProductPriceTypeID : (int)ConstantsGenerated.ProductPriceType.Retail,
                    DynamicKitGroupID = dynamicKitGroupId,
                    OrderCustomer = (OrderCustomer)orderCustomer,
                    HostessRewardRuleID = hostRewardRuleId,
                    HasChanges = true,
                    MaterialID = materialID,
                    ParticipationPercentage = participationPercentage
                };

                orderItem.StartTracking();
                ((OrderCustomer)orderCustomer).StartTracking();

                if (!parentGuid.IsNullOrEmpty())
                {
                    // orderItem.ParentOrderItemID = ((OrderCustomer) orderCustomer).OrderItems.GetByGuid(parentGuid).OrderItemID;
                    ((OrderCustomer)orderCustomer).OrderItems.GetByGuid(parentGuid).ChildOrderItems.Add(orderItem);
                }
                orderCustomer.OrderItems.Add(orderItem);

                // Ensure that all child kit items for StaticKit are added to the Order - JHE
                // Unless we are a return order, in which case we want to have the kit children added somewhere else.
                if (((Product)product).IsStaticKit() && !((OrderCustomer)orderCustomer).Order.IsReturnOrder()
                        && parentGuid.IsNullOrEmpty())
                {
                    var kitChildProductRelations = ((Product)product).ChildProductRelations.Where(cpr => cpr.ProductRelationsTypeID == (int)ConstantsGenerated.ProductRelationsType.Kit).GroupBy(k => k.MaterialID);
                    try
                    {
                        foreach (var childRelationGrp in kitChildProductRelations)
                        {
                            order.AsOrder().AddOrUpdateOrderItem(((OrderCustomer)orderCustomer), new List<OrderItemUpdateInfo>() { new OrderItemUpdateInfo()
                {
                                ProductID = (int) childRelationGrp.Key, 
                                Quantity = (childRelationGrp.Count() * quantity), 
                                OverrideQuantity = false,                                
                            } }, false, parentGuid: orderItem.Guid.ToString("N"), dynamicKitGroupId: dynamicKitGroupId);

                        }
                    }
                    catch (Exception ex)
                    {

                        this.RemoveOrderItem(orderCustomer, orderItem);
                        throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException, order.OrderID.ToIntNullable(), orderCustomer != null ? orderCustomer.AccountID.ToIntNullable() : null);

                    }
                }


                if (ApplicationContext.Instance.UseDefaultBundling)
                {
                    ((OrderCustomer)orderCustomer).NeedsAutoBundling = true;
                }

                return orderItem;
            }
            catch (Exception ex)
            {

                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException, order.OrderID.ToIntNullable(), orderCustomer != null ? orderCustomer.AccountID.ToIntNullable() : null);
            }
        }


        public virtual void AddKitItemPrices(IOrder order)
        {
            KitItemPriceRepository kitItemPricesRepository = new KitItemPriceRepository();
            OrderItemPriceRepository oiPriceRepository = new OrderItemPriceRepository();

            var queryOrderItems = order.OrderCustomers.First().OrderItems;
            var lstKitItemPrices = new List<KitItemPrice>();
            var queryKits = queryOrderItems.Where(n => ((OrderItem)n).ChildOrderItems.Any());
            foreach (var orderItem in queryKits)
            {
                var queryOderItemPrices = oiPriceRepository.Where(u => u.OrderItemID == orderItem.OrderItemID);
                foreach (var oiChild in ((OrderItem)orderItem).ChildOrderItems)
                {
                    var queryKip = kitItemPricesRepository.Where(n => n.OrderItemID == oiChild.OrderItemID);
                    if (!queryKip.Any())
                    {
                        foreach (var oip in queryOderItemPrices)
                        {
                            var o = oip.OriginalUnitPrice * oiChild.ParticipationPercentage / 100;
                            if (o != null)
                                lstKitItemPrices.Add(new KitItemPrice()
                                {
                                    OrderItemID = oiChild.OrderItemID,
                                    OriginalUnitPrice = Math.Round((Decimal)o, 2),
                                    ProductPriceTypeID = oip.ProductPriceTypeID,
                                    UnitPrice = Math.Round((Decimal)(oip.UnitPrice * oiChild.ParticipationPercentage / 100), 2),// (decimal)(oip.UnitPrice * oiChild.ParticipationPercentage / 100)
                                });
                        }
                    }
                }
            }

            kitItemPricesRepository.SaveBatch(lstKitItemPrices);
        }

        public virtual void AddOrUpdateOrderItem(IOrder order, IOrderCustomer customer, IEnumerable<OrderItemUpdateInfo> productUpdates, bool overrideQuantity, string parentGuid = null, int? dynamicKitGroupId = null)
        {
            if (customer == null)
            {
                throw new NetStepsBusinessException("Invalid Customer")
                {
                    PublicMessage = Translation.GetTerm("InvalidCustomer", "Invalid Customer")
                };
            }

            OrderItem orderItem = null;
            OrderCustomer castCustomer = (OrderCustomer)customer;

            if (ShouldConsolidateItems())
                ConsolidateOrderItems(castCustomer, parentGuid, dynamicKitGroupId);

            OrderShipment orderShipment = castCustomer.OrderShipments.FirstOrDefault();
            if (orderShipment == null)
            {
                orderShipment = order.AsOrder().GetDefaultShipment();
            }

            foreach (var productUpdateTemp in productUpdates)
            {
                var productUpdate = productUpdateTemp;
                productUpdate.OverrideQuantity = overrideQuantity;


                var product = inventoryRepository.GetProduct(productUpdate.ProductID);
                bool allowBackorder = (new List<int> { (int)Constants.ProductBackOrderBehavior.AllowBackorder, (int)Constants.ProductBackOrderBehavior.AllowBackorderInformCustomer })
                        .Contains(product.ProductBackOrderBehaviorID);

                InventoryLevels inventoryLevel = null;
                if (parentGuid == null)
                {
                    inventoryLevel = Product.CheckStock(productUpdate.ProductID, orderShipment);
                    if (!product.Active)
                    {
                        throw new NetStepsBusinessException(string.Format("The product you ordered ({0}) is not active.", product.Translations.Name()))
                        {
                            PublicMessage = Translation.GetTerm("TheProductYouOrderedIsNotActive", "The product you ordered ({0}) is not active.", product.Translations.Name())
                        };
                    }

                    if (inventoryLevel != null && inventoryLevel.IsOutOfStock && !allowBackorder && product.ProductBase.IsShippable)
                    {
                        throw new NetStepsBusinessException(string.Format("The product you tried to order ({0}) is out of stock.", product.Translations.Name()))
                        {
                            PublicMessage = Translation.GetTerm("TheProductYouTriedToOrderIsOutOfStock", "The product you tried to order ({0}) is out of stock.", product.Translations.Name())
                        };
                    }

                }

                if (UpdateProductOnCurrentOrder(castCustomer, productUpdate, parentGuid, dynamicKitGroupId))
                {
                    var matchingOrderItems = castCustomer.OrderItems.Where(oi => oi.ProductID.Value == productUpdate.ProductID && oi.DynamicKitGroupID == dynamicKitGroupId && ((parentGuid == null && oi.ParentOrderItem == null) || (oi.ParentOrderItem != null && oi.ParentOrderItem.Guid.ToString("N") == parentGuid)));
                    orderItem = matchingOrderItems.SingleOrDefault(item => !item.OrderAdjustmentOrderLineModifications.Any());
                    if (orderItem == null)
                        orderItem = matchingOrderItems.FirstOrDefault();
                    if (productUpdate.Quantity == 0)
                    {
                        order.RemoveItem(orderItem);
                        continue;
                    }
                    int currentQTYOrderedForProduct = GetCurrentQTYOrderedForProduct(order.AsOrder(), productUpdate.ProductID, castCustomer, (OrderItem)orderItem);
                    var newQuantity = currentQTYOrderedForProduct + productUpdate.Quantity - (productUpdate.OverrideQuantity ? orderItem.Quantity : 0);
                    if (inventoryLevel != null && inventoryLevel.QuantityAvailable.HasValue && newQuantity > inventoryLevel.QuantityAvailable && !allowBackorder)
                    {
                        throw new NetStepsBusinessException(string.Format("The product you ordered ({0}) only has {1} unit(s) left in stock", product.Translations.Name(), inventoryLevel.QuantityAvailable.Value))
                        {
                            PublicMessage = Translation.GetTerm("TheProductYouOnlyHasXUnitsInStock", "The product you ordered ({0}) only has {1} unit(s) left in stock", product.Translations.Name(), inventoryLevel.QuantityAvailable.Value)
                        };
                    }

                    UpdateItem(order, customer, orderItem, productUpdate.OverrideQuantity ? productUpdate.Quantity : orderItem.Quantity + productUpdate.Quantity);
                }
                else
                {
                    int currentQTYOrderedForProduct = GetCurrentQTYOrderedForProduct(order.AsOrder(), productUpdate.ProductID, castCustomer, orderItem);
                    if (inventoryLevel != null && inventoryLevel.QuantityAvailable.HasValue && (currentQTYOrderedForProduct + productUpdate.Quantity) > inventoryLevel.QuantityAvailable && !allowBackorder)
                    {
                        throw new NetStepsBusinessException(string.Format("The product you ordered ({0}) only has {1} unit(s) left in stock", product.Translations.Name(), inventoryLevel.QuantityAvailable.Value))
                        {
                            PublicMessage = Translation.GetTerm("TheProductYouOnlyHasXUnitsInStock", "The product you ordered ({0}) only has {1} unit(s) left in stock", product.Translations.Name(), inventoryLevel.QuantityAvailable.Value)
                        };
                    }

                    orderItem = (OrderItem)AddItem(order, customer, product, productUpdate.Quantity, (short)ConstantsGenerated.OrderItemType.Retail, parentGuid: parentGuid, dynamicKitGroupId: dynamicKitGroupId);
                }
            }

            castCustomer.RemoveAllPayments();
        }


        public virtual void UpdateItem(IOrder order, IOrderCustomer orderCustomer, IOrderItem orderItem, int quantity, decimal? itemPriceOverride = null, decimal? itemCVOverride = null, bool wholesaleOverride = false)
        {
            if (order.AsOrder().IsCommissionable())
                throw EntityExceptionHelper.GetAndLogNetStepsException(new Exception("Unable to update items on orders in a commissionable status."), Constants.NetStepsExceptionType.NetStepsBusinessException, order.OrderID.ToIntNullable());

            orderItem.Quantity = quantity;
            //ValidateItemQuantity(quantity, orderItem.ProductID.ToInt(), order.OrderTypeID);
            ((OrderItem)orderItem).HasChanges = true;
            order.CalculationsDirty = true;

            var product = inventoryRepository.GetProduct(orderItem.ProductID.Value);
            if (itemPriceOverride.HasValue && itemCVOverride.HasValue)
            {
                if (orderItem.ItemPrice != itemPriceOverride.Value)
                {
                    ((OrderItem)orderItem).ItemPriceActual = itemPriceOverride.Value;
                }
                else if (orderItem.ItemPrice == ((OrderItem)orderItem).ItemPriceActual)
                {
                    ((OrderItem)orderItem).ItemPriceActual = null;
                }

                ((OrderItem)orderItem).CommissionableTotalOverride = itemCVOverride.Value;
            }

            if (ApplicationContext.Instance.UseDefaultBundling)
            {
                ((OrderCustomer)orderCustomer).NeedsAutoBundling = true;
            }
        }

        #endregion

        #region OrderItem Helpers

        protected virtual bool ShouldConsolidateItems()
        {
            return true;
        }

        public virtual BasicResponse IsProductValid(IOrder order, IProduct product, int quantity)
        {
            return new BasicResponse() { Success = true };
        }

        protected virtual void ValidateOrderItem(Order order, OrderCustomer orderCustomer, ConstantsGenerated.OrderItemType orderItemType, int? hostRewardRuleId, string parentGuid, int? dynamicKitGroupId, Product product, int quantity)
        {
            if (product.IsVariantTemplate)
            {
                throw new NetStepsBusinessException("Error adding item to order. Product: " + product.Translations.Name() +
                    " is a product variant template. Please select a variant product.")
                {
                    PublicMessage = Translation.GetTerm("ProductDoesNotQualifyForTheBundleError",
                        "Error adding item to order. Product: {0} is a product variant template. Please select a variant product.",
                        product.Translations.Name())
                };
            }

            // Make sure the orderCustomer passed in is in the Order.OrderCustomers - JHE
            bool orderCustomerExistsOnOrder = orderCustomer != null && (order.OrderCustomers.Count(oc => oc.AccountID == orderCustomer.AccountID) >= 1);
            if (!orderCustomerExistsOnOrder)
                throw new NetStepsBusinessException("Error Adding item to Order. Order customer doesn't exist on order yet. Please add OrderCustomer to order first.");

            // JWL, 2012-01-20, If product has a dynamic kit with a dynamic pricing type, allow zero price items to be added
            if (!product.ContainsPrice(orderCustomer.AccountTypeID, order.CurrencyID, order.OrderTypeID) && !product.IsDynamicallyPricedKit())
                throw new NetStepsBusinessException("Error adding item to order. There is no price for product: " + product.Translations.Name())
                {
                    PublicMessage = Translation.GetTerm("NoPriceError", "Error adding item to order. There is no price for product: {0}", product.Translations.Name())
                };

            //ValidateItemQuantity(quantity, product.ProductID, order.OrderTypeID);

            var result = this.IsProductValid(order, product, quantity);
            if (!result.Success)
                throw new NetStepsBusinessException(result.Message);

            if (!parentGuid.IsNullOrEmpty() && !order.IsReturnOrder())
            {
                var valResult = this.CanBeAddedToDynamicKit(orderCustomer, product, quantity, parentGuid, dynamicKitGroupId);
                if (!valResult.Success)
                {
                    throw new NetStepsBusinessException(valResult.Message) { PublicMessage = valResult.Message };
                }
            }

            // Validate Hostess Reward Items if not a return order
            if (!order.IsReturnOrder())
            {
                switch (orderItemType)
                {
                    case ConstantsGenerated.OrderItemType.BookingCredit:
                    case ConstantsGenerated.OrderItemType.PercentOff:
                        var response = orderCustomer.ValidateHostessRewardItem(quantity, hostRewardRuleId, orderCustomer.Order);
                        if (!response.Success)
                        {
                            throw new NetStepsBusinessException(response.Message) { PublicMessage = string.Format(response.Message) };
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        //created overloaded method to allow for passing in a OrderItemUpdateInfo object instead of only a product
        //this allows for user customization of skus
        protected virtual OrderItem GetDuplicateOrderItemOnOrder(OrderCustomer orderCustomer, Product product, Constants.OrderItemType orderItemType, string parentGuid = null, int? dynamicKitGroupId = null)
        {
            return orderCustomer.OrderItems.FirstOrDefault(oi => oi.ProductID == product.ProductID && oi.OrderItemTypeID == (int)orderItemType
                && !product.IsDynamicKit() && oi.DynamicKitGroupID == dynamicKitGroupId && ((parentGuid == null && oi.ParentOrderItem == null)
                || (oi.ParentOrderItem != null && oi.ParentOrderItem.Guid.ToString("N") == parentGuid)));
        }

        protected virtual OrderItem GetDuplicateOrderItemOnOrder(OrderCustomer orderCustomer, OrderItemUpdateInfo productUpdate, Constants.OrderItemType orderItemType, string parentGuid = null, int? dynamicKitGroupId = null)
        {
            Product product = inventoryRepository.GetProduct(productUpdate.ProductID);
            return GetDuplicateOrderItemOnOrder(orderCustomer, product, orderItemType, parentGuid, dynamicKitGroupId);
        }

        protected virtual bool UpdateProductOnCurrentOrder(OrderCustomer customer, OrderItemUpdateInfo productUpdate, string parentGuid = null, int? dynamicKitGroupId = null)
        {
            var retVal = false;
            if (customer.OrderItems.Any(oi => !oi.IsHostReward && oi.ProductID == productUpdate.ProductID && !inventoryRepository.GetProduct(oi.ProductID.Value).IsDynamicKit() &&
                    oi.DynamicKitGroupID == dynamicKitGroupId && ((parentGuid == null && oi.ParentOrderItem == null) || (oi.ParentOrderItem != null && oi.ParentOrderItem.Guid.ToString("N") == parentGuid))))
                retVal = true;

            return retVal;
        }

        /// <summary>
        /// Recursively search throuh all OrderCustomers' OrderItems & ChildOrderItems
        /// </summary>
        //Added overloaded method to allow for passing a customer so totals will calculate correctly for party orders
        protected int GetCurrentQTYOrderedForProduct(Order order, int productID, OrderCustomer customer, OrderItem orderItem = null)
        {
            int result = 0;

            if (orderItem == null)
            {
                foreach (var oi in order.OrderCustomers.SelectMany(oc => oc.OrderItems).Where(oi => oi.ProductID == productID))
                {
                    if (customer != null)
                    {
                        if (oi.OrderCustomer == customer)
                            result += GetCurrentQTYOrderedForProduct(order, productID, customer, oi);
                    }
                    else
                        result += GetCurrentQTYOrderedForProduct(order, productID, customer, oi);
                }
            }
            else
            {
                result += orderItem.Quantity;
                foreach (var childItem in orderItem.ChildOrderItems.Where(oi => oi.ProductID == productID))
                {
                    result += GetCurrentQTYOrderedForProduct(order, productID, customer, childItem);
                }
            }

            return result;
        }

        /// <summary>
        /// Recursively search throuh all OrderCustomers' OrderItems & ChildOrderItems
        /// </summary>
        //Added overloaded method to allow for passing a customer so totals will calculate correctly for party orders
        protected int GetCurrentQTYOrderedForProduct(Order order, int productID, OrderItem orderItem = null)
        {
            return GetCurrentQTYOrderedForProduct(order, productID, null, null);
        }

        #endregion

        public virtual bool IsDynamicKitValid(IOrderItem orderItem)
        {
            var product = inventoryRepository.GetProduct(orderItem.ProductID.Value);
            if (product.IsDynamicKit())
            {
                var dynamicKit = product.DynamicKits[0];

                //dynamic kit cannot have child dynamic kits
                if (((OrderItem)orderItem).ChildOrderItems.Any(c => inventoryRepository.GetProduct(c.ProductID.Value).IsDynamicKit()))
                {
                    return false;
                }

                foreach (var group in dynamicKit.DynamicKitGroups)
                {
                    if (!IsDynamicKitGroupValid(orderItem, group))
                    {
                        return false;
                    }

                    foreach (var childItem in ((OrderItem)orderItem).ChildOrderItems.Where(c => c.DynamicKitGroupID.Value == group.DynamicKitGroupID))
                    {
                        if (group.DynamicKitGroupRules.Any(r => (r.ProductID == childItem.ProductID) && !r.Include))
                        {
                            return false;
                        }
                        var childProduct = inventoryRepository.GetProduct(childItem.ProductID.Value);
                        if (!group.DynamicKitGroupRules.Any(r => (r.ProductTypeID == childProduct.ProductBase.ProductTypeID || r.ProductID == childProduct.ProductID) && r.Include))
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        public virtual bool IsDynamicKitGroupValid(IOrderItem kitOrderItem, DynamicKitGroup dynamicKitGroup)
        {
            var currentCount = ((OrderItem)kitOrderItem).ChildOrderItems.Where(oi => oi.DynamicKitGroupID == dynamicKitGroup.DynamicKitGroupID).Sum(oi => oi.Quantity);
            return currentCount >= dynamicKitGroup.MinimumProductCount && currentCount <= dynamicKitGroup.MaximumProductCount;
        }

        public virtual BasicResponse CanBeAddedToDynamicKit(IOrderCustomer orderCustomer, IProduct product, int quantity, string parentGuid, int? dynamicKitGroupId)
        {
            var response = new BasicResponse { Success = true };
            var genericFailure = new BasicResponse
            {
                Success = false,
                Message = Translation.GetTerm("ProductDoesNotQualifyForTheBundleError", "Error adding item to order. Product: {0} does not qualify for the bundle", ((Product)product).Translations.Name())
            };

            if (parentGuid.IsNullOrEmpty())
            {
                return response;
            }

            var orderItem = ((OrderCustomer)orderCustomer).OrderItems.GetByGuid(parentGuid);
            var parentProduct = inventoryRepository.GetProduct(orderItem.ProductID.Value);
            if (parentProduct.IsDynamicKit())
            {
                if (((Product)product).IsDynamicKit())
                {
                    //dynamic kit cannot be added to another dynamic kit
                    return genericFailure;
                }

                var dynamicKit = parentProduct.DynamicKits[0];
                var dynamicKitGroup = dynamicKit.DynamicKitGroups.FirstOrDefault(g => g.DynamicKitGroupID == dynamicKitGroupId);
                if (dynamicKitGroup == null)
                {
                    return new BasicResponse
                    {
                        Success = false,
                        Message = Translation.GetTerm("GroupDoesNotExist", "That group does not exist.")
                    };
                }

                if (dynamicKitGroup.DynamicKitGroupRules.Any(r => (r.ProductID == ((Product)product).ProductID) && !r.Include))
                {
                    return genericFailure;
                }

                if (!dynamicKitGroup.DynamicKitGroupRules.Any(r => (r.ProductTypeID == product.ProductBase.ProductTypeID || r.ProductID == ((Product)product).ProductID) && r.Include))
                {
                    return genericFailure;
                }

                int currentCount = orderItem.ChildOrderItems.Where(oi => oi.DynamicKitGroupID == dynamicKitGroupId).Sum(oi => oi.Quantity);
                if (currentCount >= dynamicKitGroup.MaximumProductCount)
                {
                    return new BasicResponse
                    {
                        Success = false,
                        Message = Translation.GetTerm("TheItemCouldNotBeAddedBecauseTheGroupIsFull", "The product could not be added because the group is full.")
                    };
                }

                if ((currentCount + quantity) > dynamicKitGroup.MaximumProductCount)
                {
                    return new BasicResponse
                    {
                        Success = false,
                        Message = Translation.GetTerm("TheItemCouldNotBeAddedBecauseTheGroupIsAlmostFull", "The product could not be added because the group only has room for {0} more items.", dynamicKitGroup.MaximumProductCount - currentCount)
                    };
                }
            }

            return response;
        }

        public virtual bool IsStaticKitValid(IOrderItem orderItem)
        {
            var product = inventoryRepository.GetProduct(orderItem.ProductID.Value);
            if (product.IsStaticKit())
            {
                //var kitChildProductRelations = product.ChildProductRelations.Where(cpr => cpr.ProductRelationsTypeID == (int)Constants.ProductRelationsType.Kit);
                //if (((OrderItem)orderItem).ChildOrderItems.Sum(c => c.Quantity) != (kitChildProductRelations.Count() * orderItem.Quantity))
                //{
                //    return false;
                //}
                //foreach (var childRelation in kitChildProductRelations)
                //{
                //    if (!((OrderItem)orderItem).ChildOrderItems.Any(oi => oi.ProductID == childRelation.Product.ProductID))
                //    {
                //        return false;
                //    }
                //}
            }

            return true;
        }

        public virtual bool IsOrderValidForSubmission(IOrder order, BasicResponse response)
        {
            // Fail safe for not allowing grand total and payment amounts to differ.
            //if (order.AsOrder().Balance > 0)
            //{
            //    response.Message = "Total payment amounts do not equal the Grand Total. Please verify your order.";
            //    response.Success = false;
            //    return response.Success;
            //}

            if (order.AsOrder().Balance < 0)
            {
                response.Message = "Total charged on order is greater than the Grand Total. Please fix order or contact customer service for help.";
                response.Success = false;
                return response.Success;
            }

            // Fail Safe: if there are no customers or items on the order then don't process. BFH - 20090611
            if (order.OrderCustomers.Count == 0)
            {
                response.Message = "There are no customers. Please verify your order.";
                response.Success = false;
                return response.Success;
            }

            if (!ValidateOrderShipmentShippingMethod(order.AsOrder(), response))
            {
                return response.Success;
            }

            // Make sure customer data has persisted.
            foreach (OrderCustomer orderCustomer in order.OrderCustomers)
            {
                // Fail Safe: if there are no items on the order then don't process. BFH - 20090611
                if (orderCustomer.OrderItems.Count == 0 && order.AsOrder().OrderShipments[0].State == "ND")
                {
                    response.Message = "There are no items on a customer. Please verify your order.";
                    response.Success = false;
                    return response.Success;
                }
            }

            return true;
        }

        protected bool ValidateOrderShipmentShippingMethod(Order order, BasicResponse response)
        {
            foreach (OrderShipment orderShipment in order.OrderShipments)
            {
                if (orderShipment.IsShippable())
                {
                    if (!orderShipment.ShippingMethodID.HasValue)
                    {
                        response.Success = false;
                        response.Message = Translation.GetTerm("NoShippingMethod", CustomValidationMessages.NoShippingMethod);
                        return false;
                    }

                    if (!orderShipment.ValidateShippingMethod(response))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Default Currency to the Currency of the default Shipping Address - JHE
        /// </summary>
        /// <param name="order"></param>
        public virtual void SetCurrencyID(IOrder order)
        {
            order.CurrencyID = this.ReturnCurrencyID(order);
        }

        public virtual int ReturnCurrencyID(IOrder order)
        {
            var shipment = GetDefaultShipmentNoDefault(order);

            if (shipment != null && shipment.CountryID != 0)
            {
                var country = SmallCollectionCache.Instance.Countries.GetById(shipment.CountryID);
                return country.CurrencyID;
            }

            if (order.OrderCustomers != null && order.OrderCustomers[0].AccountID != 0)
            {
                return order.AsOrder().OrderCustomers[0].MarketDefaultCurrencyID;
            }

            var returnCountry = SmallCollectionCache.Instance.Countries.GetById((int)Constants.Country.UnitedStates);
            return returnCountry.CurrencyID;
        }


        /// <summary>
        /// Sets the OrderNumber property for new orders according to the numbering pattern defined by the client.
        /// Just setting it to the same number as the OrderID by default. May cause order to save first to get the 
        ///     value of OrderID set by the identity. - JHE
        /// </summary>
        /// <param name="order"></param>
        public virtual void GenerateAndSetNewOrderNumber(IOrder order, bool saveOrder = true)
        {
            // note: the usage of the Temp GUID should be deprecated now, but we should still check for it just in case - CJH
            if (order.AsOrder().OrderNumber.IsNullOrEmpty() || order.AsOrder().OrderNumber.ToCleanString().ToLower().StartsWith("Temp".ToLower()))
            {
                if (order.OrderID == 0 && saveOrder)
                    order.AsOrder().BaseSave();

                if (order.OrderID != 0)
                    order.AsOrder().OrderNumber = order.OrderID.ToString();
            }
        }

        public virtual OrderShipment GetDefaultShipment(Order order)
        {
            OrderShipment orderShipment = GetDefaultShipmentNoDefault(order);

            // They don't have a shipment; creating one. - JHE
            if (orderShipment == null)
            {
                orderShipment = new OrderShipment();

                if (order.OrderTypeID == (short)Constants.OrderType.PartyOrder || order.OrderTypeID == (short)Constants.OrderType.FundraiserOrder)
                {
                    order.OrderShipments.Add(orderShipment);
                }
                else if (order.OrderCustomers.Count == 1)
                {
                    order.OrderCustomers[0].OrderShipments.Add(orderShipment);
                    order.OrderShipments.Add(order.OrderCustomers[0].OrderShipments.LastOrDefault());
                }
            }

            if (orderShipment.CountryID == 0)
            {
                var country = Country.GetCountries().FirstOrDefault(c => c.MarketID == order.ConsultantInfo.MarketID && c.Active);
                orderShipment.CountryID = country != null ? country.CountryID : (int)Constants.Country.UnitedStates;
            }

            return orderShipment;
        }

        public virtual OrderShipment GetDefaultShipmentNoDefault(IOrder order)
        {
            // TODO: Review this logic for accuracy later - JHE
            if (order.AsOrder().OrderShipments.Count == 1)
                return order.AsOrder().OrderShipments[0];

            foreach (OrderShipment shipment in order.AsOrder().OrderShipments)
            {
                if (shipment.OrderCustomer == null && shipment.IsDirectShipment == false)
                    return shipment;
            }

            if (order.AsOrder().OrderShipments.Count > 1)
            {
                return order.AsOrder().OrderShipments[0];
            }

            return null;
        }

        public virtual bool IsReturnable(IOrder order)
        {
            bool result = false;

            // need to check to see if there are any return orders for this order and if there are any returnable items left on the order.
            if (!IsOrderFullyReturned(order) && order.OrderTypeID != Constants.OrderType.ReturnOrder.ToInt())
            {
                //@at-005
                switch ((Constants.OrderStatus)order.OrderStatusID)
                {
                    case Constants.OrderStatus.CancelledPaid:
                    case Constants.OrderStatus.Invoiced: //@
                    case Constants.OrderStatus.Printed: //@
                    case Constants.OrderStatus.Embarked: //@
                    case Constants.OrderStatus.Shipped: //@
                    case Constants.OrderStatus.Delivered: //@
                        result = true;
                        break;
                }

                //if (order.OrderStatusID == (short)Constants.OrderStatus.CancelledPaid || order.OrderStatusID == Constants.OrderStatus.Shipped.ToInt())
                //{
                //    result = true;
                //}
            }

            return result;
        }

        public virtual bool IsCancellable(IOrder order)
        {
            bool result = false;

            if (order.OrderTypeID != Constants.OrderType.ReturnOrder.ToInt())
            {
                switch ((Constants.OrderStatus)order.OrderStatusID)
                {
                    case ConstantsGenerated.OrderStatus.Pending:
                    case ConstantsGenerated.OrderStatus.Paid:
                        result = true;
                        break;
                }
            }
            else if (order.OrderTypeID == Constants.OrderType.ReturnOrder.ToInt())
            {
                switch ((Constants.OrderStatus)order.OrderStatusID)
                {
                    case ConstantsGenerated.OrderStatus.Pending:
                        result = true;
                        break;
                }
            }

            return result;
        }

        public virtual bool IsEditable(IOrder order)
        {
            bool result = false;

            switch ((Constants.OrderStatus)order.OrderStatusID)
            {
                case ConstantsGenerated.OrderStatus.Pending:
                case ConstantsGenerated.OrderStatus.PartiallyPaid:
                case ConstantsGenerated.OrderStatus.CreditCardDeclined:
                    result = true;
                    break;
            }

            return result;
        }

        public virtual bool HasConsultantOnOrder(IOrder order)
        {
            bool result = false;

            result = order.OrderCustomers.Any(oc => oc.AccountID == order.ConsultantID);

            return result;
        }

        public virtual bool IsOrderFullyReturned(IOrder order)
        {
            //IList<Order> returnOrders = Order.LoadChildOrdersFull(order.OrderID, Constants.OrderType.ReturnOrder.ToInt());
            //return IsOrderFullyReturned(order, returnOrders.AsAbstract<IOrder, Order>().ToList());
            OrderItemReturnRepository repository = new OrderItemReturnRepository();
            List<OrderReturnDetailsTable> listaReturnOrders = repository.GetQuantityReturnItemsByParentOrderID(order.OrderID);
            return IsOrderFullyReturned(order, listaReturnOrders);
        }

        public bool OrderDevueltaTotalmente(IOrder order)
        {
            OrderItemReturnRepository repository = new OrderItemReturnRepository();
            List<OrderReturnDetailsTable> listaReturnOrders = repository.GetQuantityReturnItemsByParentOrderID(order.OrderID);
            return IsOrderFullyReturned(order, listaReturnOrders);
        }

        public virtual bool IsOrderFullyReturned(IOrder order, List<OrderReturnDetailsTable> returnOrders)
        {
            using (var fullReturnedTrace = this.TraceActivity("OrderBusinessLogic::IsOrderFullyReturned"))
            {
                try
                {
                    returnOrders = returnOrders.Where(o => o.OrderStatusID == (int)Constants.OrderStatus.Paid
                                                        || o.OrderStatusID == (int)Constants.OrderStatus.PendingConfirm).ToList();
                    bool fullyReturned = true;
                    if (returnOrders.Count > 0)
                    {
                        Dictionary<int, int> returnedProducts = new Dictionary<int, int>();
                        //foreach (Order existingReturn in returnOrders.Where(o => o.OrderStatusID == (int)Constants.OrderStatus.Paid))
                        //{
                        foreach (OrderReturnDetailsTable item in returnOrders)
                        {
                            try
                            {
                                if (returnedProducts.ContainsKey(item.ProductID))
                                {
                                    returnedProducts[item.ProductID] += item.QuantityReturn;
                                }
                                else
                                {
                                    returnedProducts.Add(item.ProductID, item.QuantityReturn);
                                }
                            }
                            catch (Exception excp)
                            {
                                this.TraceError(string.Format("error while checking return orders: OrderItem {0} has an error", (item != null) ? item.OrderItemID.ToString() : "null"));
                                excp.TraceException(excp);
                                throw;
                            }
                        }
                        //}

                        OrderItemReturnRepository repository = new OrderItemReturnRepository();
                        List<OrderReturnDetailsTable> listaReturnOrders = repository.GetQuantityItemsByOrderID(order.OrderID);

                        //foreach (var item in order.OrderCustomers.SelectMany(oc => oc.OrderItems).GroupBy(oi => oi.ProductID.Value).
                        //    Select(g => new { g.Key, Count = g.Count(), suma = g.Sum(suma => suma.Quantity) }))
                        foreach (var item in listaReturnOrders.GroupBy(oi => oi.ProductID).Select(g => new { g.Key, Count = g.Count(), suma = g.Sum(suma => suma.Quantity) }))
                        {
                            //if (!returnedProducts.ContainsKey(item.Key) || Math.Abs(returnedProducts[item.Key]) < item.Count)
                            int valorKey = item.Key;
                            int cantidad = item.Count;
                            int sumatoria = item.suma;
                            int abs = 0;
                            if (returnedProducts.ContainsKey(valorKey))
                                abs = Math.Abs(returnedProducts[valorKey]);
                            if (!returnedProducts.ContainsKey(valorKey) || abs < cantidad | abs < sumatoria)
                            {
                                fullyReturned = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        fullyReturned = false;
                    }

                    return fullyReturned;
                }
                catch (Exception excp)
                {
                    excp.TraceException(excp);
                    throw;
                }
            }
        }

        public virtual bool IsOrderFullyReturned(IOrder order, List<IOrder> returnOrders)
        {
            using (var fullReturnedTrace = this.TraceActivity("OrderBusinessLogic::IsOrderFullyReturned"))
            {
                try
                {
                    returnOrders = returnOrders.Where(o => o.OrderStatusID == (int)Constants.OrderStatus.Paid).ToList();
                    bool fullyReturned = true;
                    if (returnOrders.Count > 0)
                    {
                        Dictionary<int, int> returnedProducts = new Dictionary<int, int>();
                        foreach (Order existingReturn in returnOrders.Where(o => o.OrderStatusID == (int)Constants.OrderStatus.Paid))
                        {
                            foreach (OrderItem item in existingReturn.OrderCustomers.SelectMany(oc => oc.OrderItems))
                            {
                                try
                                {
                                    if (returnedProducts.ContainsKey(item.ProductID.Value))
                                    {
                                        returnedProducts[item.ProductID.Value] += item.Quantity;
                                    }
                                    else
                                    {
                                        returnedProducts.Add(item.ProductID.Value, item.Quantity);
                                    }
                                }
                                catch (Exception excp)
                                {
                                    this.TraceError(string.Format("error while checking return orders: OrderItem {0} has an error", (item != null) ? item.OrderItemID.ToString() : "null"));
                                    excp.TraceException(excp);
                                    throw;
                                }
                            }
                        }

                        foreach (var item in order.OrderCustomers.SelectMany(oc => oc.OrderItems).GroupBy(oi => oi.ProductID.Value).
                            Select(g => new { g.Key, Count = g.Count(), suma = g.Sum(suma => suma.Quantity) }))
                        {
                            //if (!returnedProducts.ContainsKey(item.Key) || Math.Abs(returnedProducts[item.Key]) < item.Count)
                            int valorKey = item.Key;
                            int cantidad = item.Count;
                            int sumatoria = item.suma;
                            int abs = 0;
                            if (returnedProducts.ContainsKey(valorKey))
                                abs = Math.Abs(returnedProducts[valorKey]);
                            if (!returnedProducts.ContainsKey(valorKey) || abs < cantidad | abs < sumatoria)
                            {
                                fullyReturned = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        fullyReturned = false;
                    }

                    return fullyReturned;
                }
                catch (Exception excp)
                {
                    excp.TraceException(excp);
                    throw;
                }
            }
        }

        public virtual short GetStandardOrderTypeID(IOrderCustomer orderCustomer)
        {
            return Constants.OrderType.OnlineOrder.ToShort();
        }

        public virtual bool IsAutoshipOrder(short orderTypeID)
        {
            return (orderTypeID == Constants.OrderType.AutoshipOrder.ToInt() ||
                    orderTypeID == Constants.OrderType.AutoshipTemplate.ToInt());
        }

        public virtual void UpdateInventoryLevels(IOrder order, bool? returnProducts = null, bool? originalOrderCancelled = null)
        {
            try
            {
                if (!returnProducts.HasValue)
                    returnProducts = (order.OrderStatusID == Constants.OrderType.ReturnOrder.ToShort());

                foreach (OrderCustomer customer in order.OrderCustomers)
                {
                    OrderShipment orderShipment = customer.OrderShipments.FirstOrDefault();
                    if (orderShipment == null)
                        orderShipment = order.AsOrder().OrderShipments.FirstOrDefault();

                    if (orderShipment == null)
                        throw new NetStepsException("Cannot update warehouse inventory levels because the shipping address was not found.")
                        {
                            PublicMessage = Translation.GetTerm("CannotUpdateWarehouseInventoryLevels", "Cannot update warehouse inventory levels because the shipping address was not found.")
                        };

                    if (customer.OrderItems.Count == 0)
                        continue; //nothing to update move along.

                    //We only update items that are shippable for now
                    var customerOrderItemsSelect = customer.OrderItems.Select(oi => oi.ProductID.ToInt());
                    var products = inventoryRepository.GetProducts(customerOrderItemsSelect).Where(p => p.ProductBase.IsShippable && !p.IsStaticKit());
                    if (products.Any())
                    {
                        foreach (OrderItem orderItem in customer.OrderItems.Where(oi => products.Any(p => p.ProductID == oi.ProductID)))
                        {
                            //UpdateInventoryForShippableItemsOnly(orderItem, (bool)(returnProducts ?? false), orderShipment, (bool)(originalOrderCancelled ?? false));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException, order != null ? order.OrderID.ToIntNullable() : null);
            }
        }

        private static void UpdateInventoryForShippableItemsOnly(OrderItem orderItem, bool returnProducts, OrderShipment orderShipment, bool originalOrderCancelled)
        {
            if (orderItem.IsRestockable())
            {
                WarehouseProduct warehouseProduct = WarehouseProduct.GetWarehouseProduct(orderShipment, orderItem.ProductID.Value);
                warehouseProduct.StartEntityTracking();
                if (returnProducts)
                {
                    //Need to see if the item has been received and update the OnHand quantity accordingly
                    if (orderItem.OrderItemReturns != null && orderItem.OrderItemReturns.Count > 0)
                    {
                        //Orderitem.orderitemreturns should never have more than 1.  No idea why this is a collection
                        foreach (OrderItemReturn returnItem in orderItem.OrderItemReturns)
                        {
                            //If the original order was cancelled, the inventory was never taken out of quantityOnHand.
                            //Return the item quantity to the QuantityAllocated instead
                            if (returnItem.HasBeenReceived && returnItem.IsRestocked && !originalOrderCancelled)
                            {
                                warehouseProduct.QuantityOnHand += returnItem.Quantity;
                            }
                            else if (originalOrderCancelled)
                            {
                                warehouseProduct.QuantityAllocated -= returnItem.Quantity;
                            }
                        }
                    }
                }
                else
                {
                    warehouseProduct.QuantityAllocated += orderItem.Quantity;
                }
                warehouseProduct.Save();
            }
        }

        #region Overrides

        public virtual void PerformOverrides(Order order, List<OrderItemOverride> orderItemOverrides, decimal taxAmount, decimal shippingAmount)
        {
            try
            {
                OrderCustomer customer = order.OrderCustomers[0];

                //If the customer is null, validate the order customers and leave the method
                if (customer == null)
                {
                    ValidateOrderCustomers();
                    return;
                }

                //Check to see if amounts are out of range
                if (taxAmount < 0 || taxAmount > NetSteps.Common.Extensions.DecimalExtensions.GetRoundedNumber(customer.TaxAmountTotal.Value, 2) || shippingAmount < 0 || shippingAmount > NetSteps.Common.Extensions.DecimalExtensions.GetRoundedNumber(customer.ShippingTotal.Value, 2))
                {
                    ThrowOverrideValueException();
                }

                ApplyItemOverrides(orderItemOverrides, customer, order, true);
                ApplyTaxOverrides(customer, order, taxAmount, true);
                ApplyShippingOverrides(customer, order, shippingAmount, true);

                //Cleanup
                customer.RemoveAllPayments();
            }
            catch (Exception ex)
            {
                CancelOverrides(order); // Undo whatever was done
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException, order != null ? order.OrderID.ToIntNullable() : null);
            }

        }

        protected virtual void ResetOrderPrices(IOrder order, IOrderCustomer orderCustomer)
        {
            if (orderCustomer != null)
            {
                foreach (OrderItem orderItem in orderCustomer.OrderItems)
                {
                    orderItem.ItemPriceActual = null;
                    orderItem.Discount = null;
                    orderItem.DiscountPercent = null;
                }

                order.AsOrder().TaxAmountTotalOverride = null;
                order.AsOrder().ShippingTotalOverride = null;

                ((OrderCustomer)orderCustomer).RemoveAllPayments();
            }
        }

        public virtual void CancelOverrides(IOrder order)
        {
            try
            {
                OrderCustomer orderCustomer = order.AsOrder().OrderCustomers[0];

                if (orderCustomer != null)
                {
                    ResetOrderPrices(order, orderCustomer);
                }
                else
                {
                    ValidateOrderCustomers();
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException, orderID: (order != null) ? order.OrderID.ToIntNullable() : null);
            }
        }

        public virtual void UndoReplacementOrderPrices(IOrder order)
        {
            try
            {
                OrderCustomer orderCustomer = order.AsOrder().OrderCustomers[0];

                if (orderCustomer != null)
                {
                    List<OrderItemOverride> orderItemOverrides = new List<OrderItemOverride>();
                    foreach (OrderItem orderItem in orderCustomer.OrderItems)
                    {
                        orderItemOverrides.Add(new OrderItemOverride()
                        {
                            CommissionableValue = 0,
                            PricePerItem = 0,
                            OrderItemGuid = orderItem.Guid.ToString("N"),
                            OrderItemID = orderItem.OrderItemID
                        });
                    }
                    SetReplacementOrderPrices(order, orderItemOverrides, 0, 0);
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException, orderID: (order != null) ? order.OrderID.ToIntNullable() : null);
            }
        }

        public virtual void SetReplacementOrderPrices(IOrder order, List<OrderItemOverride> orderItemOverrides, decimal taxAmount, decimal shippingAmount)
        {
            try
            {
                OrderCustomer customer = order.AsOrder().OrderCustomers[0];

                //If the customer is null, validate the order customers and leave the method
                if (customer == null)
                {
                    ValidateOrderCustomers();
                    return;
                }

                ResetOrderPrices(order, customer);

                //Check to see if amounts are out of range
                if (taxAmount < 0 || shippingAmount < 0)
                {
                    ThrowOverrideValueException();
                }

                ApplyItemOverrides(orderItemOverrides, customer, order, false);
                ApplyTaxOverrides(customer, order, taxAmount, false);
                ApplyShippingOverrides(customer, order, shippingAmount, false);
            }
            catch (Exception ex)
            {
                UndoReplacementOrderPrices(order); // Undo whatever was done
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException, order != null ? order.OrderID.ToIntNullable() : null);
            }
        }

        protected virtual void ApplyTaxOverrides(IOrderCustomer customer, IOrder order, decimal taxAmount, bool ignoreSame)
        {
            if (ignoreSame && NetSteps.Common.Extensions.DecimalExtensions.GetRoundedNumber(((OrderCustomer)customer).TaxAmountTotal.Value, 2) == taxAmount)
            {
                return;
            }

            order.CalculationsDirty = true;
            order.AsOrder().TaxAmountTotalOverride = taxAmount;
        }

        protected virtual void ApplyShippingOverrides(IOrderCustomer customer, IOrder order, decimal shippingAmount, bool ignoreSame)
        {
            if (ignoreSame && NetSteps.Common.Extensions.DecimalExtensions.GetRoundedNumber(((OrderCustomer)customer).ShippingTotal.Value, 2) == shippingAmount)
            {
                return;
            }

            order.CalculationsDirty = true;
            order.AsOrder().ShippingTotalOverride = shippingAmount;
        }

        protected virtual bool ApplyItemOverrides(List<OrderItemOverride> orderItemOverrides, OrderCustomer customer, IOrder order, bool ignoreSame)
        {
            bool changesMade = false;
            foreach (OrderItemOverride orderItemOverride in orderItemOverrides)
            {
                OrderItem currentOrderItem = customer.OrderItems.FirstOrDefault(x => x.Guid.ToString("N") == orderItemOverride.OrderItemGuid);

                if (ignoreSame && currentOrderItem.ItemPrice == orderItemOverride.PricePerItem && currentOrderItem.CommissionableTotal == orderItemOverride.CommissionableValue)
                    continue; // no need to do anything, they didn't change the amounts

                //Returns false if there was no change
                ApplyOverrideDiscountOnItems(customer, orderItemOverride, order, currentOrderItem);
                changesMade = true;
            }
            return changesMade;
        }

        protected virtual void ApplyOverrideDiscountOnItems(OrderCustomer customer, OrderItemOverride orderItemOverride, IOrder order, OrderItem currentOrderItem)
        {
            var product = inventoryRepository.GetProduct(currentOrderItem.ProductID.ToInt());
            decimal currentPrice = product.GetPrice(customer.AccountInfo.AccountTypeID, order.CurrencyID, order.OrderTypeID);
            decimal currentCommissionableTotal = product.GetPrice(customer.AccountInfo.AccountTypeID, NetSteps.Data.Entities.Generated.ConstantsGenerated.PriceRelationshipType.Commissions, order.CurrencyID, order.OrderTypeID) * currentOrderItem.Quantity;

            if (product.IsDynamicallyPricedKit())
            {
                currentPrice = currentOrderItem.ItemPrice;
                currentCommissionableTotal = currentOrderItem.CommissionableTotal ?? 0;
            }

            //If amounts are out of range, throw exception
            if (orderItemOverride.PricePerItem < 0 || orderItemOverride.PricePerItem > currentPrice
                || orderItemOverride.CommissionableValue < 0 || orderItemOverride.CommissionableValue > currentCommissionableTotal)
            {
                ThrowOverrideValueException();
            }

            //Calculate discount and notify totals calculator that order totals will need to be recalculated
            order.CalculationsDirty = true;
            if (order.OrderTypeID == (short)Constants.OrderType.ReplacementOrder)
            {
                // Replacement orders can be overridden with higher prices, so we do some trickery with the discount :(
                currentOrderItem.Discount = (currentOrderItem.ItemPrice * currentOrderItem.Quantity) - (orderItemOverride.PricePerItem * currentOrderItem.Quantity);
            }
            else
            {
                // Perform a normal override
                if (currentOrderItem.ItemPrice == orderItemOverride.PricePerItem)
                    currentOrderItem.ItemPriceActual = null;
                else
                    currentOrderItem.ItemPriceActual = orderItemOverride.PricePerItem;
            }
            currentOrderItem.CommissionableTotalOverride = orderItemOverride.CommissionableValue;
        }

        protected virtual void ThrowOverrideValueException()
        {
            throw new NetStepsException("Please check all of the values to make sure they are not negative or greater than the original amount.")
            {
                PublicMessage = Translation.GetTerm("InvalidOverrideValue", "Please check all of the values to make sure they are not negative or greater than the original amount.")
            };
        }

        #endregion

        public virtual void UpdateOrderShipmentAddress(Order entity, OrderShipment shipment, int addressId)
        {
            NetSteps.Addresses.Common.Models.IAddress address = Address.LoadFull(addressId);
            UpdateOrderShipmentAddress(entity, shipment, address);
        }

        public virtual void UpdateOrderShipmentAddress(Order order, OrderShipment shipment, NetSteps.Addresses.Common.Models.IAddress address)
        {
            OrderCustomer customer = order.OrderCustomers[0];

            if (shipment == null)
            {
                // Create new shipment
                shipment = new OrderShipment();
                shipment.OrderShipmentStatusID = (int)Constants.OrderShipmentStatus.Pending;
            }

            shipment.SourceAddressID = address.AddressID > 0 ? address.AddressID : (int?)null;   // Set sourceAddressID

            Address.CopyPropertiesTo(address, shipment);

            shipment.FirstName = customer.AccountInfo != null ? customer.AccountInfo.FirstName : string.Empty;
            shipment.LastName = customer.AccountInfo != null ? customer.AccountInfo.LastName : string.Empty;

            order.OrderShipments.Add(shipment);
            shipment.Order = order;

            order.CalculationsDirty = true;
        }

        public virtual void ConsolidateOrderItems(OrderCustomer customer, string parentGuid = null, int? dynamicKitGroupId = null)
        {
            var containsMultipleOfSameProduct = false;

            foreach (var orderItem in customer.ParentOrderItems)
            {
                var product = inventoryRepository.GetProduct(orderItem.ProductID.Value);

                if (product.IsDynamicKit())
                {
                    foreach (var childOrderItem in orderItem.ChildOrderItems)
                    {
                        containsMultipleOfSameProduct = customer.OrderItems.Count(i => i.ProductID == childOrderItem.ProductID && i.DynamicKitGroupID == dynamicKitGroupId && i.ParentOrderItem == childOrderItem.ParentOrderItem) > 1;
                        if (containsMultipleOfSameProduct)
                        {
                            orderItem.Quantity += customer.OrderItems.Where(i => i.ProductID == childOrderItem.ProductID && i.DynamicKitGroupID == dynamicKitGroupId && i.ParentOrderItem == childOrderItem.ParentOrderItem && i != childOrderItem).Sum(oi => oi.Quantity);

                            var orderItemsToRemove =
                            customer.OrderItems.Where(
                                oi =>
                                    oi.ProductID == childOrderItem.ProductID && oi.DynamicKitGroupID == dynamicKitGroupId
                                    && oi.ParentOrderItem == childOrderItem.ParentOrderItem && oi != childOrderItem);

                            foreach (var orderItemToRemove in orderItemsToRemove.ToList())
                            {
                                customer.Order.RemoveItem(customer, orderItemToRemove);
                            }
                        }
                    }
                }
                else if (product.IsStaticKit())
                {

                    foreach (var childOrderItem in orderItem.ChildOrderItems)
                    {
                        containsMultipleOfSameProduct = customer.OrderItems.Count(i => i.MaterialID == childOrderItem.MaterialID && i.ParentOrderItem == childOrderItem.ParentOrderItem) > 1;
                        if (containsMultipleOfSameProduct)
                        {
                            orderItem.Quantity += customer.OrderItems.Where(i => i.ProductID == childOrderItem.ProductID && i.ParentOrderItem == childOrderItem.ParentOrderItem && i != childOrderItem).Sum(oi => oi.Quantity);

                            var orderItemsToRemove =
                            customer.OrderItems.Where(
                                oi =>
                                    oi.ProductID == childOrderItem.ProductID && oi.ParentOrderItem == childOrderItem.ParentOrderItem
                                    && oi != childOrderItem);

                            foreach (var orderItemToRemove in orderItemsToRemove.ToList())
                            {
                                customer.Order.RemoveItem(customer, orderItemToRemove);
                            }
                        }
                    }
                }
            }
        }
        public virtual BasicResponseItem<OrderPayment> ApplyPaymentToCustomer(Repositories.IOrderRepository repository, Order order, OrderCustomer customer, IPayment paymentMethod, decimal amount, NetSteps.Common.Interfaces.IUser user = null)
        {
            if (order == null)
            {
                throw new ArgumentNullException("order", "Order cannot be null");
            }

            if (customer == null)
            {
                customer = order.OrderCustomers.FirstOrDefault();
                if (customer == null)
                {
                    throw new ArgumentNullException("customer", "Customer cannot be null");
                }
            }

            if (paymentMethod == null)
            {
                throw new ArgumentNullException("paymentMethod", "Payment Method cannot be null");
            }

            BasicResponseItem<OrderPayment> response = new BasicResponseItem<OrderPayment>();
            var paymentList = new StringBuilder();

            try
            {
                if (order.CurrencyID == 0)
                {
                    SetCurrencyID(order);
                }

                OrderPayment orderPayment = null;
                response.Item = orderPayment;

                BasicResponse validationResponse = new BasicResponse();

                validationResponse = ValidatePayment(amount, order, paymentMethod.PaymentTypeID, customer, user);

                if (validationResponse.Success)
                {
                    amount = PaymentTypeValidator.DetermineNewPaymentAmount(order, paymentMethod, amount);
                    orderPayment = default(OrderPayment);

                    if (paymentMethod is AccountPaymentMethod)
                    {
                        orderPayment = customer.OrderPayments.FirstOrDefault(x => x.SourceAccountPaymentMethodID == (paymentMethod as AccountPaymentMethod).AccountPaymentMethodID && x.OrderPaymentStatusID == Constants.OrderPaymentStatus.Pending.ToShort());
                    }
                    else if (paymentMethod is NonAccountPaymentMethod)
                    {
                        if (paymentMethod.PaymentTypeID == Constants.PaymentType.GiftCard.ToInt())
                        {
                            orderPayment = customer.OrderPayments.FirstOrDefault(x => x.PaymentTypeID == (paymentMethod as NonAccountPaymentMethod).PaymentTypeID
                                    && x.OrderPaymentStatusID == Constants.OrderPaymentStatus.Pending.ToShort()
                                    && x.DecryptedAccountNumber == paymentMethod.DecryptedAccountNumber);
                        }
                        else
                        {
                            orderPayment = customer.OrderPayments.FirstOrDefault(x => x.PaymentTypeID == (paymentMethod as NonAccountPaymentMethod).PaymentTypeID && x.OrderPaymentStatusID == Constants.OrderPaymentStatus.Pending.ToShort());
                        }
                    }

                    if (orderPayment == null)
                    {
                        orderPayment = new OrderPayment(paymentMethod);
                        orderPayment.OrderID = order.OrderID;
                        if (paymentMethod is AccountPaymentMethod)
                        {
                            orderPayment.SourceAccountPaymentMethodID = (paymentMethod as AccountPaymentMethod).AccountPaymentMethodID;
                        }
                        orderPayment.CurrencyID = order.CurrencyID;
                        orderPayment.Amount = amount;

                        response.Item = orderPayment;

                        customer.OrderPayments.Add(orderPayment);
                        order.OrderPayments.Add(orderPayment);
                    }
                    else
                    {
                        ValidateHostessRewardItem(customer, paymentMethod, amount);
                        orderPayment.Amount = orderPayment.Amount += amount;
                        response.Item = orderPayment;
                    }

                    //TODO: Figure out if anything is actually using the negative order payment ids and switch it to use the guid - DES
                    if (orderPayment.OrderPaymentID == 0)
                    {
                        orderPayment.StopTracking();
                        orderPayment.OrderPaymentID = customer.OrderPayments.GetNextIntNegative(p => p.OrderPaymentID);
                        orderPayment.StartTracking();
                    }

                    ApplyCustomClientLogicAddPaymentToOrder(customer, order);

                    response.Success = true;
                }
                else
                {
                    response.Success = false;
                    response.Message = validationResponse.Message;
                }

                return response;

            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException, orderID: (order != null) ? order.OrderID.ToIntNullable() : null);
                response.Success = false;
                response.Message = exception.PublicMessage;
                return response;
            }
        }
        ///// <param name="customer">Will use the first customer on the order if left null</param>
        ///// <param name="user">User is only used to validate the role-function for GMP, otherwise, we use the Distributor account type to validate the role-function</param>
        //public virtual BasicResponseItem<OrderPayment> ApplyPaymentToCustomer(Repositories.IOrderRepository repository, Order order, OrderCustomer customer, int PaymentTypeID, string NamePayment, int IdPayment,  decimal amount, NetSteps.Common.Interfaces.IUser user = null)
        //{
        //    if (order == null)
        //    {
        //        throw new ArgumentNullException("order", "Order cannot be null");
        //    }

        //    if (customer == null)
        //    {
        //        customer = order.OrderCustomers.FirstOrDefault();
        //        if (customer == null)
        //        {
        //            throw new ArgumentNullException("customer", "Customer cannot be null");
        //        }
        //    }

        //    if (PaymentTypeID == 0)
        //    {
        //        throw new ArgumentNullException("paymentMethod", "Payment Method cannot be null");
        //    }

        //    BasicResponseItem<OrderPayment> response = new BasicResponseItem<OrderPayment>();
        //    var paymentList = new StringBuilder();

        //    try
        //    {
        //        if (order.CurrencyID == 0)
        //        {
        //            SetCurrencyID(order);
        //        }

        //        OrderPayment orderPayment = null;
        //        response.Item = orderPayment;

        //        BasicResponse validationResponse = new BasicResponse();

        //        validationResponse = ValidatePayment(amount, order, PaymentTypeID, customer, user);

        //        if (validationResponse.Success)
        //        {
        //            PaymentReturn objE = new PaymentReturn();
        //            objE.Amount= amount;
        //            objE.NameCard = NamePayment;
        //            objE.PaymentId = IdPayment;
        //            if (Order.paymentReturn.Count > 0)
        //            {
        //                foreach (var item in Order.paymentReturn)
        //                {
        //                    if (item.PaymentId == objE.PaymentId)
        //                    {
        //                        item.Amount = objE.Amount;
        //                    }
        //                    else
        //                    {
        //                        Order.paymentReturn.Add(objE);
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                Order.paymentReturn.Add(objE);
        //            }

        //            //amount = PaymentTypeValidator.DetermineNewPaymentAmount(order, paymentMethod, amount);

        //            //orderPayment = default(OrderPayment);

        //            //if (paymentMethod is AccountPaymentMethod)
        //            //{
        //            //    orderPayment = customer.OrderPayments.FirstOrDefault(x => x.SourceAccountPaymentMethodID == (paymentMethod as AccountPaymentMethod).AccountPaymentMethodID && x.OrderPaymentStatusID == Constants.OrderPaymentStatus.Pending.ToShort());
        //            //}
        //            //else 
        //            //if (paymentMethod is NonAccountPaymentMethod)
        //            //{
        //            if (PaymentTypeID == Constants.PaymentType.GiftCard.ToInt())
        //            {
        //                orderPayment = customer.OrderPayments.FirstOrDefault(x => x.PaymentTypeID == PaymentTypeID
        //                        && x.OrderPaymentStatusID == Constants.OrderPaymentStatus.Pending.ToShort()
        //                        && x.DecryptedAccountNumber == NamePayment);
        //            }
        //            else
        //            {
        //                orderPayment = customer.OrderPayments.FirstOrDefault(x => x.PaymentTypeID == PaymentTypeID && x.OrderPaymentStatusID == Constants.OrderPaymentStatus.Pending.ToShort());
        //            }
        //            //}
        //            //OrderPayment obj = new OrderPayment();
        //            //obj.AccountNumber = "2132";
        //            //obj.Amount = amount;
        //            //obj.OrderCustomer.Total = amount;
        //            //orderPayment = obj;
        //            //order.OrderPayments.Add(orderPayment);
        //            if (orderPayment == null)
        //            {
        //                orderPayment = new OrderPayment(paymentMethod);
        //                orderPayment.OrderID = order.OrderID;

        //                if (paymentMethod is AccountPaymentMethod)
        //                {
        //                    orderPayment.SourceAccountPaymentMethodID = (paymentMethod as AccountPaymentMethod).AccountPaymentMethodID;
        //                }

        //                orderPayment.CurrencyID = order.CurrencyID;
        //                orderPayment.Amount = amount;

        //                response.Item = orderPayment;

        //                customer.OrderPayments.Add(orderPayment);
        //                order.OrderPayments.Add(orderPayment);
        //            }
        //            else
        //            {
        //                ValidateHostessRewardItem(customer, paymentMethod, amount);
        //                orderPayment.Amount = orderPayment.Amount += amount;
        //                response.Item = orderPayment;
        //            }

        //            //TODO: Figure out if anything is actually using the negative order payment ids and switch it to use the guid - DES
        //            if (orderPayment.OrderPaymentID == 0)
        //            {
        //                orderPayment.StopTracking();
        //                orderPayment.OrderPaymentID = customer.OrderPayments.GetNextIntNegative(p => p.OrderPaymentID);
        //                orderPayment.StartTracking();
        //            }


        //            ApplyCustomClientLogicAddPaymentToOrder(customer, order);

        //            response.Success = true;
        //        }
        //        else
        //        {
        //            response.Success = false;
        //            response.Message = validationResponse.Message;
        //        }

        //        return response;

        //    }
        //    catch (Exception ex)
        //    {
        //        var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException, orderID: (order != null) ? order.OrderID.ToIntNullable() : null);
        //        response.Success = false;
        //        response.Message = exception.PublicMessage;
        //        return response;
        //    }
        //}
        public virtual BasicResponseItem<OrderPayment> ApplyPaymentToCustomerPreviosBalance(Repositories.IOrderRepository repository, Order order, OrderCustomer customer, IPayment paymentMethod, decimal amount, NetSteps.Common.Interfaces.IUser user = null)
        {
            if (order == null)
            {
                throw new ArgumentNullException("order", "Order cannot be null");
            }

            if (customer == null)
            {
                customer = order.OrderCustomers.FirstOrDefault();
                if (customer == null)
                {
                    throw new ArgumentNullException("customer", "Customer cannot be null");
                }
            }

            if (paymentMethod == null)
            {
                throw new ArgumentNullException("paymentMethod", "Payment Method cannot be null");
            }

            BasicResponseItem<OrderPayment> response = new BasicResponseItem<OrderPayment>();
            var paymentList = new StringBuilder();

            try
            {
                if (order.CurrencyID == 0)
                {
                    SetCurrencyID(order);
                }

                OrderPayment orderPayment = null;
                response.Item = orderPayment;

                BasicResponse validationResponse = new BasicResponse();

                //validationResponse = ValidatePayment(amount, order, paymentMethod.PaymentTypeID, customer, user);
                validationResponse.Success = true;
                if (validationResponse.Success)
                {
                    //  amount = PaymentTypeValidator.DetermineNewPaymentAmount(order, paymentMethod, amount);
                    orderPayment = default(OrderPayment);

                    if (paymentMethod is AccountPaymentMethod)
                    {
                        orderPayment = customer.OrderPayments.FirstOrDefault(x => x.SourceAccountPaymentMethodID == (paymentMethod as AccountPaymentMethod).AccountPaymentMethodID && x.OrderPaymentStatusID == Constants.OrderPaymentStatus.Pending.ToShort());
                    }
                    else if (paymentMethod is NonAccountPaymentMethod)
                    {
                        if (paymentMethod.PaymentTypeID == Constants.PaymentType.GiftCard.ToInt())
                        {
                            orderPayment = customer.OrderPayments.FirstOrDefault(x => x.PaymentTypeID == (paymentMethod as NonAccountPaymentMethod).PaymentTypeID
                                    && x.OrderPaymentStatusID == Constants.OrderPaymentStatus.Pending.ToShort()
                                    && x.DecryptedAccountNumber == paymentMethod.DecryptedAccountNumber);
                        }
                        else
                        {
                            orderPayment = customer.OrderPayments.FirstOrDefault(x => x.PaymentTypeID == (paymentMethod as NonAccountPaymentMethod).PaymentTypeID && x.OrderPaymentStatusID == Constants.OrderPaymentStatus.Pending.ToShort());
                        }
                    }

                    if (orderPayment == null)
                    {
                        orderPayment = new OrderPayment(paymentMethod);
                        orderPayment.OrderID = order.OrderID;

                        if (paymentMethod is AccountPaymentMethod)
                        {
                            orderPayment.SourceAccountPaymentMethodID = (paymentMethod as AccountPaymentMethod).AccountPaymentMethodID;
                        }

                        orderPayment.CurrencyID = order.CurrencyID;
                        orderPayment.Amount = amount;

                        response.Item = orderPayment;

                        customer.OrderPayments.Add(orderPayment);
                        order.OrderPayments.Add(orderPayment);
                    }
                    else
                    {
                        ValidateHostessRewardItem(customer, paymentMethod, amount);
                        orderPayment.Amount = orderPayment.Amount += amount;
                        response.Item = orderPayment;
                    }

                    //TODO: Figure out if anything is actually using the negative order payment ids and switch it to use the guid - DES
                    if (orderPayment.OrderPaymentID == 0)
                    {
                        orderPayment.StopTracking();
                        orderPayment.OrderPaymentID = customer.OrderPayments.GetNextIntNegative(p => p.OrderPaymentID);
                        orderPayment.StartTracking();
                    }

                    ApplyCustomClientLogicAddPaymentToOrder(customer, order);

                    response.Success = true;
                }
                else
                {
                    response.Success = false;
                    response.Message = validationResponse.Message;
                }

                return response;

            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException, orderID: (order != null) ? order.OrderID.ToIntNullable() : null);
                response.Success = false;
                response.Message = exception.PublicMessage;
                return response;
            }
        }
        public virtual void ApplyCustomClientLogicAddPaymentToOrder(OrderCustomer customer, Order order)
        {
            //Place holder for any client-specific post-logic
        }

        /// <param name="user">User is only used to validate the role-function for GMP, otherwise, we use the order account type to validate the role-function</param>
        public virtual BasicResponse ValidatePayment(decimal amount, Order order, int PaymentTypeID, OrderCustomer customer = null, NetSteps.Common.Interfaces.IUser user = null)
        {
            //Valida que el monto sea > 0
            BasicResponse response = ValidateAmountIsGreaterThanZero(amount);

            if (response.Success)
            {
                //Valida que no exceda el total 
                response = ValidatePaymentDoesNotExceedTotal(order, amount);
            }

            if (response.Success)
            {
                response = ValidatePaymentFunction(order, PaymentTypeID, customer, user);
            }

            if (response.Success)
            {
                response = ValidateProductCredit(order, PaymentTypeID, customer);
            }

            //if (response.Success)
            //{
            //    response = ValidatePaymentGatewayCharge(amount, order, customer);
            //}

            //if (response.Success)
            //{
            //    response = ValidatePaymentType(order, paymentMethod, amount);
            //}

            return response;
        }

        public virtual BasicResponse ValidatePaymentDoesNotExceedTotal(Order order, decimal amount)
        {
            decimal totalAPagar = 0;
            totalAPagar = order.PaymentTotal.ToDecimal() + (order.PartyShipmentTotal.HasValue ? order.PartyShipmentTotal.ToDecimal() : 0);
            decimal totalAllowed = (order.GrandTotal.Value - totalAPagar).GetRoundedNumber();

            //The values are rounded to the nearest penny on the UI, so we need to round to make sure that it doesn't incorrectly report that payment cannot exceed total - DES
            if (amount.GetRoundedNumber() > (order.GrandTotal - totalAPagar).GetRoundedNumber())
            {
                return new BasicResponse
                {
                    Success = false,
                    Message = Translation.GetTerm("PaymentCannotExceedTotal", "Payment cannot exceed the total.")
                };
            }
            return new BasicResponse { Success = true };
        }

        public virtual BasicResponse ValidateAmountIsGreaterThanZero(decimal amount)
        {
            if (amount <= 0)
            {
                return new BasicResponse
                {
                    Success = false,
                    Message = Translation.GetTerm("PaymentMustBeGreaterThanZero", "Payment must be greater than 0.")
                };
            }
            return new BasicResponse { Success = true };
        }

        private IPaymentTypeValidator _paymentTypeValidator;
        public IPaymentTypeValidator PaymentTypeValidator
        {
            get
            {
                if (_paymentTypeValidator == null)
                {
                    _paymentTypeValidator = Create.New<IPaymentTypeValidator>();
                }
                return _paymentTypeValidator;
            }
        }

        public virtual BasicResponse ValidatePaymentType(Order order, IPayment payment, decimal amount)
        {
            return PaymentTypeValidator.IsValidPayment(order, payment, amount);
        }

        public virtual BasicResponse ValidateProductCredit(Order order, int PaymentTypeID, OrderCustomer customer)
        {
            List<int> productCreditPaymentTypes = GetProductCreditPayments();
            if (!productCreditPaymentTypes.Contains(PaymentTypeID))
                return new BasicResponse() { Success = true };

            if (customer == null)
                customer = order.OrderCustomers.FirstOrDefault();

            //if (customer.OrderPayments.Any(
            //    op =>
            //        op != paymentMethod
            //        && op.PaymentTypeID == PaymentTypeID
            //        && op.OrderPaymentStatusID != (short)Constants.OrderPaymentStatus.Cancelled
            //        && op.OrderPaymentStatusID != (short)Constants.OrderPaymentStatus.Declined))
            //{
            //    return new BasicResponse() { Success = false, Message = Translation.GetTerm("YouMayOnlyApplyOneProductCreditPayment", "You may only apply one product credit payment") };
            //}

            return new BasicResponse() { Success = true };
        }

        /// <param name="user">User is only used to validate the role-function for GMP, otherwise, we use the order account type to validate the role-function</param>
        public virtual BasicResponse ValidatePaymentFunction(Order order, int PaymentTypeID, OrderCustomer customer = null, NetSteps.Common.Interfaces.IUser user = null)
        {

            var accountTypeID = GetAccountTypeID(order, customer);

            var paymentType = SmallCollectionCache.Instance.PaymentTypes.GetById(PaymentTypeID);

            if (paymentType == null
                    || paymentType.FunctionName.IsNullOrWhiteSpace()
                    || (user != null && user.UserID > 0 && user.HasFunction(paymentType.FunctionName))
                    || (accountTypeID > 0 && SmallCollectionCache.Instance.AccountTypes.GetById(accountTypeID).Roles.Any(r => r.HasFunction(paymentType.FunctionName))))
            {
                return new BasicResponse() { Success = true };
            }
            else
            {
                return new BasicResponse()
                {
                    Success = false,
                    Message = String.Format(Translation.GetTerm("YouDoNotHaveTheNecessaryFunction", "You do not have the necessary function: {0}."),
                            SmallCollectionCache.Instance.Functions.GetByName(paymentType.FunctionName ?? string.Empty).GetTerm())
                };
            }
        }

        protected virtual short GetAccountTypeID(Order order, OrderCustomer customer)
        {
            short accountTypeID;

            if (customer.IsNotNull())
            {
                accountTypeID = customer.AccountTypeID;

                if (order.OrderTypeID == Constants.OrderType.PartyOrder.ToInt())
                    accountTypeID = order.ConsultantInfo.AccountTypeID;
            }
            else
            {
                accountTypeID = order.ConsultantInfo.AccountTypeID;
            }

            return accountTypeID;
        }

        public virtual BasicResponse ValidatePaymentGatewayCharge(decimal amount, Order order, OrderCustomer orderCustomer = null)
        {
            BasicResponse response = new BasicResponse();
            Order clonedOrder = order.Clone();
            OrderCustomer clonedOrderCustomer = null;
            if (orderCustomer != null)
                clonedOrderCustomer = orderCustomer.Clone();
            IPayment clonedPayment = null;
            decimal currentBalance = 0;

            //if (paymentMethod is AccountPaymentMethod)
            //{
            //    clonedPayment = ((AccountPaymentMethod)paymentMethod).Clone();
            //}
            //else if (paymentMethod is Payment)
            //{
            //    clonedPayment = ((Payment)paymentMethod).Clone();
            //}
            //else if (paymentMethod is NonAccountPaymentMethod)
            //{
            //    clonedPayment = ((NonAccountPaymentMethod)paymentMethod).Clone();
            //}
            //else if (paymentMethod is OrderPayment)
            //{
            //    clonedPayment = ((OrderPayment)paymentMethod).Clone();
            //}

            OrderPayment validationOrderPayment = new OrderPayment(clonedPayment);

            validationOrderPayment.Order = clonedOrder;
            validationOrderPayment.Amount = amount;
            if (clonedOrderCustomer != null)
                validationOrderPayment.OrderCustomer = clonedOrderCustomer;

            IPaymentGateway gateway = null;
            try
            {
                gateway = PaymentGateways.Payments.GetPaymentGateway(clonedOrder, validationOrderPayment);
            }
            catch (System.NullReferenceException)
            {
                gateway = null;
            }

            if (gateway != null)
            {
                response = gateway.ValidateCharge(validationOrderPayment, ref currentBalance);
            }
            else
            {
                response.Success = false;
                response.Message = Translation.GetTerm("PaymentGatewayNotFound", "Payment Gateway Not Found");
            }

            return response;
        }

        /// <summary>
        /// Applies the payment to the order instead of the order customer (used for party orders) - DES
        /// </summary>
        public virtual BasicResponseItem<OrderPayment> ApplyPaymentToOrder(Repositories.IOrderRepository repository, Order entity, IPayment paymentMethod, decimal amount)
        {
            if (entity == null)
                throw new ArgumentNullException("order", "Order cannot be null");
            if (paymentMethod == null)
                throw new ArgumentNullException("paymentMethod", "Payment Method cannot be null");

            BasicResponseItem<OrderPayment> response = new BasicResponseItem<OrderPayment>();
            StringBuilder paymentList = new StringBuilder();

            Order order = entity;

            try
            {

                if (entity.CurrencyID == 0)
                    SetCurrencyID(entity);

                OrderPayment orderPayment = null;
                response.Item = orderPayment;

                BasicResponse validationResponse = new BasicResponse();
                int PaymentTypeID = 0;
                validationResponse = ValidatePayment(amount, order, PaymentTypeID);

                //make sure the payment amount is not 0 - Scott Wilson
                if (validationResponse.Success && amount > 0)
                {
                    OrderPayment obj = new OrderPayment();
                    obj.AccountNumber = "2132";
                    obj.Amount = amount;                    
                    orderPayment = obj;
                    //if (paymentMethod is AccountPaymentMethod)
                    //{
                    //    orderPayment = order.OrderPayments.FirstOrDefault(x => x.SourceAccountPaymentMethodID == (paymentMethod as AccountPaymentMethod).AccountPaymentMethodID && x.OrderPaymentStatusID == Constants.OrderPaymentStatus.Pending.ToShort());
                    //}
                    //else if (paymentMethod is NonAccountPaymentMethod)
                    //{
                    //    if (paymentMethod.PaymentTypeID == Constants.PaymentType.GiftCard.ToInt())
                    //        orderPayment = order.OrderPayments.FirstOrDefault(x => x.PaymentTypeID == (paymentMethod as NonAccountPaymentMethod).PaymentTypeID
                    //                && x.OrderPaymentStatusID == Constants.OrderPaymentStatus.Pending.ToShort()
                    //                && x.DecryptedAccountNumber == paymentMethod.DecryptedAccountNumber);
                    //    else
                    //        orderPayment = order.OrderPayments.FirstOrDefault(x => x.PaymentTypeID == (paymentMethod as NonAccountPaymentMethod).PaymentTypeID && x.OrderPaymentStatusID == Constants.OrderPaymentStatus.Pending.ToShort());
                    //}

                    if (orderPayment == null)
                    {
                        orderPayment = new OrderPayment(paymentMethod);
                        if (paymentMethod is AccountPaymentMethod)
                            orderPayment.SourceAccountPaymentMethodID = (paymentMethod as AccountPaymentMethod).AccountPaymentMethodID;
                        orderPayment.CurrencyID = order.CurrencyID;
                        orderPayment.Amount = amount;
                        if (paymentMethod is OrderPayment)
                        {
                            orderPayment.TransactionID = ((OrderPayment)paymentMethod).TransactionID;
                        }

                        response.Item = orderPayment;

                        order.OrderPayments.Add(orderPayment);
                    }
                    else
                    {
                        orderPayment.Amount = orderPayment.Amount += amount;

                        response.Item = orderPayment;
                    }

                    if (orderPayment.OrderPaymentID == 0)
                    {
                        orderPayment.StopTracking();
                        orderPayment.OrderPaymentID = order.OrderPayments.GetNextIntNegative(p => p.OrderPaymentID);                        
                        orderPayment.StartTracking();
                    }

                    response.Success = true;
                }
                else
                {
                    response.Success = false;
                    response.Message = validationResponse.Message;
                }

                return response;
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException, orderID: (entity != null) ? entity.OrderID.ToIntNullable() : null);
                response.Success = false;
                response.Message = exception.PublicMessage;
                return response;
            }
        }

        public virtual IEnumerable<ShippingMethodWithRate> UpdateOrderShipmentAddressAndDefaultShipping(Order order, int addressId)
        {
            Address addy = Account.LoadAddressAndVerifyAccount(addressId, order.OrderCustomers[0].AccountID);
            return UpdateOrderShipmentAddressAndDefaultShipping(order, addy);
        }

        public virtual IEnumerable<ShippingMethodWithRate> UpdateOrderShipmentAddressAndDefaultShipping(Order order, NetSteps.Addresses.Common.Models.IAddress address)
        {
            OrderShipment shipment = order.GetDefaultShipment();
            OrderCustomer customer = order.OrderCustomers[0];

            order.UpdateOrderShipmentAddress(shipment, address);
            this.SetCurrencyID(order);
            var shippingMethods = shippingCalculator.GetShippingMethodsWithRates(customer, shipment).OrderBy(sm => sm.ShippingAmount).ToList();

            // Do this before calculating totals to prevent exceptions.
            ValidateOrderShipmentShippingMethod(shipment, shippingMethods);
            order.CalculationsDirty = true;

            return shippingMethods;
        }

        public virtual void ValidateOrderShipmentShippingMethod(OrderShipment shipment, OrderCustomer customer)
        {
            var shippingMethods = shippingCalculator.GetShippingMethodsWithRates(customer, shipment);

            ValidateOrderShipmentShippingMethod(shipment, shippingMethods);
        }

        public virtual void ValidateOrderShipmentShippingMethod(OrderShipment shipment, IEnumerable<ShippingMethodWithRate> shippingMethods)
        {
            // Reset shipping method if necessary
            if (shippingMethods.Any()
                    && !shippingMethods.Any(x => x.ShippingMethodID == shipment.ShippingMethodID))
            {
                var defaultShippingMethod = SelectDefaultShippingMethod(shippingMethods);
                shipment.ShippingMethodID = defaultShippingMethod.ShippingMethodID;
                //shipment.Name = defaultShippingMethod.Name; // shipment.Name should be the name of the person the package is shipping to. - JHE

                shipment.Order.CalculationsDirty = true;
            }
        }

        private void ValidateHostessRewardItem(OrderCustomer customer, IPayment paymentMethod, decimal amount)
        {
            if (paymentMethod.PaymentTypeID == (int)Constants.PaymentType.ProductCredit)
            {
                //Make sure they aren't trying to redeem more than they have - DES
                var service = Create.New<IProductCreditLedgerService>();
                var productCreditBalance = service.GetCurrentBalanceLessPendingPayments(customer.AccountID);
                if (productCreditBalance < amount)
                    throw new NetStepsBusinessException("Not enough product credit.")
                    {
                        PublicMessage = Translation.GetTerm("NotEnoughProductCredit", "Not enough product credit.")
                    };
            }
        }

        public virtual void ValidateOrderCustomers()
        {
            throw new NetStepsException("No customers are associated with this order.")
            {
                PublicMessage = Translation.GetTerm("NoCustomersAreAssociatedWithThisOrder", "No customers are associated with this order.")
            };
        }

        public BasicResponse ValidateOrderItemsByStoreFront(IOrder order)
        {
            BasicResponse response = new BasicResponse();
            response.Success = true;

            //Derek said that the Order/Party contains the Market information for the entire order, no hybrid market orders. SOK
            int orderShippingMarketID = order.GetShippingMarketID();

            if (orderShippingMarketID != 0)
            {
                foreach (OrderCustomer customer in order.OrderCustomers)
                {
                    short? siteTypeID = ApplicationContext.Instance.SiteTypeID;
                    int orderShippingStoreFrontID = ApplicationContext.Instance.StoreFrontID;  //Default

                    if (siteTypeID != null)
                    {
                        var storeFront = SmallCollectionCache.Instance.MarketStoreFronts.FirstOrDefault(m => m.MarketID == orderShippingMarketID
                                && m.SiteTypeID == siteTypeID.Value);

                        if (storeFront == null)
                        {
                            var siteType = SmallCollectionCache.Instance.SiteTypes.GetById(orderShippingMarketID.ToShort());
                            response.Success = false;
                            response.Message = Translation.GetTerm("SiteTypeXcannotbeshippedtomarketX", "Site Type {0} cannot be shipped to market {1}.", siteType.GetTerm(), SmallCollectionCache.Instance.Markets.GetById(orderShippingMarketID).GetTerm());
                            break;
                        }

                        orderShippingStoreFrontID = storeFront.StoreFrontID;
                    }

                    //Don't check kit children or promotional items.
                    foreach (var item in customer.OrderItems.Where(oi => !oi.ParentOrderItemID.HasValue
                        && !oi.OrderAdjustmentOrderLineModifications.Any(lm => lm.ModificationOperationID == (int)NetSteps.OrderAdjustments.Common.Model.OrderAdjustmentOrderLineOperationKind.AddedItem)))
                    {
                        if (!inventoryRepository.IsProductInStoreFront(item.ProductID.Value, orderShippingStoreFrontID))
                        {
                            response.Success = false;
                            response.Message = Translation.GetTerm("ProductXcannotbeshippedtomarketX", "Product {0} cannot be shipped to market {1}.", item.SKU, SmallCollectionCache.Instance.Markets.GetById(orderShippingMarketID).GetTerm());
                            break;
                        }
                    }
                }
            }
            else
            {
                response.Success = false;
                response.Message = Translation.GetTerm("Nomarketcanbefoundforthecurrentshippingaddress", "No market can be found for the current shipping address.");
            }

            return response;
        }

        public virtual ShippingMethodWithRate SelectDefaultShippingMethod(IEnumerable<ShippingMethodWithRate> shippingMethods)
        {
            // Default to lowest shipping amount
            return shippingMethods.OrderBy(x => x.ShippingAmount).FirstOrDefault();
        }

        public virtual IEnumerable<ShippingMethodWithRate> GetShippingMethods(Order order)
        {
            if (order.OrderCustomers == null || order.OrderCustomers.Count == 0)
                throw new InvalidOperationException("Cannot get shipping methods when there are no customers");
            var shipment = order.GetDefaultShipmentNoDefault();
            if (shipment == null)
                throw new InvalidOperationException("Cannot get shipping methods when there are no shipments");

            var shippingMethods = shippingCalculator.GetShippingMethodsWithRates(order).OrderBy(sm => sm.ShippingAmount);

            return shippingMethods;
        }

        public virtual IEnumerable<ShippingMethodWithRate> GetShippingMethods(Order order, OrderShipment orderShipment)
        {
            if (order.OrderCustomers == null || order.OrderCustomers.Count == 0)
                throw new InvalidOperationException("Cannot get shipping methods when there are no customers");

            var shippingMethods = shippingCalculator.GetShippingMethodsWithRates(order, orderShipment).OrderBy(sm => sm.ShippingAmount);

            return shippingMethods;
        }

        public virtual void OnOrderSuccessfullyCompleted(IOrder order)
        {
            #region Creación de PDF y Envío de Email

            //Generar PDF en formato MemoryStream
            MemoryStream memoryStream = new MemoryStream();
            List<string> OrderList = new List<string>();
            OrderList.Add(order.AsOrder().OrderNumber);
            memoryStream = Pdf.GeneratePDFMemoryStream(OrderList);
            string uploadPath = ConfigurationManager.AppSettings["FileUploadAbsolutePath"].ToString() + ConfigurationManager.AppSettings["AttachmentPath"].ToString() + "00000000-0000-0000-0000-000000000000_" + string.Format("Order{0}.pdf", order.AsOrder().OrderNumber);
            using (FileStream file = new FileStream(uploadPath, FileMode.Create, FileAccess.Write))
            {
                memoryStream.WriteTo(file);
            }
            //-----------

            //Enviar el Email del PDF
            var corporateMailAccount = MailAccount.GetCorporateMailAccount();
            var template = EmailTemplate.GetFirstTemplateByTemplateTypeID(NetSteps.Data.Entities.Constants.EmailTemplateType.UserNotification.ToShort());
            if (template != null)
            {
                var translation = template.EmailTemplateTranslations.GetByLanguageID(ApplicationContext.Instance.CurrentLanguageID);
                if (translation != null)
                {
                    //var message = translation.GetTokenReplacedMailMessage(new UserNotificationTokenValueProvider("Juan Perez", "4045", "40", "20", "89", "150"));
                    var message = translation.GetTokenReplacedMailMessage(new UserNotificationTokenValueProvider(order.AsOrder().ConsultantInfo.FullName,
                                                                                                                 order.AsOrder().OrderNumber,
                                                                                                                 order.AsOrder().Subtotal.ToString(),
                                                                                                                 order.AsOrder().TaxAmountTotal.ToString(),
                                                                                                                 order.AsOrder().ShippingTotal.ToString(),
                                                                                                                 order.AsOrder().GrandTotal.ToString()));

                    //byte[] biteArray = new byte[memoryStream.Length];
                    //memoryStream.Position = 0;
                    //memoryStream.Read(biteArray, 0, (int)memoryStream.Length);

                    MailAttachment MailAta = new MailAttachment();
                    MailAta.FileName = string.Format("Order{0}.pdf", order.AsOrder().OrderNumber);
                    if (order.AsOrder().OrderCustomers[0].AccountTypeID != Constants.AccountType.RetailCustomer.ToShort())
                        message.To.Add(new MailMessageRecipient(order.AsOrder().ConsultantInfo.EmailAddress));
                    else if (order.AsOrder().ConsultantInfo.SponsorID != null)
                    {
                        var sponsor = Account.Load(order.AsOrder().ConsultantInfo.SponsorID.ToInt());
                        message.To.Add(new MailMessageRecipient(sponsor.EmailAddress));
                    }
                    message.Attachments.Add(MailAta);
                    message.Send(corporateMailAccount, CurrentSite.SiteID);

                }
                else
                {
                    throw new InvalidOperationException(String.Format("There is no 'UserNotification' email template translation for LanguageId:{0} defined.", ApplicationContext.Instance.CurrentLanguageID));
                }
            }
            else
            {
                throw new InvalidOperationException("There is no 'UserNotification' email template defined.");
            }
            //Fin de Enviar Email
            //-----

            #endregion
        }

        public static string CurrentSiteUrl
        {
            get
            {
                bool isSubdomain = ConfigurationManager.GetAppSetting<bool>(ConfigurationManager.VariableKey.UrlFormatIsSubdomain, true);
                var localPath = System.Web.HttpContext.Current.Request.Url.LocalPath;
                var distributor = !isSubdomain && !string.IsNullOrEmpty(localPath) && Regex.IsMatch(localPath, @"^/\w+") ? localPath.Substring(1, localPath.IndexOf('/', 1) > 0 ? localPath.IndexOf('/', 1) : localPath.Length - 1) : "";
                string currentSiteUrl = "http://" + System.Web.HttpContext.Current.Request.Url.Authority + System.Web.HttpContext.Current.Request.ApplicationPath + distributor;
                return currentSiteUrl;
            }
        }

        public static Site CurrentSite
        {
            get
            {
                var site = SiteCache.GetSiteByUrl(CurrentSiteUrl.EldDecode());
                if (site == null)
                {
                    return null;
                }
                // Update Session SiteOwner var if applicable (not null for base site) - JHE
                int currentSiteOwnerAccountID = 0;

                if (System.Web.HttpContext.Current.Session["SiteOwner"] != null)
                {
                    currentSiteOwnerAccountID = (System.Web.HttpContext.Current.Session["SiteOwner"] as Account).AccountID;
                }

                if (site != null && site.AccountID != null && site.AccountID > 0 && (currentSiteOwnerAccountID == 0 || currentSiteOwnerAccountID != site.AccountID))
                {
                    System.Web.HttpContext.Current.Session["SiteOwner"] = Account.LoadFull(site.AccountID.Value);
                }

                // 2011-11-10, JWL, Added check for current market id to set session variable.
                if (System.Web.HttpContext.Current.Session["CurrentMarketID"] == null)
                {
                    System.Web.HttpContext.Current.Session["CurrentMarketID"] = site.MarketID;
                }
                return site;
            }
        }

        public void SetHostess(IOrder order, IOrderCustomer hostess)
        {
            if (!order.OrderCustomers.Contains(hostess))
                throw new Exception("The order customer must be already added to the order before being set as a hostess.");
            //Set Old Hostess To Customer
            //There shouldn't be more than one hostess but what the heck, let's loop!
            var oldHostesses = order.AsOrder().OrderCustomers.Where(w => w.IsHostess && w.OrderCustomerID != ((OrderCustomer)hostess).OrderCustomerID);
            foreach (OrderCustomer oldHostess in oldHostesses)
            {
                oldHostess.OrderCustomerTypeID = (int)Constants.OrderCustomerType.Normal;
                oldHostess.ClearHostessRewards();
            }
            //Set New Hostess
            ((OrderCustomer)hostess).OrderCustomerTypeID = (int)Constants.OrderCustomerType.Hostess;
        }

        public IOrderCustomer GetHostess(IOrder order)
        {
            return order.AsOrder().OrderCustomers.FirstOrDefault(oc => oc.OrderCustomerTypeID == (int)Constants.OrderCustomerType.Hostess);
        }

        public Nullable<decimal> GetRemainingHostessRewardsCredit(IOrder order)
        {
            if (order.AsOrder().HostessRewardsUsed >= order.AsOrder().HostessRewardsEarned)
                return 0;
            else
                return order.AsOrder().HostessRewardsEarned - order.AsOrder().HostessRewardsUsed;
        }

        public virtual void SetConsultantID(Order order, Account currentAccount, Account siteOwner = null)
        {
            /* 
             * Refactored this method to read better and involve less repetitive logic.  Also changed to handle new configuration
             * regarding who should be the consultant on enrolment order types.
             * 
             * As part of this change, I created a new config app setting, EnrollmentOrderConsultant.
             * It determines what account will get credit for enrollment orders.  
             *    - If the value is set to "Account", the enrolled account will get credit for the enrollment order.
             *    - If the value is set to "Sponsor", the site owner's account will get credit for the enrollment order.
             * Format:
             * <add key="EnrollmentOrderConsultant" value="Account"/>
             * <add key="EnrollmentOrderConsultant" value="Sponsor"/>
             * 
             * Here is an ASCII representation of a cow, just so you know that this change is pretty important.
             * 
             *            (__)
             *            (oo)
             *     /-------\/
             *    / |     ||
             *   *  ||----||
             *      ^^    ^^  
             */

            // default consultantID should always be sponsor
            var consultantType = ConsultantType.Sponsor;

            if (currentAccount.IsOptedOut)
            {
                consultantType = ConsultantType.Corporate;
            }
            // If a distributor is creating an order, they should be set as their own consultant
            else if (currentAccount.AccountTypeID == (int)ConstantsGenerated.AccountType.Distributor)
            {
                var enrollingConsultant = ConfigurationManager.GetAppSetting<string>("EnrollmentOrderConsultant");

                // if the order type is NOT enrollment, OR the order type is enrollment and the EnrollingConsultant is NOT Sponsor
                if (order.OrderTypeID != (short)ConstantsGenerated.OrderType.EnrollmentOrder
                        || (order.OrderTypeID == (short)ConstantsGenerated.OrderType.EnrollmentOrder
                                && enrollingConsultant != "Sponsor"))
                {
                    consultantType = ConsultantType.Account;
                }
            }
            else if (currentAccount.AccountTypeID == (int)ConstantsGenerated.AccountType.RetailCustomer)
            {
                var useSponsor = ConfigurationManager.GetAppSetting(ConfigurationManager.VariableKey.UseSponsorForOrderConsultant, false);

                if (!useSponsor && siteOwner != null && siteOwner.AccountID > 0)
                {
                    consultantType = ConsultantType.SiteOwner;
                }
            }

            switch (consultantType)
            {
                case ConsultantType.Sponsor:
                    if (currentAccount.SponsorID != null && currentAccount.SponsorID > 0)
                    {
                        order.ConsultantID = (int)currentAccount.SponsorID;
                    }
                    break;

                case ConsultantType.SiteOwner:
                    if (siteOwner != null && siteOwner.AccountID > 0)
                    {
                        order.ConsultantID = siteOwner.AccountID;
                    }
                    break;

                case ConsultantType.Account:
                    order.ConsultantID = currentAccount.AccountID;
                    break;
            }

            if (order.ConsultantID == 0)
            {
                order.ConsultantID = ConfigurationManager.GetAppSetting<int>(ConfigurationManager.VariableKey.CorporateAccountID);
            }
        }

        public virtual IEnumerable<IProduct> GetPotentialDynamicKitUpSaleProducts(IOrder order, IOrderCustomer customer, IEnumerable<IProduct> sortedDynamicKitProducts)
        {
            var possibleDynamicKitProducts = new List<IProduct>();
            if (order == null || ((OrderCustomer)customer).IsTooBigForBundling())
            {
                return possibleDynamicKitProducts;
            }

            foreach (var product in sortedDynamicKitProducts)
            {
                if (CanBeDynamicKitUpSold(order, customer, product, 1))
                {
                    possibleDynamicKitProducts.Add(product);
                }
            }

            return possibleDynamicKitProducts;
        }

        public virtual bool CanBeDynamicKitUpSold(IOrder order, IOrderCustomer customer, IProduct product, int numberOfProductsAway)
        {
            if (order == null || product == null || !((Product)product).IsValid)
                return false;

            var dynamicKit = ((Product)product).DynamicKits[0];
            int requiredNumberOfProducts = dynamicKit.DynamicKitGroups.Sum(g => g.MinimumProductCount);
            var groupProductsDictionary = new Dictionary<int, List<int>>();
            int totalKitValidProducts = 0;

            foreach (var group in dynamicKit.DynamicKitGroups)
            {
                groupProductsDictionary.Add(group.DynamicKitGroupID, new List<int>());
            }

            foreach (OrderItem item in customer.OrderItems)
            {
                //only try to upgrade meaning existing item is not a dynamic kit, not currently part of dynamic kit, or is part of one that has fewer items
                if (item.IsHostReward || !IsUpgrade(item, requiredNumberOfProducts) || item.OrderAdjustmentOrderLineModifications.Any())
                {
                    continue;
                }

                for (int i = 0; i < item.Quantity; i++)
                {
                    if (AddToDynamicKitGroup(dynamicKit, item.ProductID.Value, groupProductsDictionary))
                    {
                        totalKitValidProducts++;
                    }
                }
            }

            if (requiredNumberOfProducts - numberOfProductsAway != totalKitValidProducts)
                return false;

            int totalItemsMissingFromBundle = 0;
            foreach (var group in dynamicKit.DynamicKitGroups)
            {
                if (group.MinimumProductCount > groupProductsDictionary[group.DynamicKitGroupID].Count)
                {
                    totalItemsMissingFromBundle = totalItemsMissingFromBundle + (group.MinimumProductCount - groupProductsDictionary[group.DynamicKitGroupID].Count);
                }
            }

            if (totalItemsMissingFromBundle != numberOfProductsAway)
            {
                return false;
            }

            return true;
        }

        private bool IsUpgrade(IOrderItem item, int requiredNumberOfProducts)
        {
            if (inventoryRepository.GetProduct(item.ProductID.Value).IsDynamicKit())
            {
                return false;
            }

            //If there is no parentOrderItem, then this is not a part of a kit, and is therefore upgradable
            if (!((OrderItem)item).ParentOrderItemID.HasValue)
            {
                return true;
            }

            var parentProduct = inventoryRepository.GetProduct(item.ParentOrderItem.ProductID.Value);

            //don't break up static kit
            if (parentProduct.IsStaticKit())
            {
                return false;
            }

            //only try to upgrade meaning existing item is not a dynamic kit, not currently part of dynamic kit, or is part of one that has fewer items
            if (parentProduct != null
                && item.ParentOrderItem != null && parentProduct.IsDynamicKit()
                && requiredNumberOfProducts <= parentProduct.DynamicKits[0].DynamicKitGroups.Sum(g => g.MinimumProductCount))
            {
                return false;
            }

            return true;
        }

        private bool AddToDynamicKitGroup(DynamicKit dynamicKit, int productId, Dictionary<int, List<int>> groupProducts)
        {
            foreach (var group in dynamicKit.DynamicKitGroups)
            {
                if (!CanBeAddedToDynamicKitGroup(productId, group))
                {
                    continue;
                }

                if (groupProducts[group.DynamicKitGroupID].Count < group.MinimumProductCount)
                {
                    groupProducts[group.DynamicKitGroupID].Add(productId);
                    return true;
                }

                for (int i = 0; i < groupProducts[group.DynamicKitGroupID].Count; i++)
                {
                    var productId2 = groupProducts[group.DynamicKitGroupID][i];
                    foreach (var group2 in dynamicKit.DynamicKitGroups)
                    {
                        if (group2.DynamicKitGroupID == group.DynamicKitGroupID)
                        {
                            continue;
                        }

                        if (!CanBeAddedToDynamicKitGroup(productId2, group2))
                        {
                            continue;
                        }

                        if (groupProducts[group2.DynamicKitGroupID].Count >= group2.MinimumProductCount)
                        {
                            continue;
                        }

                        //Swap
                        groupProducts[group2.DynamicKitGroupID].Add(productId2);
                        groupProducts[group.DynamicKitGroupID].Add(productId);

                        groupProducts[group.DynamicKitGroupID].RemoveAt(i);
                        return true;
                    }
                }
            }

            return false;
        }

        public virtual bool CanBeAddedToDynamicKitGroup(int productId, DynamicKitGroup group)
        {
            var product = inventoryRepository.GetProduct(productId);
            //dynamic kit cannot be added to another dynamic kit
            if (!product.IsDynamicKit())
            {
                if (group.DynamicKitGroupRules.Any(r => (r.ProductTypeID == product.ProductBase.ProductTypeID || r.ProductID == productId) && r.Include))
                {
                    if (!group.DynamicKitGroupRules.Any(r => (r.ProductID == productId) && !r.Include))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public virtual string ConvertToDynamicKit(IOrder order, IOrderCustomer customer, IProduct kitProduct)
        {
            if (kitProduct == null)
                return string.Empty;

            Order clonedOrder = order.AsOrder().Clone();
            OrderCustomer clonedCustomer = clonedOrder.OrderCustomers.GetByGuid(((OrderCustomer)customer).Guid.ToString("N"));

            AddOrUpdateOrderItem(order, customer, new List<OrderItemUpdateInfo>() { new OrderItemUpdateInfo() { ProductID = ((Product)kitProduct).ProductID, Quantity = 1 } }, false);
            var bundleItem = ((OrderCustomer)customer).OrderItems.LastOrDefault(oi => oi.ProductID.Value == ((Product)kitProduct).ProductID);
            var parentGuid = bundleItem != null ? bundleItem.Guid.ToString("N") : "";
            var dynamicKit = ((Product)kitProduct).DynamicKits[0];
            int requiredNumberOfProducts = dynamicKit.DynamicKitGroups.Sum(g => g.MinimumProductCount);
            var groupProductsDictionary = new Dictionary<int, List<int>>();

            foreach (var group in dynamicKit.DynamicKitGroups)
            {
                groupProductsDictionary.Add(group.DynamicKitGroupID, new List<int>());
            }

            var productList = new List<int>();
            foreach (OrderItem item in customer.OrderItems)
            {
                //only try to upgrade meaning existing item is not a dynamic kit, not currently part of dynamic kit, or is part of one that has fewer items
                if (!item.IsHostReward && IsUpgrade(item, requiredNumberOfProducts) && !item.OrderAdjustmentOrderLineModifications.Any())
                {
                    for (int i = 0; i < item.Quantity; i++)
                    {
                        productList.Add(item.ProductID.Value);
                        AddToDynamicKitGroup(dynamicKit, item.ProductID.Value, groupProductsDictionary);
                    }
                }
            }

            var productsToRemove = new List<int>();
            var itemsToDelete = new List<OrderItem>();
            var itemUpdates = new List<OrderItemUpdateInfo>();
            var dynamicKitItemGuidsToDelete = new List<string>();

            foreach (var groupId in groupProductsDictionary.Keys)
            {
                foreach (var tempProductId in groupProductsDictionary[groupId])
                {
                    if (productList.Contains(tempProductId))
                    {
                        productsToRemove.Add(tempProductId);
                        itemUpdates.Add(new OrderItemUpdateInfo() { ProductID = tempProductId, Quantity = 1, OverrideQuantity = false, ParentGuid = parentGuid, DynamicKitGroupID = groupId });
                    }
                }
            }

            foreach (OrderItem item in customer.OrderItems)
            {
                //only try to upgrade meaning existing item is not a dynamic kit, not currently part of dynamic kit, or is part of one that has fewer items
                if (!item.IsHostReward && IsUpgrade(item, requiredNumberOfProducts) && !item.OrderAdjustmentOrderLineModifications.Any())
                {
                    int nonChildItems = item.Quantity;
                    for (int i = 0; i < item.Quantity; i++)
                    {
                        if (productsToRemove.Contains(item.ProductID.Value))
                        {
                            nonChildItems--;
                            productsToRemove.Remove(item.ProductID.Value);
                        }
                    }
                    if (nonChildItems != item.Quantity)
                    {
                        if (nonChildItems == 0)
                        {
                            itemsToDelete.Add(item);
                        }
                        else
                        {
                            itemUpdates.Add(new OrderItemUpdateInfo() { ProductID = item.ProductID.Value, Quantity = nonChildItems, OverrideQuantity = true, ParentGuid = item.ParentOrderItem == null ? null : item.ParentOrderItem.Guid.ToString("N"), DynamicKitGroupID = item.DynamicKitGroupID });
                        }
                        //need to disolve other dynamic kits of which this item was a part
                        if (item.ParentOrderItem != null && inventoryRepository.GetProduct(item.ParentOrderItem.ProductID.Value).IsDynamicKit())
                        {
                            if (!dynamicKitItemGuidsToDelete.Contains(item.ParentOrderItem.Guid.ToString("N")))
                                dynamicKitItemGuidsToDelete.Add(item.ParentOrderItem.Guid.ToString("N"));
                        }
                    }
                }
            }

            foreach (var item in itemsToDelete)
            {
                order.RemoveItem(item);
            }

            var itemGuidsToUnassociate = new List<string>();

            foreach (OrderItemUpdateInfo update in itemUpdates.OrderBy(x => x.DynamicKitGroupID))
            {
                AddOrUpdateOrderItem((Order)order,
                    customer,
                    new List<OrderItemUpdateInfo> { update },
                    update.OverrideQuantity,
                    parentGuid: update.ParentGuid,
                    dynamicKitGroupId: update.DynamicKitGroupID
                );
            }

            foreach (string itemGuid in dynamicKitItemGuidsToDelete)
            {
                var kitItem = ((OrderCustomer)customer).OrderItems.GetByGuid(itemGuid);
                foreach (OrderItem childItem in kitItem.ChildOrderItems)
                {
                    itemGuidsToUnassociate.Add(childItem.Guid.ToString("N"));
                }
                foreach (string childItemGuid in itemGuidsToUnassociate)
                {
                    var childItem = ((OrderCustomer)customer).OrderItems.GetByGuid(childItemGuid);
                    if (childItem != null)
                    {
                        childItem.ParentOrderItemID = null;
                    }
                }

                var item = customer.OrderItems.FirstOrDefault(oi => ((OrderItem)oi).Guid.ToString("N") == itemGuid);
                if (item == null)
                {
                    throw new Exception(string.Format("There are no items on the orderCustomer {1} with guid: {0}", itemGuid, ((OrderCustomer)customer).OrderCustomerID));
                }

                RemoveOrderItem(customer, item);
            }

            if (customer.OrderItems.Sum(oi => oi.Quantity) != (clonedCustomer.OrderItems.Sum(oi => oi.Quantity) + 1) - dynamicKitItemGuidsToDelete.Count)
            {
                //something went wrong in conversion to kit so revert
                order = clonedOrder;

                return string.Empty;
            }

            return parentGuid;
        }

        public virtual IProduct GetRestockingFeeProduct()
        {
            string restockingFeeSku = ConfigurationManager.GetAppSetting<string>(ConfigurationManager.VariableKey.RestockingFeeSKU);

            if (string.IsNullOrEmpty(restockingFeeSku))
            {
                restockingFeeSku = "RestockingFeeSKU";
            }

            Product product = null;
            try
            {
                product = inventoryRepository.GetProduct(restockingFeeSku);
            }
            catch (Exception)
            {
                //restocking fee product not found
            }
            return product;
        }

        public virtual void UpdateCustomer(Order order, Account account)
        {
            if (order.OrderCustomers.Count == 0)
            {
                order.AddNewCustomer(account);
            }
            else
            {
                var customer = order.OrderCustomers[0];
                customer.AccountID = account.AccountID;
            }
        }

        public virtual List<DateTime> GetCompletedOrderDates(IOrderRepository repository, int? orderTypeID = null, int? parentOrderID = null, int? orderCustomerAccountID = null, Constants.SortDirection sortDirection = Constants.SortDirection.Ascending, int? pageSize = null)
        {
            try
            {
                var results = repository.GetCompletedOrderDates(
                    orderTypeID: orderTypeID,
                    parentOrderID: parentOrderID,
                    orderCustomerAccountID: orderCustomerAccountID,
                    sortDirection: sortDirection,
                    pageSize: pageSize
                );

                return results;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public virtual IEnumerable<HostessRewardRule> GetApplicableHostessRewardRules(Order order, int? marketID = null)
        {
            if (order.OrderTypeID == ConstantsGenerated.OrderType.PartyOrder.ToInt())
            {
                var party = Party.LoadFullByOrderID(order.OrderID);

                return party.GetApplicableHostessRewardRules(marketID);
            }

            return new List<HostessRewardRule>();
        }

        public virtual List<IOrderItem> GetHostessRewardOrderItems(IOrder order)
        {
            try
            {
                var hostessRewardOrderItemTypeIDs = SmallCollectionCache.Instance.OrderItemTypes.Where(o => o.IsHostessReward).Select(o => o.OrderItemTypeID).ToList();
                var orderItems = order.OrderCustomers.SelectMany(c => c.OrderItems).Where(oi => hostessRewardOrderItemTypeIDs.Contains(((OrderItem)oi).OrderItemTypeID)).OrderBy(oi => ((OrderItem)oi).OrderItemTypeID).ToList();

                return orderItems;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public virtual bool IsPaidInFull(IOrder order)
        {
            Contract.Requires<ArgumentNullException>(order != null);

            // Return orders have negative values.
            if (order.OrderTypeID == (short)Constants.OrderType.ReturnOrder)
            {
                return totalsCalculator.SumCompletedOrderPayments(order.AsOrder()) <= (order.GrandTotal ?? 0);
            }
            else
            {
                // All other order types.
                return totalsCalculator.SumCompletedOrderPayments(order.AsOrder()) >= (order.GrandTotal ?? 0);
            }
        }

        public virtual void FinalizeCommissionableValue(Order order) { }

        protected virtual List<int> GetProductCreditPayments()
        {
            return new List<int> { (int)Constants.PaymentType.ProductCredit };
        }

        /// <summary>
        /// Analyzes the OrderShipments to determine if the order is not shipped, partially shipped, or fully shipped.
        /// If nothing has shipped, returns "Printed" status.
        /// </summary>
        public virtual short CalculateOrderShippedStatus(IOrder order)
        {
            Contract.Requires<ArgumentNullException>(order != null);

            // Ignore cancelled shipments.
            var orderShipmentStatuses = order.AsOrder().OrderShipments
                .Select(x => x.OrderShipmentStatusID)
                .Where(x => x != (short)Constants.OrderShipmentStatus.Cancelled)
                .ToList();

            if (orderShipmentStatuses.All(x => x == (short)Constants.OrderShipmentStatus.Shipped))
            {
                // All shipments are shipped.
                return (short)Constants.OrderStatus.Shipped;
            }
            else if (orderShipmentStatuses.Any(x => x == (short)Constants.OrderShipmentStatus.Shipped || x == (short)Constants.OrderShipmentStatus.PartiallyShipped))
            {
                // Some shipments are shipped.
                return (short)Constants.OrderStatus.PartiallyShipped;
            }
            else
            {
                // No shipments are shipped, assume "Printed".
                return (short)Constants.OrderStatus.Printed;
            }
        }

        public virtual IList<int> GetValidOrderStatusIdsForOrderAdjustment()
        {
            return new[]
			{
				(int)Constants.OrderStatus.Pending,
				(int)Constants.OrderStatus.PendingError,
				(int)Constants.OrderStatus.PartiallyPaid,
				(int)Constants.OrderStatus.CreditCardDeclined,
			};
        }

        public virtual bool ShouldDividePartyShipping()
        {
            return false;
        }

        public void RemoveOrderItem(IOrderCustomer orderCustomer, IOrderItem orderItem)
        {
            if (orderItem == null)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(new Exception("Item is not on the order."), Constants.NetStepsExceptionType.NetStepsBusinessException, ((OrderCustomer)orderCustomer).Order.OrderID);
            }

            if (((OrderCustomer)orderCustomer).Order.IsCommissionable())
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(new Exception("Unable to remove items on orders in a commissionable status."), Constants.NetStepsExceptionType.NetStepsBusinessException, ((OrderCustomer)orderCustomer).Order.OrderID);
            }

            try
            {
                // Delete child order items first
                if (((OrderItem)orderItem).ChildOrderItems != null)
                {
                    //List<OrderItem> tempCollection = new List<OrderItem>();
                    //tempCollection.AddRange(orderItem.ChildOrderItems);
                    foreach (OrderItem childItem in ((OrderItem)orderItem).ChildOrderItems.ToList())
                    {
                        // Prevent loop
                        if (!childItem.Equals(orderItem))
                        {
                            childItem.OrderItemPrices.RemoveAllAndMarkAsDeleted();
                            childItem.OrderItemProperties.RemoveAllAndMarkAsDeleted();
                            if (childItem.ChangeTracker.State != ObjectState.Added)
                                childItem.MarkAsDeleted();
                            ((OrderItem)orderItem).ChildOrderItems.Remove(childItem);

                            if (orderCustomer.OrderItems.Contains(childItem))
                            {
                                orderCustomer.OrderItems.Remove(childItem);
                            }
                        }
                    }
                }

                ((OrderItem)orderItem).OrderItemPrices.RemoveAllAndMarkAsDeleted();
                ((OrderItem)orderItem).OrderItemProperties.RemoveAllAndMarkAsDeleted();
                if (((OrderItem)orderItem).ChangeTracker.State != ObjectState.Added)
                {
                    orderItem.MarkAsDeleted();
                }

                //Remove this from it's parent item if in bundle
                if (orderItem.ParentOrderItem != null)
                {
                    var parentOrderItem = orderItem.ParentOrderItem;
                    ((OrderItem)parentOrderItem).ChildOrderItems.Remove((OrderItem)orderItem);
                }

                var index = orderCustomer.OrderItems.IndexOf(orderItem);

                //If the item exists in the collection
                if (index != -1)
                {
                    orderCustomer.OrderItems.RemoveAt(index);
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException, ((OrderCustomer)orderCustomer).Order.OrderID);
            }
        }

        public bool TryCancel(IOrder order, out string message)
        {
            message = String.Empty;
            bool statusHasChaged = false;
            Constants.OrderStatus originalOrderStatusID = (Constants.OrderStatus)order.OrderStatusID;
            Constants.OrderStatus newOrderStatusID = originalOrderStatusID;
            bool allowClearAllocation = false;
            bool allowApplyCredit = false;

            switch ((Constants.OrderStatus)order.OrderStatusID)
            {
                case Constants.OrderStatus.Pending:
                    newOrderStatusID = Constants.OrderStatus.Cancelled;
                    allowClearAllocation = true;
                    break;

                case Constants.OrderStatus.PendingPerPaidConfirmation:
                    newOrderStatusID = Constants.OrderStatus.Cancelled;
                    allowClearAllocation = true;
                    allowApplyCredit = true;
                    break;

                case Constants.OrderStatus.PartiallyPaid:
                    newOrderStatusID = Constants.OrderStatus.CancelledPaid;
                    allowClearAllocation = true;
                    allowApplyCredit = true;
                    break;

                case Constants.OrderStatus.Paid:
                    newOrderStatusID = Constants.OrderStatus.CancelledPaid;
                    allowClearAllocation = true;
                    break;

                default:
                    message = Translation.GetTerm("UnableToCancelAnOrderOfSatus{0}", "Unable to cancel an order of status {0}.", SmallCollectionCache.Instance.OrderStatuses.GetById(order.OrderStatusID).GetTerm());
                    break;
            }

            statusHasChaged = originalOrderStatusID != newOrderStatusID;

            if (statusHasChaged)
            {
                order.OrderStatusID = (short)newOrderStatusID;
                order.Save();

                if (allowApplyCredit)
                {
                    NetSteps.Data.Entities.Business.CTE.ProcedimientoCuentaCorriente(order.OrderID, "C", 0, ApplicationContext.Instance.CurrentUserID);

                    //if (originalOrderStatusID == Constants.OrderStatus.PartiallyPaid)
                    //{
                    //    decimal creditAmountDecimal = order.AsOrder().OrderPayments.Sum(m => m.Amount);
                    //    ApplyCredit(order.OrderCustomers[0].AccountID, creditAmountDecimal, order.OrderID, null);
                    //}
                    //else
                    //{
                    //    decimal creditAmountDecimal = order.OrderCustomers.SelectMany(m => m.OrderItems).Sum(k => k.Quantity * k.ItemPrice);
                    //    ApplyCredit(order.OrderCustomers[0].AccountID, creditAmountDecimal, order.OrderID, order.ParentOrderID);
                    //}
                }


                if (allowClearAllocation)
                {
                    //ClearAllocation(order);
                }
            }

            return statusHasChaged;
        }

        /// <summary>
        /// Invocation to Clear allocation process
        /// </summary>
        /// <param name="order"></param>
        private void ClearAllocation(IOrder order)
        {
            //Order.PreOrders = new PreOrder();
            //foreach (var item in order.OrderCustomers.SelectMany(m => m.OrderItems))
            //{

            //    Order.PreOrders.WareHouseID = WarehouseExtensions.WareHouseByAddresID(order.OrderCustomers[0].AccountID);
            //    Order.UpdateLineOrder(item.ProductID.Value, item.Quantity, Order.PreOrders.WareHouseID);
            //}
        }

        /// <summary>
        /// Llama al proceso ApplyCredit
        /// </summary>
        /// <param name="accountId">Id de cuenta</param>       
        /// <param name="entryAmount">Monto a devolver</param>
        /// <param name="orderId">Identificador de la Orden de Devolución OrderID</param>
        /// <param name="orderPaymentId">Identificador de la Orden Padre ParentOrderID</param>
        private void ApplyCredit(int accountId, decimal entryAmount, int orderId, int? orderPaymentId)
        {
            int entryReasonId = (int)NetSteps.Data.Entities.Generated.ConstantsGenerated.LedgerEntryReasons.ProductReturn;
            int entryOrigin = (int)NetSteps.Data.Entities.Generated.ConstantsGenerated.LedgerEntryOrigins.OrderEntry;
            int entryTypeId = (int)NetSteps.Data.Entities.Generated.ConstantsGenerated.LedgerEntryTypes.ReturnAdjustment;

            NetSteps.Data.Entities.Business.CTE.ApplyCredit(accountId, entryReasonId, entryOrigin, entryTypeId, entryAmount, orderId, orderPaymentId);
        }

    }

    enum ConsultantType
    {
        Corporate,
        Sponsor,
        SiteOwner,
        Account
    }
}
