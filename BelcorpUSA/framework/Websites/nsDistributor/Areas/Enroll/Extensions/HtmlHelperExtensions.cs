using System.Web.Mvc;
using System.Web.Mvc.Html;
using NetSteps.Common.Globalization;
using NetSteps.Web.Mvc.Extensions;

namespace nsDistributor.Areas.Enroll.Extensions
{
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// A custom edit link used on the enrollment review page.
        /// </summary>
        public static MvcHtmlString EditActionLink(this HtmlHelper helper, string actionName, string controllerName)
        {
            string tooltip = Translation.GetTerm("MakeChanges", "Make Changes");
            string linkClasses = "FR UI-icon-container";
            string spanClasses = "UI-icon icon-edit";
            string spanContent = Translation.GetTerm("Edit", "Edit");

            // span element goes inside the link
            var spanBuilder = new TagBuilder("span");
            spanBuilder.MergeAttribute("class", spanClasses);
            spanBuilder.SetInnerText(spanContent);

            // Build the link with the span inside
            var linkHtml = helper
                .ActionLink("[placeholder]", actionName, controllerName,
                    new { ReturnUrl = helper.ViewContext.HttpContext.Request.Path },
                    new { title = tooltip, @class = linkClasses })
                .ToString()
                .Replace("[placeholder]", spanBuilder.ToString())
                .ToMvcHtmlString();

            return linkHtml;
        }
    }
}