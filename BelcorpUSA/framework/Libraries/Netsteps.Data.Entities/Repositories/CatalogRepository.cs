using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using NetSteps.Common.Base;
using NetSteps.Common.Expressions;
using NetSteps.Common.Extensions;
using NetSteps.Common.Utility;
using NetSteps.Core.Cache;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;

namespace NetSteps.Data.Entities.Repositories
{
    public partial class CatalogRepository
    {
		private static readonly ICache<string, IList<int>> ActiveCatalogProductsIdCache = new MruLocalMemoryCache<string, IList<int>>("Catalog.ActiveProductsByCatalogId");

        protected override Func<NetStepsEntities, IQueryable<Catalog>> loadAllFullQuery
        {
            get
            {
                return CompiledQuery.Compile<NetStepsEntities, IQueryable<Catalog>>(
                    context => context.Catalogs
                        .Include("Translations")
                        .Include("StoreFronts")
                        .Include("StoreFronts.MarketStoreFronts")
                        .Include("StoreFronts.MarketStoreFronts.Market")
                        .Include("Category")
                        .Include("Category.Translations")
                        .Include("CatalogItems")
                        .Include("AccountTypes"));
            }
        }

        public PaginatedList<CatalogSearchData> Search(FilterPaginatedListParameters<Catalog> searchParams)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var results = new PaginatedList<CatalogSearchData>(searchParams);

                    IQueryable<Catalog> matchingItems = context.Catalogs.Include("Translations");

	                if(searchParams.WhereClause != null)
	                {
		                matchingItems = matchingItems.Where(searchParams.WhereClause);
	                }

	                if(!string.IsNullOrEmpty(searchParams.OrderBy))
	                {
		                if(searchParams.OrderBy.Equals("Schedule", StringComparison.InvariantCultureIgnoreCase))
		                {
			                matchingItems =
				                matchingItems.Order(searchParams.OrderByDirection, c => c.StartDateUTC)
					                .Then(searchParams.OrderByDirection, c => c.EndDateUTC);
		                }
		                else
		                {
			                matchingItems = matchingItems.ApplyOrderByFilter(searchParams, context);
		                }
	                }
	                else
	                {
		                matchingItems = matchingItems.OrderBy(a => a.StartDateUTC);
	                }

                    // TotalCount must be set before applying Pagination - JHE
                    results.TotalCount = matchingItems.Count();

                    matchingItems = matchingItems.ApplyPagination(searchParams);

                    var catalogs = matchingItems.Select(c => new
                    {
                        Markets = c.StoreFronts.SelectMany(s => s.MarketStoreFronts.Select(m => m.MarketID)).Distinct(),
                        StoreFronts = c.StoreFronts.Select(s => s.StoreFrontID),
                        Name = !c.Translations.Any(t => t.LanguageID == ApplicationContext.Instance.CurrentLanguageID) ? c.Translations.Any() ? c.Translations.FirstOrDefault().Name : "" : c.Translations.FirstOrDefault(t => t.LanguageID == ApplicationContext.Instance.CurrentLanguageID).Name,
                        c.CatalogID,
                        CategoryName = !c.Category.Translations.Any(t => t.LanguageID == ApplicationContext.Instance.CurrentLanguageID) ? c.Category.Translations.Any() ? c.Category.Translations.FirstOrDefault().Name : "" : c.Category.Translations.FirstOrDefault(t => t.LanguageID == ApplicationContext.Instance.CurrentLanguageID).Name,
                        c.CategoryID,
                        c.Active,
                        c.StartDateUTC,
                        c.EndDateUTC,
                        c.Editable
                    }).ToList();

                    foreach (var catalog in catalogs)
                        results.Add(new CatalogSearchData()
                        {
                            Markets = SmallCollectionCache.Instance.Markets.Where(m => catalog.Markets.Contains(m.MarketID)).Select(m => m.GetTerm()).Join(", "),
                            StoreFronts = SmallCollectionCache.Instance.StoreFronts.Where(sf => catalog.StoreFronts.Contains(sf.StoreFrontID)).Select(m => m.GetTerm()).Join(", "),
                            Name = catalog.Name,
                            CatalogID = catalog.CatalogID,
                            CategoryTreeName = catalog.CategoryName,
                            CategoryTreeID = catalog.CategoryID,
                            Active = catalog.Active,
                            StartDate = catalog.StartDateUTC.HasValue ? catalog.StartDateUTC.UTCToLocal() : (DateTime?)null,
                            EndDate = catalog.EndDateUTC.HasValue ? catalog.EndDateUTC.UTCToLocal() : (DateTime?)null,
                            Editable = catalog.Editable
                        });

