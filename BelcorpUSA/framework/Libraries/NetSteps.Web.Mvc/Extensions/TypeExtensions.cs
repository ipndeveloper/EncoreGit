using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using NetSteps.Common.Extensions;

namespace NetSteps.Web.Mvc.Extensions
{
    public static class TypeExtensions
    {
        /// <summary>
        /// Finds an action method, respecting ActionMethodSelectorAttributes (default MVC behavior).
        /// </summary>
        public static MethodInfo FindActionMethod(this Type type, ControllerContext controllerContext, string actionName)
        {
            var methodsWithValidAttributes = new List<MethodInfo>();
            var methodsWithNoAttributes = new List<MethodInfo>();

            foreach (var method in type.GetMethodsByNameCached(actionName))
            {
                var methodAttributes = method.GetCustomAttributes(typeof(ActionMethodSelectorAttribute), true);

                if (!methodAttributes.Any())
                {
                    methodsWithNoAttributes.Add(method);
                }
                else if (methodAttributes.All(x => ((ActionMethodSelectorAttribute)x).IsValidForRequest(controllerContext, method)))
                {
                    methodsWithValidAttributes.Add(method);
                }
            }

            if (methodsWithValidAttributes.Count > 1)
            {
                throw new InvalidOperationException(string.Format("Action '{0}' is ambiguous on type '{1}'.", actionName, type.Name));
            }

            if (methodsWithValidAttributes.Count == 1)
            {
                return methodsWithValidAttributes[0];
            }

            if (methodsWithNoAttributes.Count > 1)
            {
                throw new InvalidOperationException(string.Format("Action '{0}' is ambiguous on type '{1}'.", actionName, type.Name));
            }

            if (methodsWithNoAttributes.Count == 1)
            {
                return methodsWithNoAttributes[0];
            }

            throw new InvalidOperationException(string.Format("Action '{0}' not found on type '{1}'.", actionName, type.Name));
        }
    }
}
