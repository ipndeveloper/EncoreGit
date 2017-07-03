using System;
using System.Windows.Data;
using System.Windows.Media;
using NetSteps.Silverlight.Extensions;

namespace NetSteps.Silverlight.Converters
{
    public class SolidBackgroundColorConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string)
            {
                Color backgroundColor = new Color();
                return backgroundColor.Parse(value.ToString());
            }
            else if (value is Color)
            {
                return value;
            }
            else
                throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
