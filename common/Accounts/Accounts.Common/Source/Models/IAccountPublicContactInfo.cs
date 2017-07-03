using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Accounts.Common.Models
{
	/// <summary>
	/// Common interface for AccountPublicContactInfo.
	/// </summary>
	[ContractClass(typeof(Contracts.AccountPublicContactInfoContracts))]
	public interface IAccountPublicContactInfo
	{
	    #region Primitive properties
	
		/// <summary>
		/// The AccountPublicContactInfoID for this AccountPublicContactInfo.
		/// </summary>
		int AccountPublicContactInfoID { get; set; }
	
		/// <summary>
		/// The AccountID for this AccountPublicContactInfo.
		/// </summary>
		Nullable<int> AccountID { get; set; }
	
		/// <summary>
		/// The Name for this AccountPublicContactInfo.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The PhoneNumber for this AccountPublicContactInfo.
		/// </summary>
		string PhoneNumber { get; set; }
	
		/// <summary>
		/// The EmailAddress for this AccountPublicContactInfo.
		/// </summary>
		string EmailAddress { get; set; }
	
		/// <summary>
		/// The HideName for this AccountPublicContactInfo.
		/// </summary>
		bool HideName { get; set; }
	
		/// <summary>
		/// The HidePhoneNumber for this AccountPublicContactInfo.
		/// </summary>
		bool HidePhoneNumber { get; set; }
	
		/// <summary>
		/// The HideEmailAddress for this AccountPublicContactInfo.
		/// </summary>
		bool HideEmailAddress { get; set; }
	
		/// <summary>
		/// The DataVersion for this AccountPublicContactInfo.
		/// </summary>
		byte[] DataVersion { get; set; }
	
		/// <summary>
		/// The ModifiedByUserID for this AccountPublicContactInfo.
		/// </summary>
		Nullable<int> ModifiedByUserID { get; set; }
	
		/// <summary>
		/// The HideAddress for this AccountPublicContactInfo.
		/// </summary>
		bool HideAddress { get; set; }
	
		/// <summary>
		/// The Title for this AccountPublicContactInfo.
		/// </summary>
		string Title { get; set; }
	
		/// <summary>
		/// The HideTitle for this AccountPublicContactInfo.
		/// </summary>
		bool HideTitle { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The Account for this AccountPublicContactInfo.
		/// </summary>
	    IAccount Account { get; set; }
	
		/// <summary>
		/// The User for this AccountPublicContactInfo.
		/// </summary>
	    IUser User { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IAccountPublicContactInfo))]
		internal abstract class AccountPublicContactInfoContracts : IAccountPublicContactInfo
		{
		    #region Primitive properties
		
			int IAccountPublicContactInfo.AccountPublicContactInfoID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IAccountPublicContactInfo.AccountID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAccountPublicContactInfo.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAccountPublicContactInfo.PhoneNumber
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAccountPublicContactInfo.EmailAddress
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IAccountPublicContactInfo.HideName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IAccountPublicContactInfo.HidePhoneNumber
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IAccountPublicContactInfo.HideEmailAddress
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			byte[] IAccountPublicContactInfo.DataVersion
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IAccountPublicContactInfo.ModifiedByUserID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IAccountPublicContactInfo.HideAddress
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAccountPublicContactInfo.Title
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IAccountPublicContactInfo.HideTitle
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IAccount IAccountPublicContactInfo.Account
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IUser IAccountPublicContactInfo.User
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
