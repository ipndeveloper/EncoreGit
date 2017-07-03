using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NetSteps.Encore.Core.IoC;
using NetSteps.Promotions.Plugins.Rewards;
using NetSteps.Promotions.Common;

namespace nsCore.Areas.Products.Models.Promotions.Converters
{
	[ContainerRegister(typeof(IPromotionModelConverter<PickFromListOfProductsCartRewardModel, IPickFromListOfProductsCartReward>), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
	public class ShopperCanPickFromListOfFreeProductsCartRewardConverter : IPromotionModelConverter<PickFromListOfProductsCartRewardModel, IPickFromListOfProductsCartReward>
	{
		public IPickFromListOfProductsCartReward Convert(PickFromListOfProductsCartRewardModel model)
		{
			IPickFromListOfProductsCartReward cartReward;
			if (model.PromotionID != 0)
			{
				var promotionService = Create.New<IPromotionService>();
				cartReward = promotionService.GetPromotion<IPickFromListOfProductsCartReward>(model.PromotionID);
			}
			else
			{
				cartReward = Create.New<IPickFromListOfProductsCartReward>();
			}

			var conversion = new CartRewardConversion(cartReward);
			conversion.SetDescription(model.Name);
			conversion.SetAccountTitles(model.PaidAsTitleIDs, 0);
			conversion.SetOrderTypeIDs(model.OrderTypeIDs);


			return conversion.CartReward;
		}

		private class CartRewardConversion
		{
			public CartRewardConversion(IPickFromListOfProductsCartReward cartReward)
			{
				CartReward = cartReward;
			}

			public IPickFromListOfProductsCartReward CartReward { get; private set; }

			internal void SetDescription(string name)
			{
				CartReward.Description = name;
			}

			internal void SetMarketIDs(IList<int> marketIDs)
			{

			}

			internal void SetActiveImmediatley(bool activeImmediatley)
			{

			}

			internal void SetOneTimeUse(bool oneTimeUse)
			{

			}
			
			internal void SetPromotionCode(string promotionCode)
			{

			}

			internal void SetAccountTitles(IList<int> accountTitleIDs, int titleTypeID)
			{

			}

			internal void SetOrderTypeIDs(IList<int> orderTypeIDs)
			{

			}

		}

		public PickFromListOfProductsCartRewardModel Convert(IPickFromListOfProductsCartReward cartReward)
		{
			PickFromListOfProductsCartRewardModel model = null;

			return model;
		}

		private class ModelConversion
		{
			public ModelConversion(IPickFromListOfProductsCartReward cartReward)
			{
				CartReward = cartReward;
				Model = new PickFromListOfProductsCartRewardModel();
			}

			public IPickFromListOfProductsCartRewardModel Model { get; private set; }

			public IPickFromListOfProductsCartReward CartReward { get; private set; }

			internal void SetDescription(string name)
			{

			}

			internal void SetMarketIDs(IList<int> marketIDs)
			{

			}

			internal void SetActiveImmediatley(bool activeImmediatley)
			{

			}

			internal void SetOneTimeUse(bool oneTimeUse)
			{

			}

			internal void SetPromotionCode(string promotionCode)
			{

			}

			internal void SetAccountTitles(IList<int> accountTitleIDs, int titleTypeID)
			{

			}

			internal void SetOrderTypeIDs(IList<int> orderTypeIDs)
			{

			}
		}
	}
}
