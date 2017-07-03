using System;

namespace NetSteps.Silverlight.Extensions
{
    public static class ByteExtensions
    {
        public static int ToInt(this Byte value)
        {
            return value.ToString().ToInt();
        }
    }
}
