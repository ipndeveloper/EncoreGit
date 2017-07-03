using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Addresses.Common.Models;
using NetSteps.Common.Base;

namespace NetSteps.Addresses.Common
{
	/// <summary>
	/// Common interface for validating addresses.
	/// </summary>
	public interface IAddressValidationProvider
	{
		/// <summary>
		/// Validates an address.
		/// </summary>
		/// <param name="address">The address to validate.</param>
		/// <returns>A <see cref="BasicResponseItem{T}"/> indicating if validation was successful.</returns>
		BasicResponseItem<IAddressBasic> ValidateAddress(IAddressBasic address);
	}
}
