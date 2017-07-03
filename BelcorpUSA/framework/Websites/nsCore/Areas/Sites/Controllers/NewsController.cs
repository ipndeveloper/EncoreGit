using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Attributes;
using System.Globalization;
using NetSteps.Web.Mvc.Helpers;

namespace nsCore.Areas.Sites.Controllers
{
    public class NewsController : BaseSitesController
    {
        /// <summary>
        /// Show the news for the current site
        /// </summary>
        [FunctionFilter("Sites-News", "~/Sites/Overview")]
        public virtual ActionResult Index(int? id)
        {
            try
            {
                CurrentSite = LoadSiteWithNewsData(id);

                if (CurrentSite == null)
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
        /// Batch operation to change the active status of a list of news
        /// </summary>
        /// <param name="items">A list of ids of all the news to change</param>
        /// <param name="active">Whether the news is active or not</param>
        /// <returns></returns>
        [FunctionFilter("Sites-News", "~/Sites/Overview")]
        public virtual ActionResult ChangeStatus(List<int> items, bool active)
        {
            try
            {
                if (items.Any())
                {
                    var site = Site.SiteWithNews(CurrentSite.SiteID);

                    foreach (int newsId in items)
                    {
                        News news = site.News.FirstOrDefault(n => n.NewsID == newsId);
                        if (news != null && news.Active != active)
                        {
                            news.Active = active;
                            news.Save();
                        }
                    }

                    site.DateLastModifiedUTC = DateTime.UtcNow;
                    site.Save();
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
        /// Batch operation to delete news
        /// </summary>
        /// <param name="items">A list of ids of news to delete</param>
        /// <returns></returns>
        [FunctionFilter("Sites-News", "~/Sites/Overview")]
        public virtual ActionResult Delete(List<int> items)
        {
            try
            {
                if (items.Count > 0)
                {
                    var site = Site.SiteWithNews(CurrentSite.SiteID);

                    foreach (int newsId in items)
                    {
                        var news = site.News.FirstOrDefault(n => n.NewsID == newsId);
                        if (news != null)
                        {
                            news.MarkAsDeleted();
                            CurrentSite.News.Remove(news); //Also remove from the in memory site object so that it doesn't still show the news item
                        }
                    }

                    site.DateLastModifiedUTC = DateTime.UtcNow; //Setting this causes sql dependency to update any site caches across all websites
                    site.Save();
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
        /// Get news by page
        /// </summary>
        /// <param name="page">The current page</param>
        /// <param name="pageSize">The size of the current page</param>
        /// <param name="orderBy">The column to order by</param>
        /// <param name="orderByDirection">The direction of the sort</param>
        /// <returns></returns>
        [OutputCache(CacheProfile = "PagedGridData")]
        public virtual ActionResult Get(int page, int pageSize, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection, bool? active, int? type, string title)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                var paginatedNews = NetSteps.Data.Entities.News.SearchNews(new NetSteps.Data.Entities.Business.NewsSearchParameters()
                {
                    NewsTypeID = type,
                    Active = active,
                    Title = title,
                    PageIndex = page,
                    PageSize = pageSize,
                    OrderBy = orderBy,
                    OrderByDirection = orderByDirection,
                    SiteID = CurrentSite.SiteID
                });
                int count = 0;
                foreach (NewsSearchData news in paginatedNews)
                {
                    builder.Append("<tr id=\"news").Append(news.NewsID).Append("\">")
                        .AppendCheckBoxCell(value: news.NewsID.ToString())
                        .AppendLinkCell("~/Sites/News/Edit/" + news.NewsID, news.Title)
                        .AppendCell(news.NewsType)
                        .AppendCell(news.StartDate.ToString("g"))
                        .AppendCell(news.EndDate.HasValue ? news.EndDate.Value.ToString("g") : Translation.GetTerm("N/A"))
                        .AppendCell(news.Active ? Translation.GetTerm("Active") : Translation.GetTerm("Inactive"))
                        .Append("</tr>");
                    ++count;
                }
                return Json(new { totalPages = paginatedNews.TotalPages, page = builder.ToString() });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        /// <summary>
        /// Show a news item to edit
        /// </summary>
        /// <param name="id">The id of the news to edit</param>
        /// <returns></returns>
        [FunctionFilter("Sites-News", "~/Sites/Overview")]
        public virtual ActionResult Edit(int? id)
        {
            try
            {

                var date = DateTime.Today.ToShortDateString();

              
                var date1 = DateTime.Today.ToString(new CultureInfo(CoreContext.CurrentCultureInfo.ToString()).DateTimeFormat.ShortDatePattern);

                if (CurrentSite == null)
                    return RedirectToAction("Index");
                CurrentSite = Site.SiteWithNews(CurrentSite.SiteID);
                ViewData["CurrentSite"] = CurrentSite;

                News news = null;
                if (id.HasValue && id.Value > 0)
                    news = CurrentSite.News.FirstOrDefault(n => n.NewsID == id.Value); //News.LoadFull(id.Value);
                else if (TempData["News"] != null)
                    news = TempData["News"] as News;
                if (news == null)
                    news = new News();
                if (news.EventContexts != null && news.EventContexts.Count > 0)
                {
                    var context = news.EventContexts.FirstOrDefault();
                    if (context != null && context.DomainEventQueueItems != null && context.DomainEventQueueItems.Count > 0)
                    {
                        var queueItem = context.DomainEventQueueItems.FirstOrDefault();
                        if (queueItem != null)
                        {
                            var devicePushMessage = Translation.GetTerm("This news item has already been slated to be pushed to devices and is currently ") + Translation.GetTerm(queueItem.QueueItemStatus.Name);
                            if (queueItem.LastRunDateUTC.HasValue)
                                devicePushMessage = string.Concat(devicePushMessage, ": " + queueItem.LastRunDateUTC.ToShortDateString() + " UTC");
                            ViewData["DevicePushMessage"] = devicePushMessage;
                        }
                    }
                }
                return View(news);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        public virtual ActionResult GetTranslation(int? newsId, int languageId)
        {
            try
            {
                if (!newsId.HasValue())
                    return Json(new { result = true, title = "", caption = "", body = "" });

                News news = News.LoadFull(newsId.Value);
                HtmlContent content = news.HtmlSection.ProductionContent(CurrentSite, languageId);
                if (content == null)
                    return Json(new { result = true, title = "", caption = "", body = "" });
                return Json(new
                {
                    result = true,
                    title = content.FirstOrEmptyElement(Constants.HtmlElementType.Title).Contents,
                    caption = content.FirstOrEmptyElement(Constants.HtmlElementType.Caption).Contents,
                    body = content.FirstOrEmptyElement(Constants.HtmlElementType.Body).Contents
                });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        /// <summary>
        /// Save news
        /// </summary>
        /// <param name="newsId">The id of the news</param>
        /// <param name="title">The title of the news</param>
        /// <param name="type">The type of news</param>
        /// <param name="date">The date the news was posted</param>
        /// <param name="active">Whether the news is active or not</param>
        /// <param name="isPublic">Whether the news is public or not</param>
        /// <param name="caption">The caption to put in the content of the news</param>
        /// <param name="body">The body of the content of the news</param>
        /// <returns></returns>
        [FunctionFilter("Sites-News", "~/Sites/Overview")]
        [ValidateInput(false)]
        public virtual ActionResult Save(int? newsId, int languageId, string title, int type, DateTime startDate, DateTime? startTime, DateTime? endDate, DateTime? endTime, bool active, bool isPublic, string caption, string body, bool isFeatured, bool isMobile)
        {
            News news = newsId.HasValue()
                            ? CurrentSite.News.FirstOrDefault(n => n.NewsID == newsId.Value) ?? new News()
                            : new News();

            try
            {
				news.StartEntityTracking();
                news.NewsTypeID = type.ToShort();
                news.StartDate = startDate.AddTime(startTime);
                news.EndDate = endDate.AddTime(endTime);
                news.Active = active;
                news.IsPublic = isPublic;
                news.IsFeatured = isFeatured;
                news.IsMobile = isMobile;

                //Create content if there is a caption or body
                if (!string.IsNullOrEmpty(title) || !string.IsNullOrEmpty(caption) || !string.IsNullOrEmpty(body))
                {
                    HtmlSection section = news.HtmlSection ?? new HtmlSection()
                        {
                            HtmlSectionEditTypeID = (int)Constants.HtmlSectionEditType.CorporateOnly,
                            HtmlContentEditorTypeID = (int)Constants.HtmlContentEditorType.RichText
                        };
                    section.SectionName = title;
                    HtmlContent content = section.ProductionContent(CurrentSite, languageId, false);

                    if (content == null)
                    {
                        content = new HtmlContent()
                            {
                                HtmlContentStatusID = (int)Constants.HtmlContentStatus.Production,
                                LanguageID = languageId,
                                Name = title,
                                PublishDate = DateTime.Now
                            };
                        section.HtmlSectionContents.Add(new HtmlSectionContent()
                        {
                            HtmlContent = content,
                            SiteID = CurrentSite.SiteID
                        });
                    }

                    if (!string.IsNullOrEmpty(title))
                    {
                        HtmlElement titleElement = content.FirstOrNewElement(Constants.HtmlElementType.Title);
                        titleElement.Contents = title;
                    }

                    if (!string.IsNullOrEmpty(caption))
                    {
                        HtmlElement captionElement = content.FirstOrNewElement(Constants.HtmlElementType.Caption);
                        captionElement.Contents = caption;
                    }

                    if (!string.IsNullOrEmpty(body))
                    {
                        HtmlElement bodyElement = content.FirstOrNewElement(Constants.HtmlElementType.Body);
                        bodyElement.Contents = body;
                    }

                    if (news.HtmlSection == null)
                    {
                        news.HtmlSection = section;
                    }
                }

                CurrentSite.DateLastModified = DateTime.Now;

                var rules = news.ValidateRecursive();
                if (!rules.IsValid)
                {
                    return Json(new { result = false, message = rules.BrokenRulesList.Select(x => x.Description).ToCommaSeparatedString() });
                }

                CurrentSite.News.Add(news);
                
                CurrentSite.Save();

                return Json(new { result = true, newsId = news.NewsID });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [FunctionFilter("Sites-News", "~/Sites/Overview")]
        public ActionResult PushNotifications(int newsID)
        {
            try
            {
                DomainEventQueueItem.AddBreakingNewsEventToQueue(newsID);
                return Json(new { result = true, message = Translation.GetTerm("BreakingNewsPushSuccessful", "Push notifications sent successfully.") });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
    }
}
