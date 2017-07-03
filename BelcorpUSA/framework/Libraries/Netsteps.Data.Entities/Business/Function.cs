using System;
using System.Linq;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities
{
	public partial class Function
	{
		#region Methods
		public static void CreateFunction(string function)
		{
			try
			{
				function = function.ToCleanString();

				if (!string.IsNullOrEmpty(function))
				{
					// Use cache first
					var functions = SmallCollectionCache.Instance.Functions.ToList();
					var existingFunction = functions.FirstOrDefault(f => f.Name.ToUpper() == function.ToUpper());

					// Check DB next before creating a new function - JHE
					if (existingFunction == null)
					{
						functions = Function.LoadAll();
						existingFunction = functions.FirstOrDefault(f => f.Name.ToUpper() == function.ToUpper());
					}

					if (existingFunction == null)
					{
						Function newFunction = new Function();
						newFunction.StartEntityTracking();
						newFunction.Name = function;
						newFunction.Active = true;
						newFunction.Save();

						// Add to the Admin roles - JHE
						var roles = Role.LoadAllFull();
						var adminRole = roles.FirstOrDefault(r => r.Name.Equals("Administrator", StringComparison.InvariantCultureIgnoreCase));
						if (adminRole != default(Role))
						{
							adminRole.Functions.Add(newFunction);
							adminRole.Save();
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
			}
		}
		#endregion
	}
}
