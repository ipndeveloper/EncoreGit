using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Accounts.Common.Models
{
	/// <summary>
	/// Common interface for AccountType.
	/// </summary>
	[ContractClass(typeof(Contracts.AccountTypeContracts))]
	public interface IAccountType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The AccountTypeID for this AccountType.
		/// </summary>
		short AccountTypeID { get; set; }
	
		/// <summary>
		/// The Name for this AccountType.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The Code for this AccountType.
		/// </summary>
		string Code { get; set; }
	
		/// <summary>
		/// The TermName for this AccountType.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this AccountType.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this AccountType.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The Roles for this AccountType.
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

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IAccountType))]
		internal abstract class AccountTypeContracts : IAccountType
		{
		    #region Primitive properties
		
			short IAccountType.AccountTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAccountType.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAccountType.Code
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAccountType.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAccountType.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IAccountType.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IRole> IAccountType.Roles
			{
				get { throw new NotImplementedException(); }
			}
		
			void IAccountType.AddRole(IRole item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IAccountType.RemoveRole(IRole item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
