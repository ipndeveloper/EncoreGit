using System;

namespace NetSteps.Common.Extensions
{
    /// <summary>
    /// Author: John Egbert
    /// Description: Bool Extensions
    /// Created: 11-01-2008
    /// </summary>
    public static class BoolExtensions
    {
        #region Conversion Methods
        public static bool ToBool(this bool? value)
        {
            return (value == null) ? false : Convert.ToBoolean(value);
        }

        public static int ToInt(this bool? value)
        {
            return (!value.HasValue || !value.Value) ? 0 : 1;
        }

        public static int ToInt(this bool value)
        {
            return value ? 1 : 0;
        }

        public static string ToJavascriptBool(this bool value)
        {
            return value.ToString().ToLower();
        }
        #endregion
    }
}
