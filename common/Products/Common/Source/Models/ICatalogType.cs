using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Products.Common.Models
{
	/// <summary>
	/// Common interface for CatalogType.
	/// </summary>
	[ContractClass(typeof(Contracts.CatalogTypeContracts))]
	public interface ICatalogType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The CatalogTypeID for this CatalogType.
		/// </summary>
		short CatalogTypeID { get; set; }
	
		/// <summary>
		/// The Name for this CatalogType.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this CatalogType.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this CatalogType.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this CatalogType.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The Catalogs for this CatalogType.
		/// </summary>
		IEnumerable<ICatalog> Catalogs { get; }
	
		/// <summary>
		/// Adds an <see cref="ICatalog"/> to the Catalogs collection.
		/// </summary>
		/// <param name="item">The <see cref="ICatalog"/> to add.</param>
		void AddCatalog(ICatalog item);
	
		/// <summary>
		/// Removes an <see cref="ICatalog"/> from the Catalogs collection.
		/// </summary>
		/// <param name="item">The <see cref="ICatalog"/> to remove.</param>
		void RemoveCatalog(ICatalog item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(ICatalogType))]
		internal abstract class CatalogTypeContracts : ICatalogType
		{
		    #region Primitive properties
		
			short ICatalogType.CatalogTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ICatalogType.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ICatalogType.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ICatalogType.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool ICatalogType.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<ICatalog> ICatalogType.Catalogs
			{
				get { throw new NotImplementedException(); }
			}
		
			void ICatalogType.AddCatalog(ICatalog item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void ICatalogType.RemoveCatalog(ICatalog item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
