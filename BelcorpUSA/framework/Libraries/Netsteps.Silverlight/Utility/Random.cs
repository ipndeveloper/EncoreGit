using System.Collections.Generic;
using System.Windows.Media;
using NetSteps.Silverlight.Extensions;

namespace NetSteps.Silverlight
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

        public static bool? GetNullableBoolean()
        {
            int num = random.Next(1, 3);
            if (num == 1)
                return null;
            else if (num == 2)
                return true;
            else
                return false;
        }

        // Return a random value of T type Enum. - JHE
        public static T GetEnum<T>()
        {
            System.Type enumType = typeof(T);
            if (!enumType.IsEnum)
                throw new System.ArgumentException("Type '" + enumType.Name + "' is not an enum");

            T obj = System.Activator.CreateInstance<T>();
            IList<T> list = (obj as System.Enum).GetValues<T>();
            return list.GetRandom<T>();
        }

        public static Color GetColor()
        {
            return new Color() { A = 255, B = Random.Next(0, 255).ToByte(), G = Random.Next(0, 255).ToByte(), R = Random.Next(0, 255).ToByte() };
        }

        public static SolidColorBrush GetSolidColorBrush()
        {
            return new SolidColorBrush(GetColor());
        }

        public static LinearGradientBrush GetGradientColorBrush()
        {
            return GetColor().ToLinearGradientBrush();
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
