using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Promotions.Common.Model;
using NetSteps.Extensibility.Core;
using NetSteps.Promotions.Common;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Promotions.Service
{
	public class PromotionValidator : IPromotionValidator
	{
		public IPromotionState CheckValidity(IPromotion promotion)
		{
			var state = Create.New<IPromotionState>();

			#region promotion status type
			switch (promotion.PromotionStatusTypeID)
			{
				case (int)PromotionStatus.Archived:
				case (int)PromotionStatus.Disabled:
				case (int)PromotionStatus.Enabled:
				case (int)PromotionStatus.Error:
				case (int)PromotionStatus.Obsolete:
					break;
				default:
					state.AddConstructionError("Promotion", String.Format("Invalid PromotionStatusID {0}.", promotion.PromotionStatusTypeID));
					break;
			}
			#endregion

			checkQualificationValidity(promotion.PromotionQualifications, state);
			checkRewardValidity(promotion.PromotionRewards, state);
			return state;
		}

		private void checkQualificationValidity(IDictionary<string, IPromotionQualificationExtension> qualifications, IPromotionState state)
		{
			if (qualifications == null)
			{
				state.AddConstructionError("Promotion", "Promotion Qualifications collection cannot be null");
			}
			else
			{
				var extensionProviderRegistry = Create.New<IDataObjectExtensionProviderRegistry>();

				foreach (var qualificationSet in qualifications)
				{
					if (qualificationSet.Value == null)
					{
						state.AddConstructionError(String.Format("Promotion Qualification {0}", qualificationSet.Key), "Qualification extension cannot be null");
					}
					else
					{
						var handler = extensionProviderRegistry.RetrieveExtensionProvider<IPromotionQualificationHandler>(qualificationSet.Value.ExtensionProviderKey);
						handler.CheckValidity(qualificationSet.Key, qualificationSet.Value, state);
					}
				}
			}
		}



		private void checkRewardValidity(IDictionary<string, IPromotionReward> rewards, IPromotionState state)
		{
			if (rewards == null)
			{
				state.AddConstructionError("Promotion", "Promotion Rewards collection cannot be null");
			}
			else
			{
				var handlerManager = Create.New<IPromotionRewardHandlerManager>();

				foreach (var rewardsSet in rewards)
				{
					if (rewardsSet.Value == null)
					{
						state.AddConstructionError(String.Format("Promotion Rewards {0}", rewardsSet.Key), "Reward extension cannot be null");
					}
					else
					{
						var handler = handlerManager.GetRewardHandler(rewardsSet.Value.PromotionRewardKind);
						handler.CheckValidity(rewardsSet.Key, rewardsSet.Value, state);
					}
				}
			}
		}
	}
}
