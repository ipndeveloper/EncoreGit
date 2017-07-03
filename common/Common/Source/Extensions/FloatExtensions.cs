using System;

namespace NetSteps.Common.Extensions
{
    /// <summary>
    /// Author: John Egbert
    /// Description: Float Extensions
    /// Created: 06-23-2010
    /// </summary>
    public static class FloatExtensions
    {
        #region Conversion Methods
        public static decimal ToDecimal(this float? value)
        {
            return value.HasValue ? Convert.ToDecimal(value) : 0;
        }

        public static double ToDouble(this float value)
        {
            return Convert.ToDouble(value);
        }
        #endregion
    }
}
