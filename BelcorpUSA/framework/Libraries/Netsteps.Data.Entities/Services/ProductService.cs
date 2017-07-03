using NetSteps.Data.Common.Services;

namespace NetSteps.Data.Entities.Services
{
    public class ProductService : IProductService
    {
        public Common.Entities.IProduct Load(int productID)
        {
            return Product.Load(productID);
        }
    }
}
