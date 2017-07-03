using System.Collections.Generic;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;

namespace nsCore.Areas.Products.Models
{
    public class VariantProductModel
    {
        public int Index { get; set; }
        public string Sku { get; set; }
        public string Name { get; set; }
        public List<ProductProperty> Properties { get; set; }
        public Product Product { get; set; }
        public int? ProductId { get; set; }
        public bool Active { get; set; }
        public int CodigoSap { get; set; }
        public int OffertType { get; set; }
        public int ExternalCode { get; set; }
        public VariantsCUVsSearchData ProductVariant{get;set;}
    }
}