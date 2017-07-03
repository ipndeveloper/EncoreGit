using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Core.Cache;
using NetSteps.Data.Entities.Cache.Resolvers;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities.Cache
{
	public static class AccountCache
	{
		private static readonly IAccountRepository AccountRepository = new AccountRepository();

		private static readonly ICache<string, Dictionary<int, string>> AccountsByText = new ActiveMruLocalMemoryCache<string, Dictionary<int, string>>("AccountsByText", new AccountSearchByTextResolver(AccountRepository));
		private static readonly ICache<Tuple<string, int, int>, Dictionary<int, string>> AccountsByTextAccountTypeAndAccountStatus = new ActiveMruLocalMemoryCache<Tuple<string, int, int>, Dictionary<int, string>>("AccountsByTextAccountTypeAndAccountStatus", new AccountSearchByTextAccountTypeAndAccountStatusResolver(AccountRepository));
		private static readonly ICache<Tuple<string, int>, Dictionary<int, string>> AccountsByTextAndAccountStatus = new ActiveMruLocalMemoryCache<Tuple<string, int>, Dictionary<int, string>>("AccountsByTextAndAccountStatus", new AccountSearchByTextAndAccountStatusResolver(AccountRepository));
		private static readonly ICache<Tuple<string, int>, Dictionary<int, string>> AccountsByTextAndAccountType = new ActiveMruLocalMemoryCache<Tuple<string, int>, Dictionary<int, string>>("AccountsByTextAndAccountType", new AccountSearchByTextAndAccountTypeResolver(AccountRepository));
		private static readonly ICache<Tuple<string, int, int>, Dictionary<int, string>> AccountsByTextAccountTypeAndSponsorId = new ActiveMruLocalMemoryCache<Tuple<string, int, int>, Dictionary<int, string>>("AccountsByTextAccountTypeAndSponsorId", new AccountSearchByTextAccountTypeAndSponsorIdResolver(AccountRepository));

		public static Dictionary<int, string> GetAccountSearchByTextResults(string text)
		{
			Dictionary<int, string> results;
			AccountsByText.TryGet(text, out results);
			return results;
		}

		public static Dictionary<int, string> GetAccountSearchByTextAccountTypeAndAccountStatusResults(string text, int accountTypeId, int accountStatusId)
		{
			Dictionary<int, string> results;
			AccountsByTextAccountTypeAndAccountStatus.TryGet(new Tuple<string, int, int>(text, accountTypeId, accountStatusId), out results);
			return results;
		}

		public static Dictionary<int, string> GetAccountSearchByTextAndAccountStatusResults(string text, int accountStatusId)
		{
			Dictionary<int, string> results;
			AccountsByTextAndAccountStatus.TryGet(new Tuple<string, int>(text, accountStatusId), out results);
			return results;
		}

		public static Dictionary<int, string> GetAccountSearchByTextAndAccountTypeResults(string text, int accountTypeId)
		{
			Dictionary<int, string> results;
			AccountsByTextAndAccountType.TryGet(new Tuple<string, int>(text, accountTypeId), out results);
			return results;
		}

		public static Dictionary<int, string> GetAccountSearchByTextAndSponsorIdResults(string text, int accountTypeId, int accountStatusId)
		{
			Dictionary<int, string> results;
			AccountsByTextAccountTypeAndSponsorId.TryGet(new Tuple<string, int, int>(text, accountTypeId, accountStatusId), out results);
			return results;
		}

		public static KeyValuePair<int, string> GetTopAccountSearchByTextResult(string text)
		{
			var results = GetAccountSearchByTextResults(text);

			return results != null ? results.First() : default(KeyValuePair<int, string>);
		}
	}
}