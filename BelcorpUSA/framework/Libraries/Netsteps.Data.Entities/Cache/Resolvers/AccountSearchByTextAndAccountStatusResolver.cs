namespace NetSteps.Data.Entities.Cache.Resolvers
{
    using System;
    using System.Collections.Generic;
    using Core.Cache;
    using Repositories;

    public class AccountSearchByTextAndAccountStatusResolver : DemuxCacheItemResolver<Tuple<string, int>, Dictionary<int, string>>
    {
        private readonly IAccountRepository _accountRepository;

        public AccountSearchByTextAndAccountStatusResolver(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        protected override bool DemultiplexedTryResolve(Tuple<string, int> key, out Dictionary<int, string> value)
        {
            value = _accountRepository.SearchAccountsByTextAndAccountStatus(key.Item1, key.Item2);
            return value != null;
        }
    }
}