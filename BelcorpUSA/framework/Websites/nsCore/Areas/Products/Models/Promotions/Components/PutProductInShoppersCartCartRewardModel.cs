using System.Collections.Generic;
using System.Diagnostics.Contracts;
using NetSteps.Encore.Core.IoC;
using nsCore.Areas.Products.Models.Promotions.Interfaces;

namespace nsCore.Areas.Products.Models.Promotions
{
	public interface IAddProductsToCartReward : ICartRewardModel
	{
		IDictionary<int, int> ProductIDQuantities { get; set; }
        int ProductPriceTypeID { get; set; }
	}

	[ContainerRegister(typeof(IAddProductsToCartReward), RegistrationBehaviors.Default, ScopeBehavior=ScopeBehavior.InstancePerRequest)]
	public class AddProductsToCartRewardModel : IAddProductsToCartReward
	{
		public IDictionary<int, int> ProductIDQuantities { get; set; }

        public int ProductPriceTypeID { get; set; }

		public AddProductsToCartRewardModel()
		{
			ProductIDQuantities = new Dictionary<int, int>();
		}

		public void AddProductToDictionary(int productId, int quantity)
		{
			if (ProductIDQuantities.ContainsKey(productId))
			{
				ProductIDQuantities[productId] = quantity;
			}
			else
			{
				ProductIDQuantities.Add(productId, quantity);
			}
			
		}

		public void RemoveProductFromDictionary(int productId)
		{
			if (ProductIDQuantities.ContainsKey(productId))
			{
				ProductIDQuantities.Remove(productId);
			}
		}

		[ContractInvariantMethod]
		private void ObjectInvariant()
		{
			Contract.Invariant(ProductIDQuantities != null);
		}
	}
}