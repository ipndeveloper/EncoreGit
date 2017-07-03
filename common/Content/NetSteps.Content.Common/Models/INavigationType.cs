using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Content.Common.Models
{
	/// <summary>
	/// Common interface for NavigationType.
	/// </summary>
	[ContractClass(typeof(Contracts.NavigationTypeContracts))]
	public interface INavigationType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The NavigationTypeID for this NavigationType.
		/// </summary>
		int NavigationTypeID { get; set; }
	
		/// <summary>
		/// The SiteTypeID for this NavigationType.
		/// </summary>
		Nullable<short> SiteTypeID { get; set; }
	
		/// <summary>
		/// The Name for this NavigationType.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this NavigationType.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this NavigationType.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this NavigationType.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(INavigationType))]
		internal abstract class NavigationTypeContracts : INavigationType
		{
		    #region Primitive properties
		
			int INavigationType.NavigationTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<short> INavigationType.SiteTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string INavigationType.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string INavigationType.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string INavigationType.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool INavigationType.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
