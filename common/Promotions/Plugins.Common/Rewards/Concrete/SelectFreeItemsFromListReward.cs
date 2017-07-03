using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Promotions.Common.ModelConcrete;
using NetSteps.Promotions.Plugins.Common.Rewards.Effects;
using NetSteps.Data.Common.Services;
using NetSteps.Encore.Core.IoC;
using NetSteps.OrderAdjustments.Common.Model;

namespace NetSteps.Promotions.Plugins.Common.Rewards.Concrete
{
	[Serializable]
	public class SelectFreeItemsFromListReward : BasePromotionReward, ISelectFreeItemsFromListReward
	{
		public SelectFreeItemsFromListReward()
		{
			Effects.Add(RewardNames.ProductIDSelector, GenerateEffect<IUserProductSelectionRewardEffect>());
			var priceTypeService = Create.New<IPriceTypeService>();

			var currencyTypes = priceTypeService.GetCurrencyPriceTypes();
			foreach (var currencyType in currencyTypes)
			{
				var effect = GenerateEffect<IReduceOrderItemPropertyValuePromotionRewardEffect>();
				((IReduceOrderItemPropertyValuePromotionRewardEffect)effect.Extension).DefaultMarketID = 1;
				((IReduceOrderItemPropertyValuePromotionRewardEffect)effect.Extension).MarketDecimalOperands.Add(1, 1);
				((IReduceOrderItemPropertyValuePromotionRewardEffect)effect.Extension).OrderAdjustmentOrderLineOperationID = (int)OrderAdjustmentOrderLineOperationKind.Multiplier;
				((IReduceOrderItemPropertyValuePromotionRewardEffect)effect.Extension).OrderItemPropertyName = currencyType.Name;
				Effects.Add(currencyType.Name, effect);
			}

			var commissionTypes = priceTypeService.GetVolumePriceTypes();
			foreach (var commissionType in commissionTypes)
			{
				var effect = GenerateEffect<IReduceOrderItemPropertyValuePromotionRewardEffect>();
				var extension = (IReduceOrderItemPropertyValuePromotionRewardEffect)effect.Extension;
				extension.DefaultMarketID = 1;
				extension.MarketDecimalOperands.Add(1, 1);
				extension.OrderAdjustmentOrderLineOperationID = (int)OrderAdjustmentOrderLineOperationKind.Multiplier;
				extension.OrderItemPropertyName = commissionType.Name;
				Effects.Add(commissionType.Name, effect);
			}
		}

		public static class RewardNames
		{
			public const string ProductIDSelector = "ProductID Selector";
		}

		private IUserProductSelectionRewardEffect SelectionEffect
		{
			get
			{
				return (IUserProductSelectionRewardEffect)Effects[RewardNames.ProductIDSelector].Extension;
			}
		}

		public IEnumerable<Helpers.IProductOption> SelectionOptions
		{
			get { return SelectionEffect.Selections; }
		}

		public void AddSelectionOption(Helpers.IProductOption option)
		{
			var existing = SelectionEffect.Selections.SingleOrDefault(x => x.ProductID == option.ProductID);
			if (existing != null)
			{
				SelectionEffect.Selections.Remove(existing);
			}
			SelectionEffect.Selections.Add(option);
		}

		public void RemoveSelectionOption(int productID)
		{
			var existing = SelectionEffect.Selections.SingleOrDefault(x => x.ProductID == productID);
			if (existing != null)
			{
				SelectionEffect.Selections.Remove(existing);
			}

		}

		public int AllowedSelectionQuantity
		{
			get 
			{ 
				return SelectionEffect.SelectionsAllowed; 
			}
			set 
			{ 
				SelectionEffect.SelectionsAllowed = value; 
			}
		}

		public override string[] OrderOfApplication
		{
			get
			{
				List<string> order = new List<string>();
				order.Add(RewardNames.ProductIDSelector);
				order.AddRange(Effects.Keys.Where(key => !key.Equals(RewardNames.ProductIDSelector)));
				return order.ToArray();
			}
		}

		public override string PromotionRewardKind
		{
			get { return RewardKinds.SelectFreeItemsFromListReward; }
		}

	}
}
