using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AddressValidator.Common.Helpers
{
    public static class DataAnnotationValidator
    {
        public static IEnumerable<ValidationResult> Validate(this object obj)
        {
            return obj.GetValidationErrors();
        }

        // Code with small modifications from http://nraykov.wordpress.com/2011/01/27/dataannotations-in-interfaces-with-extension-methods/
        public static bool IsValid<T>(this T entity)
        {
            var type = entity.GetType();
            var typeIntefaces = type.GetInterfaces();
            var isValid = true;

            foreach (var typeInterface in typeIntefaces)
            {
                foreach (var propertyInfo in typeInterface.GetProperties())
                {
                    var propertyValue = propertyInfo.GetValue(entity, null);
                    if (propertyInfo.GetCustomAttributes(typeof(ValidationAttribute), true).Cast<ValidationAttribute>().Any(attr => !attr.IsValid(propertyValue)))
                        isValid = false;

                    if (!isValid)
                        break;
                }

                if (!isValid)
                    break;
            }

            return isValid;
        }

        static IEnumerable<ValidationResult> GetValidationErrors<T>(this T entity)
        {
            var type = entity.GetType();
            var typeInterfaces = type.GetInterfaces();

            foreach (var typeInterface in typeInterfaces)
            {
                foreach (var propertyInfo in typeInterface.GetProperties())
                {
                    var propertyValue = propertyInfo.GetValue(entity, null);
                    foreach (var validationAttribute in propertyInfo.GetCustomAttributes(typeof(ValidationAttribute), true).Cast<ValidationAttribute>().Where(attr => !attr.IsValid(propertyValue)))
                        yield return new ValidationResult(validationAttribute.FormatErrorMessage(propertyInfo.Name));
                }
            }
        }
    }
}
