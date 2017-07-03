using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Validation;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using System.Web.WebPages;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Promotions.UI.Common.Interfaces;
using NetSteps.Web.Mvc.Extensions;
using NetSteps.Web.Mvc.Helpers;



namespace DistributorBackOffice.Extensions
{
    public enum ActionItems
    {
        Overview,
        Security,
        Monthly,
        PWS,
        International
    }

    public static class HtmlHelperExtensions
    {
        public static HtmlString Tab(this HtmlHelper helper, string url, string linkText, bool isArea, string cssClass = "", string function = "")
        {
            if (!string.IsNullOrEmpty(function) && SmallCollectionCache.Instance.Functions.Count(f => f.Name.ToUpper() == function.ToUpper()) == 0)
            {
                Function.CreateFunction(function);
            }
            bool hasFunction = true;
            if (!string.IsNullOrEmpty(function) && !CoreContext.CurrentAccount.User.HasFunction(function, false, true))
            {
                hasFunction = false;
            }

            StringBuilder builder = new StringBuilder();
            RouteData routeData = helper.FakeRoute(ref url);
            builder.Append("<li");
            if (isArea)
            {
                if (routeData.DataTokens.ContainsKey("area") && routeData.DataTokens["area"].Equals(helper.ViewContext.RouteData.DataTokens["area"]))
                    builder.Append(" class=\"current\"");
            }
            else
            {
                if (routeData.Values["controller"].Equals(helper.ViewContext.RouteData.Values["controller"]))
                    builder.Append(" class=\"current\"");
            }
            builder.Append("><a href=\"").Append(url).Append(!string.IsNullOrEmpty(cssClass) ? "\" class=\"" + cssClass : "").Append("\"><span class=\"lbl\">").Append(linkText).Append("</span></a></li>");

            if (hasFunction)
                return new HtmlString(builder.ToString());
            else
                return new HtmlString(string.Empty);
        }

        public static string SubTab(this HtmlHelper helper, string url, string linkText, bool onlyCheckController = false, string requestParamKey = null, object requestParamValue = null, string function = "")
        {
            StringBuilder builder = new StringBuilder();

            //if (CheckForFunction(function))
            //{
            //    RouteData routeData = helper.FakeRoute(ref url);
            //    builder.Append("<li");
            //    if ((routeData.Values["action"].Equals(helper.ViewContext.RouteData.Values["action"]) && routeData.Values["controller"].Equals(helper.ViewContext.RouteData.Values["controller"]))
            //        || (onlyCheckController && routeData.Values["controller"].Equals(helper.ViewContext.RouteData.Values["controller"])))
            //    {
            //        NameValueCollection currentRequestParams = helper.ViewContext.RequestContext.HttpContext.Request.Params;
            //        if (string.IsNullOrEmpty(requestParamKey) || (currentRequestParams.AllKeys.Contains(requestParamKey) && currentRequestParams[requestParamKey].Equals(requestParamValue)))
            //        {
            //            builder.Append(" class=\"selected\"");
            //        }
            //    }
            //    
            //}
            builder.Append("><a href=\"").Append(url).Append("\"><span>").Append(linkText).Append("</span></a></li>");
            return builder.ToString();
        }

        
        public static HtmlString DropDownNavigation(this HtmlHelper helper, string linkText, string url, IEnumerable<NavigationItem> subItems, bool? overrideSelected = null)
		{
			return helper.DropDownNavigation(linkText, url, subItems, null, overrideSelected);
		}

