using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Accounts.Common.Models
{
	/// <summary>
	/// Common interface for Role.
	/// </summary>
	[ContractClass(typeof(Contracts.RoleContracts))]
	public interface IRole
	{
	    #region Primitive properties
	
		/// <summary>
		/// The RoleID for this Role.
		/// </summary>
		int RoleID { get; set; }
	
		/// <summary>
		/// The RoleTypeID for this Role.
		/// </summary>
		short RoleTypeID { get; set; }
	
		/// <summary>
		/// The Name for this Role.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this Role.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this Role.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The StartPage for this Role.
		/// </summary>
		string StartPage { get; set; }
	
		/// <summary>
		/// The Active for this Role.
		/// </summary>
		bool Active { get; set; }
	
		/// <summary>
		/// The Editable for this Role.
		/// </summary>
		bool Editable { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The RoleType for this Role.
		/// </summary>
	    IRoleType RoleType { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The AccountTypes for this Role.
		/// </summary>
		IEnumerable<IAccountType> AccountTypes { get; }
	
		/// <summary>
		/// Adds an <see cref="IAccountType"/> to the AccountTypes collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccountType"/> to add.</param>
		void AddAccountType(IAccountType item);
	
		/// <summary>
		/// Removes an <see cref="IAccountType"/> from the AccountTypes collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccountType"/> to remove.</param>
		void RemoveAccountType(IAccountType item);
	
		/// <summary>
		/// The Functions for this Role.
		/// </summary>
		IEnumerable<IFunction> Functions { get; }
	
		/// <summary>
		/// Adds an <see cref="IFunction"/> to the Functions collection.
		/// </summary>
		/// <param name="item">The <see cref="IFunction"/> to add.</param>
		void AddFunction(IFunction item);
	
		/// <summary>
		/// Removes an <see cref="IFunction"/> from the Functions collection.
		/// </summary>
		/// <param name="item">The <see cref="IFunction"/> to remove.</param>
		void RemoveFunction(IFunction item);
	
		/// <summary>
		/// The Users for this Role.
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
		[ContractClassFor(typeof(IRole))]
		internal abstract class RoleContracts : IRole
		{
		    #region Primitive properties
		
			int IRole.RoleID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			short IRole.RoleTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IRole.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IRole.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IRole.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IRole.StartPage
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IRole.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IRole.Editable
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IRoleType IRole.RoleType
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IAccountType> IRole.AccountTypes
			{
				get { throw new NotImplementedException(); }
			}
		
			void IRole.AddAccountType(IAccountType item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IRole.RemoveAccountType(IAccountType item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IFunction> IRole.Functions
			{
				get { throw new NotImplementedException(); }
			}
		
			void IRole.AddFunction(IFunction item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IRole.RemoveFunction(IFunction item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IUser> IRole.Users
			{
				get { throw new NotImplementedException(); }
			}
		
			void IRole.AddUser(IUser item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IRole.RemoveUser(IUser item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