                    return results;
                }
            });
        }

        public void Copy(int copyFromCatalogID, int copyToCatalogID)
        {
            ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    Catalog copyFrom = context.Catalogs.Include("CatalogItems").FirstOrDefault(c => c.CatalogID == copyFromCatalogID);
                    Catalog copyTo = context.Catalogs.Include("CatalogItems").FirstOrDefault(c => c.CatalogID == copyToCatalogID);
                    if (copyFrom != null && copyTo != null)
                    {
                        var catalogItemsToCopy = copyFrom.CatalogItems.Where(ci => !copyTo.CatalogItems.Any(catItem => catItem.ProductID == ci.ProductID));
                        if (catalogItemsToCopy.Any())
                        {
                            foreach (CatalogItem item in catalogItemsToCopy)
                            {
                                copyTo.CatalogItems.Add(new CatalogItem
                                {
                                    ProductID = item.ProductID,
                                    StartDateUTC = item.StartDateUTC,
                                    EndDateUTC = item.EndDateUTC,
                                    Active = item.Active
                                });
                            }
                            copyTo.Save();
                        }
                    }
                }
            });
        }

        public void BulkAddCatalogItems(int catalogID, IEnumerable<int> productIDs, DateTime? startDate, DateTime? endDate)
        {
            ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    Catalog catalog = context.Catalogs.Include("CatalogItems").First(c => c.CatalogID == catalogID);

                    foreach (int productID in productIDs)
                    {
                        CatalogItem item = catalog.CatalogItems.FirstOrDefault(ci => ci.ProductID == productID);
                        if (item == default(CatalogItem))
                        {
                            item = new CatalogItem
                            {
                                ProductID = productID,
                                Active = true
                            };
                            catalog.CatalogItems.Add(item);
                        }

                        item.StartDateUTC = startDate.LocalToUTC();
                        item.EndDateUTC = endDate.LocalToUTC();
                    }

                    catalog.Save();
                }
            });
        }

        public List<Catalog> LoadAllFullByMarkets(IEnumerable<int> marketIDs)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    return loadAllFullQuery(context)
                        .Where(c => c.StoreFronts
                            .Any(s => s.MarketStoreFronts
                                .Any(ms => marketIDs.Contains(ms.MarketID))))
                        .ToList();
                }
            });
        }

        public List<Catalog> LoadAllFullExcept(IEnumerable<int> catalogIDs)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var whereNotIn = ExpressionHelper.MakeWhereNotInExpression<Catalog, int>(c => c.CatalogID, catalogIDs);
                    return loadAllFullQuery(context).Where(whereNotIn).ToList();
                }
            });
        }
		public IEnumerable<Catalog> LoadActiveCatalogsForStoreFront(int storeFrontId)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					return context.Catalogs
						.Include("Translations")
						.Where(c => c.Active
							&& (!c.StartDateUTC.HasValue || DateTime.UtcNow >= c.StartDateUTC)
							&& (!c.EndDateUTC.HasValue || DateTime.UtcNow <= c.EndDateUTC)
							&& c.StoreFronts.Any(sf => sf.StoreFrontID == storeFrontId)
						).ToArray();
				}
			});
		}

		//TODO: Move the caching on this method to the cache layer
		public IEnumerable<int> GetActiveCatalogProductIds(int catalogId, short? accountTypeId, int? productPriceTypeID)
		{
			string key = new StringBuilder()
				.Append(catalogId)
				.Append(accountTypeId)
				.Append(productPriceTypeID).ToString();
			IList<int> result;
			if (!ActiveCatalogProductsIdCache.TryGet(key, out result))
			{
				ActiveCatalogProductsIdCache.TryAdd(key, result = ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
				{
				using (NetStepsEntities context = CreateContext())
				{
					var query = context.CatalogItems.Where(ci =>
						ci.CatalogID == catalogId
						&& ci.Active
						&& (!ci.StartDateUTC.HasValue || DateTime.UtcNow >= ci.StartDateUTC)
						&& (!ci.EndDateUTC.HasValue || DateTime.UtcNow <= ci.EndDateUTC)
						&& ci.Product.Active
						);

					if (productPriceTypeID.HasValue)
					{
						query = query.Where(ci =>
									ci.Product.Prices.Select(x => x.ProductPriceType)
										.Any(ppt => ppt.ProductPriceTypeID == productPriceTypeID));
					}

					if (accountTypeId.HasValue)
					{
						query = query.Where(ci =>
									ci.Product.Prices.Select(x => x.ProductPriceType)
										.Any(ppt => ppt.AccountPriceTypes.Any(apt => apt.AccountTypeID == accountTypeId)));
					}
					return query.Select(ci => ci.ProductID).ToArray();
				}
				}));
		}
			return result;
		}

        public IEnumerable<short> GetAccountTypeIDsForCatalog(Catalog catalog)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var currentCatalog = context.Catalogs.Include("AccountTypes").FirstOrDefault(c => c.CatalogID == catalog.CatalogID);
                    return currentCatalog.AccountTypes.Select(at => at.AccountTypeID).ToList();
                }
            });
        }

		public Catalog GetCatalogByName(string name)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					return context.Catalogs.FirstOrDefault(c => c.Translations.Any(t => t.Name == name));
				}
			});
		}

        public void RemoveAccountTypeFilters(int catalogID)
        {
            ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    Catalog catalog = context.Catalogs.Include("AccountTypes").First(c => c.CatalogID == catalogID);

                    catalog.AccountTypes.RemoveAll();

                    catalog.Save();
                }
            });
        }

        public void AddAccountTypeFilters(int catalogID, IEnumerable<short> accountTypeIDs)
        {
            ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    Catalog catalog = context.Catalogs.Include("AccountTypes").First(c => c.CatalogID == catalogID);
                    catalog.AccountTypes.RemoveAll();

                    foreach (var accountTypeID in accountTypeIDs)
                    {
                        var account = new AccountType()
                        {
                            AccountTypeID = accountTypeID
                        };

                        catalog.AccountTypes.Add(account);
                    }

                    catalog.Save();
                }
            });
        }
	}
}