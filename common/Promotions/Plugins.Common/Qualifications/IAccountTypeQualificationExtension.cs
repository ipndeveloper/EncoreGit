using System.Collections.Generic;
using NetSteps.Promotions.Common.Model;

namespace NetSteps.Promotions.Plugins.Common.Qualifications
{
    public interface IAccountTypeQualificationExtension : IPromotionQualificationExtension
    {
        IList<short> AccountTypes { get; }
    }
}
