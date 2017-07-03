using System.Web.UI.WebControls;

namespace NetSteps.Web.Extensions
{
    /// <summary>
    /// Author: John Egbert
    /// Description: LinkButton Extensions
    /// Created: 03-18-2009
    /// </summary>
    public static class LinkButtonExtensions
    {
        public static void AddJavascriptConfirm(this LinkButton value)
        {
            AddJavascriptConfirm(value, string.Empty);
        }
        public static void AddJavascriptConfirm(this LinkButton value, string validationQuestion)
        {
            string message = (!string.IsNullOrEmpty(validationQuestion)) ? validationQuestion.Trim() : "Are you sure?";
            value.Attributes.Add("onclick", string.Format("return confirm('{0}')", message));
        }
    }
}
