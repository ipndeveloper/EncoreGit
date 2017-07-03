using System;
using System.Collections.Generic;
using NetSteps.Common.Configuration;
using NetSteps.Common.Extensions;
using NetSteps.Core.Cache;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Security;

namespace NetSteps.Data.Entities
{
	public partial class MailAccount
	{
		#region Properties
		public string Password
		{
			set
			{
				if (!value.IsNullOrEmpty())
					PasswordHash = SimpleHash.ComputeHash(value, Security.SimpleHash.Algorithm.SHA512, null);
			}
		}
		#endregion

		#region Methods
		public static MailAccount Authenticate(string email, string password)
		{
			try
			{
				return BusinessLogic.Authenticate(Repository, email, password);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static MailAccount LoadByAccountID(int accountID)
		{
			try
			{
				return BusinessLogic.LoadByAccountID(Repository, accountID);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static List<MailAccountSearchData> LoadSlimBatchByAccountIDs(IEnumerable<int> accountIDs)
		{
			try
			{
				return BusinessLogic.LoadSlimBatchByAccountIDs(Repository, accountIDs);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static bool IsAvailable(string emailAddress, int accountID)
		{
			try
			{
				return Repository.IsAvailable(emailAddress, accountID);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

        public static bool IsAvailable(string emailAddress)
        {
            try
            {
                return Repository.IsAvailable(emailAddress);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

		class CorporateMailAccountResolver : DemuxCacheItemResolver<int, MailAccount>
		{
			protected override bool DemultiplexedTryResolve(int key, out MailAccount value)
			{
				var result = MailAccount.LoadFull(key);

				if (result != null)
				{
					value = result;
					return true;
				}
				value = default(MailAccount);
				return false;
			}
		}

        static ICache<int, MailAccount> __corporateMailAccount = new ActiveMruLocalMemoryCache<int, MailAccount>("CorporateMailAccount", new CorporateMailAccountResolver());

		public static MailAccount GetCorporateMailAccount()
		{
			MailAccount value;
			int corporateMailAccountID = ConfigurationManager.GetAppSetting<int>(ConfigurationManager.VariableKey.CorporateMailAccountID);
			__corporateMailAccount.TryGet(corporateMailAccountID, out value);
			return value;
		}
		#endregion

        public static bool IsOtherAvailable(string emailAddress, int accountID)
        {
            try
            {
                return Repository.IsOtherAvailable(emailAddress, accountID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
    }
}
