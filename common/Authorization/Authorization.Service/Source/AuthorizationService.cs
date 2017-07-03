using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Authorization.Common;
using System.Configuration;
using NetSteps.Authorization.Common.Configuration;

namespace NetSteps.Authorization.Service
{
	public class AuthorizationService : IAuthorizationService
	{
		public IEnumerable<string> FilterAuthorizationFunctions(IEnumerable<string> originalFunctionSet, Predicate<string> additionalFilter)
		{
			// null check
			var section = ConfigurationManager.GetSection("netStepsAuthorization");
			if (section == null)
			{
				return originalFunctionSet;
			}

			var config =  section as AuthorizationConfiguration;
			var blockedFunctions = new List<string>();
			foreach (FunctionElement element in config.Functions)
			{
				blockedFunctions.Add(element.Name);
			}
			
			var result = originalFunctionSet.Where(function => additionalFilter(function));
			result = result.Except(blockedFunctions);
			return result;
		}
	}
}
