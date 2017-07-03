namespace NetSteps.Data.Entities.Repositories
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using NetSteps.Data.Entities.Dto;
    using NetSteps.Data.Entities.Repositories.Interfaces;
    using NetSteps.Data.Entities.Utility;
    using System.Collections.Generic;
    using NetSteps.Data.Entities.EntityModels;

    /// <summary>
    /// Implementacion de la interface Inteface
    /// </summary>
    public partial class PromotionRewardEffectRepository : IPromotionRewardEffectRepository
    {
        public IEnumerable<PromotionRewardEffectDto> GetAssociated(int promotionRewardId)
        {
            using (var context = new EntityDBContext(ConnectionStrings.BelcorpCore))
            {
                var data = from r in context.PromoPromotionRewardEffects
                           join d in context.PromoPromotionRewardEffectReduceOrderItemPropertyValuesMarketValues on r.PromotionRewardID equals d.PromotionRewardEffectID
                           where r.PromotionRewardID == promotionRewardId
                           && r.ExtensionProviderKey == "Retail"
                           select new PromotionRewardEffectDto()
                            {
                                PromotionRewardEffectID = r.PromotionRewardEffectID,
                                ExtensionProviderKey = r.ExtensionProviderKey,
                                PromotionRewardID = r.PromotionRewardID,
                                RewardPropertyKey = r.RewardPropertyKey,
                                DecimalValue = d.DecimalValue
                            };

                return data;
            }
        }

    }
}