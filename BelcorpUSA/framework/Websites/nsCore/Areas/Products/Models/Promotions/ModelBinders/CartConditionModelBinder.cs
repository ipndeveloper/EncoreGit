using System;
using System.Web.Mvc;
using NetSteps.Encore.Core.IoC;
using nsCore.Areas.Products.Models.Promotions.Interfaces;

namespace nsCore.Areas.Products.Models.Promotions.ModelBinders
{
	public class CartConditionModelBinder : DefaultModelBinder
	{
		protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
		{
			ICartConditionModel instantiation;

			CartConditionType conditionType = (CartConditionType)bindingContext.ValueProvider.GetValue("CartConditionType").ConvertTo(typeof(int));
			switch (conditionType)
			{
				case CartConditionType.SingleProduct:
					instantiation = Create.New<ISingleProductCartCondition>();
					break;
				case CartConditionType.CombinationOfProducts:
					instantiation = Create.New<ICombinationOfProductsCartCondition>();
					break;
                case CartConditionType.CustomerSubtotalRange:
                    instantiation = Create.New<ICustomerSubtotalRangeCartCondition>();
                    break;
                case CartConditionType.CustomerQVRange:
                    instantiation = Create.New<ICustomerQVRangeCartCondition>();
                    break;
                default:
					throw new InvalidOperationException("Invalid cart condition type");
			}

			bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, instantiation.GetType());
			bindingContext.ModelMetadata.Model = instantiation;
			return instantiation;
		}
	}
}