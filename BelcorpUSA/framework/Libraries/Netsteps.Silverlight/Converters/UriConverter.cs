using System;
using System.Windows.Data;

namespace NetSteps.Silverlight.Converters
{
    public class UriConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string uriString = (string)value;
            Uri outputUri = null;
            if (!Uri.TryCreate(uriString, UriKind.RelativeOrAbsolute, out outputUri))
            {
                // handle exception  
            }
            return outputUri;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
