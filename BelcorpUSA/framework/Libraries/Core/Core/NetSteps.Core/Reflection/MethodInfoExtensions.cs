using System;
using System.Linq;
using System.Reflection;
using System.Diagnostics.Contracts;

namespace NetSteps.Encore.Core.Reflection
{
	/// <summary>
	/// Contains extension methods for the MethodInfo and MethodBase types.
	/// </summary>
	public static class MethodInfoExtensions
	{
		/// <summary>
		/// Gets the parameter types for a method.
		/// </summary>
		/// <param name="method"></param>
		/// <returns></returns>
		public static Type[] GetParameterTypes(this MethodBase method)
		{
			Contract.Requires<ArgumentNullException>(method != null);
			return (from p in method.GetParameters()
							select p.ParameterType).ToArray();
		}
	}
}
