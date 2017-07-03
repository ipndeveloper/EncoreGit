
namespace NetSteps.Common.Validation.NetTiers
{
    public class CustomValidationMessages
    {
        public static string NumericCharactersAndEmptyStrings
        {
            get
            {
                return "'{0}' must only contain numeric characters or be an empty string.";
            }
        }

        public static string Email
        {
            get
            {
                return "'{PropertyValue}' is not a valid email address.";
            }
        }

        public static string ShippingPOBox
        {
            get
            {
                return "'{PropertyValue}' is not a valid shipping address.";
            }
        }

        public static string ShippingAddressCannotBeAPOBox
        {
            get
            {
                return "Invalid shipping address. The shipping address cannot be a PO Box for the selected shipping method.";
            }
        }

        public static string NoShippingMethod
        {
            get
            {
                return "No shipping method. Please choose shipping method and try again.";
            }
        }

        public static string DateTimeMin
        {
            get
            {
                return "'{0}' must be a value greater than 1/1/1753.";
            }
        }

        public static string DateTimeMinWithValue
        {
            get
            {
                return "'{PropertyName}' ({PropertyValue}) must be a value greater than 1/1/1753.";
            }
        }
    }
}
