using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Promotions.Common.ModelConcrete;
using NetSteps.Promotions.Plugins.Common.Rewards.Effects;
using NetSteps.Encore.Core.IoC;
using NetSteps.Promotions.Common.Model;
using NetSteps.Data.Common.Entities;
using NetSteps.Data.Common.Services;

namespace NetSteps.Promotions.Plugins.Common.Rewards.Concrete
{
	[Serializable]
	public class ProductPromotionReward : BasePromotionReward, IProductReward
	{

		public static class RewardNames 
		{
			public const string ProductIDSelector = "ProductID Selector";
		}

		public ProductPromotionReward()
		{
			Effects.Add(RewardNames.ProductIDSelector, GenerateSelectItemEffectEffect());
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
			get { return RewardKinds.ProductPromotionReward; }
		}

		public int ProductID 
		{
			get
			{
				if (SelectItem == null)
				{
					return 0;
				}
				else
				{
					return SelectItem.ProductID;
				}
			}
			set
			{
				SelectItem.ProductID = value;
			}
		}

		public ISelectItemWithProductIDPromotionRewardEffect SelectItem 
		{ 
			get
			{
				return (ISelectItemWithProductIDPromotionRewardEffect)base.Effects[RewardNames.ProductIDSelector].Extension;
			}
		}

		public IReduceOrderItemPropertyValuePromotionRewardEffect GetDiscountEffect(int marketID, IPriceType priceType)
		{
			if (Effects.ContainsKey(priceType.Name))
			{
				return (IReduceOrderItemPropertyValuePromotionRewardEffect)Effects[priceType.Name].Extension;
			}
			return null;
		}

		private IPromotionRewardEffect GenerateSelectItemEffectEffect()
		{
			return GenerateEffect<ISelectItemWithProductIDPromotionRewardEffect>();
		}

		private IPromotionRewardEffect GenerateOrderItemPropertyValueEffect(IPriceType priceType, int orderAdjustmentOrderLineOperationKindID)
		{
			var effect = GenerateEffect<IReduceOrderItemPropertyValuePromotionRewardEffect>();
			((IReduceOrderItemPropertyValuePromotionRewardEffect)effect.Extension).OrderAdjustmentOrderLineOperationID = orderAdjustmentOrderLineOperationKindID;
			((IReduceOrderItemPropertyValuePromotionRewardEffect)effect.Extension).OrderItemPropertyName = priceType.Name;
			return effect;
		}

		private IPromotionRewardEffect AddEffect(IPromotionRewardEffectExtension extension)
		{
			var effect = Create.New<IPromotionRewardEffect>();
			effect.Extension = extension;
			effect.ExtensionProviderKey = extension.ExtensionProviderKey;
			return effect;
		}

		public IReduceOrderItemPropertyValuePromotionRewardEffect GetPriceTypeEffect(IPriceType priceType)
		{
			if (Effects.ContainsKey(priceType.Name))
			{
				return (IReduceOrderItemPropertyValuePromotionRewardEffect)Effects[priceType.Name].Extension;
			}
			return null;
		}


		public bool ContainsPriceTypeEffect(IPriceType priceType)
		{
			return Effects.ContainsKey(priceType.Name);
		}

		public void AddPriceTypeEffect(IPriceType priceType, int orderAdjustmentOrderLineOperationKindID)
		{
			if (!ContainsPriceTypeEffect(priceType))
			{
				Effects.Add(priceType.Name, GenerateOrderItemPropertyValueEffect(priceType, orderAdjustmentOrderLineOperationKindID));
			}
		}

		public IEnumerable<IPriceType> PriceTypes
		{
			get
			{
				var priceTypeService = Create.New<IPriceTypeService>();
				return Effects.Keys.Where(key => !key.Equals(RewardNames.ProductIDSelector)).Select(key => priceTypeService.GetPriceType(key));
			}
		}


		public IEnumerable<IReduceOrderItemPropertyValuePromotionRewardEffect> PriceTypeEffects
		{
			get 
			{
				return Effects.Values.Select(x => x.Extension).OfType<IReduceOrderItemPropertyValuePromotionRewardEffect>();
			}
		
		}
	}
}
