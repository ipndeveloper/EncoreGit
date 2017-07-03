using System.Collections.Generic;
using System.Linq;
using NetSteps.Data.Common.Services;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Encore.Core.IoC;
using NetSteps.Promotions.Plugins.Common.PromotionKinds;
using nsCore.Areas.Products.Models.Promotions.Converters.Base;
using NetSteps.Promotions.Plugins.Common.Helpers;

namespace nsCore.Areas.Products.Models.Promotions.Converters
{
	[ContainerRegister(typeof(IPromotionModelConverter<FlatDiscountPromotionModel, IProductPromotionFlatDiscount>), RegistrationBehaviors.Default)]
	public class FlatDiscountPromotionConverter : SingleMarketStandardQualificationPromotionConverter<IProductPromotionFlatDiscount, FlatDiscountPromotionModel>, IPromotionModelConverter<FlatDiscountPromotionModel, IProductPromotionFlatDiscount>
	{
		IProductPromotionFlatDiscount IPromotionModelConverter<FlatDiscountPromotionModel, IProductPromotionFlatDiscount>.Convert(FlatDiscountPromotionModel model)
		{
			return Convert(model);
		}

		public override IProductPromotionFlatDiscount Convert(FlatDiscountPromotionModel model)
		{
			var promotion = base.Convert(model);
			
            SetProducts(promotion, model.PromotionProducts);

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

			return promotion;
		}

        private void PromotionConvertSingleProductCartCondition(IProductPromotionFlatDiscount promotion, ISingleProductCartCondition condition)
        {
            var option = Create.New<IProductOption>();
            option.ProductID = condition.ProductID;
            option.Quantity = condition.Quantity;
            promotion.AddRequiredProductOption(option);
        }

        private void PromotionConvertMultipleProductCartCondition(IProductPromotionFlatDiscount promotion, ICombinationOfProductsCartCondition condition)
        {
            foreach (var productID in condition.RequiredProductIDs)
            {
                var option = Create.New<IProductOption>();
                option.ProductID = productID;
                option.Quantity = 1;
                promotion.AddRequiredProductOption(option);
            }
        }

        private void PromotionConvertCustomerSubtotalRangeCondition(IProductPromotionFlatDiscount promotion, ICustomerSubtotalRangeCartCondition condition, int currencyID)
        {
            promotion.SetCustomerSubtotalRange(condition.MinimumSubtotal, condition.MaximumSubtotal, currencyID);

        }

        private void PromotionConvertCustomerQVRangeCondition(IProductPromotionFlatDiscount promotion, ICustomerQVRangeCartCondition condition, int currencyID)
        {
            promotion.SetCustomerPriceTypeTotalRange(condition.QVMinimumSubtotal, condition.QVMaximumSubtotal, currencyID, condition.PriceTypeID);
        }


		internal void SetProducts(IProductPromotionFlatDiscount promotion, List<FlatDiscountPromotionProductModel> productSelections)
		{
			var validProductIDs = productSelections.Select(selection => selection.ProductID);
			var removed = promotion.PromotedProductIDs.Where(existing => !validProductIDs.Contains(existing)).ToList();
			var priceTypeService = Create.New<IPriceTypeService>();
			foreach (var old in removed)
			{
				promotion.DeleteProductAdjustments(old);
			}

			foreach (var selection in productSelections)
			{
				var priceDiscount = selection.RetailDiscountPrice;
				var currencyPriceTypes = priceTypeService.GetCurrencyPriceTypes();
				foreach (var priceType in currencyPriceTypes)
				{
					promotion.AddProductAdjustment(selection.ProductID, priceType, priceDiscount);
				}
				var volumePriceTypes = priceTypeService.GetVolumePriceTypes();
				foreach (var priceType in volumePriceTypes)
				{

					if (priceType.PriceTypeID == (int)Constants.ProductPriceType.QV && selection.QVDiscountPrice.HasValue)
					{
						var qVDiscount = selection.QVDiscountPrice.Value;
						promotion.AddProductAdjustment(selection.ProductID, priceType , qVDiscount);
					}
					if (priceType.PriceTypeID != (int)Constants.ProductPriceType.QV && selection.CVDiscountPrice.HasValue)
					{
						var cVDiscount = selection.CVDiscountPrice.Value;
						promotion.AddProductAdjustment(selection.ProductID, priceType, cVDiscount);
					}
				}
			}
		}

		FlatDiscountPromotionModel IPromotionModelConverter<FlatDiscountPromotionModel, IProductPromotionFlatDiscount>.Convert(IProductPromotionFlatDiscount promotion)
		{
			return Convert(promotion);
		}

		public override FlatDiscountPromotionModel Convert(IProductPromotionFlatDiscount promotion)
		{
			var model = base.Convert(promotion);
			SetProducts(model, promotion);
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
            return model;
		}

        


		internal void SetProducts(FlatDiscountPromotionModel model, IProductPromotionFlatDiscount promotion)
		{
			var productIDList = promotion.PromotedProductIDs.ToList();
			var inventory = Create.New<InventoryBaseRepository>();
			var currencyID = Market.Load(promotion.MarketID).GetDefaultCurrencyID();
			var priceTypeService = Create.New<IPriceTypeService>();

			foreach (var productID in productIDList)
			{
				var product = inventory.GetProduct(productID);
				var priceDiscount = promotion.GetAdjustmentAmount(productID, priceTypeService.GetPriceType((int)Constants.ProductPriceType.Retail));
				var qVDiscount = promotion.GetAdjustmentAmount(productID, priceTypeService.GetPriceType((int)Constants.ProductPriceType.QV));
				var cVDiscount = promotion.GetAdjustmentAmount(productID, priceTypeService.GetPriceType((int)Constants.ProductPriceType.CV));

				model.PromotionProducts.Add(new FlatDiscountPromotionProductModel
				{
					ProductID = productID,
					Active = true,
					Name = product.Name,
					SKU = product.SKU,
					RetailPrice = product.GetPrice(Constants.ProductPriceType.Retail, currencyID),
					RetailDiscountPrice = priceDiscount,
					CVPrice = product.GetPrice(Constants.ProductPriceType.CV, currencyID),
					CVDiscountPrice = cVDiscount,
					QVPrice = product.GetPrice(Constants.ProductPriceType.QV, currencyID),
					QVDiscountPrice = qVDiscount,
				});
			}
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


    }
        #endregion

}