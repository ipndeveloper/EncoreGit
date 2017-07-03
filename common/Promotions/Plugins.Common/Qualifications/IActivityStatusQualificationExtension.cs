using System.Collections.Generic;
using NetSteps.Promotions.Common.Model;

namespace NetSteps.Promotions.Plugins.Common.Qualifications
{
    public interface IActivityStatusQualificationExtension : IPromotionQualificationExtension
    {
        IList<short> ActivityStatuses { get; }
    }
}
