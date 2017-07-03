using System;
using System.Collections.Generic;
using NetSteps.Data.Common.Context;
using NetSteps.Data.Common.Entities;
using NetSteps.OrderAdjustments.Common.Model;

namespace NetSteps.OrderAdjustments.Common
{
    /// <summary>
    /// Interface for the Order Adjustment handler, responsible for applying/removing adjustments to orders and retrieving applied adjustments from an order.
    /// </summary>
    public interface IOrderAdjustmentHandler
    {

        /// <summary>
        /// Applies the adjustments to the order.
        /// </summary>
        /// <param name="orderContext">The order.</param>
        /// <param name="adjustments">The adjustments.</param>
        /// <param name="orderValidityFilter">The order validity filter.</param>
        /// <param name="orderAdjustmentValidityFilter">The order adjustment validity filter.</param>
        /// <param name="stripExistingAdjustments">if set to <c>true</c> [strip existing adjustments].</param>
        void ApplyAdjustments(IOrderContext orderContext, IEnumerable<IOrderAdjustmentProfile> adjustments, Predicate<IOrderContext> orderValidityFilter, Func<IOrderContext, IEnumerable<IOrderAdjustmentProfile>, IEnumerable<IOrderAdjustmentProfile>> orderAdjustmentValidityFilter, bool stripExistingAdjustments);

        /// <summary>
        /// Gets the order adjustments.
        /// </summary>
        /// <param name="orderContext">The order context.</param>
        /// <returns></returns>
        IEnumerable<IOrderAdjustmentProfile> GetOrderAdjustments(IOrderContext orderContext);

        /// <summary>
        /// Removes the adjustment.
        /// </summary>
        /// <param name="orderContext">The order context.</param>
        /// <param name="adjustment">The adjustment.</param>
        void RemoveAdjustment(IOrderContext orderContext, IOrderAdjustment adjustment);

        /// <summary>
        /// Removes all adjustments.
        /// </summary>
        /// <param name="orderContext">The order context.</param>
        void RemoveAllAdjustments(IOrderContext orderContext);

        /// <summary>
        /// Commits the adjustments.
        /// </summary>
        /// <param name="orderContext">The order context.</param>
        void CommitAdjustments(IOrderContext orderContext);
    }

    /// <summary>
    /// Extension methods for the IOrderAdjustmentHandler.
    /// </summary>
    public static class IOrderAdjustmentHandlerExtensions
    {
        /// <summary>
        /// Applies the adjustments to the order and strips existing adjustments.
        /// </summary>
        /// <param name="handler">The handler.</param>
        /// <param name="orderContext">The order context.</param>
        /// <param name="adjustments">The adjustments.</param>
        public static void ApplyAdjustments(this IOrderAdjustmentHandler handler, IOrderContext orderContext, IEnumerable<IOrderAdjustmentProfile> adjustments)
        {
            handler.ApplyAdjustments(orderContext, adjustments, (x) => true, (x, y) => { return y; }, true);
        }

        /// <summary>
        /// Applies the adjustments to the order and strips existing adjustments.
        /// </summary>
        /// <param name="handler">The handler.</param>
        /// <param name="orderContext">The order context.</param>
        /// <param name="adjustments">The adjustments.</param>
        /// <param name="orderValidityFilter">The order validity filter.</param>
        public static void ApplyAdjustments(this IOrderAdjustmentHandler handler, IOrderContext orderContext, IEnumerable<IOrderAdjustmentProfile> adjustments, Predicate<IOrderContext> orderValidityFilter)
        {
            handler.ApplyAdjustments(orderContext, adjustments, orderValidityFilter, (x, y) => { return y; }, true);
        }

        /// <summary>
        /// Applies the adjustments to the order and strips existing adjustments.
        /// </summary>
        /// <param name="handler">The handler.</param>
        /// <param name="orderContext">The order context.</param>
        /// <param name="adjustments">The adjustments.</param>
        /// <param name="orderAdjustmentValidityFilter">The order adjustment validity filter.</param>
        public static void ApplyAdjustments(this IOrderAdjustmentHandler handler, IOrderContext orderContext, IEnumerable<IOrderAdjustmentProfile> adjustments, Func<IOrderContext, IEnumerable<IOrderAdjustmentProfile>, IEnumerable<IOrderAdjustmentProfile>> orderAdjustmentValidityFilter)
        {
            handler.ApplyAdjustments(orderContext, adjustments, (x) => true, orderAdjustmentValidityFilter, true);
        }

