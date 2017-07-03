using NetSteps.Data.Common;
using NetSteps.Promotions.Common.Model;

namespace NetSteps.Promotions.Plugins.Base
{
    public interface IPromotionQualificationRepository<T>
    {
        T RetrieveExtension(IPromotionQualification qualification, IUnitOfWork unitOfWork);
        T SaveExtension(T qualificationExtension, IUnitOfWork unitOfWork);
        void DeleteExtension(int PromotionQualificationID, IUnitOfWork unitOfWork);
    }
}
