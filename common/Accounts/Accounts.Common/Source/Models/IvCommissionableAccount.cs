using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Accounts.Common.Models
{
	/// <summary>
	/// Common interface for vCommissionableAccount.
	/// </summary>
	[ContractClass(typeof(Contracts.vCommissionableAccountContracts))]
	public interface IvCommissionableAccount
	{
	    #region Primitive properties
	
		/// <summary>
		/// The AccountID for this vCommissionableAccount.
		/// </summary>
		int AccountID { get; set; }
	
		/// <summary>
		/// The EntryDate for this vCommissionableAccount.
		/// </summary>
		Nullable<System.DateTime> EntryDate { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IvCommissionableAccount))]
		internal abstract class vCommissionableAccountContracts : IvCommissionableAccount
		{
		    #region Primitive properties
		
			int IvCommissionableAccount.AccountID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> IvCommissionableAccount.EntryDate
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
