using System;
using System.Reflection;
using System.Windows;
using System.Windows.Media;

namespace NetSteps.Silverlight.Extensions
{
    public static class ColorExtensions
    {
        /*
        public static Color ToColor(this string color)
        {
            try
            {
                Type colorType = (typeof(System.Windows.Media.Colors));
                if (colorType.GetProperty(color) != null)
                {
                    object o = colorType.InvokeMember(color, BindingFlags.GetProperty, null, null, null);
                    if (o != null)
                        return (Color)o;
                    else
                        return Colors.Black;
                }
                else
                {
                    // Try Hex conversion - JHE
                    int startIndex = color.StartsWith("#") ? 1 : 0;
                    byte a = System.Convert.ToByte("FF", 16);
                    byte r = System.Convert.ToByte("FF", 16);
                    byte g = System.Convert.ToByte("FF", 16);
                    byte b = System.Convert.ToByte("FF", 16);
                    if (color.Length >= 8)
                    {
                        a = System.Convert.ToByte(color.Substring(startIndex, 2), 16);
                        startIndex = color.StartsWith("#") ? 3 : 2;
                    }

                    r = System.Convert.ToByte(color.Substring(startIndex, 2), 16);
                    g = System.Convert.ToByte(color.Substring(startIndex + 2, 2), 16);
                    b = System.Convert.ToByte(color.Substring(startIndex + 4, 2), 16);

                    Color s = Color.FromArgb(a, r, g, b);
                    return s;
                }
            }
            catch
            {
                return Colors.Black;
            }
        }
        */
        private static Color? convertHexValueToColor(string hexValue)
        {
            hexValue = hexValue.Replace("#", string.Empty);

            if (hexValue.Length == 6)
                hexValue = "FF" + hexValue;

            if (hexValue.Length != 8)
                return null;

            byte a = Convert.ToByte(hexValue.Substring(0, 2), 16);
            byte r = Convert.ToByte(hexValue.Substring(2, 2), 16);
            byte g = Convert.ToByte(hexValue.Substring(4, 2), 16);
            byte b = Convert.ToByte(hexValue.Substring(6, 2), 16);

            return Color.FromArgb(a, r, g, b);
        }

        private static Color? convertColorNameToColor(string colorName)
        {
            Type colorType = (typeof(System.Windows.Media.Colors));
            if (colorType.GetProperty(colorName) != null)
            {
                object o = colorType.InvokeMember(colorName, BindingFlags.GetProperty, null, null, null);
                if (o != null)
                    return (Color)o;
            }
            else if (colorName.ToUpper() == "PINK")
                return "#e367e2".ToColor();

            return null;
        }

        public static Color Parse(this Color color, string hexOrColorName)
        {
            Color? result = convertColorNameToColor(hexOrColorName);
            if (result == null)
                result = convertHexValueToColor(hexOrColorName);

            return result == null ? Colors.Black : (Color)result;
        }

        public static Color ToColor(this string hexOrColorName)
        {
            Color? result = convertColorNameToColor(hexOrColorName);
            if (result == null)
                result = convertHexValueToColor(hexOrColorName);

            return result == null ? Colors.Black : (Color)result;
        }

        #region Brush Methods
        public static SolidColorBrush ToSolidColorBrush(this Color color)
        {
            return new SolidColorBrush(color);
        }

        public static LinearGradientBrush ToLinearGradientBrush(this Color color)
        {
            return color.ToLinearGradientBrush(.70);
        }
        public static LinearGradientBrush ToLinearGradientBrush(this Color color, double colorDifferenceFactor)
        {
            LinearGradientBrush linearGradientBrush = new LinearGradientBrush();
            linearGradientBrush.StartPoint = new Point() { X = 0.5, Y = 0 };
            linearGradientBrush.EndPoint = new Point() { X = 0.5, Y = 1 };
            linearGradientBrush.GradientStops.Add(new GradientStop() { Color = color.Darken(colorDifferenceFactor) });
            linearGradientBrush.GradientStops.Add(new GradientStop() { Color = color, Offset = 1 });
            return linearGradientBrush;
        }
        public static LinearGradientBrush ToLinearGradientBrush(this Color color, Color endcolor)
        {
            LinearGradientBrush linearGradientBrush = new LinearGradientBrush();
            linearGradientBrush.StartPoint = new Point() { X = 0.5, Y = 0 };
            linearGradientBrush.EndPoint = new Point() { X = 0.5, Y = 1 };
            linearGradientBrush.GradientStops.Add(new GradientStop() { Color = endcolor });
            linearGradientBrush.GradientStops.Add(new GradientStop() { Color = color, Offset = 1 });
            return linearGradientBrush;
        }
        #endregion

        public static Color Brighten(this Color color, double factor)
        {
            return Color.FromArgb(255, GetMultipliedValue(color.R, factor), GetMultipliedValue(color.G, factor), GetMultipliedValue(color.B, factor));
        }

        public static Color Darken(this Color color, double factor)
        {
            return Color.FromArgb(255, GetMultipliedValue(color.R, factor), GetMultipliedValue(color.G, factor), GetMultipliedValue(color.B, factor));
        }

        private static byte GetMultipliedValue(byte colorByte, double factor)
        {
            float result = (float)(colorByte * factor);
            if (result > 255)
                return (byte)255;
            else
                return (byte)result;
        }
    }
}
