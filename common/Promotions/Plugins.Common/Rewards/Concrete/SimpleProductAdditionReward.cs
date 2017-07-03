using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Promotions.Common.ModelConcrete;
using NetSteps.Encore.Core.IoC;
using NetSteps.Data.Common.Services;
using NetSteps.Promotions.Plugins.Common.Rewards.Effects;
using NetSteps.OrderAdjustments.Common.Model;
using NetSteps.Promotions.Plugins.Common.Helpers;

namespace NetSteps.Promotions.Plugins.Common.Rewards.Concrete
{
	[Serializable]
	public class SimpleProductAdditionReward : BasePromotionReward, ISimpleProductAdditionReward
	{
		public SimpleProductAdditionReward()
		{
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

		public override string[] OrderOfApplication
		{
			get
			{
				List<string> order = new List<string>();
				order.AddRange(Effects.Keys.Where(key => key.StartsWith(SelectorPrefix)));
				order.AddRange(Effects.Keys.Where(key => !key.StartsWith(SelectorPrefix)));
				return order.ToArray();
			}
		}

		public override string PromotionRewardKind
		{
			get { return RewardKinds.SimpleProductAdditionReward; }
		}

		private IEnumerable<IAddItemPromotionRewardEffect> ProductAddingEffects
		{
			get
			{
				return Effects.Where(x => x.Key.StartsWith(SelectorPrefix)).Select(x => (IAddItemPromotionRewardEffect)x.Value.Extension);
			}
		}

		public IEnumerable<IProductOption> Products
		{
			get
			{
				return ProductAddingEffects.Select(x =>
													  {
														  var option = Create.New<IProductOption>();
														  option.ProductID = x.ProductID;
														  option.Quantity = x.Quantity;
														  return option;
													  });
			}
		}

		public void AddProduct(Helpers.IProductOption option)
		{
			var existing = ProductAddingEffects.SingleOrDefault(x => x.ProductID == option.ProductID);
			if (existing != null)
			{
				existing.Quantity = option.Quantity;
			}
			else
			{
				var newSelector = GenerateEffect<IAddItemPromotionRewardEffect>();
				((IAddItemPromotionRewardEffect)newSelector.Extension).ProductID = option.ProductID;
				((IAddItemPromotionRewardEffect)newSelector.Extension).Quantity = option.Quantity;
				Effects.Add(SelectorName(option.ProductID), newSelector);
			}
		}

		public void RemoveProduct(int productID)
		{
			var selectorName = SelectorName(productID);
			if (Effects.ContainsKey(selectorName))
			{
				Effects.Remove(selectorName);
			}
		}

		private string SelectorName(int productID)
		{
			return String.Format("{0}{1}", SelectorPrefix, productID);
		}

		private const string SelectorPrefix = "ProductID:";
	}
}
