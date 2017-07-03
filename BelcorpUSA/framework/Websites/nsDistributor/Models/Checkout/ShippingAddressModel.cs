using System;
using System.Diagnostics.Contracts;
using NetSteps.Addresses.Common.Models;
using NetSteps.Data.Entities.Interfaces;

namespace nsDistributor.Models.Checkout
{
	public class ShippingAddressModel : IShippingAddress
	{
		#region Properties
		public int AddressID { get; set; }
		public string Address1 { get; set; }
		public string Address2 { get; set; }
		public string Address3 { get; set; }
		public string City { get; set; }
		public string Country { get; set; }
		public string PostalCode { get; set; }
		public string State { get; set; }
		public short AddressTypeID { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Attention { get; set; }
		public string Name { get; set; }
		public string County { get; set; }
		public int? StateProvinceID { get; set; }
		public int CountryID { get; set; }
		public string CountryCode { get; set; }
		public string StateProvinceAbbreviation { get; set; }
		public string PhoneNumber { get; set; }
		public bool IsDefault { get; set; }
		public int ProfileID { get; set; }
		public string ProfileName { get; set; }
		public bool IsWillCall { get; set; }
		public double? Latitude { get; set; }
		public double? Longitude { get; set; }
        public string ShipToEmailAddress { get; set; }
		#endregion

		#region Constructors
		public ShippingAddressModel() : this(null) { }
		public ShippingAddressModel(IAddress loadFrom)
		{
			if (loadFrom != default(IAddress))
				LoadFrom(loadFrom);
		}
		#endregion

		#region Infrastructure
		public void LoadFrom(IAddress address)
		{
			Contract.Requires<ArgumentException>(address != default(IAddress));

			this.AddressID = address.AddressID;
			this.Address1 = address.Address1;
			this.Address2 = address.Address2;
			this.Address3 = address.Address3;
			this.City = address.City;
			this.Country = address.Country;
			this.PostalCode = address.PostalCode;
			this.State = address.State;
			this.AddressTypeID = address.AddressTypeID;
			this.FirstName = address.FirstName;
			this.LastName = address.LastName;
			this.Attention = address.Attention;
			this.Name = address.Name;
			this.Country = address.Country;
			this.StateProvinceID = address.StateProvinceID;
			this.CountryID = address.CountryID;
			this.StateProvinceAbbreviation = address.StateProvinceAbbreviation;
			this.PhoneNumber = address.PhoneNumber;
			this.IsDefault = address.IsDefault;
			this.ProfileID = address.ProfileID;
			this.ProfileName = address.ProfileName;
			this.IsWillCall = address.IsWillCall;
			this.Latitude = address.Latitude;
			this.Longitude = address.Longitude;
		}
		#endregion
	}
}