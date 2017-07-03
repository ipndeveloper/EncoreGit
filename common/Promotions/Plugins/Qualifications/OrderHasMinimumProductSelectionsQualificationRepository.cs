using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.IoC;
using NetSteps.Promotions.Plugins.Qualifications.Base;
using NetSteps.Promotions.Plugins.Common.Qualifications;
using NetSteps.Promotions.Plugins.EntityModel;

namespace NetSteps.Promotions.Plugins.Qualifications
{
    [ContainerRegister(typeof(IOrderHasMinimumProductSelectionsQualificationRepository), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
    public class OrderHasMinimumProductSelectionsQualificationRepository : BasePromotionQualificationRepository<IOrderHasMinimumProductSelectionsQualificationExtension, PromotionQualificationOrderHasMinimumProductSelections>, IOrderHasMinimumProductSelectionsQualificationRepository
    {
    }
}
