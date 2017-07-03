using System;

namespace NetSteps.Encore.Core.Dto
{
	/// <summary>
	/// Marks a property such that it will be ignored by DTO code generation.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public class DtoIgnoreAttribute : Attribute
	{
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		public DtoIgnoreAttribute()
			: base()
		{
		}
	}
}
