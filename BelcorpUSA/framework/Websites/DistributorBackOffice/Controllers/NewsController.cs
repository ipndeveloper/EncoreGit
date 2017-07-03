using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using DistributorBackOffice.Areas.Account.Controllers;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Web.Mvc.Attributes;

namespace DistributorBackOffice.Controllers
{
    public class NewsController : BaseAccountsController
    {
        public virtual ActionResult Index(int? category)
        {
            ViewData["Categories"] = CurrentSite.News.Where(n => n.Active && DateTime.Now.ApplicationNow().IsBetween(n.StartDate, n.EndDate)).Select(n => n.NewsTypeID).Distinct();
            return View();
        }

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("News", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
        public virtual ActionResult Get(int? category, string query, int page, int? pageSize)
        {
            try
            {
                query = query.ToLower();
                IEnumerable<News> news = CurrentSite.News.Where(n =>
                {
                    var content = n.HtmlSection.ProductionContent(CurrentSite);
                    return n.Active && DateTime.Now.ApplicationNow().IsBetween(n.StartDate, n.EndDate) && (!category.HasValue || category.Value == n.NewsTypeID) && content != null
                        && ((content.Title() != null && content.Title().ToLower().Contains(query)) || (content.Caption() != null && content.Caption().ToLower().Contains(query)) || (content.Body() != null && content.Body().ToLower().Contains(query)));
                }).OrderByDescending(n => n.StartDate);
                var count = news.Count();
                var builder = new StringBuilder();

                if (pageSize.HasValue)
                    news = news.Skip(page * pageSize.Value).Take(pageSize.Value);

                foreach (var n in news)
                {
                    var content = n.HtmlSection.ProductionContent(CurrentSite);
                    var body = content.Body();
                    builder.Append("<div class=\"news\"><div class=\"FL newsDate\"><span class=\"newsMonth\">").Append(n.StartDate.ToString("MMM"))
                        .Append("</span><span class=\"newsDay\">").Append(n.StartDate.Day).Append("</span><span class=\"newsYear\">").Append(n.StartDate.Year)
                        .Append("</span></div><div class=\"FL newsInfo\"><a href=\"").Append("~/News/Details/".ResolveUrl()).Append(n.NewsID).Append("\"><span class=\"title\">")
                        .Append(content.Title()).Append("</span><span class=\"caption\">").Append(content.Caption()).Append("</a></span></div><span class=\"clr\"></span></div>");
                }

                return Json(new { totalPages = pageSize.HasValue ? Math.Ceiling(count / (double)pageSize.Value) : count > 0 ? 1 : 0, page = builder.ToString() });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [FunctionFilter("News", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
        public virtual ActionResult Details(int id)
        {
            ViewData["Categories"] = CurrentSite.News.Where(n => n.Active && DateTime.Now.ApplicationNow().IsBetween(n.StartDate, n.EndDate)).Select(n => n.NewsTypeID).Distinct();
            return View(CurrentSite.News.First(n => n.NewsID == id));
        }
    }
}
