using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Products.Common.Models
{
	/// <summary>
	/// Common interface for ProductPropertyValue.
	/// </summary>
	[ContractClass(typeof(Contracts.ProductPropertyValueContracts))]
	public interface IProductPropertyValue
	{
	    #region Primitive properties
	
		/// <summary>
		/// The ProductPropertyValueID for this ProductPropertyValue.
		/// </summary>
		int ProductPropertyValueID { get; set; }
	
		/// <summary>
		/// The ProductPropertyTypeID for this ProductPropertyValue.
		/// </summary>
		int ProductPropertyTypeID { get; set; }
	
		/// <summary>
		/// The Name for this ProductPropertyValue.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The Value for this ProductPropertyValue.
		/// </summary>
		string Value { get; set; }
	
		/// <summary>
		/// The FilePath for this ProductPropertyValue.
		/// </summary>
		string FilePath { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The ProductPropertyType for this ProductPropertyValue.
		/// </summary>
	    IProductPropertyType ProductPropertyType { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The ProductBasePropertyValues for this ProductPropertyValue.
		/// </summary>
		IEnumerable<IProductBasePropertyValue> ProductBasePropertyValues { get; }
	
		/// <summary>
		/// Adds an <see cref="IProductBasePropertyValue"/> to the ProductBasePropertyValues collection.
		/// </summary>
		/// <param name="item">The <see cref="IProductBasePropertyValue"/> to add.</param>
		void AddProductBasePropertyValue(IProductBasePropertyValue item);
	
		/// <summary>
		/// Removes an <see cref="IProductBasePropertyValue"/> from the ProductBasePropertyValues collection.
		/// </summary>
		/// <param name="item">The <see cref="IProductBasePropertyValue"/> to remove.</param>
		void RemoveProductBasePropertyValue(IProductBasePropertyValue item);
	
		/// <summary>
		/// The ProductProperties for this ProductPropertyValue.
		/// </summary>
		IEnumerable<IProductProperty> ProductProperties { get; }
	
		/// <summary>
		/// Adds an <see cref="IProductProperty"/> to the ProductProperties collection.
		/// </summary>
		/// <param name="item">The <see cref="IProductProperty"/> to add.</param>
		void AddProductProperty(IProductProperty item);
	
		/// <summary>
		/// Removes an <see cref="IProductProperty"/> from the ProductProperties collection.
		/// </summary>
		/// <param name="item">The <see cref="IProductProperty"/> to remove.</param>
		void RemoveProductProperty(IProductProperty item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IProductPropertyValue))]
		internal abstract class ProductPropertyValueContracts : IProductPropertyValue
		{
		    #region Primitive properties
		
			int IProductPropertyValue.ProductPropertyValueID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IProductPropertyValue.ProductPropertyTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IProductPropertyValue.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IProductPropertyValue.Value
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IProductPropertyValue.FilePath
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IProductPropertyType IProductPropertyValue.ProductPropertyType
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IProductBasePropertyValue> IProductPropertyValue.ProductBasePropertyValues
			{
				get { throw new NotImplementedException(); }
			}
		
			void IProductPropertyValue.AddProductBasePropertyValue(IProductBasePropertyValue item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IProductPropertyValue.RemoveProductBasePropertyValue(IProductBasePropertyValue item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IProductProperty> IProductPropertyValue.ProductProperties
			{
				get { throw new NotImplementedException(); }
			}
		
			void IProductPropertyValue.AddProductProperty(IProductProperty item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IProductPropertyValue.RemoveProductProperty(IProductProperty item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
