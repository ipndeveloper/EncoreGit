using NetSteps.Data.Entities;
using nsDistributor.Models.Shared;


namespace nsDistributor.Models.Checkout
{
	public class CheckoutReceiptModel
	{
		public Order Order { get; set; }

		public ICartModel CartModel { get; set; }

		public bool ContinueShopping { get; set; }
	}
}