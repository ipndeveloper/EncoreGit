using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Content.Common.Models
{
	/// <summary>
	/// Common interface for Layout.
	/// </summary>
	[ContractClass(typeof(Contracts.LayoutContracts))]
	public interface ILayout
	{
	    #region Primitive properties
	
		/// <summary>
		/// The LayoutID for this Layout.
		/// </summary>
		int LayoutID { get; set; }
	
		/// <summary>
		/// The Name for this Layout.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The Description for this Layout.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The ViewName for this Layout.
		/// </summary>
		string ViewName { get; set; }
	
		/// <summary>
		/// The ThumbnailPath for this Layout.
		/// </summary>
		string ThumbnailPath { get; set; }
	
		/// <summary>
		/// The Active for this Layout.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The HtmlSections for this Layout.
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
		[ContractClassFor(typeof(ILayout))]
		internal abstract class LayoutContracts : ILayout
		{
		    #region Primitive properties
		
			int ILayout.LayoutID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ILayout.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ILayout.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ILayout.ViewName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ILayout.ThumbnailPath
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool ILayout.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IHtmlSection> ILayout.HtmlSections
			{
				get { throw new NotImplementedException(); }
			}
		
			void ILayout.AddHtmlSection(IHtmlSection item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void ILayout.RemoveHtmlSection(IHtmlSection item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
