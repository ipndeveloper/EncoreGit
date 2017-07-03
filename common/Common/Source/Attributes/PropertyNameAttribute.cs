using System;

namespace NetSteps.Common.Attributes
{
	[AttributeUsage(AttributeTargets.Property)]
	public class PropertyNameAttribute : Attribute
	{
		public string PropertyName { get; set; }

		public PropertyNameAttribute(string propertyName)
		{
			PropertyName = propertyName;
		}
	}
}
