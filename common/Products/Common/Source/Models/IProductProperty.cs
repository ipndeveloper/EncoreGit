using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Products.Common.Models
{
	/// <summary>
	/// Common interface for ProductProperty.
	/// </summary>
	[ContractClass(typeof(Contracts.ProductPropertyContracts))]
	public interface IProductProperty
	{
	    #region Primitive properties
	
		/// <summary>
		/// The ProductPropertyID for this ProductProperty.
		/// </summary>
		int ProductPropertyID { get; set; }
	
		/// <summary>
		/// The ProductID for this ProductProperty.
		/// </summary>
		int ProductID { get; set; }
	
		/// <summary>
		/// The ProductPropertyTypeID for this ProductProperty.
		/// </summary>
		int ProductPropertyTypeID { get; set; }
	
		/// <summary>
		/// The ProductPropertyValueID for this ProductProperty.
		/// </summary>
		Nullable<int> ProductPropertyValueID { get; set; }
	
		/// <summary>
		/// The Active for this ProductProperty.
		/// </summary>
		bool Active { get; set; }
	
		/// <summary>
		/// The PropertyValue for this ProductProperty.
		/// </summary>
		string PropertyValue { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The Product for this ProductProperty.
		/// </summary>
	    IProduct Product { get; set; }
	
		/// <summary>
		/// The ProductPropertyType for this ProductProperty.
		/// </summary>
	    IProductPropertyType ProductPropertyType { get; set; }
	
		/// <summary>
		/// The ProductPropertyValue for this ProductProperty.
		/// </summary>
	    IProductPropertyValue ProductPropertyValue { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IProductProperty))]
		internal abstract class ProductPropertyContracts : IProductProperty
		{
		    #region Primitive properties
		
			int IProductProperty.ProductPropertyID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IProductProperty.ProductID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IProductProperty.ProductPropertyTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IProductProperty.ProductPropertyValueID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IProductProperty.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IProductProperty.PropertyValue
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IProduct IProductProperty.Product
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IProductPropertyType IProductProperty.ProductPropertyType
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IProductPropertyValue IProductProperty.ProductPropertyValue
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
