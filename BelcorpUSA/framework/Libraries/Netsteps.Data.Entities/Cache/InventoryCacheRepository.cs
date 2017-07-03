using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using NetSteps.Common.Extensions;
using NetSteps.Core.Cache;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Encore.Core.IoC;
using NetSteps.Diagnostics.Utilities;

namespace NetSteps.Data.Entities.Cache
{
	/// <summary>
	/// This class is for encore customers with product count less than 50,000.
	/// </summary>
	internal class InventoryCacheRepository : InventoryBaseRepository
	{
		#region Fields

		private ICache<int, Category> __categoryByIdCache;
		private ICache<int, IEnumerable<Catalog>> __storeFrontCatalogsCache;
		private ICache<int, IEnumerable<int>> __storeFrontProductIdCache;
		private ICache<Tuple<int, short?>, IEnumerable<int>> __activeCategoryIdsByStorefrontAndAccountTypeCache;
		private ICache<Tuple<int, int, short?>, IEnumerable<int>> __activeProductIdsByStorefrontAndCategoryCache;
		private ICache<Tuple<int, short?, int?>, IEnumerable<int>> __activeProductIdsByCatalogCache;
		private ICache<Tuple<int, short?, int?>, IEnumerable<int>> __activeDynKitProductIdsByCatalogCache;
		private ICache<Tuple<int, string, short?, IEnumerable<int>, string, bool, int?>, IEnumerable<int>> __productSearchCache;
		private ICache<Tuple<int, short?, IEnumerable<int>, bool, int?>, IEnumerable<ProductSearchResult>> __productSearchSubCache;

		#endregion

		#region Constructors

		public InventoryCacheRepository()
		{
			__categoryByIdCache = new ActiveMruLocalMemoryCache<int, Category>("CategoryById", new DelegatedDemuxCacheItemResolver<int, Category>(CategoryByIdResolver));
			__storeFrontCatalogsCache = new ActiveMruLocalMemoryCache<int, IEnumerable<Catalog>>("StoreFrontCatalogs", new DelegatedDemuxCacheItemResolver<int, IEnumerable<Catalog>>(CatalogsForStoreFrontIdResolver));
			__storeFrontProductIdCache = new ActiveMruLocalMemoryCache<int, IEnumerable<int>>("StoreFrontProductIds", new DelegatedDemuxCacheItemResolver<int, IEnumerable<int>>(ProductIdsForStoreFrontIdResolver));
			__activeCategoryIdsByStorefrontAndAccountTypeCache = new ActiveMruLocalMemoryCache<Tuple<int, short?>, IEnumerable<int>>("ActiveCategoryIdsByStorefrontAndAccountType", new DelegatedDemuxCacheItemResolver<Tuple<int, short?>, IEnumerable<int>>(ActiveCategoryIdsByStorefrontAndAccountTypeResolver));
			__activeProductIdsByStorefrontAndCategoryCache = new ActiveMruLocalMemoryCache<Tuple<int, int, short?>, IEnumerable<int>>("ActiveProductIdsByStorefrontAndCategory", new DelegatedDemuxCacheItemResolver<Tuple<int, int, short?>, IEnumerable<int>>(ActiveProductIdsByStorefrontAndCategoryResolver));
			__activeProductIdsByCatalogCache = new ActiveMruLocalMemoryCache<Tuple<int, short?, int?>, IEnumerable<int>>("ActiveProductIdsByCatalog", new DelegatedDemuxCacheItemResolver<Tuple<int, short?, int?>, IEnumerable<int>>(ActiveProductIdsByCatalogResolver));
			__activeDynKitProductIdsByCatalogCache = new ActiveMruLocalMemoryCache<Tuple<int, short?, int?>, IEnumerable<int>>("ActiveDynKitProductIdsByCatalogResolver", new DelegatedDemuxCacheItemResolver<Tuple<int, short?, int?>, IEnumerable<int>>(ActiveDynKitProductIdsByCatalogResolver));
			__productSearchCache = new ActiveMruLocalMemoryCache<Tuple<int, string, short?, IEnumerable<int>, string, bool, int?>, IEnumerable<int>>("ProductIdsForSearchContext", new DelegatedDemuxCacheItemResolver<Tuple<int, string, short?, IEnumerable<int>, string, bool, int?>, IEnumerable<int>>(ProductIdsForSearchContexResolver));
			__productSearchSubCache = new ActiveMruLocalMemoryCache<Tuple<int, short?, IEnumerable<int>, bool, int?>, IEnumerable<ProductSearchResult>>("ProductIdsForSearchContexSubResolver", new DelegatedDemuxCacheItemResolver<Tuple<int, short?, IEnumerable<int>, bool, int?>, IEnumerable<ProductSearchResult>>(ProductIdsForSearchContexSubResolver));
		}

