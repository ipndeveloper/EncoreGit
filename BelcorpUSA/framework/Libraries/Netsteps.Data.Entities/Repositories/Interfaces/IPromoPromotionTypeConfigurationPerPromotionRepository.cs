namespace NetSteps.Data.Entities.Repositories.Interfaces
{
    using NetSteps.Data.Entities.Dto;

    public interface IPromoPromotionTypeConfigurationPerPromotionRepository
    {
        bool Insert(PromoPromotionTypeConfigurationPerPromotionDto dto);
        bool Delete(PromoPromotionTypeConfigurationPerPromotionDto dto);

        bool IsAssociated(int promotionId, int promotionTypeConfigurationId);
    }
}
