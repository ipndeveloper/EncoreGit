using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NetSteps.Common.Interfaces;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;

namespace NetSteps.Web.Mvc.Extensions
{
    public enum LinkSelectionType
    {
        ActualPage,
        ActualPageAndQueryString,
        ActualPageWithPossibleID,
        SubDirectory
    }

    public static class LinkExtensions
    {
        public static string SelectedLink(this HtmlHelper helper, string url, string linkText, string controllerName, IUser user, string function = "", bool createLi = true, bool hideCompletely = true)
        {
            return helper.SelectedLink(url, linkText, LinkSelectionType.SubDirectory, controllerName, user, function);
        }

        public static string SelectedLink(this HtmlHelper helper, string url, string linkText, LinkSelectionType type, IUser user, string function = "", bool createLi = true, bool hideCompletely = true)
        {
            return helper.SelectedLink(url, linkText, type, string.Empty, user, function);
        }

        public static string SelectedLink(this HtmlHelper helper, string url, string linkText, List<string> mappedUrls, LinkSelectionType type, IUser user, string function = "", bool createLi = true, bool hideCompletely = true)
        {
            return helper.SelectedLink(url, linkText, mappedUrls, type, string.Empty, user, function);
        }

        public static string SelectedLink(this HtmlHelper helper, string url, string linkText, LinkSelectionType type, string controllerName, IUser user, string function = "", bool createLi = true, bool hideCompletely = true)
        {
            return helper.SelectedLink(url, linkText, null, type, controllerName, user, function);
        }

        public static string SelectedLink(this HtmlHelper helper, string url, string linkText, List<string> mappedUrls, LinkSelectionType type, string controllerName, IUser user, string function = "", bool createLi = true, bool hideCompletely = true)
        {
            if (!string.IsNullOrEmpty(function) && SmallCollectionCache.Instance.Functions.Count(f => f.Name.ToUpper() == function.ToUpper()) == 0)
            {
                Function.CreateFunction(function);
                //user.RevertFunctions();
            }
            bool disabled = true;
            string cssClass = string.Empty;
            if (string.IsNullOrEmpty(function) || user.HasFunction(function)) //|| user.RoleId == SmallCollectionCache.Roles.First(r => r.Name.ToLower() == "administrator").RoleId)
            {
                disabled = false;
            }
            // Parse out the query string to get the correct relative path and then add it back on - JHE
            string queryString = string.Empty;

            if (mappedUrls == null)
                mappedUrls = new List<string>();

            mappedUrls.Add(url);

            for (int i = 0; i < mappedUrls.Count; i++)
            {
                if (mappedUrls[i].Contains('?'))
                {
                    queryString = mappedUrls[i].Substring(mappedUrls[i].IndexOf('?'));
                    mappedUrls[i] = mappedUrls[i].Replace(queryString, string.Empty);
                }

                if (mappedUrls[i].StartsWith("~"))
                    mappedUrls[i] = VirtualPathUtility.ToAbsolute(mappedUrls[i]);

                mappedUrls[i] += queryString;
            }

            url = mappedUrls[mappedUrls.Count - 1];

            switch (type)
            {
                case LinkSelectionType.ActualPageAndQueryString:
                    foreach (string mappedUrl in mappedUrls)
                        if (Helpers.BaseContext.PageUrlWithQueryString == mappedUrl)
                            cssClass = "selected";
                    break;
                case LinkSelectionType.ActualPage:
                    foreach (string mappedUrl in mappedUrls)
                        if (Helpers.BaseContext.PageUrl == mappedUrl)
                            cssClass = "selected";
                    break;
                case LinkSelectionType.ActualPageWithPossibleID:
                    foreach (string mappedUrl in mappedUrls)
                        if (Helpers.BaseContext.PageUrl.StartsWith(mappedUrl))
                            cssClass = "selected";
                    break;
                case LinkSelectionType.SubDirectory:
                    //cssClass = Helpers.BaseContext.PageUrl.ToLower().Contains(string.Format("/{0}/", directoryName.ToLower())) ? "selected" : string.Empty;
                    cssClass = helper.ViewContext.RouteData.Values["controller"].ToString() == controllerName ? "selected" : string.Empty;
                    break;
            }
            if (disabled)
            {
                if (hideCompletely)
                    return string.Empty;
                return (createLi ? "<li>" : string.Empty) + string.Format("<a href=\"javascript:void(0);\" style=\"cursor:default;\"><span>{3}</span></a>", url, string.IsNullOrEmpty(cssClass) ? string.Empty : string.Format(" class=\"{0}\"", cssClass), disabled ? " disabled=\"disabled\"" : string.Empty, linkText) + (createLi ? "</li>" : string.Empty);
            }
            else
                return (createLi ? "<li>" : string.Empty) + string.Format("<a href=\"{0}\"{1}><span>{2}</span></a>", url, string.IsNullOrEmpty(cssClass) ? string.Empty : string.Format(" class=\"{0}\"", cssClass), linkText) + (createLi ? "</li>" : string.Empty);
        }

    }
}
