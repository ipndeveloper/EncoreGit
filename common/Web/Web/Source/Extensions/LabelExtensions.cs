using System;
using System.Web.UI.WebControls;
using NetSteps.Common.Extensions;

namespace NetSteps.Web.Extensions
{
    /// <summary>
    /// Author: John Egbert
    /// Description: Label Extensions
    /// Created: 04-04-2009
    /// </summary>
    public static class LabelExtensions
    {
        public static void SetShortDate(this Label label, DateTime dateTime)
        {
            label.Text = (dateTime.IsNullOrEmpty()) ? "N/A" : dateTime.ToShortDateString();
            label.ToolTip = (dateTime.IsNullOrEmpty()) ? "N/A" : dateTime.ToString();
        }

    }
}
