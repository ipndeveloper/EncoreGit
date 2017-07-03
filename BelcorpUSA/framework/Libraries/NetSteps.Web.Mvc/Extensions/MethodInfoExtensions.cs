using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using NetSteps.Common.Extensions;

namespace NetSteps.Web.Mvc.Extensions
{
    public static class MethodInfoExtensions
    {
        public static object[] GetValuesFromRequest(this MethodInfo method, ControllerBase controller)
        {
            var parameters = method.GetParametersCached();
            var values = new List<object>();

            foreach (var parameter in parameters)
            {
                // Make a new binding context for each parameter to find its value in the request
                var bindingContext = new ModelBindingContext();
                bindingContext.FallbackToEmptyPrefix = true;
                bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, parameter.ParameterType);
                bindingContext.ModelName = parameter.Name;
                bindingContext.ModelState = controller.ViewData.ModelState;
                bindingContext.PropertyFilter = null;
                bindingContext.ValueProvider = controller.ValueProvider;

                // Try to bind the parameter value
                var binder = ModelBinders.Binders.GetBinder(parameter.ParameterType);
                var obj = binder.BindModel(controller.ControllerContext, bindingContext);
                if (obj == null
                    && (parameter.ParameterType.IsPrimitive || parameter.ParameterType.IsEnum)
                    && !parameter.ParameterType.IsNullableType())
                {
                    throw new ArgumentException(string.Format("The parameters dictionary contains a null entry for parameter '{0}' of non-nullable type '{1}' for method '{2}' in '{3}'. To make a parameter optional its type should be either a reference type or a Nullable type.",
                        parameter.Name, parameter.ParameterType.Name, method.Name, method.DeclaringType.FullName));
                }
                values.Add(obj);
            }

            return values.ToArray();
        }
    }
}
