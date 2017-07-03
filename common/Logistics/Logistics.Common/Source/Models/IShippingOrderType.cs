using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Logistics.Common.Models
{
	/// <summary>
	/// Common interface for ShippingOrderType.
	/// </summary>
	[ContractClass(typeof(Contracts.ShippingOrderTypeContracts))]
	public interface IShippingOrderType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The ShippingOrderTypeID for this ShippingOrderType.
		/// </summary>
		int ShippingOrderTypeID { get; set; }
	
		/// <summary>
		/// The OrderTypeID for this ShippingOrderType.
		/// </summary>
		short OrderTypeID { get; set; }
	
		/// <summary>
		/// The ShippingMethodID for this ShippingOrderType.
		/// </summary>
		int ShippingMethodID { get; set; }
	
		/// <summary>
		/// The ShippingRegionID for this ShippingOrderType.
		/// </summary>
		Nullable<int> ShippingRegionID { get; set; }
	
		/// <summary>
		/// The ShippingRateGroupID for this ShippingOrderType.
		/// </summary>
		int ShippingRateGroupID { get; set; }
	
		/// <summary>
		/// The CountryID for this ShippingOrderType.
		/// </summary>
		int CountryID { get; set; }
	
		/// <summary>
		/// The OverrideAmount for this ShippingOrderType.
		/// </summary>
		Nullable<decimal> OverrideAmount { get; set; }
	
		/// <summary>
		/// The OverrideInclusive for this ShippingOrderType.
		/// </summary>
		Nullable<bool> OverrideInclusive { get; set; }
	
		/// <summary>
		/// The AllowDirectShipments for this ShippingOrderType.
		/// </summary>
		bool AllowDirectShipments { get; set; }
	
		/// <summary>
		/// The IsDefaultShippingMethod for this ShippingOrderType.
		/// </summary>
		bool IsDefaultShippingMethod { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IShippingOrderType))]
		internal abstract class ShippingOrderTypeContracts : IShippingOrderType
		{
		    #region Primitive properties
		
			int IShippingOrderType.ShippingOrderTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			short IShippingOrderType.OrderTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IShippingOrderType.ShippingMethodID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IShippingOrderType.ShippingRegionID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IShippingOrderType.ShippingRateGroupID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IShippingOrderType.CountryID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IShippingOrderType.OverrideAmount
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<bool> IShippingOrderType.OverrideInclusive
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IShippingOrderType.AllowDirectShipments
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IShippingOrderType.IsDefaultShippingMethod
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
