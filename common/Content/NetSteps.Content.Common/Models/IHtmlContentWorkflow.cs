using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Content.Common.Models
{
	/// <summary>
	/// Common interface for HtmlContentWorkflow.
	/// </summary>
	[ContractClass(typeof(Contracts.HtmlContentWorkflowContracts))]
	public interface IHtmlContentWorkflow
	{
	    #region Primitive properties
	
		/// <summary>
		/// The HtmlContentWorkflowID for this HtmlContentWorkflow.
		/// </summary>
		int HtmlContentWorkflowID { get; set; }
	
		/// <summary>
		/// The HtmlContentID for this HtmlContentWorkflow.
		/// </summary>
		int HtmlContentID { get; set; }
	
		/// <summary>
		/// The UserID for this HtmlContentWorkflow.
		/// </summary>
		int UserID { get; set; }
	
		/// <summary>
		/// The HtmlContentWorkflowTypeID for this HtmlContentWorkflow.
		/// </summary>
		Nullable<short> HtmlContentWorkflowTypeID { get; set; }
	
		/// <summary>
		/// The WorkflowDateUTC for this HtmlContentWorkflow.
		/// </summary>
		System.DateTime WorkflowDateUTC { get; set; }
	
		/// <summary>
		/// The Comments for this HtmlContentWorkflow.
		/// </summary>
		string Comments { get; set; }
	
		/// <summary>
		/// The Title for this HtmlContentWorkflow.
		/// </summary>
		string Title { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The HtmlContent for this HtmlContentWorkflow.
		/// </summary>
	    IHtmlContent HtmlContent { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IHtmlContentWorkflow))]
		internal abstract class HtmlContentWorkflowContracts : IHtmlContentWorkflow
		{
		    #region Primitive properties
		
			int IHtmlContentWorkflow.HtmlContentWorkflowID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IHtmlContentWorkflow.HtmlContentID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IHtmlContentWorkflow.UserID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<short> IHtmlContentWorkflow.HtmlContentWorkflowTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			System.DateTime IHtmlContentWorkflow.WorkflowDateUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IHtmlContentWorkflow.Comments
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IHtmlContentWorkflow.Title
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IHtmlContent IHtmlContentWorkflow.HtmlContent
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
