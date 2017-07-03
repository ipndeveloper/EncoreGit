using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Content.Common.Models
{
	/// <summary>
	/// Common interface for UserSiteWidget.
	/// </summary>
	[ContractClass(typeof(Contracts.UserSiteWidgetContracts))]
	public interface IUserSiteWidget
	{
	    #region Primitive properties
	
		/// <summary>
		/// The UserSiteWidgetID for this UserSiteWidget.
		/// </summary>
		int UserSiteWidgetID { get; set; }
	
		/// <summary>
		/// The UserID for this UserSiteWidget.
		/// </summary>
		int UserID { get; set; }
	
		/// <summary>
		/// The SiteID for this UserSiteWidget.
		/// </summary>
		int SiteID { get; set; }
	
		/// <summary>
		/// The WidgetID for this UserSiteWidget.
		/// </summary>
		int WidgetID { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IUserSiteWidget))]
		internal abstract class UserSiteWidgetContracts : IUserSiteWidget
		{
		    #region Primitive properties
		
			int IUserSiteWidget.UserSiteWidgetID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IUserSiteWidget.UserID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IUserSiteWidget.SiteID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IUserSiteWidget.WidgetID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
