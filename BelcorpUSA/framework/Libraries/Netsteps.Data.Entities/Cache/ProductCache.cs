using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Core.Cache;
using NetSteps.Data.Entities.Cache.Resolvers;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities.Cache
{
    public static class ProductCache
    {
        private static readonly IProductRepository __productRepository = new ProductRepository();
        private static readonly IProductBaseRepository __productBaseRepository = new ProductBaseRepository();

        private static readonly ICache<string, Tuple<string, int>> __productsBySKU = new ActiveMruLocalMemoryCache<string, Tuple<string, int>>("ProductsBySKU", new ProductBySKUResolver(__productRepository));
        private static readonly ICacheMany<int, Tuple<int, int>> __productIdToProductBaseId = new ActiveMruLocalMemoryCache<int, Tuple<int, int>>("ProductIdToProductBaseId", new ProductIdProductBaseIdResolver(__productRepository));
        private static readonly ICacheMany<int, ProductBase> __productsBasesById = new ActiveMruLocalMemoryCache<int, ProductBase>("ProductBasesById", new DelegatedDemuxCacheManyItemResolver<int, ProductBase>(ProductBaseByIdResolver, ProductBaseByIdsResolver));

        private static bool ProductBaseByIdResolver(int id, out ProductBase productBase)
        {
            productBase = __productBaseRepository.LoadFull(id);
            if (productBase != null)
            {
                MapProductsToBase(productBase.ProductBaseID, productBase.Products.Select(p => p.ProductID).ToArray());
            }
            return productBase != null;
        }

        private static bool ProductBaseByIdsResolver(IEnumerable<int> ids, out IEnumerable<KeyValuePair<int, ProductBase>> productBases)
        {
            productBases = __productBaseRepository.Where(pb => ids.Contains(pb.ProductBaseID)).Select(pb => new KeyValuePair<int, ProductBase>(pb.ProductBaseID, pb));
            if (productBases != null && productBases.Any())
            {
                foreach (var item in productBases)
                {
                    MapProductsToBase(item.Key, item.Value.Products.Select(p => p.ProductID).ToArray());
                }
            }
            return (productBases != null && productBases.Count() == ids.Count());
        }

        public static Product GetProductById(int productId)
        {
            Product product = null;

            if (productId > 0)
            {
                Tuple<int, int> productBaseIdMap;
                if (__productIdToProductBaseId.TryGet(productId, out productBaseIdMap))
                {
                    ProductBase prodBase;
                    if (__productsBasesById.TryGet(productBaseIdMap.Item2, out prodBase))
                    {
                        product = prodBase.Products.Where(p => p.ProductID == productId).FirstOrDefault();
                    }
                }
            }

            return product;
        }
        public static Product GetProductwithoutCache(int productId)
        {
            Product product = null;
            ProductRepository prepo = new ProductRepository();
            if (productId > 0)
            {
                product = prepo.LoadFull(productId);//.Where(p => p.ProductID == productId).FirstOrDefault();                                   
            }
            return product;
        }

       

        public static IEnumerable<Product> GetProductsByIds(IEnumerable<int> productIds)
        {
            //int isActive;
            if (productIds == null || !productIds.Any()) return Enumerable.Empty<Product>();

            IEnumerable<Product> products = null;
            IEnumerable<Tuple<int, int>> productBaseIdMap;

            //List<int> productIdsActi = new List<int>();
            //foreach (var item in productIds)
            //{
            //    if (Order.IsActiveProductMaterial(item) > 0)
            //    {
            //        productIdsActi.Add(item);
            //    }
            //}

            List<int> productIdsActi = Order.TakeActiveProductMaterial(productIds);

            if (productIdsActi == null || !productIdsActi.Any()) return Enumerable.Empty<Product>();


            if (__productIdToProductBaseId.TryGetMany(productIdsActi, out productBaseIdMap))
            {
                //Recorrer todos los productos. 
                IEnumerable<ProductBase> prodBases;
                var retur = __productsBasesById.TryGetMany(productBaseIdMap.Select(t => t.Item2).Distinct().OrderBy(i => i).ToArray(), out prodBases);

                if (__productsBasesById.TryGetMany(productBaseIdMap.Select(t => t.Item2).Distinct().OrderBy(i => i).ToArray(), out prodBases))
                {
                    foreach (var item in prodBases)
                    {
                        //isActive = Order.IsActiveProductMaterial(item.ProductBaseID);
                        //if (isActive == 1)
                        //{
                        if (products == null)
                            products = item.Products.Where(p => productIdsActi.Contains(p.ProductID)).ToArray();
                        else
                            products = products.Union(item.Products.Where(p => productIdsActi.Contains(p.ProductID)).ToArray());
                        //}						
                    }
                }
            }

            return products;
        }

        private static void MapProductsToBase(int baseId, IEnumerable<int> productIds)
        {
            if (productIds != null && productIds.Any())
                __productIdToProductBaseId.TryAddAny(productIds.Select(i => new KeyValuePair<int, Tuple<int, int>>(i, Tuple.Create(i, baseId))));
        }

        public static Product GetProductBySKU(string sku)
        {
            Product result = null;

            if (!String.IsNullOrWhiteSpace(sku))
            {
                Tuple<string, int> skuIdMap;
                if (__productsBySKU.TryGet(sku, out skuIdMap))
                {
                    result = GetProductById(skuIdMap.Item2);
                }
            }

            return result;
        }
    }
}