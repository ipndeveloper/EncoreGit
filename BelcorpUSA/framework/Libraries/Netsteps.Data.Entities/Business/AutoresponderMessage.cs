using System;
using System.Collections.Generic;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities
{
	public partial class AutoresponderMessage
	{
		public static List<AutoresponderMessage> GetUnseenMessagesForAccount(int accountID)
		{
			try
			{
				return Repository.GetUnseenMessagesForAccount(accountID);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}
	}
}
