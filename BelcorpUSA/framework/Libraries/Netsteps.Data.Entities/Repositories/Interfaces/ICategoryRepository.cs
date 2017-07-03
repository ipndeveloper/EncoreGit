using System.Collections.Generic;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business;

namespace NetSteps.Data.Entities.Repositories
{
	public partial interface ICategoryRepository : ISearchRepository<FilterPaginatedListParameters<Category>, PaginatedList<CategorySearchData>>
	{
		List<Category> LoadAllFullByCategoryTypeId(int categoryTypeId);
		List<Category> LoadFullTopLevelByCategoryTypeId(int categoryTypeId);
		TrackableCollection<Category> LoadFullByParent(int parentCategoryID);
		int GetCategoryTreeID(int categoryID);
		Category LoadFullTree(int categoryTreeID);
		CategoryTranslation LoadTranslation(int categoryID, int languageID);
		List<ArchiveCategorySearchData> LoadAllArchiveCategories();
		void DeleteTree(int categoryTreeID);
	    List<Category> LoadCategoriesByStoreFrontId(int storeFrontId);
        Category LoadCategoryByNumber(string categoryNumber);

		IEnumerable<int> GetCategoryIdsByStorefront(int storeFrontId);
		IEnumerable<int> GetActiveCategoryIdsByStoreFrontIdAndAccountTypeId(int storeFrontId, short? accountTypeId = null);
		IEnumerable<Category> GetActiveCategoriesByStoreFrontIdAndAccountTypeId(int storeFrontId, short? accountTypeId = null);
	}
}
