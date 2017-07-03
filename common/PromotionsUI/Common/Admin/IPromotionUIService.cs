// -----------------------------------------------------------------------
// <copyright file="IPromotionsUIService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace NetSteps.Promotions.UI.Common.Admin
{
    using System.Collections.Generic;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IPromotionUIService
    {
        IEnumerable<IPromotionUIModel> All();
        IPromotionUIModel Get(int promotionId);
        void Delete(int promotionId);
        void Save(IPromotionUIModel model);
    }
}
