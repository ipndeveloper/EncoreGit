using System;
using System.Windows.Data;

namespace NetSteps.Silverlight.Converters
{
    public class TrimTextConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string text = value as string;
            if (string.IsNullOrEmpty(text))
                return text;

            int length;

            if (!int.TryParse(parameter as string, out length))
                return value;

            if (text.Length <= length)
                return text;

            return string.Concat(text.Substring(0, length - 3), "...");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
