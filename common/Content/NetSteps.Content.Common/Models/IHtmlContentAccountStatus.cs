using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Content.Common.Models
{
	/// <summary>
	/// Common interface for HtmlContentAccountStatus.
	/// </summary>
	public interface IHtmlContentAccountStatus
	{
	    #region Primitive properties
	
		/// <summary>
		/// The Consultant for this HtmlContentAccountStatus.
		/// </summary>
		string Consultant { get; set; }
	
		/// <summary>
		/// The SectionName for this HtmlContentAccountStatus.
		/// </summary>
		string SectionName { get; set; }
	
		/// <summary>
		/// The Uploaded for this HtmlContentAccountStatus.
		/// </summary>
		string Uploaded { get; set; }
	
		/// <summary>
		/// The Status for this HtmlContentAccountStatus.
		/// </summary>
		string Status { get; set; }
	
		/// <summary>
		/// The Comments for this HtmlContentAccountStatus.
		/// </summary>
		string Comments { get; set; }
	
		/// <summary>
		/// The ID for this HtmlContentAccountStatus.
		/// </summary>
		int ID { get; set; }
	
		/// <summary>
		/// The SiteID for this HtmlContentAccountStatus.
		/// </summary>
		Nullable<int> SiteID { get; set; }

	    #endregion
	}
}
