namespace NetSteps.Data.Entities.Cache
{
    using Core.Cache;
    using Repositories;
    using Resolvers;

    public static class ApplicationCache
    {
        static readonly IApplicationRepository ApplicationRepository = new ApplicationRepository();
        static readonly ActiveMruLocalMemoryCache<string, Application> ApplicationsByNameCache = new ActiveMruLocalMemoryCache<string, Application>("ApplicationsByName", new ApplicationByNameResolver(ApplicationRepository));

        public static Application GetByName(string name)
        {
            Application cachedValue;
            ApplicationsByNameCache.TryGet(name, out cachedValue);
            return cachedValue;
        }
    }
}