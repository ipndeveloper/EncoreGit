using System;

namespace NetSteps.Data.Entities.Extensions
{
	using System.Collections.Generic;
	using System.Linq;

	using NetSteps.Addresses.Common.Models;
	using NetSteps.Common.Extensions;
    using NetSteps.Data.Entities.Cache;

	/// <summary>
	/// Author: John Egbert
	/// Description: Address Extensions
	/// Created: 05-03-2010
	/// </summary>
	public static class AddressExtensions
	{
		public static Address GetByAddressID(this IEnumerable<Address> addresses, int addressID)
		{
            Address address = addresses.FirstOrDefault(a => a.AddressID == addressID);
            return address;
		}

		public static Address GetDefaultByTypeID(this IEnumerable<Address> addresses, Constants.AddressType addressType)
		{
			var retVal = addresses
				 .Where(x => x.AddressTypeID == (short)addressType)
				 .OrderByDescending(x => x.IsDefault)
				 .FirstOrDefault();

			return retVal;
		}

        public static IAddress GetDefaultByTypeID(this IEnumerable<IAddress> addresses, Constants.AddressType addressType)
        {
            var retVal = addresses
                 .Where(x => x.AddressTypeID == (short)addressType)
                 .OrderByDescending(x => x.IsDefault)
                 .FirstOrDefault();

            return retVal;
        }

		public static List<Address> GetAllByTypeID(this IEnumerable<Address> addresses, Constants.AddressType addressType)
		{
			List<Address> allByType = new List<Address>();
			foreach (Address address in addresses)
			{
				if (address.AddressTypeID == (short)addressType)
				{
					// Check State, if the value id the full state name then replace it with the abbreviation. - JHE
					if (address.State != null && address.State.Length > 2 && address.CountryID == 1)
					{
						var stateProvince = SmallCollectionCache.Instance.StateProvinces.FirstOrDefault(t1 => t1.Name == address.State);
						var state = stateProvince == default(StateProvince) ? string.Empty : stateProvince.StateAbbreviation;
						if (address.State != state && !state.IsNullOrEmpty())
						{
							address.State = state;
							address.Save();
						}
					}

					allByType.Add(address);
				}
			}
			return allByType;
		}

		/// <summary>
		/// Returns the "default" shipping address from a sequence of addresses.
		/// Includes "Shipping", then "Main" address types and gives preference to
		/// addresses marked as default.
		/// </summary>
		public static IAddress GetDefaultShippingAddress(this IEnumerable<IAddress> addresses)
		{
			if (addresses == null)
			{
				return null;
			}


			var shippingAddressTypeIds = new[]
			{
				(short)Constants.AddressType.Shipping,
				(short)Constants.AddressType.Main
			};

			return addresses
				.Where(a => shippingAddressTypeIds.Contains(a.AddressTypeID))
				.OrderByDescending(a => a.AddressTypeID == (short)Constants.AddressType.Shipping)
				.ThenByDescending(a => a.IsDefault)
				.FirstOrDefault();
		}

		public static Address UpdateAddress(this Address existingAddress, Address formAddress)
		{
			existingAddress.AttachAddressChangedCheck();
			Address.CopyPropertiesTo(formAddress, existingAddress);
			existingAddress.LookUpAndSetGeoCode();
			return existingAddress;
		}

        public static string GetEmailAddress(this Address address)
        {
            AddressProperty emailProperty = address.AddressProperties.Where(x => x.AddressPropertyTypeID == Convert.ToInt32(Constants.AddressPropertyType.EmailAddress)).SingleOrDefault();
            if (emailProperty == null)
                return null;
            else
                return emailProperty.PropertyValue;
        }

        public static void SetEmailAddress(this Address address, string Email)
        {
            AddressProperty emailProperty = address.AddressProperties.Where(x => x.AddressPropertyTypeID == Convert.ToInt32(Constants.AddressPropertyType.EmailAddress)).SingleOrDefault();
            if (!String.IsNullOrEmpty(Email))
            {
                if (emailProperty == null)
                {
                    emailProperty = new AddressProperty() { AddressPropertyTypeID = Convert.ToInt32(Constants.AddressPropertyType.EmailAddress), PropertyValue = Email };
                    address.AddressProperties.Add(emailProperty);
                }
                else
                    emailProperty.PropertyValue = Email;

            }
            else
            {
                if (emailProperty != null)
                {
                    emailProperty.MarkAsDeleted();
                    address.AddressProperties.Remove(emailProperty);
                }
            }
        }
	}
}
