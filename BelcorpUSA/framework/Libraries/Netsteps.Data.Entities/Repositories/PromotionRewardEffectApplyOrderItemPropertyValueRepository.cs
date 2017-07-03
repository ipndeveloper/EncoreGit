namespace NetSteps.Data.Entities.Repositories
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using NetSteps.Data.Entities.Dto;
    using NetSteps.Data.Entities.Repositories.Interfaces;
    using NetSteps.Data.Entities.Utility;
    using NetSteps.Data.Entities.EntityModels;

    /// <summary>
    /// Implementacion de la interface Inteface
    /// </summary>
    public partial class PromotionRewardEffectApplyOrderItemPropertyValueRepository : IPromotionRewardEffectApplyOrderItemPropertyValueRepository
    {
        public PromotionRewardEffectApplyOrderItemPropertyValueDto GetByPromotion(int promotionId)
        {
            using (var context = new EntityDBContext(ConnectionStrings.BelcorpCore))
            {
                var result = from p in context.PromoPromotions
                             join pr in context.PromoPromotionRewards on p.PromotionID equals pr.PromotionID
                             join pre in context.PromoPromotionRewardEffects on pr.PromotionRewardID equals pre.PromotionRewardID
                             join preaoipv in context.PromoPromotionRewardEffectApplyOrderItemPropertyValues on pre.PromotionRewardEffectID equals preaoipv.PromotionRewardEffectID into promotionreward
                             from prtemp in promotionreward.DefaultIfEmpty()
                             join percent in context.PromoPromotionRewardEffectReduceOrderItemPropertyValuesMarketValues on prtemp.PromotionRewardEffectID equals percent.PromotionRewardEffectID
                             where p.PromotionID == promotionId
                             && percent.MarketID == 56 // .DecimalValue > 0 //only test
                             select new PromotionRewardEffectApplyOrderItemPropertyValueDto()
                             {
                                 PromotionRewardEffectID = prtemp.PromotionRewardEffectID,
                                 ProductPriceTypeID = prtemp.ProductPriceTypeID,
                                 PromotionRewardID = pr.PromotionRewardID,
                                 DecimalValue = percent.DecimalValue
                             };

                return result.FirstOrDefault();
            }
        }

        PromotionRewardEffectApplyOrderItemPropertyValueDto TableToDto(PromoPromotionRewardEffectApplyOrderItemPropertyValueTable table)
        {
            if (table == null)
                return null;

            return new PromotionRewardEffectApplyOrderItemPropertyValueDto()
            {
                PromotionRewardEffectID = table.PromotionRewardEffectID,
                ProductPriceTypeID = table.ProductPriceTypeID
            };
        }
    }
}