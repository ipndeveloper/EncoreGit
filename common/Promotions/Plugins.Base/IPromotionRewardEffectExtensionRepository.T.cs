using System;
using NetSteps.Promotions.Common.Model;
using NetSteps.Data.Common;

namespace NetSteps.Promotions.Plugins.Base
{
    public interface IPromotionRewardEffectExtensionRepository<T>
    {
        T RetrieveExtension(IPromotionRewardEffectExtension reward, IUnitOfWork unitOfWork);
        T SaveExtension(T rewardEffectExtension, IUnitOfWork unitOfWork);
        void DeleteExtension(int PromotionRewardEffectID, IUnitOfWork unitOfWork);
    }
}
