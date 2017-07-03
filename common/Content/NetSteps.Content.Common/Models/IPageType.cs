using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Content.Common.Models
{
	/// <summary>
	/// Common interface for PageType.
	/// </summary>
	[ContractClass(typeof(Contracts.PageTypeContracts))]
	public interface IPageType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The PageTypeID for this PageType.
		/// </summary>
		short PageTypeID { get; set; }
	
		/// <summary>
		/// The Name for this PageType.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this PageType.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this PageType.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this PageType.
		/// </summary>
		bool Active { get; set; }
	
		/// <summary>
		/// The IsUserDefined for this PageType.
		/// </summary>
		bool IsUserDefined { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The Layouts for this PageType.
		/// </summary>
		IEnumerable<ILayout> Layouts { get; }
	
		/// <summary>
		/// Adds an <see cref="ILayout"/> to the Layouts collection.
		/// </summary>
		/// <param name="item">The <see cref="ILayout"/> to add.</param>
		void AddLayout(ILayout item);
	
		/// <summary>
		/// Removes an <see cref="ILayout"/> from the Layouts collection.
		/// </summary>
		/// <param name="item">The <see cref="ILayout"/> to remove.</param>
		void RemoveLayout(ILayout item);
	
		/// <summary>
		/// The Pages for this PageType.
		/// </summary>
		IEnumerable<IPage> Pages { get; }
	
		/// <summary>
		/// Adds an <see cref="IPage"/> to the Pages collection.
		/// </summary>
		/// <param name="item">The <see cref="IPage"/> to add.</param>
		void AddPage(IPage item);
	
		/// <summary>
		/// Removes an <see cref="IPage"/> from the Pages collection.
		/// </summary>
		/// <param name="item">The <see cref="IPage"/> to remove.</param>
		void RemovePage(IPage item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IPageType))]
		internal abstract class PageTypeContracts : IPageType
		{
		    #region Primitive properties
		
			short IPageType.PageTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IPageType.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IPageType.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IPageType.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IPageType.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IPageType.IsUserDefined
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<ILayout> IPageType.Layouts
			{
				get { throw new NotImplementedException(); }
			}
		
			void IPageType.AddLayout(ILayout item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IPageType.RemoveLayout(ILayout item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IPage> IPageType.Pages
			{
				get { throw new NotImplementedException(); }
			}
		
			void IPageType.AddPage(IPage item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IPageType.RemovePage(IPage item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
