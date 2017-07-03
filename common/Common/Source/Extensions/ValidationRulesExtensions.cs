using System.Linq;
using NetSteps.Common.Validation.NetTiers;

namespace NetSteps.Common.Extensions
{
    /// <summary>
    /// Author: John Egbert
    /// Description: TimeSpan Extensions
    /// Created: 10-03-2011
    /// </summary>
    public static class ValidationRulesExtensions
    {
        public static int? GetMaxLengthRuleLength(this ValidationRules validationRules, string propertyName, string ruleName)
        {
            int? maxLength = null;
            var stringMaxLengthRule = validationRules.GetValidateRules(propertyName).FirstOrDefault(r => r.RuleName.ContainsIgnoreCase(ruleName.ToCleanString()));
            if (stringMaxLengthRule != null && stringMaxLengthRule.ValidationRuleArgs is NetSteps.Common.Validation.NetTiers.CommonRules.MaxLengthRuleArgs)
                maxLength = (stringMaxLengthRule.ValidationRuleArgs as NetSteps.Common.Validation.NetTiers.CommonRules.MaxLengthRuleArgs).MaxLength;
            return maxLength;
        }
    }
}
