using System;
using System.Globalization;
using System.Windows.Data;
using NetSteps.Silverlight.Extensions;

namespace NetSteps.Silverlight.Converters
{
    public class EmptyDateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime date = (DateTime)value;
            if (date.IsNullOrEmpty())
                return string.Empty;

            return date.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
