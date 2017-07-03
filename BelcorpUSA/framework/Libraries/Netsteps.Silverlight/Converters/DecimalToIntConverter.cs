using System;
using System.Windows.Data;

using NetSteps.Silverlight.Extensions;

namespace NetSteps.Silverlight.Converters
{
    public class DecimalToIntConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int result = System.Convert.ToInt32(value);
            if (value is decimal)
                result = ((decimal)value).ToInt();

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