		#endregion

		#region Methods

		private bool CategoryByIdResolver(int key, out Category category)
		{
			bool result = false;
			ICategoryRepository categoryRepo = Create.New<ICategoryRepository>();
			category = categoryRepo.LoadFullTree(key);
			result = category != null;
			if (result)
				PushCategoryToCache(category, true);
			return result;
		}

		private void PushCategoryToCache(Category category, bool isRoot)
		{
			if (!isRoot) __categoryByIdCache.TryAdd(category.CategoryID, category);
			foreach (var item in category.ChildCategories)
				PushCategoryToCache(item, false);
		}

		private bool CatalogsForStoreFrontIdResolver(int key, out IEnumerable<Catalog> catalogs)
		{
			ICatalogRepository catalogRepo = Create.New<ICatalogRepository>();
			catalogs = catalogRepo.LoadActiveCatalogsForStoreFront(key);
			if (catalogs != null)
				catalogs = catalogs.ToArray(); //convert to simple enumerable for Cache
			return catalogs != null;
		}

		private bool ActiveCategoryIdsByStorefrontAndAccountTypeResolver(Tuple<int, short?> ids, out IEnumerable<int> categoryIds)
		{
			ICategoryRepository categoryRepo = Create.New<ICategoryRepository>();
			categoryIds = categoryRepo.GetActiveCategoryIdsByStoreFrontIdAndAccountTypeId(ids.Item1, ids.Item2);
			if (categoryIds != null)
				categoryIds = categoryIds.ToArray(); //convert to simple enumerable for Cache
			return categoryIds != null;
		}

		private bool ProductIdsForStoreFrontIdResolver(int storeFrontId, out IEnumerable<int> productIds)
		{
			IProductRepository productRepo = Create.New<IProductRepository>();
			productIds = productRepo.GetActiveProductIdsForStoreFront(storeFrontId);
			if (productIds != null)
				productIds = productIds.ToArray();//convert to simple enumerable for Cache
			return (productIds != null && productIds.Any());
		}

		private bool ActiveProductIdsByStorefrontAndCategoryResolver(Tuple<int, int, short?> ids, out IEnumerable<int> productIds)
		{
			IProductRepository prodRepo = Create.New<IProductRepository>();
			productIds = prodRepo.GetActiveProductIdsForStoreFrontAndCategory(ids.Item1, ids.Item2, ids.Item3);
			if (productIds != null)
				productIds = productIds.ToArray();//convert to simple enumerable for Cache
			return (productIds != null && productIds.Any());
		}

		private bool ActiveProductIdsByCatalogResolver(Tuple<int, short?, int?> ids, out IEnumerable<int> productIds)
		{
			ICatalogRepository catRepo = Create.New<ICatalogRepository>();
			productIds = catRepo.GetActiveCatalogProductIds(ids.Item1, ids.Item2, ids.Item3);
			if (productIds != null)
				productIds = productIds.ToArray();//convert to simple enumerable for Cache
			return (productIds != null && productIds.Any());
		}

		private bool ActiveDynKitProductIdsByCatalogResolver(Tuple<int, short?, int?> ids, out IEnumerable<int> productIds)
		{
			IEnumerable<int> activeProductIds;
			if (__activeProductIdsByCatalogCache.TryGet(ids, out activeProductIds) && activeProductIds.Any())
			{
				IDynamicKitRepository dkRepo = Create.New<IDynamicKitRepository>();
				var dynKitProdIds = dkRepo.WhereSelect(dk => activeProductIds.Contains(dk.ProductID), dk => dk.ProductID);
				IProductRepository prdRepo = Create.New<IProductRepository>();
				var actualProdIds = prdRepo.WhereSelect(p => dynKitProdIds.Contains(p.ProductID) && p.IsVariantTemplate == false, p => p.ProductID);
				if (actualProdIds != null && actualProdIds.Any())
				{
					productIds = actualProdIds;
				}
				else
				{
					productIds = Enumerable.Empty<int>();
				}
			}
			else
			{
				productIds = Enumerable.Empty<int>();
			}
			return true;
		}

