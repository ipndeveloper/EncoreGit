using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities.Cache.Resolvers
{
    using Core.Cache;

    public class ProductByIdResolver : DemuxCacheItemResolver<int, Product>
    {
        private readonly IProductRepository _productRepository;

        public ProductByIdResolver(IProductRepository productRepository)
        {
            this._productRepository = productRepository;
        }

        protected override bool DemultiplexedTryResolve(int productId, out Product value)
        {
            value = _productRepository.LoadOne(productId);
            return value != null;
        }
    }
}