using System;
using System.Reflection;
using System.Diagnostics.Contracts;


namespace NetSteps.Encore.Core.Reflection
{
	/// <summary>
	/// Various PropertyInfo extensions.
	/// </summary>
	public static class PropertyInfoExtensions
	{
		/// <summary>
		/// Produces a backing field name for the given member
		/// </summary>
		/// <param name="member">the member</param>
		/// <returns>returns a backing field name</returns>
		public static string FormatBackingFieldName(this MemberInfo member)
		{
			Contract.Requires<ArgumentNullException>(member != null);
			Contract.Ensures(Contract.Result<string>() != null);

			// The intent is to produce a backing field name that won't clash with
			// other backing field names regardless of the depth/composition of 
			// class inheritance for the target type. Prefixing the declaring type
			// should do the trick.
			return String.Concat(member.DeclaringType.Name, "_", member.Name);
		}
	}
}
