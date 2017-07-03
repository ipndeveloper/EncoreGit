using NetSteps.Common.Extensions;
using NetSteps.Core.Cache;
using NetSteps.Data.Entities.Business;
using NetSteps.Encore.Core.IoC;
using NetSteps.Common.Interfaces;

namespace NetSteps.Data.Entities.Cache
{
	/// <summary>
	/// Author: John Egbert
	/// Description: Class to cache large pieces of data. (Like products)
	/// Ideally this class should not contain a lot of large classes due to the increase in server memory usage. 
	/// Created: 04-13-2010
	/// </summary>
	public class CachedData
	{
		public static ITermTranslationProvider Translation
		{
			get
			{
				return Create.New<ITermTranslationProvider>();
			}
		}

		#region Users
		class UserItemRsolver : DemuxCacheItemResolver<int, UserSlimSearchData>
		{
			protected override bool DemultiplexedTryResolve(int key, out UserSlimSearchData value)
			{
				value = User.LoadSlim(key);
				return value != null;
			}
		}
		static ICache<int, UserSlimSearchData> _userCache = new ActiveMruLocalMemoryCache<int, UserSlimSearchData>("user-slim", new UserItemRsolver()
			);

		public static UserSlimSearchData GetUser(int userID)
		{
			return _userCache.Get(userID);
		}
		#endregion

		class AccountEmailAddressRsolver : DemuxCacheItemResolver<int, string>
		{
			protected override bool DemultiplexedTryResolve(int accountID, out string value)
			{
				var mailAccount = MailAccount.LoadByAccountID(accountID);

				if (mailAccount == null || mailAccount.EmailAddress.IsNullOrEmpty())
				{
					var accountSlim = Account.LoadSlim(accountID);
					value = (accountSlim != null && !accountSlim.EmailAddress.IsNullOrEmpty())
						? accountSlim.EmailAddress
						: default(string);
				}
				else
				{
					value = mailAccount.EmailAddress;
				}
				return value != null;
			}
		}
		static ICache<int, string> _accountEmailAddressCache = new ActiveMruLocalMemoryCache<int, string>("account-email", new AccountEmailAddressRsolver()
			);

		public static string GetAccountEmailAddress(int accountID, Account account = null)
		{
			return _accountEmailAddressCache.Get(accountID);
		}

		class AccountSlimResolver : DemuxCacheItemResolver<int, AccountSlimSearchData>
		{
			protected override bool DemultiplexedTryResolve(int accountID, out AccountSlimSearchData value)
			{
				value = Account.LoadSlim(accountID);
				return value != null;
			}
		}
		static ICache<int, AccountSlimSearchData> _accountSlimSearchCache = new ActiveMruLocalMemoryCache<int, AccountSlimSearchData>("account-slim", new AccountSlimResolver());

		public static AccountSlimSearchData GetAccountSlimSearch(int accountID, Account account = null)
		{
			return _accountSlimSearchCache.Get(accountID);
		}

		public static void ExpireCache()
		{
			_userCache.FlushAll();
			_accountEmailAddressCache.FlushAll();
			_accountSlimSearchCache.FlushAll();
		}
	}
}
