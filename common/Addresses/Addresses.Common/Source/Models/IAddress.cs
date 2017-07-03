using NetSteps.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NetSteps.Addresses.Common.Models
{
	/// <summary>
	/// Common interface for Address.
	/// </summary>
	[ContractClass(typeof(Contracts.AddressContracts))]
	public interface IAddress : IAddressBasic
	{
	    #region Primitive properties
		/// <summary>
		/// The AddressID for this Address.
		/// </summary>
		int AddressID { get; set; }
	
		/// <summary>
		/// The AddressTypeID for this Address.
		/// </summary>
		short AddressTypeID { get; set; }
	
		/// <summary>
		/// The ProfileName for this Address.
		/// </summary>
		string ProfileName { get; set; }
	
		/// <summary>
		/// The FirstName for this Address.
		/// </summary>
		string FirstName { get; set; }
	
		/// <summary>
		/// The LastName for this Address.
		/// </summary>
		string LastName { get; set; }
	
		/// <summary>
		/// The Attention for this Address.
		/// </summary>
		string Attention { get; set; }
	
		/// <summary>
		/// The Name for this Address.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The StateProvinceID for this Address.
		/// </summary>
		Nullable<int> StateProvinceID { get; set; }
	
		/// <summary>
		/// The CountryID for this Address.
		/// </summary>
		int CountryID { get; set; }
	
		/// <summary>
		/// The PhoneNumber for this Address.
		/// </summary>
		string PhoneNumber { get; set; }
	
		/// <summary>
		/// The IsDefault for this Address.
		/// </summary>
		bool IsDefault { get; set; }
	
		/// <summary>
		/// The Latitude for this Address.
		/// </summary>
		Nullable<double> Latitude { get; set; }
	
		/// <summary>
		/// The Longitude for this Address.
		/// </summary>
		Nullable<double> Longitude { get; set; }
		
		/// <summary>
		/// The CountryCode for this Address.
		/// </summary>
		string CountryCode { get; }

        ///// <summary>
        ///// Se agrega propiedad para Street en tabla TaxCache
        ///// </summary>
        //string Street { get; set; }
		
		/// <summary>
		/// The StateProvinceAbbreviation for this Address.
		/// </summary>
		[Obsolete("This property should be removed from IAddress. Use an extension method instead.")]
		string StateProvinceAbbreviation { get; }
		
		/// <summary>
		/// The ProfileID for this Address.
		/// </summary>
		[Obsolete("This property should be removed from IAddress.")]
		int ProfileID { get; set; }
		
		/// <summary>
		/// The IsWillCall for this Address.
		/// </summary>
		[Obsolete("This property should be removed from IAddress.")]
		bool IsWillCall { get; set; }
		#endregion
	}

    /// <summary>
    /// Extension methods for IAddress
    /// </summary>
    public static class IAddressExtensions
    {
        /// <summary>
        /// Determines whether the specified address is identifiable as a PO Box.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns>
        ///   <c>true</c> if the address line is a PO Box; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsPoBox(this IAddress address)
        {
            return IsPoBox(address.Address1) || IsPoBox(address.Address2) || IsPoBox(address.Address3);
        }

        /// <summary>
        /// Determines whether the specified address line is identifiable as a PO Box.
        /// </summary>
        /// <param name="addressLine">The address line.</param>
        /// <returns>
        ///   <c>true</c> if the address line is a PO Box; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsPoBox(string addressLine)
        {
            if (string.IsNullOrWhiteSpace(addressLine))
                return false;
            return Regex.IsMatch(addressLine, RegularExpressions.PoBox);
        }
    }

	namespace Contracts
	{
		[ContractClassFor(typeof(IAddress))]
		internal abstract class AddressContracts : IAddress
		{
		    #region Primitive properties
		
			int IAddress.AddressID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			short IAddress.AddressTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAddress.ProfileName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAddress.FirstName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAddress.LastName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAddress.Attention
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAddress.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IAddress.StateProvinceID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IAddress.CountryID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAddress.PhoneNumber
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IAddress.IsDefault
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<double> IAddress.Latitude
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<double> IAddress.Longitude
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

			string IAddress.CountryCode
			{
				get { throw new NotImplementedException(); }
			}

			string IAddress.StateProvinceAbbreviation
			{
				get { throw new NotImplementedException(); }
			}

			int IAddress.ProfileID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

			bool IAddress.IsWillCall
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

			string IAddressBasic.Address1
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

			string IAddressBasic.Address2
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

			string IAddressBasic.Address3
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

			string IAddressBasic.City
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

			string IAddressBasic.County
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

			string IAddressBasic.State
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

			string IAddressBasic.PostalCode
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

			string IAddressBasic.Country
			{
				get { throw new NotImplementedException(); }
			}

            //string IAddressBasic.Street
            //{
            //    get { throw new NotImplementedException(); }
            //    set { throw new NotImplementedException(); }
            //}
            #endregion
           
        }
	}
}
