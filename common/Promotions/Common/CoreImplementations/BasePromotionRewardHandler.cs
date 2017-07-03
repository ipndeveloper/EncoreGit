using System;
using System.Collections.Generic;

using NetSteps.Diagnostics.Utilities;
using NetSteps.Encore.Core.IoC;
using NetSteps.Extensibility.Core;
using NetSteps.OrderAdjustments.Common.Model;
using NetSteps.Promotions.Common.Model;
using NetSteps.Promotions.Common.Repository;

namespace NetSteps.Promotions.Common.CoreImplementations
{
	public abstract class BasePromotionRewardHandler : IPromotionRewardHandler
	{

		public virtual bool RequiresRemoveNotification
		{
			get { return false; }
		}

		public virtual void Remove(Model.IPromotionReward reward, Data.Common.Context.IOrderContext orderContext)
		{
			// no implementation necessary for base implementation.
		}

		public virtual bool RequiresCommitNotification
		{
			get { return false; }
		}

		public virtual void Commit(Model.IPromotionReward reward, Data.Common.Context.IOrderContext orderContext)
		{
			// no implementation necessary for base implementation
		}

		public virtual bool AreEqual(IPromotionReward reward1, IPromotionReward reward2)
		{
			if (!string.Equals(reward1.PromotionRewardKind, reward2.PromotionRewardKind))
			{
				return false;
			}

			if (reward1.Effects.Count != reward2.Effects.Count)
			{
				return false;
			}

			var registry = Create.New<IDataObjectExtensionProviderRegistry>();
			foreach (var key in reward1.Effects.Keys)
			{
				if (!string.Equals(reward1.Effects[key].ExtensionProviderKey, reward2.Effects[key].ExtensionProviderKey))
				{
					return false;
				}

				var handler = registry.RetrieveExtensionProvider<IPromotionRewardEffectExtensionHandler>(reward1.Effects[key].ExtensionProviderKey);
				if (!handler.AreEqual(reward1.Effects[key].Extension as IPromotionRewardEffectExtension,
						reward2.Effects[key].Extension as IPromotionRewardEffectExtension))
				{
					return false;
				}
			}

			return true;
		}

		public virtual void AddRewardToAdjustmentProfile(Data.Common.Context.IOrderContext orderContext, OrderAdjustments.Common.Model.IOrderAdjustmentProfile adjustmentProfile, Model.IPromotionReward reward, ModelConcrete.PromotionQualificationResult matchResult)
		{
			var registry = Create.New<IDataObjectExtensionProviderRegistry>();
			var effectResult = Create.New<IPromotionRewardEffectResult>();
			var targets = new List<IOrderAdjustmentProfileOrderItemTarget>();

			foreach (var effectName in reward.OrderOfApplication)
			{
				var handler = registry.RetrieveExtensionProvider<IPromotionRewardEffectExtensionHandler>(reward.Effects[effectName].ExtensionProviderKey);

				if (handler is IPromotionRewardTargetingEffectExtensionHandler)
				{
					targets.AddRange(((IPromotionRewardTargetingEffectExtensionHandler)handler).CreateOrderLineModificationTargets(orderContext, adjustmentProfile, reward.Effects[effectName] as IPromotionRewardEffect, matchResult, effectResult));
				}
				else if (handler is IPromotionRewardTargetedEffectExtensionHandler)
				{
					((IPromotionRewardTargetedEffectExtensionHandler)handler).AddEffectToOrderLineModificationTargets(orderContext, targets, reward.Effects[effectName] as IPromotionRewardEffect, matchResult, effectResult);
				}
				else if (handler is IPromotionRewardGeneralEffectExtensionHandler)
				{
					((IPromotionRewardGeneralEffectExtensionHandler)handler).AddEffectToOrderAdjustmentProfile(orderContext, adjustmentProfile, reward.Effects[effectName] as IPromotionRewardEffect, matchResult, effectResult);
				}
				else
				{
					throw new NotImplementedException("Unknown IPromotionRewardEffectExtensionHandler type.");
				}
			}
		}

		public abstract string PromotionRewardKind { get; }


		public virtual void CheckValidity(string promotionKey, IPromotionReward promotionReward, IPromotionState state)
		{
			var dataObjectExtensionProviderRegistry = Create.New<IDataObjectExtensionProviderRegistry>();

			if (promotionReward.Effects == null)
			{
				const string message = "Promotion Rewards collection is null.";
				this.TraceError(message);
				state.AddConstructionError("Promotion", message);
			}
			else
			{
				foreach (var effect in promotionReward.Effects)
				{
					if (effect.Value == null)
					{
						var message = string.Format("Promotion Reward {0} has a null effect.", effect.Key);
						this.TraceError(message);
						state.AddConstructionError(string.Format("Promotion Reward {0}", promotionKey), message);
					}
					else if (effect.Value.Extension == null)
					{
						var message = string.Format("Promotion Reward {0} Effect {1} has a null extension.", effect.Key, effect.Value);
						this.TraceError(message);
						state.AddConstructionError(string.Format("Promotion Reward {0}", promotionKey), message);
					}
					else
					{
						var handler = (IPromotionRewardEffectExtensionHandler)dataObjectExtensionProviderRegistry.RetrieveExtensionProvider(effect.Value.ExtensionProviderKey);
						handler.CheckValidity(effect.Key, effect.Value, state);
					}
				}
			}
		}
	}
}
