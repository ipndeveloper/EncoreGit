using System;
using NetSteps.Promotions.Plugins.EntityModel;
using NetSteps.Encore.Core.IoC;
using NetSteps.Promotions.Plugins.Qualifications.Base;
using NetSteps.Promotions.Plugins.Common.Qualifications;

namespace NetSteps.Promotions.Plugins.Qualifications
{
    [ContainerRegister(typeof(IProductInOrderQualificationRepository), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
    public class ProductInOrderQualificationRepository : BasePromotionQualificationRepository<IProductInOrderQualificationExtension, PromotionQualificationProductInOrder>, IProductInOrderQualificationRepository
    {
    }
}
