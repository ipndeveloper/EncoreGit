using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Content.Common.Models
{
	/// <summary>
	/// Common interface for ProxyLink.
	/// </summary>
	[ContractClass(typeof(Contracts.ProxyLinkContracts))]
	public interface IProxyLink
	{
	    #region Primitive properties
	
		/// <summary>
		/// The ProxyLinkID for this ProxyLink.
		/// </summary>
		int ProxyLinkID { get; set; }
	
		/// <summary>
		/// The Name for this ProxyLink.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The URL for this ProxyLink.
		/// </summary>
		string URL { get; set; }
	
		/// <summary>
		/// The SortIndex for this ProxyLink.
		/// </summary>
		int SortIndex { get; set; }
	
		/// <summary>
		/// The Active for this ProxyLink.
		/// </summary>
		bool Active { get; set; }
	
		/// <summary>
		/// The TermName for this ProxyLink.
		/// </summary>
		string TermName { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IProxyLink))]
		internal abstract class ProxyLinkContracts : IProxyLink
		{
		    #region Primitive properties
		
			int IProxyLink.ProxyLinkID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IProxyLink.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IProxyLink.URL
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IProxyLink.SortIndex
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IProxyLink.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IProxyLink.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
