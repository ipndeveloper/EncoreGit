using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using NetSteps.Common.Extensions;

namespace NetSteps.Web.Mvc.Business.Inheritance
{
    public class OverridableControllerFactory : DefaultControllerFactory
    {
		private readonly IDictionary<string, Type> _overrideTypes;

		public OverridableControllerFactory(IEnumerable<Assembly> assemblies)
		{
			Contract.Requires<ArgumentNullException>(assemblies != null);

			_overrideTypes = GetTypes(assemblies);
		}

        protected override Type GetControllerType(RequestContext requestContext, string controllerName)
        {
            var area = requestContext.RouteData.DataTokens["area"];

			string key;
			if (area != null)
			{
				key = string.Format("Areas.{0}.Controllers.{1}Controller", area, controllerName);
			}
			else
			{
				key = string.Format("Controllers.{0}Controller", controllerName);
			}
			
			Type type;
			if (_overrideTypes.TryGetValue(key, out type))
			{
				return type;
			}

            return base.GetControllerType(requestContext, controllerName);
        }

		[System.Diagnostics.DebuggerStepThrough]
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null || !typeof(IController).IsAssignableFrom(controllerType))
            {
                // Base throws the appropriate exception type (usually a 404).
                // If you are debugging and you are hitting exceptions here, that is SUPPOSED to happen!
                // To ignore these exceptions, in Visual Studio, go to Debug -> Exceptions, ignore System.Web.HttpException. - Lundy
                return base.GetControllerInstance(requestContext, controllerType);
            }
            
            return controllerType.NewFast() as IController;
        }
		
		#region Helpers
		private IDictionary<string, Type> GetTypes(IEnumerable<Assembly> assemblies)
		{
			Contract.Requires<ArgumentNullException>(assemblies != null);

			var types = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);

			foreach (var assembly in assemblies)
			{
				var assemblyPrefix = assembly.GetName().Name + ".";
				foreach (var type in assembly.GetTypes())
				{
					var key = type.FullName.Replace(assemblyPrefix, "").ToLower();
					if (!types.ContainsKey(key))
					{
						types[key] = type;
					}
				}
			}

			return types;
		}
		#endregion
	}
}
