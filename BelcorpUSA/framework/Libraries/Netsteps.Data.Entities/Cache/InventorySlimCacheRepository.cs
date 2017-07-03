using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NetSteps.Common.Base;
using NetSteps.Common.Extensions;

namespace NetSteps.Data.Entities.Cache
{
    /// <summary>
    /// This class is for encore customers with product count more than 50,000.
    /// </summary>
    internal class InventorySlimCacheRepository : InventoryBaseRepository
    {
        #region Members
        private const string SKU_COLUMN = "SKU";
        private const string PRODUCT_ID_COLUMN = "ProductID";
        #endregion

        internal InventorySlimCacheRepository()
        {
            _storeFronts = new AsyncReloadDictionary<int, StoreFront>(StoreFront.GetStoreFrontWithInventory);
            _categoryTrees = new AsyncReloadDictionary<int, Category>(Category.LoadFullTree);
            InitializeCache(ApplicationContext.Instance.StoreFrontID);
        }

        #region Load Cache

        private void InitializeCache(int storeFrontId)
        {
            var tmp = _storeFronts[storeFrontId].Value;
            ReloadCatalogsAndProducts();
        }

        protected override void ReloadCache()
        {
            _storeFronts.ExpireCache();
            _categoryTrees.ExpireCache();
            ReloadCatalogsAndProducts();
        }

        private void ReloadCatalogsAndProducts()
        {
            if (_changeAction == null)
            {
                System.Data.SqlClient.SqlDependency dependency =
                    CacheDependency.GetSqlDependencyByName(CACHE_DEPENDENCY_NAME);
                _changeAction = new SqlDependencyChangeAction(dependency, ExpireCache);
            }

            var catalogs = Catalog.LoadAll();
            Interlocked.Exchange(ref _catalogs, catalogs);

            IndexedListColumns<Product> productColumns = new IndexedListColumns<Product>();
            productColumns.Add(SKU_COLUMN, x => x.SKU);
            productColumns.Add(PRODUCT_ID_COLUMN, x => x.ProductID);
            var productsLookup = new IndexedList<Product>(Product.LoadAllFullByStorefront(ApplicationContext.Instance.StoreFrontID), productColumns);
            Interlocked.Exchange(ref _productsLookup, productsLookup);
        }

        #endregion

        protected override List<Product> DoGetAllActiveProducts(int storeFrontId, short? accountTypeId = null, int? productPriceTypeID = null)
        {
            List<Catalog> activeCatalogs = GetActiveCatalogs(storeFrontId);

            Dictionary<int, Product> activeProductsAdded = new Dictionary<int, Product>();

            foreach (Catalog catalog in activeCatalogs)
            {
                IEnumerable<Product> catalogProducts = FilterActiveCatalogItems(catalog.CatalogItems.AsQueryable(), accountTypeId, productPriceTypeID);
                foreach (var catalogProduct in catalogProducts)
                {
                    if (!activeProductsAdded.ContainsKey(catalogProduct.ProductID))
                        activeProductsAdded.Add(catalogProduct.ProductID, catalogProduct);
                }
            }

            var results = activeProductsAdded.Values.ToList();

            return results;
        }

        public override List<Product> GetActiveProductsForCatalog(int storeFrontId, int catalogId, short? accountTypeId = null)
        {
            List<Catalog> activeCatalogs = GetActiveCatalogs(storeFrontId);

            if (!activeCatalogs.Any(c => c.CatalogID == catalogId))
                return new List<Product>();

            return FilterActiveCatalogItems(activeCatalogs.First(c => c.CatalogID == catalogId).CatalogItems.AsQueryable(),
                                            accountTypeId).ToList();
        }

        public override List<Product> GetNonDynamicKitProducts(int storeFrontID, short? accountTypeId = null)
        {
            return DoGetAllActiveProducts(storeFrontID, accountTypeId).Where(p => !p.IsDynamicKit()).ToList();
        }

        #region Get Products

        public override Product GetProduct(int productID)
        {

            if (_productsLookup.Get(PRODUCT_ID_COLUMN, productID).Count > 0)
                return _productsLookup.Get(PRODUCT_ID_COLUMN, productID)[0];
            else
                throw new Exception(string.Format("ProductID specified ({0}) cannot be found in cache.", productID));
        }

        public override Product GetProductwithoutCache(int productID)
        {
            if (_productsLookup.Get(PRODUCT_ID_COLUMN, productID).Count > 0)
                return _productsLookup.Get(PRODUCT_ID_COLUMN, productID)[0];
            else
                throw new Exception(string.Format("ProductID specified ({0}) cannot be found in cache.", productID));
        }

        public override List<Product> GetProducts(IEnumerable<int> productIDs)
        {
            List<Product> results = new List<Product>();
            foreach (var productID in productIDs)
            {
                if (_productsLookup.Get(PRODUCT_ID_COLUMN, productID).Count > 0)
                    results.AddRange(_productsLookup.Get(PRODUCT_ID_COLUMN, productID));
                else
                    throw new Exception(string.Format("ProductID specified ({0}) cannot be found in cache.", productID));
            }
            return results;
        }

        public override Product GetProduct(string sku)
        {
            var results = _productsLookup.Get(SKU_COLUMN, sku);
            if (results.Count > 0)
                return results[0];
            else
                throw new Exception(string.Format("Product Sku specified ({0}) cannot be found in cache.", sku));
        }

        #endregion

        public override List<Category> GetActiveCategories(int storeFrontId, short? accountTypeId = null)
        {
            var lookup = new Tuple<int, short?>(storeFrontId, accountTypeId);
            return _activeCategoriesByStoreFront.GetOrAdd(lookup, LoadActiveCategoriesByStoreFrontIdAndAccountTypeId);
        }

        protected override List<Category> LoadActiveCategoriesByStoreFrontIdAndAccountTypeId(Tuple<int, short?> lookup)
        {
            return Category.LoadCategoriesByStoreFrontId(lookup.Item1);
        }
    }
}