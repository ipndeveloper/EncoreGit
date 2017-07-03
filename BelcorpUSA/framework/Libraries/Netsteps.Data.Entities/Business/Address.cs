using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using NetSteps.Addresses.Common;
using NetSteps.Addresses.Common.Models;
using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Common.Validation.NetTiers;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Encore.Core.IoC;
using NetSteps.Data.Entities.Repositories;
using System.Text.RegularExpressions;

namespace NetSteps.Data.Entities 
{
	public partial class Address : IAddress, IGeoCode
	{
		/// <summary>
		/// Related entities that can be included by the 'Load' methods (see <see cref="LoadRelationsExtensions"/>).
		/// WARNING: Changes to this list will affect various 'Load' methods, be careful.
		/// </summary>
		[Flags]
		public enum Relations
		{
			// These are bit flags so they must be numbered appropriately (eg. 0, 1, 2, 4, 8, 16)
			// Use bit-shifting for convenience (eg. 0, 1 << 0, 1 << 1, 1 << 2)
			None = 0,
			AddressType = 1 << 0,
			AddressProperties = 1 << 1,

			/// <summary>
			/// The default relations used by the 'LoadFull' methods.
			/// </summary>
			LoadFull = AddressType | AddressProperties
		};

		#region Properties
		// Whether the address pertains to a Will Call shipping method`
		public bool IsWillCall { get; set; }

		public int ProfileID { get; set; }

		public string CountryName
		{
			get
			{
				if ((this as IAddress).CountryID > 0)
				{
					var country = SmallCollectionCache.Instance.Countries.GetById((this as IAddress).CountryID);
					return country.GetTerm();
				}
                
                    return string.Empty;
			}
		}

		public string CountryCode
		{
			get
			{
				if ((this as IAddress).CountryID > 0)
				{
					var country = SmallCollectionCache.Instance.Countries.GetById((this as IAddress).CountryID);
					return country.CountryCode;
				}

				return string.Empty;
			}
		}

		public string StateProvinceAbbreviation
		{
			get
			{
				var thisAsIAddress = this as IAddress;
				if (thisAsIAddress.StateProvinceID.HasValue && thisAsIAddress.StateProvinceID > 0)
				{
					var stateProvince = SmallCollectionCache.Instance.StateProvinces.GetById(thisAsIAddress.StateProvinceID.Value);
					return stateProvince.StateAbbreviation;
				}

				return string.Empty;
			}
		}
        //public NetSteps.Data.Entities.Constants.AddressType AddressType { get; set; }

        public List<string> StatesList
		{
			get
			{
				List<string> list = new List<string>(NetSteps.Common.Constants.States.Keys.Cast<string>());
				list.Sort();
				return list;
			}
		}

		// Can be used with business logic to determine if the location properties (in Address__propertyChanged) changed - JHE
		public bool IsLocationDirty { get; set; }

		#endregion

		#region Methods

