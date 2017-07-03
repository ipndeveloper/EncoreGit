using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Accounts.Common.Models
{
	/// <summary>
	/// Common interface for AccountSponsorType.
	/// </summary>
	[ContractClass(typeof(Contracts.AccountSponsorTypeContracts))]
	public interface IAccountSponsorType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The AccountSponsorTypeID for this AccountSponsorType.
		/// </summary>
		int AccountSponsorTypeID { get; set; }
	
		/// <summary>
		/// The Code for this AccountSponsorType.
		/// </summary>
		string Code { get; set; }
	
		/// <summary>
		/// The Name for this AccountSponsorType.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The Description for this AccountSponsorType.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The MaxPositions for this AccountSponsorType.
		/// </summary>
		Nullable<int> MaxPositions { get; set; }
	
		/// <summary>
		/// The TermName for this AccountSponsorType.
		/// </summary>
		string TermName { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The AccountSponsors for this AccountSponsorType.
		/// </summary>
		IEnumerable<IAccountSponsor> AccountSponsors { get; }
	
		/// <summary>
		/// Adds an <see cref="IAccountSponsor"/> to the AccountSponsors collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccountSponsor"/> to add.</param>
		void AddAccountSponsor(IAccountSponsor item);
	
		/// <summary>
		/// Removes an <see cref="IAccountSponsor"/> from the AccountSponsors collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccountSponsor"/> to remove.</param>
		void RemoveAccountSponsor(IAccountSponsor item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IAccountSponsorType))]
		internal abstract class AccountSponsorTypeContracts : IAccountSponsorType
		{
		    #region Primitive properties
		
			int IAccountSponsorType.AccountSponsorTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAccountSponsorType.Code
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAccountSponsorType.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAccountSponsorType.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IAccountSponsorType.MaxPositions
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAccountSponsorType.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IAccountSponsor> IAccountSponsorType.AccountSponsors
			{
				get { throw new NotImplementedException(); }
			}
		
			void IAccountSponsorType.AddAccountSponsor(IAccountSponsor item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IAccountSponsorType.RemoveAccountSponsor(IAccountSponsor item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
