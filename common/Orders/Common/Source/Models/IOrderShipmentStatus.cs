using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Orders.Common.Models
{
	/// <summary>
	/// Common interface for OrderShipmentStatus.
	/// </summary>
	[ContractClass(typeof(Contracts.OrderShipmentStatusContracts))]
	public interface IOrderShipmentStatus
	{
	    #region Primitive properties
	
		/// <summary>
		/// The OrderShipmentStatusID for this OrderShipmentStatus.
		/// </summary>
		short OrderShipmentStatusID { get; set; }
	
		/// <summary>
		/// The Name for this OrderShipmentStatus.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this OrderShipmentStatus.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this OrderShipmentStatus.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this OrderShipmentStatus.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The OrderShipments for this OrderShipmentStatus.
		/// </summary>
		IEnumerable<IOrderShipment> OrderShipments { get; }
	
		/// <summary>
		/// Adds an <see cref="IOrderShipment"/> to the OrderShipments collection.
		/// </summary>
		/// <param name="item">The <see cref="IOrderShipment"/> to add.</param>
		void AddOrderShipment(IOrderShipment item);
	
		/// <summary>
		/// Removes an <see cref="IOrderShipment"/> from the OrderShipments collection.
		/// </summary>
		/// <param name="item">The <see cref="IOrderShipment"/> to remove.</param>
		void RemoveOrderShipment(IOrderShipment item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IOrderShipmentStatus))]
		internal abstract class OrderShipmentStatusContracts : IOrderShipmentStatus
		{
		    #region Primitive properties
		
			short IOrderShipmentStatus.OrderShipmentStatusID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderShipmentStatus.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderShipmentStatus.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOrderShipmentStatus.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IOrderShipmentStatus.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IOrderShipment> IOrderShipmentStatus.OrderShipments
			{
				get { throw new NotImplementedException(); }
			}
		
			void IOrderShipmentStatus.AddOrderShipment(IOrderShipment item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IOrderShipmentStatus.RemoveOrderShipment(IOrderShipment item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
