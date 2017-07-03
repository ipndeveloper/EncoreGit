using System;

namespace NetSteps.Silverlight.Extensions
{
    public static class IntExtensions
    {
        public static int GetRandom(this int value, int maxValue)
        {
            return Random.Next(maxValue);
        }
        public static int GetRandom(this int value, int minValue, int maxValue)
        {
            return Random.Next(minValue, maxValue);
        }

        public static double ToDouble(this int value)
        {
            return Convert.ToDouble(value);
        }

        public static bool ToBool(this int value)
        {
            return Convert.ToBoolean(value);
        }

        public static Byte ToByte(this int value)
        {
            return Convert.ToByte(value);
        }

        public static bool IsEven(this int value)
        {
            return (value % 2 == 0);
        }

        public static string PadWithZeros(this int value, int numberOfZeros)
        {
            return value.ToString().PadLeft(numberOfZeros, '0');
        }
    }
}
