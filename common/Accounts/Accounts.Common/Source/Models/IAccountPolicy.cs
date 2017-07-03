using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Accounts.Common.Models
{
	/// <summary>
	/// Common interface for AccountPolicy.
	/// </summary>
	[ContractClass(typeof(Contracts.AccountPolicyContracts))]
	public interface IAccountPolicy
	{
	    #region Primitive properties
	
		/// <summary>
		/// The AccountPolicyID for this AccountPolicy.
		/// </summary>
		int AccountPolicyID { get; set; }
	
		/// <summary>
		/// The AccountID for this AccountPolicy.
		/// </summary>
		int AccountID { get; set; }
	
		/// <summary>
		/// The PolicyID for this AccountPolicy.
		/// </summary>
		int PolicyID { get; set; }
	
		/// <summary>
		/// The DateAcceptedUTC for this AccountPolicy.
		/// </summary>
		Nullable<System.DateTime> DateAcceptedUTC { get; set; }
	
		/// <summary>
		/// The ModifiedByUserID for this AccountPolicy.
		/// </summary>
		Nullable<int> ModifiedByUserID { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The Account for this AccountPolicy.
		/// </summary>
	    IAccount Account { get; set; }
	
		/// <summary>
		/// The Policy for this AccountPolicy.
		/// </summary>
	    IPolicy Policy { get; set; }
	
		/// <summary>
		/// The User for this AccountPolicy.
		/// </summary>
	    IUser User { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IAccountPolicy))]
		internal abstract class AccountPolicyContracts : IAccountPolicy
		{
		    #region Primitive properties
		
			int IAccountPolicy.AccountPolicyID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IAccountPolicy.AccountID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IAccountPolicy.PolicyID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> IAccountPolicy.DateAcceptedUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IAccountPolicy.ModifiedByUserID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IAccount IAccountPolicy.Account
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IPolicy IAccountPolicy.Policy
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IUser IAccountPolicy.User
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
