using System;
using System.Globalization;

namespace NetSteps.Common.Extensions
{
    /// <summary>
    /// Author: John Egbert
    /// Description: Int Extensions
    /// Created: 11-01-2008
    /// </summary>
    public static class IntExtensions
    {
        #region Validation Methods
        /// <summary>
        /// Determines whether the specified input is even.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>
        /// 	<c>true</c> if the specified input is even; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsEven(this int input)
        {
            bool res = false;

            if (input == 0)
                return true;

            if (input != 0 && input % 2 == 0)
                res = true;

            return res;
        }

        /// <summary>
        /// Determines whether the specified input is even or zero.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>
        /// 	<c>true</c> if [is even or zero] [the specified input]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsEvenOrZero(this int input)
        {
            bool res = false;
            if (input == 0 || input % 2 == 0)
            {
                res = true;
            }
            return res;
        }

        /// <summary>
        /// Determines whether the specified input is positive.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>
        /// 	<c>true</c> if the specified input is positive; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsPositive(this int input)
        {
            bool res = false;
            if (input > 0)
            {
                res = true;
            }
            return res;
        }

        /// <summary>
        /// Determines whether the specified input is positive.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>
        /// 	<c>true</c> if the specified input is positive; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsPositive(this int? input)
        {
            bool res = false;
            if (input != null && input > 0)
            {
                res = true;
            }
            return res;
        }

        /// <summary>
        /// Determines whether the specified input is negative.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>
        /// 	<c>true</c> if the specified input is negative; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNegative(this int input)
        {
            bool res = false;
            if (input < 0)
            {
                res = true;
            }
            return res;
        }

        /// <summary>
        /// Determines whether the specified input is zero.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>
        /// 	<c>true</c> if the specified input is zero; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsZero(this int input)
        {
            bool res = false;
            if (input == 0)
            {
                res = true;
            }
            return res;
        }

        /// <summary>
        /// Determines whether the specified input is evenly divisible by the divisor.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="divisor">The divisor.</param>
        /// <returns>
        /// 	<c>true</c> if [is evenly divisible by] [the specified input]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsEvenlyDivisibleBy(this int input, int divisor)
        {
            //?? 0 divided by anything is zero, ie true. Is that the result we want?
            //?? 0 divided by anything is zero, ie true. Is that the result we want?
            //?? 0 divided by anything is zero, ie true. Is that the result we want?
            if (divisor == 0) throw new DivideByZeroException();

            bool res = false;
            if (input % divisor == 0)
            {
                res = true;
            }
            return res;
        }

        /// <summary>
        /// Absolutes the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static int Absolute(this int input)
        {
            return Math.Abs(input);
        }

        /// <summary>
        /// Determines whether the specified input is prime.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>
        /// 	<c>true</c> if the specified input is prime; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsPrime(this int input)
        {
            if (input < 0)
                return false;

            if (input == 2)
                return true;

            if (input % 2 == 0)
                return false;

            bool prime = true;
            for (int i = 3; i <= Math.Sqrt(input); i += 2)
            {
                if (input % i == 0)
                {
                    prime = false;
                    break;
                }
            }

            if (input == 1)
                return false;

            if (input == 2)
                return true;

            return prime;
        }

        /// <summary>
        /// This method will take an integer value and determine if it is a bit value.  E.g. 1, 2, 4, 8, 16, etc
        /// </summary>
        /// <param name="target">The integer to test.</param>
        /// <returns></returns>
        public static bool IsBit(this int target)
        {
            // one is a special case, let's see if we got one
            if (target == 1)
                return true;
            else if (Math.Round(Math.Log(target) / Math.Log(2), 1) == Math.Log(target) / Math.Log(2))
                return true;
            else
                return false;
        }


        public static bool HasValue(this int? i)
        {
            return i.HasValue && i.Value > 0;
        }
        #endregion

        #region Conversion Methods
        public static double ToDouble(this int value)
        {
            return Convert.ToDouble(value);
        }

        public static short ToShort(this int value)
        {
            return Convert.ToInt16(value);
        }

