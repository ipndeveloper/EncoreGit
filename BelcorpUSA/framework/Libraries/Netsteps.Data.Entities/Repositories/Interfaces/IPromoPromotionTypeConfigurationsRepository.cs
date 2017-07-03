namespace NetSteps.Data.Entities.Repositories.Interfaces
{
    using NetSteps.Data.Entities.Dto;
    using System.Collections.Generic;

    /// <summary>
    /// Interface for PromoPromotionTypeConfigurations
    /// </summary>
    public interface IPromoPromotionTypeConfigurationsRepository
    {
        bool InactivateAll();
        bool Insert(PromoPromotionTypeConfigurationsDto oPromotionConfiguration, out int outLastGeneratedID);
        List<PromoPromotionTypeConfigurationsDto> ListAll();
        int GetLastConfiguration();
        int GetPromocionTypeDescuentoAcumulativo();
        int GetActive();
    }
}