        /// <summary>
        /// Applies an adjustment to the order and strips existing adjustments.
        /// </summary>
        /// <param name="handler">The handler.</param>
        /// <param name="orderContext">The order context.</param>
        /// <param name="adjustment">The adjustment.</param>
        public static void ApplyAdjustment(this IOrderAdjustmentHandler handler, IOrderContext orderContext, IOrderAdjustmentProfile adjustment)
        {
            handler.ApplyAdjustments(orderContext, adjustment.WrapInList(), (x) => true, (x, y) => { return y; }, true);
        }

        /// <summary>
        /// Applies an adjustment to the order and strips existing adjustments.
        /// </summary>
        /// <param name="handler">The handler.</param>
        /// <param name="orderContext">The order context.</param>
        /// <param name="adjustment">The adjustment.</param>
        /// <param name="orderValidityFilter">The order validity filter.</param>
        public static void ApplyAdjustment(this IOrderAdjustmentHandler handler, IOrderContext orderContext, IOrderAdjustmentProfile adjustment, Predicate<IOrderContext> orderValidityFilter)
        {
            handler.ApplyAdjustments(orderContext, adjustment.WrapInList(), orderValidityFilter, (x, y) => { return y; }, true);
        }

        /// <summary>
        /// Applies an adjustment to the order and strips existing adjustments.
        /// </summary>
        /// <param name="handler">The handler.</param>
        /// <param name="orderContext">The order context.</param>
        /// <param name="adjustment">The adjustment.</param>
        /// <param name="orderAdjustmentValidityFilter">The order adjustment validity filter.</param>
        public static void ApplyAdjustment(this IOrderAdjustmentHandler handler, IOrderContext orderContext, IOrderAdjustmentProfile adjustment, Func<IOrderContext, IEnumerable<IOrderAdjustmentProfile>, IEnumerable<IOrderAdjustmentProfile>> orderAdjustmentValidityFilter)
        {
            handler.ApplyAdjustments(orderContext, adjustment.WrapInList(), (x) => true, orderAdjustmentValidityFilter, true);
        }

        /// <summary>
        /// Applies an adjustment to the order and strips existing adjustments.
        /// </summary>
        /// <param name="handler">The handler.</param>
        /// <param name="orderContext">The order context.</param>
        /// <param name="adjustment">The adjustment.</param>
        /// <param name="orderValidityFilter">The order validity filter.</param>
        /// <param name="orderAdjustmentValidityFilter">The order adjustment validity filter.</param>
        public static void ApplyAdjustment(this IOrderAdjustmentHandler handler, IOrderContext orderContext, IOrderAdjustmentProfile adjustment, Predicate<IOrderContext> orderValidityFilter, Func<IOrderContext, IEnumerable<IOrderAdjustmentProfile>, IEnumerable<IOrderAdjustmentProfile>> orderAdjustmentValidityFilter)
        {
            handler.ApplyAdjustments(orderContext, adjustment.WrapInList(), orderValidityFilter, orderAdjustmentValidityFilter, true);
        }

        /// <summary>
        /// Applies the adjustments to the order.
        /// </summary>
        /// <param name="handler">The handler.</param>
        /// <param name="orderContext">The order context.</param>
        /// <param name="adjustments">The adjustments.</param>
        /// <param name="stripExistingAdjustments">if set to <c>true</c> [strip existing adjustments].</param>
        public static void ApplyAdjustments(this IOrderAdjustmentHandler handler, IOrderContext orderContext, IEnumerable<IOrderAdjustmentProfile> adjustments, bool stripExistingAdjustments)
        {
            handler.ApplyAdjustments(orderContext, adjustments, (x) => true, (x, y) => { return y; }, stripExistingAdjustments);
        }

        /// <summary>
        /// Applies the adjustments to the order.
        /// </summary>
        /// <param name="handler">The handler.</param>
        /// <param name="orderContext">The order context.</param>
        /// <param name="adjustments">The adjustments.</param>
        /// <param name="orderValidityFilter">The order validity filter.</param>
        /// <param name="stripExistingAdjustments">if set to <c>true</c> [strip existing adjustments].</param>
        public static void ApplyAdjustments(this IOrderAdjustmentHandler handler, IOrderContext orderContext, IEnumerable<IOrderAdjustmentProfile> adjustments, Predicate<IOrderContext> orderValidityFilter, bool stripExistingAdjustments)
        {
            handler.ApplyAdjustments(orderContext, adjustments, orderValidityFilter, (x, y) => { return y; }, stripExistingAdjustments);
        }

