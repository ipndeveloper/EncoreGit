using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Products.Common.Models
{
	/// <summary>
	/// Common interface for ProductPropertyType.
	/// </summary>
	[ContractClass(typeof(Contracts.ProductPropertyTypeContracts))]
	public interface IProductPropertyType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The ProductPropertyTypeID for this ProductPropertyType.
		/// </summary>
		int ProductPropertyTypeID { get; set; }
	
		/// <summary>
		/// The Name for this ProductPropertyType.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The DataType for this ProductPropertyType.
		/// </summary>
		string DataType { get; set; }
	
		/// <summary>
		/// The SKUSuffix for this ProductPropertyType.
		/// </summary>
		string SKUSuffix { get; set; }
	
		/// <summary>
		/// The Required for this ProductPropertyType.
		/// </summary>
		bool Required { get; set; }
	
		/// <summary>
		/// The SortIndex for this ProductPropertyType.
		/// </summary>
		int SortIndex { get; set; }
	
		/// <summary>
		/// The TermName for this ProductPropertyType.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Editable for this ProductPropertyType.
		/// </summary>
		bool Editable { get; set; }
	
		/// <summary>
		/// The IsInternal for this ProductPropertyType.
		/// </summary>
		bool IsInternal { get; set; }
	
		/// <summary>
		/// The IsProductVariantProperty for this ProductPropertyType.
		/// </summary>
		bool IsProductVariantProperty { get; set; }
	
		/// <summary>
		/// The HtmlInputTypeID for this ProductPropertyType.
		/// </summary>
		Nullable<short> HtmlInputTypeID { get; set; }
	
		/// <summary>
		/// The IsMaster for this ProductPropertyType.
		/// </summary>
		bool IsMaster { get; set; }
	
		/// <summary>
		/// The ShowNameAndThumbnail for this ProductPropertyType.
		/// </summary>
		bool ShowNameAndThumbnail { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The ProductPropertyTypeRelations for this ProductPropertyType.
		/// </summary>
		IEnumerable<IProductPropertyTypeRelation> ProductPropertyTypeRelations { get; }
	
		/// <summary>
		/// Adds an <see cref="IProductPropertyTypeRelation"/> to the ProductPropertyTypeRelations collection.
		/// </summary>
		/// <param name="item">The <see cref="IProductPropertyTypeRelation"/> to add.</param>
		void AddProductPropertyTypeRelation(IProductPropertyTypeRelation item);
	
		/// <summary>
		/// Removes an <see cref="IProductPropertyTypeRelation"/> from the ProductPropertyTypeRelations collection.
		/// </summary>
		/// <param name="item">The <see cref="IProductPropertyTypeRelation"/> to remove.</param>
		void RemoveProductPropertyTypeRelation(IProductPropertyTypeRelation item);
	
		/// <summary>
		/// The ProductPropertyTypeRelations1 for this ProductPropertyType.
		/// </summary>
		IEnumerable<IProductPropertyTypeRelation> ProductPropertyTypeRelations1 { get; }
	
		/// <summary>
		/// Adds an <see cref="IProductPropertyTypeRelation"/> to the ProductPropertyTypeRelations1 collection.
		/// </summary>
		/// <param name="item">The <see cref="IProductPropertyTypeRelation"/> to add.</param>
		void AddProductPropertyTypeRelations1(IProductPropertyTypeRelation item);
	
		/// <summary>
		/// Removes an <see cref="IProductPropertyTypeRelation"/> from the ProductPropertyTypeRelations1 collection.
		/// </summary>
		/// <param name="item">The <see cref="IProductPropertyTypeRelation"/> to remove.</param>
		void RemoveProductPropertyTypeRelations1(IProductPropertyTypeRelation item);
	
		/// <summary>
		/// The ProductPropertyValues for this ProductPropertyType.
		/// </summary>
		IEnumerable<IProductPropertyValue> ProductPropertyValues { get; }
	
		/// <summary>
		/// Adds an <see cref="IProductPropertyValue"/> to the ProductPropertyValues collection.
		/// </summary>
		/// <param name="item">The <see cref="IProductPropertyValue"/> to add.</param>
		void AddProductPropertyValue(IProductPropertyValue item);
	
		/// <summary>
		/// Removes an <see cref="IProductPropertyValue"/> from the ProductPropertyValues collection.
		/// </summary>
		/// <param name="item">The <see cref="IProductPropertyValue"/> to remove.</param>
		void RemoveProductPropertyValue(IProductPropertyValue item);
	
		/// <summary>
		/// The ProductTypes for this ProductPropertyType.
		/// </summary>
		IEnumerable<IProductType> ProductTypes { get; }
	
		/// <summary>
		/// Adds an <see cref="IProductType"/> to the ProductTypes collection.
		/// </summary>
		/// <param name="item">The <see cref="IProductType"/> to add.</param>
		void AddProductType(IProductType item);
	
		/// <summary>
		/// Removes an <see cref="IProductType"/> from the ProductTypes collection.
		/// </summary>
		/// <param name="item">The <see cref="IProductType"/> to remove.</param>
		void RemoveProductType(IProductType item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IProductPropertyType))]
		internal abstract class ProductPropertyTypeContracts : IProductPropertyType
		{
		    #region Primitive properties
		
			int IProductPropertyType.ProductPropertyTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IProductPropertyType.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IProductPropertyType.DataType
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IProductPropertyType.SKUSuffix
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IProductPropertyType.Required
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IProductPropertyType.SortIndex
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IProductPropertyType.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IProductPropertyType.Editable
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IProductPropertyType.IsInternal
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IProductPropertyType.IsProductVariantProperty
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<short> IProductPropertyType.HtmlInputTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IProductPropertyType.IsMaster
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IProductPropertyType.ShowNameAndThumbnail
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IProductPropertyTypeRelation> IProductPropertyType.ProductPropertyTypeRelations
			{
				get { throw new NotImplementedException(); }
			}
		
			void IProductPropertyType.AddProductPropertyTypeRelation(IProductPropertyTypeRelation item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IProductPropertyType.RemoveProductPropertyTypeRelation(IProductPropertyTypeRelation item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IProductPropertyTypeRelation> IProductPropertyType.ProductPropertyTypeRelations1
			{
				get { throw new NotImplementedException(); }
			}
		
			void IProductPropertyType.AddProductPropertyTypeRelations1(IProductPropertyTypeRelation item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IProductPropertyType.RemoveProductPropertyTypeRelations1(IProductPropertyTypeRelation item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IProductPropertyValue> IProductPropertyType.ProductPropertyValues
			{
				get { throw new NotImplementedException(); }
			}
		
			void IProductPropertyType.AddProductPropertyValue(IProductPropertyValue item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IProductPropertyType.RemoveProductPropertyValue(IProductPropertyValue item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IProductType> IProductPropertyType.ProductTypes
			{
				get { throw new NotImplementedException(); }
			}
		
			void IProductPropertyType.AddProductType(IProductType item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IProductPropertyType.RemoveProductType(IProductType item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