		/// <summary>
		/// Called before setting Address properties to detect if the Address changed and mark the current GeoCode outdated. - JHE
		/// </summary>
		public void AttachAddressChangedCheck()
		{
			this._propertyChanged -= new System.ComponentModel.PropertyChangedEventHandler(Address__propertyChanged);
			this._propertyChanged += new System.ComponentModel.PropertyChangedEventHandler(Address__propertyChanged);
		}
		private void Address__propertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Address1" || e.PropertyName == "Address2" || e.PropertyName == "Address3" || e.PropertyName == "City" ||
				e.PropertyName == "State" || e.PropertyName == "StateProvinceID" || e.PropertyName == "County" || e.PropertyName == "PostalCode" ||
				e.PropertyName == "CountryID")
			{
				IsGeoCodeCurrent = false;
				IsLocationDirty = true;
			}
		}

		public void LookUpAndSetGeoCode()
		{
			if (!IsGeoCodeCurrent && BusinessLogic.IsGeoCodeLookupEnabled(this) && !this.IsEmpty(true))
			{
				var geoCode = Create.New<IGeoCodeProvider>().GetGeoCode(this);
				if (geoCode != null)
				{
					Latitude = (geoCode.Latitude == 0) ? (double?)null : geoCode.Latitude;
					Longitude = (geoCode.Longitude == 0) ? (double?)null : geoCode.Longitude;
					IsGeoCodeCurrent = true;
				}
			}
		}

		public override string ToString()
		{
            string delimiter = "\r\n";
            string AddressString = string.Empty;

            var Address = new AddressRepository().GetAddressByID(this.AddressID);

            if (Address != null)
            {
                if (!string.IsNullOrEmpty(Address.Street))
                    AddressString += Address.Street + delimiter;

                if (!string.IsNullOrEmpty(Address.Address1))
                {
                    AddressString += Address.Address1.Trim();

                    if (!string.IsNullOrEmpty(Address.Address2))
                    {
                        AddressString += ", " + Address.Address2.Trim();
                    }

                    if (!string.IsNullOrEmpty(Address.Address3))
                    {
                        AddressString += ", " + Address.Address3.Trim();
                    }

                    AddressString += delimiter;
                }

                if (!string.IsNullOrEmpty(Address.CountryStr))
                    AddressString += Address.CountryStr + delimiter;

                if (!string.IsNullOrEmpty(Address.City))
                    AddressString += Address.City + delimiter;

                if (!string.IsNullOrEmpty(Address.State))
                    AddressString += Address.State + " ";

                if (!string.IsNullOrEmpty(Address.PostalCode))
                    AddressString += Address.PostalCode + delimiter;
            }
            

            //string AddressString = String.Format("{0}{1}\r\n{2} {3} {4} {5}\r\n{6}",
            //    (!String.IsNullOrEmpty(this.Attention) ? this.Attention + "\r\n" : string.Empty),
            //        this.Address1 +
            //        (string.IsNullOrEmpty(this.Address2) ? string.Empty : ",\r\n" + this.Address2) +
            //        (string.IsNullOrEmpty(this.Address3) ? string.Empty : ",\r\n" + this.Address3),
            //        (string.IsNullOrEmpty(this.City) ? string.Empty : this.City + ","),
            //        (string.IsNullOrEmpty(this.County) ? string.Empty : this.County + ","),
            //        this.State,
            //        (this.CountryID == 1 ? this.PostalCode.ZipCode() : this.PostalCode),
            //        this.CountryName);

            //string delimiter = "\r\n";
            //string street = " " + new AddressRepository().GetAddressByID(this.AddressID) + " " + delimiter;

            //var regex = new Regex(Regex.Escape(delimiter));
            //AddressString = regex.Replace(AddressString, street, 1);

            return AddressString;
		}

		public string ToString(bool withAttention)
		{
			return String.Format("{0}{1}\r\n{2} {3} {4} {5}\r\n{6}",
				(withAttention) ? (!String.IsNullOrEmpty(this.Attention) ? this.Attention + "\r\n" : string.Empty) : string.Empty,
					this.Address1 +
					(string.IsNullOrEmpty(this.Address2) ? string.Empty : ",\r\n" + this.Address2) +
					(string.IsNullOrEmpty(this.Address3) ? string.Empty : ",\r\n" + this.Address3),
					(string.IsNullOrEmpty(this.City) ? string.Empty : this.City + ","),
					(string.IsNullOrEmpty(this.County) ? string.Empty : this.County + ","),
					this.State, this.PostalCode, this.CountryName);
		}

        public static string ToDisplay(IAddress address, NetSteps.Data.Entities.Extensions.IAddressExtensions.AddressDisplayTypes type, bool showPhone, bool showName = false, bool showProfileName = false, bool showCountry = true, bool showShipToEmail = false, string tagText = "")
		{
			try
			{
                return BusinessLogic.ToDisplay(address, type, showPhone, showName, showProfileName, showCountry, showShipToEmail, tagText);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static Address Load(string addressNumber)
		{
			try
			{
				return Repository.GetByNumber(addressNumber);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static IEnumerable<Address> GetByAddressTypePostalCodeAndCity(int addressTypeId, string postalCode, string city)
		{
			Contract.Requires<ArgumentException>(addressTypeId > 0);
			Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(postalCode));
			Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(city));

			try
			{
				return Repository.GetByAddressTypePostalCodeAndCity(addressTypeId, postalCode, city);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static List<Address> LoadByAccountId(int accountId)
		{
			try
			{
				return Repository.GetByAccountId(accountId);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static bool IsCountryIdOnAddressValid(object target, ValidationRuleArgs e)
		{
			return BusinessLogic.IsCountryIdOnAddressValid(target, e);
		}

		public static bool ValidateAddressAccuracy(object target, ValidationRuleArgs e)
		{
			return BusinessLogic.ValidateAddressAccuracy(target as Address, e).Success;
		}
		public BasicResponseItem<List<IAddress>> ValidateAddressAccuracy()
		{
			return BusinessLogic.ValidateAddressAccuracy(this, null);
		}

		public static void CopyPropertiesTo(IAddress sourceAddress, IAddress targetAddress)
		{
			BusinessLogic.CopyPropertiesTo(sourceAddress, targetAddress);
		}

		public static void CopyPropertiesTo(IAddress sourceAddress, IAddress targetAddress, bool copyAddressId)
		{
			BusinessLogic.CopyPropertiesTo(sourceAddress, targetAddress, copyAddressId);
		}

		public static IGeoCodeData LookUpGeoCodeFromExistingAddresses(IAddress address)
		{
			try
			{
				return Repository.LookUpGeoCodeFromExistingAddresses(address);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static IGeoCodeData LookUpGeoCodeFromExistingAddressesByCityState(IAddress address)
		{
			try
			{
				return Repository.LookUpGeoCodeFromExistingAddressesByCityState(address);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public Country GetCountryFromCache()
		{
			if (CountryID == 0)
			{
				return null;
			}

			return SmallCollectionCache.Instance.Countries.GetById(CountryID);
		}

		public static bool IsUsedByAnyActiveOrderTemplates(int addressID)
		{
			return Repository.IsUsedByAnyActiveOrderTemplates(addressID);
		}
		#endregion

		#region IAddressBasic
		string IAddressBasic.Country
		{
			get
			{
				return CountryName;
			}
		}
		#endregion
	}
}
