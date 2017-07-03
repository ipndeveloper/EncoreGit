using System;
using System.Collections.Generic;
using NetSteps.Data.Common.Entities;
using NetSteps.OrderAdjustments.Common.Model;
using NetSteps.Promotions.Common.Model;
using NetSteps.Data.Common.Context;
using System.Diagnostics.Contracts;

namespace NetSteps.Promotions.Common
{
	[ContractClass(typeof(PromotionServiceContract))]
    public interface IPromotionService
    {
        IPromotion AddPromotion(IPromotion promotion, out IPromotionState promotionState);
		IPromotion UpdatePromotion(IPromotion promotion, out IPromotionState promotionState);
        bool IsInstanceOfPromotion(IOrderAdjustment adjustment, IPromotion promotion);
        IPromotion GetPromotion(int promotionID);
        TPromotion GetPromotion<TPromotion>(int promotionID) where TPromotion : IPromotion;
        IPromotion GetPromotion(Predicate<IPromotion> filter);
        TPromotion GetPromotion<TPromotion>(Predicate<TPromotion> filter) where TPromotion : IPromotion;
        bool IsInstanceOfPromotion(IOrderAdjustmentProfile profile, IPromotion addedPromotion);
        IEnumerable<IPromotion> GetPromotions(PromotionStatus promotionStatuses, Predicate<IPromotion> filter);
        IEnumerable<TPromotion> GetPromotions<TPromotion>(PromotionStatus promotionStatuses, Predicate<TPromotion> filter) where TPromotion : IPromotion;
		IEnumerable<TPromotion> GetQualifiedPromotions<TPromotion>(IOrderContext orderContext, Predicate<TPromotion> filter) where TPromotion : IPromotion;
	}

    public static class PromotionServiceExtensions
    {
        public static IEnumerable<IPromotion> GetPromotions(this IPromotionService provider)
        {
            return provider.GetPromotions(PromotionStatus.Enabled, (x) => { return true; });
        }

        public static IEnumerable<IPromotion> GetPromotions(this IPromotionService provider, PromotionStatus promotionStatuses)
        {
            return provider.GetPromotions(promotionStatuses, (x) => { return true; });
        }

        public static IEnumerable<IPromotion> GetPromotions(this IPromotionService provider, Predicate<IPromotion> filter)
        {
            return provider.GetPromotions(PromotionStatus.Enabled, filter);
        }

        public static IEnumerable<TPromotion> GetPromotions<TPromotion>(this IPromotionService provider) where TPromotion : IPromotion
        {
            return provider.GetPromotions<TPromotion>(PromotionStatus.Enabled, (x) => { return true; });
        }

        public static IEnumerable<TPromotion> GetPromotions<TPromotion>(this IPromotionService provider, Predicate<TPromotion> filter) where TPromotion : IPromotion
        {
            return provider.GetPromotions<TPromotion>(PromotionStatus.Enabled, filter);
        }
    }

	[ContractClassFor(typeof(IPromotionService))]
	public abstract class PromotionServiceContract : IPromotionService
	{

		public IPromotion AddPromotion(IPromotion promotion, out IPromotionState state)
		{
			Contract.Requires<ArgumentNullException>(promotion != null);
			Contract.Ensures(Contract.ValueAtReturn<IPromotionState>(out state) != null);
			throw new NotImplementedException();
		}

		public IPromotion UpdatePromotion(IPromotion promotion, out IPromotionState state)
		{
			Contract.Requires<ArgumentNullException>(promotion != null);
			Contract.Ensures(Contract.ValueAtReturn<IPromotionState>(out state) != null);
			throw new NotImplementedException();
		}

		public bool IsInstanceOfPromotion(IOrderAdjustment adjustment, IPromotion promotion)
		{
			Contract.Requires<ArgumentNullException>(adjustment != null);
			Contract.Requires<ArgumentNullException>(promotion != null);
			throw new NotImplementedException();
		}

		public IPromotion GetPromotion(int promotionID)
		{
			Contract.Requires<ArgumentOutOfRangeException>(promotionID > 0);
			throw new NotImplementedException();
		}

		public TPromotion GetPromotion<TPromotion>(int promotionID) where TPromotion : IPromotion
		{
			Contract.Requires<ArgumentOutOfRangeException>(promotionID > 0);
			throw new NotImplementedException();
		}

		public IPromotion GetPromotion(Predicate<IPromotion> filter)
		{
			Contract.Requires<ArgumentNullException>(filter != null);
			throw new NotImplementedException();
		}

		public TPromotion GetPromotion<TPromotion>(Predicate<TPromotion> filter) where TPromotion : IPromotion
		{
			Contract.Requires<ArgumentNullException>(filter != null);
			throw new NotImplementedException();
		}

		public bool IsInstanceOfPromotion(IOrderAdjustmentProfile profile, IPromotion addedPromotion)
		{
			Contract.Requires<ArgumentNullException>(profile != null);
			Contract.Requires<ArgumentNullException>(addedPromotion != null);
			throw new NotImplementedException();
		}

		public IEnumerable<IPromotion> GetPromotions(PromotionStatus promotionStatuses, Predicate<IPromotion> filter)
		{
			Contract.Requires<ArgumentNullException>(filter != null);
			throw new NotImplementedException();
		}

		public IEnumerable<TPromotion> GetPromotions<TPromotion>(PromotionStatus promotionStatuses, Predicate<TPromotion> filter) where TPromotion : IPromotion
		{
			Contract.Requires<ArgumentNullException>(filter != null);
			throw new NotImplementedException();
		}

		public IEnumerable<TPromotion> GetQualifiedPromotions<TPromotion>(IOrderContext orderContext, Predicate<TPromotion> filter) where TPromotion : IPromotion
		{
			Contract.Requires<ArgumentNullException>(orderContext != null);
			Contract.Requires<ArgumentNullException>(filter != null);
			throw new NotImplementedException();
		}


		
	}
}
