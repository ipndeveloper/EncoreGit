using System.Collections.Generic;
using System.Linq;
using NetSteps.Data.Common.Services;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Encore.Core.IoC;
using NetSteps.Promotions.Plugins.Common.PromotionKinds;
using NetSteps.Web.Mvc.Helpers;
using nsCore.Areas.Products.Models.Promotions.Converters.Base;
using NetSteps.Promotions.Plugins.Common.Helpers;

namespace nsCore.Areas.Products.Models.Promotions.Converters
{
	[ContainerRegister(typeof(IPromotionModelConverter<PercentOffPromotionModel, IProductPromotionPercentDiscount>), RegistrationBehaviors.Default)]
	public class PercentageDiscountPromotionConverter : MultipleMarketStandardQualificationPromotionConverter<IProductPromotionPercentDiscount, PercentOffPromotionModel>, IPromotionModelConverter<PercentOffPromotionModel, IProductPromotionPercentDiscount>
	{
		public override IProductPromotionPercentDiscount Convert(PercentOffPromotionModel model)
		{
			var promotion = base.Convert(model);
			
            SetProducts(promotion, model.MarketIDs, model.PromotionProducts);
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
                    var currencyID = NetSteps.Data.Entities.Market.Load(promotion.DefaultMarketID).GetDefaultCurrencyID();
                    PromotionConvertCustomerSubtotalRangeCondition(promotion, (ICustomerSubtotalRangeCartCondition)model.CartCondition, currencyID);
                }
                else if (typeof(ICustomerQVRangeCartCondition).IsAssignableFrom(model.CartCondition.GetType()))
                {
                    var currencyID = NetSteps.Data.Entities.Market.Load(promotion.DefaultMarketID).GetDefaultCurrencyID();
                    PromotionConvertCustomerQVRangeCondition(promotion, (ICustomerQVRangeCartCondition)model.CartCondition, currencyID);
                }
            }

			return promotion;
		}
        private void PromotionConvertSingleProductCartCondition(IProductPromotionPercentDiscount promotion, ISingleProductCartCondition condition)
        {
            var option = Create.New<IProductOption>();
            option.ProductID = condition.ProductID;
            option.Quantity = condition.Quantity;
            promotion.AddRequiredProductOption(option);
        }

        private void PromotionConvertMultipleProductCartCondition(IProductPromotionPercentDiscount promotion, ICombinationOfProductsCartCondition condition)
        {
            foreach (var productID in condition.RequiredProductIDs)
            {
                var option = Create.New<IProductOption>();
                option.ProductID = productID;
                option.Quantity = 1;
                promotion.AddRequiredProductOption(option);
            }
        }

        private void PromotionConvertCustomerSubtotalRangeCondition(IProductPromotionPercentDiscount promotion, ICustomerSubtotalRangeCartCondition condition, int currencyID)
        {
            promotion.SetCustomerSubtotalRange(condition.MinimumSubtotal, condition.MaximumSubtotal, currencyID);

        }

        private void PromotionConvertCustomerQVRangeCondition(IProductPromotionPercentDiscount promotion, ICustomerQVRangeCartCondition condition, int currencyID)
        {
            promotion.SetCustomerPriceTypeTotalRange(condition.QVMinimumSubtotal, condition.QVMaximumSubtotal, currencyID, condition.PriceTypeID);
        }


		public void SetProducts(IProductPromotionPercentDiscount promotion, IEnumerable<int> marketIDs, List<PercentOffPromotionProductModel> productSelections)
		{
			var validProductIDs = productSelections.Select(selection => selection.ProductID);
			var removed = promotion.PromotedProductIDs.Where(existing => !validProductIDs.Contains(existing)).ToList();
			var priceTypeService = Create.New<IPriceTypeService>();
			foreach (var old in removed)
			{
				promotion.DeleteProductAdjustments(old);
			}
			if (marketIDs.Count() == 0)
			{ 
                var defaultMarket = SmallCollectionCache.Instance.Markets.FirstOrDefault(x => x.Active);
                promotion.DefaultMarketID = defaultMarket != null ? defaultMarket.MarketID : 1;
			}
			else
			{
				promotion.DefaultMarketID = marketIDs.First();
			}
			foreach (var selection in productSelections)
			{
				// if they are allowed to change the values per market, this will need to be re-tooled so that each market gets its own set of modifications.  For now
				// we can just use the default for all markets.
				var currencyPriceTypes = priceTypeService.GetCurrencyPriceTypes();
				foreach (var priceType in currencyPriceTypes)
				{
					promotion.AddProductAdjustment(selection.ProductID, priceType, promotion.DefaultMarketID, selection.RetailDiscountPercent / 100M);
					promotion.AddProductAdjustment(selection.ProductID, priceType, promotion.DefaultMarketID, selection.RetailDiscountPercent / 100M);
					promotion.AddProductAdjustment(selection.ProductID, priceType, promotion.DefaultMarketID, selection.RetailDiscountPercent / 100M);
				}
				var volumePriceTypes = priceTypeService.GetVolumePriceTypes();
				foreach (var priceType in volumePriceTypes)
				{
					if (priceType.PriceTypeID == (int)Constants.ProductPriceType.QV)
					{
						var discount = selection.QVDiscountPercent.HasValue ? selection.QVDiscountPercent.Value : 0;
						promotion.AddProductAdjustment(selection.ProductID, priceType, promotion.DefaultMarketID, discount / 100M);
					}
					else
					{
						var discount = selection.CVDiscountPercent.HasValue ? selection.CVDiscountPercent.Value : 0;
						promotion.AddProductAdjustment(selection.ProductID, priceType, promotion.DefaultMarketID, discount / 100M);
					}
				}
			}
		}

		public override PercentOffPromotionModel Convert(IProductPromotionPercentDiscount promotion)
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

		public void SetProducts(PercentOffPromotionModel model, IProductPromotionPercentDiscount promotion)
		{
			var productIDList = promotion.PromotedProductIDs.ToList();
			var repo = Create.New<InventoryBaseRepository>();
			var priceTypeService = Create.New<IPriceTypeService>();
			int currencyID = CoreContext.CurrentMarket.GetDefaultCurrencyID();
			foreach (var productID in productIDList)
			{
				var product = repo.GetProduct(productID);
				model.PromotionProducts.Add(new PercentOffPromotionProductModel()
				{
					ProductID = productID,
					Active = true,
					Name = product.Name,
					SKU = product.SKU,
					RetailPrice = product.GetPrice(Constants.ProductPriceType.Retail, currencyID),
					RetailDiscountPercent = promotion.GetAdjustmentAmount(productID, priceTypeService.GetPriceType((int)Constants.ProductPriceType.Retail), promotion.DefaultMarketID) * 100M,
					CVPrice = product.GetPrice(Constants.ProductPriceType.CV, currencyID),
					CVDiscountPercent = promotion.GetAdjustmentAmount(productID, priceTypeService.GetPriceType((int)Constants.ProductPriceType.CV), promotion.DefaultMarketID) * 100M,
					QVPrice = product.GetPrice(Constants.ProductPriceType.QV, currencyID),
					QVDiscountPercent = promotion.GetAdjustmentAmount(productID, priceTypeService.GetPriceType((int)Constants.ProductPriceType.QV), promotion.DefaultMarketID) * 100M,
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


    
        #endregion
	}
}