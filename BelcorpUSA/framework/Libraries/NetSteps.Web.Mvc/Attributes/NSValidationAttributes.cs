/*
 * NetSteps Validation Attributes
 * Author: Jeremy Lundy
 * Description: A collection of extended validation attributes that provide
 *              term-translated error messages and conditional validation.
 */
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using NetSteps.Common;
using NetSteps.Common.Globalization;

namespace System.ComponentModel.DataAnnotations
{
    #region Base Class
    public abstract class NSValidationAttribute : ValidationAttribute
    {
        public string Condition { get; set; }
        public string TermName { get; set; }
        public string ErrorArgsMethod { get; set; }
        public bool DisableClientValidation { get; set; }
        
        public object[] ErrorArgs { get; set; }

        /// <summary>
        /// Performs term translation. Put custom logic here.
        /// </summary>
        private string LocalizeErrorMessage(string termName, string defaultErrorMessage)
        {
            return Translation.GetTerm(termName, defaultErrorMessage);
        }

        /// <summary>
        /// Gets the current culture. Put custom logic here.
        /// </summary>
        private CultureInfo CurrentCulture
        {
            get
            {
                return CultureInfo.CurrentCulture;
            }
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return this.IsValid(value, validationContext, this.InnerIsValid);
        }

        public override string FormatErrorMessage(string name)
        {
            return this.FormatErrorMessage(new object[] { name });
        }

        protected virtual bool InnerIsValid(object value, ValidationContext validationContext)
        {
            return this.IsValid(value);
        }

        protected string FormatErrorMessage(object[] defaultErrorArgs)
        {
            var errorMessage = string.IsNullOrWhiteSpace(this.TermName)
                ? this.ErrorMessageString
                : LocalizeErrorMessage(this.TermName, this.ErrorMessageString);

            var errorArgs = new List<object>();
            if (defaultErrorArgs != null && defaultErrorArgs.Length > 0) errorArgs.AddRange(defaultErrorArgs);
            if (this.ErrorArgs != null && this.ErrorArgs.Length > 0) errorArgs.AddRange(this.ErrorArgs);

            return string.Format(CurrentCulture, errorMessage, errorArgs.ToArray());
        }

        protected virtual ModelClientValidationRule GetDefaultClientValidationRule(string validationType, ModelMetadata metadata, ControllerContext context)
        {
            if (metadata == null || string.IsNullOrWhiteSpace(validationType) || DisableClientValidation) return null;

            if (!string.IsNullOrWhiteSpace(this.ErrorArgsMethod))
            {
                this.LoadErrorArgs(context);
            }

            // Default rule
            var rule = new ModelClientValidationRule()
            {
                ErrorMessage = this.FormatErrorMessage(metadata.GetDisplayName()),
                ValidationType = validationType
            };

            if (!string.IsNullOrWhiteSpace(this.Condition))
            {
                // May need to add validation parameters here for conditional client-side validation.
                // For now, the easiest solution is to add 'ignore: ":hidden"' to the default options
                // structure in jquery.validate.unobtrusive.js: 
            }

            return rule;
        }

        private ValidationResult IsValid(object value, ValidationContext validationContext, Func<object, ValidationContext, bool> innerIsValid)
        {
            var result = ValidationResult.Success;

            if (this.IsEnabled(validationContext) && innerIsValid != null && !innerIsValid(value, validationContext))
            {
                if (!string.IsNullOrWhiteSpace(this.ErrorArgsMethod))
                {
                    this.LoadErrorArgs(validationContext);
                }

                string[] memberNames = (validationContext.MemberName != null) ? new string[] { validationContext.MemberName } : null;
                result = new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName), memberNames);
            }

