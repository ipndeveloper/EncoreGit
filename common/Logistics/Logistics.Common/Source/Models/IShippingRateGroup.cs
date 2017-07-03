using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Logistics.Common.Models
{
	/// <summary>
	/// Common interface for ShippingRateGroup.
	/// </summary>
	[ContractClass(typeof(Contracts.ShippingRateGroupContracts))]
	public interface IShippingRateGroup
	{
	    #region Primitive properties
	
		/// <summary>
		/// The ShippingRateGroupID for this ShippingRateGroup.
		/// </summary>
		int ShippingRateGroupID { get; set; }
	
		/// <summary>
		/// The Name for this ShippingRateGroup.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The Description for this ShippingRateGroup.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The GroupCode for this ShippingRateGroup.
		/// </summary>
		string GroupCode { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The ShippingOrderTypes for this ShippingRateGroup.
		/// </summary>
		IEnumerable<IShippingOrderType> ShippingOrderTypes { get; }
	
		/// <summary>
		/// Adds an <see cref="IShippingOrderType"/> to the ShippingOrderTypes collection.
		/// </summary>
		/// <param name="item">The <see cref="IShippingOrderType"/> to add.</param>
		void AddShippingOrderType(IShippingOrderType item);
	
		/// <summary>
		/// Removes an <see cref="IShippingOrderType"/> from the ShippingOrderTypes collection.
		/// </summary>
		/// <param name="item">The <see cref="IShippingOrderType"/> to remove.</param>
		void RemoveShippingOrderType(IShippingOrderType item);
	
		/// <summary>
		/// The ShippingRates for this ShippingRateGroup.
		/// </summary>
		IEnumerable<IShippingRate> ShippingRates { get; }
	
		/// <summary>
		/// Adds an <see cref="IShippingRate"/> to the ShippingRates collection.
		/// </summary>
		/// <param name="item">The <see cref="IShippingRate"/> to add.</param>
		void AddShippingRate(IShippingRate item);
	
		/// <summary>
		/// Removes an <see cref="IShippingRate"/> from the ShippingRates collection.
		/// </summary>
		/// <param name="item">The <see cref="IShippingRate"/> to remove.</param>
		void RemoveShippingRate(IShippingRate item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IShippingRateGroup))]
		internal abstract class ShippingRateGroupContracts : IShippingRateGroup
		{
		    #region Primitive properties
		
			int IShippingRateGroup.ShippingRateGroupID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IShippingRateGroup.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IShippingRateGroup.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IShippingRateGroup.GroupCode
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IShippingOrderType> IShippingRateGroup.ShippingOrderTypes
			{
				get { throw new NotImplementedException(); }
			}
		
			void IShippingRateGroup.AddShippingOrderType(IShippingOrderType item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IShippingRateGroup.RemoveShippingOrderType(IShippingOrderType item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IShippingRate> IShippingRateGroup.ShippingRates
			{
				get { throw new NotImplementedException(); }
			}
		
			void IShippingRateGroup.AddShippingRate(IShippingRate item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IShippingRateGroup.RemoveShippingRate(IShippingRate item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
