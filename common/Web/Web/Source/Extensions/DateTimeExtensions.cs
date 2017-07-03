using System;

namespace NetSteps.Web.Extensions
{
    /// <summary>
    /// Author: John Egbert
    /// Description: DateTime Extensions
    /// Created: 05-20-2009
    /// </summary>
    public static class DateTimeExtensions
    {
        #region Conversion Methods
        public static string ToDatePickerString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd");
        }
        #endregion
    }
}
