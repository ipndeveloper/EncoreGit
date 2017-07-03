using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestMasterHelpProvider.Extensions;

namespace TestMasterHelpProvider.DataFaker
{
   public static class Random
    {
        private static System.Random random = new System.Random();

        public static int Next()
        {
            return random.Next();
        }

        public static int Next(int maxValue)
        {
            return random.Next(maxValue);
        }

        public static int Next(int minValue, int maxValue)
        {
            return random.Next(minValue, maxValue);
        }

        public static bool GetBoolean()
        {
            return random.Next(2).ToBool();
        }

        public static System.DateTime GetBirthdayAtLeast18()
        {
            return GetDateTime(System.DateTime.Now.AddYears(-80), System.DateTime.Now.AddYears(-18));
        }

        public static System.DateTime GetBirthday()
        {
            return GetDateTime(System.DateTime.Now.AddYears(-80), System.DateTime.Now.AddYears(-10));
        }

        /// <summary>
        /// This function is not perfect but works ok for now - JHE
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public static System.DateTime GetDateTime(System.DateTime minValue, System.DateTime maxValue)
        {
            var timeSpan = maxValue - minValue;
            return minValue.AddDays((double)random.Next(1, (int)timeSpan.TotalDays - 1)).AddSeconds(random.Next(1, 86400));
        }
    }
}
