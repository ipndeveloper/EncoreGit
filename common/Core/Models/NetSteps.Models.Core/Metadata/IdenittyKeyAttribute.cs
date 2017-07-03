using System;

namespace NetSteps.Models.Core.Metadata
{
	/// <summary>
	/// Identifies a single property as an IdentityKey, for generated types,
	/// will cause an implementation of IIdentifiable&lt;IK> where IK is the
	/// property's type.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public class IdentityKeyAttribute : Attribute
	{
	}
}
