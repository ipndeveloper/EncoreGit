using System;
using System.Windows;
using System.Windows.Data;

namespace NetSteps.Silverlight.Converters
{
    /// <summary>
    /// Implements the conversion between a <code>bool?</code> 
    /// and the <code>FontWeight</code> enum:
    /// null  = Normal
    /// false = Normal
    /// true  = Bold
    /// </summary>
    public class BoolToFontWeightConverter : IValueConverter
    {
        #region IValueConverter Members

        public FontWeight Convert(bool value)
        {
            return (value ? FontWeights.Normal : FontWeights.Bold);
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool? bWeight = (bool?)value;
            if (bWeight.HasValue)
                return (bWeight.Value ? "Normal" : "Bold");
            else
                return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }

        #endregion
    }
}
