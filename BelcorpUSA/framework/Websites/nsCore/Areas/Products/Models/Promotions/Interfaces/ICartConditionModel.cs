
namespace nsCore.Areas.Products.Models.Promotions.Interfaces
{
    public interface ICartConditionModel
    {
    }

	public enum CartConditionType
	{
		SingleProduct,
		CombinationOfProducts,
		CustomerSubtotalRange,
        CustomerQVRange
	}
}
