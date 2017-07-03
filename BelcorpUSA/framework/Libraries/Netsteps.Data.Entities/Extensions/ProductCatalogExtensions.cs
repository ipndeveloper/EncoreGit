using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;

namespace NetSteps.Data.Entities.Extensions
{
    public class ProductCatalogExtensions
    {
        public List<ProductCatalogSearchData> ProductCatalog = null;
        public ProductCatalogExtensions()
        {
            ProductCatalog = new List<ProductCatalogSearchData>();
        }

        public void InsertarProductCatalog(ProductCatalogSearchData param)
        {
            ProductCatalog.Add(param);
        }

    }
}
