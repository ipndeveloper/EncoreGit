using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Content.Common.Models
{
	/// <summary>
	/// Common interface for SiteWidget.
	/// </summary>
	[ContractClass(typeof(Contracts.SiteWidgetContracts))]
	public interface ISiteWidget
	{
	    #region Primitive properties
	
		/// <summary>
		/// The SiteWidgetID for this SiteWidget.
		/// </summary>
		int SiteWidgetID { get; set; }
	
		/// <summary>
		/// The SiteID for this SiteWidget.
		/// </summary>
		int SiteID { get; set; }
	
		/// <summary>
		/// The WidgetID for this SiteWidget.
		/// </summary>
		int WidgetID { get; set; }
	
		/// <summary>
		/// The DisplayColumn for this SiteWidget.
		/// </summary>
		int DisplayColumn { get; set; }
	
		/// <summary>
		/// The SortIndex for this SiteWidget.
		/// </summary>
		int SortIndex { get; set; }
	
		/// <summary>
		/// The IsOnTop for this SiteWidget.
		/// </summary>
		bool IsOnTop { get; set; }
	
		/// <summary>
		/// The Editable for this SiteWidget.
		/// </summary>
		bool Editable { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The Widget for this SiteWidget.
		/// </summary>
	    IWidget Widget { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(ISiteWidget))]
		internal abstract class SiteWidgetContracts : ISiteWidget
		{
		    #region Primitive properties
		
			int ISiteWidget.SiteWidgetID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int ISiteWidget.SiteID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int ISiteWidget.WidgetID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int ISiteWidget.DisplayColumn
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int ISiteWidget.SortIndex
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool ISiteWidget.IsOnTop
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool ISiteWidget.Editable
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IWidget ISiteWidget.Widget
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
