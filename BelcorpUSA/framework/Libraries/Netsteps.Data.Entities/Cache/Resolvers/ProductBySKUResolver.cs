using System;
using NetSteps.Core.Cache;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities.Cache
{
	public sealed class ProductBySKUResolver : DemuxCacheItemResolver<string, Tuple<string, int>>
	{
		#region Fields

		private IProductRepository _productRepository;

		#endregion

		#region Constrctors

		public ProductBySKUResolver(IProductRepository productRepository) { _productRepository = productRepository; }

		#endregion

		protected override bool DemultiplexedTryResolve(string key, out Tuple<string, int> value)
		{
			value = null;
			var id = _productRepository.GetProductIdBySKU(key);
			if (id > 0) value = Tuple.Create(key, id);
			return (value != null);
		}
	}
}
