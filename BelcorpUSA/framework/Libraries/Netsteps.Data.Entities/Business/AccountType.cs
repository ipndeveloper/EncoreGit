using System;
using System.Collections.Generic;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities
{
	public partial class AccountType
	{
		public static List<Role> GetRolesByAccountType(short accountTypeID)
		{
			try
			{
				return Repository.GetRolesByAccountType(accountTypeID);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static bool CanSendEnrollmentCompletedEmail(int accountTypeID)
		{
			try
			{
				return BusinessLogic.CanSendEnrollmentCompletedEmail(accountTypeID);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}
	}
}
