using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Accounts.Common.Models
{
	/// <summary>
	/// Common interface for AccountTag.
	/// </summary>
	[ContractClass(typeof(Contracts.AccountTagContracts))]
	public interface IAccountTag
	{
	    #region Primitive properties
	
		/// <summary>
		/// The AccountTagID for this AccountTag.
		/// </summary>
		long AccountTagID { get; set; }
	
		/// <summary>
		/// The AccountID for this AccountTag.
		/// </summary>
		int AccountID { get; set; }
	
		/// <summary>
		/// The TaggedByAccountID for this AccountTag.
		/// </summary>
		int TaggedByAccountID { get; set; }
	
		/// <summary>
		/// The AccountListValueID for this AccountTag.
		/// </summary>
		int AccountListValueID { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The Account for this AccountTag.
		/// </summary>
	    IAccount Account { get; set; }
	
		/// <summary>
		/// The Account1 for this AccountTag.
		/// </summary>
	    IAccount Account1 { get; set; }
	
		/// <summary>
		/// The AccountListValue for this AccountTag.
		/// </summary>
	    IAccountListValue AccountListValue { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IAccountTag))]
		internal abstract class AccountTagContracts : IAccountTag
		{
		    #region Primitive properties
		
			long IAccountTag.AccountTagID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IAccountTag.AccountID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IAccountTag.TaggedByAccountID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IAccountTag.AccountListValueID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IAccount IAccountTag.Account
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IAccount IAccountTag.Account1
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IAccountListValue IAccountTag.AccountListValue
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
