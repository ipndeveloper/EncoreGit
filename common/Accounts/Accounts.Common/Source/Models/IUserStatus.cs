using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Accounts.Common.Models
{
	/// <summary>
	/// Common interface for UserStatus.
	/// </summary>
	[ContractClass(typeof(Contracts.UserStatusContracts))]
	public interface IUserStatus
	{
	    #region Primitive properties
	
		/// <summary>
		/// The UserStatusID for this UserStatus.
		/// </summary>
		short UserStatusID { get; set; }
	
		/// <summary>
		/// The Name for this UserStatus.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this UserStatus.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this UserStatus.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this UserStatus.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The Users for this UserStatus.
		/// </summary>
		IEnumerable<IUser> Users { get; }
	
		/// <summary>
		/// Adds an <see cref="IUser"/> to the Users collection.
		/// </summary>
		/// <param name="item">The <see cref="IUser"/> to add.</param>
		void AddUser(IUser item);
	
		/// <summary>
		/// Removes an <see cref="IUser"/> from the Users collection.
		/// </summary>
		/// <param name="item">The <see cref="IUser"/> to remove.</param>
		void RemoveUser(IUser item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IUserStatus))]
		internal abstract class UserStatusContracts : IUserStatus
		{
		    #region Primitive properties
		
			short IUserStatus.UserStatusID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IUserStatus.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IUserStatus.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IUserStatus.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IUserStatus.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IUser> IUserStatus.Users
			{
				get { throw new NotImplementedException(); }
			}
		
			void IUserStatus.AddUser(IUser item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IUserStatus.RemoveUser(IUser item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
