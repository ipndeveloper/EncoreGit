using System;
using System.Collections.Generic;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities.Commissions
{
	public partial class AccountLedger
	{
		#region Basic Crud
		public static List<AccountLedger> LoadByAccountID(int accountID)
		{
			try
			{
				var accountLedgers = Repository.LoadByAccountID(accountID);
				foreach (var accountLedger in accountLedgers)
				{
					accountLedger.StartTracking();
					accountLedger.IsLazyLoadingEnabled = true;
				}
				return accountLedgers;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static decimal GetCurrentBalance(int accountID)
		{
			try
			{
				return Repository.GetBalance(accountID, null, null);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static decimal GetBalance(int accountID, DateTime? effectiveDate, int? entryID)
		{
			try
			{
				return Repository.GetBalance(accountID, effectiveDate, entryID);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}
		#endregion
	}
}
