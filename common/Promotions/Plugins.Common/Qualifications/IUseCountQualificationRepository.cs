using NetSteps.Data.Common.Context;
using NetSteps.Promotions.Common.Model;
using NetSteps.Promotions.Plugins.Base;

namespace NetSteps.Promotions.Plugins.Common.Qualifications
{
    public interface IUseCountQualificationRepository : IPromotionQualificationRepository<IUseCountQualificationExtension>
    {
        void RecordUse(IPromotionQualificationExtension qualification, IOrderContext orderContext, IEncorePromotionsPluginsUnitOfWork unitOfWork);

        int GetUseCount(IUseCountQualificationExtension promotionQualification, int accountID, IEncorePromotionsPluginsUnitOfWork unitOfWork);
    }
}
