using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Accounts.Common.Models
{
	/// <summary>
	/// Common interface for AccountLanguage.
	/// </summary>
	[ContractClass(typeof(Contracts.AccountLanguageContracts))]
	public interface IAccountLanguage
	{
	    #region Primitive properties
	
		/// <summary>
		/// The AccountLanguageID for this AccountLanguage.
		/// </summary>
		int AccountLanguageID { get; set; }
	
		/// <summary>
		/// The AccountID for this AccountLanguage.
		/// </summary>
		int AccountID { get; set; }
	
		/// <summary>
		/// The LanguageID for this AccountLanguage.
		/// </summary>
		Nullable<int> LanguageID { get; set; }
	
		/// <summary>
		/// The OtherLanguage for this AccountLanguage.
		/// </summary>
		string OtherLanguage { get; set; }
	
		/// <summary>
		/// The ModifiedByUserID for this AccountLanguage.
		/// </summary>
		Nullable<int> ModifiedByUserID { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The Account for this AccountLanguage.
		/// </summary>
	    IAccount Account { get; set; }
	
		/// <summary>
		/// The User for this AccountLanguage.
		/// </summary>
	    IUser User { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IAccountLanguage))]
		internal abstract class AccountLanguageContracts : IAccountLanguage
		{
		    #region Primitive properties
		
			int IAccountLanguage.AccountLanguageID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IAccountLanguage.AccountID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IAccountLanguage.LanguageID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAccountLanguage.OtherLanguage
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IAccountLanguage.ModifiedByUserID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IAccount IAccountLanguage.Account
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IUser IAccountLanguage.User
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