        public static HtmlString DropDownNavigation(this HtmlHelper helper, string linkText, string url, IEnumerable<NavigationItem> subItems, IEnumerable<NavigationItem> utilities, bool? overrideSelected = null)
        {
            bool childItemSelected = overrideSelected.HasValue ? overrideSelected.Value : false;
            NameValueCollection currentRequestParams = helper.ViewContext.RequestContext.HttpContext.Request.Params;
            if (!overrideSelected.HasValue)
            {
                foreach (NavigationItem item in subItems)
                {
                    //string url = item.Url;
                    item.MappedUrls.Add(item.Url);
                    foreach (string mappedUrl in item.MappedUrls)
                    {
                        string u = mappedUrl;
                        //To allow for full controller selection (i.e. /Products/Catalogs/* defines all actions of the catalogs controller)
                        if (mappedUrl.EndsWith("*"))
                        {
                            u = Regex.Replace(u, @"\*$", "");
                            RouteData routeData = helper.FakeRoute(ref u);
                            if ((!routeData.DataTokens.ContainsKey("area") || routeData.DataTokens["area"].Equals(helper.ViewContext.RouteData.DataTokens["area"]))
                                && routeData.Values["controller"].Equals(helper.ViewContext.RouteData.Values["controller"]))
                            {
                                if (!string.IsNullOrEmpty(item.RequestParamKey))
                                {
                                    if (currentRequestParams.AllKeys.Contains(item.RequestParamKey) && currentRequestParams[item.RequestParamKey].Equals(item.RequestParamValue))
                                    {
                                        childItemSelected = true;
                                        break;
                                    }
                                }
                                else
                                {
                                    childItemSelected = true;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            RouteData routeData = helper.FakeRoute(ref u);
                            if ((!routeData.DataTokens.ContainsKey("area") || routeData.DataTokens["area"].Equals(helper.ViewContext.RouteData.DataTokens["area"]))
                                && routeData.Values["controller"].Equals(helper.ViewContext.RouteData.Values["controller"])
                                && routeData.Values["action"].Equals(helper.ViewContext.RouteData.Values["action"].ToString()))
                            {
                                if (!string.IsNullOrEmpty(item.RequestParamKey))
                                {
                                    if (currentRequestParams.AllKeys.Contains(item.RequestParamKey) && currentRequestParams[item.RequestParamKey].Equals(item.RequestParamValue))
                                    {
                                        childItemSelected = true;
                                        break;
                                    }
                                }
                                else
                                {
                                    childItemSelected = true;
                                    break;
                                }
                            }
                        }
                    }
                    if (childItemSelected)
                        break;
                }
            }

            StringBuilder builder = new StringBuilder();
            //Only build the drop down if there are any items
            if (subItems.Count() > 0 && (subItems.Any(i => string.IsNullOrEmpty(i.Function) || CoreContext.CurrentUser.HasFunction(i.Function))))
            {
                builder.Append("<li class=\"Tab");
                if (childItemSelected)
                {
                    builder.Append(" selected");
                }
                builder.Append("\"><a href=\"").Append(string.IsNullOrEmpty(url) ? "javascript:void(0);\" style=\"cursor:default;" : VirtualPathUtility.ToAbsolute(url)).Append("\"><span>").Append(linkText).Append("</span></a><div class=\"DropDown\"><ul>");
                //Only build the links they have access to
                foreach (NavigationItem item in subItems.Where(i => string.IsNullOrEmpty(i.Function) || CoreContext.CurrentUser.HasFunction(i.Function)))
                {
                    builder.Append("<li><a href=\"").Append(VirtualPathUtility.ToAbsolute(item.Url)).Append("\"><span>").Append(item.LinkText).Append("</span></a></li>");
                }
                builder.Append("</ul>");
                //Add the utilities on the bottom
                if (utilities != null && utilities.Count() > 0)
                {
                    builder.Append("<div class=\"DropDownUtility\">");
                    foreach (NavigationItem item in utilities.Where(u => string.IsNullOrEmpty(u.Function) || CoreContext.CurrentUser.HasFunction(u.Function)))
                    {
                        builder.Append("<li><a href=\"").Append(VirtualPathUtility.ToAbsolute(item.Url)).Append("\"><span>").Append(item.LinkText).Append("</span></a></li>");
                    }
                    builder.Append("</div>");
                }
                builder.Append("<span class=\"ClearAll\"></span></div></li>");
            }
            return new HtmlString(builder.ToString());
        }

        public static HtmlString ActionItem(this HtmlHelper helper, string url, string text, ActionItems itemType)
        {
            return helper.ActionItem(url, text, itemType.ToString().ToLower());
        }
        public static HtmlString ActionItem(this HtmlHelper helper, string url, string text, string cssClass, string varyCurrentByParamName = null, string function = "")
        {
            if (!string.IsNullOrEmpty(function) && SmallCollectionCache.Instance.Functions.Count(f => f.Name.ToUpper() == function.ToUpper()) == 0)
            {
                Function.CreateFunction(function);
            }
            bool hasFunction = true;
            if (!string.IsNullOrEmpty(function) && !CoreContext.CurrentAccount.User.HasFunction(function, false, true))
            {
                hasFunction = false;
            }

            RouteData routeData = helper.FakeRoute(ref url);

            TagBuilder outerLi = new TagBuilder("li");


            if (routeData != null && routeData.Values["controller"].Equals(helper.ViewContext.RouteData.Values["controller"]) && routeData.Values["action"].Equals(helper.ViewContext.RouteData.Values["action"]))
            {
                if (!varyCurrentByParamName.IsNullOrEmpty())
                {
                    HttpRequestBase currentRequest = helper.ViewContext.RequestContext.HttpContext.Request;
                    if (currentRequest.QueryString.AllKeys.Contains(varyCurrentByParamName) && url.ContainsIgnoreCase(string.Format("{0}={1}", varyCurrentByParamName, currentRequest.QueryString[varyCurrentByParamName])))
                        outerLi.AddCssClass("current");
                }
                else
                    outerLi.AddCssClass("current");
            }

            TagBuilder aBuilder = new TagBuilder("a");
            aBuilder.MergeAttribute("href", url);
            aBuilder.AddCssClass("UI-icon-container");

            TagBuilder emptySpan = new TagBuilder("span");
            //emptySpan.AddCssClass(cssClass);
            if (!string.IsNullOrEmpty(cssClass))
            {
                foreach (var c in cssClass.Split(' '))
                    emptySpan.AddCssClass(c);
            }
            emptySpan.AddCssClass("UI-icon");


            TagBuilder textSpan = new TagBuilder("span");
            textSpan.InnerHtml = text;
            textSpan.AddCssClass("lbl");

            aBuilder.InnerHtml = emptySpan.ToString(TagRenderMode.Normal) + textSpan.ToString();

            outerLi.InnerHtml = aBuilder.ToString();

            if (hasFunction)
                return new HtmlString(outerLi.ToString());
            else
                return new HtmlString(string.Empty);
        }

        public static HelperResult WrapPromotion<T>(this HtmlHelper helper, T model, Func<dynamic, HelperResult> template) where T : IDisplayInfo
        {
            var result = helper.Partial("~/Views/Promotions/_promotionDescription.cshtml", model);
            bool showDiv = (model.ImagePaths != null && model.ImagePaths.Any());
            if (showDiv)
            {
                return new HelperResult(writer => template(result).WriteTo(writer));
            }
            return new HelperResult(writer => writer.Write(result));
        }

        public static HelperResult List<T>(this IEnumerable<T> items, Func<T, HelperResult> template)
        {
            if (items == null)
            {
                return new HelperResult(writer => writer.Write(string.Empty));
            }

            return new HelperResult(writer =>
            {
                foreach (var item in items)
                {
                    if (item is string && string.IsNullOrEmpty(item.ToString().RemoveExtraWhiteSpace()))
                    {
                        writer.Write(string.Empty);
                    }
                    else
                    {
                        template(item).WriteTo(writer);
                    }
                }
            });
        }
     }
    public class NavigationItem
    {
        public string Url, LinkText, RequestParamKey, Function;
        public object RequestParamValue;

        public List<string> MappedUrls = new List<string>();
    }
}