using System;

namespace NetSteps.Silverlight.Extensions
{
    public static class DoubleExtensions
    {
        public static string TruncateDoubleInsertCommas(this double value)
        {
            string returnString = string.Empty;
            int decimalStartPos = 0;

            returnString = string.Format("{0:n}", value);

            decimalStartPos = returnString.IndexOf(".");

            if (decimalStartPos > 0)
                returnString = returnString.Substring(0, decimalStartPos);

            return returnString;
        }

        /// <summary>
        ///  Returns a positive value representing the difference between 2 double values. - JHE
        /// </summary>
        /// <param name="value"></param>
        /// <param name="otherValue"></param>
        /// <returns></returns>
        public static double GetDelta(this double value, double otherValue)
        {
            double delta = value - otherValue;
            return (delta < 0) ? delta * -1 : delta;
        }

        /// <summary>
        /// http://blogs.msdn.com/avip/archive/2008/09/30/trivial-but-useful-extension-method.aspx
        /// </summary>
        /// <param name="num"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static double Constrain(this double num, double min, double max)
        {
            if (num < min)
                return min;
            else if (num > max)
                return max;
            else
                return num;
        }

        public static double DegreesToRadians(this double degrees)
        {
            double radians = ((degrees / 360) * 2 * Math.PI);
            return radians;
        }

        //public static bool IsNaN(this double value)
        //{
        //    return (value.ToString() == "NaN");
        //}

        public static bool IsPositive(this double value)
        {
            return (value > 0);
        }

        public static int ToInt(this double value)
        {
            return Convert.ToInt32(value);
        }

        public static double ToSilverlightPercentage(this double value)
        {
            return value * 100;
        }
    }
}
