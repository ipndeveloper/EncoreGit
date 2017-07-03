namespace NetSteps.Data.Entities.Repositories.Interfaces
{
    using NetSteps.Data.Entities.Dto;

    /// <summary>
    /// Interface for PromotionTypeConfigurationsPerOrder
    /// </summary>
    public interface IPromoPromotionTypeConfigurationsPerOrderRepository
    {
        bool Insert(PromoPromotionTypeConfigurationsPerOrderDto dto);
        bool GetBA();

        bool GetBA(int promotionTypeConfigurationId);
    }
}
