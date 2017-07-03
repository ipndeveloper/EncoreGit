using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Orders.Common.Models
{
	/// <summary>
	/// Common interface for OrderShipmentsLog.
	/// </summary>
	[ContractClass(typeof(Contracts.OrderShipmentsLogContracts))]
	public interface IOrderShipmentsLog
	{
	    #region Primitive properties
	
		/// <summary>
		/// The OrderShipmentLogID for this OrderShipmentsLog.
		/// </summary>
		int OrderShipmentLogID { get; set; }
	
		/// <summary>
		/// The LogDateUTC for this OrderShipmentsLog.
		/// </summary>
		System.DateTime LogDateUTC { get; set; }
	
		/// <summary>
		/// The Succeeded for this OrderShipmentsLog.
		/// </summary>
		Nullable<bool> Succeeded { get; set; }
	
		/// <summary>
		/// The OrderID for this OrderShipmentsLog.
		/// </summary>
		int OrderID { get; set; }
	
		/// <summary>
		/// The ShipNo for this OrderShipmentsLog.
		/// </summary>
		int ShipNo { get; set; }
	
		/// <summary>
		/// The Sku for this OrderShipmentsLog.
		/// </summary>
		string Sku { get; set; }
	
		/// <summary>
		/// The Status for this OrderShipmentsLog.
		/// </summary>
		string Status { get; set; }
	
		/// <summary>
		/// The ShippingMethod for this OrderShipmentsLog.
		/// </summary>
		string ShippingMethod { get; set; }
	
		/// <summary>
		/// The TrackingNumber for this OrderShipmentsLog.
		/// </summary>
		string TrackingNumber { get; set; }
	
		/// <summary>
		/// The Quantity for this OrderShipmentsLog.
		/// </summary>
		Nullable<int> Quantity { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The Order for this OrderShipmentsLog.
		/// </summary>
	    IOrder Order { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IOrderShipmentsLog))]
		internal abstract class OrderShipmentsLogContracts : IOrderShipmentsLog
		{
		    #region Primitive properties
		
			int IOrderShipmentsLog.OrderShipmentLogID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			System.DateTime IOrderShipmentsLog.LogDateUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<bool> IOrderShipmentsLog.Succeeded
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IOrderShipmentsLog.OrderID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IOrderShipmentsLog.ShipNo
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderShipmentsLog.Sku
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderShipmentsLog.Status
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderShipmentsLog.ShippingMethod
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderShipmentsLog.TrackingNumber
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IOrderShipmentsLog.Quantity
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IOrder IOrderShipmentsLog.Order
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