            return result;
        }

        protected virtual bool IsEnabled(ValidationContext validationContext)
        {
            var isEnabled = true;

            if (validationContext != null
                && validationContext.ObjectInstance != null
                && !string.IsNullOrWhiteSpace(this.Condition))
            {
                var conditionField = validationContext.ObjectType.GetField(this.Condition, BindingFlags.Public | BindingFlags.Static);
                if (conditionField != null)
                {
                    var conditionDelegate = conditionField.GetValue(null) as Delegate;
                    if (conditionDelegate != null)
                    {
                        var conditionType = conditionDelegate.GetType();
                        if (conditionType.IsGenericType
                            && conditionType.GetGenericTypeDefinition() == typeof(Predicate<>)
                            && conditionType.GetGenericArguments()[0] == validationContext.ObjectType)
                        {
                            isEnabled = (bool)conditionDelegate.DynamicInvoke(validationContext.ObjectInstance);
                        }
                    }
                }
            }

            return isEnabled;
        }

        private void LoadErrorArgs(ValidationContext validationContext)
        {
            if (validationContext != null
                && validationContext.ObjectInstance != null
                && !string.IsNullOrWhiteSpace(this.ErrorArgsMethod))
            {
                var errorArgsMethod = validationContext.ObjectType.GetMethod(this.ErrorArgsMethod);
                if (errorArgsMethod != null)
                {
                    var errorArgs = errorArgsMethod.Invoke(validationContext.ObjectInstance, null);
                    if (errorArgs != null && errorArgs.GetType() == typeof(object[]))
                    {
                        this.ErrorArgs = (object[])errorArgs;
                    }
                }
            }
        }

        private void LoadErrorArgs(ControllerContext controllerContext)
        {
            if (controllerContext != null
                && !string.IsNullOrWhiteSpace(this.ErrorArgsMethod))
            {
                var viewContext = controllerContext as ViewContext;
                if (viewContext != null
                    && viewContext.ViewData != null
                    && viewContext.ViewData.Model != null)
                {
                    var errorArgsMethod = viewContext.ViewData.Model.GetType().GetMethod(this.ErrorArgsMethod);
                    if (errorArgsMethod != null)
                    {
                        var errorArgs = errorArgsMethod.Invoke(viewContext.ViewData.Model, null);
                        if (errorArgs != null && errorArgs.GetType() == typeof(object[]))
                        {
                            this.ErrorArgs = (object[])errorArgs;
                        }
                    }
                }
            }
        }
    }
    #endregion

    public class NSCompareAttribute : NSValidationAttribute, IClientValidatable
    {
        public string OtherProperty { get; private set; }
        public bool IgnoreCase { get; set; }

        public NSCompareAttribute(string otherProperty)
        {
            if (otherProperty == null) throw new ArgumentNullException("otherProperty");

            this.TermName = "ErrorFieldsDoNotMatch";
            this.ErrorMessage = "'{0}' and '{1}' do not match.";
            this.OtherProperty = otherProperty;
        }

        protected override bool InnerIsValid(object value, ValidationContext validationContext)
        {
            if (validationContext == null) return false;

            var otherProperty = validationContext.ObjectType.GetProperty(this.OtherProperty);
            if (otherProperty == null) return false;

            object otherValue = otherProperty.GetValue(validationContext.ObjectInstance, null);

            // IgnoreCase only works with strings
            if (this.IgnoreCase && value is string && otherValue is string)
                return string.Equals(value as string, otherValue as string, StringComparison.OrdinalIgnoreCase);

            return object.Equals(value, otherValue);
        }

        public override string FormatErrorMessage(string name)
        {
            return base.FormatErrorMessage(new object[] { name, this.OtherProperty });
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            // TODO: Replace equalto with a method that can ignore case.
            var rule = base.GetDefaultClientValidationRule("equalto", metadata, context);
            if (rule != null)
            {
                rule.ValidationParameters["other"] = "*." + this.OtherProperty;

                yield return rule;
            }
        }
    }

    public class NSCreditCardAttribute : NSValidationAttribute, IClientValidatable
    {
        public NSCreditCardAttribute()
        {
            this.TermName = "ErrorInvalidCreditCardNumberField";
            this.ErrorMessage = "{0} is invalid.";
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            string ccValue = value as string;
            if (ccValue == null)
            {
                return false;
            }

            ccValue = ccValue.Replace("-", "");
            if (string.IsNullOrEmpty(ccValue))
            {
                return false;
            }

            int checksum = 0;
            bool evenDigit = false;

            foreach (char digit in ccValue.Reverse<char>())
            {
                if (!char.IsDigit(digit))
                {
                    return false;
                }

                int digitValue = (digit - '0') * (evenDigit ? 2 : 1);
                evenDigit = !evenDigit;

                while (digitValue > 0)
                {
                    checksum += digitValue % 10;
                    digitValue /= 10;
                }
            }
            return (checksum % 10) == 0;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = base.GetDefaultClientValidationRule("creditcard", metadata, context);
            if (rule != null) yield return rule;
        }
    }

    public class NSFutureValidDate : NSValidationAttribute, IClientValidatable
    {
        public DateTime Maximum { get; private set; }
        public DateTime Minimum { get; private set; }
        
        public NSFutureValidDate()
        {
            InitErrorMessage();
            this.Maximum = DateTime.MaxValue;
            this.Minimum = DateTime.Today;
        }

        private void InitErrorMessage()
        {
            this.TermName = "ErrorFieldOutOfRange";
            this.ErrorMessage = "{0} must be between {1} and {2}.";
        }

        public override bool IsValid(object value)
        {
            // Null value is valid (use NSRequired)
            if (value == null)
            {
                return true;
            }

            var valueDate = value as DateTime?;
            if (!valueDate.HasValue)
            {
                return false;
            }

            return valueDate >= this.Minimum && valueDate <= this.Maximum;
        }

        public override string FormatErrorMessage(string name)
        {
            return base.FormatErrorMessage(new object[] { name, this.Minimum, this.Maximum });
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = base.GetDefaultClientValidationRule("range", metadata, context);
            if (rule != null)
            {
                rule.ValidationParameters["min"] = this.Minimum;
                rule.ValidationParameters["max"] = this.Maximum;

                yield return rule;
            }
        }
    }

    public class NSEmailAttribute : NSRegularExpressionAttribute, IClientValidatable
    {
        /// <summary>
        /// Allow non-ASCII characters in the email address (Warning: Not supported by System.Net.Mail).
        /// </summary>
        public bool AllowNonASCII { get; set; }

        // For consistency, this should be the same pattern as the one used by jquery.validate.
        //private const string _emailPattern = @"^((([a-zA-Z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-zA-Z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-zA-Z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-zA-Z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-zA-Z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-zA-Z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-zA-Z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-zA-Z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$";

        private static readonly string _emailPattern = RegularExpressions.Email;

        public NSEmailAttribute()
            : base(_emailPattern)
        {
            this.TermName = "ErrorInvalidEmailAddressField";
            this.ErrorMessage = "{0} is not a valid email address.";
        }

        public override bool IsValid(object value)
        {
            if (!this.AllowNonASCII)
            {
                if (value == null)
                {
                    return true;
                }

                string valueString = value as string;
                if (valueString == null)
                {
                    return false;
                }

                try
                {
                    // This is the best method for validating an email address, because we use System.Net to send mail anyway.
                    // Note: It does not allow non-ASCII (i.e. foreign) characters the way jQuery.validate does.
                    new System.Net.Mail.MailAddress(valueString);
                }
                catch (FormatException)
                {
                    return false;
                }
            }

            // Regex validation
            return base.IsValid(value);
        }

        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = base.GetDefaultClientValidationRule("email", metadata, context);
            if (rule != null) yield return rule;
        }
    }

    public class NSRangeAttribute : NSValidationAttribute, IClientValidatable
    {
        public object Maximum { get { return this._innerAttribute.Maximum; } }
        public object Minimum { get { return this._innerAttribute.Minimum; } }
        public Type OperandType { get { return this._innerAttribute.OperandType; } }

        private RangeAttribute _innerAttribute;

        public NSRangeAttribute(double minimum, double maximum)
        {
            InitErrorMessage();
            this._innerAttribute = new RangeAttribute(minimum, maximum);
        }

        public NSRangeAttribute(int minimum, int maximum)
        {
            InitErrorMessage();
            this._innerAttribute = new RangeAttribute(minimum, maximum);
        }

        public NSRangeAttribute(Type type, string minimum, string maximum)
        {
            InitErrorMessage();
            this._innerAttribute = new RangeAttribute(type, minimum, maximum);
        }

        private void InitErrorMessage()
        {
            this.TermName = "ErrorFieldOutOfRange";
            this.ErrorMessage = "{0} must be between {1} and {2}.";
        }

        public override bool IsValid(object value)
        {
            return this._innerAttribute.IsValid(value);
        }

        public override string FormatErrorMessage(string name)
        {
            return base.FormatErrorMessage(new object[] { name, this.Minimum, this.Maximum });
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = base.GetDefaultClientValidationRule("range", metadata, context);
            if (rule != null)
            {
                rule.ValidationParameters["min"] = this.Minimum;
                rule.ValidationParameters["max"] = this.Maximum;

                yield return rule;
            }
        }
    }

    public class NSRegularExpressionAttribute : NSValidationAttribute, IClientValidatable
    {
        public string Pattern { get { return this._innerAttribute.Pattern; } }

        private RegularExpressionAttribute _innerAttribute;

        public NSRegularExpressionAttribute(string pattern)
        {
            this.TermName = "ErrorFieldDoesNotMatchPattern";
            this.ErrorMessage = "{0} is invalid.";
            this._innerAttribute = new RegularExpressionAttribute(pattern);
        }

        public override bool IsValid(object value)
        {
            return this._innerAttribute.IsValid(value);
        }

        public override string FormatErrorMessage(string name)
        {
            return base.FormatErrorMessage(new object[] { name, this.Pattern });
        }

        public virtual IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = base.GetDefaultClientValidationRule("regex", metadata, context);
            if (rule != null)
            {
                rule.ValidationParameters["pattern"] = this.Pattern;

                yield return rule;
            }
        }
    }

    public class NSRequiredAttribute : NSValidationAttribute, IClientValidatable
    {
        public bool AllowEmptyStrings { get { return this._innerAttribute.AllowEmptyStrings; } set { this._innerAttribute.AllowEmptyStrings = value; } }

        private RequiredAttribute _innerAttribute = new RequiredAttribute();

        public NSRequiredAttribute()
        {
            this.TermName = "ErrorFieldRequired";
            this.ErrorMessage = "{0} is required.";
        }

        public override bool IsValid(object value)
        {
            return this._innerAttribute.IsValid(value);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = this.GetDefaultClientValidationRule("required", metadata, context);
            if (rule != null) yield return rule;
        }
    }

    public class NSRequireCheckedAttribute : NSValidationAttribute, IClientValidatable
    {
        public NSRequireCheckedAttribute()
        {
            this.TermName = "ErrorFieldCheckedRequired";
            this.ErrorMessage = "{0} must be checked.";
        }

        public override bool IsValid(object value)
        {
            bool result;
            return bool.TryParse(value.ToString(), out result) ? result : false;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = this.GetDefaultClientValidationRule("requirechecked", metadata, context);
            if (rule != null) yield return rule;
        }
    }

    public class NSStringLengthAttribute : NSValidationAttribute, IClientValidatable
    {
        public int MaximumLength { get { return this._innerAttribute.MaximumLength; } }
        public int MinimumLength { get { return this._innerAttribute.MinimumLength; } set { this._innerAttribute.MinimumLength = value; } }

        private StringLengthAttribute _innerAttribute;
        private const string _defaultTermNameMax = "ErrorFieldLengthMax";
        private const string _defaultTermNameMinMax = "ErrorFieldLengthMinMax";
        private const string _defaultErrorMessageMax = "{0} must be fewer than {1} characters.";
        private const string _defaultErrorMessageMinMax = "{0} must be between {2} and {1} characters.";

        public NSStringLengthAttribute(int maximumLength)
        {
            this.TermName = _defaultTermNameMax;
            this.ErrorMessage = _defaultErrorMessageMax;
            this._innerAttribute = new StringLengthAttribute(maximumLength);
        }

        public override bool IsValid(object value)
        {
            return this._innerAttribute.IsValid(value);
        }

        public override string FormatErrorMessage(string name)
        {
            // Adjust default TermName/ErrorMessage based on whether MinimumLength is set.
            if (this.TermName == _defaultTermNameMax || this.TermName == _defaultTermNameMinMax)
            {
                this.TermName = this.MinimumLength > 0 ? _defaultTermNameMinMax : _defaultTermNameMax;
            }
            if (this.ErrorMessage == _defaultErrorMessageMax || this.ErrorMessage == _defaultErrorMessageMinMax)
            {
                this.ErrorMessage = this.MinimumLength > 0 ? _defaultErrorMessageMinMax : _defaultErrorMessageMax;
            }

            return base.FormatErrorMessage(new object[] { name, this.MaximumLength, this.MinimumLength });
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            if (this.MinimumLength != 0 || this.MaximumLength != int.MaxValue)
            {
                var rule = base.GetDefaultClientValidationRule("length", metadata, context);
                if (rule != null)
                {
                    if (this.MinimumLength != 0)
                    {
                        rule.ValidationParameters["min"] = this.MinimumLength;
                    }
                    if (this.MaximumLength != int.MaxValue)
                    {
                        rule.ValidationParameters["max"] = this.MaximumLength;
                    }

                    yield return rule;
                }
            }
        }
    }

    #region JS Validation Attributes
    public abstract class JSValidationAttribute : NSValidationAttribute, IClientValidatable
    {
        public override bool IsValid(object value)
        {
            return true;
        }

        protected override ModelClientValidationRule GetDefaultClientValidationRule(string validationType, ModelMetadata metadata, ControllerContext controllerContext)
        {
            var rule = base.GetDefaultClientValidationRule(validationType, metadata, controllerContext);
            if (rule != null)
            {
                if (rule.ErrorMessage.Contains("[ModelDisplayName]"))
                {
                    string displayName;
                    var viewContext = controllerContext as ViewContext;
                    if (viewContext != null
                        && viewContext.ViewData != null
                        && viewContext.ViewData.ModelMetadata != null)
                    {
                        displayName = viewContext.ViewData.ModelMetadata.DisplayName;
                    }
                    else
                    {
                        displayName = metadata.DisplayName;
                    }
                    rule.ErrorMessage = rule.ErrorMessage.Replace("[ModelDisplayName]", displayName);
                }
            }
            return rule;
        }

        public abstract IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext controllerContext);
    }

    public class JSRequireAnyAttribute : JSValidationAttribute
    {
        public string[] PropertyNames { get; set; }

        public JSRequireAnyAttribute(string[] propertyNames)
        {
            this.TermName = "ErrorModelRequired";
            this.ErrorMessage = "[ModelDisplayName] is required.";
            this.PropertyNames = propertyNames;
        }

        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext controllerContext)
        {
            var rule = this.GetDefaultClientValidationRule("requireany", metadata, controllerContext);
            if (rule != null)
            {
                rule.ValidationParameters["elements"] = string.Join(",", this.PropertyNames.Select(x => string.Format("*.{0}", x)));
                yield return rule;
            }
        }
    }

    public class JSDateModelAttribute : JSValidationAttribute
    {
        public JSDateModelAttribute()
        {
            this.TermName = "ErrorDateModelInvalid";
            this.ErrorMessage = "[ModelDisplayName] is invalid.";
        }

        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext controllerContext)
        {
            var rule = this.GetDefaultClientValidationRule("datemodel", metadata, controllerContext);
            if (rule != null)
            {
                yield return rule;
            }
        }
    }

    public class JSSSNModelAttribute : JSValidationAttribute
    {
        public JSSSNModelAttribute()
        {
            this.TermName = "ErrorSSNModelInvalid";
            this.ErrorMessage = "[ModelDisplayName] is invalid.";
        }

        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext controllerContext)
        {
            var rule = this.GetDefaultClientValidationRule("ssnmodel", metadata, controllerContext);
            if (rule != null)
            {
                yield return rule;
            }
        }
    }
    #endregion
}