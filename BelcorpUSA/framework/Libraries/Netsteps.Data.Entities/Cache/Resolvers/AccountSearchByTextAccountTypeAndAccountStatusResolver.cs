namespace NetSteps.Data.Entities.Cache.Resolvers
{
    using System;
    using System.Collections.Generic;
    using Core.Cache;
    using Repositories;

    public class AccountSearchByTextAccountTypeAndAccountStatusResolver : DemuxCacheItemResolver<Tuple<string, int, int>, Dictionary<int, string>>
    {
        private readonly IAccountRepository _accountRepository;

        public AccountSearchByTextAccountTypeAndAccountStatusResolver(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        protected override bool DemultiplexedTryResolve(Tuple<string, int, int> key, out Dictionary<int, string> value)
        {
            value = _accountRepository.SearchAccountsByTextAccountTypeAndAccountStatus(key.Item1, key.Item2, key.Item3);
            return value != null;
        }
    }
}