
namespace NetSteps.Data.Entities.Services
{
    public interface IInventoryListingService
    {
        bool IsAvailable(Product product);

        bool IsAvailable(int productId);

        bool IsOutOfStock(Product product);

        bool IsOutOfStock(int productId);
    }
}
