using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using NetSteps.Silverlight.Extensions;

namespace NetSteps.Silverlight.Converters
{
    public class DoubleToMarginConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string side = parameter.ToString().ToCleanString();

            if (value != null)
            {
                if (value is double)
                {
                    double amount = (double)value;

                    if (side.ToLower() == "Bottom".ToLower())
                        return new Thickness(0, 0, 0, amount);
                    else if (side.ToLower() == "Top".ToLower())
                        return new Thickness(0, amount, 0, 0);
                    else if (side.ToLower() == "Left".ToLower())
                        return new Thickness(amount, 0, 0, 0);
                    else if (side.ToLower() == "Right".ToLower())
                        return new Thickness(0, 0, amount, 0);
                }
                else if (value is Grid)
                {
                    Grid grid = (Grid)value;
                    if (grid.Visibility == Visibility.Collapsed)
                        return 0;

                    double amount = grid.ActualHeight;

                    if (side.ToLower() == "Bottom".ToLower())
                        return new Thickness(0, 0, 0, amount);
                    else if (side.ToLower() == "Top".ToLower())
                        return new Thickness(0, amount, 0, 0);
                    else if (side.ToLower() == "Left".ToLower())
                        return new Thickness(amount, 0, 0, 0);
                    else if (side.ToLower() == "Right".ToLower())
                        return new Thickness(0, 0, amount, 0);
                }
                else if (value is TextBox)
                {
                    TextBox textBox = (TextBox)value;
                    if (textBox.Visibility == Visibility.Collapsed)
                        return 0;

                    double amount = textBox.ActualHeight;

                    amount = amount == 0 ? 24 : amount;

                    if (side.ToLower() == "Bottom".ToLower())
                        return new Thickness(0, 0, 0, amount);
                    else if (side.ToLower() == "Top".ToLower())
                        return new Thickness(0, amount, 0, 0);
                    else if (side.ToLower() == "Left".ToLower())
                        return new Thickness(amount, 0, 0, 0);
                    else if (side.ToLower() == "Right".ToLower())
                        return new Thickness(0, 0, amount, 0);
                }
            }

            return new Thickness(0, 0, 0, 0);
        }

        // We only use one-way binding, so we don't implement this.
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
