using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Accounts.Common.Models
{
	/// <summary>
	/// Common interface for AccountPhone.
	/// </summary>
	[ContractClass(typeof(Contracts.AccountPhoneContracts))]
	public interface IAccountPhone
	{
	    #region Primitive properties
	
		/// <summary>
		/// The AccountPhoneID for this AccountPhone.
		/// </summary>
		int AccountPhoneID { get; set; }
	
		/// <summary>
		/// The AccountID for this AccountPhone.
		/// </summary>
		int AccountID { get; set; }
	
		/// <summary>
		/// The PhoneTypeID for this AccountPhone.
		/// </summary>
		int PhoneTypeID { get; set; }
	
		/// <summary>
		/// The PhoneNumber for this AccountPhone.
		/// </summary>
		string PhoneNumber { get; set; }
	
		/// <summary>
		/// The DataVersion for this AccountPhone.
		/// </summary>
		byte[] DataVersion { get; set; }
	
		/// <summary>
		/// The IsPrivate for this AccountPhone.
		/// </summary>
		bool IsPrivate { get; set; }
	
		/// <summary>
		/// The ModifiedByUserID for this AccountPhone.
		/// </summary>
		Nullable<int> ModifiedByUserID { get; set; }
	
		/// <summary>
		/// The IsDefault for this AccountPhone.
		/// </summary>
		bool IsDefault { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The Account for this AccountPhone.
		/// </summary>
	    IAccount Account { get; set; }
	
		/// <summary>
		/// The User for this AccountPhone.
		/// </summary>
	    IUser User { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IAccountPhone))]
		internal abstract class AccountPhoneContracts : IAccountPhone
		{
		    #region Primitive properties
		
			int IAccountPhone.AccountPhoneID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IAccountPhone.AccountID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IAccountPhone.PhoneTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAccountPhone.PhoneNumber
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			byte[] IAccountPhone.DataVersion
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IAccountPhone.IsPrivate
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IAccountPhone.ModifiedByUserID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IAccountPhone.IsDefault
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IAccount IAccountPhone.Account
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IUser IAccountPhone.User
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
