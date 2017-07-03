using System;
using System.Text.RegularExpressions;

namespace NetSteps.Silverlight
{
    public static class Validation
    {
        /// <summary>
        /// Returns true if the string enter match a regular expression for an email. - JHE
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool IsValidEmail(this string email)
        {
            Regex rX = new System.Text.RegularExpressions.Regex(@"[a-zA-Z0-9-_]+@([a-zA-Z0-9-_]{1,67}\.){1,5}[a-zA-Z]{2,4}");
            Match m = rX.Match(email);
            if (m.Success)
                return true;
            else
                return false;
        }

        public static bool IsValidInt(this string integer)
        {
            if (string.IsNullOrEmpty(integer))
                return false;
            else
            {
                Regex numericPattern = new Regex(@"^[-+]?\d*$");
                return numericPattern.IsMatch(integer);
            }
        }

        public static bool IsValidDecimal(this string number)
        {
            if (string.IsNullOrEmpty(number))
                return false;
            else
            {
                Regex numericPattern = new Regex(@"^[-+]?\d+(\.\d+)?$");
                return numericPattern.IsMatch(number);
            }
        }

        public static bool IsValidDateTime(this string dateTime)
        {
            DateTime date;
            if (DateTime.TryParse(dateTime, out date))
                return true;

            return false;
        }

        public static bool IsValidTime(this string time)
        {
            DateTime newDateTime = DateTime.Now;
            string dateTime = string.Format("{0} {1}", newDateTime.ToShortDateString(), time);
            if (!DateTime.TryParse(dateTime, out newDateTime))
                return false;
            else
                return true;
        }
    }
}
