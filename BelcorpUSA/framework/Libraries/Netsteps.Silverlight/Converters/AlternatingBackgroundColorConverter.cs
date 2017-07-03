using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace NetSteps.Silverlight.Converters
{
    public class AlternatingBackgroundColorConverter : IValueConverter
    {
        #region IValueConverter Members

        bool flag = false;
        Brush brush1 = CommonResources.GradientBrushes.GreyToWhite;
        Brush brush2 = CommonResources.GradientBrushes.DarkGreyToWhite;
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!string.IsNullOrEmpty((parameter == null) ? string.Empty : parameter.ToString()))
            {
                brush1 = CommonResources.SolidBrushes.White;
                brush2 = Application.Current.Resources[parameter.ToString()] as Brush;
            }

            flag = !flag;
            return flag ? brush1 : brush2;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
