using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Content.Common.Models
{
	/// <summary>
	/// Common interface for HtmlInputType.
	/// </summary>
	[ContractClass(typeof(Contracts.HtmlInputTypeContracts))]
	public interface IHtmlInputType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The HtmlInputTypeID for this HtmlInputType.
		/// </summary>
		short HtmlInputTypeID { get; set; }
	
		/// <summary>
		/// The Name for this HtmlInputType.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this HtmlInputType.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this HtmlInputType.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this HtmlInputType.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IHtmlInputType))]
		internal abstract class HtmlInputTypeContracts : IHtmlInputType
		{
		    #region Primitive properties
		
			short IHtmlInputType.HtmlInputTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IHtmlInputType.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IHtmlInputType.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IHtmlInputType.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IHtmlInputType.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
