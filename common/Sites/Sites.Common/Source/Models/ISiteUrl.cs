using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Sites.Common.Models
{
	/// <summary>
	/// Common interface for SiteUrl.
	/// </summary>
	[ContractClass(typeof(Contracts.SiteUrlContracts))]
	public interface ISiteUrl
	{
	    #region Primitive properties
	
		/// <summary>
		/// The SiteUrlID for this SiteUrl.
		/// </summary>
		int SiteUrlID { get; set; }
	
		/// <summary>
		/// The SiteID for this SiteUrl.
		/// </summary>
		Nullable<int> SiteID { get; set; }
	
		/// <summary>
		/// The Url for this SiteUrl.
		/// </summary>
		string Url { get; set; }
	
		/// <summary>
		/// The ExpirationDateUTC for this SiteUrl.
		/// </summary>
		Nullable<System.DateTime> ExpirationDateUTC { get; set; }
	
		/// <summary>
		/// The LanguageID for this SiteUrl.
		/// </summary>
		Nullable<int> LanguageID { get; set; }
	
		/// <summary>
		/// The IsPrimaryUrl for this SiteUrl.
		/// </summary>
		bool IsPrimaryUrl { get; set; }
	
		/// <summary>
		/// The ModifiedByUserID for this SiteUrl.
		/// </summary>
		Nullable<int> ModifiedByUserID { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(ISiteUrl))]
		internal abstract class SiteUrlContracts : ISiteUrl
		{
		    #region Primitive properties
		
			int ISiteUrl.SiteUrlID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> ISiteUrl.SiteID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ISiteUrl.Url
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> ISiteUrl.ExpirationDateUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> ISiteUrl.LanguageID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool ISiteUrl.IsPrimaryUrl
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> ISiteUrl.ModifiedByUserID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
