using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Products.Common.Models
{
	/// <summary>
	/// Common interface for CatalogItem.
	/// </summary>
	[ContractClass(typeof(Contracts.CatalogItemContracts))]
	public interface ICatalogItem
	{
	    #region Primitive properties
	
		/// <summary>
		/// The CatalogItemID for this CatalogItem.
		/// </summary>
		int CatalogItemID { get; set; }
	
		/// <summary>
		/// The CatalogID for this CatalogItem.
		/// </summary>
		int CatalogID { get; set; }
	
		/// <summary>
		/// The ProductID for this CatalogItem.
		/// </summary>
		int ProductID { get; set; }
	
		/// <summary>
		/// The SortIndex for this CatalogItem.
		/// </summary>
		int SortIndex { get; set; }
	
		/// <summary>
		/// The StartDateUTC for this CatalogItem.
		/// </summary>
		Nullable<System.DateTime> StartDateUTC { get; set; }
	
		/// <summary>
		/// The EndDateUTC for this CatalogItem.
		/// </summary>
		Nullable<System.DateTime> EndDateUTC { get; set; }
	
		/// <summary>
		/// The Active for this CatalogItem.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The Catalog for this CatalogItem.
		/// </summary>
	    ICatalog Catalog { get; set; }
	
		/// <summary>
		/// The Product for this CatalogItem.
		/// </summary>
	    IProduct Product { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(ICatalogItem))]
		internal abstract class CatalogItemContracts : ICatalogItem
		{
		    #region Primitive properties
		
			int ICatalogItem.CatalogItemID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int ICatalogItem.CatalogID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int ICatalogItem.ProductID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int ICatalogItem.SortIndex
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> ICatalogItem.StartDateUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> ICatalogItem.EndDateUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool ICatalogItem.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    ICatalog ICatalogItem.Catalog
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IProduct ICatalogItem.Product
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
