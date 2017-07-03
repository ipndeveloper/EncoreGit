using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Data.Common.Context;
using NetSteps.OrderAdjustments.Common;
using NetSteps.OrderAdjustments.Common.Exceptions;
using NetSteps.OrderAdjustments.Common.Model;
using NetSteps.Data.Common.Entities;
using System.Diagnostics.Contracts;
using NetSteps.Encore.Core.IoC;
using NetSteps.Extensibility.Core;

namespace NetSteps.OrderAdjustments.Service
{
    public class OrderAdjustmentService : IOrderAdjustmentService
    {
        protected internal IOrderAdjustmentProviderManager _providerManager { get; set; }

        public OrderAdjustmentService(IOrderAdjustmentProviderManager providerManager)
        {
            Contract.Assert(providerManager != null);
            
            _providerManager = providerManager;
        }

        public IList<IOrderAdjustmentProfile> GetApplicableOrderAdjustments(IOrderContext targetOrderContext, Predicate<IOrderContext> orderValidator, Predicate<IOrderAdjustmentProfile> adjustmentValidator, Func<IOrderContext, IQueryable<IOrderAdjustmentProfile>, IQueryable<IOrderAdjustmentProfile>> orderAdjustmentFilter, params IObjectFilter<IOrderAdjustmentProfile>[] filters)
        {
            Contract.Assert(targetOrderContext != null);
            Contract.Assert(orderValidator != null);
            Contract.Assert(adjustmentValidator != null);
            Contract.Assert(orderAdjustmentFilter != null);
            Contract.Assert(filters != null);
          
            if (!orderValidator(targetOrderContext))
                throw new OrderAdjustmentServiceException(OrderAdjustmentServiceException.ExceptionKind.CANNOT_RETRIEVE_ADJUSTMENTS_FOR_ORDER_FAILED_VALIDATION, "Order currently to pass the order validator when attempting to retrieve valid order adjustments.");

            IEnumerable<IOrderAdjustmentProvider> currentProviders = _providerManager.GetAllProviders();
            List<IOrderAdjustmentProfile> profileList = new List<IOrderAdjustmentProfile>();
            foreach (IOrderAdjustmentProvider provider in currentProviders)
            {
                IQueryable<IOrderAdjustmentProfile> adjustments = provider.GetApplicableAdjustments(targetOrderContext).AsQueryable();
				adjustments = adjustments.Where(adjustment => adjustment.OrderLineModificationTargets.Any() || adjustment.OrderModifications.Any() || adjustment.AddedOrderSteps.Any());
                adjustments = adjustments.Where((x) => adjustmentValidator(x));
                adjustments = orderAdjustmentFilter(targetOrderContext, adjustments);
                foreach (IObjectFilter<IOrderAdjustmentProfile> filter in filters)
                    adjustments = filter.BuildQueryFrom(adjustments);
                profileList.AddRange(adjustments);
            }
            return profileList;
        }

        public IOrderContext CommitOrderAdjustments(IOrderContext orderContext)
        {
            Contract.Assert(orderContext != null);

            var registry = Create.New<IDataObjectExtensionProviderRegistry>();
            List<IOrderAdjustmentProfile> profileList = new List<IOrderAdjustmentProfile>();
            foreach (IOrderAdjustment orderAdjustment in orderContext.Order.OrderAdjustments)
            {
                var provider = registry.RetrieveExtensionProvider(orderAdjustment.ExtensionProviderKey) as IOrderAdjustmentProvider;
                provider.CommitAdjustment(orderAdjustment, orderContext);
            }
            return orderContext;
        }

		public void CombineAdjustments(IOrderAdjustmentProfile primaryProfile, IOrderAdjustmentProfile secondaryProfile)
		{
			// affected account IDs should match!  Otherwise you'll end up crossing over adjustments that should not be combined.
			foreach (var affectedAccount in secondaryProfile.AffectedAccountIDs)
			{
				if (!primaryProfile.AffectedAccountIDs.Contains(affectedAccount))
				{
					primaryProfile.AffectedAccountIDs.Add(affectedAccount);
				}
			}
			foreach (var addedOrderStep in secondaryProfile.AddedOrderSteps)
			{
				primaryProfile.AddedOrderSteps.Add(addedOrderStep);
			}
			foreach (var target in secondaryProfile.OrderLineModificationTargets)
			{
				primaryProfile.OrderLineModificationTargets.Add(target);
			}
			foreach (var orderModification in secondaryProfile.OrderModifications)
			{
				primaryProfile.OrderModifications.Add(orderModification);
			}
		}
	}
}
