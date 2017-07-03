using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Products.Common.Models
{
	/// <summary>
	/// Common interface for StoreFront.
	/// </summary>
	[ContractClass(typeof(Contracts.StoreFrontContracts))]
	public interface IStoreFront
	{
	    #region Primitive properties
	
		/// <summary>
		/// The StoreFrontID for this StoreFront.
		/// </summary>
		int StoreFrontID { get; set; }
	
		/// <summary>
		/// The Name for this StoreFront.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this StoreFront.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this StoreFront.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this StoreFront.
		/// </summary>
		bool Active { get; set; }
	
		/// <summary>
		/// The DateLastModifiedUTC for this StoreFront.
		/// </summary>
		Nullable<System.DateTime> DateLastModifiedUTC { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The AccountPriceTypes for this StoreFront.
		/// </summary>
		IEnumerable<IAccountPriceType> AccountPriceTypes { get; }
	
		/// <summary>
		/// Adds an <see cref="IAccountPriceType"/> to the AccountPriceTypes collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccountPriceType"/> to add.</param>
		void AddAccountPriceType(IAccountPriceType item);
	
		/// <summary>
		/// Removes an <see cref="IAccountPriceType"/> from the AccountPriceTypes collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccountPriceType"/> to remove.</param>
		void RemoveAccountPriceType(IAccountPriceType item);
	
		/// <summary>
		/// The Catalogs for this StoreFront.
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
	
		/// <summary>
		/// The MarketStoreFronts for this StoreFront.
		/// </summary>
		IEnumerable<IMarketStoreFront> MarketStoreFronts { get; }
	
		/// <summary>
		/// Adds an <see cref="IMarketStoreFront"/> to the MarketStoreFronts collection.
		/// </summary>
		/// <param name="item">The <see cref="IMarketStoreFront"/> to add.</param>
		void AddMarketStoreFront(IMarketStoreFront item);
	
		/// <summary>
		/// Removes an <see cref="IMarketStoreFront"/> from the MarketStoreFronts collection.
		/// </summary>
		/// <param name="item">The <see cref="IMarketStoreFront"/> to remove.</param>
		void RemoveMarketStoreFront(IMarketStoreFront item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IStoreFront))]
		internal abstract class StoreFrontContracts : IStoreFront
		{
		    #region Primitive properties
		
			int IStoreFront.StoreFrontID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IStoreFront.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IStoreFront.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IStoreFront.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IStoreFront.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> IStoreFront.DateLastModifiedUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IAccountPriceType> IStoreFront.AccountPriceTypes
			{
				get { throw new NotImplementedException(); }
			}
		
			void IStoreFront.AddAccountPriceType(IAccountPriceType item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IStoreFront.RemoveAccountPriceType(IAccountPriceType item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<ICatalog> IStoreFront.Catalogs
			{
				get { throw new NotImplementedException(); }
			}
		
			void IStoreFront.AddCatalog(ICatalog item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IStoreFront.RemoveCatalog(ICatalog item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IMarketStoreFront> IStoreFront.MarketStoreFronts
			{
				get { throw new NotImplementedException(); }
			}
		
			void IStoreFront.AddMarketStoreFront(IMarketStoreFront item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IStoreFront.RemoveMarketStoreFront(IMarketStoreFront item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
