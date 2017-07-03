using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Content.Common.Models
{
	/// <summary>
	/// Common interface for HtmlContentHistory.
	/// </summary>
	[ContractClass(typeof(Contracts.HtmlContentHistoryContracts))]
	public interface IHtmlContentHistory
	{
	    #region Primitive properties
	
		/// <summary>
		/// The HtmlContentHistoryID for this HtmlContentHistory.
		/// </summary>
		int HtmlContentHistoryID { get; set; }
	
		/// <summary>
		/// The HtmlContentID for this HtmlContentHistory.
		/// </summary>
		int HtmlContentID { get; set; }
	
		/// <summary>
		/// The HtmlContentStatusID for this HtmlContentHistory.
		/// </summary>
		int HtmlContentStatusID { get; set; }
	
		/// <summary>
		/// The UserID for this HtmlContentHistory.
		/// </summary>
		Nullable<int> UserID { get; set; }
	
		/// <summary>
		/// The HistoryDateUTC for this HtmlContentHistory.
		/// </summary>
		Nullable<System.DateTime> HistoryDateUTC { get; set; }
	
		/// <summary>
		/// The Comments for this HtmlContentHistory.
		/// </summary>
		string Comments { get; set; }
	
		/// <summary>
		/// The MessageSeen for this HtmlContentHistory.
		/// </summary>
		bool MessageSeen { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IHtmlContentHistory))]
		internal abstract class HtmlContentHistoryContracts : IHtmlContentHistory
		{
		    #region Primitive properties
		
			int IHtmlContentHistory.HtmlContentHistoryID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IHtmlContentHistory.HtmlContentID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IHtmlContentHistory.HtmlContentStatusID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IHtmlContentHistory.UserID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> IHtmlContentHistory.HistoryDateUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IHtmlContentHistory.Comments
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IHtmlContentHistory.MessageSeen
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
