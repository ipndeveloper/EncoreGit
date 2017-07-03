using System;
using System.Web.Mvc;
using NetSteps.Encore.Core.IoC;
using Cart.DemoWebsite.Areas.Cart.Cart.Models.Interfaces;

namespace Cart.DemoWebsite.Areas.Cart.Cart.Models.ModelBinders
{
	public class CartModelBinder : DefaultModelBinder
	{
		protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
		{
			var instantiation = Create.New<ICartModel>();

			bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, instantiation.GetType());
			bindingContext.ModelMetadata.Model = instantiation;

			return instantiation;
		}
	}
}