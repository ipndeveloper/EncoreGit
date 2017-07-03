using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Accounts.Common.Models
{
	/// <summary>
	/// Common interface for Policy.
	/// </summary>
	[ContractClass(typeof(Contracts.PolicyContracts))]
	public interface IPolicy
	{
	    #region Primitive properties
	
		/// <summary>
		/// The PolicyID for this Policy.
		/// </summary>
		int PolicyID { get; set; }
	
		/// <summary>
		/// The VersionNumber for this Policy.
		/// </summary>
		string VersionNumber { get; set; }
	
		/// <summary>
		/// The DateReleasedUTC for this Policy.
		/// </summary>
		Nullable<System.DateTime> DateReleasedUTC { get; set; }
	
		/// <summary>
		/// The FilePath for this Policy.
		/// </summary>
		string FilePath { get; set; }
	
		/// <summary>
		/// The Name for this Policy.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The Active for this Policy.
		/// </summary>
		bool Active { get; set; }
	
		/// <summary>
		/// The LanguageID for this Policy.
		/// </summary>
		int LanguageID { get; set; }
	
		/// <summary>
		/// The IsAcceptanceRequired for this Policy.
		/// </summary>
		Nullable<bool> IsAcceptanceRequired { get; set; }
	
		/// <summary>
		/// The HtmlSectionID for this Policy.
		/// </summary>
		Nullable<int> HtmlSectionID { get; set; }
	
		/// <summary>
		/// The AccountTypeID for this Policy.
		/// </summary>
		Nullable<short> AccountTypeID { get; set; }
	
		/// <summary>
		/// The TermName for this Policy.
		/// </summary>
		string TermName { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IPolicy))]
		internal abstract class PolicyContracts : IPolicy
		{
		    #region Primitive properties
		
			int IPolicy.PolicyID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IPolicy.VersionNumber
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> IPolicy.DateReleasedUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IPolicy.FilePath
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IPolicy.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IPolicy.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IPolicy.LanguageID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<bool> IPolicy.IsAcceptanceRequired
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IPolicy.HtmlSectionID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<short> IPolicy.AccountTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IPolicy.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
