using System.IO;
using System.Text;
using System.Web.UI;

namespace NetSteps.Web.Extensions
{
    /// <summary>
    /// Author: John Egbert
    /// Description: Control Extensions
    /// Created: 06-09-2009
    /// </summary>
    public static class ControlExtensions
    {
        public static string ToHtml(this Control control)
        {
            StringBuilder sb = new StringBuilder();
            control.RenderControl(new HtmlTextWriter(new StringWriter(sb)));
            return sb.ToString();
        }
    }
}