        public static short ToShort(this int? value)
        {
            if (value == null)
                return 0;
            else
                return Convert.ToInt16(value);
        }

        public static short ToShort(this Int16? value)
        {
            if (value == null)
                return 0;
            else
                return Convert.ToInt16(value);
        }

        public static short? ToShortNullable(this int value)
        {
            return (short?)Convert.ToInt16(value);
        }

        public static short? ToShortNullable(this int? value)
        {
            if (value == null || value == 0)
                return null;
            else
                return (short?)Convert.ToInt16(value);
        }

        public static bool ToBool(this int value)
        {
            return Convert.ToBoolean(value);
        }

        public static Byte ToByte(this int value)
        {
            return Convert.ToByte(value);
        }

        public static int ToInt(this int? value)
        {
            if (value == null)
                return 0;
            else
                return Convert.ToInt32(value);
        }
        public static int ToInt(this int? value, int defaultValue)
        {
            if (value == null)
                return defaultValue;
            else
                return Convert.ToInt32(value);
        }

        public static int? ToIntNullable(this int value)
        {
            if (value == 0)
                return null;
            else
                return value;
        }

        public static int? ToIntNullable(this int? value)
        {
            if (value == null || value == 0)
                return null;
            else
                return value;
        }

        public static int ToInt(this short? value)
        {
            if (value == null)
                return 0;
            else
                return Convert.ToInt32(value);
        }
        #endregion
        
        public static int GetRandom(this int value, int maxValue)
        {
            return Random.Next(maxValue);
        }
        public static int GetRandom(this int value, int minValue, int maxValue)
        {
            return Random.Next(minValue, maxValue);
        }

        public static string Suffix(this int value)
        {
            if (value >= 10 && value < 20)
            {
                return "th";
            }
            switch (value % 10)
            {
                case 1:
                    return "st";
                case 2:
                    return "nd";
                case 3:
                    return "rd";
                default:
                    return "th";
            }
        }

        public static string GetMonthName(this int monthId, NetSteps.Common.Constants.MonthDisplayType monthType)
        {
            try
            {
                if (monthType == NetSteps.Common.Constants.MonthDisplayType.NumberOnly)
                    return monthId.ToString();
                else
                {
                    DateTime dtMonth = new DateTime(DateTime.Now.ApplicationNow().Year, monthId, 1);
                    string monthName = dtMonth.ToString("MMMM");
                    if (monthType == NetSteps.Common.Constants.MonthDisplayType.FullName)
                        return monthName;
                    else
                        return monthName.ToUpper().Substring(0, 3);
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string GetMonthName(this int monthId, CultureInfo cultureInfo)
        {
            return new DateTime(2000, monthId, 1).ToString("MMMM", cultureInfo);
        }

        public static string PadWithZeros(this int value, int numberOfZeros)
        {
            return value.ToString().PadLeft(numberOfZeros, '0');
        }

        /// <summary>
        /// Taken from http://sharpertutorials.com/pretty-format-bytes-kb-mb-gb/ - JHE
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string FormatBytes(this int bytes)
        {
            string filesize;
            if (bytes >= 1073741824)
            {
                decimal size = decimal.Divide(bytes, 1073741824);
                filesize = string.Format("{0:##.##} GB", size);
            }
            else if (bytes >= 1048576)
            {
                decimal size = decimal.Divide(bytes, 1048576);
                filesize = string.Format("{0:##.##} MB", size);
            }
            else if (bytes >= 1024)
            {
                decimal size = decimal.Divide(bytes, 1024);
                filesize = string.Format("{0:##.##} KB", size);
            }
            else if (bytes > 0 & bytes < 1024)
            {
                decimal size = bytes;
                filesize = string.Format("{0:##.##} B", size);
            }
            else
            {
                filesize = "0 B";
            }
            return filesize;
        }

        public static bool IsNullOrGreaterThan(this int? value, int otherValue)
        {
            return (!value.HasValue || otherValue <= value.Value);
        }

        public static bool IsNullOrLessThan(this int? value, int otherValue)
        {
            return (!value.HasValue || otherValue >= value.Value);
        }

    }
}
