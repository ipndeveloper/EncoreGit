using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Products.Common.Models
{
	/// <summary>
	/// Common interface for ProductPriceType.
	/// </summary>
	[ContractClass(typeof(Contracts.ProductPriceTypeContracts))]
	public interface IProductPriceType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The ProductPriceTypeID for this ProductPriceType.
		/// </summary>
		int ProductPriceTypeID { get; set; }
	
		/// <summary>
		/// The Name for this ProductPriceType.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this ProductPriceType.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this ProductPriceType.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this ProductPriceType.
		/// </summary>
		bool Active { get; set; }
	
		/// <summary>
		/// The Editable for this ProductPriceType.
		/// </summary>
		bool Editable { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IProductPriceType))]
		internal abstract class ProductPriceTypeContracts : IProductPriceType
		{
		    #region Primitive properties
		
			int IProductPriceType.ProductPriceTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IProductPriceType.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IProductPriceType.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IProductPriceType.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IProductPriceType.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IProductPriceType.Editable
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
