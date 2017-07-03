using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;
using NetSteps.Common.Base;
using NetSteps.Common.Exceptions;
using NetSteps.Common.Extensions;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Expressions;
using NetSteps.Data.Entities.Extensions;

namespace NetSteps.Data.Entities.Repositories
{
    public partial class ProductBaseRepository
    {
        protected override Func<NetStepsEntities, IQueryable<ProductBase>> loadAllFullQuery
        {
            get
            {
                return CompiledQuery.Compile<NetStepsEntities, IQueryable<ProductBase>>(
                    context => context
                        .ProductBases
                        .Include("ExcludedStateProvinces")
                        .Include("Categories")
                        .Include("Categories.Translations")
                        .Include("Translations")
                        .Include("Products")
                        .Include("Products.Translations")
                        .Include("Products.Prices")
                        .Include("Products.Files")
                        .Include("Products.Properties")
                        .Include("Products.CatalogItems")
                        .Include("Products.CatalogItems.Catalog")
                        .Include("Products.CatalogItems.Catalog.Translations")
                        .Include("Products.CatalogItems.Catalog.StoreFronts")
                        .Include("Products.DynamicKits")
                        .Include("Products.DynamicKits.DynamicKitGroups")
                        .Include("Products.DynamicKits.DynamicKitGroups.Translations")
                        .Include("Products.DynamicKits.DynamicKitGroups.DynamicKitGroupRules")
                        .Include("Products.WarehouseProducts")
                        .Include("Products.ChildProductRelations")
                        .Include("ProductBaseProperties")
                        .Include("ProductBaseProperties.ProductPropertyType")
                        .Include("ProductBaseProperties.ProductPropertyType.ProductPropertyValues")
                        .Include("Products.ProductVariantInfo")
                        .Include("Products.ExcludedShippingMethods"));
            }
        }

        /// <summary>
        /// This method is an overridden Delete method to provide a Transaction delete of a ProductBase and
        /// all the products that use that ProductBase along with all translations. - JHE 
        /// TODO: Test these new overridden Transactional deletes methods - JHE
        /// </summary>
        /// <param name="obj"></param>
        public override void Delete(ProductBase obj)
        {
            Delete(obj.ProductBaseID);
        }

