using System;

namespace NetSteps.Models.Core.Metadata
{
	/// <summary>
	/// Signifies that a property should be ignored by Model 
	/// code generation and code-emitting tools.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public class ModelIgnoreAttribute : Attribute
	{
	}
}
