using System;

namespace NetSteps.Silverlight.Extensions
{
    public static class DecimalExtensions
    {
        public static int ToInt(this decimal value)
        {
            return Convert.ToInt32(value);
        }
    }
}
