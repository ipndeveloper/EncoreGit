namespace NetSteps.Data.Entities.Cache.Resolvers
{
    using System;
    using System.Collections.Generic;
    using Core.Cache;
    using Repositories;

    public class AccountSearchByTextAccountTypeAndSponsorIdResolver : DemuxCacheItemResolver<Tuple<string, int, int>, Dictionary<int, string>>
    {
        private readonly IAccountRepository _accountRepository;

        public AccountSearchByTextAccountTypeAndSponsorIdResolver(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        protected override bool DemultiplexedTryResolve(Tuple<string, int, int> key, out Dictionary<int, string> value)
        {
            value = _accountRepository.SearchAccountsByTextAccountTypeAndSponsorId(key.Item1, key.Item2, key.Item3);
            return value != null;
        }
    }
}