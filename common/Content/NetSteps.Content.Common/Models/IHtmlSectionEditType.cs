using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Content.Common.Models
{
	/// <summary>
	/// Common interface for HtmlSectionEditType.
	/// </summary>
	[ContractClass(typeof(Contracts.HtmlSectionEditTypeContracts))]
	public interface IHtmlSectionEditType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The HtmlSectionEditTypeID for this HtmlSectionEditType.
		/// </summary>
		short HtmlSectionEditTypeID { get; set; }
	
		/// <summary>
		/// The Name for this HtmlSectionEditType.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this HtmlSectionEditType.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this HtmlSectionEditType.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this HtmlSectionEditType.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The HtmlSections for this HtmlSectionEditType.
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
		[ContractClassFor(typeof(IHtmlSectionEditType))]
		internal abstract class HtmlSectionEditTypeContracts : IHtmlSectionEditType
		{
		    #region Primitive properties
		
			short IHtmlSectionEditType.HtmlSectionEditTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IHtmlSectionEditType.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IHtmlSectionEditType.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IHtmlSectionEditType.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IHtmlSectionEditType.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IHtmlSection> IHtmlSectionEditType.HtmlSections
			{
				get { throw new NotImplementedException(); }
			}
		
			void IHtmlSectionEditType.AddHtmlSection(IHtmlSection item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IHtmlSectionEditType.RemoveHtmlSection(IHtmlSection item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
