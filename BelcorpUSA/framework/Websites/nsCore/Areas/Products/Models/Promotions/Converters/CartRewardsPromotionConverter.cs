using System.Linq;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Encore.Core.IoC;
using NetSteps.Promotions.Plugins.Common.Helpers;
using NetSteps.Promotions.Plugins.Common.PromotionKinds;
using nsCore.Areas.Products.Models.Promotions.Converters.Base;
using NetSteps.Promotions.Plugins.Common.Rewards.Concrete;
using NetSteps.Promotions.Plugins.Common.Rewards.Effects;

namespace nsCore.Areas.Products.Models.Promotions.Converters
{
	[ContainerRegister(typeof(IPromotionModelConverter<CartRewardsPromotionModel, IOrderPromotionDefaultCartRewards>), RegistrationBehaviors.Default)]
	public class CartRewardsPromotionConverter : SingleMarketStandardQualificationPromotionConverter<IOrderPromotionDefaultCartRewards, CartRewardsPromotionModel>, IPromotionModelConverter<CartRewardsPromotionModel, IOrderPromotionDefaultCartRewards>
	{
		IOrderPromotionDefaultCartRewards IPromotionModelConverter<CartRewardsPromotionModel, IOrderPromotionDefaultCartRewards>.Convert(CartRewardsPromotionModel model)
		{
			return Convert(model);
		}

		public override IOrderPromotionDefaultCartRewards Convert(CartRewardsPromotionModel model)
		{
			var promotion = base.Convert(model);
			PromotionResetQualificationOptions(promotion);

			if (model.CartCondition != null)
			{
				if (typeof(ISingleProductCartCondition).IsAssignableFrom(model.CartCondition.GetType()))
				{
					PromotionConvertSingleProductCartCondition(promotion, (ISingleProductCartCondition)model.CartCondition);
				}
				else if (typeof(ICombinationOfProductsCartCondition).IsAssignableFrom(model.CartCondition.GetType()))
				{
					PromotionConvertMultipleProductCartCondition(promotion, (ICombinationOfProductsCartCondition)model.CartCondition);
				}
				else if (typeof(ICustomerSubtotalRangeCartCondition).IsAssignableFrom(model.CartCondition.GetType()))
				{
					var currencyID = NetSteps.Data.Entities.Market.Load(model.MarketID).GetDefaultCurrencyID();
                    PromotionConvertCustomerSubtotalRangeCondition(promotion, (ICustomerSubtotalRangeCartCondition)model.CartCondition, currencyID);
				}
                else if (typeof(ICustomerQVRangeCartCondition).IsAssignableFrom(model.CartCondition.GetType()))
                {
                    var currencyID = NetSteps.Data.Entities.Market.Load(model.MarketID).GetDefaultCurrencyID();
                    PromotionConvertCustomerQVRangeCondition(promotion, (ICustomerQVRangeCartCondition)model.CartCondition, currencyID);
                }
			}

			PromotionResetRewardOptions(promotion);

			if (typeof(IAddProductsToCartReward).IsAssignableFrom(model.CartReward.GetType()))
			{
				PromotionConvertAddedProductReward(promotion, (IAddProductsToCartReward)model.CartReward);
			}

			if (typeof(IPickFromListOfProductsCartRewardModel).IsAssignableFrom(model.CartReward.GetType()))
			{
				PromotionConvertSelectFromListReward(promotion, (IPickFromListOfProductsCartRewardModel)model.CartReward);
               
			}

			if (typeof(ICartDiscountCartReward).IsAssignableFrom(model.CartReward.GetType()))
			{
				PromotionConvertReducedSubtotalOrShippingReward(promotion, (ICartDiscountCartReward)model.CartReward);
			}

			return promotion;
		}

		#region Model to Promotion
		
		private void PromotionResetQualificationOptions(IOrderPromotionDefaultCartRewards promotion)
		{
			// remove total range qualifications
			
            promotion.RemoveCustomerSubtotalRanges();
            promotion.RemoveOrderSubtotalRanges();
            promotion.RemoveCustomerPriceTypeTotalRanges();
            promotion.RemoveOrderPriceTypeTotalRanges();
			
            // remove products
			var removed = promotion.RequiredProductOptions.Select(x => x.ProductID).ToList();
			foreach (var removedProductID in removed)
			{
				promotion.RemoveRequiredProductOption(removedProductID);
			}
		}

