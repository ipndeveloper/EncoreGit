using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Transactions;
using NetSteps.Addresses.Common.Models;
using NetSteps.Common.Base;
using NetSteps.Common.Exceptions;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Common.Utility;
using NetSteps.Core.Cache;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Encore.Core.IoC;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace NetSteps.Data.Entities.Repositories
{
    public partial class ProductRepository
    {
        private static readonly ICache<string, IList<int>> ExcludedShippingMethodIdCache = new MruLocalMemoryCache<string, IList<int>>("Product.ExcludedShippingMethodIdCache");
        #region Members
        protected override Func<NetStepsEntities, int, IQueryable<Product>> loadFullQuery
        {
            get
            {
                return CompiledQuery.Compile<NetStepsEntities, int, IQueryable<Product>>(
                        (context, productId) => context.Products
                                                .Include("ProductBase")
                                                .Include("ProductBase.Translations")
                                                .Include("ProductBase.Categories")
                                                .Include("ProductBase.Categories.Translations")
                                                .Include("ProductBase.ProductBaseProperties")
                                                .Include("ProductBase.ProductBaseProperties.ProductPropertyType")
                                                .Include("ProductBase.ProductBaseProperties.ProductPropertyType.ProductPropertyValues")
                                                .Include("Prices")
                                                .Include("Files")
                                                .Include("Properties")
                                                .Include("CatalogItems")
                                                .Include("CatalogItems.Catalog")
                                                .Include("CatalogItems.Catalog.Translations")
                                                .Include("CatalogItems.Catalog.StoreFronts")
                                                .Include("WarehouseProducts")
                                                .Include("ChildProductRelations")
                                                .Include("ParentProductRelations")
                                                .Include("DynamicKits")
                                                .Include("DynamicKits.DynamicKitGroups")
                                                .Include("DynamicKits.DynamicKitGroups.Translations")
                                                .Include("DynamicKits.DynamicKitGroups.DynamicKitGroupRules")
                                                .Include("Translations")
                                                .Include("ProductVariantInfo")
                                                .Include("ExcludedShippingMethods")
                                                .Where(p => p.ProductID == productId)
                                                );
            }
        }



        protected override Func<NetStepsEntities, IQueryable<Product>> loadAllFullQuery
        {
            get
            {
                return CompiledQuery.Compile<NetStepsEntities, IQueryable<Product>>(
                 (context) => from p in context.Products
                                        .Include("ProductBase")
                                        .Include("ProductBase.Translations")
                                        .Include("ProductBase.Categories")
                                        .Include("ProductBase.Categories.Translations")
                                        .Include("ProductBase.ProductBaseProperties")
                                        .Include("ProductBase.ProductBaseProperties.ProductPropertyType")
                                        .Include("ProductBase.ProductBaseProperties.ProductPropertyType.ProductPropertyValues")
                                        .Include("Prices")
                                        .Include("Files")
                                        .Include("Properties")
                                        .Include("Properties.ProductPropertyValue")
                                        .Include("CatalogItems")
                                        .Include("CatalogItems.Catalog")
                                        .Include("CatalogItems.Catalog.Translations")
                                        .Include("CatalogItems.Catalog.StoreFronts")
                                        .Include("WarehouseProducts")
                                        .Include("ChildProductRelations")
                                        .Include("ParentProductRelations")
                                        .Include("DynamicKits")
                                        .Include("DynamicKits.DynamicKitGroups")
                                        .Include("DynamicKits.DynamicKitGroups.Translations")
                                        .Include("DynamicKits.DynamicKitGroups.DynamicKitGroupRules")
                                        .Include("Translations")
                                        .Include("ProductVariantInfo")
                                        .Include("ExcludedShippingMethods")
                              //.Include("ProductTaxCategories")
                              select p);
            }
        }

        protected Func<NetStepsEntities, int, int, int, IQueryable<ProductPrice>> SelectProductPrice
        {
            get
            {
                return CompiledQuery.Compile<NetStepsEntities, int, int, int, IQueryable<ProductPrice>>(
                    (context, productID, currencyID, productPriceTypeID) => from pp in context.ProductPrices
                                                                            where pp.ProductID == productID && pp.CurrencyID == currencyID && pp.ProductPriceTypeID == productPriceTypeID
                                                                            select pp);
            }
        }

        protected Func<NetStepsEntities, int, int, IQueryable<ProductPrice>> SelectProductPrices
        {
            get
            {
                return CompiledQuery.Compile<NetStepsEntities, int, int, IQueryable<ProductPrice>>(
                    (context, productID, currencyID) => from pp in context.ProductPrices
                                                        where pp.ProductID == productID && pp.CurrencyID == currencyID
                                                        select pp);
            }
        }
        #endregion

        public Product Load(string productNumber)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
             {
                 using (NetStepsEntities context = CreateContext())
                 {
                     return loadAllFullQuery(context).FirstOrDefault(p => p.ProductNumber == productNumber);
                 }
             });
        }

        public Product LoadOne(int productId)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    return loadFullQuery(context, productId).FirstOrDefault();
                }
            });
        }

        public decimal GetProductPrice(int productID, int currencyID, int productPriceTypeID)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var loadedObj = SelectProductPrice(context, productID, currencyID, productPriceTypeID).FirstOrDefault();
                    if (loadedObj != null)
                        return loadedObj.Price;
                    else
                        throw new NetStepsDataException(string.Format("Error getting ProductPrice for ProductID: {0}, CurrencyID: {1}, ProductPriceTypeID: {2}.", productID, currencyID, productPriceTypeID));
                }
            });
        }

        public List<ProductPrice> GetProductPrices(int productID, int currencyID)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var loadedObj = SelectProductPrices(context, productID, currencyID).ToList();
                    return loadedObj;
                }
            });
        }

        public Product LoadWithRelations(int productID)
        {
            NetStepsEntities context = new NetStepsEntities();
            return context.Products.Include("ChildProductRelations").Include("ParentProductRelations").FirstOrDefault(p => p.ProductID == productID);
        }

        // TODO: Create a Transactional overridden/new save method for Product to save a Product and ProductBase in one transaction - JHE


        /// <summary>
        /// This method is an overridden Delete method to provide a Transaction delete of a Product and it's 
        /// ProductBase along with translations. - JHE 
        /// TODO: Test these new overridden Transactional deletes methods - JHE
        /// </summary>
        /// <param name="obj"></param>
        //public override void Delete(Product obj)
        //{
        //    ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
        //    {
        //        using (NetStepsEntities context = CreateContext())
        //        {
        //            obj.Translations.RemoveAllAndMarkAsDeleted();
        //            obj.MarkAsDeleted();

        //            Save(obj, context);
        //        }
        //    });
        //}

        /// <summary>
        /// This method is an overridden Delete method to provide a Transaction delete of a Product
        /// along with all translations. - JHE 
        /// TODO: Test these new overridden Transactional deletes methods - JHE
        /// </summary>
        /// <param name="obj"></param>
        public override void Delete(Product obj)
        {
            Delete(obj.ProductID);
        }

        public override void Delete(int primaryKey)
        {
            ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                using (TransactionScope transaction = new TransactionScope()) // TODO: Test this new Transaction Code - JHE
                {
                    var obj = context.Products.Include("Translations")
                                                .Include("Prices")
                                                .Include("Properties")
                                                .Include("CatalogItems")
                                                .Include("WarehouseProducts")
                                                .Include("ChildProductRelations")
                                                .Include("ParentProductRelations")
                                                .Include("AutoshipScheduleProducts")
                                                .Include("PromotionProducts")
                                                .Include("TaxCategories")
                                                .Include("Files")
                                                .Include("DynamicKits")
                                                .Include("DynamicKits.DynamicKitGroups")
                                                .Include("DynamicKits.DynamicKitGroups.Translations")
                                                .Include("DynamicKits.DynamicKitGroups.DynamicKitGroupRules")
                                                .Include("ProductVariantInfo")
                                                .FirstOrDefault(p => p.ProductID == primaryKey);

                    if (obj == default(Product))
                        return;

                    obj.StartEntityTracking();

                    // Linker Tables still need to use the Self-Tracking Entities to delete the object - JHE
                    obj.TaxCategories.RemoveAll();

                    context.DeleteObjects(obj.Translations);
                    context.DeleteObjects(obj.Prices);
                    context.DeleteObjects(obj.Properties);
                    context.DeleteObjects(obj.CatalogItems);
                    context.DeleteObjects(obj.WarehouseProducts);
                    context.DeleteObjects(obj.ChildProductRelations);
                    context.DeleteObjects(obj.ParentProductRelations);
                    context.DeleteObjects(obj.AutoshipScheduleProducts);

                    foreach (var file in obj.Files)
                    {
                        try
                        {
                            System.IO.File.Delete(file.FilePath.ReplaceFileUploadPathToken().WebUploadPathToAbsoluteUploadPath());
                        }
                        catch { }
                    }

                    context.DeleteObjects(obj.Files);

                    foreach (var kit in obj.DynamicKits)
                    {
                        foreach (var group in kit.DynamicKitGroups)
                        {
                            context.DeleteObjects(group.DynamicKitGroupRules);
                            context.DeleteObjects(group.Translations);
                        }

                        context.DeleteObjects(kit.DynamicKitGroups);
                    }

                    context.DeleteObjects(obj.DynamicKits);

                    context.DeleteObject(obj);

                    Save(obj, context);

                    transaction.Complete();
                }
            });
        }

        public void ChangeActiveStatus(int productID, bool active)
        {
            ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    context.ExecuteStoreCommand("UPDATE Products SET Active = @p0 WHERE ProductID = @p1", active, productID);
                }
            });
        }

        public List<Product> GetVariants(int productID)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (var context = CreateContext())
                {
                    var product = context.Products.Single(p => p.ProductID == productID);
                    return context.Products.Include("Translations").Include("Properties").Include("ProductBase").Include("ProductBase.ProductBaseProperties")
                                                .Include("ProductBase.ProductBaseProperties.ProductPropertyType")
                                                .Include("ProductBase.ProductBaseProperties.ProductPropertyType.ProductPropertyValues")
                                                .Include("ProductVariantInfo")
                                                .Include("Prices")
                        .Where(p => p.ProductBaseID == product.ProductBaseID && !p.IsVariantTemplate).ToList();
                }
            });
        }

        public List<Product> Search(string query)
        {
            List<Product> ProductList = null;
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    context.SetReadUncommitted();
                    ProductList = context.Products
                        .Where(p => p.SKU.Contains(query) || p.Translations.Any(t => t.Name.Contains(query) || t.ShortDescription.Contains(query) || t.LongDescription.Contains(query))).Take(10).ToList();

                    return ProductList;
                }
            });
        }

        public List<Product> SearchProductForOrder(string query, int? pageSize = 100)
        {
            List<Product> results;
            //var matchingProducts = __productSearchCache.Get(Tuple.Create(storeFrontId, query, accountTypeId, catalogsToSearch, startsWith, includeDynamicKits, productPriceTypeID));
            using (NetStepsEntities context = CreateContext())
            {
                context.SetReadUncommitted();
                var products = context.Products.
                                Where(p => p.Active && (p.SKU.Contains(query) ||
                                p.Translations.Any(t => t.Name.Contains(query) || 
                                t.ShortDescription.Contains(query) || 
                                t.LongDescription.Contains(query))));

                if (pageSize.HasValue && pageSize.Value > 0)
                    products = products.Take(pageSize.Value);

                var matchingProducts = products.Select(p => p.ProductID).ToList();

                if (matchingProducts.Any()) results = ProductCache.GetProductsByIds(matchingProducts).ToList();
                else results = new List<Product>();
            }
            return results;
        }

        public PaginatedList<Product> Search(FilterPaginatedListParameters<Product> searchParams)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    PaginatedList<Product> results = new PaginatedList<Product>(searchParams);

                    IQueryable<Product> products = context.Products.Include("Translations").Include("Properties").Include("ProductBase").Include("ProductBase.ProductBaseProperties")
                                                .Include("ProductBase.ProductBaseProperties.ProductPropertyType")
                                                .Include("ProductBase.ProductBaseProperties.ProductPropertyType.ProductPropertyValues");

                    if (searchParams.WhereClause != null)
                        products = products.Where(searchParams.WhereClause);

                    results.TotalCount = products.Count();

                    products = products.ApplyOrderByFilter(searchParams, context);

                    products = products.ApplyPagination(searchParams);

                    results.AddRange(products.ToList());

                    return results;
                }
            });
        }

        public List<ProductSlimSearchData> SlimSearch(string query, int? pageSize = 1000)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    context.SetReadUncommitted();
                    var products = context.Products
                        .Where(p => p.SKU.Contains(query)
                            || p.Translations.Any(t => t.Name.Contains(query) || t.ShortDescription.Contains(query) || t.LongDescription.Contains(query)));

                    if (pageSize.HasValue && pageSize.Value > 0)
                        products = products.Take(pageSize.Value);

                    /*var pros =*/
                    return products.Select(p => new ProductSlimSearchData()
                    {
                        ProductID = p.ProductID,
                        SKU = p.SKU,
                        Name = !p.Translations.Any(t => t.LanguageID == ApplicationContext.Instance.CurrentLanguageID) ? p.Translations.Any() ? p.Translations.FirstOrDefault().Name : "" : p.Translations.FirstOrDefault(t => t.LanguageID == ApplicationContext.Instance.CurrentLanguageID).Name,
                        ProductBaseID = p.ProductBaseID,
                        IsVariant = p.ProductBase.Products.Count > 1 && !p.IsVariantTemplate,
                        IsVariantTemplate = p.IsVariantTemplate,
                        ProductBaseHasProperties = p.ProductBase.ProductBaseProperties.Count > 0
                    })
                        .ToList();
                    //return pros.ToDictionary(p => p.ProductID, p => p.SKU + " - " + p.Name);
                }
            });
        }

        public List<Product> LoadAllFullByStorefront(int storefrontID)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                //set it to read uncommited
                //create the transaction scope, passing our options in
                using (NetStepsEntities context = CreateContext())
                {
                    context.CommandTimeout = (int?)120;
                    context.SetReadUncommitted();

                    var products = loadAllFullQuery(context).Where(p => p.CatalogItems.Any(ci => ci.Catalog.StoreFronts.Any(sf => sf.StoreFrontID == storefrontID))).ToList();

                    context.CommandTimeout = null;
                    return products;
                }

            });
        }

        public Product LoadFullBySKU(string sku)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {

                using (
                    NetStepsEntities context =
                        CreateContext())
                {
                    context.SetReadUncommitted();
                    return loadAllFullQuery(context).FirstOrDefault(x => x.SKU == sku);
                }
            });
        }

        //public override Product LoadFull(int productID)
        //{
        //    return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
        //    {
        //        using (
        //            NetStepsEntities context =
        //                CreateContext())
        //        {
        //            context.SetReadUncommitted();
        //            return loadAllFullQuery(context).FirstOrDefault(x => x.ProductID == productID);
        //        }
        //    });
        //}

        public List<Product> LoadAllFullExcept(IEnumerable<int> productIDs)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                // http://stackoverflow.com/questions/926656/entity-framework-with-nolock - Read without lock - JHE
                //declare the transaction options
                //create the transaction scope, passing our options in
                using (NetStepsEntities context = CreateContext())
                {
                    context.SetReadUncommitted();
                    var distinctIDs = productIDs.Distinct().ToList();
                    var productIDsToLoad = context.Products.Where(p => !distinctIDs.Contains(p.ProductID)).Select(p => p.ProductID).ToList();

                    // NOTE: If the distinctIDs is greater than 1500 items, split into multiple queries so SQL Server can handle them - JHE
                    List<List<int>> setsOfItemsToLoad = new List<List<int>>();
                    if (productIDsToLoad.Count > 1500)
                        setsOfItemsToLoad = productIDsToLoad.SplitList(1500);
                    else
                        setsOfItemsToLoad.Add(productIDsToLoad);

                    List<Product> products = new List<Product>();

                    ParallelOptions options = new ParallelOptions();
                    Parallel.ForEach(setsOfItemsToLoad, options, setsOfItems =>
                    {
                        using (NetStepsEntities context2 = CreateContext())
                        {
                            // NOTE: Doing a SQL 'IN' instead of a SQL 'NOT IN' for a 6X performance boost - JHE
                            var results = loadAllFullQuery(context2).Where(p => setsOfItems.Contains(p.ProductID));

                            // NOTE: Using the less performant SQL 'NOT IN' version because SQL Server itself runs out of memory processing the SQL 'IN' version above - JHE
                            //var results = loadAllFullQuery(context).Where(p => !distinctIDs.Contains(p.ProductID));

                            //var resultsSql = results.ToTraceString();

                            products.AddRange(results.ToList());
                        }
                    });

                    return products;
                }

            });
        }

        public List<ProductSlimSearchData> LoadAllSlim(FilterPaginatedListParameters<Product> searchParams)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    IQueryable<Product> products = context.Products;

                    if (searchParams.WhereClause != null)
                    {
                        products = products.Where(searchParams.WhereClause);
                    }

                    return products.Select(p => new ProductSlimSearchData()
                    {
                        ProductID = p.ProductID,
                        SKU = p.SKU,
                        Name = !p.Translations.Any(t => t.LanguageID == ApplicationContext.Instance.CurrentLanguageID) ? p.Translations.Any() ? p.Translations.FirstOrDefault().Name : "" : p.Translations.FirstOrDefault(t => t.LanguageID == ApplicationContext.Instance.CurrentLanguageID).Name,
                        ProductBaseID = p.ProductBaseID,
                        IsVariant = p.ProductBase.Products.Count > 1 && !p.IsVariantTemplate,
                        IsVariantTemplate = p.IsVariantTemplate,
                    }).ToList();
                }
            });
        }

        public override void Save(Product product)
        {
            if (product.ContainsUnmodifiedDuplicateEntitiesInObjectGraph<StoreFront>())
            {
                var dup = product.Clone();
                dup.RemoveUnmodifiedDuplicateEntitiesInObjectGraph<StoreFront>();
                base.Save(dup);
                product.AcceptEntityChanges();
                // TODO: The IDs created by database Identities need to be set back to the 'product' object from the dup object to avoid a problem if the same object is modified and saved again. - JHE
            }
            else
            {
                base.Save(product);
            }
        }

        public List<int> GetOutOfStockProductIDs(IAddress address = null)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    if (address == null || address.IsEmpty() || !address.StateProvinceID.IsPositive())
                        return context.Products.Where(p => p.WarehouseProducts.All(wp => wp.IsAvailable && (wp.QuantityOnHand <= (wp.QuantityBuffer + wp.QuantityAllocated) && wp.Product.Active)) && p.ProductBase.IsShippable).Select(p => p.ProductID).ToList();

                    Warehouse warehouse = WarehouseProductRepository.GetWarehouse(context, address);

                    return context.WarehouseProducts.Where(wp => wp.WarehouseID == warehouse.WarehouseID && wp.IsAvailable && wp.QuantityOnHand <= (wp.QuantityBuffer + wp.QuantityAllocated) && wp.Product.Active && wp.Product.ProductBase.IsShippable).Select(p => p.ProductID).ToList();
                }
            });
        }

        public InventoryLevels CheckStock(int productId, IAddress address = null)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var inventory = Create.New<InventoryBaseRepository>();

                    InventoryLevels result = new InventoryLevels() { IsOutOfStock = false, QuantityAvailable = null };
                    Product product = null;
                    try
                    {
                        product = inventory.GetProduct(productId);
                    }
                    catch (Exception)
                    {
                        throw new NetStepsException("Product could not be found.")
                        {
                            PublicMessage = Translation.GetTerm("ProductNotFound", "Product could not be found.")
                        };
                    }

                    //non shippable product is never out of stock
                    if (!product.ProductBase.IsShippable)
                    {
                        return result;
                    }

                    if (product.IsStaticKit())
                    {
                        return result;
                    }

                    if (address == null || address.IsEmpty() || !address.StateProvinceID.IsPositive())
                    {
                        if (address != null && address.CountryID.IsPositive())
                            result.QuantityAvailable = context.WarehouseProducts.Where(wp => wp.ProductID == productId && wp.IsAvailable && wp.Product.Active
                                && wp.Warehouse.ShippingRegions.Any(sr => sr.StateProvinces.Any(sp => sp.CountryID == address.CountryID)))
                                .Select(wp => wp.QuantityOnHand - wp.QuantityBuffer - wp.QuantityAllocated).ToList()
                                .Sum(i => Math.Max(0L, i));
                        else
                            result.QuantityAvailable = context.WarehouseProducts.Where(wp => wp.ProductID == productId && wp.IsAvailable && wp.Product.Active)
                                .Select(wp => wp.QuantityOnHand - wp.QuantityBuffer - wp.QuantityAllocated).ToList()
                                .Sum(i => Math.Max(0L, i));
                    }
                    else
                    {
                        Warehouse warehouse = WarehouseProductRepository.GetWarehouse(context, address, new List<int> { productId });
                        var warehouseProduct = context.WarehouseProducts.FirstOrDefault(wp => wp.IsAvailable && wp.WarehouseID == warehouse.WarehouseID && wp.ProductID == productId && wp.Product.Active);
                        if (warehouseProduct == null)
                        {
                            throw new NetStepsException(string.Format("No WarehouseProducts for WarehouseID: {0}, ProductID {1}", warehouse.WarehouseID, productId))
                            {
                                PublicMessage = Translation.GetTerm("ProductUnavailable", "Product is unavailable")
                            };
                        }

                        result.QuantityAvailable = warehouseProduct.QuantityOnHand - warehouseProduct.QuantityBuffer - warehouseProduct.QuantityAllocated;
                    }

                    result.IsOutOfStock = result.QuantityAvailable <= 0;
                    return result;
                }
            });
        }

        public override PaginatedList<AuditLogRow> GetAuditLog(int primaryKey, AuditLogSearchParameters searchParameters)
        {
            ValidatePrimaryKeyForLoad(primaryKey);

            var product = Product.LoadFull(primaryKey);

            return GetAuditLog(product, searchParameters);
        }
        public virtual PaginatedList<AuditLogRow> GetAuditLog(Product fullyLoadedProduct, AuditLogSearchParameters searchParameters)
        {
            List<AuditTableValueItem> list = new List<AuditTableValueItem>();
            list.Add(new AuditTableValueItem()
            {
                TableName = EntitySetName,
                PrimaryKey = Convert.ToInt32(fullyLoadedProduct.ProductID)
            });

            list.Add(new AuditTableValueItem()
            {
                TableName = "ProductBases",
                PrimaryKey = Convert.ToInt32(fullyLoadedProduct.ProductBaseID)
            });

            if (fullyLoadedProduct != null && fullyLoadedProduct.ProductID > 0)
            {
                if (fullyLoadedProduct.Prices != null)
                {
                    foreach (var productPrice in fullyLoadedProduct.Prices)
                    {
                        list.Add(new AuditTableValueItem()
                        {
                            TableName = "ProductPrices",
                            PrimaryKey = productPrice.ProductPriceID
                        });
                    }
                }

                if (fullyLoadedProduct.Files != null)
                {
                    foreach (var productFile in fullyLoadedProduct.Files)
                    {
                        list.Add(new AuditTableValueItem()
                        {
                            TableName = "ProductFiles",
                            PrimaryKey = productFile.ProductFileID
                        });
                    }
                }
            }

            return GetAuditLog(list, searchParameters);
        }

        //TODO: Move the caching on this method to the cache layer
        public IList<int> GetExcludedShippingMethodIds(int productId)
        {
            IList<int> result;
            string key = String.Concat(productId);
            if (!ExcludedShippingMethodIdCache.TryGet(key, out result))
            {
                ExcludedShippingMethodIdCache.TryAdd(key, result = ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
                {
                    using (NetStepsEntities context = CreateContext())
                    {
                        return context.Products.Where(p => p.ProductID == productId).SelectMany(p => p.ExcludedShippingMethods.Select(sm => sm.ShippingMethodID)).ToList();
                    }
                }));
            }
            return result ?? Enumerable.Empty<int>().ToList();
        }

        //TODO: Move the caching on this method to the cache layer
        public IEnumerable<int> GetExcludedShippingMethodIds(IEnumerable<int?> productIds)
        {
            IList<int> result;
            GetExcludedShippingMethodIds(productIds.Where(p => p.HasValue).Select(s => s.Value), out result);
            return result;
        }

        //TODO: Move the caching on this method to the cache layer
        public IList<int> GetExcludedShippingMethodIds(
             IEnumerable<int> productIds,
             out IList<int> productIdsWithExclusions)
        {
            productIdsWithExclusions = new List<int>();
            // Short-circuit
            if (!productIds.Any())
            {
                return new List<int>();
            }
            string key = String.Join("|", productIds);
            IList<int> result;
            if (!ExcludedShippingMethodIdCache.TryGet(key, out result))
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var exclusions = context.Products
                        .Where(p => productIds.Contains(p.ProductID))
                        .SelectMany(p => p.ExcludedShippingMethods.Select(esm => new
                        {
                            p.ProductID,
                            esm.ShippingMethodID
                        }))
                        .ToList();

                    productIdsWithExclusions = exclusions
                        .Select(x => x.ProductID)
                        .Distinct()
                        .ToList();

                    ExcludedShippingMethodIdCache.TryAdd(key, result = exclusions
                    .Select(x => x.ShippingMethodID)
                    .Distinct()
                        .ToList());
                }
            }
            return result;
        }

        public int GetBaseProductId(int productId)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    return context.Products.Where(p => p.ProductID == productId).Select(p => p.ProductBaseID).FirstOrDefault();
                }
            });
        }

        public IEnumerable<Tuple<int, int>> GetBaseProductIds(IEnumerable<int> productIds)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    return context.Products.Where(p => productIds.Contains(p.ProductID))
                        .Select(p => new { pid = p.ProductID, bpid = p.ProductBaseID })
                        .Each(s => Tuple.Create(s.pid, s.bpid));
                }
            });
        }

        public int GetProductIdBySKU(string sku)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    return context.Products.Where(p => p.SKU == sku).Select(p => p.ProductID).FirstOrDefault();
                }
            });
        }

        public IEnumerable<int> GetActiveProductIdsForStoreFront(int storeFrontId)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                IEnumerable<int> results = new int[] { };
                using (NetStepsEntities context = CreateContext())
                {
                    var ids = from storefront in context.StoreFronts
                              from catalog in storefront.Catalogs
                              from catalogitem in catalog.CatalogItems
                              where storefront.StoreFrontID == storeFrontId
                               && catalogitem.Active
                               && (!catalogitem.StartDateUTC.HasValue || DateTime.UtcNow >= catalogitem.StartDateUTC)
                               && (!catalogitem.EndDateUTC.HasValue || DateTime.UtcNow <= catalogitem.EndDateUTC)
                              select catalogitem.ProductID;

                    results = ids.Distinct().Sort().ToArray();
                }
                return results;
            });
        }

        public IEnumerable<int> GetActiveProductIdsForStoreFrontAndCategory(int storeFrontId, int categoryId, short? accountTypeId)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                IEnumerable<int> results = new int[] { };
                using (NetStepsEntities context = CreateContext())
                {
                    if (accountTypeId.HasValue)
                    {
                        var ids = from storefront in context.StoreFronts
                                  from catalog in storefront.Catalogs
                                  from catalogitem in catalog.CatalogItems
                                  join product in context.Products on catalogitem.ProductID equals product.ProductID
                                  from price in product.Prices
                                  join pricetype in context.ProductPriceTypes on price.ProductPriceTypeID equals pricetype.ProductPriceTypeID
                                  from accounttype in pricetype.AccountPriceTypes
                                  join prodbase in context.ProductBases on product.ProductBaseID equals prodbase.ProductBaseID
                                  from category in prodbase.Categories
                                  where storefront.StoreFrontID == storeFrontId
                                   && catalogitem.Active
                                   && (!catalogitem.StartDateUTC.HasValue || DateTime.UtcNow >= catalogitem.StartDateUTC)
                                   && (!catalogitem.EndDateUTC.HasValue || DateTime.UtcNow <= catalogitem.EndDateUTC)
                                   && category.CategoryID == categoryId
                                   && accounttype.AccountTypeID == accountTypeId
                                  select catalogitem.ProductID;

                        results = ids.Distinct().Sort().ToArray();
                    }
                    else
                    {
                        var ids = from storefront in context.StoreFronts
                                  from catalog in storefront.Catalogs
                                  from catalogitem in catalog.CatalogItems
                                  join product in context.Products on catalogitem.ProductID equals product.ProductID
                                  join prodbase in context.ProductBases on product.ProductBaseID equals prodbase.ProductBaseID
                                  from category in prodbase.Categories
                                  where storefront.StoreFrontID == storeFrontId
                                   && catalogitem.Active
                                   && (!catalogitem.StartDateUTC.HasValue || DateTime.UtcNow >= catalogitem.StartDateUTC)
                                   && (!catalogitem.EndDateUTC.HasValue || DateTime.UtcNow <= catalogitem.EndDateUTC)
                                   && category.CategoryID == categoryId
                                  select catalogitem.ProductID;

                        results = ids.Distinct().Sort().ToArray();
                    }
                }
                return results;
            });
        }

        #region Load Helpers

        public override Product LoadFull(int ProductID)
        {
            var Product = FirstOrDefaultFull(x => x.ProductID == ProductID);

            if (Product == null)
            {
                throw new NetStepsDataException(string.Format("No Product found with ProductID = {0}.", ProductID));
            }

            return Product;
        }

        public override List<Product> LoadBatchFull(IEnumerable<int> ProductIDs)
        {
            return WhereFull(x => ProductIDs.Contains(x.ProductID));
        }

        public override List<Product> LoadAllFull()
        {
            return WhereFull(x => true);
        }

        public virtual Product FirstOrDefaultFull(Expression<Func<Product, bool>> predicate)
        {
            Contract.Requires<ArgumentNullException>(predicate != null);

            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    return FirstOrDefaultFull(predicate, context);
                }
            });
        }

        public virtual Product FirstOrDefaultFull(Expression<Func<Product, bool>> predicate, NetStepsEntities context)
        {
            Contract.Requires<ArgumentNullException>(predicate != null);
            Contract.Requires<ArgumentNullException>(context != null);

            return FirstOrDefault(predicate, Product.Relations.LoadFull, context);
        }

        public virtual Product FirstOrDefault(Expression<Func<Product, bool>> predicate, Product.Relations relations)
        {
            Contract.Requires<ArgumentNullException>(predicate != null);

            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    return FirstOrDefault(predicate, relations, context);
                }
            });
        }

        public virtual Product FirstOrDefault(Expression<Func<Product, bool>> predicate, Product.Relations relations, NetStepsEntities context)
        {
            Contract.Requires<ArgumentNullException>(predicate != null);
            Contract.Requires<ArgumentNullException>(context != null);

            var Product = context.Products
                .FirstOrDefault(predicate);

            if (Product == null)
            {
                return null;
            }

            Product.LoadRelations(context, relations);

            return Product;
        }

        public virtual List<Product> WhereFull(Expression<Func<Product, bool>> predicate)
        {
            Contract.Requires<ArgumentNullException>(predicate != null);

            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    return WhereFull(predicate, context);
                }
            });
        }

        public virtual List<Product> WhereFull(Expression<Func<Product, bool>> predicate, NetStepsEntities context)
        {
            Contract.Requires<ArgumentNullException>(predicate != null);
            Contract.Requires<ArgumentNullException>(context != null);

            return Where(predicate, Product.Relations.LoadFull, context);
        }

        public virtual List<Product> Where(Expression<Func<Product, bool>> predicate, Product.Relations relations)
        {
            Contract.Requires<ArgumentNullException>(predicate != null);

            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    return Where(predicate, relations, context);
                }
            });
        }

        public virtual List<Product> Where(Expression<Func<Product, bool>> predicate, Product.Relations relations, NetStepsEntities context)
        {
            Contract.Requires<ArgumentNullException>(predicate != null);
            Contract.Requires<ArgumentNullException>(context != null);

            var Products = context.Products
                .Where(predicate)
                .ToList();

            Products.LoadRelations(context, relations);

            return Products;
        }
        #endregion


        public static PaginatedList<ProductCreditBalanceSearchData> BrowseProductCreditLedger(ProductCreditLedgerParameters searchParams)
        {

            var rules = ProductRepository.BrowseProductCreditLedgerLis(searchParams);
            IQueryable<ProductCreditBalanceSearchData> matchingItems = rules.AsQueryable<ProductCreditBalanceSearchData>();

            var resultTotalCount = matchingItems.Count();
            matchingItems = matchingItems.ApplyPagination(searchParams);
            return matchingItems.ToPaginatedList<ProductCreditBalanceSearchData>(searchParams, resultTotalCount);
        }

        public static List<ProductCreditBalanceSearchData> BrowseProductCreditLedgerLis(ProductCreditLedgerParameters searchParams)
        {

            List<ProductCreditBalanceSearchData> result = new List<ProductCreditBalanceSearchData>();
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {  
                { "@AccountID",searchParams.AccountID },
                { "@pcMax",searchParams.CreditBalanceMax } ,
                { "@pcMin",searchParams.CreditBalanceMin } ,
                { "@FechaIni",searchParams.EntryDateFrom } ,
                { "@FechaFin",searchParams.EntryDateTo } ,
                { "@State",searchParams.State } 
                
                };

                SqlDataReader reader = DataAccess.GetDataReader("upsProductCreditBalance", parameters, "Commissions");

                if (reader.HasRows)
                {
                    result = new List<ProductCreditBalanceSearchData>();
                    while (reader.Read())
                    {
                        result.Add(new ProductCreditBalanceSearchData()
                        {
                            BA_ID = Convert.ToString(reader["BA_ID"]),
                            Name = Convert.ToString(reader["Name"]),
                            EffectiveDate = Convert.ToString(reader["EffectiveDate"]),
                            EntryDescription = Convert.ToString(reader["EntryDescription"]),
                            EntryReasonName = Convert.ToString(reader["EntryReasonName"]),
                            EntryOriginName = Convert.ToString(reader["EntryOriginName"]),
                            EntryTypeName = Convert.ToString(reader["EntryTypeName"]),
                            Credit_Balance = Convert.ToString(reader["Credit_Balance"]),
                            EndingBalance = Convert.ToString(reader["EndingBalance"]),
                            Ticket = Convert.ToString(reader["Ticket"]),
                            Order = Convert.ToString(reader["Order"]),                          
                            Soporte = Convert.ToString(reader["Soporte"])
                            

                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
            return result;
        }

        public List<int> SearchProductsBySAPSKU(List<ProductBase> ProductsList, string SAPSKU)
        {
            try
            {
                List<int> ProductsListNew = new List<int>();

                DataTable dtProductBaseIDs = new DataTable();
                dtProductBaseIDs.Columns.Add("OrderID");

                foreach (var product in ProductsList)
                {
                    dtProductBaseIDs.Rows.Add(product.ProductBaseID);
                }

                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[uspSearchProductsBySAPSKU]";
                    cmd.Parameters.AddWithValue("@ProductBaseIDs", dtProductBaseIDs).SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@SAPSKU", SAPSKU);

                    cmd.ExecuteNonQuery();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                ProductsListNew.Add(Convert.ToInt32(reader["ProductBaseID"]));
                            }
                        }
                    }
                }

                return ProductsListNew;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
    }
}
