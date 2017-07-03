using System;
using NetSteps.Promotions.Plugins.Qualifications.Base;
using NetSteps.Promotions.Plugins.EntityModel;
using NetSteps.Encore.Core.IoC;
using NetSteps.Promotions.Plugins.Common.Qualifications;

namespace NetSteps.Promotions.Plugins.Qualifications
{
    [ContainerRegister(typeof(IContinuityQualificationRepository), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
    public class ContinuityQualificationRepository : BasePromotionQualificationRepository<IContinuityQualificationExtension, PromotionQualificationContinuity>, IContinuityQualificationRepository
    {
    }
}
