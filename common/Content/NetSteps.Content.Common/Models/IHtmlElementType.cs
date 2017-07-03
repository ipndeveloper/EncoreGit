using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Content.Common.Models
{
	/// <summary>
	/// Common interface for HtmlElementType.
	/// </summary>
	[ContractClass(typeof(Contracts.HtmlElementTypeContracts))]
	public interface IHtmlElementType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The HtmlElementTypeID for this HtmlElementType.
		/// </summary>
		byte HtmlElementTypeID { get; set; }
	
		/// <summary>
		/// The Name for this HtmlElementType.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The ContainerTagName for this HtmlElementType.
		/// </summary>
		string ContainerTagName { get; set; }
	
		/// <summary>
		/// The ContainerCssClass for this HtmlElementType.
		/// </summary>
		string ContainerCssClass { get; set; }
	
		/// <summary>
		/// The Active for this HtmlElementType.
		/// </summary>
		bool Active { get; set; }
	
		/// <summary>
		/// The TermName for this HtmlElementType.
		/// </summary>
		string TermName { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The HtmlElements for this HtmlElementType.
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

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IHtmlElementType))]
		internal abstract class HtmlElementTypeContracts : IHtmlElementType
		{
		    #region Primitive properties
		
			byte IHtmlElementType.HtmlElementTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IHtmlElementType.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IHtmlElementType.ContainerTagName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IHtmlElementType.ContainerCssClass
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IHtmlElementType.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IHtmlElementType.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IHtmlElement> IHtmlElementType.HtmlElements
			{
				get { throw new NotImplementedException(); }
			}
		
			void IHtmlElementType.AddHtmlElement(IHtmlElement item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IHtmlElementType.RemoveHtmlElement(IHtmlElement item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
