using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using NetSteps.Common.Interfaces;
using NetSteps.Common.Validation;

namespace NetSteps.Common.Extensions
{
    /// <summary>
    /// Author: John Egbert
    /// Description: IValidatedModel Extensions
    /// Add IValidatedModel to objects with DataAnnotations attributes you would like to validate.
    /// Created: 03-05-2010
    /// </summary>
    public static class IValidatedModelExtensions
    {
        /// <summary>
        /// Get all validation errors for the given instance. - JHE
        /// </summary>
        public static IEnumerable<ValidationMessage> GetErrors(this IValidatedModel instance)
        {
            EntityValidationRules rules = new EntityValidationRules(instance);
            return rules.BrokenRules;
        }

        /// <summary>
        /// Get all validation errors for the given instance's property. - JHE
        /// </summary>
        public static IEnumerable<ValidationMessage> GetErrors(this IValidatedModel instance, string propertyName)
        {
            EntityValidationRules rules = new EntityValidationRules(instance);
            return rules.GetBrokenRules(propertyName);
        }

        /// <summary>
        /// Are there any validation errors? 
        /// </summary>
        public static bool IsValid(this IValidatedModel instance)
        {
            EntityValidationRules rules = new EntityValidationRules(instance);
            return rules.IsValid;
        }

        /// <summary>
        /// This will cause validation DataAnnotation rules to be run on the property that changed
        /// and an Exception will be thrown if any rules are broken. - JHE
        /// </summary>
        /// <param name="instance"></param>
        public static void RegisterThrowExceptionOnValidationError(this IValidatedModel instance)
        {
            if (instance is INotifyPropertyChanged)
            {
                (instance as INotifyPropertyChanged).PropertyChanged -= new PropertyChangedEventHandler((object sender, PropertyChangedEventArgs e) =>
                {
                    var errors = instance.GetErrors(e.PropertyName);
                    if (!instance.IsValid())
                        throw new Exception(errors.First().FullErrorMessage);
                });
                (instance as INotifyPropertyChanged).PropertyChanged += new PropertyChangedEventHandler((object sender, PropertyChangedEventArgs e) =>
                {
                    var errors = instance.GetErrors(e.PropertyName);
                    if (!instance.IsValid())
                        throw new Exception(errors.First().FullErrorMessage);
                });
            }
        }
    }
}