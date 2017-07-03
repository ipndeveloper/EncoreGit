using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Content.Common.Models
{
	/// <summary>
	/// Common interface for HtmlSectionContent.
	/// </summary>
	[ContractClass(typeof(Contracts.HtmlSectionContentContracts))]
	public interface IHtmlSectionContent
	{
	    #region Primitive properties
	
		/// <summary>
		/// The HtmlSectionContentID for this HtmlSectionContent.
		/// </summary>
		int HtmlSectionContentID { get; set; }
	
		/// <summary>
		/// The HtmlSectionID for this HtmlSectionContent.
		/// </summary>
		int HtmlSectionID { get; set; }
	
		/// <summary>
		/// The HtmlContentID for this HtmlSectionContent.
		/// </summary>
		int HtmlContentID { get; set; }
	
		/// <summary>
		/// The SiteID for this HtmlSectionContent.
		/// </summary>
		Nullable<int> SiteID { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IHtmlSectionContent))]
		internal abstract class HtmlSectionContentContracts : IHtmlSectionContent
		{
		    #region Primitive properties
		
			int IHtmlSectionContent.HtmlSectionContentID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IHtmlSectionContent.HtmlSectionID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IHtmlSectionContent.HtmlContentID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IHtmlSectionContent.SiteID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
