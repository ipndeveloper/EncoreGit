using System.Collections.Generic;
using NetSteps.Addresses.Common.Models;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business;

namespace NetSteps.Data.Entities.Repositories
{
	using System;

	public partial interface IProductRepository : ISearchRepository<FilterPaginatedListParameters<Product>, PaginatedList<Product>>
    {
        decimal GetProductPrice(int productID, int currencyID, int productPriceTypeID);
        List<ProductPrice> GetProductPrices(int productID, int currencyID);
        List<ProductSlimSearchData> SlimSearch(string query, int? pageSize = 1000);
        List<ProductSlimSearchData> LoadAllSlim(FilterPaginatedListParameters<Product> searchParams);
        List<int> GetOutOfStockProductIDs(IAddress address = null);
        InventoryLevels CheckStock(int productId, IAddress address = null);
        List<Product> LoadAllFullExcept(IEnumerable<int> productIDs);
        List<Product> LoadAllFullByStorefront(int storefrontID);
        PaginatedList<AuditLogRow> GetAuditLog(Product fullyLoadedProduct, AuditLogSearchParameters searchParameters);
        Product LoadFullBySKU(string sku);
        List<Product> Search(string query);

        List<Product> SearchProductForOrder(string query, int? pageSize = 100);

        Product Load(string productNumber);
        void ChangeActiveStatus(int productID, bool active);
		Product LoadWithRelations(int productID);
		List<Product> GetVariants(int productID);
		IList<int> GetExcludedShippingMethodIds(int productID);
		IList<int> GetExcludedShippingMethodIds(IEnumerable<int> productIds, out IList<int> productIdsWithExclusions);
		IEnumerable<int> GetExcludedShippingMethodIds(IEnumerable<int?> productIds);
		Product LoadOne(int productId);
		int GetBaseProductId(int productId);
		IEnumerable<Tuple<int, int>> GetBaseProductIds(IEnumerable<int> productIds);
		int GetProductIdBySKU(string sku);

		IEnumerable<int> GetActiveProductIdsForStoreFront(int storeFrontId);
		IEnumerable<int> GetActiveProductIdsForStoreFrontAndCategory(int storeFrontId, int categoryId, short? accountTypeId);
    }
}
