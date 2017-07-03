using System.Collections.Generic;
using System.Diagnostics.Contracts;
using NetSteps.Encore.Core.IoC;
using nsCore.Areas.Products.Models.Promotions.Interfaces;

namespace nsCore.Areas.Products.Models.Promotions
{
	public interface IPickFromListOfProductsCartRewardModel : ICartRewardModel
	{
		int MaxQuantity { get; set; }
		IList<int> ProductIDs { get; set; }
        bool? IsEspecialPromotion { get; set; }

	}

	[ContainerRegister(typeof(IPickFromListOfProductsCartRewardModel), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
	public class PickFromListOfProductsCartRewardModel : IPickFromListOfProductsCartRewardModel
	{
		public IList<int> ProductIDs { get; set; }
		public int MaxQuantity { get; set; }
        public bool? IsEspecialPromotion { get; set; }

		public PickFromListOfProductsCartRewardModel()
		{
			ProductIDs = new List<int>();
            IsEspecialPromotion = false;
		}

		[ContractInvariantMethod]
		private void ObjectInvariant()
		{
			Contract.Invariant(MaxQuantity >= 0);
			Contract.Invariant(ProductIDs != null);
		}
	}
}