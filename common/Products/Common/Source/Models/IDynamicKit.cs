using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Products.Common.Models
{
	/// <summary>
	/// Common interface for DynamicKit.
	/// </summary>
	[ContractClass(typeof(Contracts.DynamicKitContracts))]
	public interface IDynamicKit
	{
	    #region Primitive properties
	
		/// <summary>
		/// The DynamicKitID for this DynamicKit.
		/// </summary>
		int DynamicKitID { get; set; }
	
		/// <summary>
		/// The ProductID for this DynamicKit.
		/// </summary>
		int ProductID { get; set; }
	
		/// <summary>
		/// The DynamicKitPricingTypeID for this DynamicKit.
		/// </summary>
		Nullable<int> DynamicKitPricingTypeID { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The DynamicKitPricingType for this DynamicKit.
		/// </summary>
	    IDynamicKitPricingType DynamicKitPricingType { get; set; }
	
		/// <summary>
		/// The Product for this DynamicKit.
		/// </summary>
	    IProduct Product { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The DynamicKitGroups for this DynamicKit.
		/// </summary>
		IEnumerable<IDynamicKitGroup> DynamicKitGroups { get; }
	
		/// <summary>
		/// Adds an <see cref="IDynamicKitGroup"/> to the DynamicKitGroups collection.
		/// </summary>
		/// <param name="item">The <see cref="IDynamicKitGroup"/> to add.</param>
		void AddDynamicKitGroup(IDynamicKitGroup item);
	
		/// <summary>
		/// Removes an <see cref="IDynamicKitGroup"/> from the DynamicKitGroups collection.
		/// </summary>
		/// <param name="item">The <see cref="IDynamicKitGroup"/> to remove.</param>
		void RemoveDynamicKitGroup(IDynamicKitGroup item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IDynamicKit))]
		internal abstract class DynamicKitContracts : IDynamicKit
		{
		    #region Primitive properties
		
			int IDynamicKit.DynamicKitID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IDynamicKit.ProductID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IDynamicKit.DynamicKitPricingTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IDynamicKitPricingType IDynamicKit.DynamicKitPricingType
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IProduct IDynamicKit.Product
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IDynamicKitGroup> IDynamicKit.DynamicKitGroups
			{
				get { throw new NotImplementedException(); }
			}
		
			void IDynamicKit.AddDynamicKitGroup(IDynamicKitGroup item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IDynamicKit.RemoveDynamicKitGroup(IDynamicKitGroup item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
