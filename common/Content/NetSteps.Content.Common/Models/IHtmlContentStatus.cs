using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Content.Common.Models
{
	/// <summary>
	/// Common interface for HtmlContentStatus.
	/// </summary>
	[ContractClass(typeof(Contracts.HtmlContentStatusContracts))]
	public interface IHtmlContentStatus
	{
	    #region Primitive properties
	
		/// <summary>
		/// The HtmlContentStatusID for this HtmlContentStatus.
		/// </summary>
		int HtmlContentStatusID { get; set; }
	
		/// <summary>
		/// The Name for this HtmlContentStatus.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this HtmlContentStatus.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this HtmlContentStatus.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this HtmlContentStatus.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The HtmlContentHistories for this HtmlContentStatus.
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
		/// The HtmlContents for this HtmlContentStatus.
		/// </summary>
		IEnumerable<IHtmlContent> HtmlContents { get; }
	
		/// <summary>
		/// Adds an <see cref="IHtmlContent"/> to the HtmlContents collection.
		/// </summary>
		/// <param name="item">The <see cref="IHtmlContent"/> to add.</param>
		void AddHtmlContent(IHtmlContent item);
	
		/// <summary>
		/// Removes an <see cref="IHtmlContent"/> from the HtmlContents collection.
		/// </summary>
		/// <param name="item">The <see cref="IHtmlContent"/> to remove.</param>
		void RemoveHtmlContent(IHtmlContent item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IHtmlContentStatus))]
		internal abstract class HtmlContentStatusContracts : IHtmlContentStatus
		{
		    #region Primitive properties
		
			int IHtmlContentStatus.HtmlContentStatusID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IHtmlContentStatus.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IHtmlContentStatus.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IHtmlContentStatus.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IHtmlContentStatus.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IHtmlContentHistory> IHtmlContentStatus.HtmlContentHistories
			{
				get { throw new NotImplementedException(); }
			}
		
			void IHtmlContentStatus.AddHtmlContentHistory(IHtmlContentHistory item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IHtmlContentStatus.RemoveHtmlContentHistory(IHtmlContentHistory item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IHtmlContent> IHtmlContentStatus.HtmlContents
			{
				get { throw new NotImplementedException(); }
			}
		
			void IHtmlContentStatus.AddHtmlContent(IHtmlContent item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IHtmlContentStatus.RemoveHtmlContent(IHtmlContent item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
