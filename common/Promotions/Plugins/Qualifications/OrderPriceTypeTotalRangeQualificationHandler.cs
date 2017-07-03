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
using NetSteps.Data.Common.Entities;

namespace NetSteps.Promotions.Plugins.Qualifications
{
    public class OrderPriceTypeTotalRangeQualificationHandler : BasePromotionQualificationHandler<IOrderPriceTypeTotalRangeQualificationExtension, IOrderPriceTypeTotalRangeQualificationRepository, IEncorePromotionsPluginsUnitOfWork>, IOrderPriceTypeTotalRangeQualificationHandler
    {
       public OrderPriceTypeTotalRangeQualificationHandler()
        {
        }

        public override string GetProviderKey()
        {
            return QualificationExtensionProviderKeys.OrderPriceTypeTotalRangeProviderKey;
        }

        public override PromotionQualificationResult Matches(IPromotion promotion, IPromotionQualificationExtension promotionQualification, IOrderContext orderContext)
        {
            Contract.Assert(promotionQualification != null);
            Contract.Assert(orderContext != null);
            Contract.Assert(typeof(IOrderPriceTypeTotalRangeQualificationExtension).IsAssignableFrom(promotionQualification.GetType()));

            var qualification = (IOrderPriceTypeTotalRangeQualificationExtension)promotionQualification;
            if (!qualification.OrderPriceTypeTotalRangesByCurrencyID.ContainsKey(orderContext.Order.CurrencyID))
            {
                return PromotionQualificationResult.NoMatch;
            }
            var PriceTypeTotalRange = qualification.OrderPriceTypeTotalRangesByCurrencyID[orderContext.Order.CurrencyID];
			// THIS DOES NOT LOAD CHILD ORDERS.  It is assumed that they are already loaded and present as a child of the entity. Should be refactored
			// with the order refactor.
            var priceTypeTotal = GetPriceTypeTotal(orderContext.Order, qualification.ProductPriceTypeID);
			foreach (var childOrder in orderContext.Order.ChildOrders)
			{
				priceTypeTotal += GetPriceTypeTotal(childOrder, qualification.ProductPriceTypeID);
			}
            return PriceTypeTotalRange.IsInRange(priceTypeTotal) ? PromotionQualificationResult.MatchForAllCustomers : PromotionQualificationResult.NoMatch;
        }

        private decimal GetPriceTypeTotal(IOrder order, int priceType)
        {
            return order.OrderCustomers.Sum(orderCustomer => orderCustomer.OrderItems.Sum(orderItem => orderItem.GetAdjustedPrice(priceType) * orderItem.Quantity));
        }

        public override bool AreEqual(IPromotionQualificationExtension promotionQualification1, IPromotionQualificationExtension promotionQualification2)
        {
            Contract.Assert(promotionQualification1 != null);
            Contract.Assert(promotionQualification2 != null);
            Contract.Assert(promotionQualification1 is IOrderPriceTypeTotalRangeQualificationExtension);
            Contract.Assert(promotionQualification2 is IOrderPriceTypeTotalRangeQualificationExtension);

            var extension1 = promotionQualification1 as IOrderPriceTypeTotalRangeQualificationExtension;
            var extension2 = promotionQualification2 as IOrderPriceTypeTotalRangeQualificationExtension;

            if (extension1.OrderPriceTypeTotalRangesByCurrencyID.Keys.Except(extension2.OrderPriceTypeTotalRangesByCurrencyID.Keys).Any())
                return false;
			if (extension2.OrderPriceTypeTotalRangesByCurrencyID.Keys.Except(extension1.OrderPriceTypeTotalRangesByCurrencyID.Keys).Any())
                return false;

			return extension1.OrderPriceTypeTotalRangesByCurrencyID.Keys.All(key => extension1.OrderPriceTypeTotalRangesByCurrencyID[key].IsEqualTo(extension2.OrderPriceTypeTotalRangesByCurrencyID[key]));
        }

        public override bool ValidFor<TProperty>(IPromotionQualificationExtension qualification, string propertyName, TProperty value)
        {
            Contract.Assert(qualification != null);

            return qualification.ValidFor(propertyName, value);
        }

		public override void CheckValidity(string qualificationKey, IOrderPriceTypeTotalRangeQualificationExtension qualification, IPromotionState state)
		{
			if (qualification.OrderPriceTypeTotalRangesByCurrencyID == null || !qualification.OrderPriceTypeTotalRangesByCurrencyID.Any())
			{
				state.AddConstructionError
					(
						String.Format("Promotion Qualification {0}", qualificationKey),
						"Minimum PriceTypeTotals dictionary is null or empty."
					);
			}
		}
	}
}
