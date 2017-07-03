using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Addresses.Common.Models;

namespace NetSteps.Addresses.Common
{
	/// <summary>
	/// Common interface for looking up geocodes.
	/// </summary>
	public interface IGeoCodeProvider
	{
		/// <summary>
		/// Gets the geocode for an address.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <returns>The geocode.</returns>
		IGeoCodeData GetGeoCode(IAddressBasic address);
	}
}
