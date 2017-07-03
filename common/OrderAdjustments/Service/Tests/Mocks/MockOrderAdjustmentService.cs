using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Data.Common.Context;
using NetSteps.OrderAdjustments.Common;
using NetSteps.OrderAdjustments.Common.Exceptions;
using NetSteps.OrderAdjustments.Common.Model;

namespace NetSteps.OrderAdjustments.Service.Test.Mocks
{
    public class MockOrderAdjustmentService : IOrderAdjustmentService
    {
        protected internal IOrderAdjustmentProviderManager _providerManager { get; set; }

        public MockOrderAdjustmentService(IOrderAdjustmentProviderManager providerManager)
        {
            _providerManager = providerManager;
        }

        public List<IOrderAdjustmentProfile> GetApplicableOrderAdjustments(IOrderContext targetOrderContext,
            Predicate<IOrderContext> orderValidator,
            Predicate<IOrderAdjustmentProfile> adjustmentValidator,
            Func<IOrderContext, IQueryable<IOrderAdjustmentProfile>, IQueryable<IOrderAdjustmentProfile>> adjustmentFilter,

            params IObjectFilter<IOrderAdjustmentProfile>[] filters)
        {
            if (!orderValidator(targetOrderContext))
                throw new OrderAdjustmentServiceException(OrderAdjustmentServiceException.ExceptionKind.CANNOT_RETRIEVE_ADJUSTMENTS_FOR_ORDER_FAILED_VALIDATION, "Order currently to pass the order validator when attempting to retrieve valid order adjustments.");

            IEnumerable<IOrderAdjustmentProvider> currentProviders = _providerManager.GetAllProviders();
            List<IOrderAdjustmentProfile> profileList = new List<IOrderAdjustmentProfile>();
            foreach (IOrderAdjustmentProvider provider in currentProviders)
            {
                IQueryable<IOrderAdjustmentProfile> adjustments = provider.GetApplicableAdjustments(targetOrderContext).AsQueryable();
                adjustments = adjustments.Where((x) => adjustmentValidator(x));
                adjustments = adjustmentFilter(targetOrderContext, adjustments);
                foreach (IObjectFilter<IOrderAdjustmentProfile> filter in filters)
                    adjustments = filter.BuildQueryFrom(adjustments);
                profileList.AddRange(adjustments);
            }
            return profileList;
        }


        IList<IOrderAdjustmentProfile> IOrderAdjustmentService.GetApplicableOrderAdjustments(IOrderContext targetOrderContext, Predicate<IOrderContext> orderValidator, Predicate<IOrderAdjustmentProfile> adjustmentValidator, Func<IOrderContext, IQueryable<IOrderAdjustmentProfile>, IQueryable<IOrderAdjustmentProfile>> adjustmentFilter, params IObjectFilter<IOrderAdjustmentProfile>[] filters)
        {
            throw new NotImplementedException();
        }

        public IOrderContext CommitOrderAdjustments(IOrderContext orderContext)
        {
            throw new NotImplementedException();
        }

		public void CombineAdjustments(IOrderAdjustmentProfile primaryProfile, IOrderAdjustmentProfile secondaryProfile)
		{
			throw new NotImplementedException();
		}
	}
}
