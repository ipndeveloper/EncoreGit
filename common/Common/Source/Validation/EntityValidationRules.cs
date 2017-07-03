using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace NetSteps.Common.Validation
{
    /// <summary>
    /// Author: John Egbert
    /// Description: Helper class to work with System.ComponentModel.DataAnnotations for validation.
    /// Example Usage:  EntityValidationRules rules = new EntityValidationRules(archive);
    ///                 if (!rules.IsValid)
    ///                     throw new Exception(rules.GetBrokenRules(e2.PropertyName).First().FullErrorMessage);
    /// Created: 03-05-2010
    /// </summary>
    public class EntityValidationRules
    {
        #region Members
        private object _validatingObject = null;
        #endregion

        #region Properties
        public Dictionary<PropertyDescriptor, List<ValidationAttribute>> _rules = null;
        public Dictionary<PropertyDescriptor, List<ValidationAttribute>> Rules
        {
            get
            {
                if (_rules == null)
                    _rules = DataAnnotationHelpers.GetPropertiesWithDataAnnotationAttributes(_validatingObject);
                return _rules;
            }
            set
            {
                _rules = value;
            }
        }

        public bool IsValid
        {
            get
            {
                return BrokenRules.Count() == 0;
            }
        }

        public IEnumerable<ValidationMessage> BrokenRules
        {
            get
            {
                var propertyValidationRules = DataAnnotationHelpers.GetPropertiesWithDataAnnotationAttributes(_validatingObject);

                List<ValidationMessage> errors = new List<ValidationMessage>();
                foreach (var key in propertyValidationRules.Keys)
                {
                    foreach (var attribute in propertyValidationRules[key])
                        if (!attribute.IsValid(key.GetValue(_validatingObject)))
                            errors.Add(new ValidationMessage { PropertyName = key.Name, ErrorMessage = attribute.FormatErrorMessage(string.Empty) });
                }
                return errors;
            }
        }
        #endregion

        #region Methods
        public IEnumerable<ValidationMessage> GetBrokenRules(string propertyName)
        {
            List<ValidationMessage> errors = new List<ValidationMessage>();
            foreach (var key in Rules.Keys)
            {
                if (key.Name == propertyName)
                    foreach (var attribute in Rules[key])
                        if (!attribute.IsValid(key.GetValue(_validatingObject)))
                            errors.Add(new ValidationMessage { PropertyName = key.Name, ErrorMessage = attribute.FormatErrorMessage(string.Empty) });
            }
            return errors;
        }

        public EntityValidationRules(object obj)
        {
            _validatingObject = obj;
        }
        #endregion
    }
}