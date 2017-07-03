using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Content.Common.Models
{
	/// <summary>
	/// Common interface for HtmlSectionChoice.
	/// </summary>
	[ContractClass(typeof(Contracts.HtmlSectionChoiceContracts))]
	public interface IHtmlSectionChoice
	{
	    #region Primitive properties
	
		/// <summary>
		/// The HtmlSectionChoiceID for this HtmlSectionChoice.
		/// </summary>
		int HtmlSectionChoiceID { get; set; }
	
		/// <summary>
		/// The HtmlSectionID for this HtmlSectionChoice.
		/// </summary>
		int HtmlSectionID { get; set; }
	
		/// <summary>
		/// The HtmlContentID for this HtmlSectionChoice.
		/// </summary>
		int HtmlContentID { get; set; }
	
		/// <summary>
		/// The SortIndex for this HtmlSectionChoice.
		/// </summary>
		int SortIndex { get; set; }
	
		/// <summary>
		/// The SiteID for this HtmlSectionChoice.
		/// </summary>
		int SiteID { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IHtmlSectionChoice))]
		internal abstract class HtmlSectionChoiceContracts : IHtmlSectionChoice
		{
		    #region Primitive properties
		
			int IHtmlSectionChoice.HtmlSectionChoiceID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IHtmlSectionChoice.HtmlSectionID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IHtmlSectionChoice.HtmlContentID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IHtmlSectionChoice.SortIndex
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IHtmlSectionChoice.SiteID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