        public override void Delete(int primaryKey)
        {
            ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                using (TransactionScope transaction = new TransactionScope()) // TODO: Test this new Transaction Code - JHE
                {
                    var obj = context.ProductBases
                        .Include("Categories")
                        .Include("Translations")
                        .Include("Testimonials")
                        .Include("ProductBaseProperties")
                        .Include("Products")
                        .Include("Products.Translations")
                        .Include("Products.Prices")
                        .Include("Products.Properties")
                        .Include("Products.CatalogItems")
                        .Include("Products.WarehouseProducts")
                        .Include("Products.ChildProductRelations")
                        .Include("Products.ParentProductRelations")
                        .Include("Products.AutoshipScheduleProducts")
                        .Include("Products.PromotionProducts")
                        .Include("Products.TaxCategories")
                        .Include("Products.Files")
                        .Include("Products.DynamicKits")
                        .Include("Products.DynamicKits.DynamicKitGroups")
                        .Include("Products.DynamicKits.DynamicKitGroups.Translations")
                        .Include("Products.DynamicKits.DynamicKitGroups.DynamicKitGroupRules")
                        .Include("Products.ProductVariantInfo")
                        .Include("Products.ExcludedShippingMethods")
                        .FirstOrDefault(pb => pb.ProductBaseID == primaryKey);

                    if (obj == default(ProductBase))
                        return;

                    obj.StartEntityTracking();

                    if (obj.Products != null)
                    {
                        foreach (var product in obj.Products)
                        {
                            // Linker Tables still need to use the Self-Tracking Entities to delete the object - JHE
                            product.TaxCategories.RemoveAll();

                            context.DeleteObjects(product.Translations);
                            context.DeleteObjects(product.Prices);
                            context.DeleteObjects(product.Properties);
                            context.DeleteObjects(product.CatalogItems);
                            context.DeleteObjects(product.WarehouseProducts);
                            context.DeleteObjects(product.ChildProductRelations);
                            context.DeleteObjects(product.ParentProductRelations);
                            context.DeleteObjects(product.AutoshipScheduleProducts);

                            foreach (var file in product.Files)
                            {
                                try
                                {
                                    System.IO.File.Delete(file.FilePath.ReplaceFileUploadPathToken().WebUploadPathToAbsoluteUploadPath());
                                }
                                catch { }
                            }

                            context.DeleteObjects(product.Files);

                            foreach (var kit in product.DynamicKits)
                            {
                                foreach (var group in kit.DynamicKitGroups)
                                {
                                    context.DeleteObjects(group.DynamicKitGroupRules);
                                    context.DeleteObjects(group.Translations);
                                }

                                context.DeleteObjects(kit.DynamicKitGroups);
                            }

                            context.DeleteObjects(product.DynamicKits);
                        }
                        context.DeleteObjects(obj.Products);
                    }

                    obj.Categories.RemoveAll();
                    context.DeleteObjects(obj.Translations);
                    context.DeleteObjects(obj.Testimonials);
                    context.DeleteObjects(obj.ProductBaseProperties);

                    context.DeleteObject(obj);

                    Save(obj, context);

                    transaction.Complete();
                }
            });
        }

        public PaginatedList<ProductBaseSearchData> Search(ProductBaseSearchParameters searchParams)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (var context = CreateContext())
                {
                    var results = new PaginatedList<ProductBaseSearchData>(searchParams);
                    IQueryable<ProductBase> productBases = context.ProductBases;

                    if (!string.IsNullOrEmpty(searchParams.Query))
                    {
                        productBases = productBases.Where(
                            pb => pb.Products.Any(p => p.SKU.Contains(searchParams.Query)
                                || p.Translations.Any(t => t.Name.Contains(searchParams.Query)
                                    || t.ShortDescription.Contains(searchParams.Query)
                                    || t.LongDescription.Contains(searchParams.Query))
                                )
                            );
                    }

                    if (!string.IsNullOrEmpty(searchParams.BPCS))
                    {
                        var productPropertyTypeId = context.ProductPropertyTypes.First(x => x.Name == "BPCS code").ProductPropertyTypeID;
                        productBases = productBases.Where(
                            pb => pb.Products.Any(p1 => p1.Properties.Any(pp => pp.ProductPropertyTypeID == productPropertyTypeId && pp.PropertyValue.Contains(searchParams.BPCS)))
                            );
                    }

                    if (searchParams.ProductTypeID.HasValue)
                        productBases = productBases.Where(pb => pb.ProductTypeID == searchParams.ProductTypeID.Value);

                    if (searchParams.Active.HasValue)
                        productBases = productBases.Where(pb => pb.Products.Any(p => p.Active) == searchParams.Active.Value);

                    if (searchParams.IsShippable.HasValue)
                        productBases = productBases.Where(pb => pb.IsShippable == searchParams.IsShippable.Value);

                    if (searchParams.ChargeShipping.HasValue)
                        productBases = productBases.Where(pb => pb.ChargeShipping == searchParams.ChargeShipping.Value);

                    if (!string.IsNullOrEmpty(searchParams.SAPSKU))
                    {
                        List<int> ProductsListNew = new ProductRepository().SearchProductsBySAPSKU(productBases.ToList(), searchParams.SAPSKU);

                        productBases = productBases.Where(x => ProductsListNew.Contains(x.ProductBaseID));
                        
                        //var productPropertyTypeId = context.ProductPropertyTypes.First(x => x.Name == "SAP Code").ProductPropertyTypeID;
                        //productBases = productBases.Where(
                        //    pb => pb.Products.Any(p1 => p1.Properties.Any(pp => pp.ProductPropertyTypeID == productPropertyTypeId && pp.PropertyValue.Contains(searchParams.SAPSKU)))
                        //    );
                    }

                    results.TotalCount = productBases.Count();

                    if (!string.IsNullOrEmpty(searchParams.OrderBy))
                    {
                        if (searchParams.OrderBy == "Active")
                            productBases = productBases.ApplyOrderByFilter(searchParams.OrderByDirection, p => p.Products.Any(product => product.Active));
                        else
                            productBases = productBases.ApplyOrderByFilter(searchParams, context);
                    }
                    else
                        productBases = productBases.ApplyOrderByFilter(searchParams.OrderByDirection, p => p.BaseSKU);



                    var ci = EntitiesExpressionHelper.MakeTranslationExpression<CatalogItem>("Catalog", "Name", ApplicationContext.Instance.CurrentLanguageID);
                    var c = EntitiesExpressionHelper.MakeTranslationExpression<Category>("Name", ApplicationContext.Instance.CurrentLanguageID);
                    
                    List<CatalogPeriod> listaCatalogPeriod = PeriodDataAccess.GetCatalogPeriodsOfOpenPeriod();

                    var pbs = productBases.Select(pb => new
                    {
                        pb.ProductBaseID,
                        pb.BaseSKU,
                        Active = pb.Products.Any(p => p.Active),
                        Name = !pb.Translations.Any(t => t.LanguageID == ApplicationContext.Instance.CurrentLanguageID) ? pb.Translations.Count() > 0 ? pb.Translations.FirstOrDefault().Name : "" : pb.Translations.FirstOrDefault(t => t.LanguageID == ApplicationContext.Instance.CurrentLanguageID).Name,
                        pb.ProductTypeID,
                        Catalogs = pb.Products.SelectMany(p => p.CatalogItems.AsQueryable().Select(ci)),
                        Categories = pb.Categories.AsQueryable().Select(c),
                        BPCS = pb.Products.FirstOrDefault().Properties.FirstOrDefault(p => p.ProductPropertyType.Name == "BPCS code"),
                        SAP = pb.Products.FirstOrDefault().Properties.FirstOrDefault(p => p.ProductPropertyType.Name == "SAP Code"),
                    });

                    pbs = pbs.ApplyPagination(searchParams);

                    var pbsList = pbs.ToList();
                    results.AddRange(pbsList.Select(pb => new ProductBaseSearchData()
                    {
                        ProductBaseID = pb.ProductBaseID,
                        BPCS = pb.BPCS != null ? pb.BPCS.PropertyValue : null,
                        SAPSKU = pb.SAP != null ? pb.SAP.PropertyValue : null,
                        SKU = pb.BaseSKU,
                        Active = pb.Active,
                        Name = pb.Name,
                        ProductTypeID = pb.ProductTypeID,
                        ProductType = SmallCollectionCache.Instance.ProductTypes.GetById(pb.ProductTypeID).GetTerm(),
                        //Catalogs = pb.Catalogs.Distinct().Join(", "),
                        Catalogs = pb.Catalogs.Join(listaCatalogPeriod,
                        catalogoItem => catalogoItem.ToString(),
                        catalogoPeriodo => "Catalog  " + catalogoPeriodo.PeriodID.ToString(), (catalogoItem, catalogoPeriodo) => catalogoItem).Distinct().Join(", "),
                        Categories = pb.Categories.Join(", ")
                    }));

                    if (!String.IsNullOrWhiteSpace(searchParams.SAPSKU) || !String.IsNullOrWhiteSpace(searchParams.BPCS) || !String.IsNullOrWhiteSpace(searchParams.Query))
                    {
                        var kitParents = GetKitParents(results, context);
                        if (kitParents.Any())
                        {
                            results.AddRange(kitParents);
                        }
                    }

                    return results;
                }
            });
        }

        public PaginatedList<ProductSearchData> GetCampaignInformation(ProductBaseSearchParameters searchParams)
        {
            SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Core"].ConnectionString);
            try
            {
                #region Parameters

                List<ProductSearchData> result = new List<ProductSearchData>();
                SqlParameter[] LstParameter = new SqlParameter[] 
                { 
                    new SqlParameter() { SqlDbType=System.Data.SqlDbType.NVarChar, Value = searchParams.PeriodID, ParameterName = "PeriodID"},
                    new SqlParameter() { SqlDbType=System.Data.SqlDbType.Int, Value = searchParams.LanguageID, ParameterName = "LanguageID"},
                    new SqlParameter() { SqlDbType=System.Data.SqlDbType.Int, Value = searchParams.ProductTypeID, ParameterName = "ProductTypeID"},
                    new SqlParameter() { SqlDbType=System.Data.SqlDbType.VarChar, Value = searchParams.Query, ParameterName = "SKUoName"},
                    new SqlParameter() { SqlDbType=System.Data.SqlDbType.VarChar, Value = searchParams.SAPSKU, ParameterName = "SAPSKU"}
                };

                #endregion

                #region GetData

                connection.Open();
                SqlDataReader dr = DataAccess.queryDatabase("[dbo].[upsGetCampaignInformation]", connection, LstParameter);

                while (dr.Read())
                {
                    ProductSearchData item = new ProductSearchData();

                    if (!dr.IsDBNull(dr.GetOrdinal("PeriodID"))) item.PeriodID = Convert.ToInt32(dr["PeriodID"]);
                    if (!dr.IsDBNull(dr.GetOrdinal("SKU"))) item.CUV = Convert.ToString(dr["SKU"]);
                    if (!dr.IsDBNull(dr.GetOrdinal("Name"))) item.Name = Convert.ToString(dr["Name"]);
                    if (!dr.IsDBNull(dr.GetOrdinal("ExternalCode"))) item.ExternalCode = Convert.ToInt32(dr["ExternalCode"]);
                    if (!dr.IsDBNull(dr.GetOrdinal("IsKit"))) item.IsKit = Convert.ToInt32(dr["IsKit"]);
                    if (!dr.IsDBNull(dr.GetOrdinal("SAPSKU"))) item.SAPSKU = Convert.ToString(dr["SAPSKU"]);
                    if (!dr.IsDBNull(dr.GetOrdinal("OfferType"))) item.OfferType = Convert.ToInt32(dr["OfferType"]);

                    if (!dr.IsDBNull(dr.GetOrdinal("PrecioTabela"))) item.PrecioTabela = Convert.ToDouble(dr["PrecioTabela"]);
                    if (!dr.IsDBNull(dr.GetOrdinal("PrecioComision"))) item.PrecioComision = Convert.ToDouble(dr["PrecioComision"]);
                    if (!dr.IsDBNull(dr.GetOrdinal("Puntos"))) item.Puntos = Convert.ToDouble(dr["Puntos"]);
                    if (!dr.IsDBNull(dr.GetOrdinal("PrecioPracticado"))) item.PrecioPracticado = Convert.ToDouble(dr["PrecioPracticado"]);
                    if (!dr.IsDBNull(dr.GetOrdinal("Tipo"))) item.Tipo = Convert.ToString(dr["Tipo"]);
                    if (!dr.IsDBNull(dr.GetOrdinal("Categorias"))) item.Categorias = Convert.ToString(dr["Categorias"]);
                    if (!dr.IsDBNull(dr.GetOrdinal("Catalogos"))) item.Catalogos = Convert.ToString(dr["Catalogos"]);
                    if (!dr.IsDBNull(dr.GetOrdinal("Situacion"))) item.Situacion = Convert.ToBoolean(dr["Situacion"]);

                    result.Add(item);
                }

                #endregion

                #region Convert to

                //List<ProductMatrix> result = new List<ProductMatrix>();

                //for (int i = 0; i < result.Count(); i++)
                //{
                //    result.Add((ProductMatrix)result[i]);
                //}

                #endregion

                #region Pagination

                IQueryable<ProductSearchData> matchingItems = result.AsQueryable<ProductSearchData>();
                var resultTotalCount = matchingItems.Count();
                if (searchParams.PageSize > 0)
                {
                    matchingItems = matchingItems.ApplyPagination(searchParams);
                    return matchingItems.ToPaginatedList<ProductSearchData>(searchParams, resultTotalCount);
                }
                else return matchingItems.ToPaginatedList<ProductSearchData>(searchParams, resultTotalCount);

                #endregion

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }

        private IEnumerable<ProductBaseSearchData> GetKitParents(PaginatedList<ProductBaseSearchData> sources, NetStepsEntities context)
        {
            var results = Enumerable.Empty<ProductBaseSearchData>();
            if (sources.Any())
            {
                var sourceIds = sources.Select(d => d.ProductBaseID).ToArray();

                var ci = EntitiesExpressionHelper.MakeTranslationExpression<CatalogItem>("Catalog", "Name", ApplicationContext.Instance.CurrentLanguageID);
                var c = EntitiesExpressionHelper.MakeTranslationExpression<Category>("Name", ApplicationContext.Instance.CurrentLanguageID);

                var parentIds = (from pb in context.ProductBases
                                 from p in pb.Products
                                 from pr in p.ParentProductRelations
                                 where sourceIds.Contains(pb.ProductBaseID)
                                 select pr.ParentProductID).Distinct().ToArray();

                if (parentIds.Any())
                {
                    var pbs = context.ProductBases.Where(pb => pb.Products.Any(p => parentIds.Contains(p.ProductID)) && !sourceIds.Contains(pb.ProductBaseID)).Select(pb => new
                    {
                        pb.ProductBaseID,
                        pb.BaseSKU,
                        Active = pb.Products.Any(p => p.Active),
                        Name = !pb.Translations.Any(t => t.LanguageID == ApplicationContext.Instance.CurrentLanguageID) ? pb.Translations.Count() > 0 ? pb.Translations.FirstOrDefault().Name : "" : pb.Translations.FirstOrDefault(t => t.LanguageID == ApplicationContext.Instance.CurrentLanguageID).Name,
                        pb.ProductTypeID,
                        Catalogs = pb.Products.SelectMany(p => p.CatalogItems.AsQueryable().Select(ci)),
                        Categories = pb.Categories.AsQueryable().Select(c),
                        BPCS = pb.Products.FirstOrDefault().Properties.FirstOrDefault(p => p.ProductPropertyType.Name == "BPCS code"),
                        SAP = pb.Products.FirstOrDefault().Properties.FirstOrDefault(p => p.ProductPropertyType.Name == "SAP Code"),
                    }).ToArray();
                    results = pbs.Select(pb => new ProductBaseSearchData()
                    {
                        ProductBaseID = pb.ProductBaseID,
                        BPCS = pb.BPCS != null ? pb.BPCS.PropertyValue : null,
                        SAPSKU = pb.SAP != null ? pb.SAP.PropertyValue : null,
                        SKU = pb.BaseSKU,
                        Active = pb.Active,
                        Name = pb.Name,
                        ProductTypeID = pb.ProductTypeID,
                        ProductType = SmallCollectionCache.Instance.ProductTypes.GetById(pb.ProductTypeID).GetTerm(),
                        Catalogs = pb.Catalogs.Distinct().Join(", "),
                        Categories = pb.Categories.Join(", ")
                    }).ToArray();
                }
            }
            return results;
        }

        public void ChangeActiveStatus(int productBaseID, bool active)
        {
            ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    context.ExecuteStoreCommand("UPDATE Products SET Active = @p0 WHERE ProductBaseID = @p1", active, productBaseID);
                }
            });
        }

        #region Load Helpers

        public override ProductBase LoadFull(int ProductBaseID)
        {
            var ProductBase = FirstOrDefaultFull(x => x.ProductBaseID == ProductBaseID);

            if (ProductBase == null)
            {
                throw new NetStepsDataException(string.Format("No ProductBase found with ProductBaseID = {0}.", ProductBaseID));
            }

            return ProductBase;
        }

        public override List<ProductBase> LoadBatchFull(IEnumerable<int> ProductBaseIDs)
        {
            return WhereFull(x => ProductBaseIDs.Contains(x.ProductBaseID));
        }

        public override List<ProductBase> LoadAllFull()
        {
            return WhereFull(x => true);
        }

        public override List<ProductBase> Where(Expression<Func<ProductBase, bool>> predicate)
        {
            return Where(predicate, ProductBase.Relations.LoadFull);
        }

        public virtual ProductBase FirstOrDefaultFull(Expression<Func<ProductBase, bool>> predicate)
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

        public virtual ProductBase FirstOrDefaultFull(Expression<Func<ProductBase, bool>> predicate, NetStepsEntities context)
        {
            Contract.Requires<ArgumentNullException>(predicate != null);
            Contract.Requires<ArgumentNullException>(context != null);

            return FirstOrDefault(predicate, ProductBase.Relations.LoadFull, context);
        }

        public virtual ProductBase FirstOrDefault(Expression<Func<ProductBase, bool>> predicate, ProductBase.Relations relations)
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

        public virtual ProductBase FirstOrDefault(Expression<Func<ProductBase, bool>> predicate, ProductBase.Relations relations, NetStepsEntities context)
        {
            Contract.Requires<ArgumentNullException>(predicate != null);
            Contract.Requires<ArgumentNullException>(context != null);

            var ProductBase = context.ProductBases
                .FirstOrDefault(predicate);

            if (ProductBase == null)
            {
                return null;
            }

            ProductBase.LoadRelations(context, relations);

            return ProductBase;
        }

        public virtual List<ProductBase> WhereFull(Expression<Func<ProductBase, bool>> predicate)
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

        public virtual List<ProductBase> WhereFull(Expression<Func<ProductBase, bool>> predicate, NetStepsEntities context)
        {
            Contract.Requires<ArgumentNullException>(predicate != null);
            Contract.Requires<ArgumentNullException>(context != null);

            return Where(predicate, ProductBase.Relations.LoadFull, context);
        }

        public virtual List<ProductBase> Where(Expression<Func<ProductBase, bool>> predicate, ProductBase.Relations relations)
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

        public virtual List<ProductBase> Where(Expression<Func<ProductBase, bool>> predicate, ProductBase.Relations relations, NetStepsEntities context)
        {
            Contract.Requires<ArgumentNullException>(predicate != null);
            Contract.Requires<ArgumentNullException>(context != null);

            var ProductBases = context.ProductBases
                .Where(predicate)
                .ToList();

            ProductBases.LoadRelations(context, relations);

            return ProductBases;
        }

        #endregion
    }
}