		protected override void ReloadCache()
		{
			__activeCategoryIdsByStorefrontAndAccountTypeCache.FlushAll();
			__categoryByIdCache.FlushAll();
			__storeFrontCatalogsCache.FlushAll();
		}

		protected override Catalog DoGetCatalogByName(string name)
		{
			ICatalogRepository catalogRepo = Create.New<ICatalogRepository>();
			return catalogRepo.GetCatalogByName(name);
		}

		protected override List<Product> DoGetAllActiveProducts(int storeFrontId, short? accountTypeId = null, int? productPriceTypeID = null)
		{
			List<Product> results = null;

			var activeCatalogs = GetActiveCatalogs(storeFrontId);

			if (activeCatalogs != null && activeCatalogs.Any())
			{
				var cResults = new System.Collections.Concurrent.ConcurrentBag<Product>();

				Parallel.ForEach(activeCatalogs, ac =>
					{
						var catalogProducts = GetActiveCatalogProducts(ac.CatalogID, accountTypeId, productPriceTypeID);
						catalogProducts.ForEach(p => cResults.Add(p));
					}
				);

				results = cResults.DistinctBy(p => p.ProductID).ToList();
			}
			else
			{
				results = new List<Product>();
			}

			return results;
		}

		protected override IEnumerable<Product> DoGetDynamicKitProducts(int storeFrontId, bool sort, bool sortDescending)
		{
			List<Product> results = null;

			var activeCatalogs = GetActiveCatalogs(storeFrontId);

			if (activeCatalogs != null && activeCatalogs.Any())
			{
				var cResults = new System.Collections.Concurrent.ConcurrentBag<Product>();

				Parallel.ForEach(activeCatalogs, ac =>
					{
						IEnumerable<int> productIds;
						if (__activeDynKitProductIdsByCatalogCache.TryGet(Tuple.Create<int, short?, int?>(ac.CatalogID, null, null), out productIds) && productIds.Any())
						{
							foreach (var item in ProductCache.GetProductsByIds(productIds))
							{
								cResults.Add(item);
							}
						}
					});

				if (sort)
				{
					if (sortDescending)
					{
						results = cResults.DistinctBy(p => p.ProductID).OrderByDescending(kp => (kp.DynamicKits[0].DynamicKitGroups.Sum(g => g.MinimumProductCount))).ToList();
					}
					else
					{
						results = cResults.DistinctBy(p => p.ProductID).OrderBy(kp => (kp.DynamicKits[0].DynamicKitGroups.Sum(g => g.MinimumProductCount))).ToList();
					}
				}
				else
				{
					results = cResults.DistinctBy(p => p.ProductID).ToList();
				}
			}
			else
			{
				results = new List<Product>();
			}

			return results;
		}

		private List<Product> GetActiveCatalogProducts(int catalogId, short? accountTypeId, int? productPriceTypeID)
		{
			List<Product> results;

			IEnumerable<int> productIds;
			if (__activeProductIdsByCatalogCache.TryGet(Tuple.Create(catalogId, accountTypeId, productPriceTypeID), out productIds) && productIds.Any())
				results = ProductCache.GetProductsByIds(productIds).ToList();
			else
				results = new List<Product>();
			return results;
		}

		public override List<Product> GetActiveProductsForCatalog(int storeFrontId, int catalogId, short? accountTypeId = null)
		{
			IEnumerable<Catalog> activeCatalogs = GetActiveCatalogs(storeFrontId);

			if (!activeCatalogs.Any(c => c.CatalogID == catalogId))
				return new List<Product>();

			return GetActiveCatalogProducts(catalogId, accountTypeId, null);
		}

		public override List<Product> GetNonDynamicKitProducts(int storeFrontID, short? accountTypeId = null)
		{
			return DoGetAllActiveProducts(storeFrontID, accountTypeId).Where(p => !p.IsDynamicKit()).ToList();
		}

