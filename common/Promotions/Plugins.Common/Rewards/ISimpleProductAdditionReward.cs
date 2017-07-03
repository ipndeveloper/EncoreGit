using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Promotions.Common.Model;
using NetSteps.Promotions.Plugins.Common.Helpers;

namespace NetSteps.Promotions.Plugins.Common.Rewards
{
	public interface ISimpleProductAdditionReward : IPromotionReward
	{
		IEnumerable<IProductOption> Products { get; }
		void AddProduct(IProductOption option);
		void RemoveProduct(int productID);
	}
}
