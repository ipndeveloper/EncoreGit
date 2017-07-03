using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using NetSteps.Content.Common.Models;

namespace NetSteps.Products.Common.Models
{
	/// <summary>
	/// Common interface for Product.
	/// </summary>
	[ContractClass(typeof(Contracts.ProductContracts))]
	public interface IProduct
	{
	    #region Primitive properties
	
		/// <summary>
		/// The ProductID for this Product.
		/// </summary>
		int ProductID { get; set; }
	
		/// <summary>
		/// The ProductBaseID for this Product.
		/// </summary>
		int ProductBaseID { get; set; }
	
		/// <summary>
		/// The SKU for this Product.
		/// </summary>
		string SKU { get; set; }
	
		/// <summary>
		/// The SortIndex for this Product.
		/// </summary>
		int SortIndex { get; set; }
	
		/// <summary>
		/// The Active for this Product.
		/// </summary>
		bool Active { get; set; }
	
		/// <summary>
		/// The Weight for this Product.
		/// </summary>
		Nullable<double> Weight { get; set; }
	
		/// <summary>
		/// The WarehouseStickyShip for this Product.
		/// </summary>
		bool WarehouseStickyShip { get; set; }
	
		/// <summary>
		/// The ProductBackOrderBehaviorID for this Product.
		/// </summary>
		short ProductBackOrderBehaviorID { get; set; }
	
		/// <summary>
		/// The ShowKitContents for this Product.
		/// </summary>
		bool ShowKitContents { get; set; }
	
		/// <summary>
		/// The ModifiedByUserID for this Product.
		/// </summary>
		Nullable<int> ModifiedByUserID { get; set; }
	
		/// <summary>
		/// The ProductNumber for this Product.
		/// </summary>
		string ProductNumber { get; set; }
	
		/// <summary>
		/// The IsVariantTemplate for this Product.
		/// </summary>
		bool IsVariantTemplate { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The ProductVariantInfo for this Product.
		/// </summary>
	    IProductVariantInfo ProductVariantInfo { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The ChildProductRelations for this Product.
		/// </summary>
		IEnumerable<IProductRelation> ChildProductRelations { get; }
	
		/// <summary>
		/// Adds an <see cref="IProductRelation"/> to the ChildProductRelations collection.
		/// </summary>
		/// <param name="item">The <see cref="IProductRelation"/> to add.</param>
		void AddChildProductRelation(IProductRelation item);
	
		/// <summary>
		/// Removes an <see cref="IProductRelation"/> from the ChildProductRelations collection.
		/// </summary>
		/// <param name="item">The <see cref="IProductRelation"/> to remove.</param>
		void RemoveChildProductRelation(IProductRelation item);
	
		/// <summary>
		/// The Files for this Product.
		/// </summary>
		IEnumerable<IProductFile> Files { get; }
	
		/// <summary>
		/// Adds an <see cref="IProductFile"/> to the Files collection.
		/// </summary>
		/// <param name="item">The <see cref="IProductFile"/> to add.</param>
		void AddFile(IProductFile item);
	
		/// <summary>
		/// Removes an <see cref="IProductFile"/> from the Files collection.
		/// </summary>
		/// <param name="item">The <see cref="IProductFile"/> to remove.</param>
		void RemoveFile(IProductFile item);
	
		/// <summary>
		/// The ParentProductRelations for this Product.
		/// </summary>
		IEnumerable<IProductRelation> ParentProductRelations { get; }
	
		/// <summary>
		/// Adds an <see cref="IProductRelation"/> to the ParentProductRelations collection.
		/// </summary>
		/// <param name="item">The <see cref="IProductRelation"/> to add.</param>
		void AddParentProductRelation(IProductRelation item);
	
		/// <summary>
		/// Removes an <see cref="IProductRelation"/> from the ParentProductRelations collection.
		/// </summary>
		/// <param name="item">The <see cref="IProductRelation"/> to remove.</param>
		void RemoveParentProductRelation(IProductRelation item);
	
		/// <summary>
		/// The Prices for this Product.
		/// </summary>
		IEnumerable<IProductPrice> Prices { get; }
	
		/// <summary>
		/// Adds an <see cref="IProductPrice"/> to the Prices collection.
		/// </summary>
		/// <param name="item">The <see cref="IProductPrice"/> to add.</param>
		void AddPrice(IProductPrice item);
	
		/// <summary>
		/// Removes an <see cref="IProductPrice"/> from the Prices collection.
		/// </summary>
		/// <param name="item">The <see cref="IProductPrice"/> to remove.</param>
		void RemovePrice(IProductPrice item);
	
		/// <summary>
		/// The Properties for this Product.
		/// </summary>
		IEnumerable<IProductProperty> Properties { get; }
	
		/// <summary>
		/// Adds an <see cref="IProductProperty"/> to the Properties collection.
		/// </summary>
		/// <param name="item">The <see cref="IProductProperty"/> to add.</param>
		void AddProperty(IProductProperty item);
	
		/// <summary>
		/// Removes an <see cref="IProductProperty"/> from the Properties collection.
		/// </summary>
		/// <param name="item">The <see cref="IProductProperty"/> to remove.</param>
		void RemoveProperty(IProductProperty item);
	
		/// <summary>
		/// The Translations for this Product.
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
		[ContractClassFor(typeof(IProduct))]
		internal abstract class ProductContracts : IProduct
		{
		    #region Primitive properties
		
			int IProduct.ProductID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IProduct.ProductBaseID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IProduct.SKU
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IProduct.SortIndex
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IProduct.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<double> IProduct.Weight
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IProduct.WarehouseStickyShip
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			short IProduct.ProductBackOrderBehaviorID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IProduct.ShowKitContents
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IProduct.ModifiedByUserID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IProduct.ProductNumber
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IProduct.IsVariantTemplate
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IProductVariantInfo IProduct.ProductVariantInfo
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IProductRelation> IProduct.ChildProductRelations
			{
				get { throw new NotImplementedException(); }
			}
		
			void IProduct.AddChildProductRelation(IProductRelation item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IProduct.RemoveChildProductRelation(IProductRelation item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IProductFile> IProduct.Files
			{
				get { throw new NotImplementedException(); }
			}
		
			void IProduct.AddFile(IProductFile item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IProduct.RemoveFile(IProductFile item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IProductRelation> IProduct.ParentProductRelations
			{
				get { throw new NotImplementedException(); }
			}
		
			void IProduct.AddParentProductRelation(IProductRelation item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IProduct.RemoveParentProductRelation(IProductRelation item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IProductPrice> IProduct.Prices
			{
				get { throw new NotImplementedException(); }
			}
		
			void IProduct.AddPrice(IProductPrice item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IProduct.RemovePrice(IProductPrice item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IProductProperty> IProduct.Properties
			{
				get { throw new NotImplementedException(); }
			}
		
			void IProduct.AddProperty(IProductProperty item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IProduct.RemoveProperty(IProductProperty item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IDescriptionTranslation> IProduct.Translations
			{
				get { throw new NotImplementedException(); }
			}
		
			void IProduct.AddTranslation(IDescriptionTranslation item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IProduct.RemoveTranslation(IDescriptionTranslation item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
