using System;
using System.Globalization;
using System.Windows.Data;

namespace NetSteps.Silverlight.Converters
{
    public class DateMonthYearConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime date = (DateTime)value;
            if (date.Date.Equals(DateTime.MinValue.Date) || date.Date.Equals(DateTime.MaxValue.Date))
                return string.Empty;

            return date.ToString("MMM yy");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string strValue = value.ToString();
            DateTime resultDateTime;
            return DateTime.TryParse(strValue, out resultDateTime) ? resultDateTime : value;
        }
    }
}
