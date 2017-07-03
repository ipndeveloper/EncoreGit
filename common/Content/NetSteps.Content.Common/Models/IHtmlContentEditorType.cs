using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Content.Common.Models
{
	/// <summary>
	/// Common interface for HtmlContentEditorType.
	/// </summary>
	[ContractClass(typeof(Contracts.HtmlContentEditorTypeContracts))]
	public interface IHtmlContentEditorType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The HtmlContentEditorTypeID for this HtmlContentEditorType.
		/// </summary>
		short HtmlContentEditorTypeID { get; set; }
	
		/// <summary>
		/// The Name for this HtmlContentEditorType.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this HtmlContentEditorType.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this HtmlContentEditorType.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this HtmlContentEditorType.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The HtmlSections for this HtmlContentEditorType.
		/// </summary>
		IEnumerable<IHtmlSection> HtmlSections { get; }
	
		/// <summary>
		/// Adds an <see cref="IHtmlSection"/> to the HtmlSections collection.
		/// </summary>
		/// <param name="item">The <see cref="IHtmlSection"/> to add.</param>
		void AddHtmlSection(IHtmlSection item);
	
		/// <summary>
		/// Removes an <see cref="IHtmlSection"/> from the HtmlSections collection.
		/// </summary>
		/// <param name="item">The <see cref="IHtmlSection"/> to remove.</param>
		void RemoveHtmlSection(IHtmlSection item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IHtmlContentEditorType))]
		internal abstract class HtmlContentEditorTypeContracts : IHtmlContentEditorType
		{
		    #region Primitive properties
		
			short IHtmlContentEditorType.HtmlContentEditorTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IHtmlContentEditorType.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IHtmlContentEditorType.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IHtmlContentEditorType.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IHtmlContentEditorType.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IHtmlSection> IHtmlContentEditorType.HtmlSections
			{
				get { throw new NotImplementedException(); }
			}
		
			void IHtmlContentEditorType.AddHtmlSection(IHtmlSection item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IHtmlContentEditorType.RemoveHtmlSection(IHtmlSection item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
