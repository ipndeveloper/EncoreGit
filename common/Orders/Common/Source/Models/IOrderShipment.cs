using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Orders.Common.Models
{
	/// <summary>
	/// Common interface for OrderShipment.
	/// </summary>
	[ContractClass(typeof(Contracts.OrderShipmentContracts))]
	public interface IOrderShipment
	{
	    #region Primitive properties
	
		/// <summary>
		/// The OrderShipmentID for this OrderShipment.
		/// </summary>
		int OrderShipmentID { get; set; }
	
		/// <summary>
		/// The OrderID for this OrderShipment.
		/// </summary>
		int OrderID { get; set; }
	
		/// <summary>
		/// The OrderCustomerID for this OrderShipment.
		/// </summary>
		Nullable<int> OrderCustomerID { get; set; }
	
		/// <summary>
		/// The ShippingMethodID for this OrderShipment.
		/// </summary>
		Nullable<int> ShippingMethodID { get; set; }
	
		/// <summary>
		/// The OrderShipmentStatusID for this OrderShipment.
		/// </summary>
		short OrderShipmentStatusID { get; set; }
	
		/// <summary>
		/// The FirstName for this OrderShipment.
		/// </summary>
		string FirstName { get; set; }
	
		/// <summary>
		/// The LastName for this OrderShipment.
		/// </summary>
		string LastName { get; set; }
	
		/// <summary>
		/// The Attention for this OrderShipment.
		/// </summary>
		string Attention { get; set; }
	
		/// <summary>
		/// The Name for this OrderShipment.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The Address1 for this OrderShipment.
		/// </summary>
		string Address1 { get; set; }
	
		/// <summary>
		/// The Address2 for this OrderShipment.
		/// </summary>
		string Address2 { get; set; }
	
		/// <summary>
		/// The Address3 for this OrderShipment.
		/// </summary>
		string Address3 { get; set; }
	
		/// <summary>
		/// The City for this OrderShipment.
		/// </summary>
		string City { get; set; }
	
		/// <summary>
		/// The County for this OrderShipment.
		/// </summary>
		string County { get; set; }
	
		/// <summary>
		/// The State for this OrderShipment.
		/// </summary>
		string State { get; set; }
	
		/// <summary>
		/// The StateProvinceID for this OrderShipment.
		/// </summary>
		Nullable<int> StateProvinceID { get; set; }
	
		/// <summary>
		/// The PostalCode for this OrderShipment.
		/// </summary>
		string PostalCode { get; set; }
	
		/// <summary>
		/// The CountryID for this OrderShipment.
		/// </summary>
		int CountryID { get; set; }
	
		/// <summary>
		/// The Email for this OrderShipment.
		/// </summary>
		string Email { get; set; }
	
		/// <summary>
		/// The DayPhone for this OrderShipment.
		/// </summary>
		string DayPhone { get; set; }
	
		/// <summary>
		/// The EveningPhone for this OrderShipment.
		/// </summary>
		string EveningPhone { get; set; }
	
		/// <summary>
		/// The TrackingNumber for this OrderShipment.
		/// </summary>
		string TrackingNumber { get; set; }
	
		/// <summary>
		/// The TrackingURL for this OrderShipment.
		/// </summary>
		string TrackingURL { get; set; }
	
		/// <summary>
		/// The DateShippedUTC for this OrderShipment.
		/// </summary>
		Nullable<System.DateTime> DateShippedUTC { get; set; }
	
		/// <summary>
		/// The GovernmentReceiptNumber for this OrderShipment.
		/// </summary>
		string GovernmentReceiptNumber { get; set; }
	
		/// <summary>
		/// The IsDirectShipment for this OrderShipment.
		/// </summary>
		bool IsDirectShipment { get; set; }
	
		/// <summary>
		/// The IsWillCall for this OrderShipment.
		/// </summary>
		bool IsWillCall { get; set; }
	
		/// <summary>
		/// The DataVersion for this OrderShipment.
		/// </summary>
		byte[] DataVersion { get; set; }
	
		/// <summary>
		/// The ModifiedByUserID for this OrderShipment.
		/// </summary>
		Nullable<int> ModifiedByUserID { get; set; }
	
		/// <summary>
		/// The SourceAddressID for this OrderShipment.
		/// </summary>
		Nullable<int> SourceAddressID { get; set; }
	
		/// <summary>
		/// The PickupPointCode for this OrderShipment.
		/// </summary>
		string PickupPointCode { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The Order for this OrderShipment.
		/// </summary>
	    IOrder Order { get; set; }
	
		/// <summary>
		/// The OrderCustomer for this OrderShipment.
		/// </summary>
	    IOrderCustomer OrderCustomer { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The OrderShipmentPackages for this OrderShipment.
		/// </summary>
		IEnumerable<IOrderShipmentPackage> OrderShipmentPackages { get; }
	
		/// <summary>
		/// Adds an <see cref="IOrderShipmentPackage"/> to the OrderShipmentPackages collection.
		/// </summary>
		/// <param name="item">The <see cref="IOrderShipmentPackage"/> to add.</param>
		void AddOrderShipmentPackage(IOrderShipmentPackage item);
	
		/// <summary>
		/// Removes an <see cref="IOrderShipmentPackage"/> from the OrderShipmentPackages collection.
		/// </summary>
		/// <param name="item">The <see cref="IOrderShipmentPackage"/> to remove.</param>
		void RemoveOrderShipmentPackage(IOrderShipmentPackage item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IOrderShipment))]
		internal abstract class OrderShipmentContracts : IOrderShipment
		{
		    #region Primitive properties
		
			int IOrderShipment.OrderShipmentID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IOrderShipment.OrderID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IOrderShipment.OrderCustomerID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IOrderShipment.ShippingMethodID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			short IOrderShipment.OrderShipmentStatusID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderShipment.FirstName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderShipment.LastName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderShipment.Attention
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderShipment.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderShipment.Address1
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderShipment.Address2
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderShipment.Address3
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderShipment.City
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderShipment.County
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderShipment.State
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IOrderShipment.StateProvinceID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderShipment.PostalCode
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IOrderShipment.CountryID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderShipment.Email
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderShipment.DayPhone
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderShipment.EveningPhone
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderShipment.TrackingNumber
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderShipment.TrackingURL
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> IOrderShipment.DateShippedUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderShipment.GovernmentReceiptNumber
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IOrderShipment.IsDirectShipment
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IOrderShipment.IsWillCall
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			byte[] IOrderShipment.DataVersion
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IOrderShipment.ModifiedByUserID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IOrderShipment.SourceAddressID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderShipment.PickupPointCode
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IOrder IOrderShipment.Order
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IOrderCustomer IOrderShipment.OrderCustomer
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IOrderShipmentPackage> IOrderShipment.OrderShipmentPackages
			{
				get { throw new NotImplementedException(); }
			}
		
			void IOrderShipment.AddOrderShipmentPackage(IOrderShipmentPackage item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IOrderShipment.RemoveOrderShipmentPackage(IOrderShipmentPackage item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
