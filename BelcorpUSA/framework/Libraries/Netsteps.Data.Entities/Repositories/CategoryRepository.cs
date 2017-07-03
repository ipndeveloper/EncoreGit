using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class CategoryRepository
    {
        #region Members
        protected override Func<NetStepsEntities, IQueryable<Category>> loadAllFullQuery
        {
            get
            {
                return CompiledQuery.Compile<NetStepsEntities, IQueryable<Category>>(
                    context => context.Categories
                        .Include("Translations")
                                    .Include("Translations.HtmlContent")
                                    .Include("Translations.HtmlContent.HtmlElements")
                                    .Include("Catalogs")
                                    .Include("Catalogs.Translations")
                                    .Include("Catalogs.StoreFronts"));
            }
        }
        #endregion

        public Category LoadCategoryByNumber(string categoryNumber)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    return loadAllFullQuery(context).Where(c => c.CategoryNumber == categoryNumber).FirstOrDefault();
                }
            });
        }

		public List<Category> LoadAllFullByCategoryTypeId(int categoryTypeId)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					return loadAllFullQuery(context).Where(c => c.CategoryTypeID == categoryTypeId).ToList();
				}
			});
		}

        public List<Category> LoadFullTopLevelByCategoryTypeId(int categoryTypeId)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    return loadAllFullQuery(context).Where(c => c.CategoryTypeID == categoryTypeId && !c.ParentCategoryID.HasValue).ToList();
                }
            });
        }

        public TrackableCollection<Category> LoadFullByParent(int parentCategoryID)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    return loadAllFullQuery(context).Where(c => c.ParentCategoryID == parentCategoryID).ToTrackableCollection();
                }
            });
        }

        public override void Delete(int primaryKey)
        {
            bool isBeingUsed = true;
            ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    if (!context.Catalogs.Any(c => c.CategoryID == primaryKey))
                        isBeingUsed = false;
                }
            });
            if (!isBeingUsed)
                base.Delete(primaryKey);
        }

        public PaginatedList<CategorySearchData> Search(FilterPaginatedListParameters<Category> searchParams)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
									var inventory = Create.New<InventoryBaseRepository>();
									PaginatedList<CategorySearchData> results = new PaginatedList<CategorySearchData>(searchParams);

                    IQueryable<Category> matchingItems = context.Categories;

                    if (searchParams.WhereClause != null)
                        matchingItems = matchingItems.Where(searchParams.WhereClause);

                    if (!string.IsNullOrEmpty(searchParams.OrderBy))
                    {
                        matchingItems = matchingItems.ApplyOrderByFilter(searchParams, context);
                    }
                    else
                        matchingItems = matchingItems.OrderBy(a => a.CategoryID);

                    // TotalCount must be set before applying Pagination - JHE
                    results.TotalCount = matchingItems.Count();

                    matchingItems = matchingItems.ApplyPagination(searchParams);

                    var currentLanguageId = ApplicationContext.Instance.CurrentLanguageID;
                    var categories = matchingItems.Select(c => new
                    {
                        c.CategoryID,
                        Name = c.Translations.Any( t => t.LanguageID == currentLanguageId )
                            ? c.Translations.FirstOrDefault( t => t.LanguageID == currentLanguageId ).Name
                            : c.Translations.Any() ? c.Translations.FirstOrDefault().Name : String.Empty,
                        UsedBy = c.Catalogs.Select(cat => new
                        {
                            CatalogID = cat.CatalogID,
                            CatalogName = cat.Translations.Any( t => t.LanguageID == currentLanguageId )
                                ? cat.Translations.FirstOrDefault( t => t.LanguageID == currentLanguageId ).Name
                                : cat.Translations.Any() ? cat.Translations.FirstOrDefault().Name : String.Empty,
                            StoreFronts = cat.StoreFronts.Select(sf => sf.StoreFrontID)
                        })
                    });

                    foreach (var category in categories.ToList())
                    {
                        CategorySearchData categorySearchData = new CategorySearchData()
                        {
                            TreeID = category.CategoryID,
                            TreeName = category.Name,
                            UsedBy = category.UsedBy.Select(ub => new CategorySearchData.UsedBySearchData()
                            {
                                CatalogID = ub.CatalogID,
                                CatalogName = ub.CatalogName,
                                StoreFronts = SmallCollectionCache.Instance.StoreFronts.Where(sf => ub.StoreFronts.Contains(sf.StoreFrontID)).Select(sf => sf.GetTerm()).Join(", ")
                            }),
                        };

                        Category categoryWithSubs = inventory.GetCategoryTree(category.CategoryID);
                        List<Category> categoryList = new List<Category>();
                        categoryList.Add(categoryWithSubs);
                        categoryList.AddRange(categoryWithSubs.ChildCategories.Traverse<Category>(c => c.ChildCategories));
                        List<int> storefrontIDs = category.UsedBy.SelectMany(ub => ub.StoreFronts).ToList();
                        List<int> categoryListIDs = categoryList.Select(cl => cl.CategoryID).ToList();
                        categorySearchData.ProductCount = context.Catalogs.Where(c => c.StoreFronts.Any(sf => storefrontIDs.Contains(sf.StoreFrontID))
                                && c.Active
                                && (c.StartDateUTC == null || c.StartDateUTC <= DateTime.UtcNow)
                                && (c.EndDateUTC == null || c.EndDateUTC > DateTime.UtcNow))
                            .SelectMany(c => c.CatalogItems).Where(ci => ci.Active
                                && (ci.StartDateUTC == null || ci.StartDateUTC <= DateTime.UtcNow)
                                && (ci.EndDateUTC == null || ci.EndDateUTC > DateTime.UtcNow))
                            .Select(ci => ci.Product).Where(p => p.ProductBase.Categories.Any(c => categoryListIDs.Contains(c.CategoryID))).Select(p => p.ProductID).Distinct().Count();

                        results.Add(categorySearchData);
                    }

                    return results;
                }
            });
        }

        public int GetCategoryTreeID(int categoryID)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    return context.usp_category_get_top_of_tree_id(categoryID).First().Value;
                }
            });
        }

        public Category LoadFullTree(int categoryTreeID)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var categoryIDs = context.usp_categories_get_tree(categoryTreeID);

                    var topCategory = context.Categories
                        .Include("Translations")
                        .Where(x => categoryIDs.Contains(x.CategoryID))
                        .ToList() // ToList() is required so that EF will auto-populate all of the "ChildCategories" collections
                        .FirstOrDefault(x => x.CategoryID == categoryTreeID);

                    return topCategory;
                }
            });
        }

        public CategoryTranslation LoadTranslation(int categoryID, int languageID)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    return context.CategoryTranslations
                        .Include("HtmlContent")
                        .Include("HtmlContent.HtmlElements")
                        .FirstOrDefault(ct => ct.CategoryID == categoryID && ct.LanguageID == languageID);
                }
            });
        }

        public List<ArchiveCategorySearchData> LoadAllArchiveCategories()
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var currentLanguageId = ApplicationContext.Instance.CurrentLanguageID;
                    var categories = context.Categories.Where(c => c.CategoryTypeID == (int)Constants.CategoryType.Archive).Select(c => new ArchiveCategorySearchData()
                    {
                        CategoryID = c.CategoryID,
                        ParentCategoryID = c.ParentCategoryID,
                        Name = c.Translations.Any( t => t.LanguageID == currentLanguageId )
                            ? c.Translations.FirstOrDefault( t => t.LanguageID == currentLanguageId ).Name
                            : c.Translations.Any() ? c.Translations.FirstOrDefault().Name : String.Empty,
                        ArchiveCount = c.Archives.Count(a => a.Active && a.IsDownloadable),
                        SortIndex = c.SortIndex
                    }).ToList();

                    return categories;
                }
            });
        }

        public override void Delete(Category obj)
        {
            if (obj.Translations != null)
            {
                foreach (var translation in obj.Translations)
                {
                    translation.HtmlContent.HtmlElements.RemoveAllAndMarkAsDeleted();
                    if (translation.HtmlContent.ChangeTracker.State != ObjectState.Added)
                        translation.HtmlContent.MarkAsDeleted();
                }
                obj.Translations.RemoveAllAndMarkAsDeleted();
            }
            obj.ProductBases.RemoveAll();
            obj.Archives.RemoveAll();
            base.Delete(obj);
        }

        public void DeleteTree(int categoryTreeID)
        {
            ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    if (!context.Catalogs.Any(c => c.CategoryID == categoryTreeID))
                    {
                        string sql = @"DECLARE @AllCategories TABLE (CategoryID INT NOT NULL, ParentCategoryID INT NULL);
                        WITH CategoryHierarchy (CategoryID, ParentCategoryID)
                        AS
                        (
	                        SELECT CategoryID, ParentCategoryID
	                        FROM Categories 
	                        WHERE CategoryID = @p0

	                        UNION ALL

	                        SELECT c.CategoryID, c.ParentCategoryID
	                        FROM Categories c
	                        INNER JOIN CategoryHierarchy ch
		                        ON c.ParentCategoryID = ch.CategoryID
                        )
                        INSERT INTO @AllCategories SELECT * FROM CategoryHierarchy

                        DELETE FROM HtmlElements WHERE HtmlContentID IN (SELECT HtmlContentID FROM CategoryTranslations WHERE CategoryID IN (SELECT CategoryID FROM @AllCategories))
                        DELETE FROM CategoryTranslations WHERE CategoryID IN (SELECT CategoryID FROM @AllCategories)
                        DELETE FROM HtmlContent WHERE HtmlContentID IN (SELECT HtmlContentID FROM CategoryTranslations WHERE CategoryID IN (SELECT CategoryID FROM @AllCategories))
                        DELETE FROM ProductBaseCategories WHERE CategoryID IN (SELECT CategoryID FROM @AllCategories)
                        DELETE FROM ArchiveCategories WHERE CategoryID IN (SELECT CategoryID FROM @AllCategories)
                        DELETE FROM Categories WHERE CategoryID IN (SELECT CategoryID FROM @AllCategories)";

                        context.ExecuteStoreCommand(sql, categoryTreeID);
                    }
                    else
                        throw new NetSteps.Common.Exceptions.DeleteDataException("Cannot delete a category tree in use by a catalog");
                }
            });
        }

        public List<Category> LoadCategoriesByStoreFrontId(int storeFrontId)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    //TODO: Find top of category tree that matches the storefront, then load it and all of it's children
                    return loadAllFullQuery(context).ToList();
							}
            });
        }

		public IEnumerable<int> GetCategoryIdsByStorefront(int storeFrontId)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				IEnumerable<Tuple<int, int?>> resultIds = new Tuple<int, int?>[] { };
				IEnumerable<int> results = new int[] { };
				using (NetStepsEntities context = CreateContext())
				{
					var leafSelect =
							from storefront in context.StoreFronts
							from catalog in storefront.Catalogs
							from catalogitem in catalog.CatalogItems
							join product in context.Products on catalogitem.ProductID equals product.ProductID
							join productbase in context.ProductBases on product.ProductBaseID equals productbase.ProductBaseID
							from category in productbase.Categories
							where storefront.StoreFrontID == storeFrontId
							select new { CategoryId = category.CategoryID, ParentId = category.ParentCategoryID };

					resultIds = leafSelect.Distinct().Each(a => Tuple.Create(a.CategoryId, a.ParentId));

					if (resultIds != null && resultIds.Any())
					{
						var pIds = resultIds.Where(t => t.Item2.HasValue).Select(t => t.Item2.Value).Distinct().Sort().ToArray();
						while (pIds != null && pIds.Any())
						{
							var nextLevelSelect =
								from category in context.Categories
								where pIds.Contains(category.CategoryID)
								select new { CategoryId = category.CategoryID, ParentId = category.ParentCategoryID };

							var nextLevel = nextLevelSelect.Distinct().Each(a => Tuple.Create(a.CategoryId, a.ParentId));
							if (nextLevel != null && nextLevel.Any())
							{
								resultIds = resultIds.UnionBy(nextLevel, c => c.Item1);
								pIds = nextLevel.Where(t => t.Item2.HasValue && !pIds.Contains(t.Item2.Value)).Select(t => t.Item2.Value).Distinct().Sort().ToArray();
							}
							else break;
						}

						results = resultIds.Select(t => t.Item1).Sort();
					}
				}

				return results;
			});
		}

		public IEnumerable<int> GetActiveCategoryIdsByStoreFrontIdAndAccountTypeId(int storeFrontId, short? accountTypeId = null)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				IEnumerable<Tuple<int, int>> resultIds = new Tuple<int, int>[] { };

				using (NetStepsEntities context = CreateContext())
				{
					if (!accountTypeId.HasValue)
					{
						var leafSelect =
							from storefront in context.StoreFronts
							from catalog in storefront.Catalogs
							from catalogitem in catalog.CatalogItems
							join product in context.Products on catalogitem.ProductID equals product.ProductID
							join productbase in context.ProductBases on product.ProductBaseID equals productbase.ProductBaseID
							from category in productbase.Categories
							where storefront.StoreFrontID == storeFrontId
							   && (product.Active
								   && (catalogitem.Active
									   && (!catalogitem.StartDateUTC.HasValue || DateTime.UtcNow >= catalogitem.StartDateUTC)
									   && (!catalogitem.EndDateUTC.HasValue || DateTime.UtcNow <= catalogitem.EndDateUTC)))
							   && category.ParentCategoryID != null
							select new { CategoryId = category.CategoryID, ParentId = category.ParentCategoryID };

						resultIds = leafSelect.Distinct().Each(a => Tuple.Create(a.CategoryId, a.ParentId.Value));
					}
					else
					{
						var leafSelect =
							from storefront in context.StoreFronts
							from catalog in storefront.Catalogs
							from catalogitem in catalog.CatalogItems
							join product in context.Products on catalogitem.ProductID equals product.ProductID
							join productbase in context.ProductBases on product.ProductBaseID equals productbase.ProductBaseID
							from price in product.Prices
							join priceType in context.ProductPriceTypes on price.ProductPriceTypeID equals priceType.ProductPriceTypeID
							from accountPriceType in priceType.AccountPriceTypes
							from category in productbase.Categories
							where storefront.StoreFrontID == storeFrontId
							   && (product.Active
								   && (catalogitem.Active
									   && (!catalogitem.StartDateUTC.HasValue || DateTime.UtcNow >= catalogitem.StartDateUTC)
									   && (!catalogitem.EndDateUTC.HasValue || DateTime.UtcNow <= catalogitem.EndDateUTC)))
							   && category.ParentCategoryID != null
							   && accountPriceType.AccountTypeID == accountTypeId.Value
							select new { CategoryId = category.CategoryID, ParentId = category.ParentCategoryID };

						resultIds = leafSelect.Distinct().Each(a => Tuple.Create(a.CategoryId, a.ParentId.Value));
					}

					if (resultIds != null && resultIds.Any())
					{
						var pIds = resultIds.Select(t => t.Item2).Distinct().Sort().ToArray();
						while (pIds != null && pIds.Any())
						{
							var nextLevelSelect =
								from category in context.Categories
								where pIds.Contains(category.CategoryID)
								  && category.ParentCategoryID != null
								select new { CategoryId = category.CategoryID, ParentId = category.ParentCategoryID };

							var nextLevel = nextLevelSelect.Distinct().Each(a => Tuple.Create(a.CategoryId, a.ParentId.Value));
							if (nextLevel != null && nextLevel.Any())
							{
								resultIds = resultIds.UnionBy(nextLevel, c => c.Item1);
								pIds = nextLevel.Where(t => !pIds.Contains(t.Item2)).Select(t => t.Item2).Distinct().Sort().ToArray();
							}
							else break;
						}
					}
				}

				return resultIds.Select(t => t.Item1).Sort();
			});
		}

		public IEnumerable<Category> GetActiveCategoriesByStoreFrontIdAndAccountTypeId(int storeFrontId, short? accountTypeId = null)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				IEnumerable<Category> results = null;
				var lookupIds = GetActiveCategoryIdsByStoreFrontIdAndAccountTypeId(storeFrontId, accountTypeId);

				if (lookupIds != null && lookupIds.Any())
				{
					using (NetStepsEntities context = CreateContext())
					{
						results = context.Categories
							.Include("Translations")
							.Include("Translations.HtmlContent")
							.Include("Translations.HtmlContent.HtmlElements")
							.Include("Catalogs")
							.Include("Catalogs.Translations")
							.Include("Catalogs.StoreFronts")
							.Where(c => lookupIds.Contains(c.CategoryID))
							.ToArray();
					}
				}

				return results;
			});
		}
    }
}