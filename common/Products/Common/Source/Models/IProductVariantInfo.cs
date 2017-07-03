using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Products.Common.Models
{
	/// <summary>
	/// Common interface for ProductVariantInfo.
	/// </summary>
	[ContractClass(typeof(Contracts.ProductVariantInfoContracts))]
	public interface IProductVariantInfo
	{
	    #region Primitive properties
	
		/// <summary>
		/// The ProductID for this ProductVariantInfo.
		/// </summary>
		int ProductID { get; set; }
	
		/// <summary>
		/// The UseMasterPricing for this ProductVariantInfo.
		/// </summary>
		bool UseMasterPricing { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The Product for this ProductVariantInfo.
		/// </summary>
	    IProduct Product { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IProductVariantInfo))]
		internal abstract class ProductVariantInfoContracts : IProductVariantInfo
		{
		    #region Primitive properties
		
			int IProductVariantInfo.ProductID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IProductVariantInfo.UseMasterPricing
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IProduct IProductVariantInfo.Product
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
