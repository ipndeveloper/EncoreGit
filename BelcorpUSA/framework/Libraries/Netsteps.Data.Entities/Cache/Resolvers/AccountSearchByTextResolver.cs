namespace NetSteps.Data.Entities.Cache.Resolvers
{
    using System.Collections.Generic;
    using Core.Cache;
    using Repositories;

    public class AccountSearchByTextResolver : DemuxCacheItemResolver<string, Dictionary<int, string>>
    {
        private readonly IAccountRepository _accountRepository;

        public AccountSearchByTextResolver(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        protected override bool DemultiplexedTryResolve(string key, out Dictionary<int, string> value)
        {
            value = _accountRepository.SearchAccountsByText(key);
            return value != null;
        }
    }
}