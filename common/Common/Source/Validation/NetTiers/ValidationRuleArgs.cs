using System.Xml.Serialization;
using NetSteps.Common.Globalization;

namespace NetSteps.Common.Validation.NetTiers
{
    /// <summary>
    /// Object that provides additional information about an validation rule.
    /// </summary>
    public class ValidationRuleArgs
    {
        private string _propertyName;
        private string _description;
        private object _tag;
        private string _errorMessage;

        /// <summary>
        /// Gets or sets the tag.
        /// </summary>
        /// <value>The tag.</value>
        [XmlIgnore]
        public object Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        /// <summary>
        /// The name of the property to be validated.
        /// </summary>
        public string PropertyName
        {
            get { return _propertyName; }
        }
        public string PropertyNameTranslated
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_propertyName))
                    return Translation.TermTranslation.GetTerm(_propertyName, _propertyName);
                else
                    return _propertyName;
            }
        }

        /// <summary>
        /// Detailed description of why the rule was invalidated.  This should be set from the method handling the rule.
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        /// <summary>
        /// The Regular expression that the string have to match.
        /// </summary>
        public string ErrorMessage
        {
            get { return _errorMessage; }
        }

        /// <summary>
        /// Creates an instance of the object
        /// </summary>
        /// <param name="propertyName">The name of the property to be validated.</param>
        public ValidationRuleArgs(string propertyName)
        {
            _propertyName = propertyName;
        }

        public ValidationRuleArgs(string propertyName, string errorMessage)
        {
            _propertyName = propertyName;
            _errorMessage = errorMessage;
        }

        /// <summary>
        /// Return a string representation of the object.
        /// </summary>
        public override string ToString()
        {
            return _propertyName;
        }

        public string GetCustomErrorMessage(string propertyValue)
        {
            if (!string.IsNullOrWhiteSpace(ErrorMessage))
            {
                if (ErrorMessage.Contains("{0}"))
                    return string.Format(ErrorMessage, PropertyNameTranslated);
                else if (ErrorMessage.Contains("{PropertyName}") || ErrorMessage.Contains("{PropertyValue}"))
                    return ErrorMessage.Replace("{PropertyName}", PropertyNameTranslated).Replace("{PropertyValue}", propertyValue);
                else
                    return ErrorMessage;
            }
            else
                return string.Empty;
        }
    }
}
