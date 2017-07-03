using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.IoC;
using NetSteps.Promotions.Plugins.Common.Rewards.Effects;
using NetSteps.Promotions.Plugins.Rewards.Base;
using NetSteps.Promotions.Plugins.EntityModel;
using NetSteps.Promotions.Plugins.Common.Helpers;

namespace NetSteps.Promotions.Plugins.Rewards.Effects
{
	[ContainerRegister(typeof(IUserProductSelectionRewardEffectRepository), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
	public class UserProductSelectionRewardEffectRepository : BasePromotionRewardEffectExtensionRepository<IUserProductSelectionRewardEffect, PromotionRewardEffectUserProductSelection>, IUserProductSelectionRewardEffectRepository
	{
		protected override PromotionRewardEffectUserProductSelection Convert(IUserProductSelectionRewardEffect dto)
		{
			PromotionRewardEffectUserProductSelection entity = base.Convert(dto);
			if (dto.Selections != null)
			{
				foreach (var selection in dto.Selections)
				{
					entity.PromotionRewardEffectUserProductSelectionValues.Add(new PromotionRewardEffectUserProductSelectionValue() { ProductID = selection.ProductID, Quantity = selection.Quantity });
				}
			}
			return entity;
		}

		protected override IUserProductSelectionRewardEffect Convert(PromotionRewardEffectUserProductSelection entity)
		{
			var extension = base.Convert(entity);
			foreach (var item in entity.PromotionRewardEffectUserProductSelectionValues)
			{
				var option = Create.New<IProductOption>();
				option.ProductID = item.ProductID;
				option.Quantity = item.Quantity;
				extension.Selections.Add(option);
			}
			return extension;
		}

		protected override string[] Includes
		{
			get
			{
				return new string[] { "PromotionRewardEffectUserProductSelectionValues" };
			}
		}
	}
}
