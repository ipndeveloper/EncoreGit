using System.Web.UI.WebControls;

namespace NetSteps.Web.Extensions
{
    /// <summary>
    /// Author: John Egbert
    /// Description: ImageButton Extensions
    /// Created: 03-18-209
    /// </summary>
    public static class ImageButtonExtensions
    {
        public static void AddJavascriptConfirm(this ImageButton value)
        {
            AddJavascriptConfirm(value, string.Empty);
        }
        public static void AddJavascriptConfirm(this ImageButton value, string validationQuestion)
        {
            string message = (!string.IsNullOrEmpty(validationQuestion)) ? validationQuestion.Trim() : "Are you sure?";
            value.Attributes.Add("onclick", string.Format("return confirm('{0}')", message));
        }
    }
}
