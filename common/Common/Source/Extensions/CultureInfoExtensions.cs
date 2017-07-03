using System.Globalization;
using NetSteps.Common.Globalization;

namespace NetSteps.Common.Extensions
{
	/// <summary>
	/// Author: John Egbert
	/// Description: CultureInfo Extensions
	/// Created: 10-13-2010
	/// </summary>
	public static class CultureInfoExtensions
	{
		public static bool IsUnitedStates(this CultureInfo value)
		{
			return value.Name == CountryCultureInfoCode.UnitedStates;
		}
		public static bool IsCanada(this CultureInfo value)
		{
			return value.Name == CountryCultureInfoCode.Canada;
		}
		public static bool IsJapan(this CultureInfo value)
		{
			return value.Name == CountryCultureInfoCode.Japan;
		}
	}
}
