using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Addresses.Common.Models;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Cache;

namespace NetSteps.Data.Entities.Extensions
{
	/// <summary>
	/// Author: Daniel Stafford
	/// Description: IAddress Extensions
	/// Created: 03-03-2010
	/// </summary>
	public static class IAddressExtensions
	{
		public enum AddressDisplayTypes
		{
			Web,
			QAS,
			Windows,
			SingleLine
		}

		public static string ToDisplay(this IAddress address)
		{
			return address.ToDisplay(AddressDisplayTypes.Web, false);
		}

		public static string ToDisplay(this IAddress address, string tagText)
		{
			return address.ToDisplay(AddressDisplayTypes.Web, false, tagText: tagText);
		}

		public static string ToDisplay(this IAddress address, AddressDisplayTypes type)
		{
			return address.ToDisplay(type, false);
		}

		public static string ToDisplay(this IAddress address, bool showPhone, bool showName = false, bool showProfileName = false, bool showShipToEmail = false)
		{
			return address.ToDisplay(AddressDisplayTypes.Web, showPhone: showPhone, showName: showName, showProfileName: showProfileName, showShipToEmail: showShipToEmail);
		}

        public static string ToDisplay(this IAddress address, AddressDisplayTypes type, bool showPhone, bool showName = false, bool showProfileName = false, bool showCountry = false, bool showShipToEmail = false, string tagText = "")
		{
			return Address.ToDisplay(address, type, showPhone: showPhone, showName: showName, showProfileName: showProfileName, showCountry: showCountry, showShipToEmail: showShipToEmail, tagText: tagText);
		}

		public static void Trim(this IAddress address)
		{
			if (address != null)
			{
				if (address.Address1 != null)
					address.Address1 = address.Address1.Trim();
				if (address.Address2 != null)
					address.Address2 = address.Address2.Trim();
				if (address.Address3 != null)
					address.Address3 = address.Address3.Trim();
				if (address.Attention != null)
					address.Attention = address.Attention.Trim();
				if (address.City != null)
					address.City = address.City.Trim();
				if (address.County != null)
					address.County = address.County.Trim();
				if (address.FirstName != null)
					address.FirstName = address.FirstName.Trim();
				if (address.LastName != null)
					address.LastName = address.LastName.Trim();
				if (address.Name != null)
					address.Name = address.Name.Trim();
				if (address.PhoneNumber != null)
					address.PhoneNumber = address.PhoneNumber.Trim();
				if (address.PostalCode != null)
					address.PostalCode = address.PostalCode.Trim();
				if (address.State != null)
					address.State = address.State.Trim();
			}
		}

		public static bool IsEmpty(this IAddress address, bool ignoreCountry = false)
		{
			if (address != null)
			{
				address.Trim();
				if (!string.IsNullOrEmpty(address.Attention) ||
					!string.IsNullOrEmpty(address.Name) ||
					!string.IsNullOrEmpty(address.FirstName) ||
					!string.IsNullOrEmpty(address.LastName) ||
					!string.IsNullOrEmpty(address.Address1) ||
					!string.IsNullOrEmpty(address.Address2) ||
					!string.IsNullOrEmpty(address.Address3) ||
					!string.IsNullOrEmpty(address.County) ||
					!string.IsNullOrEmpty(address.City) ||
					!string.IsNullOrEmpty(address.State) ||
					(address.StateProvinceID.HasValue && address.StateProvinceID.Value > 0) ||
					(!string.IsNullOrEmpty(address.Country) && !ignoreCountry) ||
					(address.CountryID > 0 && !ignoreCountry) ||
					!string.IsNullOrEmpty(address.PhoneNumber))
					return false;
				else
					return true;
			}
			return true;
		}
		/// <summary>
		/// Will check if the address has at least a countryId and a zipcode so we can attempt to figure out the taxes.
		/// Later this method should also contain knowledge of which countries has a flat tax fee.
		/// </summary>
		/// <param name="address"></param>
		/// <returns></returns>
		public static bool IsTaxInfoAvailable(this IAddress address)
		{
			return (address != null && address.CountryID > 0 && !address.PostalCode.IsNullOrEmpty());
		}

		public static void SetState(this IAddress address, string state, int countryId)
		{
			if (String.IsNullOrWhiteSpace(state))
			{
				return;
			}

			state = state.Trim();
			bool isInt = state.IsValidInt() && state.ToInt() > 0;
			var stateProvince = SmallCollectionCache.Instance.StateProvinces.FirstOrDefault(sp => sp.CountryID == countryId && sp.StateAbbreviation.Equals(state, StringComparison.InvariantCultureIgnoreCase) || sp.Name.Equals(state, StringComparison.InvariantCultureIgnoreCase));

			address.StateProvinceID = isInt ? state.ToInt() : stateProvince == default(StateProvince) ? (int?)null : stateProvince.StateProvinceID;
			address.State = isInt ? SmallCollectionCache.Instance.StateProvinces.GetById(state.ToInt()).StateAbbreviation : state.ToCleanString();
		}

		public static IEnumerable<IAddress> Distinct(this IEnumerable<IAddress> addresses)
		{
			return addresses.Distinct((addy1, addy2) => addy1.IsEqualTo(addy2));
		}

		public static void AddRangeDistinct(this IList<IAddress> distinctAddresses, IEnumerable<IAddress> additionalAddresses)
		{
			distinctAddresses.AddRange(additionalAddresses.Where(address => !distinctAddresses.Any(addy => addy.IsEqualTo(address))));
		}

		public static bool IsEqualTo(this IAddress addy1, IAddress addy2, bool includeFirstAndLastNames = true)
		{
			if (addy1 == null && addy2 == null) return true;
			if (addy1 == null || addy2 == null) return false;

			addy1.Trim();
			addy2.Trim();
			return AreEqual(addy1.Attention, addy2.Attention) &&
					(!includeFirstAndLastNames || AreEqual(addy1.FirstName, addy2.FirstName)) &&
					(!includeFirstAndLastNames || AreEqual(addy1.LastName, addy2.LastName)) &&
					AreEqual(addy1.Name, addy2.Name) &&
					AreEqual(addy1.Address1, addy2.Address1) &&
					AreEqual(addy1.Address2, addy2.Address2) &&
					AreEqual(addy1.Address3, addy2.Address3) &&
					AreEqual(addy1.City, addy2.City) &&
					AreEqual(addy1.County, addy2.County) &&
					(addy1.StateProvinceID == addy2.StateProvinceID || AreEqual(addy1.State, addy2.State)) &&
					addy1.CountryID == addy2.CountryID;
		}
		public static bool AreEqual(string a, string b)
		{
			if (string.IsNullOrEmpty(a))
			{
				return string.IsNullOrEmpty(b);
			}
			else
			{
				return string.Equals(a, b);
			}
		}
	}
}
