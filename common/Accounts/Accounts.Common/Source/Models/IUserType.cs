using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Accounts.Common.Models
{
	/// <summary>
	/// Common interface for UserType.
	/// </summary>
	[ContractClass(typeof(Contracts.UserTypeContracts))]
	public interface IUserType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The UserTypeID for this UserType.
		/// </summary>
		short UserTypeID { get; set; }
	
		/// <summary>
		/// The Name for this UserType.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this UserType.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this UserType.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this UserType.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The Users for this UserType.
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
		[ContractClassFor(typeof(IUserType))]
		internal abstract class UserTypeContracts : IUserType
		{
		    #region Primitive properties
		
			short IUserType.UserTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IUserType.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IUserType.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IUserType.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IUserType.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IUser> IUserType.Users
			{
				get { throw new NotImplementedException(); }
			}
		
			void IUserType.AddUser(IUser item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IUserType.RemoveUser(IUser item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
