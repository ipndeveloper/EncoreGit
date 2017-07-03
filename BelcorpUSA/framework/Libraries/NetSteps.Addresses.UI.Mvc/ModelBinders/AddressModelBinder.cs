using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using NetSteps.Addresses.UI.Common.Models;
using NetSteps.Addresses.UI.Common.Services;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Addresses.UI.Mvc.ModelBinders
{
	public class AddressModelBinder : DefaultModelBinder
	{

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType != typeof(IAddressUIModel))
                return base.BindModel(controllerContext, bindingContext);


            // Because the type is not an exact match of IAddressModel, during 
            // the default binding logic does not work for complex types
            // For example the ControllerAction(complexModel) where 
            // complexModel.ShippingAddress contains the address to be bound to
            var result = base.BindModel(controllerContext, bindingContext);
            var addressType = result.GetType();
            var props = addressType.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.FlattenHierarchy | System.Reflection.BindingFlags.Instance);
            var formatString = ((!String.IsNullOrEmpty(bindingContext.ModelName) && bindingContext.ModelName != "model") ? "{0}[{1}]" : "{1}");
            Type nullableType = typeof(Nullable<>);

            props.ToList().ForEach((i) =>
            {
                if (i.GetCustomAttributes(typeof(ScriptIgnoreAttribute), true).Length > 0) return;
                var requestPropertyName = String.Format(formatString, bindingContext.ModelName, i.Name);
                ValueProviderResult bindingProperty = bindingContext.ValueProvider.GetValue(requestPropertyName);

                if (bindingProperty != null)
                {
                    string bindingValue = bindingProperty.AttemptedValue;
                    if (i.PropertyType == typeof(string))
                    {
                        bindingValue = bindingValue == "null" ? string.Empty : bindingValue;
                        i.SetValue(result, bindingValue, null);
                    }
                    else if (i.PropertyType.IsPrimitive)
                    {
                        if (bindingValue == "null")
                        {
                            i.SetValue(result, null, null);
                        }
                        else
                        {
                            var convertedValue = Convert.ChangeType(bindingValue, i.PropertyType);
                            i.SetValue(result, convertedValue, null);
                        }
                    }
                    else if (i.PropertyType.IsGenericType && i.PropertyType.GetGenericTypeDefinition() == nullableType)
                    {
                        object newValue;

                        if (bindingValue == "null")
                        {
                            newValue = Activator.CreateInstance(i.PropertyType, new object[] { });
                        }
                        else
                        {
                            var convertedValue = Convert.ChangeType(bindingValue, i.PropertyType.GetGenericArguments()[0]);
                            newValue = Activator.CreateInstance(i.PropertyType, new object[] { convertedValue });
                        }
                        i.SetValue(result, newValue, null);
                    }
                    else
                    {
                        throw new NotSupportedException(
                            "An attempt to post a non-primitive property value to a server has been encountered"
                        );
                    }
                }


            });

            return result;
        }


		protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
		{
			object result = null;

            if (modelType == typeof(IAddressUIModel))
            {
                ValueProviderResult countryCode = null;
                if (bindingContext.ValueProvider.ContainsPrefix(bindingContext.ModelName))
                    countryCode = bindingContext.ValueProvider.GetValue(String.Concat(bindingContext.ModelName, "[CountryCode]"));
                else
                    countryCode = bindingContext.ValueProvider.GetValue("CountryCode");

                IAddressUIModel addrModel = null;

                if (countryCode != null && AddressModelRegistry.Instance.TryAddressModelInstance(countryCode.AttemptedValue, out addrModel))
                    result = addrModel;
                else
                    result = Create.New<IAddressUIModel>();

                return result as IAddressUIModel;
            }
            else
            {
                result = base.CreateModel(controllerContext, bindingContext, modelType);
                return result;
            }
		}
	}
}
