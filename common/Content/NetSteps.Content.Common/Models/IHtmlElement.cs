using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Content.Common.Models
{
	/// <summary>
	/// Common interface for HtmlElement.
	/// </summary>
	[ContractClass(typeof(Contracts.HtmlElementContracts))]
	public interface IHtmlElement
	{
	    #region Primitive properties
	
		/// <summary>
		/// The HtmlElementID for this HtmlElement.
		/// </summary>
		long HtmlElementID { get; set; }
	
		/// <summary>
		/// The HtmlElementTypeID for this HtmlElement.
		/// </summary>
		byte HtmlElementTypeID { get; set; }
	
		/// <summary>
		/// The HtmlContentID for this HtmlElement.
		/// </summary>
		int HtmlContentID { get; set; }
	
		/// <summary>
		/// The Contents for this HtmlElement.
		/// </summary>
		string Contents { get; set; }
	
		/// <summary>
		/// The Active for this HtmlElement.
		/// </summary>
		bool Active { get; set; }
	
		/// <summary>
		/// The SortIndex for this HtmlElement.
		/// </summary>
		byte SortIndex { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The HtmlContent for this HtmlElement.
		/// </summary>
	    IHtmlContent HtmlContent { get; set; }
	
		/// <summary>
		/// The HtmlElementType for this HtmlElement.
		/// </summary>
	    IHtmlElementType HtmlElementType { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IHtmlElement))]
		internal abstract class HtmlElementContracts : IHtmlElement
		{
		    #region Primitive properties
		
			long IHtmlElement.HtmlElementID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			byte IHtmlElement.HtmlElementTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IHtmlElement.HtmlContentID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IHtmlElement.Contents
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IHtmlElement.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			byte IHtmlElement.SortIndex
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IHtmlContent IHtmlElement.HtmlContent
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IHtmlElementType IHtmlElement.HtmlElementType
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
