using System.Collections.Generic;
using System.Linq;
using NetSteps.Core.Cache;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities.Cache.Resolvers
{
	public sealed class ProductBaseByIdResolver : DemuxCacheManyItemResolver<int, ProductBase>
	{
		private IProductBaseRepository _productBaseRepository;

		public ProductBaseByIdResolver(IProductBaseRepository productBaseRepository)
		{
			_productBaseRepository = productBaseRepository;
		}

		protected override bool DemultiplexedTryResolve(int key, out ProductBase value)
		{
			value = _productBaseRepository.LoadFull(key);
			return value != null;
		}

		protected override bool DemultiplexedTryResolveMany(IEnumerable<int> keys, out IEnumerable<KeyValuePair<int, ProductBase>> values)
		{
			values = _productBaseRepository.Where(pb => keys.Contains(pb.ProductBaseID)).Select(pb => new KeyValuePair<int, ProductBase>(pb.ProductBaseID, pb));
			return (values != null && values.Count() == keys.Count());
		}
	}
}
