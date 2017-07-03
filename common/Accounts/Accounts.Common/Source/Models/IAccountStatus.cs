using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Accounts.Common.Models
{
	/// <summary>
	/// Common interface for AccountStatus.
	/// </summary>
	[ContractClass(typeof(Contracts.AccountStatusContracts))]
	public interface IAccountStatus
	{
	    #region Primitive properties
	
		/// <summary>
		/// The AccountStatusID for this AccountStatus.
		/// </summary>
		short AccountStatusID { get; set; }
	
		/// <summary>
		/// The Name for this AccountStatus.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The Description for this AccountStatus.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Editable for this AccountStatus.
		/// </summary>
		bool Editable { get; set; }
	
		/// <summary>
		/// The Active for this AccountStatus.
		/// </summary>
		bool Active { get; set; }
	
		/// <summary>
		/// The TermName for this AccountStatus.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The ReportAsActive for this AccountStatus.
		/// </summary>
		bool ReportAsActive { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The Accounts for this AccountStatus.
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

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IAccountStatus))]
		internal abstract class AccountStatusContracts : IAccountStatus
		{
		    #region Primitive properties
		
			short IAccountStatus.AccountStatusID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAccountStatus.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAccountStatus.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IAccountStatus.Editable
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IAccountStatus.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAccountStatus.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IAccountStatus.ReportAsActive
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IAccount> IAccountStatus.Accounts
			{
				get { throw new NotImplementedException(); }
			}
		
			void IAccountStatus.AddAccount(IAccount item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IAccountStatus.RemoveAccount(IAccount item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
