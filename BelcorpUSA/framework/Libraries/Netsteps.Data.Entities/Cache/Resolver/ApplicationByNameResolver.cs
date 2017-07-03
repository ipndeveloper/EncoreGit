namespace NetSteps.Data.Entities.Cache.Resolver
{
    using Core.Cache;
    using Repositories;

    internal class ApplicationByNameResolver : DemuxCacheItemResolver<string, Application>
    {
        private readonly IApplicationRepository _applicationRepository;

        internal ApplicationByNameResolver(IApplicationRepository applicationRepository)
        {
            this._applicationRepository = applicationRepository;
        }

        protected override bool DemultiplexedTryResolve(string key, out Application value)
        {
            value = _applicationRepository.GetByName(key);
            return value != null;
        }
    }
}