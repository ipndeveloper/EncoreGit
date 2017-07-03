using System;
using System.Collections.Generic;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities
{
	public partial class HtmlContentHistory
	{
		public static List<HtmlContentHistory> GetHistoryForContent(int contentId)
		{
			try
			{
				return Repository.GetHistoryForContent(contentId);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static List<HtmlContentHistory> GetFullHistoryForUser(int userId)
		{
			try
			{
				return Repository.GetFullHistoryForUser(userId);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static List<HtmlContentHistory> GetUnseenHistoryForUser(int siteId, int userId)
		{
			try
			{
				return Repository.GetUnseenHistoryForUser(siteId, userId);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static List<HtmlContentHistory> GetUnseenHistoryForSectionAndUser(int htmlSectionId, int userId)
		{
			try
			{
				return Repository.GetUnseenHistoryForSectionAndUser(htmlSectionId, userId);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

        public static int GetUnseenHistoryCountForUser(int siteId, int userId, bool hasCMSContentApprovingFunction, int baseSiteId)
        {
            return Repository.GetUnseenHistoryCountForUser(siteId, userId, hasCMSContentApprovingFunction, baseSiteId);
        }
	}
}
