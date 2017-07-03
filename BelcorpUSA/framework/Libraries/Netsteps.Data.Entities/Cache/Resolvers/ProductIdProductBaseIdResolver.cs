using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Core.Cache;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities.Cache.Resolvers
{
	public class ProductIdProductBaseIdResolver : DemuxCacheManyItemResolver<int, Tuple<int, int>>
	{
		private IProductRepository _productRepository;

		public ProductIdProductBaseIdResolver(IProductRepository productRepository)
		{
			_productRepository = productRepository;
		}

		protected override bool DemultiplexedTryResolve(int key, out Tuple<int, int> value)
		{
			value = null;
			var bid = _productRepository.GetBaseProductId(key);
			if (bid > 0) value = Tuple.Create(key, bid);
			return (value != null && value.Item2 > 0);
		}

		protected override bool DemultiplexedTryResolveMany(IEnumerable<int> keys, out IEnumerable<KeyValuePair<int, Tuple<int, int>>> values)
		{
			values = _productRepository.GetBaseProductIds(keys).Select(t => new KeyValuePair<int, Tuple<int, int>>(t.Item1, t)).ToArray();
			return (values != null && values.Count() == keys.Count());
		}
	}
}
