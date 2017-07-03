using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NetSteps.Encore.Core.IoC;
using NetSteps.Promotions.Plugins.Common.PromotionKinds;
using NetSteps.Promotions.Common;

namespace nsCore.Areas.Products.Models.Promotions.Converters
{
	//[ContainerRegister(typeof(IPromotionModelConverter<AddProductsToCartRewardModel, IPickFromListOfProductsCartReward>), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
	public class PutProductsInShoppersCartCartRewardConverter
	{
		public IPickFromListOfProductsCartReward Convert(AddProductsToCartRewardModel model)
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
			//conversion.SetDescription()

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

		public AddProductsToCartRewardModel Convert(IPickFromListOfProductsCartReward cartReward)
		{
			AddProductsToCartRewardModel model = null;

			return model;
		}

		private class ModelConversion
		{
			public ModelConversion(IPickFromListOfProductsCartReward cartReward)
			{
				CartReward = cartReward;
				Model = new AddProductsToCartRewardModel();
			}

			public AddProductsToCartRewardModel Model { get; private set; }

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