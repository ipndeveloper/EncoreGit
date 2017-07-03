using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Accounts.Common.Models
{
	/// <summary>
	/// Common interface for User.
	/// </summary>
	[ContractClass(typeof(Contracts.UserContracts))]
	public interface IUser
	{
	    #region Primitive properties
	
		/// <summary>
		/// The UserID for this User.
		/// </summary>
		int UserID { get; set; }
	
		/// <summary>
		/// The UserTypeID for this User.
		/// </summary>
		short UserTypeID { get; set; }
	
		/// <summary>
		/// The Username for this User.
		/// </summary>
		string Username { get; set; }
	
		/// <summary>
		/// The PasswordHash for this User.
		/// </summary>
		string PasswordHash { get; set; }
	
		/// <summary>
		/// The PasswordQuestion for this User.
		/// </summary>
		string PasswordQuestion { get; set; }
	
		/// <summary>
		/// The PasswordAnswer for this User.
		/// </summary>
		string PasswordAnswer { get; set; }
	
		/// <summary>
		/// The LoginMessage for this User.
		/// </summary>
		string LoginMessage { get; set; }
	
		/// <summary>
		/// The LastLoginUTC for this User.
		/// </summary>
		Nullable<System.DateTime> LastLoginUTC { get; set; }
	
		/// <summary>
		/// The DataVersion for this User.
		/// </summary>
		byte[] DataVersion { get; set; }
	
		/// <summary>
		/// The UserStatusID for this User.
		/// </summary>
		short UserStatusID { get; set; }
	
		/// <summary>
		/// The TotalLoginCount for this User.
		/// </summary>
		int TotalLoginCount { get; set; }
	
		/// <summary>
		/// The ConsecutiveFailedLogins for this User.
		/// </summary>
		int ConsecutiveFailedLogins { get; set; }
	
		/// <summary>
		/// The ModifiedByUserID for this User.
		/// </summary>
		Nullable<int> ModifiedByUserID { get; set; }
	
		/// <summary>
		/// The DefaultLanguageID for this User.
		/// </summary>
		int DefaultLanguageID { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The Roles for this User.
		/// </summary>
		IEnumerable<IRole> Roles { get; }
	
		/// <summary>
		/// Adds an <see cref="IRole"/> to the Roles collection.
		/// </summary>
		/// <param name="item">The <see cref="IRole"/> to add.</param>
		void AddRole(IRole item);
	
		/// <summary>
		/// Removes an <see cref="IRole"/> from the Roles collection.
		/// </summary>
		/// <param name="item">The <see cref="IRole"/> to remove.</param>
		void RemoveRole(IRole item);
	
		/// <summary>
		/// The UserFunctionOverrides for this User.
		/// </summary>
		IEnumerable<IUserFunctionOverride> UserFunctionOverrides { get; }
	
		/// <summary>
		/// Adds an <see cref="IUserFunctionOverride"/> to the UserFunctionOverrides collection.
		/// </summary>
		/// <param name="item">The <see cref="IUserFunctionOverride"/> to add.</param>
		void AddUserFunctionOverride(IUserFunctionOverride item);
	
		/// <summary>
		/// Removes an <see cref="IUserFunctionOverride"/> from the UserFunctionOverrides collection.
		/// </summary>
		/// <param name="item">The <see cref="IUserFunctionOverride"/> to remove.</param>
		void RemoveUserFunctionOverride(IUserFunctionOverride item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IUser))]
		internal abstract class UserContracts : IUser
		{
		    #region Primitive properties
		
			int IUser.UserID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			short IUser.UserTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IUser.Username
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IUser.PasswordHash
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IUser.PasswordQuestion
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IUser.PasswordAnswer
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IUser.LoginMessage
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> IUser.LastLoginUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			byte[] IUser.DataVersion
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			short IUser.UserStatusID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IUser.TotalLoginCount
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IUser.ConsecutiveFailedLogins
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IUser.ModifiedByUserID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IUser.DefaultLanguageID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IRole> IUser.Roles
			{
				get { throw new NotImplementedException(); }
			}
		
			void IUser.AddRole(IRole item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IUser.RemoveRole(IRole item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IUserFunctionOverride> IUser.UserFunctionOverrides
			{
				get { throw new NotImplementedException(); }
			}
		
			void IUser.AddUserFunctionOverride(IUserFunctionOverride item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IUser.RemoveUserFunctionOverride(IUserFunctionOverride item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
