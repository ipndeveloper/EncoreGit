using System;
using System.Collections.Generic;
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
	public partial class HtmlSectionRepository
    {
        #region Members
        protected override Func<NetStepsEntities, IQueryable<HtmlSection>> loadAllFullQuery
        {
            get
            {
                return CompiledQuery.Compile<NetStepsEntities, IQueryable<HtmlSection>>(
                    (context) => context.HtmlSections
                                        .Include("HtmlSectionChoices")
                                        .Include("HtmlSectionChoices.HtmlContent")
                                        .Include("HtmlSectionChoices.HtmlContent.HtmlElements")
                                        .Include("HtmlSectionContents")
                                        .Include("HtmlSectionContents.HtmlContent")
                                        .Include("HtmlSectionContents.HtmlContent.HtmlElements"));
            }
        }
        #endregion

        private readonly Func<NetStepsEntities, int, HtmlSection> _compiledGetById = CompiledQuery.Compile
            <NetStepsEntities, int, HtmlSection>(
                (context, htmlSectionId) => (from section
                                                 in context.HtmlSections
                                                 .Include("HtmlSectionContents")
                                             where section.HtmlSectionID.Equals(htmlSectionId)
                                             select section).First()
            );

        public HtmlSection GetById(int htmlSectionId)
        {
            using (NetStepsEntities context = CreateContext())
            {
                return _compiledGetById(context, htmlSectionId);
            }
        }

        public HtmlSection LoadFullByContentID(int htmlContentID, int siteID)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var section = (from s in loadAllFullQuery(context)
                                   join sc in context.HtmlSectionContents on s.HtmlSectionID equals sc.HtmlSectionID
                                   where sc.HtmlContentID == htmlContentID
                                   select s).FirstOrDefault();
                    return section;
                }
            });
        }

        public HtmlSection LoadFullByTypeAndSectionName(short htmlSectionEditTypeID, string sectionName)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var section = (from s in loadAllFullQuery(context)
                                   where s.HtmlSectionEditTypeID == htmlSectionEditTypeID && s.SectionName == sectionName
                                   select s).FirstOrDefault();
                    return section;
                }
            });
        }

        public HtmlSection LoadFullByHtmlSectionIDAndSiteID(int htmlSectionID, int siteID)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var section = (from s in loadAllFullQuery(context)
                                   join sc in context.HtmlSectionContents on s.HtmlSectionID equals sc.HtmlSectionID
                                   where s.HtmlSectionID == htmlSectionID && sc.SiteID == siteID
                                   select s).FirstOrDefault();
                    return section;
                }
            });
        }

        public void SelectChoice(int siteID, int htmlSectionID, int htmlContentID)
        {
            ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    //context.u
                    throw new NotImplementedException("Finish implementing: usp_htmlsectioncontent_choose");
                }
            });
        }


        public PaginatedList<HtmlContentSearchData> SearchContent(HtmlContentSearchParameters searchParameters)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    PaginatedList<HtmlContentSearchData> results = new PaginatedList<HtmlContentSearchData>(searchParameters);

                    int htmlSectionID = searchParameters.HtmlSectionID;
                    IQueryable<HtmlContent> matchingItems = from hs in context.HtmlSections
                                                            join hsc in context.HtmlSectionContents on hs.HtmlSectionID equals hsc.HtmlSectionID
                                                            join hc in context.HtmlContents on hsc.HtmlContentID equals hc.HtmlContentID
                                                            where hs.HtmlSectionID == htmlSectionID
                                                            select hc;

                    if (!string.IsNullOrEmpty(searchParameters.Name))
                        matchingItems = matchingItems.Where(o => o.Name.Contains(searchParameters.Name));

                    if (searchParameters.LanguageID.HasValue)
                        matchingItems = matchingItems.Where(o => o.LanguageID == searchParameters.LanguageID);

                    if (searchParameters.HtmlContentStatusID.HasValue)
                        matchingItems = matchingItems.Where(o => o.HtmlContentStatusID == searchParameters.HtmlContentStatusID);

                    if (searchParameters.StartDate.HasValue)
                    {
                        DateTime startDateUTC = searchParameters.StartDate.Value.StartOfDay().LocalToUTC();
                        matchingItems = matchingItems.Where(a => a.PublishDateUTC >= startDateUTC);
                    }
                    if (searchParameters.EndDate.HasValue)
                    {
                        DateTime endDateUTC = searchParameters.EndDate.Value.EndOfDay().LocalToUTC();
                        matchingItems = matchingItems.Where(a => a.PublishDateUTC <= endDateUTC);
                    }

                    if (searchParameters.WhereClause != null)
                        matchingItems = matchingItems.Where(searchParameters.WhereClause);

                    if (!searchParameters.OrderBy.IsNullOrEmpty())
                    {
                        switch (searchParameters.OrderBy)
                        {
                            default:
                                matchingItems = matchingItems.ApplyOrderByFilter(searchParameters, context);
                                break;
                        }
                    }
                    else
                        matchingItems = matchingItems.OrderBy(o => o.PublishDateUTC);

                    results.TotalCount = matchingItems.Count();

                    // Apply Paging filter - JHE
                    if (searchParameters.PageSize.HasValue)
                        matchingItems = matchingItems.Skip(searchParameters.PageIndex * searchParameters.PageSize.Value).Take(searchParameters.PageSize.Value);

                    var ordersInfos = from o in matchingItems
                                      select new
                                      {
                                          o.HtmlContentID,
                                          o.LanguageID,
                                          o.HtmlContentStatusID,
                                          o.Name,
                                          o.PublishDate
                                      };

                    foreach (var o in ordersInfos.ToList())
                        results.Add(new HtmlContentSearchData()
                        {
                            HtmlContentID = o.HtmlContentID,
                            LanguageID = o.LanguageID.ToInt(),
                            HtmlContentStatusID = o.HtmlContentStatusID,
                            Name = o.Name,
                            PublishDate = o.PublishDate,
                            Language = SmallCollectionCache.Instance.OrderTypes.GetById(o.LanguageID.ToShort()).GetTerm(),
                            HtmlContentStatus = SmallCollectionCache.Instance.HtmlContentStatuses.GetById(o.HtmlContentStatusID).GetTerm()
                        });

                    return results;
                }
            });
        }

        public Dictionary<int, int> GetChoiceUsage(int htmlSectionId, int productionContentId, int baseSiteId)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    //var siteCount = context.Sites.Count(s => s.BaseSiteID == baseSiteId);
                   // var contentChosenCount = context.HtmlSectionContents.Count(hsc => hsc.HtmlSectionID == htmlSectionId && hsc.Site.BaseSiteID == baseSiteId);
                    //var distinctSectionChoices =
                    //    context.HtmlSectionChoices.Where(
                    //        hsc => hsc.HtmlSectionID == htmlSectionId && hsc.Site.BaseSiteID == baseSiteId).DistinctBy(
                    //            hsc => hsc.HtmlContentID);
                   // var result = distinctSectionChoices.ToDictionary(choice => choice.HtmlContentID, choice => context.HtmlSectionContents.Count(hsc => hsc.HtmlSectionID == htmlSectionId && hsc.Site.BaseSiteID == baseSiteId && hsc.HtmlContentID == choice.HtmlContentID));
                    //+ (choice.HtmlContentID == productionContentId ? siteCount - contentChosenCount : 0)

                    //Old code
                    //return context.HtmlSectionChoices.Where(hsc => hsc.HtmlSectionID == htmlSectionId && hsc.Site.BaseSiteID == baseSiteId)
                    //.DistinctBy(hsc => hsc.HtmlContentID)
                    //.ToDictionary(choice => choice.HtmlContentID, choice => context.HtmlSectionContents.Count(hsc => hsc.HtmlSectionID == htmlSectionId && hsc.Site.BaseSiteID == baseSiteId && hsc.HtmlContentID == choice.HtmlContentID) + (choice.HtmlContentID == productionContentId ? siteCount - contentChosenCount : 0));


                    //This feature has been removed for now. It was causing a massive slowdown and it wasn't even correct. We need to find out what is needed and figure out the proper way to do it.
                    var result = new Dictionary<int, int>();
                    return result;
                }
            });
        }
    }
}
