using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using NetSteps.Content.Common.Models;

namespace NetSteps.Products.Common.Models
{
	/// <summary>
	/// Common interface for Catalog.
	/// </summary>
	[ContractClass(typeof(Contracts.CatalogContracts))]
	public interface ICatalog
	{
	    #region Primitive properties
	
		/// <summary>
		/// The CatalogID for this Catalog.
		/// </summary>
		int CatalogID { get; set; }
	
		/// <summary>
		/// The ParentCatalogID for this Catalog.
		/// </summary>
		Nullable<int> ParentCatalogID { get; set; }
	
		/// <summary>
		/// The CategoryID for this Catalog.
		/// </summary>
		int CategoryID { get; set; }
	
		/// <summary>
		/// The DisplayImage for this Catalog.
		/// </summary>
		string DisplayImage { get; set; }
	
		/// <summary>
		/// The SortIndex for this Catalog.
		/// </summary>
		int SortIndex { get; set; }
	
		/// <summary>
		/// The StartDateUTC for this Catalog.
		/// </summary>
		Nullable<System.DateTime> StartDateUTC { get; set; }
	
		/// <summary>
		/// The EndDateUTC for this Catalog.
		/// </summary>
		Nullable<System.DateTime> EndDateUTC { get; set; }
	
		/// <summary>
		/// The Active for this Catalog.
		/// </summary>
		bool Active { get; set; }
	
		/// <summary>
		/// The CatalogTypeID for this Catalog.
		/// </summary>
		short CatalogTypeID { get; set; }
	
		/// <summary>
		/// The Editable for this Catalog.
		/// </summary>
		bool Editable { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The CatalogItems for this Catalog.
		/// </summary>
		IEnumerable<ICatalogItem> CatalogItems { get; }
	
		/// <summary>
		/// Adds an <see cref="ICatalogItem"/> to the CatalogItems collection.
		/// </summary>
		/// <param name="item">The <see cref="ICatalogItem"/> to add.</param>
		void AddCatalogItem(ICatalogItem item);
	
		/// <summary>
		/// Removes an <see cref="ICatalogItem"/> from the CatalogItems collection.
		/// </summary>
		/// <param name="item">The <see cref="ICatalogItem"/> to remove.</param>
		void RemoveCatalogItem(ICatalogItem item);
	
		/// <summary>
		/// The Translations for this Catalog.
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
		[ContractClassFor(typeof(ICatalog))]
		internal abstract class CatalogContracts : ICatalog
		{
		    #region Primitive properties
		
			int ICatalog.CatalogID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> ICatalog.ParentCatalogID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int ICatalog.CategoryID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ICatalog.DisplayImage
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int ICatalog.SortIndex
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> ICatalog.StartDateUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> ICatalog.EndDateUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool ICatalog.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			short ICatalog.CatalogTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool ICatalog.Editable
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<ICatalogItem> ICatalog.CatalogItems
			{
				get { throw new NotImplementedException(); }
			}
		
			void ICatalog.AddCatalogItem(ICatalogItem item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void ICatalog.RemoveCatalogItem(ICatalogItem item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IDescriptionTranslation> ICatalog.Translations
			{
				get { throw new NotImplementedException(); }
			}
		
			void ICatalog.AddTranslation(IDescriptionTranslation item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void ICatalog.RemoveTranslation(IDescriptionTranslation item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
