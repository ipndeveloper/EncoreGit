using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics.Contracts;
using NetSteps.Data.Common.Context;
using NetSteps.Data.Common.Entities;
using NetSteps.Promotions.Plugins.Base;
using NetSteps.Promotions.Common.Model;
using NetSteps.Promotions.Plugins.Common.Qualifications;
using NetSteps.Promotions.Plugins.Common;
using NetSteps.Promotions.Common.ModelConcrete;

namespace NetSteps.Promotions.Plugins.Qualifications
{
    public class ProductInOrderQualificationHandler : BasePromotionQualificationHandler<IProductInOrderQualificationExtension, IProductInOrderQualificationRepository, IEncorePromotionsPluginsUnitOfWork>, IProductInOrderQualificationHandler
    {
        public ProductInOrderQualificationHandler()
        {
        }

        public override string GetProviderKey()
        {
            return QualificationExtensionProviderKeys.ProductInOrderProviderKey;
        }

        public override PromotionQualificationResult Matches(IPromotion promotion, IPromotionQualificationExtension promotionQualification, IOrderContext orderContext)
        {
            Contract.Assert(promotionQualification != null);
            Contract.Assert(orderContext != null);
            Contract.Assert(promotionQualification is IProductInOrderQualificationExtension);
            IProductInOrderQualificationExtension qualification = (IProductInOrderQualificationExtension)promotionQualification;

            PromotionTypeConfigurationPerPromotionRepository item = new PromotionTypeConfigurationPerPromotionRepository();

            #region OrCondition

            bool ExistsOrCondition = item.ExistsOrCondition(promotion.PromotionID);

            #endregion

            #region Item Condition (QvRanges - Items Combo)

            var first = item.ExistsAndConditionQVTotal(promotion.PromotionID).First();
            bool AndConditionQvTotal = first.Key;
            decimal QvMin = first.Value.First().Key;
            decimal QvMax = first.Value.First().Value;
            int ProductPriceTypeIDqv = 21;

            #endregion

            if (AndConditionQvTotal)
            {
                #region Validating QvRanges

                bool qvRangesResult = false;
                decimal qvRangeOrder = GetPriceTypeTotal(orderContext.Order.OrderCustomers[0], ProductPriceTypeIDqv, promotion.PromotionID);

                qvRangesResult = qvRangeOrder >= QvMin && qvRangeOrder <= QvMax ? true : false;

                #endregion

                if (!ExistsOrCondition)
                {
                    var customerIds = orderContext.Order.OrderCustomers.Where(x => x.AdjustableOrderItems.Where(y => y.ProductID == qualification.ProductID && y.ParentOrderItem == null).Sum(y => y.Quantity) >= qualification.Quantity).Select(x => x.AccountID);
                    if (customerIds.Count() > 0 && qvRangesResult)
                        return PromotionQualificationResult.MatchForSelectCustomers(customerIds);
                    return PromotionQualificationResult.NoMatch;
                }
                else
                {
                    Dictionary<int, int> PromotionQualifications = item.GetPromotionQualifications(promotion.PromotionID);
                    foreach (var pq in PromotionQualifications)
                    {
                        var customerIds = orderContext.Order.OrderCustomers.Where(x => x.AdjustableOrderItems.Where(y => y.ProductID == pq.Key && y.ParentOrderItem == null).Sum(y => y.Quantity) >= pq.Value).Select(x => x.AccountID);
                        if (customerIds.Count() > 0 && qvRangesResult) return PromotionQualificationResult.MatchForSelectCustomers(customerIds);
                    }
                    return PromotionQualificationResult.NoMatch;
                }
            }
            else
            {
                if (!ExistsOrCondition)
                {
                    var customerIds = orderContext.Order.OrderCustomers.Where(x => x.AdjustableOrderItems.Where(y => y.ProductID == qualification.ProductID && y.ParentOrderItem == null).Sum(y => y.Quantity) >= qualification.Quantity).Select(x => x.AccountID);
                    if (customerIds.Count() > 0)
                        return PromotionQualificationResult.MatchForSelectCustomers(customerIds);
                    return PromotionQualificationResult.NoMatch;
                }
                else
                {
                    Dictionary<int, int> PromotionQualifications = item.GetPromotionQualifications(promotion.PromotionID);
                    foreach (var pq in PromotionQualifications)
                    {
                        var customerIds = orderContext.Order.OrderCustomers.Where(x => x.AdjustableOrderItems.Where(y => y.ProductID == pq.Key && y.ParentOrderItem == null).Sum(y => y.Quantity) >= pq.Value).Select(x => x.AccountID);
                        if (customerIds.Count() > 0) return PromotionQualificationResult.MatchForSelectCustomers(customerIds);
                    }
                    return PromotionQualificationResult.NoMatch;
                }
            }
        }

        private decimal GetPriceTypeTotal(IOrderCustomer orderCustomer, int priceType, int PromotionID)
        {
            PromotionTypeConfigurationPerPromotionRepository promo = new PromotionTypeConfigurationPerPromotionRepository();
            Dictionary<int, int> PromotionQualifications = promo.GetPromotionQualifications(PromotionID);
            decimal sum = 0;
            foreach (var orderItem in orderCustomer.OrderItems)
            {
                if (PromotionQualifications.ContainsKey(Convert.ToInt32(orderItem.ProductID))) sum += orderItem.GetAdjustedPrice(priceType) * orderItem.Quantity;
            }
            return sum;
        }

        public override bool AreEqual(IPromotionQualificationExtension promotionQualification1, IPromotionQualificationExtension promotionQualification2)
        {
            Contract.Assert(promotionQualification1 != null);
            Contract.Assert(promotionQualification2 != null);
            Contract.Assert(promotionQualification1 is IProductInOrderQualificationExtension);
            Contract.Assert(promotionQualification2 is IProductInOrderQualificationExtension);

            var extension1 = promotionQualification1 as IProductInOrderQualificationExtension;
            var extension2 = promotionQualification2 as IProductInOrderQualificationExtension;

            return extension1.ProductID == extension2.ProductID && extension1.Quantity == extension2.Quantity;
        }

        public override bool ValidFor<TProperty>(IPromotionQualificationExtension qualification, string propertyName, TProperty value)
        {
            Contract.Assert(qualification != null);

            return qualification.ValidFor(propertyName, value);
        }

        public override void CheckValidity(string qualificationKey, IProductInOrderQualificationExtension qualification, IPromotionState state)
        {
            if (qualification.ProductID < 0)
            {
                state.AddConstructionError
                    (
                        String.Format("Promotion Qualification {0}", qualificationKey),
                        "Product ID cannot be less than 0."
                    );
            }

            if (qualification.Quantity <= 0)
            {
                state.AddConstructionError
                    (
                        String.Format("Promotion Qualification {0}", qualificationKey),
                        "Product quantity cannot be less than 1."
                    );
            }
        }
    }
}
