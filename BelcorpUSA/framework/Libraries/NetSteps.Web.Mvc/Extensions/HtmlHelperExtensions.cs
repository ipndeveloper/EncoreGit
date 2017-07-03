using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities.Business.HelperObjects.OrderPackages;
using NetSteps.Web.Extensions;

namespace NetSteps.Web.Mvc.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString ActionLinkSelected(this HtmlHelper htmlHelper, string linkText, string actionName, object routeValues)
        {
            var currentOutput = htmlHelper.ActionLink(linkText, actionName, routeValues);
            string returnValue = currentOutput.ToHtmlString();
            if (WebContext.PageUrlWithQueryString == returnValue.GetHtmlAttributeValue("href"))
            {
                returnValue = returnValue.SetHtmlAttributeValue("class", "selected");
            }

            return MvcHtmlString.Create(returnValue);
        }

        public static MvcForm BeginFormWithHtmlAttributes(this HtmlHelper htmlHelper, object htmlAttributes)
        {
            return htmlHelper.BeginForm(null, null, FormMethod.Post, htmlAttributes);
        }

		public static MvcForm BeginFormWithHtmlAttributes(this HtmlHelper htmlHelper, object routeValues, object htmlAttributes)
		{
			return htmlHelper.BeginForm(null, null, routeValues, FormMethod.Post, htmlAttributes);
		}

        public static MvcHtmlString Label(this HtmlHelper html, string expression, string termName, string defaultValue)
        {
            return html.Label(expression, Translation.GetTerm(termName, defaultValue));
        }

        public static MvcHtmlString LabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string termName, string defaultValue)
        {
            return html.LabelFor(expression, Translation.GetTerm(termName, defaultValue));
        }


        /// <summary>
        /// Creates an 'a' tag with the specified values.
        /// Default: sets href to javascript:void(0); and term translates the text.
        /// </summary>
        public static MvcHtmlString Link(this HtmlHelper htmlHelper, string text, bool termTranslate = true, string href = "", string cssClasses = "", string id = "")
        {
            string url = !String.IsNullOrEmpty(href) ? href : "javascript:void(0);";

            string termTranslatedText = Translate(text, termTranslate);

            var tag = new TagBuilder("a");
            tag.MergeAttribute("id", id);
            tag.MergeAttribute("href", url);
            tag.SetInnerText(termTranslatedText);
            tag.AddCssClass(cssClasses);

            return MvcHtmlString.Create(tag.ToString());
        }

		public static MvcHtmlString Link(this HtmlHelper htmlHelper,
			string linkText,
			string href,
			IDictionary<string, object> htmlAttributes = null)
		{
			var tag = new TagBuilder("a")
			{
				InnerHtml = (!String.IsNullOrEmpty(linkText)) ? HttpUtility.HtmlEncode(linkText) : String.Empty
			};
			if (htmlAttributes != null)
			{
				tag.MergeAttributes(htmlAttributes);
			}
			tag.MergeAttribute("href", href);
			return MvcHtmlString.Create(tag.ToString());
		}

        /// <summary>
        /// Creates an 'a' tag with the specified values.
        /// Default: sets href to javascript:void(0); and term translates the text.
        /// </summary>
        public static MvcHtmlString LinkWithSpan(this HtmlHelper htmlHelper, string text, bool termTranslate = true, string href = "", string cssClasses = "", string id = "")
        {
            string url = !String.IsNullOrEmpty(href) ? href : "javascript:void(0);";

            string termTranslatedText = Translate(text, termTranslate);

            TagBuilder span = new TagBuilder("span");
            span.SetInnerText(termTranslatedText);

            TagBuilder tag = new TagBuilder("a");
            tag.MergeAttribute("id", id);
            tag.MergeAttribute("href", url);
            tag.AddCssClass(cssClasses);

            tag.InnerHtml = span.ToString();

            return MvcHtmlString.Create(tag.ToString());
        }

        public static MvcHtmlString TrackingNumberUrlLink(this HtmlHelper htmlHelper, OrderPackageInfoModel package)
        {
            string href = !String.IsNullOrWhiteSpace(package.TrackingUrl)
                              ? package.TrackingUrl
                              : string.Format(package.BaseTrackUrl, package.TrackingNumber);

            TagBuilder tag = new TagBuilder("a") { InnerHtml = package.TrackingNumber };
            tag.MergeAttribute("target", "_blank");
            tag.MergeAttribute("rel", "external");
            tag.MergeAttribute("href", href);

            return MvcHtmlString.Create(tag.ToString());
        }


        #region ActionLinks Term Translated

        private static string Translate(string term, bool termTranslate = true)
        {
            string termTranslatedText = termTranslate
                                                                            ? Translation.GetTerm(term.Replace(" ", ""), term)
                                                                            : term;
            return termTranslatedText;
        }

        public static MvcHtmlString NSActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, object routeValues, object htmlAttributes)
        {
            return htmlHelper.ActionLink(Translate(linkText), actionName, controllerName, routeValues, htmlAttributes);
        }

        public static MvcHtmlString NSActionLink(this HtmlHelper htmlHelper, string linkText, string actionName)
        {
            return htmlHelper.ActionLink(Translate(linkText), actionName);
        }

        public static MvcHtmlString NSActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, object routeValues)
        {
            return htmlHelper.ActionLink(Translate(linkText), actionName, routeValues);
        }

        public static MvcHtmlString NSActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, object routeValues, object htmlAttributes)
        {
            return htmlHelper.ActionLink(Translate(linkText), actionName, routeValues, htmlAttributes);
        }


        public static MvcHtmlString NSActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName)
        {
            return htmlHelper.ActionLink(Translate(linkText), actionName, controllerName);
        }

        #endregion

        private static ConcurrentDictionary<string, RouteData> __fakeRoutes = new ConcurrentDictionary<string, RouteData>();

        /// <summary>
        /// Gets route data for a URL. The result is cached and virtual URLs are transformed 
        /// to their concrete (runtime) value.
        /// </summary>
        /// <param name="helper">the html helper</param>
        /// <param name="url">the url</param>
        /// <returns></returns>
        public static RouteData FakeRoute(this HtmlHelper helper, ref string url)
        {
            Contract.Requires<ArgumentNullException>(helper != null);
            Contract.Requires<ArgumentNullException>(url != null);
            Contract.Requires<ArgumentException>(url.Length > 0);

            if (url.StartsWith("javascript:"))
                return null;

            if (url == "#")
                return null;

            HttpRequestBase currentRequest = helper.ViewContext.RequestContext.HttpContext.Request;

            if (url.StartsWith("~"))
                url = VirtualPathUtility.ToAbsolute(url);
            if (!url.StartsWith(currentRequest.ApplicationPath))
                url = currentRequest.ApplicationPath + url;

            var localUrl = url;
            var result = __fakeRoutes.GetOrAdd(localUrl,
                u =>
                {
                    HttpRequest request = new HttpRequest(VirtualPathUtility.GetFileName(localUrl), "http://" + currentRequest.Url.Authority + localUrl, currentRequest.QueryString.ToString());
                    HttpResponse response = new HttpResponse(new StringWriter());
                    HttpContext context = new HttpContext(request, response);
                    HttpContextWrapper wrapper = new HttpContextWrapper(context);
                    return RouteTable.Routes.GetRouteData(wrapper);
                });
            return result;
        }

        /// <summary>
        /// Gets the HTML field prefix (if any) for the current context and appends an underscore for convenience.
        /// </summary>
        public static MvcHtmlString HtmlFieldPrefix(this HtmlHelper helper)
        {
            Contract.Requires<ArgumentNullException>(helper != null);

            var prefix = helper.ViewData.TemplateInfo.HtmlFieldPrefix;
            if (!string.IsNullOrEmpty(prefix))
            {
                prefix = prefix + "_";
            }

            return new MvcHtmlString(prefix);
        }

		/// <summary>
		/// Converts a boolean value to a lowercase string for use in JavaScript.
		/// </summary>
		public static MvcHtmlString ToJSBoolean(this bool value)
		{
			return new MvcHtmlString(value.ToString().ToLower());
		}
    }
}
