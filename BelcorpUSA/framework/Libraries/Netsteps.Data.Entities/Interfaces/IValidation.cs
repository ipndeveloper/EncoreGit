using System.Collections.Generic;
using NetSteps.Common.Validation.NetTiers;

namespace NetSteps.Data.Entities.Interfaces
{
    /// <summary>
    /// Author: John Egbert
    /// Description: Used mark entities as Validatable
    /// Created: 03-30-2010
    /// </summary>
    public interface IValidation
    {
        bool IsValid { get; }
        ValidationRules ValidationRules { get; }
        BrokenRulesList BrokenRulesList { get; }

        void AddValidationRuleHandler(ValidationRuleHandler handler, System.String propertyName);
        void AddValidationRuleHandler(ValidationRuleHandler handler, ValidationRuleArgs args);
        void Validate();
        void Validate(string propertyName);
        void Validate(System.Enum column);

        List<string> ValidatedChildPropertiesSetByParent();
    }
}
