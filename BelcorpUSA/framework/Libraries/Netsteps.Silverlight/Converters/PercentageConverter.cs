using System;
using System.Windows.Data;

namespace NetSteps.Silverlight.Converters
{
    public class PercentageConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
                return string.Format("{0}%", value.ToString());
            else
                return "0%";
        }

        //We only use one-way binding, so we don't implement this.
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
