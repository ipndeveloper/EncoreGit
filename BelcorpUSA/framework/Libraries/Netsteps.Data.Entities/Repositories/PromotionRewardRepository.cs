namespace NetSteps.Data.Entities.Repositories
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using NetSteps.Data.Entities.Dto;
    using NetSteps.Data.Entities.Repositories.Interfaces;
    using NetSteps.Data.Entities.Utility;

    /// <summary>
    /// Implementacion de la interface Inteface
    /// </summary>
    public partial class PromotionRewardRepository : IPromotionRewardRepository
    {
        public PromotionRewardDto GetByPromotionID(int promotionId)
        {
            using (var context = new EntityDBContext(ConnectionStrings.BelcorpCore))
            {
                var data = (from r in context.PromoPromotionRewards
                            where r.PromotionID == promotionId
                            select r).FirstOrDefault();

                return TableToDto(data);
            }
        }

        private PromotionRewardDto TableToDto(NetSteps.Data.Entities.EntityModels.PromoPromotionRewardTable table)
        {
            if (table == null)
                return null;

            return new PromotionRewardDto()
            {
                PromotionID = table.PromotionID,
                PromotionRewardID = table.PromotionRewardID,
                PromotionPropertyKey = table.PromotionPropertyKey,
                PromotionRewardKind = table.PromotionRewardKind
            };
        }
    }
}