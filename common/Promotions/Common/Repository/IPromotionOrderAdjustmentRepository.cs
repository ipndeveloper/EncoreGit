using NetSteps.Data.Common.Entities;
using NetSteps.Promotions.Common.Model;

namespace NetSteps.Promotions.Common.Repository
{
    public interface IPromotionOrderAdjustmentRepository
    {
        IPromotionOrderAdjustment SaveAdjustmentExtension(IOrderAdjustment adjustment, IPromotionUnitOfWork unitOfWork);
        IPromotionOrderAdjustment FindPromotionOrderAdjustment(int OrderAdjustmentID, IPromotionUnitOfWork unitOfWork);
        void DeletePromotionOrderAdjustment(int OrderAdjustmentID, IPromotionUnitOfWork unitOfWork);
    }
}
