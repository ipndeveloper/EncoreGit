using System;

namespace NetSteps.Models.Core.Metadata
{
	/// <summary>
	/// Declares the default value for a property.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public class DefaultValueAttribute : Attribute
	{
		/// <summary>
		/// Creates a new instance establishing a default value for a property.
		/// </summary>
		/// <param name="value">the value</param>
		public DefaultValueAttribute(object value)
		{
			Value = value;
		}
		/// <summary>
		/// Gets the default value.
		/// </summary>
		public object Value { get; private set; }
	}
}
