using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Products.Common.Models
{
	/// <summary>
	/// Common interface for ProductPropertyTypeRelation.
	/// </summary>
	[ContractClass(typeof(Contracts.ProductPropertyTypeRelationContracts))]
	public interface IProductPropertyTypeRelation
	{
	    #region Primitive properties
	
		/// <summary>
		/// The ProductRelationTypeID for this ProductPropertyTypeRelation.
		/// </summary>
		int ProductRelationTypeID { get; set; }
	
		/// <summary>
		/// The ProductPropertyTypeID for this ProductPropertyTypeRelation.
		/// </summary>
		int ProductPropertyTypeID { get; set; }
	
		/// <summary>
		/// The RelatedProductPropertyTypeID for this ProductPropertyTypeRelation.
		/// </summary>
		int RelatedProductPropertyTypeID { get; set; }
	
		/// <summary>
		/// The Exclusion for this ProductPropertyTypeRelation.
		/// </summary>
		Nullable<bool> Exclusion { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The ProductPropertyType for this ProductPropertyTypeRelation.
		/// </summary>
	    IProductPropertyType ProductPropertyType { get; set; }
	
		/// <summary>
		/// The ProductPropertyType1 for this ProductPropertyTypeRelation.
		/// </summary>
	    IProductPropertyType ProductPropertyType1 { get; set; }
	
		/// <summary>
		/// The ProductRelationsType for this ProductPropertyTypeRelation.
		/// </summary>
	    IProductRelationsType ProductRelationsType { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IProductPropertyTypeRelation))]
		internal abstract class ProductPropertyTypeRelationContracts : IProductPropertyTypeRelation
		{
		    #region Primitive properties
		
			int IProductPropertyTypeRelation.ProductRelationTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IProductPropertyTypeRelation.ProductPropertyTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IProductPropertyTypeRelation.RelatedProductPropertyTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<bool> IProductPropertyTypeRelation.Exclusion
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IProductPropertyType IProductPropertyTypeRelation.ProductPropertyType
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IProductPropertyType IProductPropertyTypeRelation.ProductPropertyType1
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IProductRelationsType IProductPropertyTypeRelation.ProductRelationsType
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
