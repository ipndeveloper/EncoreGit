using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Content.Common.Models
{
	/// <summary>
	/// Common interface for PageTranslation.
	/// </summary>
	[ContractClass(typeof(Contracts.PageTranslationContracts))]
	public interface IPageTranslation
	{
	    #region Primitive properties
	
		/// <summary>
		/// The PageTranslationID for this PageTranslation.
		/// </summary>
		int PageTranslationID { get; set; }
	
		/// <summary>
		/// The PageID for this PageTranslation.
		/// </summary>
		int PageID { get; set; }
	
		/// <summary>
		/// The LanguageID for this PageTranslation.
		/// </summary>
		int LanguageID { get; set; }
	
		/// <summary>
		/// The Title for this PageTranslation.
		/// </summary>
		string Title { get; set; }
	
		/// <summary>
		/// The Description for this PageTranslation.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Keywords for this PageTranslation.
		/// </summary>
		string Keywords { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The Page for this PageTranslation.
		/// </summary>
	    IPage Page { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IPageTranslation))]
		internal abstract class PageTranslationContracts : IPageTranslation
		{
		    #region Primitive properties
		
			int IPageTranslation.PageTranslationID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IPageTranslation.PageID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IPageTranslation.LanguageID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IPageTranslation.Title
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IPageTranslation.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IPageTranslation.Keywords
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IPage IPageTranslation.Page
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
