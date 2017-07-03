using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NetSteps.Common.Validation;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Web.Mvc.Extensions;
using NetSteps.Web.Mvc.Helpers;

namespace nsCore.Extensions
{
	public static class HtmlHelperExtensions
	{
		public static string Tab(this HtmlHelper helper, string url, string linkText, string function = "", IEnumerable<string> mappedAreas = null)
		{
			StringBuilder builder = new StringBuilder();
			if (CheckForFunction(function))
			{
				RouteData routeData = helper.FakeRoute(ref url);
				builder.Append("<li><a href=\"").Append(url).Append("\"");
				if (routeData.DataTokens.ContainsKey("area") && (routeData.DataTokens["area"].Equals(helper.ViewContext.RouteData.DataTokens["area"]) || (mappedAreas != null && mappedAreas.Any(a => helper.ViewContext.RouteData.DataTokens["area"].Equals(a)))))
					builder.Append(" class=\"selected\"");
				builder.Append("><span>").Append(linkText).Append("</span></a></li>");
			}
			return builder.ToString();
		}

		public static string DropDownNavigation(this HtmlHelper helper, string linkText, string url, IEnumerable<NavigationItem> subItems, bool? overrideSelected = null)
		{
			return helper.DropDownNavigation(linkText, url, subItems, null, overrideSelected);
		}

		public static string DropDownNavigation(this HtmlHelper helper, string linkText, string url, IEnumerable<NavigationItem> subItems, IEnumerable<NavigationItem> utilities, bool? overrideSelected = null)
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
				builder.Append("<li class=\"SubTab");
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
			return builder.ToString();
		}

		public static string SubTab(this HtmlHelper helper, string url, string linkText, bool onlyCheckController = false, string requestParamKey = null, object requestParamValue = null, string function = "")
		{
			StringBuilder builder = new StringBuilder();

			if (CheckForFunction(function))
			{
				RouteData routeData = helper.FakeRoute(ref url);
				builder.Append("<li");
				if ((routeData.Values["action"].Equals(helper.ViewContext.RouteData.Values["action"]) && routeData.Values["controller"].Equals(helper.ViewContext.RouteData.Values["controller"]))
					|| (onlyCheckController && routeData.Values["controller"].Equals(helper.ViewContext.RouteData.Values["controller"])))
				{
					NameValueCollection currentRequestParams = helper.ViewContext.RequestContext.HttpContext.Request.Params;
					if (string.IsNullOrEmpty(requestParamKey) || (currentRequestParams.AllKeys.Contains(requestParamKey) && currentRequestParams[requestParamKey].Equals(requestParamValue)))
					{
						builder.Append(" class=\"selected\"");
					}
				}
				builder.Append("><a href=\"").Append(url).Append("\"><span>").Append(linkText).Append("</span></a></li>");
			}
			return builder.ToString();
		}

		private static bool CheckForFunction(string function)
		{
			if (!string.IsNullOrEmpty(function) && SmallCollectionCache.Instance.Functions.Count(f => f.Name.ToUpper() == function.ToUpper()) == 0)
			{
				Function.CreateFunction(function);
				//user.RevertFunctions();
			}
			return string.IsNullOrEmpty(function) || CoreContext.CurrentUser.HasFunction(function, false);
		}
				
		public static string ValidationRules(this HtmlHelper helper, object model)
		{
			//TODO: finish this validation creation - DES
			var properties = NetSteps.Common.Validation.DataAnnotationHelpers.GetPropertiesWithDataAnnotationAttributes(model);
			StringBuilder builder = new StringBuilder();
			foreach (var property in properties)
			{
				foreach (ValidationAttribute attr in property.Value)
				{
					if (attr is RegexAttribute)
					{
					}
					else if (attr is StringRangeAttribute)
					{
					}
				}
			}
			return builder.ToString();
		}
	}

	public class NavigationItem
	{
		public string Url, LinkText, RequestParamKey, Function;
		public object RequestParamValue;

		public List<string> MappedUrls = new List<string>();
	}
}