		private void PromotionConvertSingleProductCartCondition(IOrderPromotionDefaultCartRewards promotion, ISingleProductCartCondition condition)
		{
			var option = Create.New<IProductOption>();
			option.ProductID = condition.ProductID;
			option.Quantity = condition.Quantity;
			promotion.AddRequiredProductOption(option);
		}

		private void PromotionConvertMultipleProductCartCondition(IOrderPromotionDefaultCartRewards promotion, ICombinationOfProductsCartCondition condition)
		{
			foreach (var productID in condition.RequiredProductIDs)
			{
				var option = Create.New<IProductOption>();
				option.ProductID = productID;
				option.Quantity = 1;
				promotion.AddRequiredProductOption(option);
			}
		}

		private void PromotionConvertCustomerSubtotalRangeCondition(IOrderPromotionDefaultCartRewards promotion, ICustomerSubtotalRangeCartCondition condition, int currencyID)
		{
				promotion.SetCustomerSubtotalRange(condition.MinimumSubtotal, condition.MaximumSubtotal, currencyID);
		
        }

        private void PromotionConvertCustomerQVRangeCondition(IOrderPromotionDefaultCartRewards promotion, ICustomerQVRangeCartCondition condition, int currencyID)
        {
            promotion.SetCustomerPriceTypeTotalRange(condition.QVMinimumSubtotal, condition.QVMaximumSubtotal, currencyID, condition.PriceTypeID);
	    }

		private void PromotionResetRewardOptions(IOrderPromotionDefaultCartRewards promotion)
		{
			// remove added items
			var productIDs = promotion.ProductsAddedAsReward.Select(x => x.ProductID).ToList();
			foreach (var productID in productIDs)
			{
				promotion.RemoveRewardProduct(productID);
			}

			// remove product selections
			var removed = promotion.ProductSelectionsAsReward.Select(x => x.ProductID).ToList();
			foreach (var removedProductID in removed)
			{
				promotion.RemoveRewardProductSelection(removedProductID);
			}

			// remove order subtotal reduction
			promotion.ReduceSubtotalByPercent = 0;

			// remove shipping total reduction
			promotion.FreeShippingAsReward = false;
		}

		private void PromotionConvertAddedProductReward(IOrderPromotionDefaultCartRewards promotion, IAddProductsToCartReward reward)
		{
			foreach (var productAndQuantity in reward.ProductIDQuantities)
			{
				var option = Create.New<IProductOption>();
				option.ProductID = productAndQuantity.Key;
				option.Quantity = productAndQuantity.Value;
				promotion.AddRewardProduct(option);
			}
		}

		private void PromotionConvertSelectFromListReward(IOrderPromotionDefaultCartRewards promotion, IPickFromListOfProductsCartRewardModel reward)
		{
			promotion.ProductSelectionsMaximumCount = reward.MaxQuantity;
            var rw = (SelectFreeItemsFromListReward)promotion.PromotionRewards.First().Value;
            var effect = rw.Effects["ProductID Selector"];
            var extension = (IUserProductSelectionRewardEffect)(effect.Extension);
            extension.IsEspecialPromotion = reward.IsEspecialPromotion;
            
            foreach (var productID in reward.ProductIDs)
			{
				
                var option = Create.New<IProductOption>();
				option.ProductID = productID;
				option.Quantity = 1;
				promotion.AddRewardProductSelection(option);
			}
		}

		private void PromotionConvertReducedSubtotalOrShippingReward(IOrderPromotionDefaultCartRewards promotion, ICartDiscountCartReward reward)
		{
			if (reward.DiscountPercent.HasValue)
			{
				promotion.ReduceSubtotalByPercent = reward.DiscountPercent.Value / 100;
			}
			promotion.FreeShippingAsReward = reward.FreeShipping;
		}

		#endregion

		CartRewardsPromotionModel IPromotionModelConverter<CartRewardsPromotionModel, IOrderPromotionDefaultCartRewards>.Convert(IOrderPromotionDefaultCartRewards promotion)
		{
			return Convert(promotion);
		}

