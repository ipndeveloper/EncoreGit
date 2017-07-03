using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Web.Mvc.Controls;
using NetSteps.Web.Mvc.Extensions;
using nsDistributor.Controllers;

namespace nsDistributor.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static HtmlString CreateMenu(this HtmlHelper helper, Page page, out string header)
        {
            var baseSite = BaseController.CurrentSite.IsBase ? BaseController.CurrentSite : SiteCache.GetSiteByID(BaseController.CurrentSite.BaseSiteID.ToInt());
            var parentLink = baseSite.Navigations.FirstOrDefault(n => n.PageID == page.PageID && n.NavigationTypeID == Constants.NavigationType.Header.ToInt());
            if (parentLink != null)
            {
                header = parentLink.GetLinkText(BaseController.CurrentLanguageID);
                IEnumerable<Navigation> navigation;
                bool isSecondaryNav = true;
                if (parentLink.ParentID.HasValue)
                {
                    isSecondaryNav = baseSite.Navigations.Where(n => n.NavigationID == parentLink.ParentID.Value).Any(n => n.IsSecondaryNavigation);
                    navigation = baseSite.Navigations.Where(n => n.ParentID == parentLink.ParentID.Value);
                }
                else
                {
                    isSecondaryNav = parentLink.IsSecondaryNavigation;
					navigation = baseSite.Navigations.Where(n => n.ParentID == parentLink.NavigationID);
                }
                if (isSecondaryNav)
                    return helper.Navigation(navigation, false, parentLink.ParentNavigation != null ? parentLink.ParentNavigation.NavigationID : parentLink.NavigationID);
            }

            header = "";
            return new HtmlString("");
        }


        public static HtmlString ActionItem(this HtmlHelper helper, string url, string text, string cssClass, string varyCurrentByParamName = null, string function = "")
        {
            if (!string.IsNullOrEmpty(function) && SmallCollectionCache.Instance.Functions.Count(f => f.Name.ToUpper() == function.ToUpper()) == 0)
            {
                Function.CreateFunction(function);
            }
            bool hasFunction = true;
            if (!string.IsNullOrEmpty(function) && !CoreContext.CurrentAccount.User.HasFunction(function))
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
            aBuilder.AddCssClass(cssClass);

            TagBuilder textSpan = new TagBuilder("span");
            textSpan.InnerHtml = text;

            aBuilder.InnerHtml = textSpan.ToString();

            outerLi.InnerHtml = aBuilder.ToString();

            if (hasFunction)
                return new HtmlString(outerLi.ToString());
            else
                return new HtmlString(string.Empty);
        }

    }
}