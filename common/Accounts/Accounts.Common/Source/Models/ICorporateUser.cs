using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Accounts.Common.Models
{
	/// <summary>
	/// Common interface for CorporateUser.
	/// </summary>
	[ContractClass(typeof(Contracts.CorporateUserContracts))]
	public interface ICorporateUser
	{
	    #region Primitive properties
	
		/// <summary>
		/// The CorporateUserID for this CorporateUser.
		/// </summary>
		int CorporateUserID { get; set; }
	
		/// <summary>
		/// The UserID for this CorporateUser.
		/// </summary>
		int UserID { get; set; }
	
		/// <summary>
		/// The FirstName for this CorporateUser.
		/// </summary>
		string FirstName { get; set; }
	
		/// <summary>
		/// The LastName for this CorporateUser.
		/// </summary>
		string LastName { get; set; }
	
		/// <summary>
		/// The Email for this CorporateUser.
		/// </summary>
		string Email { get; set; }
	
		/// <summary>
		/// The PhoneNumber for this CorporateUser.
		/// </summary>
		string PhoneNumber { get; set; }
	
		/// <summary>
		/// The DataVersion for this CorporateUser.
		/// </summary>
		byte[] DataVersion { get; set; }
	
		/// <summary>
		/// The HasAccessToAllSites for this CorporateUser.
		/// </summary>
		bool HasAccessToAllSites { get; set; }
	
		/// <summary>
		/// The ModifiedByUserID for this CorporateUser.
		/// </summary>
		Nullable<int> ModifiedByUserID { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The User for this CorporateUser.
		/// </summary>
	    IUser User { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(ICorporateUser))]
		internal abstract class CorporateUserContracts : ICorporateUser
		{
		    #region Primitive properties
		
			int ICorporateUser.CorporateUserID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int ICorporateUser.UserID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ICorporateUser.FirstName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ICorporateUser.LastName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ICorporateUser.Email
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ICorporateUser.PhoneNumber
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			byte[] ICorporateUser.DataVersion
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool ICorporateUser.HasAccessToAllSites
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> ICorporateUser.ModifiedByUserID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IUser ICorporateUser.User
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
