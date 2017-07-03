namespace NetSteps.Data.Entities.Repositories
{
    using System.Linq;
    using System;

    public partial class ProductPriceRepository : NetSteps.Data.Entities.Repositories.Interfaces.IProductPriceRepository
    {
        public decimal GetRetilPerItem(int ProductID) 
        {
            using (var DbContext = new EntityDBContext(ConnectionStrings.BelcorpCore))
            {
                var ReatilPerItem = (DbContext.ProductPrices.Where(r => r.ProductID == ProductID && r.ProductPriceTypeID == 1).Select(r=> r.Price));
                if (ReatilPerItem == null)
                    return Convert.ToDecimal("0.0");
                else
                    return Convert.ToDecimal(ReatilPerItem.FirstOrDefault());
            }
        }
    }
}
