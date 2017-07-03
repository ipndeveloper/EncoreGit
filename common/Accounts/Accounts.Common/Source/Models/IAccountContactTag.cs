using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Accounts.Common.Models
{
	/// <summary>
	/// Common interface for AccountContactTag.
	/// </summary>
	[ContractClass(typeof(Contracts.AccountContactTagContracts))]
	public interface IAccountContactTag
	{
	    #region Primitive properties
	
		/// <summary>
		/// The AccountContactTagID for this AccountContactTag.
		/// </summary>
		int AccountContactTagID { get; set; }
	
		/// <summary>
		/// The AccountID for this AccountContactTag.
		/// </summary>
		Nullable<int> AccountID { get; set; }
	
		/// <summary>
		/// The ContactCategoryID for this AccountContactTag.
		/// </summary>
		Nullable<int> ContactCategoryID { get; set; }
	
		/// <summary>
		/// The ContactStatusID for this AccountContactTag.
		/// </summary>
		Nullable<int> ContactStatusID { get; set; }
	
		/// <summary>
		/// The ContactTypeID for this AccountContactTag.
		/// </summary>
		Nullable<int> ContactTypeID { get; set; }
	
		/// <summary>
		/// The Source for this AccountContactTag.
		/// </summary>
		string Source { get; set; }
	
		/// <summary>
		/// The ModifiedByUserID for this AccountContactTag.
		/// </summary>
		Nullable<int> ModifiedByUserID { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The Account for this AccountContactTag.
		/// </summary>
	    IAccount Account { get; set; }
	
		/// <summary>
		/// The AccountListValue for this AccountContactTag.
		/// </summary>
	    IAccountListValue AccountListValue { get; set; }
	
		/// <summary>
		/// The AccountListValue1 for this AccountContactTag.
		/// </summary>
	    IAccountListValue AccountListValue1 { get; set; }
	
		/// <summary>
		/// The AccountListValue2 for this AccountContactTag.
		/// </summary>
	    IAccountListValue AccountListValue2 { get; set; }
	
		/// <summary>
		/// The User for this AccountContactTag.
		/// </summary>
	    IUser User { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IAccountContactTag))]
		internal abstract class AccountContactTagContracts : IAccountContactTag
		{
		    #region Primitive properties
		
			int IAccountContactTag.AccountContactTagID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IAccountContactTag.AccountID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IAccountContactTag.ContactCategoryID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IAccountContactTag.ContactStatusID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IAccountContactTag.ContactTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAccountContactTag.Source
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IAccountContactTag.ModifiedByUserID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IAccount IAccountContactTag.Account
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IAccountListValue IAccountContactTag.AccountListValue
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IAccountListValue IAccountContactTag.AccountListValue1
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IAccountListValue IAccountContactTag.AccountListValue2
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IUser IAccountContactTag.User
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