		public override Product GetProduct(int productID)
		{
			Product result = ProductCache.GetProductById(productID);
			if (result == null) throw new Exception(string.Format("ProductID specified ({0}) cannot be found in cache.", productID));
			return result;
		}

	    public override Product GetProductwithoutCache(int productID)
	    {
            Product result = ProductCache.GetProductwithoutCache(productID);
            
            return result;
	    }

		public override List<Product> GetProducts(IEnumerable<int> productIDs)
		{
			List<Product> results = ProductCache.GetProductsByIds(productIDs).ToList();

			var missingProds = from ProdId in productIDs
							   join p in results on ProdId equals p.ProductID into pids
							   from mids in pids.DefaultIfEmpty()
							   where mids == null
							   select ProdId;

			if (missingProds.Any()) throw new Exception(string.Format("ProductID(s) specified ({0}) cannot be found in cache.", String.Join(", ", missingProds)));

			return results;
		}

		public override Product GetProduct(string sku)
		{
			Product result = ProductCache.GetProductBySKU(sku);
			if (result == null) throw new Exception(string.Format("Product SKU specified ({0}) cannot be found in cache.", sku));
			return result;
		}

		public override IEnumerable<Product> GetProducts(int storeFrontId, int categoryId, short? accountTypeId)
		{
			IEnumerable<Product> result = new List<Product>();
			IEnumerable<int> pIds = null;
			if (__activeProductIdsByStorefrontAndCategoryCache.TryGet(Tuple.Create(storeFrontId, categoryId, accountTypeId), out pIds))
				result = ProductCache.GetProductsByIds(pIds);
			return result;
		}

		protected override List<Catalog> DoGetActiveCatalogs(int storeFrontId)
		{
			IEnumerable<Catalog> result;
			__storeFrontCatalogsCache.TryGet(storeFrontId, out result);
			return result.ToList();
		}

		public override List<Category> GetActiveCategories(int storeFrontId, short? accountTypeId = null)
		{
			var lookup = Tuple.Create(storeFrontId, accountTypeId);
			//return _activeCategoriesByStoreFront.GetOrAdd(lookup, LoadActiveCategoriesByStoreFrontIdAndAccountTypeId);
			return LoadActiveCategoriesByStoreFrontIdAndAccountTypeId(lookup);
		}

		protected override List<Category> LoadActiveCategoriesByStoreFrontIdAndAccountTypeId(Tuple<int, short?> lookup)
		{
			List<Category> results = new List<Category>();
			IEnumerable<int> categoryIds;
			if (__activeCategoryIdsByStorefrontAndAccountTypeCache.TryGet(lookup, out categoryIds))
			{
				foreach (var item in categoryIds)
				{
					Category category = null;
					if (__categoryByIdCache.TryGet(item, out category))
						results.Add(category);
				}
			}
			return results;
		}

		protected override Category DoGetCategoryTree(int categoryTreeId)
		{
			Category result;
			__categoryByIdCache.TryGet(categoryTreeId, out result);
			return result;
		}

		protected override bool DoIsProductInStoreFront(int productID, int storeFrontID)
		{
			bool result = false;
			IEnumerable<int> ids;
			if (__storeFrontProductIdCache.TryGet(storeFrontID, out ids))
				result = ids.Contains(productID);
			return result;
		}

		public override List<Product> SearchProducts(int storeFrontId, string query, short? accountTypeId = null, IEnumerable<int> catalogsToSearch = null, string startsWith = null, bool includeDynamicKits = true, int? productPriceTypeID = null)
		{
			List<Product> results;

			var matchingProducts = __productSearchCache.Get(Tuple.Create(storeFrontId, query, accountTypeId, catalogsToSearch, startsWith, includeDynamicKits, productPriceTypeID));

			if (matchingProducts.Any())
			{
				results = ProductCache.GetProductsByIds(matchingProducts).ToList();
			}
			else
			{
				results = new List<Product>();
			}

			return results;
		}

