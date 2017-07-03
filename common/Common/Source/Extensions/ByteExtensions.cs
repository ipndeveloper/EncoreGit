using System;

namespace NetSteps.Common.Extensions
{
	/// <summary>
	/// Author: John Egbert
	/// Description: Byte Extensions
	/// Created: 10-19-2008
	/// </summary>
	public static class ByteExtensions
	{
		#region Conversion Methods
		public static int ToInt(this Byte value)
		{
			return value.ToString().ToInt();
		}
		#endregion

		public static bool AreEqual(this Byte[] value, Byte[] value2)
		{
			if (value == null && value2 == null)
				return true;
			else if (value != null || value2 != null)
				return false;
			else
				return Convert.ToBase64String(value) == Convert.ToBase64String(value2);
		}
	}
}
