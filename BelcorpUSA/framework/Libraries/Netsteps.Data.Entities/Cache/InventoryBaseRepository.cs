using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Addresses.Common.Models;
using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Encore.Core.IoC;
using NetSteps.Data.Entities.Extensions;

namespace NetSteps.Data.Entities.Cache
{
    public abstract class InventoryBaseRepository
    {
        public readonly string CACHE_DEPENDENCY_NAME = "Inventory";

        protected ConcurrentDictionary<Tuple<int, short?>, List<Category>> _activeCategoriesByStoreFront = new ConcurrentDictionary<Tuple<int, short?>, List<Category>>();
        protected ConcurrentDictionary<int, List<Product>> _dynamicKitProducts = new ConcurrentDictionary<int, List<Product>>();

        protected AsyncReloadDictionary<int, StoreFront> _storeFronts;
        protected IndexedList<Product> _productsLookup;
        protected List<Catalog> _catalogs;
        protected AsyncReloadDictionary<int, Category> _categoryTrees;
        protected SqlDependencyChangeAction _changeAction;

        public abstract Product GetProduct(int productID);
        public abstract Product GetProductwithoutCache(int productID);
        public abstract List<Product> GetProducts(IEnumerable<int> productIDs);
        public abstract Product GetProduct(string sku);
        protected abstract void ReloadCache();

        private StoreFront GetStoreFront(int storeFrontId)
        {
            return DoGetStoreFront(storeFrontId);
        }

        protected virtual StoreFront DoGetStoreFront(int storeFrontId)
        {
            var storeFront = _storeFronts[storeFrontId].Value;
            if (storeFront == null)
                throw new Exception(string.Format("StoreFrontId not found: {0}", storeFrontId));
            return storeFront;
        }

        protected IQueryable<Product> FilterActiveCatalogItems(IQueryable<CatalogItem> items, short? accountTypeId = null, int? productPriceTypeID = null)
        {
            IQueryable<CatalogItem> query = items.Where(x =>
                    (x.StartDateUTC == null || x.StartDateUTC <= DateTime.UtcNow)
                    && (x.EndDateUTC == null || x.EndDateUTC > DateTime.UtcNow)
            );
#if DEBUG
			var q = query.ToTraceString();
#endif
            query = query.Where(p => p.Product.Active);
#if DEBUG
			q = query.ToTraceString();
#endif

            if (productPriceTypeID.HasValue)
            {
                query =
                        query.Where(
                                p =>
                                p.Product.Prices.Select(x => x.ProductPriceType).Any(
                                        ppt => ppt.ProductPriceTypeID == productPriceTypeID));
            }

            if (accountTypeId.HasValue)
            {
                query =
                        query.Where(
                                p =>
                                p.Product.Prices.Select(x => x.ProductPriceType).Any(
                                        ppt => ppt.AccountPriceTypes.Any(apt => apt.AccountTypeID == accountTypeId)));
            }
#if DEBUG
			q = query.ToTraceString();
#endif
            var result = query.Select(d => d.Product).Distinct();

            return result;
        }

        protected abstract List<Product> DoGetAllActiveProducts(int storeFrontId, short? accountTypeId = null, int? productPriceTypeID = null);

        public abstract List<Product> GetNonDynamicKitProducts(int storeFrontID, short? accountTypeId = null);

        public List<Catalog> GetActiveCatalogs(int storeFrontId)
        {
            return DoGetActiveCatalogs(storeFrontId);
        }

        public List<Catalog> GetActiveCatalogs(int StoreFrontID, int LanguageID)
        {
            return CatalogExtensions.GetActiveCatalogs(StoreFrontID, LanguageID);
        }

        protected virtual List<Catalog> DoGetActiveCatalogs(int storeFrontId)
        {
            return GetStoreFront(storeFrontId).Catalogs.Where(c => c.Active && DateTime.Now.ApplicationNow().IsBetween(c.StartDate, c.EndDate)).ToList();
        }

        public List<Product> GetAllActiveProducts(int storeFrontId, short? accountTypeId = null, int? productPriceTypeID = null)
        {
            return DoGetAllActiveProducts(storeFrontId, accountTypeId, productPriceTypeID);
        }

