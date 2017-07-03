namespace NetSteps.Data.Entities.Repositories.Interfaces
{
    using System.Collections.Generic;
    using NetSteps.Data.Entities.Dto;

    /// <summary>
    /// Interface for PromoPromotion
    /// </summary>
    public interface IPromoPromotionRepository
    {
        List<PromoPromotionDto> ListPromotions(string Description);

        List<PromoPromotionDto> ListPromotionsByPromotionTypeConfigurationPerPromotions();

        void InsertPromotionRewardEffectApplyOrderItemPropertyValues(int promotionID, int productPriceTypeID);

        void UpdatePromotionRewardEffectApplyOrderItemPropertyValues(int promotionID, int productPriceTypeID);

        void UpdatePromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmounts(int promotionQualificationId, bool cumulative);

        PromoPromotionDto GetByID(int promotionId);

        #region HasACombinationOfItems
        
        void InsOrCondition(int promotionID);

        bool ExistsOrCondition(int promotionID);

        #endregion

        #region HasADefinedQVTotal

        void InsAndConditionQVTotal(int promotionID, decimal QvMin, decimal QvMax);

        Dictionary<bool, Dictionary<decimal, decimal>> ExistsAndConditionQVTotal(int promotionID);

        #endregion
    }
}
