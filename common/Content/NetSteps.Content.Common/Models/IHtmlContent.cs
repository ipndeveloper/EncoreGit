using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Content.Common.Models
{
	/// <summary>
	/// Common interface for HtmlContent.
	/// </summary>
	[ContractClass(typeof(Contracts.HtmlContentContracts))]
	public interface IHtmlContent
	{
	    #region Primitive properties
	
		/// <summary>
		/// The HtmlContentID for this HtmlContent.
		/// </summary>
		int HtmlContentID { get; set; }
	
		/// <summary>
		/// The HtmlContentStatusID for this HtmlContent.
		/// </summary>
		int HtmlContentStatusID { get; set; }
	
		/// <summary>
		/// The LanguageID for this HtmlContent.
		/// </summary>
		Nullable<int> LanguageID { get; set; }
	
		/// <summary>
		/// The Name for this HtmlContent.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The PublishDateUTC for this HtmlContent.
		/// </summary>
		Nullable<System.DateTime> PublishDateUTC { get; set; }
	
		/// <summary>
		/// The SortIndex for this HtmlContent.
		/// </summary>
		int SortIndex { get; set; }
	
		/// <summary>
		/// The CreatedByUserID for this HtmlContent.
		/// </summary>
		Nullable<int> CreatedByUserID { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The HtmlContentHistories for this HtmlContent.
		/// </summary>
		IEnumerable<IHtmlContentHistory> HtmlContentHistories { get; }
	
		/// <summary>
		/// Adds an <see cref="IHtmlContentHistory"/> to the HtmlContentHistories collection.
		/// </summary>
		/// <param name="item">The <see cref="IHtmlContentHistory"/> to add.</param>
		void AddHtmlContentHistory(IHtmlContentHistory item);
	
		/// <summary>
		/// Removes an <see cref="IHtmlContentHistory"/> from the HtmlContentHistories collection.
		/// </summary>
		/// <param name="item">The <see cref="IHtmlContentHistory"/> to remove.</param>
		void RemoveHtmlContentHistory(IHtmlContentHistory item);
	
		/// <summary>
		/// The HtmlContentWorkflows for this HtmlContent.
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
	
		/// <summary>
		/// The HtmlElements for this HtmlContent.
		/// </summary>
		IEnumerable<IHtmlElement> HtmlElements { get; }
	
		/// <summary>
		/// Adds an <see cref="IHtmlElement"/> to the HtmlElements collection.
		/// </summary>
		/// <param name="item">The <see cref="IHtmlElement"/> to add.</param>
		void AddHtmlElement(IHtmlElement item);
	
		/// <summary>
		/// Removes an <see cref="IHtmlElement"/> from the HtmlElements collection.
		/// </summary>
		/// <param name="item">The <see cref="IHtmlElement"/> to remove.</param>
		void RemoveHtmlElement(IHtmlElement item);
	
		/// <summary>
		/// The HtmlSectionChoices for this HtmlContent.
		/// </summary>
		IEnumerable<IHtmlSectionChoice> HtmlSectionChoices { get; }
	
		/// <summary>
		/// Adds an <see cref="IHtmlSectionChoice"/> to the HtmlSectionChoices collection.
		/// </summary>
		/// <param name="item">The <see cref="IHtmlSectionChoice"/> to add.</param>
		void AddHtmlSectionChoice(IHtmlSectionChoice item);
	
		/// <summary>
		/// Removes an <see cref="IHtmlSectionChoice"/> from the HtmlSectionChoices collection.
		/// </summary>
		/// <param name="item">The <see cref="IHtmlSectionChoice"/> to remove.</param>
		void RemoveHtmlSectionChoice(IHtmlSectionChoice item);
	
		/// <summary>
		/// The HtmlSectionContents for this HtmlContent.
		/// </summary>
		IEnumerable<IHtmlSectionContent> HtmlSectionContents { get; }
	
		/// <summary>
		/// Adds an <see cref="IHtmlSectionContent"/> to the HtmlSectionContents collection.
		/// </summary>
		/// <param name="item">The <see cref="IHtmlSectionContent"/> to add.</param>
		void AddHtmlSectionContent(IHtmlSectionContent item);
	
		/// <summary>
		/// Removes an <see cref="IHtmlSectionContent"/> from the HtmlSectionContents collection.
		/// </summary>
		/// <param name="item">The <see cref="IHtmlSectionContent"/> to remove.</param>
		void RemoveHtmlSectionContent(IHtmlSectionContent item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IHtmlContent))]
		internal abstract class HtmlContentContracts : IHtmlContent
		{
		    #region Primitive properties
		
			int IHtmlContent.HtmlContentID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IHtmlContent.HtmlContentStatusID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IHtmlContent.LanguageID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IHtmlContent.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> IHtmlContent.PublishDateUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IHtmlContent.SortIndex
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IHtmlContent.CreatedByUserID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IHtmlContentHistory> IHtmlContent.HtmlContentHistories
			{
				get { throw new NotImplementedException(); }
			}
		
			void IHtmlContent.AddHtmlContentHistory(IHtmlContentHistory item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IHtmlContent.RemoveHtmlContentHistory(IHtmlContentHistory item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IHtmlContentWorkflow> IHtmlContent.HtmlContentWorkflows
			{
				get { throw new NotImplementedException(); }
			}
		
			void IHtmlContent.AddHtmlContentWorkflow(IHtmlContentWorkflow item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IHtmlContent.RemoveHtmlContentWorkflow(IHtmlContentWorkflow item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IHtmlElement> IHtmlContent.HtmlElements
			{
				get { throw new NotImplementedException(); }
			}
		
			void IHtmlContent.AddHtmlElement(IHtmlElement item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IHtmlContent.RemoveHtmlElement(IHtmlElement item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IHtmlSectionChoice> IHtmlContent.HtmlSectionChoices
			{
				get { throw new NotImplementedException(); }
			}
		
			void IHtmlContent.AddHtmlSectionChoice(IHtmlSectionChoice item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IHtmlContent.RemoveHtmlSectionChoice(IHtmlSectionChoice item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IHtmlSectionContent> IHtmlContent.HtmlSectionContents
			{
				get { throw new NotImplementedException(); }
			}
		
			void IHtmlContent.AddHtmlSectionContent(IHtmlSectionContent item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IHtmlContent.RemoveHtmlSectionContent(IHtmlSectionContent item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
