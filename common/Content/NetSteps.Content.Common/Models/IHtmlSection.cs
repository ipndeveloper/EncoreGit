using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Content.Common.Models
{
	/// <summary>
	/// Common interface for HtmlSection.
	/// </summary>
	[ContractClass(typeof(Contracts.HtmlSectionContracts))]
	public interface IHtmlSection
	{
	    #region Primitive properties
	
		/// <summary>
		/// The HtmlSectionID for this HtmlSection.
		/// </summary>
		int HtmlSectionID { get; set; }
	
		/// <summary>
		/// The HtmlSectionEditTypeID for this HtmlSection.
		/// </summary>
		short HtmlSectionEditTypeID { get; set; }
	
		/// <summary>
		/// The SectionName for this HtmlSection.
		/// </summary>
		string SectionName { get; set; }
	
		/// <summary>
		/// The RequiresApproval for this HtmlSection.
		/// </summary>
		bool RequiresApproval { get; set; }
	
		/// <summary>
		/// The HtmlContentEditorTypeID for this HtmlSection.
		/// </summary>
		short HtmlContentEditorTypeID { get; set; }
	
		/// <summary>
		/// The Width for this HtmlSection.
		/// </summary>
		Nullable<short> Width { get; set; }
	
		/// <summary>
		/// The Height for this HtmlSection.
		/// </summary>
		Nullable<short> Height { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The HtmlSectionChoices for this HtmlSection.
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
		/// The HtmlSectionContents for this HtmlSection.
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
		[ContractClassFor(typeof(IHtmlSection))]
		internal abstract class HtmlSectionContracts : IHtmlSection
		{
		    #region Primitive properties
		
			int IHtmlSection.HtmlSectionID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			short IHtmlSection.HtmlSectionEditTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IHtmlSection.SectionName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IHtmlSection.RequiresApproval
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			short IHtmlSection.HtmlContentEditorTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<short> IHtmlSection.Width
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<short> IHtmlSection.Height
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IHtmlSectionChoice> IHtmlSection.HtmlSectionChoices
			{
				get { throw new NotImplementedException(); }
			}
		
			void IHtmlSection.AddHtmlSectionChoice(IHtmlSectionChoice item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IHtmlSection.RemoveHtmlSectionChoice(IHtmlSectionChoice item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IHtmlSectionContent> IHtmlSection.HtmlSectionContents
			{
				get { throw new NotImplementedException(); }
			}
		
			void IHtmlSection.AddHtmlSectionContent(IHtmlSectionContent item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IHtmlSection.RemoveHtmlSectionContent(IHtmlSectionContent item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
