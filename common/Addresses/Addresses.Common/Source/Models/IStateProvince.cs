using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Addresses.Common.Models
{
	/// <summary>
	/// Common interface for StateProvince.
	/// </summary>
	[ContractClass(typeof(Contracts.StateProvinceContracts))]
	public interface IStateProvince
	{
	    #region Primitive properties
	
		/// <summary>
		/// The StateProvinceID for this StateProvince.
		/// </summary>
		int StateProvinceID { get; set; }
	
		/// <summary>
		/// The Name for this StateProvince.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The CountryID for this StateProvince.
		/// </summary>
		int CountryID { get; set; }
	
		/// <summary>
		/// The ChargeTaxOnShipping for this StateProvince.
		/// </summary>
		Nullable<bool> ChargeTaxOnShipping { get; set; }
	
		/// <summary>
		/// The IsContinental for this StateProvince.
		/// </summary>
		Nullable<bool> IsContinental { get; set; }
	
		/// <summary>
		/// The ShippingRegionID for this StateProvince.
		/// </summary>
		Nullable<int> ShippingRegionID { get; set; }
	
		/// <summary>
		/// The CountryRegionID for this StateProvince.
		/// </summary>
		Nullable<int> CountryRegionID { get; set; }
	
		/// <summary>
		/// The StateAbbreviation for this StateProvince.
		/// </summary>
		string StateAbbreviation { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IStateProvince))]
		internal abstract class StateProvinceContracts : IStateProvince
		{
		    #region Primitive properties
		
			int IStateProvince.StateProvinceID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IStateProvince.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IStateProvince.CountryID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<bool> IStateProvince.ChargeTaxOnShipping
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<bool> IStateProvince.IsContinental
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IStateProvince.ShippingRegionID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IStateProvince.CountryRegionID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IStateProvince.StateAbbreviation
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
