using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Orders.Common.Models
{
	/// <summary>
	/// Common interface for OrderShipmentPackageItem.
	/// </summary>
	[ContractClass(typeof(Contracts.OrderShipmentPackageItemContracts))]
	public interface IOrderShipmentPackageItem
	{
	    #region Primitive properties
	
		/// <summary>
		/// The OrderShipmentPackageItemID for this OrderShipmentPackageItem.
		/// </summary>
		int OrderShipmentPackageItemID { get; set; }
	
		/// <summary>
		/// The OrderShipmentPackageID for this OrderShipmentPackageItem.
		/// </summary>
		int OrderShipmentPackageID { get; set; }
	
		/// <summary>
		/// The OrderItemID for this OrderShipmentPackageItem.
		/// </summary>
		int OrderItemID { get; set; }
	
		/// <summary>
		/// The Quantity for this OrderShipmentPackageItem.
		/// </summary>
		int Quantity { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The OrderItem for this OrderShipmentPackageItem.
		/// </summary>
	    IOrderItem OrderItem { get; set; }
	
		/// <summary>
		/// The OrderShipmentPackage for this OrderShipmentPackageItem.
		/// </summary>
	    IOrderShipmentPackage OrderShipmentPackage { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IOrderShipmentPackageItem))]
		internal abstract class OrderShipmentPackageItemContracts : IOrderShipmentPackageItem
		{
		    #region Primitive properties
		
			int IOrderShipmentPackageItem.OrderShipmentPackageItemID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IOrderShipmentPackageItem.OrderShipmentPackageID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IOrderShipmentPackageItem.OrderItemID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IOrderShipmentPackageItem.Quantity
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IOrderItem IOrderShipmentPackageItem.OrderItem
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IOrderShipmentPackage IOrderShipmentPackageItem.OrderShipmentPackage
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
