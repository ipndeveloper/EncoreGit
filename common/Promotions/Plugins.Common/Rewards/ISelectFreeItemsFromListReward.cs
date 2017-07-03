using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Promotions.Common.Model;
using NetSteps.Promotions.Plugins.Common.Helpers;

namespace NetSteps.Promotions.Plugins.Common.Rewards
{
	public interface ISelectFreeItemsFromListReward : IPromotionReward
	{
		IEnumerable<IProductOption> SelectionOptions { get; }
		void AddSelectionOption(IProductOption option);
		void RemoveSelectionOption(int productID);
		int AllowedSelectionQuantity { get; set; }
	}
}
