using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestMasterHelpProvider.Extensions
{
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
            if (input != 0 && input % 2 == 0)
            {
                res = true;
            }
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

        #endregion

        #region Conversion Methods

		/// <summary>
		/// Converts this int to a double.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
        public static double ToDouble(this int value)
        {
            return Convert.ToDouble(value);
        }

		/// <summary>
		/// Converts this int to a bool.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
        public static bool ToBool(this int value)
        {
            return Convert.ToBoolean(value);
        }

        #endregion

		/// <summary>
		/// Using this int as the min Value gets a number within this and the maxValue range.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="maxValue"></param>
		/// <returns></returns>
        public static int GetRandom(this int value, int maxValue = Int32.MaxValue)
        {
            return Random.Next(value, maxValue);
        }

		/// <summary>
		/// Adds an ordinal suffix and returns a string.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
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
    }
}