		private bool ProductIdsForSearchContexResolver(Tuple<int, string, short?, IEnumerable<int>, string, bool, int?> ids, out IEnumerable<int> productIds)
		{
			PruductSearchArgs args = new PruductSearchArgs(ids);

			IEnumerable<ProductSearchResult> bag;
			if (__productSearchSubCache.TryGet(Tuple.Create(ids.Item1, ids.Item3, ids.Item4, ids.Item6, ids.Item7), out bag) && bag.Any())
			{
				if (!String.IsNullOrWhiteSpace(args.StartsWith))
				{
					productIds = bag
						.DistinctBy(prs => prs.ProductID)
						.Where(prs => prs.Name.StartsWith(args.StartsWith, StringComparison.OrdinalIgnoreCase))
						.Select(prs => prs.ProductID).ToArray();
				}
				else if (!String.IsNullOrWhiteSpace(args.Query))
				{
					var qlow = args.Query.ToLower();
					productIds = bag
						.DistinctBy(prs => prs.ProductID)
						.Where(prs => prs.SKU.Contains(qlow)
							|| prs.Name.Contains(qlow)
							|| prs.ShortDescription.Contains(qlow)
							|| prs.LongDescription.Contains(qlow))
						.Select(prs => prs.ProductID).ToArray();
				}
				else
				{
					productIds = bag.DistinctBy(prs => prs.ProductID).Select(prs => prs.ProductID).ToArray();
				}
			}
			else
			{
				productIds = Enumerable.Empty<int>();
			}

			return true;
		}

		private bool ProductIdsForSearchContexSubResolver(Tuple<int, short?, IEnumerable<int>, bool, int?> ids, out IEnumerable<ProductSearchResult> productSearchResults)
		{
			PruductSearchSubArgs args = new PruductSearchSubArgs(ids);

			ConcurrentBag<ProductSearchResult> bag = new ConcurrentBag<ProductSearchResult>();

			using (var context = Create.New<NetStepsEntities>())
			{
				using (IDbConnection conn = (context.Connection as System.Data.EntityClient.EntityConnection).StoreConnection)
				{
					if (conn.State == ConnectionState.Closed)
					{
						conn.Open();
					}
					Action<int?> doSearch = (catalogID) =>
					{

						IDbCommand dbCommand = conn.CreateCommand();
						dbCommand.CommandType = CommandType.Text;

						var storeFrontId = dbCommand.CreateParameter();
						storeFrontId.DbType = DbType.Int32;
						storeFrontId.Direction = ParameterDirection.Input;
						storeFrontId.ParameterName = "@storeFrontId";
						storeFrontId.Value = args.StoreFrontId;
						dbCommand.Parameters.Add(storeFrontId);

						var sqlText = @"
SELECT
	DISTINCT p.ProductID [ProductID]
	, p.sku [SKU]
	, dt.name [Name]
	, dt.ShortDescription [ShortDescription]
	, dt.LongDescription [LongDescription]
FROM [dbo].[Catalogs] c
INNER JOIN [dbo].[StoreFrontCatalogs] sfc
	ON sfc.CatalogID = c.CatalogID
INNER JOIN [dbo].[StoreFronts] sf
	ON sf.StoreFrontID = sfc.StoreFrontID
	AND sf.StoreFrontID = @storeFrontID
	AND sf.Active = 1
INNER JOIN [dbo].[CatalogItems] ci
	ON ci.CatalogID = c.CatalogID
	AND ci.Active = 1
	AND (ci.StartDateUTC IS NULL OR ci.StartDateUTC <= GETUTCDATE())
	AND (ci.EndDateUTC IS NULL OR ci.EndDateUTC >= GETUTCDATE())
INNER JOIN [dbo].[Products] p
	ON p.ProductID = ci.ProductID
	AND p.Active = 1
INNER JOIN [dbo].[ProductPrices] pp
	ON pp.ProductID = p.ProductID";
						if (args.ProductPriceTypeID.HasValue)
						{
							var prodPriceTypeID = dbCommand.CreateParameter();
							prodPriceTypeID.DbType = DbType.Int32;
							prodPriceTypeID.Direction = ParameterDirection.Input;
							prodPriceTypeID.ParameterName = "@prodPriceTypeID";
							prodPriceTypeID.Value = args.ProductPriceTypeID.Value;
							dbCommand.Parameters.Add(prodPriceTypeID);

							sqlText += @"
	AND pp.ProductPriceTypeID = @prodPriceTypeID";
						}
						sqlText += @"
INNER JOIN [dbo].[AccountPriceTypes] apt
	ON apt.ProductPriceTypeID = pp.ProductPriceTypeID";
						if (args.AccountTypeId.HasValue)
						{
							var actTypeID = dbCommand.CreateParameter();
							actTypeID.DbType = DbType.Int16;
							actTypeID.Direction = ParameterDirection.Input;
							actTypeID.ParameterName = "@actTypeID";
							actTypeID.Value = args.AccountTypeId.Value;
							dbCommand.Parameters.Add(actTypeID);

							sqlText += @"
	AND apt.AccountTypeID = @actTypeID";
						}
						sqlText += @"
LEFT JOIN [dbo].[ProductTranslations] pt
	ON pt.ProductID = p.ProductID
LEFT JOIN [dbo].[DescriptionTranslations] dt
	ON dt.DescriptionTranslationID = pt.DescriptionTranslationID
WHERE
	(c.StartDateUTC IS NULL OR c.StartDateUTC <= GETUTCDATE())
	AND (c.EndDateUTC IS NULL OR c.EndDateUTC >= GETUTCDATE())";
						if (!args.IncludeDynamicKits)
						{
							sqlText += @"
	AND NOT EXISTS(SELECT TOP 1 DynamicKitID FROM [dbo].[DynamicKits] dk WHERE dk.ProductID = p.ProductID)";
						}
						if (catalogID.HasValue)
						{
							var catId = dbCommand.CreateParameter();
							catId.DbType = DbType.Int32;
							catId.Direction = ParameterDirection.Input;
							catId.ParameterName = "@catId";
							catId.Value = catalogID;
							dbCommand.Parameters.Add(catId);

							sqlText += @"
	AND c.CatalogID = @catID";
						}

						dbCommand.CommandText = sqlText;

						try
						{
							using (IDataReader reader = dbCommand.ExecuteReader())
							{
								while (reader.Read())
								{
									bag.Add(new ProductSearchResult()
									{
										ProductID = reader.GetInt32("ProductID"),
										Name = reader.GetString("Name").ToLower(),
										SKU = reader.GetString("SKU").ToLower(),
										ShortDescription = reader.GetString("ShortDescription").ToLower(),
										LongDescription = reader.GetString("LongDescription").ToLower()
									});
								}
							}
						}
						catch (Exception e)
						{
							this.TraceException(e);
						}
					};

					if (args.CatalogsToSearch == null || !args.CatalogsToSearch.Any())
					{
						doSearch(null);
					}
					else
					{
						Parallel.ForEach(args.CatalogsToSearch, (catId) => doSearch(catId));
					}
				}
			}

			if (bag != null && bag.Any())
			{
				productSearchResults = bag.DistinctBy(prs => prs.ProductID).ToArray();
			}
			else
			{
				productSearchResults = Enumerable.Empty<ProductSearchResult>();
			}

			return true;
		}

