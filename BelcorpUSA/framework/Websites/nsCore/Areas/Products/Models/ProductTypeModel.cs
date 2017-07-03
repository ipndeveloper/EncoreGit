using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;

namespace nsCore.Areas.Products.Models
{
    public class ProductTypeModel
    {
        public int ProductTypeID { get; set; }
        public string Name { get; set; }
        public string TermName { get; set; }
        public bool Active { get; set; }
    
        public ProductTypeModel LoadResources(ProductType productType)
        {
            ProductTypeID = productType.ProductTypeID;
            Name = productType.Name;
            TermName = Translation.GetTerm(productType.TermName);
            Active = productType.Active;

            return this;
        }
    }
}