using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Addresses.Common.Models
{
	/// <summary>
	/// Extension methods for IAddressBasic models.
	/// </summary>
	public static class IAddressBasicExtensions
	{
		/// <summary>
		/// Determines if an <see cref="IAddressBasic"/> is empty.
		/// </summary>
		public static bool IsEmpty(this IAddressBasic address)
		{
			Contract.Requires<ArgumentNullException>(address != null);

			return String.IsNullOrEmpty(address.Address1)
				&& String.IsNullOrEmpty(address.Address2)
				&& String.IsNullOrEmpty(address.Address3)
				&& String.IsNullOrEmpty(address.City)
				&& String.IsNullOrEmpty(address.County)
				&& String.IsNullOrEmpty(address.State)
				&& String.IsNullOrEmpty(address.PostalCode)
				&& String.IsNullOrEmpty(address.Country);
		}
	}
}
