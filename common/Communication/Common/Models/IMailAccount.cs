using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Common.Models
{
	/// <summary>
	/// Common interface for MailAccount.
	/// </summary>
	[ContractClass(typeof(Contracts.MailAccountContracts))]
	public interface IMailAccount
	{
	    #region Primitive properties
	
		/// <summary>
		/// The MailAccountID for this MailAccount.
		/// </summary>
		int MailAccountID { get; set; }
	
		/// <summary>
		/// The AccountID for this MailAccount.
		/// </summary>
		int AccountID { get; set; }
	
		/// <summary>
		/// The EmailAddress for this MailAccount.
		/// </summary>
		string EmailAddress { get; set; }
	
		/// <summary>
		/// The PasswordOLD for this MailAccount.
		/// </summary>
		string PasswordOLD { get; set; }
	
		/// <summary>
		/// The PasswordHash for this MailAccount.
		/// </summary>
		string PasswordHash { get; set; }
	
		/// <summary>
		/// The LastLoginUTC for this MailAccount.
		/// </summary>
		Nullable<System.DateTime> LastLoginUTC { get; set; }
	
		/// <summary>
		/// The Active for this MailAccount.
		/// </summary>
		bool Active { get; set; }
	
		/// <summary>
		/// The DataVersion for this MailAccount.
		/// </summary>
		byte[] DataVersion { get; set; }
	
		/// <summary>
		/// The IsLockedOut for this MailAccount.
		/// </summary>
		bool IsLockedOut { get; set; }
	
		/// <summary>
		/// The ModifiedByUserID for this MailAccount.
		/// </summary>
		Nullable<int> ModifiedByUserID { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IMailAccount))]
		internal abstract class MailAccountContracts : IMailAccount
		{
		    #region Primitive properties
		
			int IMailAccount.MailAccountID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IMailAccount.AccountID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IMailAccount.EmailAddress
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IMailAccount.PasswordOLD
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IMailAccount.PasswordHash
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> IMailAccount.LastLoginUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IMailAccount.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			byte[] IMailAccount.DataVersion
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IMailAccount.IsLockedOut
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IMailAccount.ModifiedByUserID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
