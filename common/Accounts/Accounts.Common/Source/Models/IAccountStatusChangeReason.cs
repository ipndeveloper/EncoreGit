using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Accounts.Common.Models
{
	/// <summary>
	/// Common interface for AccountStatusChangeReason.
	/// </summary>
	[ContractClass(typeof(Contracts.AccountStatusChangeReasonContracts))]
	public interface IAccountStatusChangeReason
	{
	    #region Primitive properties
	
		/// <summary>
		/// The AccountStatusChangeReasonID for this AccountStatusChangeReason.
		/// </summary>
		short AccountStatusChangeReasonID { get; set; }
	
		/// <summary>
		/// The Name for this AccountStatusChangeReason.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The Description for this AccountStatusChangeReason.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Editable for this AccountStatusChangeReason.
		/// </summary>
		bool Editable { get; set; }
	
		/// <summary>
		/// The Active for this AccountStatusChangeReason.
		/// </summary>
		bool Active { get; set; }
	
		/// <summary>
		/// The ModifiedByUserID for this AccountStatusChangeReason.
		/// </summary>
		Nullable<int> ModifiedByUserID { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The User for this AccountStatusChangeReason.
		/// </summary>
	    IUser User { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The Accounts for this AccountStatusChangeReason.
		/// </summary>
		IEnumerable<IAccount> Accounts { get; }
	
		/// <summary>
		/// Adds an <see cref="IAccount"/> to the Accounts collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccount"/> to add.</param>
		void AddAccount(IAccount item);
	
		/// <summary>
		/// Removes an <see cref="IAccount"/> from the Accounts collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccount"/> to remove.</param>
		void RemoveAccount(IAccount item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IAccountStatusChangeReason))]
		internal abstract class AccountStatusChangeReasonContracts : IAccountStatusChangeReason
		{
		    #region Primitive properties
		
			short IAccountStatusChangeReason.AccountStatusChangeReasonID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAccountStatusChangeReason.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAccountStatusChangeReason.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IAccountStatusChangeReason.Editable
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IAccountStatusChangeReason.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IAccountStatusChangeReason.ModifiedByUserID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IUser IAccountStatusChangeReason.User
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IAccount> IAccountStatusChangeReason.Accounts
			{
				get { throw new NotImplementedException(); }
			}
		
			void IAccountStatusChangeReason.AddAccount(IAccount item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IAccountStatusChangeReason.RemoveAccount(IAccount item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
