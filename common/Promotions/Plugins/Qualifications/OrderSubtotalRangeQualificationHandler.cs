using System;
using System.Linq;
using System.Diagnostics.Contracts;
using NetSteps.Data.Common.Context;
using NetSteps.Promotions.Common.Model;
using NetSteps.Promotions.Plugins.Base;
using NetSteps.Promotions.Plugins.Common.Qualifications;
using NetSteps.Promotions.Plugins.Common;
using NetSteps.Promotions.Common.ModelConcrete;
using NetSteps.Promotions.Plugins.Common.Qualifications.Concrete;

namespace NetSteps.Promotions.Plugins.Qualifications
{
    public class OrderSubtotalRangeQualificationHandler : BasePromotionQualificationHandler<IOrderSubtotalRangeQualificationExtension, IOrderSubtotalRangeQualificationRepository, IEncorePromotionsPluginsUnitOfWork>, IOrderSubtotalRangeQualificationHandler
    {
       public OrderSubtotalRangeQualificationHandler()
        {
        }

        public override string GetProviderKey()
        {
            return QualificationExtensionProviderKeys.OrderSubtotalRangeProviderKey;
        }

        public override PromotionQualificationResult Matches(IPromotion promotion, IPromotionQualificationExtension promotionQualification, IOrderContext orderContext)
        {
            Contract.Assert(promotionQualification != null);
            Contract.Assert(orderContext != null);
            Contract.Assert(typeof(IOrderSubtotalRangeQualificationExtension).IsAssignableFrom(promotionQualification.GetType()));

            var qualification = (IOrderSubtotalRangeQualificationExtension)promotionQualification;
            if (!qualification.OrderSubtotalRangesByCurrencyID.ContainsKey(orderContext.Order.CurrencyID))
            {
                return PromotionQualificationResult.NoMatch;
            }
            var subtotalRange = qualification.OrderSubtotalRangesByCurrencyID[orderContext.Order.CurrencyID];
			// THIS DOES NOT LOAD CHILD ORDERS.  It is assumed that they are already loaded and present as a child of the entity. Should be refactored
			// with the order refactor.
			var subtotal = orderContext.Order.Subtotal.Value;
			foreach (var childOrder in orderContext.Order.ChildOrders)
			{
				subtotal += childOrder.Subtotal.Value;
			}
            return subtotalRange.IsInRange(subtotal) ? PromotionQualificationResult.MatchForAllCustomers : PromotionQualificationResult.NoMatch;
        }

        public override bool AreEqual(IPromotionQualificationExtension promotionQualification1, IPromotionQualificationExtension promotionQualification2)
        {
            Contract.Assert(promotionQualification1 != null);
            Contract.Assert(promotionQualification2 != null);
            Contract.Assert(promotionQualification1 is IOrderSubtotalRangeQualificationExtension);
            Contract.Assert(promotionQualification2 is IOrderSubtotalRangeQualificationExtension);

            var extension1 = promotionQualification1 as IOrderSubtotalRangeQualificationExtension;
            var extension2 = promotionQualification2 as IOrderSubtotalRangeQualificationExtension;

            if (extension1.OrderSubtotalRangesByCurrencyID.Keys.Except(extension2.OrderSubtotalRangesByCurrencyID.Keys).Any())
                return false;
			if (extension2.OrderSubtotalRangesByCurrencyID.Keys.Except(extension1.OrderSubtotalRangesByCurrencyID.Keys).Any())
                return false;

			return extension1.OrderSubtotalRangesByCurrencyID.Keys.All(key => extension1.OrderSubtotalRangesByCurrencyID[key].IsEqualTo(extension2.OrderSubtotalRangesByCurrencyID[key]));
        }

        public override bool ValidFor<TProperty>(IPromotionQualificationExtension qualification, string propertyName, TProperty value)
        {
            Contract.Assert(qualification != null);

            return qualification.ValidFor(propertyName, value);
        }

		public override void CheckValidity(string qualificationKey, IOrderSubtotalRangeQualificationExtension qualification, IPromotionState state)
		{
			if (qualification.OrderSubtotalRangesByCurrencyID == null || !qualification.OrderSubtotalRangesByCurrencyID.Any())
			{
				state.AddConstructionError
					(
						String.Format("Promotion Qualification {0}", qualificationKey),
						"Order subtotal range dictionary is null or empty."
					);
			}
		}
	}
}
