using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Content.Common.Models
{
	/// <summary>
	/// Common interface for Navigation.
	/// </summary>
	[ContractClass(typeof(Contracts.NavigationContracts))]
	public interface INavigation
	{
	    #region Primitive properties
	
		/// <summary>
		/// The NavigationID for this Navigation.
		/// </summary>
		int NavigationID { get; set; }
	
		/// <summary>
		/// The BaseNavigationID for this Navigation.
		/// </summary>
		Nullable<int> BaseNavigationID { get; set; }
	
		/// <summary>
		/// The NavigationTypeID for this Navigation.
		/// </summary>
		Nullable<int> NavigationTypeID { get; set; }
	
		/// <summary>
		/// The SiteID for this Navigation.
		/// </summary>
		Nullable<int> SiteID { get; set; }
	
		/// <summary>
		/// The LinkUrl for this Navigation.
		/// </summary>
		string LinkUrl { get; set; }
	
		/// <summary>
		/// The PageID for this Navigation.
		/// </summary>
		Nullable<int> PageID { get; set; }
	
		/// <summary>
		/// The StartDateUTC for this Navigation.
		/// </summary>
		Nullable<System.DateTime> StartDateUTC { get; set; }
	
		/// <summary>
		/// The EndDateUTC for this Navigation.
		/// </summary>
		Nullable<System.DateTime> EndDateUTC { get; set; }
	
		/// <summary>
		/// The ParentID for this Navigation.
		/// </summary>
		Nullable<int> ParentID { get; set; }
	
		/// <summary>
		/// The SortIndex for this Navigation.
		/// </summary>
		int SortIndex { get; set; }
	
		/// <summary>
		/// The MinChildren for this Navigation.
		/// </summary>
		Nullable<int> MinChildren { get; set; }
	
		/// <summary>
		/// The MaxChildren for this Navigation.
		/// </summary>
		Nullable<int> MaxChildren { get; set; }
	
		/// <summary>
		/// The Active for this Navigation.
		/// </summary>
		bool Active { get; set; }
	
		/// <summary>
		/// The IsDeletable for this Navigation.
		/// </summary>
		Nullable<bool> IsDeletable { get; set; }
	
		/// <summary>
		/// The ModifiedByUserID for this Navigation.
		/// </summary>
		Nullable<int> ModifiedByUserID { get; set; }
	
		/// <summary>
		/// The IsDropDown for this Navigation.
		/// </summary>
		bool IsDropDown { get; set; }
	
		/// <summary>
		/// The IsSecondaryNavigation for this Navigation.
		/// </summary>
		bool IsSecondaryNavigation { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The BaseNavigation for this Navigation.
		/// </summary>
	    INavigation BaseNavigation { get; set; }
	
		/// <summary>
		/// The ParentNavigation for this Navigation.
		/// </summary>
	    INavigation ParentNavigation { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The BaseChildrenNavigations for this Navigation.
		/// </summary>
		IEnumerable<INavigation> BaseChildrenNavigations { get; }
	
		/// <summary>
		/// Adds an <see cref="INavigation"/> to the BaseChildrenNavigations collection.
		/// </summary>
		/// <param name="item">The <see cref="INavigation"/> to add.</param>
		void AddBaseChildrenNavigation(INavigation item);
	
		/// <summary>
		/// Removes an <see cref="INavigation"/> from the BaseChildrenNavigations collection.
		/// </summary>
		/// <param name="item">The <see cref="INavigation"/> to remove.</param>
		void RemoveBaseChildrenNavigation(INavigation item);
	
		/// <summary>
		/// The ChildNavigations for this Navigation.
		/// </summary>
		IEnumerable<INavigation> ChildNavigations { get; }
	
		/// <summary>
		/// Adds an <see cref="INavigation"/> to the ChildNavigations collection.
		/// </summary>
		/// <param name="item">The <see cref="INavigation"/> to add.</param>
		void AddChildNavigation(INavigation item);
	
		/// <summary>
		/// Removes an <see cref="INavigation"/> from the ChildNavigations collection.
		/// </summary>
		/// <param name="item">The <see cref="INavigation"/> to remove.</param>
		void RemoveChildNavigation(INavigation item);
	
		/// <summary>
		/// The Translations for this Navigation.
		/// </summary>
		IEnumerable<INavigationTranslation> Translations { get; }
	
		/// <summary>
		/// Adds an <see cref="INavigationTranslation"/> to the Translations collection.
		/// </summary>
		/// <param name="item">The <see cref="INavigationTranslation"/> to add.</param>
		void AddTranslation(INavigationTranslation item);
	
		/// <summary>
		/// Removes an <see cref="INavigationTranslation"/> from the Translations collection.
		/// </summary>
		/// <param name="item">The <see cref="INavigationTranslation"/> to remove.</param>
		void RemoveTranslation(INavigationTranslation item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(INavigation))]
		internal abstract class NavigationContracts : INavigation
		{
		    #region Primitive properties
		
			int INavigation.NavigationID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> INavigation.BaseNavigationID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> INavigation.NavigationTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> INavigation.SiteID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string INavigation.LinkUrl
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> INavigation.PageID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> INavigation.StartDateUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> INavigation.EndDateUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> INavigation.ParentID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int INavigation.SortIndex
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> INavigation.MinChildren
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> INavigation.MaxChildren
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool INavigation.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<bool> INavigation.IsDeletable
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> INavigation.ModifiedByUserID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool INavigation.IsDropDown
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool INavigation.IsSecondaryNavigation
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    INavigation INavigation.BaseNavigation
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    INavigation INavigation.ParentNavigation
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<INavigation> INavigation.BaseChildrenNavigations
			{
				get { throw new NotImplementedException(); }
			}
		
			void INavigation.AddBaseChildrenNavigation(INavigation item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void INavigation.RemoveBaseChildrenNavigation(INavigation item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<INavigation> INavigation.ChildNavigations
			{
				get { throw new NotImplementedException(); }
			}
		
			void INavigation.AddChildNavigation(INavigation item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void INavigation.RemoveChildNavigation(INavigation item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<INavigationTranslation> INavigation.Translations
			{
				get { throw new NotImplementedException(); }
			}
		
			void INavigation.AddTranslation(INavigationTranslation item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void INavigation.RemoveTranslation(INavigationTranslation item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
