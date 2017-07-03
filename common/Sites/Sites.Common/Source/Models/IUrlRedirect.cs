using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Sites.Common.Models
{
	/// <summary>
	/// Common interface for UrlRedirect.
	/// </summary>
	[ContractClass(typeof(Contracts.UrlRedirectContracts))]
	public interface IUrlRedirect
	{
	    #region Primitive properties
	
		/// <summary>
		/// The UrlRedirectID for this UrlRedirect.
		/// </summary>
		int UrlRedirectID { get; set; }
	
		/// <summary>
		/// The SiteTypeID for this UrlRedirect.
		/// </summary>
		short SiteTypeID { get; set; }
	
		/// <summary>
		/// The Url for this UrlRedirect.
		/// </summary>
		string Url { get; set; }
	
		/// <summary>
		/// The TargetUrl for this UrlRedirect.
		/// </summary>
		string TargetUrl { get; set; }
	
		/// <summary>
		/// The IsPermanent for this UrlRedirect.
		/// </summary>
		bool IsPermanent { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IUrlRedirect))]
		internal abstract class UrlRedirectContracts : IUrlRedirect
		{
		    #region Primitive properties
		
			int IUrlRedirect.UrlRedirectID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			short IUrlRedirect.SiteTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IUrlRedirect.Url
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IUrlRedirect.TargetUrl
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IUrlRedirect.IsPermanent
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
