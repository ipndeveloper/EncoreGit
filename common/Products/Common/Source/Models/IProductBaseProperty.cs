using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Products.Common.Models
{
	/// <summary>
	/// Common interface for ProductBaseProperty.
	/// </summary>
	[ContractClass(typeof(Contracts.ProductBasePropertyContracts))]
	public interface IProductBaseProperty
	{
	    #region Primitive properties
	
		/// <summary>
		/// The ProductBasePropertyID for this ProductBaseProperty.
		/// </summary>
		int ProductBasePropertyID { get; set; }
	
		/// <summary>
		/// The ProductBaseID for this ProductBaseProperty.
		/// </summary>
		int ProductBaseID { get; set; }
	
		/// <summary>
		/// The ProductPropertyTypeID for this ProductBaseProperty.
		/// </summary>
		int ProductPropertyTypeID { get; set; }
	
		/// <summary>
		/// The Name for this ProductBaseProperty.
		/// </summary>
		string Name { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The ProductBas for this ProductBaseProperty.
		/// </summary>
	    IProductBase ProductBas { get; set; }
	
		/// <summary>
		/// The ProductPropertyType for this ProductBaseProperty.
		/// </summary>
	    IProductPropertyType ProductPropertyType { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IProductBaseProperty))]
		internal abstract class ProductBasePropertyContracts : IProductBaseProperty
		{
		    #region Primitive properties
		
			int IProductBaseProperty.ProductBasePropertyID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IProductBaseProperty.ProductBaseID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IProductBaseProperty.ProductPropertyTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IProductBaseProperty.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IProductBase IProductBaseProperty.ProductBas
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IProductPropertyType IProductBaseProperty.ProductPropertyType
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
