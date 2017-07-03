using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Content.Common.Models
{
	/// <summary>
	/// Common interface for Widget.
	/// </summary>
	[ContractClass(typeof(Contracts.WidgetContracts))]
	public interface IWidget
	{
	    #region Primitive properties
	
		/// <summary>
		/// The WidgetID for this Widget.
		/// </summary>
		int WidgetID { get; set; }
	
		/// <summary>
		/// The Name for this Widget.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this Widget.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this Widget.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The ViewName for this Widget.
		/// </summary>
		string ViewName { get; set; }
	
		/// <summary>
		/// The Path for this Widget.
		/// </summary>
		string Path { get; set; }
	
		/// <summary>
		/// The Active for this Widget.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The SiteWidgets for this Widget.
		/// </summary>
		IEnumerable<ISiteWidget> SiteWidgets { get; }
	
		/// <summary>
		/// Adds an <see cref="ISiteWidget"/> to the SiteWidgets collection.
		/// </summary>
		/// <param name="item">The <see cref="ISiteWidget"/> to add.</param>
		void AddSiteWidget(ISiteWidget item);
	
		/// <summary>
		/// Removes an <see cref="ISiteWidget"/> from the SiteWidgets collection.
		/// </summary>
		/// <param name="item">The <see cref="ISiteWidget"/> to remove.</param>
		void RemoveSiteWidget(ISiteWidget item);
	
		/// <summary>
		/// The UserSiteWidgets for this Widget.
		/// </summary>
		IEnumerable<IUserSiteWidget> UserSiteWidgets { get; }
	
		/// <summary>
		/// Adds an <see cref="IUserSiteWidget"/> to the UserSiteWidgets collection.
		/// </summary>
		/// <param name="item">The <see cref="IUserSiteWidget"/> to add.</param>
		void AddUserSiteWidget(IUserSiteWidget item);
	
		/// <summary>
		/// Removes an <see cref="IUserSiteWidget"/> from the UserSiteWidgets collection.
		/// </summary>
		/// <param name="item">The <see cref="IUserSiteWidget"/> to remove.</param>
		void RemoveUserSiteWidget(IUserSiteWidget item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IWidget))]
		internal abstract class WidgetContracts : IWidget
		{
		    #region Primitive properties
		
			int IWidget.WidgetID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IWidget.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IWidget.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IWidget.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IWidget.ViewName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IWidget.Path
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IWidget.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<ISiteWidget> IWidget.SiteWidgets
			{
				get { throw new NotImplementedException(); }
			}
		
			void IWidget.AddSiteWidget(ISiteWidget item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IWidget.RemoveSiteWidget(ISiteWidget item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IUserSiteWidget> IWidget.UserSiteWidgets
			{
				get { throw new NotImplementedException(); }
			}
		
			void IWidget.AddUserSiteWidget(IUserSiteWidget item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IWidget.RemoveUserSiteWidget(IUserSiteWidget item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
