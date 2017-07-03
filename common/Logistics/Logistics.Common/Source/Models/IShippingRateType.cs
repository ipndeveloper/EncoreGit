using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Logistics.Common.Models
{
	/// <summary>
	/// Common interface for ShippingRateType.
	/// </summary>
	[ContractClass(typeof(Contracts.ShippingRateTypeContracts))]
	public interface IShippingRateType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The ShippingRateTypeID for this ShippingRateType.
		/// </summary>
		short ShippingRateTypeID { get; set; }
	
		/// <summary>
		/// The Name for this ShippingRateType.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this ShippingRateType.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this ShippingRateType.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this ShippingRateType.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The ShippingRates for this ShippingRateType.
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
		[ContractClassFor(typeof(IShippingRateType))]
		internal abstract class ShippingRateTypeContracts : IShippingRateType
		{
		    #region Primitive properties
		
			short IShippingRateType.ShippingRateTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IShippingRateType.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IShippingRateType.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IShippingRateType.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IShippingRateType.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IShippingRate> IShippingRateType.ShippingRates
			{
				get { throw new NotImplementedException(); }
			}
		
			void IShippingRateType.AddShippingRate(IShippingRate item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IShippingRateType.RemoveShippingRate(IShippingRate item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
