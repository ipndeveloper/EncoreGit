using System;

namespace NetSteps.Models.Core.Metadata
{
	/// <summary>
	/// Stereotypes an interface as a Model.
	/// </summary>
	[AttributeUsage(AttributeTargets.Interface)]
	public class ModelAttribute : Attribute
	{
	}
}
