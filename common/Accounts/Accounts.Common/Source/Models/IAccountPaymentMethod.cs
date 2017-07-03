using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using NetSteps.Addresses.Common.Models;

namespace NetSteps.Accounts.Common.Models
{
	/// <summary>
	/// Common interface for AccountPaymentMethod.
	/// </summary>
	[ContractClass(typeof(Contracts.AccountPaymentMethodContracts))]
	public interface IAccountPaymentMethod
	{
	    #region Primitive properties
	
		/// <summary>
		/// The AccountPaymentMethodID for this AccountPaymentMethod.
		/// </summary>
		int AccountPaymentMethodID { get; set; }
	
		/// <summary>
		/// The AccountID for this AccountPaymentMethod.
		/// </summary>
		int AccountID { get; set; }
	
		/// <summary>
		/// The PaymentTypeID for this AccountPaymentMethod.
		/// </summary>
		int PaymentTypeID { get; set; }
	
		/// <summary>
		/// The ProfileName for this AccountPaymentMethod.
		/// </summary>
		string ProfileName { get; set; }
	
		/// <summary>
		/// The FirstName for this AccountPaymentMethod.
		/// </summary>
		string FirstName { get; set; }
	
		/// <summary>
		/// The LastName for this AccountPaymentMethod.
		/// </summary>
		string LastName { get; set; }
	
		/// <summary>
		/// The NameOnCard for this AccountPaymentMethod.
		/// </summary>
		string NameOnCard { get; set; }
	
		/// <summary>
		/// The AccountNumber for this AccountPaymentMethod.
		/// </summary>
		string AccountNumber { get; }
	
		/// <summary>
		/// The ExpirationDateUTC for this AccountPaymentMethod.
		/// </summary>
		Nullable<System.DateTime> ExpirationDateUTC { get; set; }
	
		/// <summary>
		/// The BillingAddressID for this AccountPaymentMethod.
		/// </summary>
		Nullable<int> BillingAddressID { get; set; }
	
		/// <summary>
		/// The IsDefault for this AccountPaymentMethod.
		/// </summary>
		bool IsDefault { get; set; }
	
		/// <summary>
		/// The DataVersion for this AccountPaymentMethod.
		/// </summary>
		byte[] DataVersion { get; set; }
	
		/// <summary>
		/// The ModifiedByUserID for this AccountPaymentMethod.
		/// </summary>
		Nullable<int> ModifiedByUserID { get; set; }
	
		/// <summary>
		/// The RoutingNumber for this AccountPaymentMethod.
		/// </summary>
		string RoutingNumber { get; set; }
	
		/// <summary>
		/// The BankAccountTypeID for this AccountPaymentMethod.
		/// </summary>
		Nullable<short> BankAccountTypeID { get; set; }
	
		/// <summary>
		/// The BankName for this AccountPaymentMethod.
		/// </summary>
		string BankName { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The Account for this AccountPaymentMethod.
		/// </summary>
	    IAccount Account { get; set; }
	
		/// <summary>
		/// The BillingAddress for this AccountPaymentMethod.
		/// </summary>
	    IAddress BillingAddress { get; set; }
	
		/// <summary>
		/// The User for this AccountPaymentMethod.
		/// </summary>
	    IUser User { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IAccountPaymentMethod))]
		internal abstract class AccountPaymentMethodContracts : IAccountPaymentMethod
		{
		    #region Primitive properties
		
			int IAccountPaymentMethod.AccountPaymentMethodID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IAccountPaymentMethod.AccountID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IAccountPaymentMethod.PaymentTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAccountPaymentMethod.ProfileName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAccountPaymentMethod.FirstName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAccountPaymentMethod.LastName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAccountPaymentMethod.NameOnCard
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAccountPaymentMethod.AccountNumber
			{
				get { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> IAccountPaymentMethod.ExpirationDateUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IAccountPaymentMethod.BillingAddressID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IAccountPaymentMethod.IsDefault
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			byte[] IAccountPaymentMethod.DataVersion
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IAccountPaymentMethod.ModifiedByUserID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAccountPaymentMethod.RoutingNumber
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<short> IAccountPaymentMethod.BankAccountTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAccountPaymentMethod.BankName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IAccount IAccountPaymentMethod.Account
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IAddress IAccountPaymentMethod.BillingAddress
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IUser IAccountPaymentMethod.User
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
