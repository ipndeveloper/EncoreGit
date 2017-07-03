using System.Collections.Generic;

namespace NetSteps.Data.Entities.Services
{
    public interface IProductListingService
    {
        List<Product> ExcludeInvalidProducts(IEnumerable<Product> products, short? accountTypeId = null, int currencyID = (int)Constants.Currency.UsDollar, NetSteps.Addresses.Common.Models.IAddress address = null);
        
        void ExpireCache();
        
        List<Catalog> GetActiveCatalogs(int storeFrontId);
        
        List<Category> GetActiveCategories(int storeFrontId, short? accountTypeId = null);
        
        List<Product> GetActiveProductsForCatalog(int storeFrontId, int catalogId, short? accountTypeId = null);
        
        List<Product> GetActiveProductsForCategory(int storeFrontId, int categoryId, short? accountTypeId = null);
        
        List<Product> GetActiveValidProductsForCategory(int storeFrontId, int categoryId, int numberOfProducts, out bool hasMore, short? accountTypeId = null, int? catalogId = null);
        
        List<Product> GetAllActiveProducts(int storeFrontId, short? accountTypeId = null, int? productPriceTypeID = null);
        
        Catalog GetCatalog(string name);
        
        Category GetCategoryTree(int categoryTreeId);
        
        IEnumerable<Product> GetDynamicKitProducts(int storeFrontId, bool sort = false, bool sortDescending = false);
        
        List<Product> GetNonDynamicKitProducts(int storeFrontID, short? accountTypeId = null);
        
        Product GetProduct(int productID);
        
        Product GetProduct(string sku);
        
        List<Product> GetProducts(IEnumerable<int> productIDs);
        
        IEnumerable<Product> GetProducts(int storeFrontId, int categoryId, short? accountTypeId);

        bool IsProductInStoreFront(int productID, int storeFrontID);

        bool IsProductInStoreFront(string sku, int storeFrontID);

        List<Product> SearchProducts(int storeFrontId, string query, short? accountTypeId = null, IEnumerable<int> catalogsToSearch = null, string startsWith = null, bool includeDynamicKits = true, int? productPriceTypeID = null);
    }
}
