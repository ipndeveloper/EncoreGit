using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Logistics.Common.Models
{
	/// <summary>
	/// Common interface for ShippingRate.
	/// </summary>
	[ContractClass(typeof(Contracts.ShippingRateContracts))]
	public interface IShippingRate
	{
	    #region Primitive properties
	
		/// <summary>
		/// The ShippingRateID for this ShippingRate.
		/// </summary>
		int ShippingRateID { get; set; }
	
		/// <summary>
		/// The ShippingRateGroupID for this ShippingRate.
		/// </summary>
		int ShippingRateGroupID { get; set; }
	
		/// <summary>
		/// The ValueName for this ShippingRate.
		/// </summary>
		string ValueName { get; set; }
	
		/// <summary>
		/// The ValueFrom for this ShippingRate.
		/// </summary>
		Nullable<decimal> ValueFrom { get; set; }
	
		/// <summary>
		/// The ValueTo for this ShippingRate.
		/// </summary>
		Nullable<decimal> ValueTo { get; set; }
	
		/// <summary>
		/// The ShippingAmount for this ShippingRate.
		/// </summary>
		Nullable<decimal> ShippingAmount { get; set; }
	
		/// <summary>
		/// The DirectShipmentFee for this ShippingRate.
		/// </summary>
		Nullable<decimal> DirectShipmentFee { get; set; }
	
		/// <summary>
		/// The HandlingFee for this ShippingRate.
		/// </summary>
		Nullable<decimal> HandlingFee { get; set; }
	
		/// <summary>
		/// The IncrementalAmount for this ShippingRate.
		/// </summary>
		Nullable<decimal> IncrementalAmount { get; set; }
	
		/// <summary>
		/// The IncrementalFee for this ShippingRate.
		/// </summary>
		Nullable<decimal> IncrementalFee { get; set; }
	
		/// <summary>
		/// The CurrencyID for this ShippingRate.
		/// </summary>
		int CurrencyID { get; set; }
	
		/// <summary>
		/// The ShippingPercentage for this ShippingRate.
		/// </summary>
		Nullable<decimal> ShippingPercentage { get; set; }
	
		/// <summary>
		/// The ShippingRateTypeID for this ShippingRate.
		/// </summary>
		Nullable<short> ShippingRateTypeID { get; set; }
	
		/// <summary>
		/// The MinimumAmount for this ShippingRate.
		/// </summary>
		Nullable<decimal> MinimumAmount { get; set; }
	
		/// <summary>
		/// The DirectShipmentPercentage for this ShippingRate.
		/// </summary>
		Nullable<decimal> DirectShipmentPercentage { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The ShippingRateGroup for this ShippingRate.
		/// </summary>
	    IShippingRateGroup ShippingRateGroup { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IShippingRate))]
		internal abstract class ShippingRateContracts : IShippingRate
		{
		    #region Primitive properties
		
			int IShippingRate.ShippingRateID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IShippingRate.ShippingRateGroupID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IShippingRate.ValueName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IShippingRate.ValueFrom
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IShippingRate.ValueTo
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IShippingRate.ShippingAmount
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IShippingRate.DirectShipmentFee
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IShippingRate.HandlingFee
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IShippingRate.IncrementalAmount
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IShippingRate.IncrementalFee
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IShippingRate.CurrencyID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IShippingRate.ShippingPercentage
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<short> IShippingRate.ShippingRateTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IShippingRate.MinimumAmount
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IShippingRate.DirectShipmentPercentage
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IShippingRateGroup IShippingRate.ShippingRateGroup
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
