using System;
using System.Reflection;

namespace NetSteps.Encore.Core.Reflection.Emit
{

	internal interface IPropertyRef : IValueRef
	{
		PropertyInfo GetPropertyInfo();
	}

	
}