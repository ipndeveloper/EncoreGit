using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Content.Common.Models
{
	/// <summary>
	/// Common interface for HtmlContentWorkflowType.
	/// </summary>
	[ContractClass(typeof(Contracts.HtmlContentWorkflowTypeContracts))]
	public interface IHtmlContentWorkflowType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The HtmlContentWorkflowTypeID for this HtmlContentWorkflowType.
		/// </summary>
		short HtmlContentWorkflowTypeID { get; set; }
	
		/// <summary>
		/// The Name for this HtmlContentWorkflowType.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this HtmlContentWorkflowType.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this HtmlContentWorkflowType.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this HtmlContentWorkflowType.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The HtmlContentWorkflows for this HtmlContentWorkflowType.
		/// </summary>
		IEnumerable<IHtmlContentWorkflow> HtmlContentWorkflows { get; }
	
		/// <summary>
		/// Adds an <see cref="IHtmlContentWorkflow"/> to the HtmlContentWorkflows collection.
		/// </summary>
		/// <param name="item">The <see cref="IHtmlContentWorkflow"/> to add.</param>
		void AddHtmlContentWorkflow(IHtmlContentWorkflow item);
	
		/// <summary>
		/// Removes an <see cref="IHtmlContentWorkflow"/> from the HtmlContentWorkflows collection.
		/// </summary>
		/// <param name="item">The <see cref="IHtmlContentWorkflow"/> to remove.</param>
		void RemoveHtmlContentWorkflow(IHtmlContentWorkflow item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IHtmlContentWorkflowType))]
		internal abstract class HtmlContentWorkflowTypeContracts : IHtmlContentWorkflowType
		{
		    #region Primitive properties
		
			short IHtmlContentWorkflowType.HtmlContentWorkflowTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IHtmlContentWorkflowType.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IHtmlContentWorkflowType.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IHtmlContentWorkflowType.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IHtmlContentWorkflowType.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IHtmlContentWorkflow> IHtmlContentWorkflowType.HtmlContentWorkflows
			{
				get { throw new NotImplementedException(); }
			}
		
			void IHtmlContentWorkflowType.AddHtmlContentWorkflow(IHtmlContentWorkflow item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IHtmlContentWorkflowType.RemoveHtmlContentWorkflow(IHtmlContentWorkflow item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
