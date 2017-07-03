using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Models.Core
{
	/// <summary>
	/// Contains global constants.
	/// </summary>
	internal static class Constants
	{
		/// <summary>
		/// Prime seed for hashcodes; I chose this for its bit distribution. ~P
		/// </summary>
		internal const int RandomPrime = 0xf3e9b;
	}
}
