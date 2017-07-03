using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Products.Common.Models
{
	/// <summary>
	/// Common interface for DynamicKitPricingType.
	/// </summary>
	[ContractClass(typeof(Contracts.DynamicKitPricingTypeContracts))]
	public interface IDynamicKitPricingType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The DynamicKitPricingTypeID for this DynamicKitPricingType.
		/// </summary>
		int DynamicKitPricingTypeID { get; set; }
	
		/// <summary>
		/// The Name for this DynamicKitPricingType.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this DynamicKitPricingType.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this DynamicKitPricingType.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this DynamicKitPricingType.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The DynamicKits for this DynamicKitPricingType.
		/// </summary>
		IEnumerable<IDynamicKit> DynamicKits { get; }
	
		/// <summary>
		/// Adds an <see cref="IDynamicKit"/> to the DynamicKits collection.
		/// </summary>
		/// <param name="item">The <see cref="IDynamicKit"/> to add.</param>
		void AddDynamicKit(IDynamicKit item);
	
		/// <summary>
		/// Removes an <see cref="IDynamicKit"/> from the DynamicKits collection.
		/// </summary>
		/// <param name="item">The <see cref="IDynamicKit"/> to remove.</param>
		void RemoveDynamicKit(IDynamicKit item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IDynamicKitPricingType))]
		internal abstract class DynamicKitPricingTypeContracts : IDynamicKitPricingType
		{
		    #region Primitive properties
		
			int IDynamicKitPricingType.DynamicKitPricingTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IDynamicKitPricingType.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IDynamicKitPricingType.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IDynamicKitPricingType.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IDynamicKitPricingType.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IDynamicKit> IDynamicKitPricingType.DynamicKits
			{
				get { throw new NotImplementedException(); }
			}
		
			void IDynamicKitPricingType.AddDynamicKit(IDynamicKit item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IDynamicKitPricingType.RemoveDynamicKit(IDynamicKit item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
