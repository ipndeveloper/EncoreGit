using System;
using System.Data.Objects;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class NewsRepository
    {
        #region Members
        protected override Func<NetStepsEntities, IQueryable<News>> loadAllFullQuery
        {
            get
            {
                return CompiledQuery.Compile<NetStepsEntities, IQueryable<News>>(
                 (context) => context.News
                                        .Include("HtmlSection")
                                        .Include("HtmlSection.HtmlSectionContents.HtmlContent")
                                        .Include("HtmlSection.HtmlSectionContents.HtmlContent.HtmlElements")
                                        .Include("EventContexts")
                                        .Include("EventContexts.DomainEventQueueItems")
                                        .Include("EventContexts.DomainEventQueueItems.QueueItemStatus"));
            }
        }

        protected override Func<NetStepsEntities, int, IQueryable<News>> loadFullQuery
        {
            get
            {
                return CompiledQuery.Compile<NetStepsEntities, int, IQueryable<News>>(
                 (context, newsID) => context.News
                                        .Include("HtmlSection")
                                        .Include("HtmlSection.HtmlSectionContents")
                                        .Include("HtmlSection.HtmlSectionContents.HtmlContent")
                                        .Include("HtmlSection.HtmlSectionContents.HtmlContent.HtmlElements")
                                        .Include("EventContexts")
                                        .Include("EventContexts.DomainEventQueueItems")
                                        .Include("EventContexts.DomainEventQueueItems.QueueItemStatus")
                                        .Where(n => n.NewsID == newsID));
            }
        }

        #endregion

        public PaginatedList<NewsSearchData> Search(NewsSearchParameters searchParameters)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    PaginatedList<NewsSearchData> results = new PaginatedList<NewsSearchData>(searchParameters);

                    IQueryable<News> matchingItems = context.News;

                    if (searchParameters.MarketID.HasValue)
                        matchingItems = matchingItems.Where(n => n.MarketID == searchParameters.MarketID.Value);

                    if (searchParameters.SiteID.HasValue)
                        matchingItems = matchingItems.Where(n => n.Sites.Any(s => s.SiteID == searchParameters.SiteID));

                    if (searchParameters.NewsTypeID.HasValue)
                        matchingItems = matchingItems.Where(n => n.NewsTypeID == searchParameters.NewsTypeID.Value);

                    matchingItems = matchingItems.ApplyDateRangeFilters("StartDateUTC", "EndDateUTC", searchParameters);

                    if (searchParameters.Active.HasValue)
                        matchingItems = matchingItems.Where(n => n.Active == searchParameters.Active.Value);

                    if (!string.IsNullOrEmpty(searchParameters.Title))
                        matchingItems = matchingItems.Where(n => n.HtmlSection != null
                            && n.HtmlSection.HtmlSectionContents.Any(hsc => hsc.HtmlContent.HtmlContentStatusID == (int)Constants.HtmlContentStatus.Production
                            && hsc.HtmlContent.HtmlElements.Any(e => e.HtmlElementTypeID == (int)Constants.HtmlElementType.Title && e.Contents.Contains(searchParameters.Title))));

                    if (searchParameters.WhereClause != null)
                        matchingItems = matchingItems.Where(searchParameters.WhereClause);

                    if (!string.IsNullOrEmpty(searchParameters.OrderBy))
                    {
                        if (searchParameters.OrderBy.Equals("Title", StringComparison.InvariantCultureIgnoreCase))
                            matchingItems = matchingItems.ApplyOrderByFilter(searchParameters.OrderByDirection, n => n.HtmlSection.HtmlSectionContents
                                              .FirstOrDefault(hsc => hsc.HtmlContent.HtmlContentStatusID == (int)Constants.HtmlContentStatus.Production).HtmlContent.HtmlElements
                                              .FirstOrDefault(e => e.HtmlElementTypeID == (int)Constants.HtmlElementType.Title).Contents);
                        else
                            matchingItems = matchingItems.ApplyOrderByFilters(searchParameters, a => a.StartDateUTC, context);
                    }

                    // TotalCount must be set before applying Pagination - JHE
                    results.TotalCount = matchingItems.Count();

                    matchingItems = matchingItems.ApplyPagination(searchParameters);

                    var newsResults = matchingItems.Select(n => new
                                      {
                                          n.NewsID,
                                          n.NewsTypeID,
                                          n.Active,
                                          Title = n.HtmlSection.HtmlSectionContents
                                              .FirstOrDefault(hsc => hsc.HtmlContent.HtmlContentStatusID == (int)Constants.HtmlContentStatus.Production).HtmlContent.HtmlElements
                                              .FirstOrDefault(e => e.HtmlElementTypeID == (int)Constants.HtmlElementType.Title).Contents,
                                          n.StartDateUTC,
                                          n.EndDateUTC
                                      });

                    foreach (var n in newsResults.ToList())
                        results.Add(new NewsSearchData()
                        {
                            NewsID = n.NewsID,
                            NewsTypeID = n.NewsTypeID,
                            Active = n.Active,
                            Title = n.Title,
                            NewsType = SmallCollectionCache.Instance.NewsTypes.GetById(n.NewsTypeID).GetTerm(),
                            StartDate = n.StartDateUTC.UTCToLocal(),
                            EndDate = n.EndDateUTC.HasValue ? n.EndDateUTC.UTCToLocal() : (DateTime?)null
                        });

                    return results;
                }
            });
        }
    }
}
