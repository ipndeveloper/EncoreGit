using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Accounts.Common.Models
{
	/// <summary>
	/// Common interface for Gender.
	/// </summary>
	[ContractClass(typeof(Contracts.GenderContracts))]
	public interface IGender
	{
	    #region Primitive properties
	
		/// <summary>
		/// The GenderID for this Gender.
		/// </summary>
		short GenderID { get; set; }
	
		/// <summary>
		/// The Name for this Gender.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this Gender.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this Gender.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this Gender.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The Accounts for this Gender.
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
		[ContractClassFor(typeof(IGender))]
		internal abstract class GenderContracts : IGender
		{
		    #region Primitive properties
		
			short IGender.GenderID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IGender.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IGender.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IGender.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IGender.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IAccount> IGender.Accounts
			{
				get { throw new NotImplementedException(); }
			}
		
			void IGender.AddAccount(IAccount item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IGender.RemoveAccount(IAccount item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
