using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace NetSteps.Web.Mvc.Extensions
{
    public static class UrlExtensions
    {
        public static bool IsCurrent(this UrlHelper urlHelper, String areaName)
        {
            return urlHelper.IsCurrent(areaName, null, null);
        }
 
        public static bool IsCurrent(this UrlHelper urlHelper, String areaName, String controllerName)
        {
            return urlHelper.IsCurrent(areaName, controllerName, null);
        }
 
        public static bool IsCurrent(this UrlHelper urlHelper, String areaName, String controllerName, params String[] actionNames)
        {
            return urlHelper.RequestContext.IsCurrentRoute(areaName, controllerName, actionNames);
        }
 
        public static string Selected(this UrlHelper urlHelper, String areaName)
        {
            return urlHelper.Selected(areaName, null, null);
        }
 
        public static string Selected(this UrlHelper urlHelper, String areaName, String controllerName)
        {
            return urlHelper.Selected(areaName, controllerName, null);
        }
 
        public static string Selected(this UrlHelper urlHelper, String areaName, String controllerName, params String[] actionNames)
        {
            return urlHelper.IsCurrent(areaName, controllerName, actionNames) ? "selected" : String.Empty;
        }
    }
 
    public static class HtmlExtensions
    {
        public static MvcHtmlString ActionMenuItem(this HtmlHelper htmlHelper, String linkText, String actionName, String controllerName)
        {
            var tag = new TagBuilder("li");
 
            if ( htmlHelper.ViewContext.RequestContext.IsCurrentRoute(null, controllerName, actionName) )
            {
                tag.AddCssClass("selected");
            }
 
            tag.InnerHtml = htmlHelper.ActionLink(linkText, actionName, controllerName).ToString();
 
            return MvcHtmlString.Create(tag.ToString());
        }
    }
 
    public static class RequestExtensions
    {
        public static bool IsCurrentRoute(this RequestContext context, String areaName)
        {
            return context.IsCurrentRoute(areaName, null, null);
        }
 
        public static bool IsCurrentRoute(this RequestContext context, String areaName, String controllerName)
        {
            return context.IsCurrentRoute(areaName, controllerName, null);
        }
 
        public static bool IsCurrentRoute(this RequestContext context, String areaName, String controllerName, params String[] actionNames)
        {
            var routeData = context.RouteData;
            var routeArea = routeData.DataTokens["area"] as String;
            var current = false;
 
            if ( ((String.IsNullOrEmpty(routeArea) && String.IsNullOrEmpty(areaName)) || (routeArea == areaName)) &&
                 ((String.IsNullOrEmpty(controllerName)) || (routeData.GetRequiredString("controller") == controllerName)) &&
                 ((actionNames == null) || actionNames.Contains(routeData.GetRequiredString("action"))) )
            {
                current = true;
            }
 
            return current;
        }
    }
}
