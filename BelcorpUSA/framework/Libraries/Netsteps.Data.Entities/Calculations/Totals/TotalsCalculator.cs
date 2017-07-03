using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using NetSteps.Common.Extensions;
using NetSteps.Common.Interfaces;
using NetSteps.Data.Common.Entities;
using NetSteps.Data.Common.Services;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Generated;
using NetSteps.Data.Entities.Interfaces;
using NetSteps.Data.Entities.Services;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Data.Entities
{
    /// <summary>
    /// Default Implementation of TotalsCalculator. This class is overridable for per client customizations.
    /// </summary>
    [ContainerRegister(typeof(ITotalsCalculator), RegistrationBehaviors.Default)]
    public class TotalsCalculator : ITotalsCalculator, IDefaultImplementation
    {
        #region Order Totals

        public virtual void CalculateOrder(Order order, short? accountTypeIDToUseForCalculations = null)
        {
            // If the order is commissionable we should not be modifying its values.
            if (order.IsCommissionable())
                return;

            lock (order)
            {
                try
                {
                    order.StartEntityTracking();
                    ResetOrderTotals(order);
                    ValidateAndAttachDefaultShipment(order);

                    // No need to calculate on Comp Orders
                    if (order.OrderTypeID == Constants.OrderType.CompOrder.ToInt())
                        return;

                    if (order.OrderTypeID == Constants.OrderType.PartyOrder.ToInt())
                        CalculateHostessRewardsEarned(order);

                    foreach (OrderCustomer orderCustomer in order.OrderCustomers.ToList())
                    {
                        orderCustomer.StartEntityTracking();
                        ResetOrderCustomerTotals(orderCustomer);
                        CalculateOrderCustomerProductTotals(orderCustomer, accountTypeIDToUseForCalculations);
                        CustomCalculations(orderCustomer, order);
                    }

                    if (order.OrderPendingState == Constants.OrderPendingStates.Quote)
                    {
                        order.CalculatePartyShipping();
                        order.CalculatePartyTax();
                    }

                    Parallel.ForEach<OrderCustomer>(order.OrderCustomers.ToList(), (customer) => CalculateOrderCustomerTotals(customer));

                    this.CalculateOrderTotals(order);

                    if (order.OrderTypeID == Constants.OrderType.PartyOrder.ToInt())
                        CalculateHostessRewardsUsed(order);

                    CalculateOrderBalances(order);
                }
                catch (Exception ex)
                {
                    throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
                }
            }
        }

        protected virtual void CalculateOrderTotals(Order order)
        {

            Contract.Requires(order != null);
            decimal? amount = 0;
            bool isPayment = false;
            if (Order.paymentReturn != null && Order.paymentReturn.Count > 0)
            {
                foreach (var item in Order.paymentReturn)
                {
                    amount = amount + item.Amount;
                }
                isPayment = true;
            }
            foreach (OrderCustomer orderCustomer in order.OrderCustomers)
            {
                order.ShippingTotal += orderCustomer.AdjustedShippingTotal;
                order.TaxableTotal += orderCustomer.TaxableTotal.ToDecimal();
                order.TaxAmountOrderItems += orderCustomer.TaxAmountOrderItems.ToDecimal();
                order.TaxAmountShipping += orderCustomer.TaxAmountShipping.ToDecimal();
                order.TaxAmountTotal += orderCustomer.AdjustedTaxTotal;
                order.Subtotal += orderCustomer.AdjustedSubTotal;
                order.SubtotalRetail += orderCustomer.SubtotalRetail;
                order.SubtotalAdjusted += orderCustomer.Subtotal.ToDecimal(); //This was being set to SubtotalHostess, which is just orderCustomer.Subtotal......... !?!?!?!
                order.PaymentTotal += orderCustomer.PaymentTotal.ToDecimal();
                order.HandlingTotal += orderCustomer.AdjustedHandlingTotal;
                order.Balance += orderCustomer.Balance.ToDecimal();
                order.DeferredBalance += orderCustomer.DeferredBalance;
                order.CommissionableTotal += orderCustomer.CommissionableTotal.ToDecimal();
                if (isPayment)
                {
                    order.GrandTotal = orderCustomer.Total.ToDecimal() - amount;
                }
                else
                {
                    order.GrandTotal += orderCustomer.Total.ToDecimal();
                }
            }

            // If we are not overriding taxes the value is included in the customer total and thus already added to the grand total
            if (order.TaxAmountTotalOverride != null)
            {
                order.TaxAmountTotal = order.TaxAmountTotalOverride;
                order.GrandTotal += order.TaxAmountTotalOverride;
            }
            else
            {
                order.TaxableTotal += order.PartyShipmentTotal;
                order.TaxAmountShipping += order.PartyShippingTax;
                order.TaxAmountTotal += order.PartyShippingTax;
                order.GrandTotal += order.PartyShippingTax;
            }

            // If we are not overriding shipping and distributing the party shipping the value is included in the customer total and thus already added to the grand total
            if (order.ShippingTotalOverride != null)
            {
                order.ShippingTotal = order.ShippingTotalOverride;
                order.GrandTotal += order.ShippingTotal + order.HandlingTotal;
            }
            else if (!order.ShouldDividePartyShipping)
            {
                order.ShippingTotal += order.PartyShipmentTotal;
                order.HandlingTotal += order.PartyHandlingTotal;
                order.GrandTotal += order.PartyShipmentTotal + order.PartyHandlingTotal;
            }
        }

        protected virtual void ResetOrderTotals(Order order)
        {
            Contract.Requires(order != null);

            try
            {
                order.Balance = 0;
                order.Subtotal = 0;
                order.GrandTotal = 0;
                order.PaymentTotal = 0;
                order.SubtotalRetail = 0;
                order.SubtotalAdjusted = 0;
                order.DeferredBalance = 0;
                order.CommissionableTotal = 0;
                order.HostessOverage = 0;
                order.HostessRewardsUsed = 0;
                order.HostessRewardsEarned = 0;
                order.TaxableTotal = 0;
                order.TaxAmountTotal = 0;
                order.TaxAmountShipping = 0;
                order.TaxAmountOrderItems = 0;
                order.ShippingTotal = 0;
                order.HandlingTotal = 0;
                if (order.OrderPendingState == Constants.OrderPendingStates.Quote)
                {
                    order.PartyShipmentTotal = 0m;
                    order.PartyHandlingTotal = 0m;
                    order.PartyShippingTax = 0m;
                }
                else
                {
                    //If they have not been previously set we need to set them.  Null values break the calculation.
                    if (!order.PartyShipmentTotal.HasValue)
                        order.PartyShipmentTotal = 0m;
                    if (!order.PartyHandlingTotal.HasValue)
                        order.PartyHandlingTotal = 0m;
                    if (!order.PartyShippingTax.HasValue)
                        order.PartyShippingTax = 0m;
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        /// <summary>
        /// Calculates any payments on the order and updates the PaymentTotal and Balance fields.  Will no longer be necessary once these fields are made to be calculated fields like they should be.
        /// </summary>
        /// <param name="order"></param>
        protected virtual void CalculateOrderBalances(Order order)
        {
            Contract.Requires(order != null);

            try
            {
                order.PaymentTotal += order.OrderPayments.Where(op => !op.OrderCustomerID.HasValue && (op.OrderPaymentStatusID == (int)Constants.OrderPaymentStatus.Pending || op.OrderPaymentStatusID == (int)Constants.OrderPaymentStatus.Completed)).Sum(p => p.Amount).GetRoundedNumber();
                order.Balance = order.GrandTotal - order.PaymentTotal.ToDecimal() - order.DeferredBalance;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        protected virtual void FinalizeTax(Order order)
        {
            Contract.Requires(order != null);

            try
            {
                foreach (OrderCustomer orderCustomer in order.OrderCustomers)
                {
                    orderCustomer.FinalizeTax();
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        protected void ValidateAndAttachDefaultShipment(Order order)
        {
            Contract.Requires(order != null);

            if ((order.OrderTypeID != (short)Constants.OrderType.PartyOrder
                && order.OrderTypeID != (short)Constants.OrderType.FundraiserOrder)
                && order.OrderShipments.Count == 1
                && order.OrderCustomers.Count == 1
                && !order.OrderCustomers[0].OrderShipments.Any())
            {
                order.OrderCustomers[0].OrderShipments.Add(order.OrderShipments[0]);
            }
        }

        protected virtual void CalculateHostessRewardsEarned(Order order)
        {
            Contract.Requires(order != null);

            Party party = null;
            if (order.Parties != null && order.Parties.Count > 0)
                party = order.Parties.FirstOrDefault();

            if (party == null)
                return;

            var totalHostessRewards = SumHostessRewards(order);
            var applicableHostessRewards = party.GetApplicableHostessRewardRules();

            var totalHostessCredit = 0M;
            foreach (var reward in applicableHostessRewards)
            {
                if (reward.Reward.HasValue)
                    totalHostessCredit += reward.Reward.Value;
                else if (reward.CreditPercent.HasValue)
                    totalHostessCredit += totalHostessRewards * reward.CreditPercent.Value;
            }

            order.HostessRewardsEarned = Math.Round(totalHostessCredit, 2);
        }

        protected virtual void CalculateHostessRewardsUsed(Order order)
        {
            Contract.Requires(order != null);

            // This line is not needed as HostessRewardsUsed is incremented when a HostCredit is redeemed.  Leaving it here for reference only.
            //order.HostessRewardsUsed = order.OrderCustomers.SelectMany(oc => oc.OrderItems).Where(oi => oi.OrderItemTypeID == (int)Constants.OrderItemType.HostCredit).Sum(oi => oi.Discount);
            order.HostessOverage = order.OrderCustomers.SelectMany(oc => oc.OrderItems).Where(oi => oi.OrderItemTypeID == (int)Constants.OrderItemType.HostCredit).Sum(oi => oi.GetAdjustedPrice() * oi.Quantity);
        }

        public virtual decimal SumHostessRewards(Order order)
        {
            var sum = 0.0m;
            if (order != null)
            {
                sum = SumOrderItemsForHostessRewards(order);
                foreach (var childOrder in order.OnlineChildOrders.Where(o => o.OrderTypeID != (int)Constants.OrderType.ReturnOrder))
                {
                    sum += SumOrderItemsForHostessRewards(childOrder);
                }
            }
            return sum;
        }

        protected virtual decimal SumOrderItemsForHostessRewards(Order order)
        {
            Contract.Requires(order != null);

            decimal sum = 0M;
            if (order.OrderCustomers.Count > 0 && order.OrderCustomers.Any(oc => oc.OrderItems.Count > 0) && (order.OrderTypeID == (short)Constants.OrderType.PartyOrder
                || (order.OrderTypeID != (short)Constants.OrderType.PartyOrder && (order.OrderStatusID != (short)Constants.OrderStatus.CreditCardDeclined) && order.IsCommissionable())))
            {
                foreach (OrderCustomer orderCustomer in order.OrderCustomers)
                {
                    foreach (OrderItem orderItem in orderCustomer.OrderItems.Where(oi => !oi.ParentOrderItemID.HasValue && oi.OrderItemTypeID == (int)Constants.OrderItemType.Retail && !oi.IsHostReward))
                    {
                        var priceTypeID = orderItem.ProductPriceTypeID ?? orderCustomer.ProductPriceTypeID;
                        // add the sum, taking into account price adjustments made through the order adjustment system.  Previous to this edit,
                        // hostess rewards results were calculated based upon retail prices, not the prices actually paid.
                        sum += orderItem.GetAdjustedPrice(priceTypeID) * orderItem.Quantity;
                    }
                }
            }

            return sum;
        }

        [Obsolete("This was the wrong place to override commissionable values.  Override CalculateCommissionableValue instead.")]
        protected virtual void OverrideCommissionOrderTotals(OrderCustomer orderCustomer)
        {

        }

        public virtual decimal SumCompletedOrderPayments(Order order)
        {
            return order.OrderPayments
                .Where(op => op.OrderPaymentStatusID == (short)Constants.OrderPaymentStatus.Completed)
                .Sum(op => op.Amount)
                .GetRoundedNumber();
        }
        #endregion

        #region OrderCustomer Totals
        protected virtual void CalculateOrderCustomerTotals(OrderCustomer orderCustomer, short? accountTypeIDToUseForCalculations = null)
        {
            Contract.Requires(orderCustomer != null);

            try
            {
                // Do not calculate shipping or taxes unless the pending state is Quote.
                if (orderCustomer.Order.OrderPendingState == Constants.OrderPendingStates.Quote || orderCustomer.Order.OrderPendingState == Constants.OrderPendingStates.Open)
                {
                    switch ((Constants.OrderType)orderCustomer.Order.OrderTypeID)
                    {
                        case Constants.OrderType.ReturnOrder:
                            orderCustomer.CalculateReturnOrderTax();
                            break;
                        case Constants.OrderType.CompOrder:
                            break;
                        default:
                            orderCustomer.CalculateShipping();
                            int countryId = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["EnvironmentCountry"]);
                            if (countryId == (int)Constants.Country.UnitedStates)
                            {
                                orderCustomer.CalculateTax(); //Antonio Campos 2015/01/04 Comentado 
                            }
                            break;
                    }
                }

                var feesTotal = orderCustomer.ParentSubtotalOrderItemTotalByType(Constants.OrderItemType.Fees);

                orderCustomer.Total =
                    orderCustomer.AdjustedSubTotal
                        + orderCustomer.AdjustedShippingTotal
                        + orderCustomer.AdjustedTaxTotal
                        + orderCustomer.AdjustedHandlingTotal
                        + feesTotal;

                CalculateOrderCustomerBalances(orderCustomer);
                orderCustomer.CalculationsDirty = false;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        protected virtual void CalculateOrderCustomerProductTotals(OrderCustomer orderCustomer, short? accountTypeIDToUseForCalculations = null)
        {
            Contract.Requires(orderCustomer != null);

            try
            {
                var order = orderCustomer.Order;
                short accountTypeID = accountTypeIDToUseForCalculations != null ? accountTypeIDToUseForCalculations.Value : orderCustomer.AccountTypeID;

                var inventory = Create.New<InventoryBaseRepository>();
                foreach (OrderItem orderItem in orderCustomer.OrderItems.ToList())
                {
                    // If we have no quantity there is nothing to calculate.
                    if (orderItem.Quantity == 0)
                        continue;

                    orderItem.StartEntityTracking();
                    CalculateProductTotals(orderCustomer, inventory, orderItem, accountTypeID, order);
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        protected virtual void CalculateProductTotals(OrderCustomer orderCustomer, InventoryBaseRepository inventory, OrderItem orderItem, short accountTypeID, Order order)
        {
            Contract.Requires(orderCustomer != null);

            var product = inventory.GetProduct(orderItem.ProductID.ToInt());
            CalculatePriceTypes(accountTypeID, orderItem, order, product);

            //set the ItemPrice on the orderItem to the new itemPrice
            orderItem.SetItemPrice(orderCustomer.ProductPriceTypeID);

            if (orderCustomer.Order.OrderTypeID != Constants.OrderType.ReplacementOrder.ToInt())
            {
                CalculateCommissionableValue(orderCustomer, orderItem, order, product);
                orderCustomer.CommissionableTotal += orderItem.CommissionableTotal;
            }

            RedeemHostessReward(orderItem);

            // Don't include fees in taxable subtotals
            if (orderItem.OrderItemTypeID != Constants.OrderItemType.Fees.ToInt())
            {
                orderCustomer.Subtotal += (orderItem.GetAdjustedPrice() * orderItem.Quantity).GetRoundedNumber();
                if (orderItem.ParentOrderItemID == null) orderCustomer.QtySubtotal += orderItem.Quantity;
            }

            switch (orderItem.OrderItemTypeID)
            {
                case (int)Constants.OrderItemType.Retail:
                    orderCustomer.SubtotalRetail += orderItem.GetAdjustedPrice() * orderItem.Quantity;
                    orderCustomer.QtyRetail += orderItem.Quantity;
                    break;
                default:
                    orderCustomer.SubtotalHostessDiscounted += orderItem.Discount.ToDecimal();
                    break;
            }

            //CGI(CMR)-06/04/2015-Inicio
            var orderItemPrices = orderItem.OrderItemPrices;
            OrderItemPrice qvPrice = new OrderItemPrice();
            OrderItemPrice retailPrice = new OrderItemPrice();
            foreach (var q in orderItemPrices)
            {
                var pType = ProductPriceType.Load(q.ProductPriceTypeID);
                if (pType.Name.Equals("QV"))
                {
                    qvPrice = q;
                    orderCustomer.SubTotalQV += (qvPrice.UnitPrice * orderItem.Quantity);
                }
                if (q.ProductPriceTypeID.Equals(orderCustomer.ProductPriceTypeID)
                    && orderItem.HasChanges) //recien agregado para los productos FREE
                //if (pType.Name.Equals("Retail")) 
                {
                    retailPrice = q;
                    decimal adjustedItemPrice = retailPrice.UnitPrice;
                    decimal preadjustedItemPrice = retailPrice.OriginalUnitPrice.ToDecimal();

                    orderCustomer.SumAdjustedItemPrice += adjustedItemPrice;
                    orderCustomer.SumPreadjustedItemPrice += preadjustedItemPrice;

                    orderCustomer.SubTotalAdjustedItemPrice += (adjustedItemPrice * orderItem.Quantity);
                    orderCustomer.SubTotalPreadjustedItemPrice += (preadjustedItemPrice * orderItem.Quantity);
                }
            }

            //decimal adjustedItemPrice = orderItem.GetAdjustedPrice(orderItem.ProductPriceTypeID.Value);
            //decimal preadjustedItemPrice = orderItem.GetPreAdjustmentPrice(orderItem.ProductPriceTypeID.Value);

            //orderCustomer.GetAdjustedPrice = adjustedItemPrice;
            //orderCustomer.GetPreAdjustmentPrice = preadjustedItemPrice;

            //orderCustomer.AdjustedItemPrice += (adjustedItemPrice * orderItem.Quantity).GetRoundedNumber();
            //orderCustomer.PreadjustedItemPrice += (preadjustedItemPrice * orderItem.Quantity).GetRoundedNumber();
            //CGI(CMR)-06/04/2015-Fin
        }

        protected virtual void ResetOrderCustomerTotals(OrderCustomer orderCustomer)
        {
            Contract.Requires(orderCustomer != null);

            try
            {
                orderCustomer.Subtotal = 0;
                orderCustomer.SubtotalHostess = 0;
                orderCustomer.SubtotalHostessDiscounted = 0;
                orderCustomer.Total = 0;
                orderCustomer.SubtotalRetail = 0;
                orderCustomer.CommissionableTotal = 0;
                orderCustomer.Balance = 0;
                orderCustomer.PaymentTotal = 0;
                orderCustomer.DeferredBalance = 0;
                orderCustomer.QtySubtotal = 0;
                orderCustomer.QtyRetail = 0;
                if (orderCustomer.Order.OrderPendingState == Constants.OrderPendingStates.Quote)
                {
                    orderCustomer.ShippingTotal = 0;
                    orderCustomer.HandlingTotal = 0;
                    orderCustomer.TaxAmountTotal = 0;
                }

                /*CGI(CMR)-09/04/2015-Inicio*/
                orderCustomer.SumAdjustedItemPrice = 0;
                orderCustomer.SumPreadjustedItemPrice = 0;

                orderCustomer.SubTotalAdjustedItemPrice = 0;
                orderCustomer.SubTotalPreadjustedItemPrice = 0;
                /*CGI(CMR)-09/04/2015-Fin*/
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        protected virtual void CustomCalculations(OrderCustomer customer, Order order)
        {
            //Placeholder for custom, client-specific logic
        }

        protected virtual void CalculateOrderCustomerBalances(OrderCustomer orderCustomer)
        {
            Contract.Requires(orderCustomer != null);

            try
            {
                bool isDeferred = false;

                foreach (OrderPayment payment in orderCustomer.OrderPayments.Where(op => op.OrderPaymentStatusID == (int)Constants.OrderPaymentStatus.Pending || op.OrderPaymentStatusID == (int)Constants.OrderPaymentStatus.Completed))
                {
                    orderCustomer.PaymentTotal += payment.Amount;
                    isDeferred = payment.IsDeferred;
                }
                if (isDeferred)
                    orderCustomer.DeferredBalance = orderCustomer.Total.ToDecimal() - orderCustomer.PaymentTotal.ToDecimal();

                orderCustomer.Balance = orderCustomer.Total - orderCustomer.PaymentTotal.ToDecimal() - orderCustomer.DeferredBalance;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        [Obsolete("This method should no longer be necessary.  Instead ensure that any custom PriceTypes are included in the PriceType list and they will be calculated.")]
        protected virtual void ApplyOrderItemPrices(Order order, OrderItem orderItem, Product product, decimal percentOfTotal, bool isReturnOrder, decimal originalCommissionablePrice)
        {
            //Client specific OrderItemPrices
        }

        protected virtual void CalculateCommissionableValue(OrderCustomer orderCustomer, OrderItem orderItem, Order order, Product product)
        {
            // Putting code contracts in this method caused a runtime error in the Jewel Kade inheriting class.
            decimal unitPrice = 0m;
            OrderItemPrice orderItemDefault = orderItem.OrderItemPrices.FirstOrDefault(q => q.ProductPriceTypeID == orderCustomer.CommissionablePriceTypeID);
            if (orderItemDefault != null)
                unitPrice = orderItemDefault.UnitPrice * orderItem.Quantity;

            orderItem.CommissionableTotal = unitPrice;
        }

        protected virtual void CalculatePriceTypes(short accountTypeID, OrderItem orderItem, Order order, Product product)
        {
            Contract.Requires(orderItem != null);
            Contract.Requires(order != null);
            Contract.Requires(product != null);

            //if the item type is anything other than retail or not set, then it is a discount item. In which case we have to use retail pricing.
            accountTypeID = orderItem.OrderItemTypeID == (int)ConstantsGenerated.OrderItemType.Retail ||
                orderItem.OrderItemTypeID == (int)ConstantsGenerated.OrderItemType.NotSet
                    ? accountTypeID
                    : (short)ConstantsGenerated.AccountType.RetailCustomer;
            var pricingService = Create.New<IProductPricingService>();
            var priceTypeService = Create.New<IPriceTypeService>();
            var priceTypes = new List<IPriceType>();
            priceTypes.AddRange(priceTypeService.GetCurrencyPriceTypes());
            priceTypes.AddRange(priceTypeService.GetVolumePriceTypes());
            foreach (var priceType in priceTypes)
            {
                if (!pricingService.ProductContainsPrice(product, accountTypeID, order.CurrencyID, order.OrderTypeID) && !pricingService.ProductIsDynamicPricedKit(product))
                    throw new Exception(string.Format("No Price found for product: {0} ({1}) with Price Type of {2} ({3})", product.Name, product.ProductID, orderItem.ProductPriceTypeID.HasValue ? SmallCollectionCache.Instance.ProductPriceTypes.GetById(orderItem.ProductPriceTypeID.Value).Name : "", orderItem.ProductPriceTypeID));

                if (!pricingService.ProductContainsPrice(product, accountTypeID, Constants.PriceRelationshipType.Commissions, order.CurrencyID, order.OrderTypeID) && !pricingService.ProductIsDynamicPricedKit(product))
                    throw new Exception(string.Format("No 'Commissionable' price found for product: {0}({1})", product.Name, product.ProductID));

                /*CS:10MAY2016.Inicio.Error al Obtener mas de un Elemento:Comentado por Antonio*/
                //var orderItemPrice = orderItem.OrderItemPrices.SingleOrDefault(oip => oip.ProductPriceTypeID == priceType.PriceTypeID);
                var orderItemPrice = orderItem.OrderItemPrices.FirstOrDefault(oip => oip.ProductPriceTypeID == priceType.PriceTypeID);
                /*CS:10MAY2016.Fin*/
                if (orderItemPrice == null)
                {
                    orderItemPrice = new OrderItemPrice();
                    orderItemPrice.ProductPriceTypeID = priceType.PriceTypeID;
                    orderItem.OrderItemPrices.Add(orderItemPrice);
                }

                // If the item belongs to a bundle then all OrderItemPrices must be 0.
                if (!orderItem.OrderCustomer.Order.IsReturnOrder())
                {
                    orderItemPrice.OriginalUnitPrice = orderItem.ParentOrderItemID == null ? pricingService.GetPrice(product, priceType.PriceTypeID, order.CurrencyID) : 0;
                    orderItemPrice.UnitPrice = orderItem.ParentOrderItemID == null ? orderItem.GetAdjustedPrice(priceType.PriceTypeID) : 0;
                }


            }
        }

        protected virtual void RedeemHostessReward(OrderItem orderItem)
        {
            Contract.Requires(orderItem != null);

            if (!orderItem.IsHostReward)
                return;
            var hostessRewardRuleID = orderItem.HostessRewardRuleID;
            if (hostessRewardRuleID == null)
                return;
            try
            {
                var order = orderItem.OrderCustomer.Order;
                switch ((Constants.OrderItemType)orderItem.OrderItemTypeID)
                {
                    case Constants.OrderItemType.HostCredit:
                        var remainingHostessRewards = order.HostessRewardsEarned - order.HostessRewardsUsed;
                        orderItem.Discount = (orderItem.ItemPrice * orderItem.Quantity) <= remainingHostessRewards ? (orderItem.ItemPrice * orderItem.Quantity) : remainingHostessRewards;
                        order.HostessRewardsUsed += orderItem.Discount;
                        break;
                    case Constants.OrderItemType.PercentOff:
                    case Constants.OrderItemType.ItemDiscount:
                    case Constants.OrderItemType.BookingCredit:
                        orderItem.DiscountPercent = SmallCollectionCache.Instance.HostessRewardRules.GetById(hostessRewardRuleID.Value).ProductDiscount;
                        break;
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        #endregion
    }
}
