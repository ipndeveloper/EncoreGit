using System;
using System.Globalization;
using System.Windows.Data;
using NetSteps.Silverlight.Extensions;

namespace NetSteps.Silverlight.Converters
{
    public class DownlineDetailNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result = string.Format("{0}", value);

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class DownlineDetailValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result = string.Format("{0}", (value == null || "null".Equals(value.ToStringSafe().ToLower())) ? string.Empty : value);

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
