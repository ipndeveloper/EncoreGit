using System;
using System.Globalization;
using System.Windows.Data;
using NetSteps.Silverlight.Extensions;

namespace NetSteps.Silverlight.Converters
{
    public class EnumToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Enum)
                return (value as Enum).PascalToSpaced();
            else
                return string.Empty;
                //throw new Exception("Invalid object type. Object must be an enumeration.");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Type objectType = value.GetType();
            return Enum.Parse(objectType, value.ToString(), true);
            //throw new NotImplementedException();
        }
    }
}
