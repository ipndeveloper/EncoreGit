namespace NetSteps.Data.Entities.Repositories.Interfaces
{
    using System;
    using System.Collections.Generic;
    using NetSteps.Data.Entities.Dto;

    /// <summary>
    /// Descripcion de la interface
    /// </summary>
    public interface IPromotionConfigurationControlRepository
    {
        void UpdateAmount(int accountId, int periodId, decimal amount, int promotionId);
        void Insert(PromoPromotionConfigurationControlDto dto);

        PromoPromotionConfigurationControlDto GetByAccount(int accountId);

        void UpdatePromotion(int promotionTypeConfigurationId, int newPromotionID);

        PromoPromotionConfigurationControlDto GetByAccount(int accountId, int periodId);

        PromoPromotionConfigurationControlDto GetByPromotionID(int promotionID);
    }
}
