using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Data.Common.Context;
using NetSteps.OrderAdjustments.Common.Model;

namespace NetSteps.OrderAdjustments.Common
{
    public interface IOrderAdjustmentService
    {
		/// <summary>
		/// Gets the order adjustments which are applicable for the supplied order context.  This will check all registered providers and return a set which may be filtered before application to the order.  Note that this method does not make any changes to the order or order context.
		/// </summary>
		/// <param name="targetOrderContext">The target order context.</param>
		/// <param name="orderValidator">The order validator.</param>
		/// <param name="adjustmentValidator">The adjustment validator.</param>
		/// <param name="adjustmentFilter">The adjustment filter.</param>
		/// <param name="filters">The filters.</param>
		/// <returns></returns>
        IList<IOrderAdjustmentProfile> GetApplicableOrderAdjustments(IOrderContext targetOrderContext, Predicate<IOrderContext> orderValidator, Predicate<IOrderAdjustmentProfile> adjustmentValidator, Func<IOrderContext, IQueryable<IOrderAdjustmentProfile>, IQueryable<IOrderAdjustmentProfile>> adjustmentFilter, params IObjectFilter<IOrderAdjustmentProfile>[] filters);
		
		/// <summary>
		/// Intended for order completion.  This will notify all order adjustment providers that adjustments need to be committed.  Should be wrapped in the same transaction.
		/// </summary>
		/// <param name="orderContext">The order context.</param>
		/// <returns></returns>
        IOrderContext CommitOrderAdjustments(IOrderContext orderContext);

		/// <summary>
		/// Combines two order adjustments using the primary profile as a base (i.e. description will remain the description of the primary profile).
		/// </summary>
		/// <param name="primaryProfile">The primary profile.</param>
		/// <param name="secondaryProfile">The secondary profile.</param>
		void CombineAdjustments(IOrderAdjustmentProfile primaryProfile, IOrderAdjustmentProfile secondaryProfile);
    }

    public static class IOrderAdjustmentServiceExtensions
    {

        public static IList<IOrderAdjustmentProfile> GetApplicableOrderAdjustments(this IOrderAdjustmentService service, IOrderContext targetOrderContext, params IObjectFilter<IOrderAdjustmentProfile>[] filters)
        {
            return service.GetApplicableOrderAdjustments(targetOrderContext, (x) => true, (x) => true, (x, y) => { return y; }, filters);
        }

        public static IList<IOrderAdjustmentProfile> GetApplicableOrderAdjustments(this IOrderAdjustmentService service, IOrderContext targetOrderContext, Predicate<IOrderContext> orderValidator, params IObjectFilter<IOrderAdjustmentProfile>[] filters)
        {
            return service.GetApplicableOrderAdjustments(targetOrderContext, orderValidator, (x) => true, (x, y) => { return y; }, filters);
        }

        public static IList<IOrderAdjustmentProfile> GetApplicableOrderAdjustments(this IOrderAdjustmentService service, IOrderContext targetOrderContext, Predicate<IOrderAdjustmentProfile> adjustmentValidator, params IObjectFilter<IOrderAdjustmentProfile>[] filters)
        {
            return service.GetApplicableOrderAdjustments(targetOrderContext, (x) => true, adjustmentValidator, (x, y) => { return y; }, filters);
        }

        public static IList<IOrderAdjustmentProfile> GetApplicableOrderAdjustments(this IOrderAdjustmentService service, IOrderContext targetOrderContext, Predicate<IOrderContext> orderValidator, Predicate<IOrderAdjustmentProfile> adjustmentValidator, params IObjectFilter<IOrderAdjustmentProfile>[] filters)
        {
            return service.GetApplicableOrderAdjustments(targetOrderContext, orderValidator, adjustmentValidator, (x, y) => { return y; }, filters);
        }

        public static IList<IOrderAdjustmentProfile> GetApplicableOrderAdjustments(this IOrderAdjustmentService service, IOrderContext targetOrderContext, Func<IOrderContext, IQueryable<IOrderAdjustmentProfile>, IQueryable<IOrderAdjustmentProfile>> adjustmentFilter, params IObjectFilter<IOrderAdjustmentProfile>[] filters)
        {
            return service.GetApplicableOrderAdjustments(targetOrderContext, (x) => true, (x) => true, adjustmentFilter, filters);
        }

        public static IList<IOrderAdjustmentProfile> GetApplicableOrderAdjustments(this IOrderAdjustmentService service, IOrderContext targetOrderContext, Predicate<IOrderContext> orderValidator, Func<IOrderContext, IQueryable<IOrderAdjustmentProfile>, IQueryable<IOrderAdjustmentProfile>> adjustmentFilter, params IObjectFilter<IOrderAdjustmentProfile>[] filters)
        {
            return service.GetApplicableOrderAdjustments(targetOrderContext, orderValidator, (x) => true, adjustmentFilter, filters);
        }

        public static IList<IOrderAdjustmentProfile> GetApplicableOrderAdjustments(this IOrderAdjustmentService service, IOrderContext targetOrderContext, Predicate<IOrderAdjustmentProfile> adjustmentValidator, Func<IOrderContext, IQueryable<IOrderAdjustmentProfile>, IQueryable<IOrderAdjustmentProfile>> adjustmentFilter, params IObjectFilter<IOrderAdjustmentProfile>[] filters)
        {
            return service.GetApplicableOrderAdjustments(targetOrderContext, (x) => true, adjustmentValidator, adjustmentFilter, filters);
        }
    }
}
