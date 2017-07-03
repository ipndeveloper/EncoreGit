using System;
using NetSteps.Promotions.Plugins.Qualifications.Base;
using NetSteps.Promotions.Plugins.EntityModel;
using NetSteps.Encore.Core.IoC;
using NetSteps.Promotions.Plugins.Common.Qualifications;

namespace NetSteps.Promotions.Plugins.Qualifications
{
    [ContainerRegister(typeof(IPromotionCodeQualificationRepository), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
    public class PromotionCodeQualificationRepository : BasePromotionQualificationRepository<IPromotionCodeQualificationExtension, PromotionQualificationPromotionCode>, IPromotionCodeQualificationRepository
    {
    }
}
