using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Accounts.Common.Models
{
	/// <summary>
	/// Common interface for AccountSponsor.
	/// </summary>
	[ContractClass(typeof(Contracts.AccountSponsorContracts))]
	public interface IAccountSponsor
	{
	    #region Primitive properties
	
		/// <summary>
		/// The AccountID for this AccountSponsor.
		/// </summary>
		int AccountID { get; set; }
	
		/// <summary>
		/// The SponsorID for this AccountSponsor.
		/// </summary>
		Nullable<int> SponsorID { get; set; }
	
		/// <summary>
		/// The AccountSponsorTypeID for this AccountSponsor.
		/// </summary>
		int AccountSponsorTypeID { get; set; }
	
		/// <summary>
		/// The Position for this AccountSponsor.
		/// </summary>
		int Position { get; set; }
	
		/// <summary>
		/// The EffectiveDateUTC for this AccountSponsor.
		/// </summary>
		System.DateTime EffectiveDateUTC { get; set; }
	
		/// <summary>
		/// The ModifiedByUserID for this AccountSponsor.
		/// </summary>
		Nullable<int> ModifiedByUserID { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The Account for this AccountSponsor.
		/// </summary>
	    IAccount Account { get; set; }
	
		/// <summary>
		/// The Account1 for this AccountSponsor.
		/// </summary>
	    IAccount Account1 { get; set; }
	
		/// <summary>
		/// The AccountSponsorType for this AccountSponsor.
		/// </summary>
	    IAccountSponsorType AccountSponsorType { get; set; }
	
		/// <summary>
		/// The User for this AccountSponsor.
		/// </summary>
	    IUser User { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IAccountSponsor))]
		internal abstract class AccountSponsorContracts : IAccountSponsor
		{
		    #region Primitive properties
		
			int IAccountSponsor.AccountID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IAccountSponsor.SponsorID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IAccountSponsor.AccountSponsorTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IAccountSponsor.Position
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			System.DateTime IAccountSponsor.EffectiveDateUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IAccountSponsor.ModifiedByUserID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IAccount IAccountSponsor.Account
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IAccount IAccountSponsor.Account1
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IAccountSponsorType IAccountSponsor.AccountSponsorType
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IUser IAccountSponsor.User
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