		public override CartRewardsPromotionModel Convert(IOrderPromotionDefaultCartRewards promotion)
		{
			var model = base.Convert(promotion);

			#region Qualifications

			if (promotion.RequiredProductOptions.Count() == 0)
			{
				// minimum subtotal
                if (promotion.PromotionQualifications.Any(x => x.Value.ExtensionProviderKey == NetSteps.Promotions.Plugins.Common.Qualifications.QualificationExtensionProviderKeys.CustomerPriceTypeTotalRangeProviderKey))
                {
                    model.CartCondition = ModelConvertCustomerQVRangeQualification(promotion);
                }
                else
                {
                    model.CartCondition = ModelConvertCustomerSubtotalRangeQualification(promotion);
                }
			}
			else if (promotion.RequiredProductOptions.Count() == 1)
			{
				model.CartCondition = ModelConvertSingleProductQualification(promotion);
			}
			else
			{
				model.CartCondition = ModelConvertMultipleProductQualification(promotion);
			}

			#endregion
			
			if (promotion.ProductsAddedAsReward.Count() > 0)
			{
				model.CartReward = ModelConvertProductsAddedReward(promotion);
			}
			else if (promotion.ProductSelectionsAsReward.Count() > 0)
			{
				model.CartReward = ModelConvertProductSelectionReward(promotion);
			}
			else
			{
				model.CartReward = ModelConvertReduceTotalsReward(promotion);
			}

			return model;
		}

		#region PromotionToModel

        private ICustomerQVRangeCartCondition ModelConvertCustomerQVRangeQualification(IOrderPromotionDefaultCartRewards promotion)
        {
            var model = Create.New<ICustomerQVRangeCartCondition>();
            var defaultRange = promotion.GetDefaultCustomerPriceTypeTotalRange();
            model.QVMinimumSubtotal = defaultRange.Minimum;
            model.QVMaximumSubtotal = defaultRange.Maximum;
            model.PriceTypeID = promotion.PriceTypeTotalRangeProductPriceTypeID;
            return model;
        }

		private ICustomerSubtotalRangeCartCondition ModelConvertCustomerSubtotalRangeQualification(IOrderPromotionDefaultCartRewards promotion)
		{
            var model = Create.New<ICustomerSubtotalRangeCartCondition>();
			var defaultRange = promotion.GetDefaultCustomerSubtotalRange();
            model.MinimumSubtotal = defaultRange.Minimum;
            model.MaximumSubtotal = defaultRange.Maximum;
			return model;
		}


		private ISingleProductCartCondition ModelConvertSingleProductQualification(IOrderPromotionDefaultCartRewards promotion)
		{
			var model = Create.New<ISingleProductCartCondition>();
			var option = promotion.RequiredProductOptions.First();
			model.ProductID = option.ProductID;
			model.Quantity = option.Quantity;
			return model;
		}


		private ICombinationOfProductsCartCondition ModelConvertMultipleProductQualification(IOrderPromotionDefaultCartRewards promotion)
		{
			var model = Create.New<ICombinationOfProductsCartCondition>();
			foreach (var option in promotion.RequiredProductOptions)
			{
				model.RequiredProductIDs.Add(option.ProductID);
			}
			return model;
		}


		private IAddProductsToCartReward ModelConvertProductsAddedReward(IOrderPromotionDefaultCartRewards promotion)
		{
			var model = Create.New<IAddProductsToCartReward>();
			foreach (var option in promotion.ProductsAddedAsReward)
			{
				model.ProductIDQuantities.Add(option.ProductID, option.Quantity);
			}
			return model;
		}


		private IPickFromListOfProductsCartRewardModel ModelConvertProductSelectionReward(IOrderPromotionDefaultCartRewards promotion)
		{
			var model = Create.New<IPickFromListOfProductsCartRewardModel>();
			model.MaxQuantity = promotion.ProductSelectionsMaximumCount;

            var rw = (SelectFreeItemsFromListReward)promotion.PromotionRewards.First().Value;
            var effect = rw.Effects["ProductID Selector"];
            var extension = (IUserProductSelectionRewardEffect)(effect.Extension);
            model.IsEspecialPromotion = extension.IsEspecialPromotion;


			foreach (var option in promotion.ProductSelectionsAsReward)
			{
				model.ProductIDs.Add(option.ProductID);
			}
			return model;
		}

		private ICartDiscountCartReward ModelConvertReduceTotalsReward(IOrderPromotionDefaultCartRewards promotion)
		{
			var model = Create.New<ICartDiscountCartReward>();
			model.DiscountPercent = promotion.ReduceSubtotalByPercent * 100;
			model.FreeShipping = promotion.FreeShippingAsReward;
			return model;
		}

		#endregion

	}
}