using System;
using System.Reflection;

namespace NetSteps.Common.Extensions
{
	public static class MethodInfoExtensions
	{
		public static MethodInfo MakeGenericMethodCached(this MethodInfo method, params Type[] typeArguments)
		{
			return Reflection.Reflection.MakeGenericMethod(method, typeArguments);
		}

		public static ParameterInfo[] GetParametersCached(this MethodInfo method)
		{
			return Reflection.Reflection.FindMethodParameters(method);
		}
	}
}