        /// <summary>
        /// Applies the adjustments to the order.
        /// </summary>
        /// <param name="handler">The handler.</param>
        /// <param name="orderContext">The order context.</param>
        /// <param name="adjustments">The adjustments.</param>
        /// <param name="orderAdjustmentValidityFilter">The order adjustment validity filter.</param>
        /// <param name="stripExistingAdjustments">if set to <c>true</c> [strip existing adjustments].</param>
        public static void ApplyAdjustments(this IOrderAdjustmentHandler handler, IOrderContext orderContext, IEnumerable<IOrderAdjustmentProfile> adjustments, Func<IOrderContext, IEnumerable<IOrderAdjustmentProfile>, IEnumerable<IOrderAdjustmentProfile>> orderAdjustmentValidityFilter, bool stripExistingAdjustments)
        {
            handler.ApplyAdjustments(orderContext, adjustments, (x) => true, orderAdjustmentValidityFilter, stripExistingAdjustments);
        }

        /// <summary>
        /// Applies an adjustment to the order.
        /// </summary>
        /// <param name="handler">The handler.</param>
        /// <param name="orderContext">The order context.</param>
        /// <param name="adjustment">The adjustment.</param>
        /// <param name="stripExistingAdjustments">if set to <c>true</c> [strip existing adjustments].</param>
        public static void ApplyAdjustment(this IOrderAdjustmentHandler handler, IOrderContext orderContext, IOrderAdjustmentProfile adjustment, bool stripExistingAdjustments)
        {
            handler.ApplyAdjustments(orderContext, adjustment.WrapInList(), (x) => true, (x, y) => { return y; }, stripExistingAdjustments);
        }

        /// <summary>
        /// Applies an adjustment to the order.
        /// </summary>
        /// <param name="handler">The handler.</param>
        /// <param name="orderContext">The order context.</param>
        /// <param name="adjustment">The adjustment.</param>
        /// <param name="orderValidityFilter">The order validity filter.</param>
        /// <param name="stripExistingAdjustments">if set to <c>true</c> [strip existing adjustments].</param>
        public static void ApplyAdjustment(this IOrderAdjustmentHandler handler, IOrderContext orderContext, IOrderAdjustmentProfile adjustment, Predicate<IOrderContext> orderValidityFilter, bool stripExistingAdjustments)
        {
            handler.ApplyAdjustments(orderContext, adjustment.WrapInList(), orderValidityFilter, (x, y) => { return y; }, stripExistingAdjustments);
        }

        /// <summary>
        /// Applies an adjustment to the order.
        /// </summary>
        /// <param name="handler">The handler.</param>
        /// <param name="orderContext">The order context.</param>
        /// <param name="adjustment">The adjustment.</param>
        /// <param name="orderAdjustmentValidityFilter">The order adjustment validity filter.</param>
        /// <param name="stripExistingAdjustments">if set to <c>true</c> [strip existing adjustments].</param>
        public static void ApplyAdjustment(this IOrderAdjustmentHandler handler, IOrderContext orderContext, IOrderAdjustmentProfile adjustment, Func<IOrderContext, IEnumerable<IOrderAdjustmentProfile>, IEnumerable<IOrderAdjustmentProfile>> orderAdjustmentValidityFilter, bool stripExistingAdjustments)
        {
            handler.ApplyAdjustments(orderContext, adjustment.WrapInList(), (x) => true, orderAdjustmentValidityFilter, stripExistingAdjustments);
        }

        /// <summary>
        /// Applies an adjustment to the order.
        /// </summary>
        /// <param name="handler">The handler.</param>
        /// <param name="orderContext">The order context.</param>
        /// <param name="adjustment">The adjustment.</param>
        /// <param name="orderValidityFilter">The order validity filter.</param>
        /// <param name="orderAdjustmentValidityFilter">The order adjustment validity filter.</param>
        /// <param name="stripExistingAdjustments">if set to <c>true</c> [strip existing adjustments].</param>
        public static void ApplyAdjustment(this IOrderAdjustmentHandler handler, IOrderContext orderContext, IOrderAdjustmentProfile adjustment, Predicate<IOrderContext> orderValidityFilter, Func<IOrderContext, IEnumerable<IOrderAdjustmentProfile>, IEnumerable<IOrderAdjustmentProfile>> orderAdjustmentValidityFilter, bool stripExistingAdjustments)
        {
            handler.ApplyAdjustments(orderContext, adjustment.WrapInList(), orderValidityFilter, orderAdjustmentValidityFilter, stripExistingAdjustments);
        }
    }
}