		private class PruductSearchSubArgs
		{
			public PruductSearchSubArgs(Tuple<int, short?, IEnumerable<int>, bool, int?> args)
				: this(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5)
			{ }

			public PruductSearchSubArgs(int storeFrontId, short? accountTypeId, IEnumerable<int> catalogsToSearch, bool includeDynamicKits, int? productPriceTypeId)
			{
				StoreFrontId = storeFrontId;
				AccountTypeId = accountTypeId;
				CatalogsToSearch = catalogsToSearch;
				IncludeDynamicKits = includeDynamicKits;
				ProductPriceTypeID = productPriceTypeId;
			}

			public int StoreFrontId;
			public short? AccountTypeId;
			public IEnumerable<int> CatalogsToSearch;
			public bool IncludeDynamicKits;
			public int? ProductPriceTypeID;
		}

		private class PruductSearchArgs : PruductSearchSubArgs
		{
			public PruductSearchArgs(Tuple<int, string, short?, IEnumerable<int>, string, bool, int?> args)
				: base(args.Item1, args.Item3, args.Item4, args.Item6, args.Item7)
			{
				Query = args.Item2;
				StartsWith = args.Item5;
			}

			public string Query;
			public string StartsWith;
		}

		[Serializable]
		public struct ProductSearchResult
		{
			public int ProductID;
			public string SKU;
			public string Name;
			public string ShortDescription;
			public string LongDescription;
		}

		#endregion
	}
}