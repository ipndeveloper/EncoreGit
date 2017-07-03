using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using NetSteps.Common.Extensions;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class HtmlContentRepository
	{
		#region Members
		protected override Func<NetStepsEntities, IQueryable<HtmlContent>> loadAllFullQuery
		{
			get
			{
				return CompiledQuery.Compile<NetStepsEntities, IQueryable<HtmlContent>>(
					(context) => context.HtmlContents
						//.Include("HtmlContentHistories")
						//.Include("HtmlContentWorkflows")
										.Include("HtmlElements"));
			}
		}
		#endregion

		public List<HtmlContentAccountStatus> GetContentAndDistributorNameByStatus(int statusID)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					return context.usp_htmlcontent_select_content_and_distributor_by_status(statusID).ToList();
				}
			});
		}


		public List<HtmlContent> LoadBySectionNameAndSiteID(string sectionName, int siteID)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					var htmlContents = (from hc in context.HtmlContents
										join hsc in context.HtmlSectionContents on hc.HtmlContentID equals hsc.HtmlContentID
										join hs in context.HtmlSections on hsc.HtmlSectionID equals hs.HtmlSectionID
										where hsc.SiteID == siteID && hs.SectionName == sectionName
										select hc).ToList();

					return htmlContents;
				}
			});
		}


		// Port of usp_htmlcontent_select_contents - JHE
		public List<HtmlContent> LoadContents(int htmlSectionID, int siteID, int htmlContentStatusID)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					// Check to see if content exists and find BaseSiteID to load content if necessary - JHE
					var htmlContents = (from hc in context.HtmlContents
										join hsc in context.HtmlSectionContents on hc.HtmlContentID equals hsc.HtmlContentID
										where hsc.SiteID == siteID && hc.HtmlContentStatusID == htmlContentStatusID && hsc.HtmlSectionID == htmlSectionID
										select hc).ToList();

					if (htmlContents.Count == 0)
					{
						siteID = context.Sites.Where(s => s.SiteID == siteID).Select(s => s.BaseSiteID).FirstOrDefault().ToInt();
					}

					htmlContents = (from hc in context.HtmlContents
									join hsc in context.HtmlSectionContents on hc.HtmlContentID equals hsc.HtmlContentID
									where hsc.SiteID == siteID && hc.HtmlContentStatusID == htmlContentStatusID && hsc.HtmlSectionID == htmlSectionID
									select hc).ToList();

					return htmlContents;
				}
			});
		}

		// Ported from usp_htmlcontent_select_choices - JHE
		public List<HtmlContent> LoadChoices(int htmlSectionID)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					var htmlContents = (from hc in context.HtmlContents
										join hsc in context.HtmlSectionContents on hc.HtmlContentID equals hsc.HtmlContentID
										where hsc.HtmlSectionID == htmlSectionID
										select hc).ToList();

					return htmlContents;
				}
			});
		}

		// Ported from usp_htmlcontent_select_by_childsite - JHE
		public List<HtmlContent> GetChildSiteContent(int htmlSectionID, int siteID)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					var sectionChoises = from sc in context.HtmlSectionChoices
										 where sc.HtmlSectionID == htmlSectionID
										 select sc.HtmlContentID;

					var htmlContents = (from hc in context.HtmlContents
										join hsc in context.HtmlSectionContents on hc.HtmlContentID equals hsc.HtmlContentID
										where hsc.SiteID == siteID && hsc.HtmlSectionID == htmlSectionID && (!sectionChoises.Contains(hsc.HtmlContentID))
										select hc).ToList();

					return htmlContents;
				}
			});
		}

		public override void Delete(int primaryKey)
		{
			ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					if (context.HtmlSectionContents.Any(hsc => hsc.HtmlContentID == primaryKey))
						throw new NetSteps.Common.Exceptions.NetStepsDataException("Can't delete this html content while it's in use.");

					string sql = @"DELETE FROM HtmlSectionChoices WHERE HtmlContentID = @p0;
						DELETE FROM HtmlElements WHERE HtmlContentID = @p0;
						DELETE FROM HtmlContent WHERE HtmlContentID = @p0;";

					context.ExecuteStoreCommand(sql, primaryKey);
				}
			});
		}

        private Func<NetStepsEntities, Tuple<HtmlSection, int, int, int>, HtmlSectionContent> _compiledGetHtmlContent = CompiledQuery.Compile<NetStepsEntities, Tuple<HtmlSection, int, int, int>, HtmlSectionContent>(
            (context, parameters) => (from section
													in context.HtmlSectionContents.Include("HtmlContent").Include("HtmlContent.HtmlElements")
                                      where (section.SiteID ?? 0).Equals(parameters.Item2)
                                            && section.HtmlSectionID.Equals(parameters.Item1.HtmlSectionID)
                                            && section.HtmlContent.HtmlContentStatusID.Equals(parameters.Item3)
                                            && (section.HtmlContent.LanguageID ?? 0).Equals(parameters.Item4)
                                      orderby section.HtmlContent.PublishDateUTC ?? DateTime.MinValue descending
                                      select section).FirstOrDefault()
            );

        internal HtmlContent GetHtmlContent(HtmlSection htmlSection, Site site, Generated.ConstantsGenerated.HtmlContentStatus status, int languageId)
        {
            using (NetStepsEntities context = CreateContext())
            {
                int statusId = (int)status;
                var sectionContent = _compiledGetHtmlContent(context, Tuple.Create(htmlSection, site.SiteID, statusId, languageId));

                if (sectionContent == default(HtmlSectionContent))
                {
                    if (site.IsBase)
                    {
                        return null;
                    }

                    sectionContent = _compiledGetHtmlContent(context, Tuple.Create(htmlSection, site.BaseSiteID ?? 0, statusId, languageId));

                    if (sectionContent == default(HtmlSectionContent))
                    {
                        return null;
                    }
                }

                return sectionContent.HtmlContent;
            }
        }



		private readonly Func<NetStepsEntities, Tuple<HtmlSection, int>, IQueryable<int>>
			_compiledGetHtmlSectionContentStatus =
				CompiledQuery.Compile<NetStepsEntities, Tuple<HtmlSection, int>, IQueryable<int>>(
					(context, parameters) =>
					context.HtmlContentHistories
						.Where(h => h.HtmlContent.CreatedByUserID == parameters.Item2
							&& h.HtmlContent.HtmlSectionContents.Any(hsc => hsc.HtmlSectionID == parameters.Item1.HtmlSectionID)
							&& !h.MessageSeen && h.HtmlContentStatusID != (int)Constants.HtmlContentStatus.Pushed)
						.GroupBy(h => h.HtmlContentStatusID)
						.Select(s => s.Key));


        public Tuple<bool?, bool> GetHtmlSectionContentStatus(HtmlSection htmlSection, int userId)
        {
			if (!htmlSection.HtmlSectionContents.Any(sc => sc.HtmlContent.CreatedByUserID.Equals(ApplicationContext.Instance.CurrentUserID)))
			{
				return new Tuple<bool?, bool>(null, false);
			}
            bool submittedContent = false;
            bool? approved = null;

            // If the user did not create the content there will be no messages, don't need to incur the cost of this dtabase hit if not needed.
                using (NetStepsEntities context = CreateContext())
                {
					var unreadHistory = context.HtmlContentHistories
						.Where(h => h.HtmlContent.CreatedByUserID == userId
							&& h.HtmlContent.HtmlSectionContents.Any(hsc => hsc.HtmlSectionID == htmlSection.HtmlSectionID)
							&& !h.MessageSeen && h.HtmlContentStatusID != (int)Constants.HtmlContentStatus.Pushed)
						.GroupBy(h => h.HtmlContentStatusID)
						.Select(s => s.Key);

                    //var unreadHistory = _compiledGetHtmlSectionContentStatus(context, Tuple.Create<HtmlSection, int>(htmlSection, userId));

                    if (unreadHistory.Any())
                    {
                        submittedContent = true;
                    }

                    if (submittedContent)
                    {
                        approved = unreadHistory.Any(history => history != (int)Constants.HtmlContentStatus.Disapproved);
                    }
                }

            return new Tuple<bool?, bool>(approved, submittedContent);
        }
    }
}
