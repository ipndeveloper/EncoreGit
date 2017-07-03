using System;
using System.Web.Mvc;
using NetSteps.Encore.Core.IoC;
using nsCore.Areas.Products.Models.Promotions.Interfaces;

namespace nsCore.Areas.Products.Models.Promotions.ModelBinders
{
	public class CartRewardModelBinder : DefaultModelBinder
	{
		protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
		{
			ICartRewardModel instantiation;

			CartRewardType conditionType = (CartRewardType)bindingContext.ValueProvider.GetValue("CartRewardType").ConvertTo(typeof(int));
			switch (conditionType)
			{
				case CartRewardType.AddProductsToCart:
					instantiation = Create.New<IAddProductsToCartReward>();
					break;
				case CartRewardType.PickFromListOfProducts:
					instantiation = Create.New<IPickFromListOfProductsCartRewardModel>();
					break;
				case CartRewardType.DiscountOrFreeShipping:
					instantiation = Create.New<ICartDiscountCartReward>();
					break;
				default:
					throw new InvalidOperationException("Invalid cart reward type");
			}

			bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, instantiation.GetType());
			return instantiation;
		}
	}
}