
namespace NetSteps.Common.Extensions
{
	/// <summary>
	/// Author: John Egbert
	/// Description: Char Extensions
	/// Created: 11-01-2008
	/// </summary>
	public static class CharExtensions
	{
		#region Conversion Methods
		public static bool IsNullOrEmpty(this char value)
		{
			return value == '\0' || value == ' ';
		}
		#endregion
	}
}
