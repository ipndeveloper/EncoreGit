using System;

namespace NetSteps.Common.Extensions
{
	/// <summary>
	/// Author: John Egbert
	/// Description: Guid Extensions
	/// Created: 10-29-2009
	/// </summary>
	public static class GuidExtensions
	{
		#region Validation Methods
		public static bool IsNullOrEmpty(this Guid value)
		{
			Guid emptyGuid = new Guid();
			return value == null || emptyGuid.ToString() == value.ToString();
		}
		#endregion
	}
}
