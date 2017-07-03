
using NetSteps.Promotions.Common.Model;
namespace NetSteps.Promotions.Plugins.Common.Qualifications
{
    public interface IUseCountQualificationExtension : IPromotionQualificationExtension
    {
        int MaximumUseCount { get; set; }
        bool FirstOrdersOnly { get; set; }
    }
}
