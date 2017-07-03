using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Sites.Common.Models
{
	/// <summary>
	/// Common interface for SiteSettingItem.
	/// </summary>
	public interface ISiteSettingItem
	{
	    #region Primitive properties
	
		/// <summary>
		/// The site setting name.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The site setting value.
		/// </summary>
		string Value { get; set; }
	
		/// <summary>
		/// The SiteID of the site where this value came from.
		/// </summary>
		int? SiteID { get; set; }

	    #endregion
	}
}