        public virtual List<Product> SearchProducts(
                int storeFrontId,
                string query,
                short? accountTypeId = null,
                IEnumerable<int> catalogsToSearch = null,
                string startsWith = null,
                bool includeDynamicKits = true,
                int? productPriceTypeID = null)
        {
            query = query.ToCleanString().ToLower();

            var matchingProducts = (catalogsToSearch == null || !catalogsToSearch.Any())
                    ? GetAllActiveProducts(storeFrontId, accountTypeId, productPriceTypeID)
                    : catalogsToSearch
                            .SelectMany(c => GetActiveProductsForCatalog(storeFrontId, c, accountTypeId))
                            .DistinctBy(p => p.ProductID);

            var results = new List<Product>();

            if (!includeDynamicKits)
                matchingProducts = matchingProducts.Where(p => !p.IsDynamicKit());

            //Add the products in order of importance in search criteria - DES
            if (startsWith != null)
            {
                results.AddRange(matchingProducts.Where(p => p.Translations.Any(t => t.Name.StartsWith(startsWith, System.StringComparison.OrdinalIgnoreCase))));
            }
            else
            {
                results.AddRange(matchingProducts.Where(p => p.SKU.ContainsIgnoreCase(query)));
                results.AddRange(matchingProducts.Where(p =>
                    p.Translations.Any(t => t.Name.ContainsIgnoreCase(query)
                        || t.ShortDescription.ContainsIgnoreCase(query)
                        || t.LongDescription.ContainsIgnoreCase(query))
                    && !results.Contains(p)));
            }
            return results;
        }

        public abstract List<Product> GetActiveProductsForCatalog(int storeFrontId, int catalogId, short? accountTypeId = null);

        public abstract List<Category> GetActiveCategories(int storeFrontId, short? accountTypeId = null);

        protected abstract List<Category> LoadActiveCategoriesByStoreFrontIdAndAccountTypeId(Tuple<int, short?> lookup);

        public List<Product> GetActiveProductsForCategory(int storeFrontId, int categoryId, short? accountTypeId = null)
        {
            var categoryProducts = GetProducts(storeFrontId, categoryId, accountTypeId);
            if (categoryProducts != null && categoryProducts.Any())
            {
                return categoryProducts.Where(x => x.Active).ToList();
            }

            return new List<Product>();
        }


        public List<Product> GetActiveProductsForCategorys(int storeFrontId, int categoryId, short? accountTypeId = null)
        {
            //Visualizar la face de la categoria
            int PhaseCategory = PreOrderExtension.GetPhaseCategory(categoryId);
            IEnumerable<Product> categoryProducts = null;
            if (PhaseCategory == 2)
            {
                categoryProducts = ProductCache.GetProductsByIds(PreOrderExtension.GetProductByCategory(categoryId));
            }
            else
            {
                categoryProducts = GetProducts(storeFrontId, categoryId, accountTypeId);
            }
            if (categoryProducts != null && categoryProducts.Any())
            {
                return categoryProducts.Where(x => x.Active).ToList();
            }

            return new List<Product>();
        }

        public List<Product> GetActiveValidProductsForCategory(int storeFrontId, int categoryId, int numberOfProducts, out bool hasMore, short? accountTypeId = null, int? catalogId = null)
        {
            hasMore = false;

            IEnumerable<Product> categoryProducts = GetProducts(storeFrontId, categoryId, accountTypeId);
            if (catalogId.HasValue)
                categoryProducts = categoryProducts.Where(p => p.CatalogItems.Any(c => c.CatalogID == catalogId.Value && DateTime.Now.ApplicationNow().IsBetween(c.StartDate, c.EndDate)));

            var products = ExcludeInvalidProducts(categoryProducts, accountTypeId).Where(p => p.ProductBase.Products.Count == 1 || p.IsVariantTemplate).ToList();
            if (products.Count() > numberOfProducts)
            {
                hasMore = true;
                products = products.Take(numberOfProducts).ToList();
            }
            return products.Where(x => x.Active).ToList();
        }

        public virtual IEnumerable<Product> GetProducts(int storeFrontId, int categoryId, short? accountTypeId)
        {
            List<Product> activeProducts = GetAllActiveProducts(storeFrontId, accountTypeId);
            return activeProducts.Where(p => p.ProductBase.Categories.Any(c => c.CategoryID == categoryId)).OrderByDescending(p => p.CatalogItems.Max(c => c.StartDate));
        }

        public virtual List<Product> ExcludeInvalidProducts(IEnumerable<Product> products, short? accountTypeId = null, int currencyID = (int)Constants.Currency.UsDollar, IAddress address = null)
        {
            //remove variant templates, or those products that are out of stock with a backorderbehavior != hide
            var filteredProducts = products.Where(p => p.IsVariantTemplate || (!IsOutOfStock(p) || !p.ProductBase.IsShippable || p.ProductBackOrderBehaviorID != Constants.ProductBackOrderBehavior.Hide.ToInt()));
            var currentDate = System.DateTime.Now;
            filteredProducts = products.Where(
                p => p.CatalogItems.Where(ct => ct.Catalog.Active
                     && ct.Catalog.StoreFronts.Where(st => st.Name == "PWS").Count() > 0
                     && (ct.Catalog.StartDate == null || ct.Catalog.StartDate <= currentDate)
                     && (ct.Catalog.EndDate == null || ct.Catalog.EndDate >= currentDate)
                     && ct.Active
                     && (ct.StartDate == null || ct.StartDate <= currentDate)
                     && (ct.EndDate == null || ct.EndDate >= currentDate)
                     ).Count() > 0
                );
            if (accountTypeId.HasValue)
                return filteredProducts.Where(p => p.ContainsPrice(accountTypeId.Value, currencyID)).ToList();
            else
                return filteredProducts.Where(p => p.Prices.Any(x => x.CurrencyID == currencyID)).ToList();
        }


