using System;
using System.Collections.Generic;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business;

namespace NetSteps.Data.Entities.Repositories
{
	public partial interface ICatalogRepository : ISearchRepository<FilterPaginatedListParameters<Catalog>, PaginatedList<CatalogSearchData>>
	{
		void Copy(int copyFromCatalogID, int copyToCatalogID);

		void BulkAddCatalogItems(int catalogID, IEnumerable<int> productIDs, DateTime? startDate, DateTime? endDate);
		List<Catalog> LoadAllFullByMarkets(IEnumerable<int> marketIDs);
		List<Catalog> LoadAllFullExcept(IEnumerable<int> catalogIDs);

		IEnumerable<Catalog> LoadActiveCatalogsForStoreFront(int storeFrontId);
		IEnumerable<int> GetActiveCatalogProductIds(int catalogId, short? accountTypeId, int? productPriceTypeID);

        void RemoveAccountTypeFilters(int catalogID);
        void AddAccountTypeFilters(int catalogID, IEnumerable<short> accountTypeIDs);

		Catalog GetCatalogByName(string name);
        IEnumerable<short> GetAccountTypeIDsForCatalog(Catalog catalog);
	}
}
