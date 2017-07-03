using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Web.Mvc.Attributes;

namespace nsCore.Areas.Sites.Controllers
{
    public class SiteMapController : BaseSitesController
    {
        protected virtual int NavigationTypeId
        {
            get
            {
                if (Session["NavigationTypeId"] == null)
                {
                    var navigationTypes = CurrentSite.SiteType.NavigationTypes;
                    var navigationType = navigationTypes.OrderBy(t => t.NavigationTypeID).FirstOrDefault();
                    if (navigationType != null)
                        Session["NavigationTypeId"] = navigationType.NavigationTypeID;
                    else
                    {
                        throw new Exception("NavigationType not found for SiteTypeID: " + CurrentSite.SiteTypeID);
                    }
                }
                return (int)Session["NavigationTypeId"];
            }
            set { Session["NavigationTypeId"] = value; }
        }

        /// <summary>
        /// Show a site map of all of the navigation
        /// </summary>
        /// <param name="id">The id of the current site</param>
        /// <returns></returns>
        [FunctionFilter("Sites-Site Map", "~/Sites/Overview")]
        public virtual ActionResult Index(int? id, int? navigationType)
        {
            try
            {
                var currentSite = LoadSiteWithSiteMapData(id);
                CurrentSite = currentSite;

                if (currentSite == null)
                    return RedirectToAction("Index", "Landing");

                if (navigationType.HasValue)
                    NavigationTypeId = navigationType.Value;
                else
                    Session["NavigationTypeId"] = null;
                ViewData["Pages"] = currentSite.Pages.Where(p => p.Active);
                StringBuilder builder = new StringBuilder();
                BuildNavigationTreeRecursively(null, builder);
                ViewData["Links"] = builder.ToString();
                ViewData["NavigationTypes"] = currentSite.SiteType.NavigationTypes.ToList();

                return View();
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        /// <summary>
        /// Build out the navigation tree as a ul > li
        /// </summary>
        /// <param name="parent">The parent link</param>
        /// <param name="builder">The builder to append to</param>
        protected virtual void BuildNavigationTreeRecursively(Navigation parent, StringBuilder builder)
        {
            builder.Append("<ul>");
            var navigations = (parent == null ? CurrentSite.Navigations.GetByNavigationTypeID(NavigationTypeId).Where(n => n.ParentID == null).ToList() : parent.ChildNavigations.ToList()).OrderBy(n => n.SortIndex).ToList();
            foreach (Navigation link in navigations)
            {
                int currentEditingLanguageID = CurrentSite.GetDefaultEditingLanguageID();
                builder.Append("<li id=\"").Append(link.NavigationID).Append("\"><a class=\"NavNode\" href=\"javascript:void(0);\">")
                    .Append(link.GetLinkText(currentEditingLanguageID)).Append("</a>&nbsp;<a href=\"javascript:void(0);\" class=\"AddChild\" title=\"" + Translation.GetTerm("AddaChildLink", "Add a child link") + "\">+</a>");
                if (link.ChildNavigations.Count > 0)
                {
                    BuildNavigationTreeRecursively(link, builder);
                }
                builder.Append("</li>");
            }
            builder.Append("</ul>");
        }

        /// <summary>
        /// Reorder navigation under a parent so that they are in order
        /// </summary>
        /// <param name="parentId">The id of the parent link</param>
        /// <param name="navigationIds">All of the link ids on that level in order</param>
        [FunctionFilter("Sites-Site Map", "~/Sites/Overview")]
        public virtual void Move(int parentId, List<int> navigationIds)
        {
            try
            {
                for (int i = 0; i < navigationIds.Count; i++)
                {
                    Navigation link = CurrentSite.Navigations.First(n => n.NavigationID == navigationIds[i]);
                    if (link.SortIndex != i || link.ParentID != parentId)
                    {
						//only start the entity tracking if there is actually going to be a change made.
						link.StartEntityTracking();
                        link.SortIndex = i;
                        link.ParentID = parentId == 0 ? (int?)null : parentId;
                        link.Save();
                    }
                }
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        /// <summary>
        /// Get a single navigation element to display
        /// </summary>
        /// <param name="navigationId">The id of the navigation to display</param>
        /// <returns></returns>
        public virtual ActionResult Get(int navigationId, int languageId)
        {
            try
            {
                Navigation link = Navigation.LoadFull(navigationId);
                return Json(new
                {
                    result = true
                    , isInternal = link.PageID.HasValue && link.PageID.Value > 0
                    , url = link.LinkUrl
                    , linkText = link.GetLinkText(languageId)
                    , pageId = link.PageID
                    , active = link.Active
                    , parentId = link.ParentID
                    , isDropDown = link.IsDropDown
                    , isSecondaryNav = link.IsSecondaryNavigation
                    , isChildNavTree = link.IsChildNavTree
                });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public virtual ActionResult GetTranslation(int? navigationId, int languageId)
        {
            try
            {
                if (!navigationId.HasValue || navigationId.Value == 0)
                    return Json(new { result = true, linkText = "" });

                Navigation link = Navigation.LoadFull(navigationId.Value);
                return Json(new { result = true, linkText = link.GetLinkText(languageId) });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public enum LinkDestination
        {
            ExistingPage = 1,
            ExternalURL = 2
        }

        /// <summary>
        /// Save a navigation link
        /// </summary>
        /// <param name="navigationId">The id of the link</param>
        /// <param name="destination">Whether the link goes to an existing page or to an external url</param>
        /// <param name="linkText">The text of the link</param>
        /// <param name="url">The url of the link</param>
        /// <param name="pageId">The id of the page the link points to</param>
        /// <param name="parentId">The id of the parent of this link</param>
        /// <returns></returns>
        [FunctionFilter("Sites-Site Map", "~/Sites/Overview")]
        [ValidateInput(false)]
        public virtual ActionResult Save(int? navigationId,
            LinkDestination destination, string linkText,
            string url, int? pageId, int? parentId, int languageId, bool isDropDown, bool isSecondaryNav, bool isChildNavTree)
        {
            try
            {
                Navigation link;
                if (navigationId.HasValue && navigationId.Value > 0)
                {
                    link = CurrentSite.Navigations.First(n => n.NavigationID == navigationId);
					link.StartEntityTracking();
                }
                else
                {
                    // Create a link as the last link in the list
                    link = new Navigation();
                    link.StartEntityTracking();
                    link.Active = false;
                    link.NavigationTypeID = NavigationTypeId;

                    CurrentSite.Navigations.Add(link);

                    List<Navigation> siblings = Navigation.LoadSingleLevelNav(CurrentSite.SiteID, NavigationTypeId, parentId);
                    link.SortIndex = (short)(siblings.Count > 0 ? siblings.Max(nl => nl.SortIndex) + 1 : 0);
                }

                var description = link.Translations.GetByLanguageIdOrDefaultInList(languageId);
				description.StartEntityTracking();
                description.LinkText = linkText;

                link.LinkUrl = url;
                link.PageID = destination == LinkDestination.ExistingPage ? pageId : (int?)null;
                link.ParentID = parentId.HasValue && parentId.Value > 0 ? parentId.Value : (int?)null;
                link.IsDropDown = isDropDown;
                link.IsSecondaryNavigation = isSecondaryNav;
                link.IsChildNavTree = isChildNavTree;

                link.StartDate = DateTime.Now;

                CurrentSite.Save();

                return Json(new { result = true, navigationId = link.NavigationID });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        /// <summary>
        /// Change the status of the navigation link
        /// </summary>
        /// <param name="navigationId">The id of the link to change</param>
        /// <param name="isActive">Whether the link is active or not</param>
        [FunctionFilter("Sites-Site Map", "~/Sites/Overview")]
        public virtual ActionResult ChangeStatus(int navigationId, bool isActive)
        {
            try
            {
                //Navigation link = Navigation.Load(navigationId);
                Navigation link = CurrentSite.Navigations.FirstOrDefault(n => n.NavigationID == navigationId);
                if (link.Active != isActive)
                {
                    link.StartEntityTracking();
                    link.Active = isActive;
                    link.Save();
                }
                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [FunctionFilter("Sites-Site Map", "~/Sites/Overview")]
        public virtual ActionResult Delete(int navigationId)
        {
            try
            {
				CurrentSite.StartEntityTracking();
                DeleteNavigationRecursive(navigationId);
                CurrentSite.DateLastModified = DateTime.Now;
                CurrentSite.Save();
                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [NonAction]
        protected virtual void DeleteNavigationRecursive(int navigationId)
        {
            Navigation navigation = CurrentSite.Navigations.FirstOrDefault(n => n.NavigationID == navigationId);
            if (navigation != null)
            {
				navigation.StartEntityTracking();
                foreach (var item in navigation.Translations.ToList())
                    item.MarkAsDeleted();
                foreach (var childNavigationId in navigation.ChildNavigations.Select(n => n.NavigationID).ToList())
                    DeleteNavigationRecursive(childNavigationId);
                if (navigation.ChangeTracker.State != ObjectState.Added)
                    navigation.MarkAsDeleted();
            }
        }
    }
}
