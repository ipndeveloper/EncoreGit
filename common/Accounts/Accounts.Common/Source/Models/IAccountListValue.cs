using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Accounts.Common.Models
{
	/// <summary>
	/// Common interface for AccountListValue.
	/// </summary>
	[ContractClass(typeof(Contracts.AccountListValueContracts))]
	public interface IAccountListValue
	{
	    #region Primitive properties
	
		/// <summary>
		/// The AccountListValueID for this AccountListValue.
		/// </summary>
		int AccountListValueID { get; set; }
	
		/// <summary>
		/// The AccountID for this AccountListValue.
		/// </summary>
		Nullable<int> AccountID { get; set; }
	
		/// <summary>
		/// The ListValueTypeID for this AccountListValue.
		/// </summary>
		short ListValueTypeID { get; set; }
	
		/// <summary>
		/// The Value for this AccountListValue.
		/// </summary>
		string Value { get; set; }
	
		/// <summary>
		/// The Active for this AccountListValue.
		/// </summary>
		bool Active { get; set; }
	
		/// <summary>
		/// The IsCorporate for this AccountListValue.
		/// </summary>
		bool IsCorporate { get; set; }
	
		/// <summary>
		/// The ModifiedByUserID for this AccountListValue.
		/// </summary>
		Nullable<int> ModifiedByUserID { get; set; }
	
		/// <summary>
		/// The TermName for this AccountListValue.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Editable for this AccountListValue.
		/// </summary>
		bool Editable { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The Account for this AccountListValue.
		/// </summary>
	    IAccount Account { get; set; }
	
		/// <summary>
		/// The ListValueType for this AccountListValue.
		/// </summary>
	    IListValueType ListValueType { get; set; }
	
		/// <summary>
		/// The User for this AccountListValue.
		/// </summary>
	    IUser User { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The AccountContactTags for this AccountListValue.
		/// </summary>
		IEnumerable<IAccountContactTag> AccountContactTags { get; }
	
		/// <summary>
		/// Adds an <see cref="IAccountContactTag"/> to the AccountContactTags collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccountContactTag"/> to add.</param>
		void AddAccountContactTag(IAccountContactTag item);
	
		/// <summary>
		/// Removes an <see cref="IAccountContactTag"/> from the AccountContactTags collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccountContactTag"/> to remove.</param>
		void RemoveAccountContactTag(IAccountContactTag item);
	
		/// <summary>
		/// The AccountContactTags1 for this AccountListValue.
		/// </summary>
		IEnumerable<IAccountContactTag> AccountContactTags1 { get; }
	
		/// <summary>
		/// Adds an <see cref="IAccountContactTag"/> to the AccountContactTags1 collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccountContactTag"/> to add.</param>
		void AddAccountContactTags1(IAccountContactTag item);
	
		/// <summary>
		/// Removes an <see cref="IAccountContactTag"/> from the AccountContactTags1 collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccountContactTag"/> to remove.</param>
		void RemoveAccountContactTags1(IAccountContactTag item);
	
		/// <summary>
		/// The AccountContactTags2 for this AccountListValue.
		/// </summary>
		IEnumerable<IAccountContactTag> AccountContactTags2 { get; }
	
		/// <summary>
		/// Adds an <see cref="IAccountContactTag"/> to the AccountContactTags2 collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccountContactTag"/> to add.</param>
		void AddAccountContactTags2(IAccountContactTag item);
	
		/// <summary>
		/// Removes an <see cref="IAccountContactTag"/> from the AccountContactTags2 collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccountContactTag"/> to remove.</param>
		void RemoveAccountContactTags2(IAccountContactTag item);
	
		/// <summary>
		/// The Accounts for this AccountListValue.
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
	
		/// <summary>
		/// The AccountTags for this AccountListValue.
		/// </summary>
		IEnumerable<IAccountTag> AccountTags { get; }
	
		/// <summary>
		/// Adds an <see cref="IAccountTag"/> to the AccountTags collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccountTag"/> to add.</param>
		void AddAccountTag(IAccountTag item);
	
		/// <summary>
		/// Removes an <see cref="IAccountTag"/> from the AccountTags collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccountTag"/> to remove.</param>
		void RemoveAccountTag(IAccountTag item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IAccountListValue))]
		internal abstract class AccountListValueContracts : IAccountListValue
		{
		    #region Primitive properties
		
			int IAccountListValue.AccountListValueID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IAccountListValue.AccountID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			short IAccountListValue.ListValueTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAccountListValue.Value
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IAccountListValue.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IAccountListValue.IsCorporate
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IAccountListValue.ModifiedByUserID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAccountListValue.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IAccountListValue.Editable
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IAccount IAccountListValue.Account
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IListValueType IAccountListValue.ListValueType
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IUser IAccountListValue.User
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IAccountContactTag> IAccountListValue.AccountContactTags
			{
				get { throw new NotImplementedException(); }
			}
		
			void IAccountListValue.AddAccountContactTag(IAccountContactTag item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IAccountListValue.RemoveAccountContactTag(IAccountContactTag item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IAccountContactTag> IAccountListValue.AccountContactTags1
			{
				get { throw new NotImplementedException(); }
			}
		
			void IAccountListValue.AddAccountContactTags1(IAccountContactTag item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IAccountListValue.RemoveAccountContactTags1(IAccountContactTag item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IAccountContactTag> IAccountListValue.AccountContactTags2
			{
				get { throw new NotImplementedException(); }
			}
		
			void IAccountListValue.AddAccountContactTags2(IAccountContactTag item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IAccountListValue.RemoveAccountContactTags2(IAccountContactTag item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IAccount> IAccountListValue.Accounts
			{
				get { throw new NotImplementedException(); }
			}
		
			void IAccountListValue.AddAccount(IAccount item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IAccountListValue.RemoveAccount(IAccount item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IAccountTag> IAccountListValue.AccountTags
			{
				get { throw new NotImplementedException(); }
			}
		
			void IAccountListValue.AddAccountTag(IAccountTag item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IAccountListValue.RemoveAccountTag(IAccountTag item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
