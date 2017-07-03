using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NetSteps.Promotions.Common.Repository;
using NetSteps.Promotions.Plugins.Common;
using NetSteps.Promotions.Plugins.Common.Rewards.Effects;
using NetSteps.Promotions.Plugins.Rewards.Base;
using System.Diagnostics.Contracts;
using NetSteps.Promotions.Common.ModelConcrete;
using NetSteps.Promotions.Common.Model;
using NetSteps.OrderAdjustments.Common.Model;
using NetSteps.Data.Common.Context;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Promotions.Plugins.Rewards.Effects
{
	public class AddItemByFactorInOrderRewardEffectHandler : BasePromotionRewardEffectExtensionHandler<IAddItemByFactorInCartPromotionRewardEffect, IAddItemByFactorInCartPromotionRewardEffectRepository, IEncorePromotionsPluginsUnitOfWork>, IPromotionRewardGeneralEffectExtensionHandler
	{
		public virtual void AddEffectToOrderAdjustmentProfile(IOrderContext orderContext, IOrderAdjustmentProfile adjustmentProfile, IPromotionRewardEffect rewardEffect, PromotionQualificationResult matchResult, IPromotionRewardEffectResult effectResult)
		{
			Contract.Assert(orderContext != null);
			Contract.Assert(adjustmentProfile != null);
			Contract.Assert(rewardEffect != null);

			var effect = rewardEffect.Extension as IAddItemByFactorInCartPromotionRewardEffect;
			foreach (var orderCustomer in orderContext.Order.OrderCustomers)
			{
				if (matchResult.MatchForCustomerAccountID(orderCustomer.AccountID))
				{
					var nonKitItems = orderCustomer.AdjustableOrderItems;
					foreach (var orderItem in nonKitItems)
					{
						if (orderItem.ProductID == effect.ProductIDInCart)
						{
							var target = Create.New<IOrderAdjustmentProfileOrderItemTarget>();
							target.OrderCustomerAccountID = orderCustomer.AccountID;
							target.ProductID = effect.ProductIDToAdd;
							target.Quantity = (int)(effect.Factor * orderItem.Quantity);
							adjustmentProfile.OrderLineModificationTargets.Add(target);

							var addAdjust = Create.New<IOrderAdjustmentProfileOrderLineModification>();
							addAdjust.Description = String.Format("Added {0}.", target.Quantity);
							addAdjust.ModificationOperationID = (int)OrderAdjustmentOrderLineOperationKind.AddedItem;
							addAdjust.ModificationValue = target.Quantity;
							addAdjust.Property = "Added";
							target.Modifications.Add(addAdjust);
						}
					}
				}
			}
		}

		public override bool AreEqual(Promotions.Common.Model.IPromotionRewardEffectExtension effect1, Promotions.Common.Model.IPromotionRewardEffectExtension effect2)
		{
			if (!string.Equals(effect1.ExtensionProviderKey, effect2.ExtensionProviderKey))
				return false;
			if (effect1.PromotionRewardEffectID != effect2.PromotionRewardEffectID)
				return false;
			var effectExt1 = effect1 as IAddItemByFactorInCartPromotionRewardEffect;
			var effectExt2 = effect2 as IAddItemByFactorInCartPromotionRewardEffect;
			if (effectExt1.Factor != effectExt2.Factor)
				return false;
			if (effectExt1.MaximumRewardCount != effectExt2.MaximumRewardCount)
				return false;
			if (effectExt1.ProductIDInCart != effectExt2.ProductIDInCart)
				return false;
			if (effectExt1.ProductIDToAdd != effectExt2.ProductIDToAdd)
				return false;
			return true;
		}

		public override string GetProviderKey()
		{
			return NetStepsPromotionRewardEffectExtensionProviderKeys.AddItemByFactorInCart;
		}

		public override void CheckValidity(string promotionRewardEffectKey, IAddItemByFactorInCartPromotionRewardEffect rewardEffect, IPromotionState state)
		{
			if (rewardEffect.Factor <= 0)
			{
				state.AddConstructionError
					(
						String.Format("Promotion Reward Effect {0}", promotionRewardEffectKey),
						"Factor cannot be less than or equal to 0."
					);
			}
			if (rewardEffect.MaximumRewardCount < 0)
			{
				state.AddConstructionError
					(
						String.Format("Promotion Reward Effect {0}", promotionRewardEffectKey),
						"MaximumRewardCount cannot be less than 0."
					);
			}
			if (rewardEffect.ProductIDInCart <= 0)
			{
				state.AddConstructionError
					(
						String.Format("Promotion Reward Effect {0}", promotionRewardEffectKey),
						"ProductID in the cart cannot be less than or equal to 0."
					);
			}
			if (rewardEffect.ProductIDToAdd <= 0)
			{
				state.AddConstructionError
					(
						String.Format("Promotion Reward Effect {0}", promotionRewardEffectKey),
						"ProductID to add cannot be less than or equal to 0."
					);
			}
		}
	}
}
