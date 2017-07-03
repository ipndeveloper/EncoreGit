using System;
using System.Windows.Data;

using NetSteps.Silverlight.Extensions;

namespace NetSteps.Silverlight.Converters
{
    public class DecimalToMoneyConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string result = value.ToStringSafe();
            if (value is decimal)
                result = ((decimal)value).ToString("C");

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
