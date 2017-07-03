using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Authorization.Common
{
	public interface IAuthorizationService
	{
		IEnumerable<string> FilterAuthorizationFunctions(IEnumerable<string> originalFunctionSet, Predicate<string> additionalFilter);
 	}

	public static class AuthorizationServiceExtensions
	{
		public static IEnumerable<string> FilterAuthorizationFunctions(this IAuthorizationService service, IEnumerable<string> originalFunctionSet)
		{
			return service.FilterAuthorizationFunctions(originalFunctionSet, (definedFunction) => { return true; });
		}
	}
}