        public virtual bool IsOutOfStock(Product product)
        {
            return product.WarehouseProducts.All(wp => !wp.IsAvailable || wp.QuantityOnHand <= (wp.QuantityBuffer + wp.QuantityAllocated));
        }

        public virtual bool IsAvailable(Product product)
        {
            if (!product.ProductBase.IsShippable)
                return true;

            return product.WarehouseProducts.All(wp => (wp.IsAvailable && wp.QuantityOnHand > (wp.QuantityBuffer + wp.QuantityAllocated)
                || wp.Product.ProductBackOrderBehaviorID == (int)Constants.ProductBackOrderBehavior.AllowBackorder
                || wp.Product.ProductBackOrderBehaviorID == (int)Constants.ProductBackOrderBehavior.AllowBackorderInformCustomer));
        }

        public virtual bool IsAvailable(int productId)
        {
            return IsAvailable(GetProduct(productId));
        }

        public virtual bool IsOutOfStock(int productId)
        {
            return IsOutOfStock(GetProduct(productId));
        }

        public Category GetCategoryTree(int categoryTreeId)
        {
            return DoGetCategoryTree(categoryTreeId);
        }

        protected virtual Category DoGetCategoryTree(int categoryTreeId)
        {
            return _categoryTrees[categoryTreeId].Value;
        }

        #region Get From lookup cache


        public IEnumerable<Product> GetDynamicKitProducts(int storeFrontId, bool sort = false, bool sortDescending = false)
        {
            return DoGetDynamicKitProducts(storeFrontId, sort, sortDescending);
        }

        protected virtual IEnumerable<Product> DoGetDynamicKitProducts(int storeFrontId, bool sort, bool sortDescending)
        {
            List<Product> dynamicKitProducts = null;
            if (!_dynamicKitProducts.ContainsKey(storeFrontId))
            {
                dynamicKitProducts = GetAllActiveProducts(storeFrontId).Where(p => p.IsDynamicKit() && !p.IsVariantTemplate).ToList();
                _dynamicKitProducts.AddOrUpdate(storeFrontId, dynamicKitProducts, (key2, value) => value);
            }
            dynamicKitProducts = _dynamicKitProducts[storeFrontId];

            if (dynamicKitProducts == null) dynamicKitProducts = new List<Product>();

            if (!sort)
            {
                return dynamicKitProducts;
            }

            if (sortDescending)
            {
                return dynamicKitProducts.OrderByDescending(kp => (kp.DynamicKits[0].DynamicKitGroups.Sum(g => g.MinimumProductCount)));
            }

            return dynamicKitProducts.OrderBy(kp => (kp.DynamicKits[0].DynamicKitGroups.Sum(g => g.MinimumProductCount)));
        }
        /// <summary>
        /// Searches through catalog cache by name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Slim Version of Catalog no child objects</returns>
        public Catalog GetCatalog(string name)
        {
            return DoGetCatalogByName(name);
        }

        protected virtual Catalog DoGetCatalogByName(string name)
        {
            foreach (var catalog in _catalogs)
            {
                if (catalog.Translations.Any(t => t.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)))
                    return catalog;
            }
            return null;
        }

        public bool IsProductInStoreFront(string sku, int storeFrontID)
        {
            var inventory = Create.New<InventoryBaseRepository>();

            bool result = false;

            Product product = inventory.GetProduct(sku);

            if (product != null)
            {
                result = IsProductInStoreFront(product.ProductID, storeFrontID);
            }

            return result;
        }

        public bool IsProductInStoreFront(int productID, int storeFrontID)
        {
            return DoIsProductInStoreFront(productID, storeFrontID);
        }

        protected virtual bool DoIsProductInStoreFront(int productID, int storeFrontID)
        {
            bool result = false;

            result = GetAllActiveProducts(storeFrontID).Any(p => p.ProductID == productID);

            return result;
        }

        public void ExpireCache()
        {
            // Since we will invalidate the cache in a few lines, unhook the onchange event handler
            // otherwise the CacheDependency.InvalidateCache will force another reload of the cache.
            CacheDependency.GetSqlDependencyByName(this.CACHE_DEPENDENCY_NAME).OnChange -= null;

            if (this._changeAction != null)
            {
                ((IDisposable)this._changeAction).Dispose();
                this._changeAction = null;
            }

            // This call will change a date stamp in the database that is used as a dependency.
            // Invalidate cache will update that date thus allowing dependencies listening to
            // it to take some action. Usually this is reloading the cache on the PWS or DWS.
            CacheDependency.InvalidateCache(this.CACHE_DEPENDENCY_NAME);

            this.ReloadCache();
            this._activeCategoriesByStoreFront.Clear();
        }
        #endregion

    }
}
