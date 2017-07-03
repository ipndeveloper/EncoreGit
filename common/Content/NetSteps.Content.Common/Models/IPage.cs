using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Content.Common.Models
{
	/// <summary>
	/// Common interface for Page.
	/// </summary>
	[ContractClass(typeof(Contracts.PageContracts))]
	public interface IPage
	{
	    #region Primitive properties
	
		/// <summary>
		/// The PageID for this Page.
		/// </summary>
		int PageID { get; set; }
	
		/// <summary>
		/// The ParentID for this Page.
		/// </summary>
		Nullable<int> ParentID { get; set; }
	
		/// <summary>
		/// The SiteID for this Page.
		/// </summary>
		Nullable<int> SiteID { get; set; }
	
		/// <summary>
		/// The Name for this Page.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The Url for this Page.
		/// </summary>
		string Url { get; set; }
	
		/// <summary>
		/// The RequiresAuthentication for this Page.
		/// </summary>
		Nullable<bool> RequiresAuthentication { get; set; }
	
		/// <summary>
		/// The Active for this Page.
		/// </summary>
		bool Active { get; set; }
	
		/// <summary>
		/// The Editable for this Page.
		/// </summary>
		bool Editable { get; set; }
	
		/// <summary>
		/// The UseSsl for this Page.
		/// </summary>
		Nullable<bool> UseSsl { get; set; }
	
		/// <summary>
		/// The IsStartPage for this Page.
		/// </summary>
		bool IsStartPage { get; set; }
	
		/// <summary>
		/// The ModifiedByUserID for this Page.
		/// </summary>
		Nullable<int> ModifiedByUserID { get; set; }
	
		/// <summary>
		/// The LayoutID for this Page.
		/// </summary>
		int LayoutID { get; set; }
	
		/// <summary>
		/// The PageTypeID for this Page.
		/// </summary>
		short PageTypeID { get; set; }
	
		/// <summary>
		/// The ExternalUrl for this Page.
		/// </summary>
		string ExternalUrl { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The HtmlSections for this Page.
		/// </summary>
		IEnumerable<IHtmlSection> HtmlSections { get; }
	
		/// <summary>
		/// Adds an <see cref="IHtmlSection"/> to the HtmlSections collection.
		/// </summary>
		/// <param name="item">The <see cref="IHtmlSection"/> to add.</param>
		void AddHtmlSection(IHtmlSection item);
	
		/// <summary>
		/// Removes an <see cref="IHtmlSection"/> from the HtmlSections collection.
		/// </summary>
		/// <param name="item">The <see cref="IHtmlSection"/> to remove.</param>
		void RemoveHtmlSection(IHtmlSection item);
	
		/// <summary>
		/// The Navigations for this Page.
		/// </summary>
		IEnumerable<INavigation> Navigations { get; }
	
		/// <summary>
		/// Adds an <see cref="INavigation"/> to the Navigations collection.
		/// </summary>
		/// <param name="item">The <see cref="INavigation"/> to add.</param>
		void AddNavigation(INavigation item);
	
		/// <summary>
		/// Removes an <see cref="INavigation"/> from the Navigations collection.
		/// </summary>
		/// <param name="item">The <see cref="INavigation"/> to remove.</param>
		void RemoveNavigation(INavigation item);
	
		/// <summary>
		/// The Translations for this Page.
		/// </summary>
		IEnumerable<IPageTranslation> Translations { get; }
	
		/// <summary>
		/// Adds an <see cref="IPageTranslation"/> to the Translations collection.
		/// </summary>
		/// <param name="item">The <see cref="IPageTranslation"/> to add.</param>
		void AddTranslation(IPageTranslation item);
	
		/// <summary>
		/// Removes an <see cref="IPageTranslation"/> from the Translations collection.
		/// </summary>
		/// <param name="item">The <see cref="IPageTranslation"/> to remove.</param>
		void RemoveTranslation(IPageTranslation item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IPage))]
		internal abstract class PageContracts : IPage
		{
		    #region Primitive properties
		
			int IPage.PageID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IPage.ParentID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IPage.SiteID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IPage.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IPage.Url
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<bool> IPage.RequiresAuthentication
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IPage.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IPage.Editable
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<bool> IPage.UseSsl
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IPage.IsStartPage
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IPage.ModifiedByUserID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IPage.LayoutID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			short IPage.PageTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IPage.ExternalUrl
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IHtmlSection> IPage.HtmlSections
			{
				get { throw new NotImplementedException(); }
			}
		
			void IPage.AddHtmlSection(IHtmlSection item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IPage.RemoveHtmlSection(IHtmlSection item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<INavigation> IPage.Navigations
			{
				get { throw new NotImplementedException(); }
			}
		
			void IPage.AddNavigation(INavigation item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IPage.RemoveNavigation(INavigation item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IPageTranslation> IPage.Translations
			{
				get { throw new NotImplementedException(); }
			}
		
			void IPage.AddTranslation(IPageTranslation item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IPage.RemoveTranslation(IPageTranslation item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
