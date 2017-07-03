using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Attributes;
using nsCore.Areas.Sites.Models;

namespace nsCore.Areas.Sites.Controllers
{
    public class PagesController : BaseSitesController
    {
        /// <summary>
        /// Show all of the pages for this site
        /// </summary>
        /// <param name="id">The id of the current site</param>
        /// <returns></returns>
        [FunctionFilter("Sites-Site Pages", "~/Sites/Overview")]
        public virtual ActionResult Index(int? id)
        {
            try
            {
                if (id.HasValue && id.Value > 0)
                    CurrentSite = Site.Load(id.Value);

                if (CurrentSite.IsNull())
                    return RedirectToAction("Index", "Landing");

                return View();
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        /// <summary>
        /// Gets pages for this site by page
        /// </summary>
        /// <param name="page">The current page</param>
        /// <param name="pageSize">The size of the current page</param>
        /// <returns></returns>
        [OutputCache(CacheProfile = "PagedGridData")]
        public virtual ActionResult Get(int page, int pageSize, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection, bool? active, string name, string url)
        {
            try
            {
                StringBuilder pageBuilder = new StringBuilder();
                //int count = 0;
                IEnumerable<Page> pages = Site.LoadPages(CurrentSite.SiteID);  //CurrentSite.Pages;

                if (active.HasValue)
                    pages = pages.Where(p => p.Active == active.Value);
                if (!string.IsNullOrEmpty(name))
                    pages = pages.Where(p => p.Name.ContainsIgnoreCase(name));
                if (!string.IsNullOrEmpty(url))
                    pages = pages.Where(p => p.Url.ContainsIgnoreCase(url));

                switch (orderBy)
                {
                    case "Title":
                        pages = orderByDirection == NetSteps.Common.Constants.SortDirection.Ascending ? pages.OrderBy(p => p.GetTitle(ApplicationContext.Instance.CurrentLanguageID)) : pages.OrderByDescending(p => p.GetTitle(ApplicationContext.Instance.CurrentLanguageID));
                        break;
                    case "Description":
                        pages = orderByDirection == NetSteps.Common.Constants.SortDirection.Ascending ? pages.OrderBy(p => p.GetDescription(ApplicationContext.Instance.CurrentLanguageID)) : pages.OrderByDescending(p => p.GetDescription(ApplicationContext.Instance.CurrentLanguageID));
                        break;
                    default:
                        pages = orderByDirection == NetSteps.Common.Constants.SortDirection.Ascending ? pages.OrderBy(orderBy) : pages.OrderByDescending(orderBy);
                        break;
                }

                var pageResult = pages.ToList();

                foreach (Page p in pageResult.Skip(page * pageSize).Take(pageSize))
                {
                    pageBuilder.Append("<tr id=\"").Append(p.PageID).Append("\">")
                        .AppendCheckBoxCell(value: p.PageID.ToString());

                    if (p.Editable)
                        pageBuilder.AppendLinkCell("~/Sites/Pages/Edit/" + p.PageID, p.Name);
                    else
                        pageBuilder.AppendCell(p.Name);

                    pageBuilder.AppendCell(p.Translations.GetByLanguageIdOrDefaultForDisplay().Title)
                        .AppendCell(p.Translations.GetByLanguageIdOrDefaultForDisplay().Description)
                        .AppendCell(p.Url)
                        .AppendCell(p.Active ? Translation.GetTerm("Active") : Translation.GetTerm("Inactive"))
                        .AppendCell(SmallCollectionCache.Instance.Layouts.GetById(p.LayoutID).Name)
                        .Append("</tr>");
                }
                return Json(new { totalPages = Math.Ceiling(pages.Count() / (double)pageSize), page = pageBuilder.ToString() });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }


        public virtual ActionResult ChangeStatus(List<int> items, bool active)
        {
            try
            {
                if (items != null)
                {
                    List<Page> pages = Page.LoadBatch(items);
                    foreach (int pageId in items)
                    {
                        Page page = pages.First(p => p.PageID == pageId);
                        if (page.Active != active)
                        {
                            page.Active = active;
                            page.Save();
                        }
                    }

                }
                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        /// <summary>
        /// Show a page to edit
        /// </summary>
        /// <param name="id">The id of the page</param>
        /// <returns></returns>
        [FunctionFilter("Sites-Site Pages", "~/Sites/Overview")]
        public virtual ActionResult Edit(int? id)
        {
            Page page = new Page() { PageTypeID = (short)NetSteps.Data.Entities.Constants.PageType.User };

            try
            {
                if (CurrentSite == null)
                {
                    return RedirectToAction("Index", "Landing");
                }

                IEnumerable<PageType> userDefinedPageTypes = PageType.LoadAllFull().Where(pt => pt.IsUserDefined);
                ViewBag.PageTypes = userDefinedPageTypes.ToList();

                var siteLayouts = Layout.GetLayoutsForSite(CurrentSite.SiteID).Select(l => SmallCollectionCache.Instance.Layouts.GetById(l));


                if (id.HasValue && id.Value > 0)
                {
                    page = Page.LoadFull(id.Value);

                    if (page.PageTypeID == NetSteps.Data.Entities.Constants.PageType.User.ToInt())
                    {
                        ViewBag.Layouts = page.PageType.Layouts.Where(l => siteLayouts.ToList(x => x.LayoutID).Contains(l.LayoutID)).ToList();
                    }
                    if (page.IsNotNull() && !page.Editable)
                    {
                        return RedirectToAction("Index");
                    }
                }
                else if (TempData["Page"] != null)
                {
                    page = (Page)TempData["Page"];
                }
                else
                {
                    page.PageType = PageType.LoadFull(page.PageTypeID);

                    ViewBag.Layouts = page.PageType.Layouts.Where(l => siteLayouts.ToList(x => x.LayoutID).Contains(l.LayoutID)).ToList();
                }

                return View(page);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        public virtual ActionResult GetTranslation(int? pageId, int languageId)
        {
            try
            {
                if (!pageId.HasValue)
                    return Json(new { result = true, name = "", description = "", keywords = "" });

                Page page = Page.PageWithTranslations(pageId.Value); // CurrentSite.Pages.FirstOrDefault(p => p.PageID == pageId.Value);
                var description = page.Translations.GetByLanguageIdOrDefault(languageId);

                return Json(new { result = true, title = description.Title, description = description.Description, keywords = description.Keywords });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }


        [FunctionFilter("Sites-Site Pages", "~/Sites/Overview")]
        public virtual ActionResult Save(PageModel model)
        {
            try
            {
                Page page = LoadLayoutAndPage(model);
                page.Name = model.Name;

                var translation = page.Translations.GetByLanguageIdOrDefaultInList(model.LanguageId);
                translation.LanguageID = model.LanguageId;
                translation.Title = model.Title;
                translation.Description = model.Description;
                translation.Keywords = model.Keywords;

                // Make sure we have 1 and only 1 slash at the beginning
                if (!model.Url.StartsWith("/"))
                    model.Url = "/" + model.Url;
                page.Url = model.Url.Replace("//", "/");
                page.ExternalUrl = model.ExternalUrl.IsNullOrEmpty() ? String.Empty : model.ExternalUrl;

                page.Active = model.Active;

                if (page.PageTypeID == NetSteps.Data.Entities.Constants.PageType.User.ToInt())
                {
                    var layout = Layout.LoadFull(model.LayoutId);
                    Page.ChangePageLayout(page, layout);
                }

                page.Save();

                return Json(new { result = true, pageId = page.PageID });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        private Page LoadLayoutAndPage(PageModel model)
        {
            Page page;
            if (model.LayoutId == 0)
            {
                var siteLayouts =
                    Layout.GetLayoutsForSite(CurrentSite.SiteID).Select(l => SmallCollectionCache.Instance.Layouts.GetById(l)).ToList();

                if (siteLayouts.Count > 0)
                {
                    model.LayoutId = siteLayouts.First().LayoutID;
                }
            }

            if (model.PageId > 0)
            {
                page = Page.LoadFull(model.PageId);
            }
            else
            {
                page = new Page()
                           {
                               IsStartPage = false,
                               Editable = true,
                               UseSsl = false,
                               PageTypeID = (int)NetSteps.Data.Entities.Generated.ConstantsGenerated.PageType.User,
                               SiteID = CurrentSite.SiteID
                           };
            }
            return page;
        }

        public virtual ActionResult GetPageTypeLayouts(short id)
        {
            var results = PageType.LoadFull(id).Layouts;
            var returnList = results.Select(x => new { LayoutID = x.LayoutID.ToString(), Name = x.Name }).ToArray();

            return Json(new { success = true, data = returnList });
        }
    }
}
