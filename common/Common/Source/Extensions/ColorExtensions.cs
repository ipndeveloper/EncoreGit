using System.Drawing;

namespace NetSteps.Common.Extensions
{
    /// <summary>
    /// Author: John Egbert
    /// Description: Color Extensions
    /// Created: 11-01-2008
    /// </summary>
    public static class ColorExtensions
    {
        #region Validation Methods
        public static bool IsEmpty(this System.Drawing.Color color)
        {
            return (color.R == 0 && color.G == 0 && color.B == 0 && color.A == 0);
        }
        #endregion

        #region Conversion Methods
        public static void FromHexValue(this System.Drawing.Color value, string colorHexValue)
        {
            value = ColorTranslator.FromHtml(colorHexValue);
        }

        public static string ToHexValue(this System.Drawing.Color value)
        {
            return ToHexValue(value, false);
        }
        public static string ToHexValue(this System.Drawing.Color value, bool removePoundSymbol)
        {
            return (removePoundSymbol) ? ColorTranslator.ToHtml(value).Replace("#", string.Empty) : ColorTranslator.ToHtml(value);
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
