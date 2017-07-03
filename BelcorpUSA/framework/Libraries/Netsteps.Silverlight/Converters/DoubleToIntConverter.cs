using System;
using System.Globalization;
using System.Windows.Data;

namespace NetSteps.Silverlight.Converters
{
    /// <summary>
    /// Represents a double to int converter.
    /// </summary>
    public class DoubleToIntConverter : IValueConverter
    {
        #region IValueConverter Members

        /// <summary>
        /// Converts a double value to int.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <param name="targetType">Target parameter.</param>
        /// <param name="parameter">Additional parameter.</param>
        /// <param name="culture">Culture to convert in.</param>
        /// <returns>Converted value.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value.ToString() != string.Empty)
                {
                    var dec = decimal.Parse(value.ToString());
                    return (int)(Math.Round(dec, 0));
                }
            }
            catch { }
            return 0;
        }

        /// <summary>
        /// Converts a int value back to double.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <param name="targetType">Target parameter.</param>
        /// <param name="parameter">Additional parameter.</param>
        /// <param name="culture">Culture to convert in.</param>
        /// <returns>Converted value.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        #endregion
    }
}