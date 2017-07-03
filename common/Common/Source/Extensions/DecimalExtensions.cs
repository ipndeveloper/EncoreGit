using System;
using System.Globalization;

namespace NetSteps.Common.Extensions
{
    /// <summary>
    /// Author: John Egbert
    /// Description: Decimal Extensions
    /// Created: 11-01-2008
    /// </summary>
    public static class DecimalExtensions
    {
        #region Conversion Methods
        public static int ToInt(this decimal value)
        {
            return Convert.ToInt32(value);
        }

        public static string ToMoneyString(this decimal value)
        {
            return value.ToMoneyString(new CultureInfo("en-US", false));
        }
        public static string ToMoneyString(this decimal value, CultureInfo cultureInfo)
        {
            return value.ToString("c", cultureInfo);
        }

        public static string ToMoneyString(this decimal? value)
        {
            return value.ToMoneyString(new CultureInfo("en-US", false));
        }
        public static string ToMoneyString(this decimal? value, CultureInfo cultureInfo)
        {
            return value.ToDecimal().ToString("c", cultureInfo);
        }

        public static decimal ToDecimal(this decimal? value)
        {
            return value.HasValue ? value.Value : 0;
        }

        public static double ToDouble(this decimal value)
        {
            return Convert.ToDouble(value);
        }
        #endregion

        public static string Truncate(this decimal value)
        {
            return value.Truncate(false);
        }

        public static string Truncate(this decimal value, bool insertCommas)
        {
            string returnString = string.Empty;
            int decimalStartPos = 0;

            if (insertCommas)
                returnString = string.Format("{0:n}", value);
            else
                returnString = value.ToString();

            decimalStartPos = returnString.IndexOf(".");

            if (decimalStartPos > 0)
                returnString = returnString.Substring(0, decimalStartPos);

            return returnString;
        }

        public static decimal GetRoundedNumber(this decimal numberToRound)
        {
            return Math.Round(numberToRound, 2, MidpointRounding.AwayFromZero);
        }

        public static decimal GetRoundedNumber(this decimal? numberToRound)
        {
            return Math.Round(numberToRound.ToDecimal(), 2, MidpointRounding.AwayFromZero);
        }

        public static decimal GetRoundedNumber(this decimal numberToRound, int numberOfDecimals)
        {
            return Math.Round(numberToRound, numberOfDecimals, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// Asymmetric rounding rounds away from 0 for positive and towards 0 for negative
        /// </summary>
        /// <param name="numberToRound"></param>
        /// <param name="numberOfDecimals"></param>
        /// <returns></returns>
        public static decimal AsymmetricRoundedNumber(this decimal? numberToRound, int numberOfDecimals = 2)
        {
            return numberToRound.ToDecimal().AsymmetricRoundedNumber(numberOfDecimals);
        }

        /// <summary>
        /// Asymmetric rounding rounds away from 0 for positive and towards 0 for negative
        /// </summary>
        /// <param name="numberToRound"></param>
        /// <param name="numberOfDecimals"></param>
        /// <returns></returns>
        public static decimal AsymmetricRoundedNumber(this decimal numberToRound, int numberOfDecimals = 2)
        {
            if (numberOfDecimals < 0)
                throw new ArgumentException("Number of decimal places must be 0 or greater", "numberOfDecimals");

            decimal num = numberToRound;
            for (int i = 1; i <= numberOfDecimals; i++)
                num *= 10;

            num += .5M;

            num = (decimal)((int)num);

            for (int i = 1; i <= numberOfDecimals; i++)
                num /= 10;

            return num;
        }

        public static string ToPercentageString(this decimal value)
        {
            return value.ToPercentageString(new CultureInfo("en-US", false));
        }
        public static string ToPercentageString(this decimal value, CultureInfo cultureInfo)
        {
            return value.ToString("#0.##%", cultureInfo);
        }


        public static bool IsNullOrGreaterThan(this decimal? value, decimal otherValue)
        {
            return (!value.HasValue || otherValue <= value.Value);
        }

        public static bool IsNullOrLessThan(this decimal? value, decimal otherValue)
        {
            return (!value.HasValue || otherValue >= value.Value);
        }

        public static string TruncateDoubleInsertCommas(this decimal value, int decimalPoints = 0)
        {
            return value.ToDouble().TruncateDoubleInsertCommas(decimalPoints);
        }
    }
}
