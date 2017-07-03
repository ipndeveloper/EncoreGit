using System;
using System.Data.Objects;
using System.Linq;
using NetSteps.Common.Exceptions;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class StoreFrontRepository
    {
        protected override Func<NetStepsEntities, IQueryable<StoreFront>> loadAllFullQuery
        {
            get
            {
                return CompiledQuery.Compile<NetStepsEntities, IQueryable<StoreFront>>(context => context.StoreFronts
                    .Include("MarketStoreFronts")
                    .Include("MarketStoreFronts.Market")
                    .Include("Catalogs")
                    .Include("Catalogs.Translations")
                    .Include("Catalogs.AccountTypes")
                );
            }
        }

        /// <summary>
        /// Loads a store front with lots of related objects. Used by the inventory cache.
        /// Uses a sequence of small SELECTs for better SQL performance.
        /// Uses .Include only for parent objects, NOT for child objects.
        /// </summary>
        public StoreFront GetStoreFrontWithInventory(int storeFrontId)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                using (new NetSteps.Common.OperationDebugTimer("Load Inventory: Repository"))
                {
                    // This is called from the inventory cache, where we would prefer a long background loading time to caching an exception.
                    context.CommandTimeout = 300;

                    // StoreFront
                    var storeFront = context.StoreFronts
                        .FirstOrDefault(sf => sf.StoreFrontID == storeFrontId);
                    if (storeFront == null) throw new NetStepsDataException(string.Format("No StoreFront found with StoreFrontID = {1}.", storeFrontId));

                    // MarketStoreFronts
                    // MarketStoreFronts.Market
                    context.MarketStoreFronts
                        .Include("Market")
                        .Where(msf => msf.StoreFrontID == storeFrontId)
                        .ToList();

                    // Catalogs
                    var catalogs = context.Catalogs
                        .Include("AccountTypes")
                        .Where(c => c.StoreFronts.Any(sf => sf.StoreFrontID == storeFrontId))
                        .ToList();
                    storeFront.Catalogs.AddRange(catalogs);

                    // Catalogs.Translations
                    var allCatalogTranslations = context.DescriptionTranslations
                        .Where(dt => dt.Catalogs.Any(c => c.StoreFronts.Any(sf => sf.StoreFrontID == storeFrontId)))
                        .Select(dt => new { Translation = dt, CatalogIDs = dt.Catalogs.Select(c => c.CatalogID) })
                        .ToList();
                    foreach (var catalog in catalogs)
                    {
                        var catalogTranslations = allCatalogTranslations
                            .Where(ct => ct.CatalogIDs.Contains(catalog.CatalogID))
                            .Select(ct => ct.Translation);
                        catalog.Translations.AddRange(catalogTranslations);
                    }

                    // Catalogs.CatalogItems
                    // Catalogs.CatalogItems.Product
                    // Catalogs.CatalogItems.Product.ProductBase
                    var catalogItems = context.CatalogItems
                        .Include("Product.ProductBase")
                        .Where(ci => ci.Catalog.StoreFronts.Any(sf => sf.StoreFrontID == storeFrontId))
                        .ToList();
                    var products = catalogItems
                        .Select(ci => ci.Product)
                        .Distinct();
                    var productBases = products
                        .Select(p => p.ProductBase)
                        .Distinct();

                    // Catalogs.CatalogItems.Product.Translations
                    var allProductTranslations = context.DescriptionTranslations
                        .Where(dt => dt.Products.Any(p => p.CatalogItems.Any(ci => ci.Catalog.StoreFronts.Any(sf => sf.StoreFrontID == storeFrontId))))
                        .Select(dt => new { Translation = dt, ProductIds = dt.Products.Select(p => p.ProductID) })
                        .ToList();
                    foreach (var product in products)
                    {
                        var productTranslations = allProductTranslations
                            .Where(pt => pt.ProductIds.Contains(product.ProductID))
                            .Select(pt => pt.Translation);
                        product.Translations.AddRange(productTranslations);
                    }

                    // Catalogs.CatalogItems.Product.Prices
                    // Catalogs.CatalogItems.Product.Prices.ProductPriceType
                    context.ProductPrices
                        .Include("ProductPriceType")
                        .Where(pp => pp.Product.CatalogItems.Any(ci => ci.Catalog.StoreFronts.Any(sf => sf.StoreFrontID == storeFrontId)))
                        .ToList();

                    // Catalogs.CatalogItems.Product.Prices.ProductPriceType.AccountPriceTypes
                    context.AccountPriceTypes
                        .Where(apt => apt.ProductPriceType.ProductPrices.Any(pp => pp.Product.CatalogItems.Any(ci => ci.Catalog.StoreFronts.Any(sf => sf.StoreFrontID == storeFrontId))))
                        .ToList();

                    // Catalogs.CatalogItems.Product.DynamicKits
                    context.DynamicKits
                        .Where(dk => dk.Product.CatalogItems.Any(ci => ci.Catalog.StoreFronts.Any(sf => sf.StoreFrontID == storeFrontId)))
                        .ToList();

                    // Catalogs.CatalogItems.Product.DynamicKits.DynamicKitGroups
                    var dynamicKitGroups = context.DynamicKitGroups
                        .Where(dkg => dkg.DynamicKit.Product.CatalogItems.Any(ci => ci.Catalog.StoreFronts.Any(sf => sf.StoreFrontID == storeFrontId)))
                        .ToList();

                    // Catalogs.CatalogItems.Product.DynamicKits.DynamicKitGroups.DynamicKitGroupRules
                    context.DynamicKitGroupRules
                        .Where(dkgr => dkgr.DynamicKitGroup.DynamicKit.Product.CatalogItems.Any(ci => ci.Catalog.StoreFronts.Any(sf => sf.StoreFrontID == storeFrontId)))
                        .ToList();

                    // Catalogs.CatalogItems.Product.DynamicKits.DynamicKitGroups.Translations
                    var allDynamicKitGroupTranslations = context.DescriptionTranslations
                        .Where(dt => dt.DynamicKitGroups.Any(dkg => dkg.DynamicKit.Product.CatalogItems.Any(ci => ci.Catalog.StoreFronts.Any(sf => sf.StoreFrontID == storeFrontId))))
                        .Select(dt => new { Translation = dt, DynamicKitGroupIDs = dt.DynamicKitGroups.Select(dkg => dkg.DynamicKitGroupID) })
                        .ToList();
                    foreach (var dynamicKitGroup in dynamicKitGroups)
                    {
                        var dynamicKitGroupTranslations = allDynamicKitGroupTranslations
                            .Where(dkgt => dkgt.DynamicKitGroupIDs.Contains(dynamicKitGroup.DynamicKitGroupID))
                            .Select(pt => pt.Translation);
                        dynamicKitGroup.Translations.AddRange(dynamicKitGroupTranslations);
                    }

                    // Catalogs.CatalogItems.Product.Properties
                    // Catalogs.CatalogItems.Product.Properties.ProductPropertyValue
                    context.ProductProperties
                        .Include("ProductPropertyValue")
                        .Where(pp => pp.Product.CatalogItems.Any(ci => ci.Catalog.StoreFronts.Any(sf => sf.StoreFrontID == storeFrontId)))
                        .ToList();

                    // Catalogs.CatalogItems.Product.Files
                    context.ProductFiles
                        .Where(pf => pf.Product.CatalogItems.Any(ci => ci.Catalog.StoreFronts.Any(sf => sf.StoreFrontID == storeFrontId)))
                        .ToList();

                    // Catalogs.CatalogItems.Product.ChildProductRelations
                    // Catalogs.CatalogItems.Product.ParentProductRelations
                    context.ProductRelations
                        .Where(pr =>
                            pr.Product.CatalogItems.Any(ci => ci.Catalog.StoreFronts.Any(sf => sf.StoreFrontID == storeFrontId))
                            || pr.Product1.CatalogItems.Any(ci => ci.Catalog.StoreFronts.Any(sf => sf.StoreFrontID == storeFrontId)))
                        .ToList();

                    // Catalogs.CatalogItems.Product.TaxCategories
                    context.TaxCategories
                        .Where(tc => tc.Products.Any(p => p.CatalogItems.Any(ci => ci.Catalog.StoreFronts.Any(sf => sf.StoreFrontID == storeFrontId))))
                        .ToList();

                    // Catalogs.CatalogItems.Product.WarehouseProducts
                    // Catalogs.CatalogItems.Product.WarehouseProducts.Warehouse
                    context.WarehouseProducts
                        .Include("Warehouse")
                        .Where(wp => wp.Product.CatalogItems.Any(ci => ci.Catalog.StoreFronts.Any(sf => sf.StoreFrontID == storeFrontId)))
                        .ToList();

                    // Catalogs.CatalogItems.Product.WarehouseProducts.Warehouse.ShippingRegions
                    context.ShippingRegions
                        .Where(sr => sr.Warehouse.WarehouseProducts.Any(wp => wp.Product.CatalogItems.Any(ci => ci.Catalog.StoreFronts.Any(sf => sf.StoreFrontID == storeFrontId))))
                        .ToList();

                    // Catalogs.CatalogItems.Product.ProductBase.ProductBaseProperties
                    // Catalogs.CatalogItems.Product.ProductBase.ProductBaseProperties.ProductPropertyType
                    var productBaseProperties = context.ProductBaseProperties
                        .Include("ProductPropertyType")
                        .Where(pbp => pbp.ProductBas.Products.Any(p => p.CatalogItems.Any(ci => ci.Catalog.StoreFronts.Any(sf => sf.StoreFrontID == storeFrontId))))
                        .ToList();

                    // Catalogs.CatalogItems.Product.ProductBase.ProductBaseProperties.ProductPropertyType.ProductPropertyValues
                    context.ProductPropertyValues
                        .Where(ppv => ppv.ProductPropertyType.ProductBaseProperties.Any(pbp => pbp.ProductBas.Products.Any(p => p.CatalogItems.Any(ci => ci.Catalog.StoreFronts.Any(sf => sf.StoreFrontID == storeFrontId)))))
                        .ToList();
                    
                    // Catalogs.CatalogItems.Product.ProductBase.ProductType
                    context.ProductTypes
                        .Where(pt => pt.ProductBases.Any(pb => pb.Products.Any(p => p.CatalogItems.Any(ci => ci.Catalog.StoreFronts.Any(sf => sf.StoreFrontID == storeFrontId)))))
                        .ToList();

                    // Catalogs.CatalogItems.Product.ProductBase.ProductType.ProductPropertyTypes
                    context.ProductPropertyTypes
                        .Where(ppt => ppt.ProductTypes.Any(pt => pt.ProductBases.Any(pb => pb.Products.Any(p => p.CatalogItems.Any(ci => ci.Catalog.StoreFronts.Any(sf => sf.StoreFrontID == storeFrontId))))))
                        .ToList();

                    // Catalogs.CatalogItems.Product.ProductBase.Categories
                    var allCategories = context.Categories
                        .Where(c => c.ProductBases.Any(pb => pb.Products.Any(p => p.CatalogItems.Any(ci => ci.Catalog.StoreFronts.Any(sf => sf.StoreFrontID == storeFrontId)))))
                        .Select(c => new { Category = c, ProductBaseIDs = c.ProductBases.Select(pb => pb.ProductBaseID) })
                        .ToList();
                    foreach (var productBase in productBases)
                    {
                        var productBaseCategories = allCategories
                            .Where(x => x.ProductBaseIDs.Contains(productBase.ProductBaseID))
                            .Select(x => x.Category);
                        productBase.Categories.AddRange(productBaseCategories);
                    }

                    // Catalogs.CatalogItems.Product.ProductBase.Categories.Translations
                    context.CategoryTranslations
                        .Where(ct => ct.Category.ProductBases.Any(pb => pb.Products.Any(p => p.CatalogItems.Any(ci => ci.Catalog.StoreFronts.Any(sf => sf.StoreFrontID == storeFrontId)))))
                        .ToList();

                    return storeFront;
                }
            });
        }
    }
}
