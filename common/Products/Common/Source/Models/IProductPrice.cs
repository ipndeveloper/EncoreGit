using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Products.Common.Models
{
	/// <summary>
	/// Common interface for ProductPrice.
	/// </summary>
	[ContractClass(typeof(Contracts.ProductPriceContracts))]
	public interface IProductPrice
	{
	    #region Primitive properties
	
		/// <summary>
		/// The ProductPriceID for this ProductPrice.
		/// </summary>
		int ProductPriceID { get; set; }
	
		/// <summary>
		/// The ProductPriceTypeID for this ProductPrice.
		/// </summary>
		int ProductPriceTypeID { get; set; }
	
		/// <summary>
		/// The ProductID for this ProductPrice.
		/// </summary>
		int ProductID { get; set; }
	
		/// <summary>
		/// The CurrencyID for this ProductPrice.
		/// </summary>
		int CurrencyID { get; set; }
	
		/// <summary>
		/// The Price for this ProductPrice.
		/// </summary>
		decimal Price { get; set; }
	
		/// <summary>
		/// The ModifiedByUserID for this ProductPrice.
		/// </summary>
		Nullable<int> ModifiedByUserID { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The Product for this ProductPrice.
		/// </summary>
	    IProduct Product { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IProductPrice))]
		internal abstract class ProductPriceContracts : IProductPrice
		{
		    #region Primitive properties
		
			int IProductPrice.ProductPriceID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IProductPrice.ProductPriceTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IProductPrice.ProductID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IProductPrice.CurrencyID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			decimal IProductPrice.Price
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IProductPrice.ModifiedByUserID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IProduct IProductPrice.Product
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
