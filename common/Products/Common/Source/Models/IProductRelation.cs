using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Products.Common.Models
{
	/// <summary>
	/// Common interface for ProductRelation.
	/// </summary>
	[ContractClass(typeof(Contracts.ProductRelationContracts))]
	public interface IProductRelation
	{
	    #region Primitive properties
	
		/// <summary>
		/// The ProductRelationID for this ProductRelation.
		/// </summary>
		int ProductRelationID { get; set; }
	
		/// <summary>
		/// The ProductRelationsTypeID for this ProductRelation.
		/// </summary>
		int ProductRelationsTypeID { get; set; }
	
		/// <summary>
		/// The ParentProductID for this ProductRelation.
		/// </summary>
		int ParentProductID { get; set; }
	
		/// <summary>
		/// The ChildProductID for this ProductRelation.
		/// </summary>
		int ChildProductID { get; set; }
	
		/// <summary>
		/// The Exclusion for this ProductRelation.
		/// </summary>
		Nullable<bool> Exclusion { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The Product for this ProductRelation.
		/// </summary>
	    IProduct Product { get; set; }
	
		/// <summary>
		/// The Product1 for this ProductRelation.
		/// </summary>
	    IProduct Product1 { get; set; }
	
		/// <summary>
		/// The ProductRelationsType for this ProductRelation.
		/// </summary>
	    IProductRelationsType ProductRelationsType { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IProductRelation))]
		internal abstract class ProductRelationContracts : IProductRelation
		{
		    #region Primitive properties
		
			int IProductRelation.ProductRelationID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IProductRelation.ProductRelationsTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IProductRelation.ParentProductID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IProductRelation.ChildProductID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<bool> IProductRelation.Exclusion
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IProduct IProductRelation.Product
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IProduct IProductRelation.Product1
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IProductRelationsType IProductRelation.ProductRelationsType
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
