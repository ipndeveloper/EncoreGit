// -----------------------------------------------------------------------
// <copyright file="IPromotionOrderService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace NetSteps.Promotions.UI.Common.Interfaces
{

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IPromotionOrderUIService
    {
        void ApplyPromotionCode(string promotionCode);
        void RemovePromotionCode();
        bool IsValidPromotionCode(string promotionCode);

        void UpdateOrderPromotions();

    }
}
