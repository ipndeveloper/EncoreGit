using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class HtmlContentHistoryRepository : IHtmlContentHistoryRepository
	{
		public List<HtmlContentHistory> GetHistoryForContent(int contentId)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					return context.HtmlContentHistories.Where(hch => hch.HtmlContentID == contentId).ToList();
				}
			});
		}

		public List<HtmlContentHistory> GetFullHistoryForUser(int userId)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					return context.HtmlContentHistories.Where(hch => hch.HtmlContent.CreatedByUserID == userId).ToList();
				}
			});
		}

		public List<HtmlContentHistory> GetUnseenHistoryForUser(int siteId, int userId)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					return context.HtmlContentHistories.Where(hch => hch.HtmlContent.CreatedByUserID == userId && context.HtmlSectionContents.Any(hsc => hsc.SiteID == siteId && hsc.HtmlContentID == hch.HtmlContentID) && !hch.MessageSeen).ToList();
				}
			});
		}

		public List<HtmlContentHistory> GetUnseenHistoryForSectionAndUser(int htmlSectionId, int userId)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					return context.HtmlContentHistories.Where(hch => hch.HtmlContent.CreatedByUserID == userId && hch.HtmlContent.HtmlSectionContents.Any(hsc => hsc.HtmlSectionID == htmlSectionId) && !hch.MessageSeen).ToList();
				}
			});
		}

        private const int Submitted = (int)Constants.HtmlContentStatus.Submitted;

        private readonly Func<NetStepsEntities, Tuple<int, int,int>, int> _compiledGetUnseenHistoryCountForUserWithApprovals =
            CompiledQuery.Compile<NetStepsEntities, Tuple<int, int, int>, int>(
                (context, parameters) => (from htmlContent
                                    in context.HtmlContents
                                          where ((htmlContent.CreatedByUserID ?? 0).Equals(parameters.Item2)
                                                 && htmlContent.HtmlSectionContents.Any(hsc => (hsc.SiteID ?? 0).Equals(parameters.Item1))
                                                 &&
                                                 htmlContent.HtmlContentHistories.Any(history => history.MessageSeen.Equals(false)))
                                                || (htmlContent.HtmlSectionContents.Any(hsc => ((hsc.SiteID ?? 0).Equals(parameters.Item3 > 0 ? parameters.Item3 : parameters.Item1)) && hsc.HtmlContent.HtmlContentStatusID.Equals(Submitted)))
                                          select htmlContent).Count()
            );

        private readonly Func<NetStepsEntities, Tuple<int, int, int>, int> _compiledGetUnseenHistoryCountForUser =
            CompiledQuery.Compile<NetStepsEntities, Tuple<int, int, int>, int>(
                (context, parameters) => (from history
                               in context.HtmlContentHistories
                                          where (history.HtmlContent.CreatedByUserID ?? 0).Equals(parameters.Item2)
                                                && history.HtmlContent.HtmlSectionContents.Any(hsc => (hsc.SiteID ?? 0).Equals(parameters.Item1))
                                                && history.MessageSeen.Equals(false)
                                          select history).Count()
            );

        public int GetUnseenHistoryCountForUser(int siteId, int userId, bool hasCMSContentApprovingFunction, int baseSiteId)
        {
            var parameters = Tuple.Create<int, int, int>(siteId, userId, baseSiteId);

            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    if (hasCMSContentApprovingFunction)
                    {
                        // Add content awaiting approval messages
                        return _compiledGetUnseenHistoryCountForUserWithApprovals(context, parameters);
                    }

                    return _compiledGetUnseenHistoryCountForUser(context, parameters);
                }
            });
        }
	}
}
