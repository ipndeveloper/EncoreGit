using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Orders.Common.Models
{
	/// <summary>
	/// Common interface for OrderShipmentPackage.
	/// </summary>
	[ContractClass(typeof(Contracts.OrderShipmentPackageContracts))]
	public interface IOrderShipmentPackage
	{
	    #region Primitive properties
	
		/// <summary>
		/// The OrderShipmentPackageID for this OrderShipmentPackage.
		/// </summary>
		int OrderShipmentPackageID { get; set; }
	
		/// <summary>
		/// The OrderShipmentID for this OrderShipmentPackage.
		/// </summary>
		int OrderShipmentID { get; set; }
	
		/// <summary>
		/// The ShippingMethodID for this OrderShipmentPackage.
		/// </summary>
		Nullable<int> ShippingMethodID { get; set; }
	
		/// <summary>
		/// The TrackingNumber for this OrderShipmentPackage.
		/// </summary>
		string TrackingNumber { get; set; }
	
		/// <summary>
		/// The DateShippedUTC for this OrderShipmentPackage.
		/// </summary>
		System.DateTime DateShippedUTC { get; set; }
	
		/// <summary>
		/// The TrackingUrl for this OrderShipmentPackage.
		/// </summary>
		string TrackingUrl { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The OrderShipment for this OrderShipmentPackage.
		/// </summary>
	    IOrderShipment OrderShipment { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The OrderShipmentPackageItems for this OrderShipmentPackage.
		/// </summary>
		IEnumerable<IOrderShipmentPackageItem> OrderShipmentPackageItems { get; }
	
		/// <summary>
		/// Adds an <see cref="IOrderShipmentPackageItem"/> to the OrderShipmentPackageItems collection.
		/// </summary>
		/// <param name="item">The <see cref="IOrderShipmentPackageItem"/> to add.</param>
		void AddOrderShipmentPackageItem(IOrderShipmentPackageItem item);
	
		/// <summary>
		/// Removes an <see cref="IOrderShipmentPackageItem"/> from the OrderShipmentPackageItems collection.
		/// </summary>
		/// <param name="item">The <see cref="IOrderShipmentPackageItem"/> to remove.</param>
		void RemoveOrderShipmentPackageItem(IOrderShipmentPackageItem item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IOrderShipmentPackage))]
		internal abstract class OrderShipmentPackageContracts : IOrderShipmentPackage
		{
		    #region Primitive properties
		
			int IOrderShipmentPackage.OrderShipmentPackageID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IOrderShipmentPackage.OrderShipmentID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IOrderShipmentPackage.ShippingMethodID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderShipmentPackage.TrackingNumber
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			System.DateTime IOrderShipmentPackage.DateShippedUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderShipmentPackage.TrackingUrl
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IOrderShipment IOrderShipmentPackage.OrderShipment
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IOrderShipmentPackageItem> IOrderShipmentPackage.OrderShipmentPackageItems
			{
				get { throw new NotImplementedException(); }
			}
		
			void IOrderShipmentPackage.AddOrderShipmentPackageItem(IOrderShipmentPackageItem item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IOrderShipmentPackage.RemoveOrderShipmentPackageItem(IOrderShipmentPackageItem item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
