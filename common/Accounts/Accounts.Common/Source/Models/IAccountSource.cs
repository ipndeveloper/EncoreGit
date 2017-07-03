using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Accounts.Common.Models
{
	/// <summary>
	/// Common interface for AccountSource.
	/// </summary>
	[ContractClass(typeof(Contracts.AccountSourceContracts))]
	public interface IAccountSource
	{
	    #region Primitive properties
	
		/// <summary>
		/// The AccountSourceID for this AccountSource.
		/// </summary>
		short AccountSourceID { get; set; }
	
		/// <summary>
		/// The Name for this AccountSource.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this AccountSource.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this AccountSource.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this AccountSource.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The Accounts for this AccountSource.
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
		[ContractClassFor(typeof(IAccountSource))]
		internal abstract class AccountSourceContracts : IAccountSource
		{
		    #region Primitive properties
		
			short IAccountSource.AccountSourceID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAccountSource.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAccountSource.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAccountSource.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IAccountSource.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IAccount> IAccountSource.Accounts
			{
				get { throw new NotImplementedException(); }
			}
		
			void IAccountSource.AddAccount(IAccount item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IAccountSource.RemoveAccount(IAccount item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
