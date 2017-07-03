using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using NetSteps.Common.Models;
using NetSteps.Content.Common.Models;

namespace NetSteps.Products.Common.Models
{
	/// <summary>
	/// Common interface for ProductBase.
	/// </summary>
	[ContractClass(typeof(Contracts.ProductBaseContracts))]
	public interface IProductBase
	{
	    #region Primitive properties
	
		/// <summary>
		/// The ProductBaseID for this ProductBase.
		/// </summary>
		int ProductBaseID { get; set; }
	
		/// <summary>
		/// The ProductTypeID for this ProductBase.
		/// </summary>
		int ProductTypeID { get; set; }
	
		/// <summary>
		/// The TaxCategoryID for this ProductBase.
		/// </summary>
		Nullable<int> TaxCategoryID { get; set; }
	
		/// <summary>
		/// The BaseSKU for this ProductBase.
		/// </summary>
		string BaseSKU { get; set; }
	
		/// <summary>
		/// The ChargeShipping for this ProductBase.
		/// </summary>
		bool ChargeShipping { get; set; }
	
		/// <summary>
		/// The ChargeTax for this ProductBase.
		/// </summary>
		bool ChargeTax { get; set; }
	
		/// <summary>
		/// The ChargeTaxOnShipping for this ProductBase.
		/// </summary>
		bool ChargeTaxOnShipping { get; set; }
	
		/// <summary>
		/// The IsTaxedAtChild for this ProductBase.
		/// </summary>
		Nullable<bool> IsTaxedAtChild { get; set; }
	
		/// <summary>
		/// The IsShippable for this ProductBase.
		/// </summary>
		bool IsShippable { get; set; }
	
		/// <summary>
		/// The ModifiedByUserID for this ProductBase.
		/// </summary>
		Nullable<int> ModifiedByUserID { get; set; }
	
		/// <summary>
		/// The Editable for this ProductBase.
		/// </summary>
		bool Editable { get; set; }
	
		/// <summary>
		/// The UpdateInventoryOnBase for this ProductBase.
		/// </summary>
		bool UpdateInventoryOnBase { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The Categories for this ProductBase.
		/// </summary>
		IEnumerable<ICategory> Categories { get; }
	
		/// <summary>
		/// Adds an <see cref="ICategory"/> to the Categories collection.
		/// </summary>
		/// <param name="item">The <see cref="ICategory"/> to add.</param>
		void AddCategory(ICategory item);
	
		/// <summary>
		/// Removes an <see cref="ICategory"/> from the Categories collection.
		/// </summary>
		/// <param name="item">The <see cref="ICategory"/> to remove.</param>
		void RemoveCategory(ICategory item);
	
		/// <summary>
		/// The ProductBaseProperties for this ProductBase.
		/// </summary>
		IEnumerable<IProductBaseProperty> ProductBaseProperties { get; }
	
		/// <summary>
		/// Adds an <see cref="IProductBaseProperty"/> to the ProductBaseProperties collection.
		/// </summary>
		/// <param name="item">The <see cref="IProductBaseProperty"/> to add.</param>
		void AddProductBaseProperty(IProductBaseProperty item);
	
		/// <summary>
		/// Removes an <see cref="IProductBaseProperty"/> from the ProductBaseProperties collection.
		/// </summary>
		/// <param name="item">The <see cref="IProductBaseProperty"/> to remove.</param>
		void RemoveProductBaseProperty(IProductBaseProperty item);
	
		/// <summary>
		/// The ProductBasePropertyValues for this ProductBase.
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
		/// The Translations for this ProductBase.
		/// </summary>
		IEnumerable<IDescriptionTranslation> Translations { get; }
	
		/// <summary>
		/// Adds an <see cref="IDescriptionTranslation"/> to the Translations collection.
		/// </summary>
		/// <param name="item">The <see cref="IDescriptionTranslation"/> to add.</param>
		void AddTranslation(IDescriptionTranslation item);
	
		/// <summary>
		/// Removes an <see cref="IDescriptionTranslation"/> from the Translations collection.
		/// </summary>
		/// <param name="item">The <see cref="IDescriptionTranslation"/> to remove.</param>
		void RemoveTranslation(IDescriptionTranslation item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IProductBase))]
		internal abstract class ProductBaseContracts : IProductBase
		{
		    #region Primitive properties
		
			int IProductBase.ProductBaseID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IProductBase.ProductTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IProductBase.TaxCategoryID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IProductBase.BaseSKU
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IProductBase.ChargeShipping
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IProductBase.ChargeTax
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IProductBase.ChargeTaxOnShipping
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<bool> IProductBase.IsTaxedAtChild
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IProductBase.IsShippable
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IProductBase.ModifiedByUserID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IProductBase.Editable
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IProductBase.UpdateInventoryOnBase
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<ICategory> IProductBase.Categories
			{
				get { throw new NotImplementedException(); }
			}
		
			void IProductBase.AddCategory(ICategory item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IProductBase.RemoveCategory(ICategory item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IProductBaseProperty> IProductBase.ProductBaseProperties
			{
				get { throw new NotImplementedException(); }
			}
		
			void IProductBase.AddProductBaseProperty(IProductBaseProperty item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IProductBase.RemoveProductBaseProperty(IProductBaseProperty item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IProductBasePropertyValue> IProductBase.ProductBasePropertyValues
			{
				get { throw new NotImplementedException(); }
			}
		
			void IProductBase.AddProductBasePropertyValue(IProductBasePropertyValue item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IProductBase.RemoveProductBasePropertyValue(IProductBasePropertyValue item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IDescriptionTranslation> IProductBase.Translations
			{
				get { throw new NotImplementedException(); }
			}
		
			void IProductBase.AddTranslation(IDescriptionTranslation item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IProductBase.RemoveTranslation(IDescriptionTranslation item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
